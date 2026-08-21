using System.Collections.Generic;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Maps
{
    /// <summary>
    /// Represents a container of map related data in a Secret of Mana ROM file.
    /// </summary>
    public sealed class MapContext
    {
        private DataTable<uint> BaseGraphicsAddressTable = DataTable<uint>.Empty;
        private DataTable<ushort> Map8x8TilesetAddressTable = DataTable<ushort>.Empty;
        private DataTable<uint> Map16x16BaseAddressTable = DataTable<uint>.Empty;
        private DataTable<ushort> MapPieceIndexMaximumTable = DataTable<ushort>.Empty;
        private DataTable<ushort> MapPieceAddressTable = DataTable<ushort>.Empty;
        private DataTable<MapPiece> MapPieceTable = DataTable<MapPiece>.Empty;
        private IReadOnlyDictionary<int, DataTable<Map8X8TileInstruction>> Map8x8InstructionSets = new Dictionary<int, DataTable<Map8X8TileInstruction>>();

        /// <summary>
        /// Gets the data table of map headers and sprite objects.
        /// </summary>
        public DataTable<MapHeader> MapHeaderTable { get; private set; } = DataTable<MapHeader>.Empty;

        /// <summary>
        /// Gets the data table of map object tables.
        /// </summary>
        public DataTable<MapObjectTable> MapObjectTable { get; private set; } = DataTable<MapObjectTable>.Empty;

        public DataTable<MapDisplaySettings> DisplaySettingsTable { get; private set; } = DataTable<MapDisplaySettings>.Empty;

        public DataTable<ManaDoor> DoorTable { get; private set; } = DataTable<ManaDoor>.Empty;

        public DataTable<MapCollision> MapCollisionDefinitionTable { get; private set; } = DataTable<MapCollision>.Empty;

        public DataTable<MapCollisionSet> MapTilesetCollisionTable { get; private set; } = DataTable<MapCollisionSet>.Empty;

        public DataTable<DataTable<MapTrigger>> MapTriggersTable { get; private set; } = DataTable<DataTable<MapTrigger>>.Empty;

        public DataTable<ManaPoint8> FlammieFlightCoordinateTable { get; private set; } = DataTable<ManaPoint8>.Empty;

        /// <summary>
        /// Gets the data table of palette sets.
        /// </summary>
        public DataTable<DataTable<SpritePalette>> PaletteSetTable { get; private set; } = DataTable<DataTable<SpritePalette>>.Empty;
        
        /// <summary>
        /// Gets the data table of 8x8 map tilesets.
        /// </summary>
        public DataTable<Tileset> Map8x8TilesetTable { get; private set; } = DataTable<Tileset>.Empty;

        /// <summary>
        /// Gets the data table of 16x16 map tileset instructions.
        /// </summary>
        public DataTable<DataTable<Map16x16Tile>> Map16x16TilesetTable { get; private set; } = DataTable<DataTable<Map16x16Tile>>.Empty;

        /// <summary>
        /// Initializes the context by reading map data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            MapRomReader reader = RomReaderFactory.GetRomReader<MapRomReader>();

            this.BaseGraphicsAddressTable = reader.ReadMapGraphicalBaseAddressTable();
            this.Map8x8TilesetAddressTable = reader.Read8x8MapTilesetAddressTable();
            this.Map16x16BaseAddressTable = reader.Read16x16MapTilesetAddressTable();
            this.MapPieceIndexMaximumTable = reader.ReadMapPieceIndexMaximumTable();
            this.MapPieceAddressTable = reader.ReadMapPieceAddressTable();
            this.MapPieceTable = reader.ReadMapPieces(this.GetMapPieceAddress);

            this.MapHeaderTable = reader.ReadMapHeaderTable();
            this.MapObjectTable = reader.ReadMapObjectTable();
            this.PaletteSetTable = reader.ReadMapPaletteSetTable();
            this.DisplaySettingsTable = reader.ReadDisplaySettingsTable();
            this.DoorTable = reader.ReadDoorTable();
            this.MapCollisionDefinitionTable = reader.ReadMapCollisionDefinitionTable();
            this.MapTilesetCollisionTable = reader.ReadMapTilesetCollisionTable();
            this.MapTriggersTable = reader.ReadMapTriggersTable();
            this.FlammieFlightCoordinateTable = reader.ReadFlammieFlightCoordinateTable();

            this.Map8x8InstructionSets = reader.Read8x8MapInstructions(this.Map8x8TilesetAddressTable);
            this.Map8x8TilesetTable = reader.Read8x8MapTilesets(this.BaseGraphicsAddressTable, this.Map8x8InstructionSets);
            this.Map16x16TilesetTable = reader.Read16x16MapTilesetTable(this.Map16x16BaseAddressTable);
        }

        /// <summary>
        /// Calculates the full address of a map piece.
        /// </summary>
        /// <param name="mapPieceIndex">The index of the map piece.</param>
        /// <returns>The 24-bit address of the map piece in the ROM file.</returns>
        private int GetMapPieceAddress(int mapPieceIndex)
        {
            if (mapPieceIndex < this.MapPieceIndexMaximumTable[0]) { return Constants.Bank0DOffset | this.MapPieceAddressTable[mapPieceIndex - 1]; }
            if (mapPieceIndex < this.MapPieceIndexMaximumTable[1]) { return Constants.Bank0EOffset | this.MapPieceAddressTable[mapPieceIndex - 1]; }
            if (mapPieceIndex < this.MapPieceIndexMaximumTable[2]) { return Constants.Bank0FOffset | this.MapPieceAddressTable[mapPieceIndex - 1]; }

            return Constants.Bank0COffset | this.MapPieceAddressTable[mapPieceIndex - 1];
        }

        public ManaMap CreateMap(int mapId, bool allowInvalidHeader = false)
        {
            // Collect the metric fuck-ton of data required to actually build a map.
            MapHeader header = this.MapHeaderTable[mapId];
            MapObjectTable objectTable = this.MapObjectTable[mapId];
            if ((header.IsValid || allowInvalidHeader) && objectTable.IsValid)
            {
                Tileset tileset8x8 = this.Map8x8TilesetTable[header.Tileset8x8Index];
                DataTable<Map16x16Tile> tileset16x16 = this.Map16x16TilesetTable[header.Tileset16x16Index];
                MapCollisionSet collisionSet = this.MapTilesetCollisionTable[header.Tileset16x16Index];
                MapDisplaySettings displaySettings = this.DisplaySettingsTable[header.DisplaySettingsIndex];
                DataTable<SpritePalette> paletteSet = this.PaletteSetTable[header.PaletteSetIndex];
                DataTable<MapTrigger> triggerTable = this.MapTriggersTable[mapId];

                MapPiece background = this.MapPieceTable[objectTable.Layer1Background.Index];
                DataTable<MapPiece> layer1 = GetLayerPieces(objectTable.Layer1);

                MapPiece foreground = MapPiece.Empty;
                DataTable<MapPiece> layer2 = DataTable<MapPiece>.Empty;
                if (objectTable.HasLayer2)
                {
                    foreground = this.MapPieceTable[objectTable.Layer2Background.Index];
                    layer2 = GetLayerPieces(objectTable.Layer2);
                }

                return new ManaMap(header, displaySettings, objectTable, background, foreground, layer1, layer2, tileset8x8, tileset16x16, collisionSet, triggerTable, paletteSet);
            }

            return new ManaMap(header, objectTable);

            DataTable<MapPiece> GetLayerPieces(DataTable<MapObject> mapObjects)
            {
                List<MapPiece> pieces = new List<MapPiece>(mapObjects.RowCount);
                foreach (MapObject mapObject in mapObjects)
                {
                    pieces.Add(this.MapPieceTable[mapObject.Index]);
                }
                return new DataTable<MapPiece>(pieces);
            }
        }
    }
}
/*
        public ManaMap GetMap2(int mapId, bool allowInvalidHeader = false)
        {
            MapHeader header = this.MapHeaderTable[mapId];
            MapObjectTable objectTable = this.MapObjectTable[mapId];
            if ((header.IsValid || allowInvalidHeader) && objectTable.IsValid)
            {
                MapDisplaySettings displaySettings = this.DisplaySettingsTable[header.DisplaySettingsIndex];
                Tileset tileset8x8 = this.Map8x8TilesetTable[header.Tileset8x8Index];
                DataTable<Map16x16Tile> tileset16x16 = this.Map16x16TilesetTable[header.Tileset16x16Index];
                DataTable<SpritePalette> paletteSet = this.PaletteSetTable[header.PaletteSetIndex & 0x7F];

                MapRomReader reader = RomReaderFactory.GetRomReader<MapRomReader>();
                MapPiece background = reader.ReadMapPiece(objectTable.Layer1Background.Id, this.GetMapPieceAddress(objectTable.Layer1Background.Id));
                DataTable<MapPiece> layer1 = LoadLayerPieces(reader, objectTable.Layer1);

                MapPiece foreground = MapPiece.Empty;
                DataTable<MapPiece> layer2 = DataTable<MapPiece>.Empty;

                if (objectTable.HasLayer2)
                {
                    foreground = reader.ReadMapPiece(objectTable.Layer2Background.Id, this.GetMapPieceAddress(objectTable.Layer2Background.Id));
                    layer2 = LoadLayerPieces(reader, objectTable.Layer2);
                }

                return new ManaMap(header, displaySettings, objectTable, background, foreground, layer1, layer2, tileset8x8, tileset16x16, paletteSet);
            }

            return new ManaMap(header, objectTable);

            DataTable<MapPiece> LoadLayerPieces(MapRomReader reader, DataTable<MapObject> mapObjects)
            {
                List<MapPiece> pieces = new List<MapPiece>(mapObjects.RowCount);
                foreach (MapObject mapObject in mapObjects)
                {
                    pieces.Add(reader.ReadMapPiece(mapObject.Id, this.GetMapPieceAddress(mapObject.Id)));
                }
                return new DataTable<MapPiece>(pieces);
            }
        }
 */