using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.RomReaders;
using ManaMagic.Core.Sprites;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

namespace ManaMagic.Controls.UserControls.Debug
{
    public partial class DebugDrawCanvasUserControl : UserControl
    {
        public DebugDrawCanvasUserControl()
        {
            InitializeComponent();

            this.previousCanvas1Button.Enabled = false;
            this.Load += DebugDrawCanvasUserControl_Load;
        }

        public sealed class SpriteTileset : Tileset
        {
            public IReadOnlyDictionary<ushort, int> ReverseLookup { get; }

            public SpriteTileset(int index, IReadOnlyList<GraphicTile> tiles, TileType tileType, IReadOnlyDictionary<ushort, int> reverseLookup) : base(index, tiles, tileType)
            {
                this.ReverseLookup = reverseLookup;
            }
        }

        private SpriteTileset tileset;
        private SpriteGraphicsData graphicsData;
        private SpriteRomReader romReader;
        private int frameIndex = 0;

        private void SpriteTest2(int index, int tilesToLoad)
        {
            this.graphicsData = ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[index];
            SpritePalette palette = ManaMagicContext.Current.Context.SpriteContext.PaletteTable[index];

            this.tileset = this.GetSpriteTileset(romReader, tilesToLoad);

            using SuperNintendoGraphics graphics = this.tileset.DrawTileset(palette, 32);
            this.canvas1PictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        private void ReadAndDrawSpriteFrame()
        {
            // 205 wrong
            this.romReader.Seek(this.graphicsData.FrameIndexOffset + 0x130000);

            romReader.Position += (this.frameIndex * 2);
            ushort frameOffset = this.romReader.ReadUInt16();

            int framePointer = 0x130000 + frameOffset;
            this.romReader.Seek(framePointer);

            byte frameControl = this.romReader.Read();
            bool hasCollision = (frameControl & 0x40) > 0;
            byte tile16Count = (byte)(frameControl & 0x0F);
            byte tile8Count = (byte)(tile16Count * 4);
            List<ushort> tileOffsets = new List<ushort>(tile8Count);
            for (int tileIndex = 0; tileIndex <= tile8Count; tileIndex++)
            {
                // $00/F676 29 F0 3F    AND #$3FF0              A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0348 VC:041 FC:31 I:01
                // $00/F679 0A          ASL A                   A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0388 VC:041 FC:31 I:01
                // $00/F67A 65 0A       ADC $0A[$00:000A]       A:1AA0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0418 VC:041 FC:31 I:01
                // $00/F67C 99 00 00    STA $0000,y[$7E:0E60]   A:7FE0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0466 VC:041 FC:31 I:01
                ushort encodedOffset = this.romReader.ReadUInt16();

                bool loadMultipleTiles = (encodedOffset & 0x8000) > 0;
                bool loadWithIncrement = (encodedOffset & 0x4000) > 0;
                byte loopCount = (byte)((encodedOffset & 0x0F) + 1);

                // D3/3D27:	14 C0 -> C0 14
                if (loadMultipleTiles)
                {
                    for (int loopIndex = 0; loopIndex < loopCount; loopIndex++)
                    {
                        if (loadWithIncrement)
                        {
                            ushort offset = (ushort)(((encodedOffset * 2) & 0x7FE0) + (loopIndex * 32));
                            tileOffsets.Add(offset);
                        }
                        else
                        {
                            ushort offset = (ushort)((encodedOffset * 2) & 0x7FE0);
                            tileOffsets.Add(offset);
                        }
                        tileIndex++;
                    }
                }
                else
                {
                    ushort offset = (ushort)((encodedOffset & 0x3FF0) * 2);
                    tileOffsets.Add(offset);
                }
            }

            //List<(byte XCoordinate, byte YCoordinate)> coordList = new List<(byte XCoordinate, byte YCoordinate)>();
            List<(sbyte XCoordinate, sbyte YCoordinate)> coordList = new List<(sbyte XCoordinate, sbyte YCoordinate)>();
            List<FlipType> flipTypeList = new List<FlipType>();
            for (int tileIndex = 0; tileIndex < tile16Count; tileIndex++)
            {
                byte yCoordinate = this.romReader.Read();
                byte xCoordinate = this.romReader.Read();
                FlipType flipType = FlipType.None;

                bool xNegFlag = (xCoordinate & 0x40) > 0;
                bool yNegFlag = (yCoordinate & 0x40) > 0;

                if ((xCoordinate & 0x80) > 0) { flipType |= FlipType.HorizontalFlip; }
                if ((xCoordinate & 0x80) > 0) { flipType |= FlipType.VerticalFlip; }
                flipTypeList.Add(flipType);

                //xCoordinate = (sbyte)((xCoordinate & 0xF0) >> 4);
                xCoordinate = (byte)((xCoordinate & 0x3F) >> 2);
            yCoordinate = (byte)((yCoordinate & 0x3F) >> 2);

                if (xNegFlag) { xCoordinate |= 0x80; }
                if (yNegFlag) { yCoordinate |= 0x80; }

                sbyte xCoord2 = xNegFlag ? (sbyte)-xCoordinate : (sbyte)xCoordinate;
                sbyte yCoord2 = yNegFlag ? (sbyte)-yCoordinate : (sbyte)yCoordinate;

                //xCoordinate >>= 1;
                //yCoordinate >>= 1;

                coordList.Add(((sbyte)xCoordinate, (sbyte)yCoordinate));
                //coordList.Add((xCoordinate, yCoordinate));
                //coordList.Add((xCoord2, yCoord2));

                // [73]: 0111_0011
                // [33]: 0011_0011
                // [13]: 0001_0011
            }

            for (int i = 0; i < tileOffsets.Count; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X2}]: {tileOffsets[i]:X4} ({tileset.ReverseLookup[tileOffsets[i]]:X2})");
            }
            for (int i = 0; i < coordList.Count; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"X-Coordinate: {coordList[i].XCoordinate}, Y-Coordinate: {coordList[i].YCoordinate}");
            }
            ////////////////////////////////////////////////F8 6C
            //$00/F745 E2 20       SEP #$20                A:F8EC
            //6C: 0110_1100
            //7C: 0111_1100
            //EC: 1110_1100

            int size = 384;
            using SuperNintendoGraphics graphics2 = SuperNintendoGraphics.CreateGraphics(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[0], size, size);
            graphics2.DrawGridlines(Rgb555Color.Navy, size / 8, size / 8);

            List<GraphicTile> tiles = new List<GraphicTile>();

            for (int i = 0; i < tile16Count; i++)
            {
                tiles.Clear();
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[0 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[1 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[2 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[3 + (i * 4)]]]);

                FlipType flip = flipTypeList[i];
                graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[i].XCoordinate / 1), (size / 2) + (coordList[i].YCoordinate / 1), flip);
            }

            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[0]]]);
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[1]]]);
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[2]]]);
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[3]]]);
            //graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[0].XCoordinate / 1), (size / 2) + (coordList[0].YCoordinate / 1));

            //tiles.Clear();
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[4]]]);
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[5]]]);
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[6]]]);
            //tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[7]]]);
            //graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[1].XCoordinate / 1), (size / 2) + (coordList[1].YCoordinate / 1));

            this.canvas2PictureBox.Image = new Bitmap(graphics2.GetBitmap(true), graphics2.Size.Width * 2, graphics2.Size.Height * 2);
        }

        private SpriteTileset GetSpriteTileset(SpriteRomReader romReader, int tilesToLoad)
        {
            List<GraphicTile4Bpp> tileList = new List<GraphicTile4Bpp>(tilesToLoad);
            Dictionary<ushort, int> reverseLookup = new Dictionary<ushort, int>(tilesToLoad);

            romReader.Seek((int)this.graphicsData.GraphicsAddress);

            Span<byte> tileBuffer = stackalloc byte[32];
            for (int i = 0; i < tilesToLoad; i++)
            {
                ushort offset = (ushort)(romReader.Position - graphicsData.GraphicsAddress);
                reverseLookup.Add(offset, i);

                tileBuffer.Fill(0);
                for (int j = 0; j < 32; j++)
                {
                    tileBuffer[j] = romReader.Read();
                }
                tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
            }

            SpriteTileset tileset = new SpriteTileset(0, tileList, TileType.FourBitsPerPixel, reverseLookup);
            return tileset;
        }

        private void SpriteTest()
        {
            SpriteRomReader romReader = RomReaderFactory.GetRomReader<SpriteRomReader>();
            romReader.Seek((int)ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[0].GraphicsAddress);

            List<GraphicTile4Bpp> tileList = new List<GraphicTile4Bpp>(84);
            Dictionary<ushort, int> reverseLookup = new Dictionary<ushort, int>(84);
            Span<byte> tileBuffer = stackalloc byte[32];
            for (int i = 0; i < 214; i++)
            {
                ushort offset = (ushort)(romReader.Position - ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[0].GraphicsAddress);
                reverseLookup.Add(offset, i);

                tileBuffer.Fill(0);
                for (int j = 0; j < 32; j++)
                {
                    tileBuffer[j] = romReader.Read();
                }
                tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
            }

            Tileset tileset = new Tileset(0, tileList, TileType.FourBitsPerPixel);
            using SuperNintendoGraphics graphics = tileset.DrawTileset(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[0], 32);
            this.canvas1PictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);

            romReader.Seek((int)ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[0].FrameIndexOffset + 0x130000);

            //romReader.Position += 2;
            ushort frameOffset = romReader.ReadUInt16();

            int framePointer = 0x130000 + frameOffset;
            romReader.Seek(framePointer);

            byte frameControl = romReader.Read();
            bool hasCollision = (frameControl & 0x40) > 0;
            byte tile16Count = (byte)((frameControl & 0x0F) * 4);
            List<ushort> tileOffsets = new List<ushort>(tile16Count);
            for (int tileIndex = 0; tileIndex < tile16Count; tileIndex++)
            {
                // $00/F676 29 F0 3F    AND #$3FF0              A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0348 VC:041 FC:31 I:01
                // $00/F679 0A          ASL A                   A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0388 VC:041 FC:31 I:01
                // $00/F67A 65 0A       ADC $0A[$00:000A]       A:1AA0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0418 VC:041 FC:31 I:01
                // $00/F67C 99 00 00    STA $0000,y[$7E:0E60]   A:7FE0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0466 VC:041 FC:31 I:01
                ushort encodedOffset = romReader.ReadUInt16();

                bool loadMultipleTiles = (encodedOffset & 0x8000) > 0;
                bool loadWithIncrement = (encodedOffset & 0x4000) > 0;
                byte loopCount = (byte)((encodedOffset & 0x0F) + 1);

                // D3/3D27:	14 C0 -> C0 14
                if (loadMultipleTiles)
                {
                    for (int loopIndex = 0; loopIndex < loopCount; loopIndex++)
                    {
                        if (loadWithIncrement)
                        {
                            ushort offset = (ushort)(((encodedOffset * 2) & 0x7FE0) + (loopIndex * 32));
                            tileOffsets.Add(offset);
                        }
                        else
                        {
                            ushort offset = (ushort)((encodedOffset * 2) & 0x7FE0);
                            tileOffsets.Add(offset);
                        }
                        tileIndex++;
                    }
                }
                else
                {
                    ushort offset = (ushort)((encodedOffset & 0x3FF0) * 2);
                    tileOffsets.Add(offset);
                }
            }

            List<(sbyte XCoordinate, sbyte YCoordinate)> coordList = new List<(sbyte XCoordinate, sbyte YCoordinate)>();
            for (int tileIndex = 0; tileIndex < tile16Count / 4; tileIndex++)
            {
                sbyte yCoordinate = romReader.ReadSByte();
                sbyte xCoordinate = romReader.ReadSByte();

                //xCoordinate = (sbyte)((xCoordinate & 0xF0) >> 4);
                xCoordinate = (sbyte)(xCoordinate & 0x3F);
                //yCoordinate = (sbyte)(yCoordinate & 0xBF);
                //xCoordinate = (sbyte)(xCoordinate | 0x80);
                //xCoordinate = (sbyte)-xCoordinate;
                coordList.Add((xCoordinate, yCoordinate));
            }

            for (int i = 0; i < tileOffsets.Count; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X2}]: {tileOffsets[i]:X4} ({reverseLookup[tileOffsets[i]]:X2})");
            }
            for (int i = 0; i < tileOffsets.Count / 4; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"X-Coordinate: {coordList[i].XCoordinate}, Y-Coordinate: {coordList[i].YCoordinate}");
            }
            ////////////////////////////////////////////////F8 6C
            //$00/F745 E2 20       SEP #$20                A:F8EC
            //6C: 0110_1100
            //7C: 0111_1100
            //EC: 1110_1100

            int size = 384;
            using SuperNintendoGraphics graphics2 = SuperNintendoGraphics.CreateGraphics(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[0], size, size);
            graphics2.DrawGridlines(Rgb555Color.Navy, size / 8, size / 8);

            List<GraphicTile> tiles = new List<GraphicTile>();
            tiles.Add(tileset[reverseLookup[tileOffsets[0]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[1]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[2]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[3]]]);
            graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[0].XCoordinate / 1), (size / 2) + (coordList[0].YCoordinate / 1));

            tiles.Clear();
            tiles.Add(tileset[reverseLookup[tileOffsets[4]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[5]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[6]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[7]]]);
            graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[1].XCoordinate / 1), (size / 2) + (coordList[1].YCoordinate / 1));

            this.canvas2PictureBox.Image = new Bitmap(graphics2.GetBitmap(true), graphics2.Size.Width * 2, graphics2.Size.Height * 2);
        }

        private void DebugDrawCanvasUserControl_Load(object sender, EventArgs e)
        {
            //this.SpriteTest();

            this.romReader = RomReaderFactory.GetRomReader<SpriteRomReader>();
            this.SpriteTest2(0x00, 214); // Rabite

            this.ReadAndDrawSpriteFrame();
        }

        private void PreviousCanvas1Button_Click(object sender, EventArgs e)
        {
        }

        private void NextCanvas1Button_Click(object sender, EventArgs e)
        {
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void C2NextButton_Click(object sender, EventArgs e)
        {
            this.frameIndex++;
            this.ReadAndDrawSpriteFrame();
        }

        private void C2PrevButton_Click(object sender, EventArgs e)
        {
            this.frameIndex--;
            this.ReadAndDrawSpriteFrame();
        }
    }


}
/* Code Graveyard: Maps
        private int tileset8x8Index = 0;

        public DebugDrawCanvasUserControl()
        {
            InitializeComponent();

            this.previousCanvas1Button.Enabled = false;
            this.Load += DebugDrawCanvasUserControl_Load;
        }

        private void DebugDrawCanvasUserControl_Load(object sender, EventArgs e)
        {
            this.canvas1PictureBox.Image = TestDraw8x8MapTileset(this.tileset8x8Index);
            this.canvas2PictureBox.Image = TestDraw16x16MapTileset(this.tileset8x8Index);

            numericUpDown1.Value = 0x9F;
            this.canvas1PictureBox.Image = this.TestDrawMap().GetBitmap(true);
        }

        private void previousCanvas1Button_Click(object sender, EventArgs e)
        {
            this.tileset8x8Index--;
            this.nextCanvas1Button.Enabled = true;
            if (this.tileset8x8Index == 0)
            {
                this.previousCanvas1Button.Enabled = false;
            }
            this.canvas1PictureBox.Image = TestDraw8x8MapTileset(this.tileset8x8Index);
            this.canvas2PictureBox.Image = TestDraw16x16MapTileset(this.tileset8x8Index);
        }

        private void nextCanvas1Button_Click(object sender, EventArgs e)
        {
            this.tileset8x8Index++;
            this.previousCanvas1Button.Enabled = true;
            if ((this.tileset8x8Index + 1) >= (SecretOfManaContext.Default.MapContext.Map8x8TilesetTable.RowCount - 1))
            {
                this.nextCanvas1Button.Enabled = false;
            }
            this.canvas1PictureBox.Image = TestDraw8x8MapTileset(this.tileset8x8Index);
            this.canvas2PictureBox.Image = TestDraw16x16MapTileset(this.tileset8x8Index);
        }

        private SuperNintendoGraphics TestDrawMap()
        {
            //const int mapId = 0x58;
            //const int mapId = 0x57;
            int mapId = (int)numericUpDown1.Value;
            return SecretOfManaContext.Default.MapContext.CreateMap(mapId).DrawCompleteMap();
        }

        private SuperNintendoGraphics TestDrawMap2()
        {
            //const int mapId = 0x9F; // Cannon Travel
            //const int mapId = 0x83; // Potos
            int mapId = (int)numericUpDown1.Value;
            MapRomReader reader = RomReaderFactory.GetRomReader<MapRomReader>();

            MapHeader header = SecretOfManaContext.Default.MapContext.MapHeaderTable[mapId];
            MapObjectTable objectTable = SecretOfManaContext.Default.MapContext.MapObjectTable[mapId];

            Tileset tileset8x8 = SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[header.Tileset8x8Index];
            DataTable<Map16x16Tile> tileset16x16 = SecretOfManaContext.Default.MapContext.Map16x16TilesetTable[header.Tileset16x16Index];
            DataTable<SpritePalette> paletteSet = SecretOfManaContext.Default.MapContext.PaletteSetTable[header.PaletteSetIndex & 0x7F];

            MapPiece mapBackground = reader.ReadMapPiece(0, SecretOfManaContext.Default.MapContext.GetMapPieceAddress(objectTable.Layer1Background.Index));
            List<Map16x16Tile> mapTilesBG = new List<Map16x16Tile>(mapBackground.Width * mapBackground.Height);
            foreach (byte tileId in mapBackground.TileIdTable)
            {
                mapTilesBG.Add(tileset16x16[tileId]);
            }
            SuperNintendoGraphics bg = TestDrawMap(header.Tileset8x8Index, mapBackground.Width, mapBackground.Height, 0, 0, tileset8x8, mapTilesBG, paletteSet);
            foreach (MapObject mapObject in objectTable.Layer1)
            {
                MapPiece mapPiece = reader.ReadMapPiece(0, SecretOfManaContext.Default.MapContext.GetMapPieceAddress(mapObject.Index));
                List<Map16x16Tile> mapTilesObj = new List<Map16x16Tile>(mapPiece.Width * mapPiece.Height);
                foreach (byte tileId in mapPiece.TileIdTable)
                {
                    mapTilesObj.Add(tileset16x16[tileId]);
                }
                using SuperNintendoGraphics obj = TestDrawMap(header.Tileset8x8Index, mapPiece.Width, mapPiece.Height, 0, 0, tileset8x8, mapTilesObj, paletteSet);
                bg.Merge(obj, mapObject.Coordinates.X * 8, mapObject.Coordinates.Y * 8);
            }

            MapPiece mapForeground = reader.ReadMapPiece(0, SecretOfManaContext.Default.MapContext.GetMapPieceAddress(objectTable.Layer2Background.Index));
            List<Map16x16Tile> mapTilesFG = new List<Map16x16Tile>(mapForeground.Width * mapForeground.Height);
            foreach (byte tileId in mapForeground.TileIdTable)
            {
                mapTilesFG.Add(tileset16x16[tileId + 192]);
            }

            using SuperNintendoGraphics fg = TestDrawMap(header.Tileset8x8Index, mapForeground.Width, mapForeground.Height, 0, 0, tileset8x8, mapTilesFG, paletteSet);
            bg.Merge(fg);
            return bg;
        }

        public SuperNintendoGraphics TestDrawMap(int index, int width, int height, int xOffset, int yOffset, Tileset map8x8Tileset, List<Map16x16Tile> map16x16Tileset, DataTable<SpritePalette> paletteSet)
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, width * 16, height * 16);

            int rowIndex = 0;
            int columnIndex = 0;
            for (int i = 0; i < map16x16Tileset.Count; i++)
            {
                Map16x16Tile tile = map16x16Tileset[i];
                GraphicTile quadrant1 = map8x8Tileset[tile.Quadrants[0].TileIndex];
                GraphicTile quadrant2 = map8x8Tileset[tile.Quadrants[1].TileIndex];
                GraphicTile quadrant3 = map8x8Tileset[tile.Quadrants[2].TileIndex];
                GraphicTile quadrant4 = map8x8Tileset[tile.Quadrants[3].TileIndex];

                graphics.DrawTile(quadrant1, (rowIndex * 16) + xOffset, (columnIndex * 16) + yOffset, tile.Quadrants[0].FlipType, tile.Quadrants[0].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant2, (rowIndex * 16) + 8 + xOffset, (columnIndex * 16) + yOffset, tile.Quadrants[1].FlipType, tile.Quadrants[1].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant3, (rowIndex * 16) + xOffset, (columnIndex * 16) + 8 + yOffset, tile.Quadrants[2].FlipType, tile.Quadrants[2].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant4, (rowIndex * 16) + 8 + xOffset, (columnIndex * 16) + 8 + yOffset, tile.Quadrants[3].FlipType, tile.Quadrants[3].ModifiedPaletteIndex);

                rowIndex++;
                // Wrap around to the start of the next row if we have reached the end of the current row.
                if (rowIndex == width)
                {
                    rowIndex = 0;
                    columnIndex++;
                }
            }
            return graphics;
        }

        private (byte Width, byte Height, byte[] Map) ReadPotos(int address)
        {
            GenericRomReader reader = RomReaderFactory.GetRomReader<GenericRomReader>();
            reader.Seek(address);

            byte width = (byte)((reader.Read() / 2) + 1);
            byte height = (byte)((reader.Read() / 2) + 1);
            int tileCount = width * height;
            byte[] map = new byte[tileCount];

            int tilesIndex = 0;
            while (tilesIndex < tileCount)
            {
                byte current = reader.Read();
                if (current <= 0xBF)
                {
                    map[tilesIndex] = current;
                    tilesIndex++;
                }
                else if ((current >= 0xC0) && (current <= 0xDF))
                {
                    // Get Tile XX Spaces Back, Repeat It YY Number Of Times.
                    //int val = current;
                    byte length = (byte)((current & 0B_0000_0111) + 1);
                    byte backCount = (byte)(((current & 0B_0001_1000) >> 0x03) + 1);
                    byte tileId = map[tilesIndex - backCount];


                    for (int j = 0; j < length; j++)
                    {
                        map[tilesIndex] = tileId;
                        tilesIndex++;
                    }
                }
                else if (current == 0xE0) // 11100000
                {
                    // Go Up 1/2 Row(s), Repeat Tiles From There XX Times.

                    // Remember dumbass, replication is not emulation.
                    // The game reserves 0x80 bytes per row regardless of the actual width of the map.
                    // So to go up a row the game subtracts 0x80 from the current location. 
                    // Since we are defining the truth width of the map we have to subtract the width instead.

                    // If Bit 7 of the operand is not set then we go up a single row. If it is set then we go up two rows instead.
                    // TilesToRepeat = [(operand & 0x7F) + 1], meaning at most we can repeat an entire row with this command.
                    int val = width;
                    byte operand = reader.Read();
                    if ((operand & 0x80) != 0) { val *= 2; }
                    operand &= 0x7F;
                    operand++;

                    for (int j = 0; j < operand; j++)
                    {
                        map[tilesIndex] = map[tilesIndex - val];
                        tilesIndex++;
                    }
                }
                else if ((current >= 0xE1) && (current <= 0xE7))
                {
                    // Go up one row and repeat XX + 1 tiles.
                    byte numTiles = (byte)((current & 0x07) + 1);


                    for (int j = 0; j < numTiles; j++)
                    {
                        map[tilesIndex] = map[tilesIndex - width];
                        tilesIndex++;
                    }
                }
                else if ((current >= 0xE8) && (current <= 0xEF))
                {
                    byte operand = reader.Read();
                    // We'll be going backwards this many tiles from the current location to get a tile to draw.
                    // Fuck this still doesn't work right.
                    //ushort backCount = (ushort)((readByte & 0x07) | (operand >> 7));

                    // This seems to be the right way?
                    //ushort backCount = (ushort)((readByte & 0x07) + (operand >> 7));

                    // Third Attempt
                    ushort backCount = (ushort)(((current & 0x07) * 2) + ((operand >> 7)));

                    //backCount *= 2;

                    // And we'll draw it this many times.
                    ushort drawCount = (ushort)((operand & 0x7F) + 1);

                    for (int j = 0; j < drawCount; j++)
                    {
                        if (tilesIndex < tileCount)
                        {
                            map[tilesIndex] = map[(tilesIndex - backCount) - 1];
                            tilesIndex++;
                        }
                    }
                }
                else if (current == 0xF0)
                {
                    byte operand = reader.Read();
                    bool forward = (operand < 0x80);
                    int count = (operand & 0x7F) + 1;


                    byte t = map[tilesIndex - 1];
                    for (int j = 0; j < count; j++)
                    {
                        if (forward) { t++; }
                        else { t--; }
                        map[tilesIndex] = t;
                        tilesIndex++;
                    }
                }
                else if ((current >= 0xF1) && (current <= 0xF7))
                {
                    // Fill XX Spaces, Increment Tile Id Each Time.
                    int tilesToDraw = (current & 0x07) + 1;

                    // Get the last tile Id.
                    byte t = map[tilesIndex - 1];

                    for (int j = 0; j < tilesToDraw; j++)
                    {
                        t++;
                        map[tilesIndex] = t;
                        tilesIndex++;
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            return (width, height, map);
        }

        private Bitmap TestDraw8x8MapTileset(int index)
        {
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[index];
            if (!metadata.IsDummiedOut)
            {
                Tileset tileset = SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[metadata.Index];
                SpritePalette palette = SecretOfManaContext.Default.MapContext.PaletteSetTable[metadata.DefaultPaletteSet][metadata.DefaultPaletteIndex];

                using SuperNintendoGraphics graphics = tileset.DrawTileset(palette, 32);
                return new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }
            return null;
        }

        private Bitmap TestDraw16x16MapTileset(int index)
        {
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[index];
            if (!metadata.IsDummiedOut)
            {
                Tileset tileset8x8 = SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[metadata.Index];
                DataTable<Map16x16Tile> tileset16x16 = SecretOfManaContext.Default.MapContext.Map16x16TilesetTable[metadata.Index];
                DataTable<SpritePalette> paletteSet = SecretOfManaContext.Default.MapContext.PaletteSetTable[metadata.DefaultPaletteSet];

                using SuperNintendoGraphics graphics = this.TestDrawMap16x16(0x00, tileset8x8, tileset16x16, paletteSet);
                return new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }
            return null;
        }

        public SuperNintendoGraphics TestDrawMap16x16(int index, Tileset map8x8Tileset, DataTable<Map16x16Tile> map16x16Tileset, DataTable<SpritePalette> paletteSet)
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, 32 * 16, 32 * 12);

            int rowIndex = 0;
            int columnIndex = 0;
            for (int i = 0; i < 192; i++)//map16x16Tileset.Count
            //for (int i = 192; i < 384; i++)//map16x16Tileset.Count
            {
                Map16x16Tile tile = map16x16Tileset[i];
                GraphicTile quadrant1 = map8x8Tileset[tile.Quadrants[0].TileIndex];
                GraphicTile quadrant2 = map8x8Tileset[tile.Quadrants[1].TileIndex];
                GraphicTile quadrant3 = map8x8Tileset[tile.Quadrants[2].TileIndex];
                GraphicTile quadrant4 = map8x8Tileset[tile.Quadrants[3].TileIndex];

                graphics.DrawTile(quadrant1, (rowIndex * 16), (columnIndex * 16), tile.Quadrants[0].FlipType, tile.Quadrants[0].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant2, (rowIndex * 16) + 8, (columnIndex * 16), tile.Quadrants[1].FlipType, tile.Quadrants[1].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant3, (rowIndex * 16), (columnIndex * 16) + 8, tile.Quadrants[2].FlipType, tile.Quadrants[2].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant4, (rowIndex * 16) + 8, (columnIndex * 16) + 8, tile.Quadrants[3].FlipType, tile.Quadrants[3].ModifiedPaletteIndex);

                rowIndex++;
                // Wrap around to the start of the next row if we have reached the end of the current row.
                if (rowIndex == 16)
                {
                    rowIndex = 0;
                    columnIndex++;
                }
            }
            return graphics;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.TestDrawMapEx();
           // try
            //{
            //    this.canvas1PictureBox.Image = this.TestDrawMap().GetBitmap(true);
            //}
            //catch (Exception ex)
            //{
            //    this.canvas1PictureBox.Image = null;
            //    LoggerEngine.Logger.LogError(LogComponent.General, ex.Message);
            //}
        }

        private void TestDrawMapEx()
        {
            //const int mapId = 0x58;
            //const int mapId = 0x57;
            int mapId = (int)numericUpDown1.Value;
            try
            {
                ManaMap map = SecretOfManaContext.Default.MapContext.CreateMap(mapId);
                if (map.IsValid)
                {
                    using SuperNintendoGraphics graphics = SecretOfManaContext.Default.MapContext.CreateMap(mapId).DrawCompleteMap();
                    this.canvas1PictureBox.Image = graphics.GetBitmap(true);
                    return;
                }
                this.canvas1PictureBox.Image = null;

                if (!map.Header.IsValid && !map.ObjectTable.IsValid)
                {
                    LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Header and Object Table are invalid.");
                }
                else if (!map.Header.IsValid)
                {
                    LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Header is invalid.");
                }
                else if (!map.ObjectTable.IsValid)
                {
                    LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Object Table is invalid.");
                }
                else { LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}."); }
            }
            catch (Exception ex)
            {
                this.canvas1PictureBox.Image = null;
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
            }
        }

        private void TestDrawMapEx2()
        {
            //const int mapId = 0x58;
            //const int mapId = 0x57;
            int mapId = (int)numericUpDown1.Value;
            ManaMap map = SecretOfManaContext.Default.MapContext.CreateMap(mapId);
            if (map.IsValid)
            {
                using SuperNintendoGraphics graphics = this.TestDrawMap2();
                this.canvas1PictureBox.Image = graphics.GetBitmap(true);
                return;
            }
            this.canvas1PictureBox.Image = null;
            LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}");
        }
 */