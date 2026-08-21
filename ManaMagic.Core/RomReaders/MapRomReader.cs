using System;
using System.Collections.Generic;
using System.Linq;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Maps.Decompressor;
using ManaMagic.Core.Metadata;
using ZwellTech;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    /// <summary>
    /// Provides a Secret of Mana ROM reader that encapsulates methods to read map related data.
    /// </summary>
    public sealed class MapRomReader : RomReader
    {
        private readonly MapDecompressor decompressor;

        /// <inheritdoc/>
        public MapRomReader(RomFile rom) : base(rom) { this.decompressor = new MapDecompressor(this); }

        /// <summary>
        /// Reads the map base graphics 24-bit address table.
        /// </summary>
        /// <returns>A data table of map graphics base addresses.</returns>
        public DataTable<uint> ReadMapGraphicalBaseAddressTable()
        {
            this.Seek((int)Constants.Bank0C.MapGraphicalBaseAddressTableAddress);

            List<uint> addressList = new List<uint>((int)Constants.Bank0C.MapGraphicalBaseAddressTableSize);
            for (int i = 0; i < Constants.Bank0C.MapGraphicalBaseAddressTableSize; i++)
            {
                addressList.Add(this.ReadUInt24());
            }
            return new DataTable<uint>(addressList);
        }

        /// <summary>
        /// Reads the 8x8 map tileset 16-bit address table.
        /// </summary>
        /// <returns>A data table of the 8x8 map tileset addresses.</returns>
        public DataTable<ushort> Read8x8MapTilesetAddressTable()
        {
            this.Seek((int)Constants.Bank0C.Map8x8TilesetPointerTableAddress);

            List<ushort> addressList = new List<ushort>((int)Constants.Bank0C.Map8x8TilesetPointerTableSize);
            for (int i = 0; i < Constants.Bank0C.Map8x8TilesetPointerTableSize; i++)
            {
                addressList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(addressList);
        }

        /// <summary>
        /// Reads the 16x16 map tileset 16-bit address table.
        /// </summary>
        /// <returns>A data table of the 16x16 map tileset addresses.</returns>
        public DataTable<uint> Read16x16MapTilesetAddressTable()
        {
            this.Seek((int)Constants.Bank0B.Map16x16TilesetPointerTableAddress);

            List<uint> addressList = new List<uint>((int)Constants.Bank0B.Map16x16TilesetPointerTableSize);
            for (int i = 0; i < Constants.Bank0B.Map16x16TilesetPointerTableSize; i++)
            {
                addressList.Add(this.ReadUInt24());
            }
            return new DataTable<uint>(addressList);
        }
        
        public DataTable<ushort> ReadMapPieceIndexMaximumTable()
        {
            this.Seek((int)Constants.Bank0D.MapPieceIndexMaximumTableAddress);

            List<ushort> maxIndexList = new List<ushort>((int)Constants.Bank0D.MapPieceIndexMaximumTableSize);
            for (int i = 0; i < Constants.Bank0D.MapPieceIndexMaximumTableSize; i++)
            {
                maxIndexList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(maxIndexList);
        }

        public DataTable<ushort> ReadMapPieceAddressTable()
        {
            this.Seek((int)Constants.Bank0D.MapPiecePointerTableAddress);

            List<ushort> addressList = new List<ushort>((int)Constants.Bank0D.MapPiecePointerTableSize);
            for (int i = 0; i < Constants.Bank0D.MapPiecePointerTableSize; i++)
            {
                addressList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(addressList);
        }

        public DataTable<MapDisplaySettings> ReadDisplaySettingsTable()
        {
            this.Seek((int)Constants.Bank08.MapDisplaySettingsTable);

            List<MapDisplaySettings> displaySettingsList = new List<MapDisplaySettings>((int)Constants.Bank08.MapDisplaySettingsTableSize);
            for (int i = 0; i < Constants.Bank08.MapDisplaySettingsTableSize; i++)
            {
                byte mosaic = this.Read();
                byte mainScreenLayersEnabled = this.Read();
                byte subScreenLayersEnabled = this.Read();
                byte colorMath = this.Read();
                byte unusedValue01 = this.Read();
                byte unusedValue02 = this.Read();
                byte unusedValue03 = this.Read();
                Rgb555Color backgroundColor = Rgb555Color.FromRgb(this.ReadUInt16(), Rgb555Format.BGR);

                MapDisplaySettings settings = new MapDisplaySettings((byte)i, mosaic, mainScreenLayersEnabled, subScreenLayersEnabled, colorMath, unusedValue01, unusedValue02, unusedValue03, backgroundColor);
                displaySettingsList.Add(settings);
            }
            return new DataTable<MapDisplaySettings>(displaySettingsList);
        }

        public DataTable<MapCollision> ReadMapCollisionDefinitionTable()
        {
            this.Seek((int)Constants.Bank0B.CollisionTypeDefinitionsTableAddress);

            List<MapCollision> collisionList = new List<MapCollision>((int)Constants.Bank0B.CollisionTypeDefinitionsTableSize);
            for (int i = 0; i < Constants.Bank0B.CollisionTypeDefinitionsTableSize; i++)
            {
                byte b1 = this.Read();
                byte b2 = this.Read();
                byte b3 = this.Read();
                byte b4 = this.Read();

                MapCollision settings = new MapCollision((MapCollisionType)i, b1, b2, b3, b4);
                collisionList.Add(settings);
            }
            return new DataTable<MapCollision>(collisionList);
        }

        public DataTable<MapCollisionSet> ReadMapTilesetCollisionTable()
        {
            this.Seek((int)Constants.Bank0B.MapCollisionDefinitionTableAddress);

            List<MapCollisionSet> collisionList = new List<MapCollisionSet>((int)Constants.Bank0B.MapCollisionDefinitionTableSize);
            for (int i = 0; i < Constants.Bank0B.MapCollisionDefinitionTableSize; i++)
            {
                List<MapCollisionType> layer1 = new List<MapCollisionType>((int)Constants.Bank0B.MapCollisionDefinitionSize);
                List<MapCollisionType> layer2 = new List<MapCollisionType>((int)Constants.Bank0B.MapCollisionDefinitionSize);
                for (int layer1Index = 0; layer1Index < Constants.Bank0B.MapCollisionDefinitionSize; layer1Index++)
                {
                    MapCollisionType collision = (MapCollisionType)this.Read();
                    layer1.Add(collision);
                }

                for (int layer2Index = 0; layer2Index < Constants.Bank0B.MapCollisionDefinitionSize; layer2Index++)
                {
                    MapCollisionType collision = (MapCollisionType)this.Read();
                    layer2.Add(collision);
                }
                collisionList.Add(new MapCollisionSet(layer1, layer2));
            }
            return new DataTable<MapCollisionSet>(collisionList);
        }

        public DataTable<ManaDoor> ReadDoorTable()
        {
            this.Seek((int)Constants.Bank08.DoorTable);

            List<ManaDoor> doorList = new List<ManaDoor>((int)Constants.Bank08.DoorTableSize);
            for (int i = 0; i < Constants.Bank08.DoorTableSize; i++)
            {
                byte mapId = this.Read();
                byte xCoordinate = this.Read();
                byte b3 = this.Read();
                byte b4 = this.Read();

                ManaDoor door = new ManaDoor((ushort)i, mapId, xCoordinate, b3, b4);
                doorList.Add(door);
            }
            return new DataTable<ManaDoor>(doorList);
        }

        public DataTable<ManaPoint8> ReadFlammieFlightCoordinateTable()
        {
            this.Seek((int)Constants.Bank06.FlammieFlightCoordinateTableAddress);

            List<ManaPoint8> coordList = new List<ManaPoint8>((int)Constants.Bank06.FlammieFlightCoordinateTableSize);
            for (int i = 0; i < Constants.Bank06.FlammieFlightCoordinateTableSize; i++)
            {
                byte x = this.Read();
                byte y = this.Read();

                ManaPoint8 point = new ManaPoint8((byte)i, x, y);
                coordList.Add(point);
            }
            return new DataTable<ManaPoint8>(coordList);
        }

        public DataTable<DataTable<MapTrigger>> ReadMapTriggersTable()
        {
            this.Seek((int)Constants.Bank08.MapTriggerPointerTableAddress);

            List<int> pointerTable = new List<int>((int)Constants.Bank08.NumberOfMaps);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                pointerTable.Add(Constants.Bank08Offset | this.ReadUInt16());
            }

            List<DataTable<MapTrigger>> mapTriggerList = new List<DataTable<MapTrigger>>(Constants.Bank08.NumberOfMaps);
            for (int mapIndex = 0; mapIndex < pointerTable.Count; mapIndex++)
            {
                List<MapTrigger> mapTriggers = new List<MapTrigger>();

                int pointer = pointerTable[mapIndex];
                int nextPointer = (mapIndex + 1) < pointerTable.Count ? pointerTable[mapIndex + 1] : 0;

                // Fuck you Map 12A. For unimaginable reasons, despite using a system that requires sequential order,
                // there is a break in the trigger data between Map 12A and Map 12B. Map 12A points to an FF buffer at C8/4F7E.
                // The trigger data for Map 12B starts at C8/8A00. This means the trigger list for Map 12A is a whopping 7,489 (mostly invalid) entries.
                // In game Map 12A has no tiles with trigger collision so this doesn't matter, but the map is effectively barred from having triggers.
                // Another bullshit thing to account for.
                bool hasTriggers = (mapIndex != 0x012A) && (nextPointer != 0) && (pointer != nextPointer);
                if (hasTriggers)
                {
                    this.Seek(pointer);

                    int length = (nextPointer - pointer) / 2;
                    for (int triggerIndex = 0; triggerIndex < length; triggerIndex++)
                    {
                        mapTriggers.Add(new MapTrigger(this.ReadUInt16()));
                    }
                }
                mapTriggerList.Add(new DataTable<MapTrigger>(mapTriggers));
            }
            return new DataTable<DataTable<MapTrigger>>(mapTriggerList);
        }

        /// <summary>
        /// Reads the map palette set table. Each set contains seven 15-color palettes.
        /// </summary>
        /// <returns>A data table of map palette sets.</returns>
        public DataTable<DataTable<SpritePalette>> ReadMapPaletteSetTable()
        {
            this.Seek((int)Constants.Bank0C.MapPaletteSetTableAddress);

            List<DataTable<SpritePalette>> paletteSets = new List<DataTable<SpritePalette>>(Constants.Bank0C.MapPaletteSetTableSize);
            for (int setIndex = 0; setIndex < Constants.Bank0C.MapPaletteSetTableSize; setIndex++)
            {
                List<SpritePalette> paletteList = new List<SpritePalette>(Constants.Bank0C.MapPalettesPerSet);
                for (int paletteIndex = 0; paletteIndex < Constants.Bank0C.MapPalettesPerSet; paletteIndex++)
                {
                    List<Rgb555Color> colors = new List<Rgb555Color>(Constants.Bank0C.MapColorsPerPaletteInSet);
                    colors.Add(Rgb555Color.White);
                    for (int colorIndex = 0; colorIndex < Constants.Bank0C.MapColorsPerPaletteInSet; colorIndex++)
                    {
                        colors.Add(Rgb555Color.FromRgb(this.ReadUInt16(), Rgb555Format.BGR));
                    }
                    paletteList.Add(new SpritePalette((byte)paletteIndex, colors));
                }
                paletteSets.Add(new DataTable<SpritePalette>(paletteList));
            }
            return new DataTable<DataTable<SpritePalette>>(paletteSets);
        }

        /// <summary>
        /// Reads the 8x8 map tileset instruction sets.
        /// </summary>
        /// <param name="pointers">A datatable of pointers to the instruction sets.</param>
        /// <returns>A dictionary of 8x8 map instruction sets.</returns>
        public IReadOnlyDictionary<int, DataTable<Map8X8TileInstruction>> Read8x8MapInstructions(DataTable<ushort> pointers)
        {
            Dictionary<int, DataTable<Map8X8TileInstruction>> instructionDictionary = new Dictionary<int, DataTable<Map8X8TileInstruction>>();
            for (int i = 0; i < pointers.RowCount; i++)
            {
                List<Map8X8TileInstruction> instructions = new List<Map8X8TileInstruction>();
                if (((i + 1) < pointers.RowCount) && (pointers[i] != pointers[i + 1]))
                {
                    this.Seek(Constants.Bank0COffset | pointers[i]);

                    int bytesRead = 0;
                    int length = pointers[i + 1] - pointers[i];

                    while (bytesRead < length)
                    {
                        byte b1 = this.Read();
                        byte b2 = this.Read();
                        byte b3 = this.Read();

                        instructions.Add(new Map8X8TileInstruction(b1, b2, b3));
                        bytesRead += 3;
                    }
                }
                instructionDictionary.Add(i, new DataTable<Map8X8TileInstruction>(instructions));
            }

            return instructionDictionary;
        }

        /// <summary>
        /// Reads the data table of 8x8 map tilesets.
        /// </summary>
        /// <param name="baseAddresses">The table of base addresses for map graphics.</param>
        /// <param name="map8x8InstructionSets">The dictionary of map tile instructions to create the tilesets.</param>
        /// <returns>A data table of <see cref="Tileset"/>s containing 8x8 map tiles.</returns>
        public DataTable<Tileset> Read8x8MapTilesets(DataTable<uint> baseAddresses, IReadOnlyDictionary<int, DataTable<Map8X8TileInstruction>> map8x8InstructionSets)
        {
            List<Tileset> tilesets = new List<Tileset>(map8x8InstructionSets.Count);
            foreach (KeyValuePair<int, DataTable<Map8X8TileInstruction>> kvp in map8x8InstructionSets)
            {
                Tileset tileset = this.Read8x8MapTileset(kvp.Key, baseAddresses, kvp.Value);
                tilesets.Add(tileset);
            }
            return new DataTable<Tileset>(tilesets);
        }

        /// <summary>
        /// Reads the data table of 16x16 map tilesets.
        /// </summary>
        /// <param name="addressTable">The address table containing pointers to the 16x16 tileset instructions.</param>
        /// <returns>A data table of <see cref="Map16x16Tile"/> data tables that contain the 16x16 tileset construction data.</returns>
        public DataTable<DataTable<Map16x16Tile>> Read16x16MapTilesetTable(DataTable<uint> addressTable)
        {
            const int tilesToLoad = 0x0600; // 1536

            //int tilesetIndex = 0;
            List<DataTable<Map16x16Tile>> tilesets = new List<DataTable<Map16x16Tile>>((int)Constants.Bank0B.Map16x16TilesetPointerTableSize);
            foreach (uint address in addressTable)
            {
                //this.Seek((int)address);

                // Variables need to be outside the read loop since they can and will be reused for multiple tiles.
                bool vFlip = false;         // If true; Tile is flipped vertically.
                bool hFlip = false;         // If true; Tile is flipped horizontally.
                bool priority = false;      // The draw priority of the tile.
                byte paletteIndex = 0;      // Index of the palette to use for the 8x8 tile. (Valid: 0-6)
                ushort tileIndex = 0;       // Index of the 8x8 tile. (Valid: 0-???)

                byte bitsRead = 0;
                uint readAddress = address;
                List<Map16x16Tile> tileMap = new List<Map16x16Tile>();
                List<Map16x16TileQuadrant> tileList = new List<Map16x16TileQuadrant>();
                for (int i = 0; i < tilesToLoad; i++)
                {
                    byte controlByte = (byte)this.ReadBits(2, ref readAddress, ref bitsRead);
                    switch (controlByte)
                    {
                        case 0x00: // Repeat the previous tile.
                            break;
                        case 0x01: // 8-bit Increment Tile Index
                            if ((tileIndex & 0xFF) == 0xFF) { tileIndex -= 0xFF; }
                            else { tileIndex++; }
                            break;
                        case 0x02: // 8-bit Decrement Tile Index
                            if ((tileIndex & 0xFF) == 0x00) { tileIndex += 0xFF; }
                            else { tileIndex--; }
                            break;
                        case 0x03: // Read a full set of data in various ways.
                            bool doLargeRead = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                            if (doLargeRead)
                            {

                                bool doFullRead = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                if (doFullRead)
                                {
                                    // Read a full set of data from the bit stream.
                                    vFlip = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                    hFlip = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                    priority = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                    paletteIndex = (byte)(this.ReadBits(3, ref readAddress, ref bitsRead));
                                    tileIndex = this.ReadBits(10, ref readAddress, ref bitsRead);
                                }
                                else
                                {
                                    // Do a smaller read. If the flag is set then the next three bits indicate drawing flags.
                                    // If it isn't set, it's the palette index.
                                    // Afterwords read 6 more bits and add it to the current tile index to get the new one.
                                    bool loadType = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                    if (loadType)
                                    {
                                        vFlip = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                        hFlip = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                        priority = this.ReadBits(1, ref readAddress, ref bitsRead) != 0;
                                    }
                                    else
                                    {
                                        paletteIndex = (byte)this.ReadBits(3, ref readAddress, ref bitsRead);
                                    }

                                    ushort tileIndexPart = this.ReadBits(6, ref readAddress, ref bitsRead);
                                    short tileIndexAdvance = 0;
                                    if ((tileIndexPart & 0x20) != 0x00) { tileIndexPart |= 0xFFC0; }
                                    tileIndexAdvance = (short)tileIndexPart;
                                    tileIndex = (ushort)(tileIndex + tileIndexAdvance);
                                }
                            }
                            else
                            {
                                ushort tileIndexPart = this.ReadBits(7, ref readAddress, ref bitsRead);
                                short tileIndexAdvance = 0;
                                if ((tileIndexPart & 0x40) != 0x00) { tileIndexPart |= 0xFF80; }
                                tileIndexAdvance = (short)tileIndexPart;
                                tileIndex = (ushort)(tileIndex + tileIndexAdvance);
                            }
                            break;
                        default:
                            ThrowHelper.ThrowInvalidOperationException("Invalid control instruction.");
                            break;
                    }

                    tileList.Add(new Map16x16TileQuadrant(vFlip, hFlip, priority, paletteIndex, tileIndex));
                    if (tileList.Count == 4)
                    {
                        tileMap.Add(new Map16x16Tile(tileList));
                        tileList.Clear();
                    }
                }

                tilesets.Add(new DataTable<Map16x16Tile>(tileMap));
                //tilesetIndex++;
            }

            return new DataTable<DataTable<Map16x16Tile>>(tilesets);
        }

        /// <summary>
        /// Reads the map headers and sprite object tables.
        /// </summary>
        /// <returns>A data table of the map headers.</returns>
        public DataTable<MapHeader> ReadMapHeaderTable()
        {
            List<MapHeader> headers = new List<MapHeader>(Constants.Bank08.NumberOfMaps);
            ByteBitCounter mapEventCounter = new ByteBitCounter("MapEventOptions");
            ByteBitCounter mapSpecialCounter = new ByteBitCounter("MapSpecialItemsOptions");
            this.Seek(Constants.Bank08.MapHeaderNPCPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                ushort offset = this.ReadUInt16();
                ushort nextOffset = this.PeekUInt16();
                int pointerTablePosition = this.Position;

                if (offset == nextOffset)
                {
                    // If the current offset is equal to the next one then the pointer is effectively null.
                    // No data exists for this index. Attempting to load them in game will hang due to pointer math being used to determine length.
                    headers.Add(MapHeader.InvalidHeader);
                }
                else if (i == 0x1FF)
                {
                    // 1FF points to a null buffer.
                    headers.Add(MapHeader.InvalidHeader);
                }
                else
                {
                    // This index is valid.
                    int currentPointer = Constants.Bank08Offset | offset;
                    int nextPointer = Constants.Bank08Offset | nextOffset;
                    this.Seek(currentPointer);

                    // First 8 bytes are header data for the map, containing various options for how the map is loaded and what can be done on it.
                    // Most of these bytes have compression and/or encoding.
                    byte tileset8Id = this.Read();
                    byte paletteId = this.Read();
                    byte tileset16Id = this.Read();
                    byte settings = this.Read();
                    byte itemsAllowed = this.Read();
                    byte displaySettingsIndex = this.Read();
                    byte unknownValue = this.Read();
                    byte npcPaletteId = this.Read();

                    // After the header comes the sprite listing for the map. There can be as few as zero entries and the maximum is uncapped.
                    // Each row of sprite data is 8 bytes long. As usual, most of these bytes have compression and/or encoding.
                    List<MapSpriteObject> spriteObjects = new List<MapSpriteObject>();
                    while (this.Position < nextPointer)
                    {
                        EventFlag flag = (EventFlag)this.Read();
                        byte range = this.Read();
                        byte xCoord = this.Read();
                        byte yCoord = this.Read();
                        byte direction = this.Read();
                        byte spriteId = this.Read();
                        byte eventIdLow = this.Read();
                        byte eventIdHigh = this.Read();

                        MapSpriteObject mapSpriteObject = new MapSpriteObject(flag, range, xCoord, yCoord, direction, spriteId, eventIdLow, eventIdHigh);
                        spriteObjects.Add(mapSpriteObject);
                    }

                    // Table read completed.
                    mapEventCounter.RecordBits(settings);
                    mapSpecialCounter.RecordBits(itemsAllowed);
                    MapHeader header = new MapHeader(tileset8Id, paletteId, tileset16Id, settings, itemsAllowed, displaySettingsIndex, unknownValue, npcPaletteId, new DataTable<MapSpriteObject>(spriteObjects), true);
                    headers.Add(header);
                }

                // Move to the next position in the table.
                this.Seek(pointerTablePosition);
            }

            //LoggerEngine.Logger.LogDebug(LogComponent.Debug, mapEventCounter.ToString());
            //LoggerEngine.Logger.LogDebug(LogComponent.Debug, mapSpecialCounter.ToString());
            return new DataTable<MapHeader>(headers);
        }

        /// <summary>
        /// Reads the map object table. This table contains the layer objects for the map.
        /// </summary>
        /// <returns>A data table of map object tables.</returns>
        public DataTable<MapObjectTable> ReadMapObjectTable()
        {
            List<MapObjectTable> mapObjectTables = new List<MapObjectTable>(Constants.Bank08.NumberOfMaps);
            this.Seek(Constants.Bank08.MapLayerDefinitionPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                ushort offset = this.ReadUInt16();
                ushort nextOffset = this.PeekUInt16();
                int pointerTablePosition = this.Position;

                if (offset == nextOffset)
                {
                    // If the current offset is equal to the next one then the pointer is effectively null.
                    // No data exists for this index. Attempting to load them in game will hang due to pointer math being used to determine length.
                    mapObjectTables.Add(MapObjectTable.InvalidObjectTable);
                }
                else if (i == 0xF3 || i == 0x1FF)
                {
                    // F3 points to some localization garbage that is a mix of maps F1 and F2 along with some unknown bits.
                    // 1FF points to a null buffer.
                    // Neither can be loaded successfully.
                    mapObjectTables.Add(MapObjectTable.InvalidObjectTable);
                }
                else
                {
                    // This index is valid.
                    // Data has to be read from the location of the current pointer ntil we reach the pointer for the next index.
                    List<MapObject> layer1 = new List<MapObject>();
                    List<MapObject> layer2 = new List<MapObject>();
                    List<MapObject> activeLayer = layer1;

                    int currentPointer = Constants.Bank08Offset | offset;
                    int nextPointer = Constants.Bank08Offset | nextOffset;
                    this.Seek(currentPointer);
                    bool map13DAdjustment = false;
                    while (this.Position < nextPointer)
                    {
                        // Map Object Data is broken into layers.
                        // The start of Layer 1 objects is marked by the byte FE.
                        // The start of Layer 2 objects is marked by the byte FF.
                        // Map 13D (Kilroy's Arena) is broken and is missing the FE marker for its Layer 1 objects.
                        // This ultimately doesn't matter in game because it only cares about the FF marker, but makes parsing more annoying.
                        byte layer = this.Read();
                        //if (i == 0x13D) { System.Diagnostics.Debugger.Break(); }
                        if (layer == 0xFE)// || (i == 0x13D && (this.Position - 1) == currentPointer))
                        {
                            // If the byte we just read was FE then we are on Layer 1.
                            activeLayer = layer1;
                        }
                        else if (i == 0x13D && (this.Position - 1) == currentPointer)
                        {
                            // Fucking robots. First Death Machine takes his chainsaw and goes home.
                            // Then Kilroy makes parsing needlessly difficult with data errors.
                            // Since this map is missing its FE marker we are one byte ahead of where we should be.
                            // So during our next pass though the loop we need to make an additional adjustment to the position to get back on track.
                            activeLayer = layer1;
                            map13DAdjustment = true;
                        }
                        else if (layer == 0xFF)
                        {
                            // If the byte we just read was FF then we are on Layer 2.
                            activeLayer = layer2;
                        }
                        else
                        {
                            // Read byte was neither FE or FF, so we are on an object row.
                            // Back up the read position so the first byte can be read again.
                            this.Position--;
                            if (map13DAdjustment)
                            {
                                this.Position--;
                                map13DAdjustment = false;
                            }

                            // Each map object is 5 bytes long.
                            // Most of these bytes are compressed and/or encoded.
                            EventFlag flag = (EventFlag)this.Read();
                            byte range = this.Read();
                            byte id = this.Read();
                            byte xCoord = this.Read();
                            byte yCoord = this.Read();

                            MapObject mapObject = new MapObject(flag, range, id, xCoord, yCoord);
                            activeLayer.Add(mapObject);
                        }
                    }
                    MapObject background = MapObject.Empty;
                    MapObject foreground = MapObject.Empty;
                    if (layer1.Count > 0)
                    {
                        background = layer1[0];
                        layer1.RemoveAt(0);
                    }
                    if (layer2.Count > 0)
                    {
                        foreground = layer2[0];
                        layer2.RemoveAt(0);
                    }

                    // Object table read completed.
                    MapObjectTable mapObjectTable = new MapObjectTable(background, foreground, new DataTable<MapObject>(layer1), new DataTable<MapObject>(layer2), true);
                    mapObjectTables.Add(mapObjectTable);
                }

                // Move to the next position in the table.
                this.Seek(pointerTablePosition);
            }

            return new DataTable<MapObjectTable>(mapObjectTables);
        }

        public DataTable<MapPiece> ReadMapPieces(Func<int, int> getAddressFunc)
        {
            List<MapPiece> mapPieces = new List<MapPiece>((int)Constants.Bank0D.MapPiecePointerTableSize);

            // A map piece index of zero is invalid and unused, so adding an invalid piece at that index to make my life easier.
            mapPieces.Add(MapPiece.Empty);

            for (ushort i = 1; i <= Constants.Bank0D.MapPiecePointerTableSize; i++)
            {
                if (ManaMetadata.InvalidMapPieceIndexes.Contains(i))
                {
                    mapPieces.Add(MapPiece.Empty);
                    continue;
                }
                int address = getAddressFunc(i);
                MapPiece piece = this.decompressor.Decompress(i, address);
                mapPieces.Add(piece);
            }

            return new DataTable<MapPiece>(mapPieces);
        }

        public MapPiece ReadMapPiece(ushort index, int address)
        {
            return this.decompressor.Decompress(index, address);
        }

        private ushort ReadBits(byte bitsToread, ref uint address, ref byte bitsRead)
        {
            ushort readBits = 0;
            for (int i = 0; i < bitsToread; i++)
            {
                this.Seek((int)address);
                readBits <<= 0x01;

                byte mask = (byte)(0x01 << (0x07 - bitsRead));
                byte peekByte = (byte)(this.Peek() & mask);

                if (peekByte > 0)
                {
                    readBits |= 0x01;
                }

                bitsRead++;
                if (bitsRead == 8)
                {
                    bitsRead = 0;
                    address++;
                }
            }

            return readBits;
        }

        private Tileset Read8x8MapTileset(int tilesetIndex, DataTable<uint> baseAddresses, DataTable<Map8X8TileInstruction> instructions)
        {
            List<GraphicTile4Bpp> tileList = new List<GraphicTile4Bpp>(256);
            foreach (Map8X8TileInstruction instruction in instructions)
            {
                int index = instruction.BaseGraphicsAddressIndex;
                uint baseAddress = baseAddresses[index];

                switch (index)
                {
                    case 00:
                    //tileList.Add(GraphicTile4Bpp.Empty);
                    //break;
                    case 01:
                        this.Read3BppTiles(baseAddress, instruction, ref tileList);
                        break;
                    case 02:
                    case 03:
                        this.Read4BppTiles(tilesetIndex, baseAddress, instruction, ref tileList);
                        break;
                }
            }

            this.ReadAnimationTiles(tilesetIndex, tileList);
            return new Tileset(tilesetIndex, tileList, TileType.FourBitsPerPixel);
        }

        private void Read3BppTiles(uint baseAddress, Map8X8TileInstruction instruction, ref List<GraphicTile4Bpp> tileList)
        {
            //uint calcOffset = ((instruction.AddressOffset2 & 0x1FFF) * 8) + ((instruction.AddressOffset2 & 0x1FFF) * 16);
            uint address = baseAddress + instruction.GetAddressOffset(TileType.ThreeBitsPerPixel);
            this.Seek((int)address);

            Span<byte> tileBuffer = stackalloc byte[32];
            byte fillByte = (byte)(((instruction.InstructionControl & 0x20) == 0) ? 0x00 : 0xFF);
            for (int tileIndex = 0; tileIndex < instruction.InstructionSize + 1; tileIndex++)
            {
                tileBuffer.Fill(0);
                for (int byteIndex = 0; byteIndex < 16; byteIndex++)
                {
                    tileBuffer[byteIndex] = this.Read();
                }
                for (int byteIndex = 16; byteIndex < 32; byteIndex += 2)
                {
                    tileBuffer[byteIndex + 1] = fillByte;
                    tileBuffer[byteIndex] = this.Read();
                }
                //"0,0,0,0,0,0,0,3,1,1,1,1,4,12,4,29,0,0,0,0,0,0,0,0,0,0,0,0,3,0,2,0"
                //"0,0,0,0,0,0,0,3,1,1,1,1,4,12,4,29,0,0,0,0,0,0,0,0,0,0,0,0,3,0,2,0"
                string x = string.Join(",", tileBuffer.ToArray());
                tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
            }
        }

        private void Read4BppTiles(int tilesetIndex, uint baseAddress, Map8X8TileInstruction instruction, ref List<GraphicTile4Bpp> tileList)
        {
            uint address = baseAddress + instruction.GetAddressOffset(TileType.FourBitsPerPixel);
            this.Seek((int)address);

            Span<byte> tileBuffer = stackalloc byte[32];
            for (int tileIndex = 0; tileIndex < instruction.InstructionSize + 1; tileIndex++)
            {
                tileBuffer.Fill(0);
                for (int byteIndex = 0; byteIndex < 32; byteIndex++)
                {
                    if (this.IsEndOfFile)
                    {
                        // Why do we need to do this?
                        // Because several tilesets contain invalid instructions that read past the end of the ROM.
                        // The Matango Exterior tileset even does it twice.
                        // Due to mirroring the game will wrap around and read from the top of Bank 00.
                        // The data there are jump tables so the tiles are garbage.
                        // The invalid tiles are never referenced in game so the over-read ultimately doesn't matter.
                        // Replcate the behavior here, wrap around and create junk tiles.
                        this.Seek(0);

                        string message = $"8x8 Map Tileset '{ManaMetadata.Map8x8TilesetMetadata[tilesetIndex].Name} ({tilesetIndex:X2})' read past the end of the ROM file.";
                        LoggerEngine.Logger.LogDebug(LogComponent.RomReader, message);
                    }
                    tileBuffer[byteIndex] = this.Read();
                }
                tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
            }
        }

        private void ReadAnimationTiles(int tilesetIndex, List<GraphicTile4Bpp> tileList)
        {
            if ((tilesetIndex + 1) < Constants.Bank08.MapAnimatedTilesIndexTableSize)
            {
                this.Seek(Constants.Bank08.MapAnimatedTilesIndexTable + tilesetIndex);
                byte animationIndex = this.Read();
                byte nextAnimationIndex = this.Peek();

                int length = (nextAnimationIndex - animationIndex) * 2;
                Span<byte> tileBuffer = stackalloc byte[32];
                for (int i = 0; i < length; i++)
                {
                    this.Seek(Constants.Bank08.MapAnimatedTilesEncodedPointerTable + (animationIndex * 2) + i);
                    byte encodedOffset = this.Read();
                    ushort decodedOffset = (ushort)((((encodedOffset & 0x3F) << 1) | 0x80) << 8);
                    int address = Constants.Bank1COffset | decodedOffset;

                    this.Seek(address);
                    for (int tileIndex = 0; tileIndex < 4; tileIndex++)
                    {
                        tileBuffer.Fill(0);
                        for (int byteIndex = 0; byteIndex < 32; byteIndex++)
                        {
                            tileBuffer[byteIndex] = this.Read();
                        }
                        tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
                    }
                }
            }
        }
    }
}