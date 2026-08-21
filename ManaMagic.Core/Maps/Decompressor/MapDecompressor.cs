using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManaMagic.Core.RomReaders;
using ZwellTech;
using ZwellTech.Logging;
using ReplacePieceDictionary = System.Collections.Generic.IReadOnlyDictionary<ManaMagic.Core.Maps.Decompressor.MapCompressionCommand, ManaMagic.Core.Maps.Decompressor.ReplaceMapPiece>;

#nullable enable

// Lets talk map piece compression. For the most part this is not to bad.
// Assuming the satan flag isn't set then the decompression has the following format.
// The first two bytes are the width and height respectively. These bytes are encoded, with the lowest bits of each value containing the satan flags.
// Divide by 2 and add one to get the width and height of the map piece in 16-bit tiles.
// Afterwards the values have the following meanings:
// 00-BF: These are just tile indexes.
// C0-CF: Looks back 1-2 tiles for the index and writes it 1-8 times.
// D0-DF: Looks back 1-4 tiles for the index and writes it 1-8 times.
// E0:    Takes an extra operand. Go up 1/2 row(s) for the index and copies 1-128 tiles from there.
// E1-E7: Takes an extra operand. Go up 1 row for the index and copies 1-8 tiles from there.
// E8-EF: Takes an extra operand. Looks back 0-15 tiles and copies 1-128 tiles from there.
// F0:    Takes an extra operand. Fills 1-128 tiles, starting with the last tile Id and incrementing/decrementing the tile Id each time.
// F1-F7: Fills 1-8 tiles, starting with LastTileId+1 and incrementing the tile Id each time.
// F8-FD: User Defined Satan Commands.
// FE:    Advance To The Next Satan Command Read.
// FF:    End User Defined Satan Command Read.
//
// The Satan Flag indicates that commands F8-FF are used.
// If the flag is set then the map piece contains embedded map pieces that are compressed just like normal map pieces including a width and height.
// These embedded pieces are listed right after the width/height.
// The first piece read will be assigned to byte F8, then next to F9, and so on. An FE terminator byte comes between each definition.
// An FF terminator byte ends the replacement map piece read.
// These commands are processed on a second pass. So instead of placing the piece when a F8-FD command is encountered we instead have to remember its location.
// After we have decompressed the entire map piece then we have to go back and use the location of fhe F8-FD command as a starting point to draw the replacement
// piece with the proper height and width.

namespace ManaMagic.Core.Maps.Decompressor
{
    internal sealed class MapDecompressor
    {
        private readonly MapRomReader reader;

        public MapDecompressor(MapRomReader reader)
        {
            this.reader = reader;
        }

        public MapPiece Decompress(ushort index, int address)
        {
            this.reader.Seek(address);

            // First two bytes are the width and height of the map piece in 16-bit tiles.
            // The bottom two bits determine if the replacement commands are used.
            byte baseWidth = this.reader.Read();
            byte baseHeight = this.reader.Read();

            // The lowest bit of the height/width is a flag, so the values need to be shifted to the right to get the true height/width.
            byte width = (byte)((baseWidth / 2) + 1);
            byte height = (byte)((baseHeight / 2) + 1);

            // If either of the bottom bits of the height/width are set then the map piece was designed by satan.
            bool hasReplaceMapPieces = ((baseWidth & 0x01) > 0) || ((baseHeight & 0x01) > 0);
            ReplacePieceDictionary replaceMapPieces = this.ReadReplaceMapPieces(index, hasReplaceMapPieces);

            int tileIndex = 0;
            int tileCount = width * height;
            byte[] mapPieces = new byte[tileCount];
            while (tileIndex < tileCount)
            {
                // If 0xC0 is not set in the command byte then it is just a tile ID to be added to the array at the current index.
                // If 0xC0 is set then the command byte is a decompression command, which may or may not have an operand and loads between 1 and 128 tile Ids.
                // Or if its 0xF8+ then its a goddamn replace map piece command.
                byte commandByte = this.reader.Read();
                MapCompressionCommand command = MapDecompressor.GetCompressionCommand(commandByte);
                this.ProcessCommand(command, commandByte, mapPieces, width, tileCount, replaceMapPieces, ref tileIndex);
            }

            // Now that the map has been decompressed the replacement commands (if any) need to be ran to write the pieces into their respective locations.
            this.RunMapPieceReplaceCommands(mapPieces, width, replaceMapPieces);

            // Only including the hasReplaceMapPieces for debugging to track which maps use these pieces.
            return new MapPiece(index, width, height, mapPieces, hasReplaceMapPieces);
        }

        private void ProcessCommand(MapCompressionCommand command, byte commandByte, byte[] mapPieces, byte width, int tileCount, ReplacePieceDictionary replaceMapPieces, ref int tileIndex)
        {
            byte operand = MapDecompressor.HasOperand(command, commandByte) ? this.reader.Read() : (byte)0;
            switch (command)
            {
                case MapCompressionCommand.Tile:
                    // Add the tile index to the map.
                    mapPieces[tileIndex] = commandByte;
                    tileIndex++;
                    break;
                case MapCompressionCommand.ShortLookBackTileAndRepeat:
                case MapCompressionCommand.LongLookBackTileAndRepeat:
                    // Looks back 1-4 tiles for the index and writes it 1-8 times.
                    this.LookBackTileAndRepeat(commandByte, mapPieces, ref tileIndex);
                    break;
                case MapCompressionCommand.CopyFromLookBack:
                    // Copies by looking back entire rows or performs a large lookback copy.
                    this.CopyFromLookBack(commandByte, width, tileCount, mapPieces, operand, ref tileIndex);
                    break;
                case MapCompressionCommand.FillWithIncrement:
                    // Fills tile Ids by starting from the last tile Id and incrementing or decrementing it.
                    this.FillWithIncrement(commandByte, mapPieces, operand, ref tileIndex);
                    break;
                case MapCompressionCommand.ReplacePieceIndexA:
                case MapCompressionCommand.ReplacePieceIndexB:
                case MapCompressionCommand.ReplacePieceIndexC:
                case MapCompressionCommand.ReplacePieceIndexD:
                case MapCompressionCommand.ReplacePieceIndexE:
                case MapCompressionCommand.ReplacePieceIndexF:
                    // Record the index we encountered the command for later processing.
                    MarkReplaceMapPiece(command, mapPieces, replaceMapPieces, ref tileIndex);
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("Unknown map compression command.");
                    break;
            }

            static void MarkReplaceMapPiece(MapCompressionCommand command, byte[] mapPieces, ReplacePieceDictionary replaceMapPieces, ref int tileIndex)
            {
                replaceMapPieces[command].InsertionIndexes.Add(tileIndex);

                mapPieces[tileIndex] = 0x00;
                tileIndex++;
            }
        }

        private void ProcessCommand(MapCompressionCommand command, byte commandByte, byte operand, byte[] mapPieces, byte width, int tileCount, ref int tileIndex)
        {
            switch (command)
            {
                case MapCompressionCommand.Tile:
                    // Add the tile index to the map.
                    mapPieces[tileIndex] = commandByte;
                    tileIndex++;
                    break;
                case MapCompressionCommand.ShortLookBackTileAndRepeat:
                case MapCompressionCommand.LongLookBackTileAndRepeat:
                    // Looks back 1-4 tiles for the index and writes it 1-8 times.
                    this.LookBackTileAndRepeat(commandByte, mapPieces, ref tileIndex);
                    break;
                case MapCompressionCommand.CopyFromLookBack:
                    // Copies by looking back entire rows or performs a large lookback copy.
                    this.CopyFromLookBack(commandByte, width, tileCount, mapPieces, operand, ref tileIndex);
                    break;
                case MapCompressionCommand.FillWithIncrement:
                    // Fills tile Ids by starting from the last tile Id and incrementing or decrementing it.
                    this.FillWithIncrement(commandByte, mapPieces, operand, ref tileIndex);
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("Unknown map compression command.");
                    break;
            }
        }

        private ReplacePieceDictionary ReadReplaceMapPieces(ushort index, bool hasReplaceMapPieces)
        {
            Dictionary<MapCompressionCommand, ReplaceMapPiece> replaceMapPieces = new Dictionary<MapCompressionCommand, ReplaceMapPiece>()
            {
                { MapCompressionCommand.ReplacePieceIndexA, new ReplaceMapPiece() },
                { MapCompressionCommand.ReplacePieceIndexB, new ReplaceMapPiece() },
                { MapCompressionCommand.ReplacePieceIndexC, new ReplaceMapPiece() },
                { MapCompressionCommand.ReplacePieceIndexD, new ReplaceMapPiece() },
                { MapCompressionCommand.ReplacePieceIndexE, new ReplaceMapPiece() },
                { MapCompressionCommand.ReplacePieceIndexF, new ReplaceMapPiece() },
            };

            if (hasReplaceMapPieces)
            {
                MapCompressionCommand currentIndex = MapCompressionCommand.ReplacePieceIndexA;
                ReplaceMapPiece currentMapPiece = replaceMapPieces[currentIndex];

                byte size = this.reader.Read();
                currentMapPiece.Height = (byte)(((size & 0xF0) >> 4) + 1);
                currentMapPiece.Width = (byte)((size & 0x0F) + 1);

                byte current = this.reader.Read();
                while ((MapCompressionCommand)current != MapCompressionCommand.EndReplacePieces)
                {
                    if ((MapCompressionCommand)current == MapCompressionCommand.AdvanceReplacePieceIndex)
                    {
                        currentIndex++;
                        currentMapPiece = replaceMapPieces[currentIndex];

                        size = this.reader.Read();
                        currentMapPiece.Height = (byte)(((size & 0xF0) >> 4) + 1);
                        currentMapPiece.Width = (byte)((size & 0x0F) + 1);
                    }
                    else
                    {
                        currentMapPiece.Instructions.Add(current);
                    }
                    current = this.reader.Read();
                }
                LogDebugInfo(index, replaceMapPieces);
            }
            return replaceMapPieces;

            static void LogDebugInfo(ushort index, ReplacePieceDictionary replaceMapPieces)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(").Append(index.ToString("X4")).Append(") Replace Map Pieces Detected. ");
                foreach (KeyValuePair<MapCompressionCommand, ReplaceMapPiece> kvp in replaceMapPieces)
                {
                    sb.Append(((byte)kvp.Key).ToString("X2")).Append(": ").Append(kvp.Value.Initialized);
                    if (kvp.Value.Initialized)
                    {
                        int c0Count = kvp.Value.Instructions.Count(p => p >= 0xC0 && p <= 0xCF);
                        int d0Count = kvp.Value.Instructions.Count(p => p >= 0xD0 && p <= 0xDF);
                        int e0Count = kvp.Value.Instructions.Count(p => p >= 0xE0 && p <= 0xEF);
                        int f0Count = kvp.Value.Instructions.Count(p => p >= 0xF0 && p <= 0xF7);
                        int f8Count = kvp.Value.Instructions.Count(p => p >= 0xF8 && p <= 0xFF);

                        sb.Append(" (C0: ").Append(c0Count.ToString("X2")).Append(", ");
                        sb.Append("D0: ").Append(d0Count.ToString("X2")).Append(", ");
                        sb.Append("E0: ").Append(e0Count.ToString("X2")).Append(", ");
                        sb.Append("F0: ").Append(f0Count.ToString("X2")).Append(", ");
                        sb.Append("F8: ").Append(f8Count.ToString("X2")).Append(")");

                        if (kvp.Key != MapCompressionCommand.ReplacePieceIndexF)
                        {
                            sb.Append(", ");
                        }
                    }
                    else
                    {
                        if (kvp.Key != MapCompressionCommand.ReplacePieceIndexF)
                        {
                            sb.Append(", ");
                        }
                    }
                }
                LoggerEngine.Logger.LogDebug(LogComponent.Compression, sb.ToString());
            }
        }

        private void RunMapPieceReplaceCommands(byte[] mapPieces, byte mapWidth, ReplacePieceDictionary replaceMapPieces)
        {
            foreach (KeyValuePair<MapCompressionCommand, ReplaceMapPiece> kvp in replaceMapPieces)
            {
                if (kvp.Value.Initialized)
                {
                    int readIndex = 0;
                    int tileIndex = 0;
                    int tileCount = kvp.Value.Width * kvp.Value.Height;
                    byte[] replacePiece = new byte[tileCount];

                    // If the replace command was enabled then we need to decode the map piece for the index.
                    // These are encoded the same as normal map pieces so the decoder just needs ran again with a difference source.
                    while (tileIndex < tileCount)
                    {
                        byte commandByte = kvp.Value.Instructions[readIndex];
                        readIndex++;

                        byte operand = 0;
                        MapCompressionCommand command = MapDecompressor.GetCompressionCommand(commandByte);
                        if (MapDecompressor.HasOperand(command, commandByte))
                        {
                            operand = kvp.Value.Instructions[readIndex];
                            readIndex++;
                        }
                        this.ProcessCommand(command, commandByte, operand, replacePiece, kvp.Value.Width, tileCount, ref tileIndex);
                    }

                    // Now that the replacement map piece has been decompressed, we need to go to each index where the command was used and insert the piece there.
                    // The replacement piece has to be drawn with the height/width specified in the command, so moving the height will always start from the insert index.
                    foreach (int insertIndex in kvp.Value.InsertionIndexes)
                    {
                        int replaceIndex = 0;
                        for (int currentHeight = 0; currentHeight < kvp.Value.Height; currentHeight++)
                        {
                            for (int currentWidth = 0; currentWidth < kvp.Value.Width; currentWidth++)
                            {
                                int writeIndex = insertIndex + ((currentHeight * mapWidth) + currentWidth);
                                mapPieces[writeIndex] = replacePiece[replaceIndex];
                                replaceIndex++;
                            }
                        }
                    }
                }
            }
        }

        private void LookBackTileAndRepeat(byte command, byte[] map, ref int tileIndex)
        {
            // Get Tile XX Spaces Back, Repeat It YY Number Of Times.
            // Looks back 1-2 (C0) or 1-4 (D0) tiles for the index and writes it 1-8 times.
            byte lookback = (byte)(((command & 0B_0001_1000) >> 0x03) + 1); //(1,2), (1,4)
            byte length = (byte)((command & 0B_0000_0111) + 1); // (1,8)
            byte copyTileIndex = map[tileIndex - lookback];

            for (int j = 0; j < length; j++)
            {
                map[tileIndex] = copyTileIndex;
                tileIndex++;
            }
        }

        private void CopyFromLookBack(byte commandByte, byte width, int tileCount, byte[] map, byte operand, ref int tileIndex)
        {
            LookBackCopyCommand command = MapDecompressor.GetLookBackCopyCommand(commandByte);
            switch (command)
            {
                case LookBackCopyCommand.RowLargeCopy:
                    // Go up 1/2 row(s) for the index and copies 1-128 tiles from there.
                    this.RowLargeCopyFromLookBack(width, map, operand, ref tileIndex);
                    break;
                case LookBackCopyCommand.RowSmallCopy:
                    // Go up 1 row for the index and copies 1-8 tiles from there.
                    this.SmallRowCopyFromLookBack(commandByte, width, map, ref tileIndex);
                    break;
                case LookBackCopyCommand.LookBackCopy:
                    // Looks back 0-15 tiles and copies 1-128 tiles from there.
                    this.LargeCopyFromLookBack(commandByte, tileCount, map, operand, ref tileIndex);
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("Unknown lookback copy compression command.");
                    break;
            }
        }

        private void RowLargeCopyFromLookBack(byte width, byte[] map, byte operand, ref int tileIndex)
        {
            // Go Up 1/2 Row(s), Copy XX Tiles.

            // In memory SoM maps are always 128x128 tiles regardless of the actual map size.
            // So to go up a row it subtracts 128 from the current location.
            // For this decompression the defined size of the map is used, so width is used instead of 128.

            // If 0x80 of the operand is not set then we go up a single row. If it is set then we go up two rows instead.
            if ((operand & 0B_1000_0000) != 0) { width *= 2; }

            byte length = (byte)((operand & 0B_0111_1111) + 1); // (1,128)
            for (int i = 0; i < length; i++)
            {
                map[tileIndex] = map[tileIndex - width];
                tileIndex++;
            }
        }

        private void SmallRowCopyFromLookBack(byte command, byte width, byte[] map, ref int tileIndex)
        {
            // Go Up 1 Row, Copy XX Tiles.

            // In memory SoM maps are always 128x128 tiles regardless of the actual map size.
            // So to go up a row it subtracts 128 from the current location.
            // For this decompression the defined size of the map is used, so width is used instead of 128.
            byte length = (byte)((command & 0B_0000_0111) + 1); // (1,8)
            for (int i = 0; i < length; i++)
            {
                map[tileIndex] = map[tileIndex - width];
                tileIndex++;
            }
        }

        private void LargeCopyFromLookBack(byte command, int tileCount, byte[] map, byte operand, ref int tileIndex)
        {
            // Go Back XX + 1 Tiles, Copy YY Tiles
            ushort lookback = (ushort)(((command & 0B_0000_0111) * 2) + (operand >> 7)); // (0,15)
            ushort length = (ushort)((operand & 0x7F) + 1); // (1,128)
            for (int j = 0; j < length; j++)
            {
                if (tileIndex < tileCount)
                {
                    map[tileIndex] = map[(tileIndex - lookback) - 1];
                    tileIndex++;
                }
            }
        }

        private void FillWithIncrement(byte commandByte, byte[] map, byte operand, ref int tileIndex)
        {
            MapFillCommand command = MapDecompressor.GetFillCommand(commandByte);
            switch (command)
            {
                case MapFillCommand.IncrementOrDecrement:
                    // Fills 1-128 tiles, starting with the last tile Id and incrementing/decrementing the tile Id each time.
                    this.FillWithIncrementOrDecrement(map, operand, ref tileIndex);
                    break;
                case MapFillCommand.SmallIncrement:
                    // Fills 1-8 tiles, starting with LastTileId+1 and incrementing the tile Id each time.
                    this.SmallFillWithIncrement(commandByte, map, ref tileIndex);
                    break;
                case MapFillCommand.Unknown:
                    // This shouldn't happen anymore.
                    ThrowHelper.ThrowNotImplementedException($"Encountered Fill Command Between F8 and FF. Value: {commandByte:X2}");
                    break;
            }
        }

        private void FillWithIncrementOrDecrement(byte[] map, byte operand, ref int tileIndex)
        {
            // Fill XX Tiles, Starting With The Last Tile And Increment/Decrement Tile Id Each Time
            bool increment = (operand < 0B_1000_0000);
            int length = (operand & 0B_0111_1111) + 1; // (1,128)
            byte fillTileIndex = map[tileIndex - 1];
            for (int i = 0; i < length; i++)
            {
                _ = increment ? fillTileIndex++ : fillTileIndex--;
                map[tileIndex] = fillTileIndex;
                tileIndex++;
            }
        }

        private void SmallFillWithIncrement(byte command, byte[] map, ref int tileIndex)
        {
            // Fill XX Tiles, Starting With The LastTileID+1 And Incrementing Each Time.
            int length = (command & 0B_0000_0111) + 1; // (1,8)
            byte fillTileIndex = map[tileIndex - 1];
            for (int i = 0; i < length; i++)
            {
                fillTileIndex++;
                map[tileIndex] = fillTileIndex;
                tileIndex++;
            }
        }
        
        private static MapCompressionCommand GetCompressionCommand(byte commandByte)
        {
            MapCompressionCommand command = MapCompressionCommand.Tile;
            if (commandByte >= 0xC0)
            {
                command = commandByte <= 0xF7 ? (MapCompressionCommand)(commandByte & 0B_1111_0000) : (MapCompressionCommand)commandByte;
            }
            return command;
        }

        private static LookBackCopyCommand GetLookBackCopyCommand(byte current)
        {
            if ((current & 0B_0000_1000) > 0)
            {
                return LookBackCopyCommand.LookBackCopy;
            }
            return ((current & 0B_0000_0111) > 0) ? LookBackCopyCommand.RowSmallCopy : LookBackCopyCommand.RowLargeCopy;
        }

        private static MapFillCommand GetFillCommand(byte current)
        {
            if ((current & 0B_0000_1000) > 0)
            {
                return MapFillCommand.Unknown;
            }
            return ((current & 0B_0000_0111) > 0) ? MapFillCommand.SmallIncrement : MapFillCommand.IncrementOrDecrement;
        }

        private static bool HasOperand(MapCompressionCommand command, byte commandByte)
        {
            switch (command)
            {
                //case MapCompressionCommand.ShortLookBackTileAndRepeat:
                //case MapCompressionCommand.LongLookBackTileAndRepeat:
                case MapCompressionCommand.CopyFromLookBack:
                    LookBackCopyCommand copyCommand = MapDecompressor.GetLookBackCopyCommand(commandByte);
                    switch (copyCommand)
                    {
                        case LookBackCopyCommand.RowLargeCopy:
                        case LookBackCopyCommand.LookBackCopy:
                            return true;
                    }
                    break;
                //case MapCompressionCommand.CopyFromLookBack: return true;
                case MapCompressionCommand.FillWithIncrement:
                    MapFillCommand fillCommand = MapDecompressor.GetFillCommand(commandByte);
                    return fillCommand == MapFillCommand.IncrementOrDecrement;
            }
            return false;
        }
    }
}

// [05:52:29] (Debug) Debug: [001C]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [001D]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [001E]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [001F]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [003C]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [0082]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [0085]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [0089]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [008B]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [008C]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [008E]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [008F]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [00C2]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [00C5]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [00DE]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [011A]: Piece uses F8-FF Decompression Command <- Wrong
// [05:52:29] (Debug) Debug: [015E]: Piece uses F8-FF Decompression Command
// [05:52:29] (Debug) Debug: [01C1]: Piece uses F8-FF Decompression Command <- Wrong

/*
[07:33:43] (Compression) Debug: (000B) [Map 0082 (Potos Fields 1) Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 00, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (001E) [Map 0085 (Goblin Village) Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 02, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (001F) [Map 0085 (Goblin Village) Layer 2 BG] Replace Map Pieces Detected. F8: True (C0: 01, D0: 00, E0: 00, F0: 01, F8: 00), F9: True (C0: 00, D0: 00, E0: 00, F0: 00, F8: 00), FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (0023) [Map 0089 Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 02, F0: 00, F8: 00), F9: True (C0: 01, D0: 00, E0: 00, F0: 00, F8: 00), FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (0025) [Map 009B Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 01, D0: 00, E0: 00, F0: 00, F8: 00), F9: True (C0: 00, D0: 00, E0: 00, F0: 03, F8: 00), FA: True (C0: 01, D0: 00, E0: 00, F0: 00, F8: 00), FB: True (C0: 02, D0: 00, E0: 01, F0: 00, F8: 00), FC: True (C0: 01, D0: 00, E0: 00, F0: 00, F8: 00), FD: False
[07:33:43] (Compression) Debug: (00A0) [Map 001C Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 03, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (00A1) [Map 001D Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 03, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (00A2) [Map 001E Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 03, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (00A3) [Map 001F Layer 1 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 03, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (0127) [Map 003C (Spring Beak Boss Arena) Layer 2 BG] Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 01, F0: 00, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (0167) Replace Map Pieces Detected. F8: True (C0: 00, D0: 01, E0: 00, F0: 00, F8: 00), F9: True (C0: 00, D0: 00, E0: 00, F0: 00, F8: 00), FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (016F) Replace Map Pieces Detected. F8: True (C0: 06, D0: 00, E0: 06, F0: 02, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (01F6) Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 00, F0: 00, F8: 00), F9: False, FA: False, FB: False, FC: False, FD: False
[07:33:43] (Compression) Debug: (0383) Replace Map Pieces Detected. F8: True (C0: 00, D0: 00, E0: 01, F0: 00, F8: 00), F9: True (C0: 01, D0: 00, E0: 00, F0: 00, F8: 00), FA: False, FB: False, FC: False, FD: False

 */

/* Code Graveyard
        private void RowLargeCopyFromLookBack(byte width, byte[] map, ref int tileIndex)
        {
            // Go Up 1/2 Row(s), Copy XX Tiles.

            // In memory SoM maps are always 128x128 tiles regardless of the actual map size.
            // So to go up a row it subtracts 128 from the current location.
            // For this decompression the defined size of the map is used, so width is used instead of 128.
            byte operand = this.reader.Read();
            this.RowLargeCopyFromLookBack(width, map, operand, ref tileIndex);

            //// If 0x80 of the operand is not set then we go up a single row. If it is set then we go up two rows instead.
            //if ((operand & 0B_1000_0000) != 0) { width *= 2; }

            //byte length = (byte)((operand & 0B_0111_1111) + 1); // (1,128)
            //for (int i = 0; i < length; i++)
            //{
            //    map[tileIndex] = map[tileIndex - width];
            //    tileIndex++;
            //}
        }

        private void LargeCopyFromLookBack(byte command, int tileCount, byte[] map, ref int tileIndex)
        {
            // Go Back XX + 1 Tiles, Copy YY Tiles
            byte operand = this.reader.Read();
            this.LargeCopyFromLookBack(command, tileCount, map, operand, ref tileIndex);

            //ushort lookback = (ushort)(((command & 0B_0000_0111) * 2) + (operand >> 7)); // (0,15)
            //ushort length = (ushort)((operand & 0x7F) + 1); // (1,128)
            //for (int j = 0; j < length; j++)
            //{
            //    if (tileIndex < tileCount)
            //    {
            //        map[tileIndex] = map[(tileIndex - lookback) - 1];
            //        tileIndex++;
            //    }
            //}
        }

        private void FillWithIncrementOrDecrement(byte[] map, ref int tileIndex)
        {
            // Fill XX Tiles, Starting With The Last Tile And Increment/Decrement Tile Id Each Time
            byte operand = this.reader.Read();
            this.FillWithIncrementOrDecrement(map, operand, ref tileIndex);

            //bool increment = (operand < 0B_1000_0000);
            //int length = (operand & 0B_0111_1111) + 1; // (1,128)
            //byte tileId = map[tileIndex - 1];
            //for (int i = 0; i < length; i++)
            //{
            //    _ = increment ? tileId++ : tileId--;
            //    map[tileIndex] = tileId;
            //    tileIndex++;
            //}
        }

        private void CopyFromLookBack(byte commandByte, byte width, int tileCount, byte[] map, ref int tileIndex)
        {
            LookBackCopyCommand command = MapDecompressor.GetLookBackCopyCommand(commandByte);
            byte operand = 0;
            switch (command)
            {
                case LookBackCopyCommand.RowLargeCopy:
                    // Go up 1/2 row(s) for the index and copies 1-128 tiles from there.
                    operand = this.reader.Read();
                    this.RowLargeCopyFromLookBack(width, map, operand, ref tileIndex);
                    break;
                case LookBackCopyCommand.RowSmallCopy:
                    // Go up 1 row for the index and copies 1-8 tiles from there.
                    this.SmallRowCopyFromLookBack(commandByte, width, map, ref tileIndex);
                    break;
                case LookBackCopyCommand.LookBackCopy:
                    // Looks back 0-15 tiles and copies 1-128 tiles from there.
                    operand = this.reader.Read();
                    this.LargeCopyFromLookBack(commandByte, tileCount, map, operand, ref tileIndex);
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("Unknown lookback copy compression command.");
                    break;
            }
        }

        private void FillWithIncrement(byte commandByte, byte[] map, ref int tileIndex)
        {
            MapFillCommand command = MapDecompressor.GetFillCommand(commandByte);
            byte operand = 0;
            switch (command)
            {
                case MapFillCommand.IncrementOrDecrement:
                    // Fills 1-128 tiles, starting with the last tile Id and incrementing/decrementing the tile Id each time.
                    operand = this.reader.Read();
                    this.FillWithIncrementOrDecrement(map, operand, ref tileIndex);
                    break;
                case MapFillCommand.SmallIncrement:
                    // Fills 1-8 tiles, starting with LastTileId+1 and incrementing the tile Id each time.
                    this.SmallFillWithIncrement(commandByte, map, ref tileIndex);
                    break;
                case MapFillCommand.Unknown:
                    // TODO
                    ThrowHelper.ThrowNotImplementedException($"Encountered Fill Command Between F8 and FF. Value: {commandByte:X2}");
                    break;
            }
        }
 */