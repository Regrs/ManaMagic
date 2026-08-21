using System;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed class ManaMap
    {
        public static ManaMap Empty { get; } = new ManaMap(MapHeader.InvalidHeader, MapObjectTable.InvalidObjectTable);

        public MapHeader Header { get; }
        public MapObjectTable ObjectTable { get; }
        public MapDisplaySettings DisplaySettings { get; } = MapDisplaySettings.Empty;
        public DataTable<SpritePalette> PaletteSet { get; } = DataTable<SpritePalette>.Empty;
        public DataTable<MapTrigger> Triggers { get; } = DataTable<MapTrigger>.Empty;

        public Tileset Tileset8x8 { get; } = Tileset.Empty;
        public DataTable<Map16x16Tile> Tileset16x16 { get; } = DataTable<Map16x16Tile>.Empty;
        public MapCollisionSet TilesetCollision { get; } = MapCollisionSet.Empty;

        public MapPiece Layer1Background { get; } = MapPiece.Empty;
        public MapPiece Layer2Background { get; } = MapPiece.Empty;
        public DataTable<MapPiece> Layer1 { get; } = DataTable<MapPiece>.Empty;
        public DataTable<MapPiece> Layer2 { get; } = DataTable<MapPiece>.Empty;

        public bool IsValid { get { return this.Header.IsValid && this.ObjectTable.IsValid; } }

        public ManaMap(MapHeader header, MapObjectTable objectTable)
        {
            this.Header = header;
            this.ObjectTable = objectTable;
        }

        public ManaMap(MapHeader header,
                       MapDisplaySettings displaySettings,
                       MapObjectTable objectTable,
                       MapPiece background,
                       MapPiece foreground,
                       DataTable<MapPiece> layer1,
                       DataTable<MapPiece> layer2,
                       Tileset tileset8x8,
                       DataTable<Map16x16Tile> tileset16x16,
                       MapCollisionSet tilesetCollision,
                       DataTable<MapTrigger> triggers,
                       DataTable<SpritePalette> paletteSet)
        {
            this.Header = header;
            this.ObjectTable = objectTable;
            this.DisplaySettings = displaySettings;
            this.PaletteSet = paletteSet;
            this.Triggers = triggers;
            this.Layer1Background = background;
            this.Layer2Background = foreground;
            this.Layer1 = layer1;
            this.Layer2 = layer2;
            this.Tileset8x8 = tileset8x8;
            this.Tileset16x16 = tileset16x16;
            this.TilesetCollision = tilesetCollision;
        }

        public SuperNintendoGraphics DrawCompleteMap()
        {
            byte maxWidth = Math.Max(this.Layer1Background.Width, this.Layer2Background.Width);
            byte maxHeight = Math.Max(this.Layer1Background.Height, this.Layer2Background.Height);

            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(this.PaletteSet, maxWidth * 16, maxHeight * 16);
            graphics.Fill(this.DisplaySettings.BackgroundColor);

            using SuperNintendoGraphics layer1BG1 = this.DrawMapPiece(this.Layer1Background, this.PaletteSet, false, false);
            for (int i = 0; i < this.ObjectTable.Layer1.RowCount; i++)
            {
                MapObject mapObject = this.ObjectTable.Layer1[i];
                MapPiece mapPiece = this.Layer1[i];

                using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, this.PaletteSet, false, false);
                layer1BG1.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
            }

            using SuperNintendoGraphics layer1BG2 = this.DrawMapPiece(this.Layer1Background, this.PaletteSet, false, true);
            for (int i = 0; i < this.ObjectTable.Layer1.RowCount; i++)
            {
                MapObject mapObject = this.ObjectTable.Layer1[i];
                MapPiece mapPiece = this.Layer1[i];

                using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, this.PaletteSet, false, true);
                layer1BG2.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
            }

            using SuperNintendoGraphics layer2BG1 = this.DrawMapPiece(this.Layer2Background, this.PaletteSet, maxWidth, maxHeight, true, false);
            for (int i = 0; i < this.ObjectTable.Layer2.RowCount; i++)
            {
                MapObject mapObject = this.ObjectTable.Layer2[i];
                MapPiece mapPiece = this.Layer2[i];

                using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, this.PaletteSet, true, false);
                layer2BG1.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
            }

            using SuperNintendoGraphics layer2BG2 = this.DrawMapPiece(this.Layer2Background, this.PaletteSet, true, true);
            for (int i = 0; i < this.ObjectTable.Layer2.RowCount; i++)
            {
                MapObject mapObject = this.ObjectTable.Layer2[i];
                MapPiece mapPiece = this.Layer2[i];

                using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, this.PaletteSet, true, true);
                layer2BG2.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
            }

            graphics.Merge(layer2BG1);
            graphics.Merge(layer1BG1);
            graphics.Merge(layer2BG2);
            graphics.Merge(layer1BG2);

            return graphics;
        }

        public SuperNintendoGraphics DrawMap(MapDrawingOptions options)
        {
            return this.DrawMap(options, this.PaletteSet);
        }

        public SuperNintendoGraphics DrawMap(MapDrawingOptions options, DataTable<SpritePalette> paletteSet)
        {
            byte maxWidth = Math.Max(this.Layer1Background.Width, this.Layer2Background.Width);
            byte maxHeight = Math.Max(this.Layer1Background.Height, this.Layer2Background.Height);

            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, maxWidth * 16, maxHeight * 16);

            if (options.DrawBackground)
            {
                graphics.Fill(this.DisplaySettings.BackgroundColor);
            }

            // BG1 tiles with priority 1
            // BG2 tiles with priority 1
            // BG1 tiles with priority 0
            // BG2 tiles with priority 0

            if (this.ObjectTable.HasLayer2 && options.DrawLayer2Background01)
            {
                using SuperNintendoGraphics layer2BG1 = this.DrawMapPiece(this.Layer2Background, paletteSet, maxWidth, maxHeight, true, false);
                for (int i = 0; i < this.ObjectTable.Layer2.RowCount; i++)
                {
                    MapObject mapObject = this.ObjectTable.Layer2[i];
                    MapPiece mapPiece = this.Layer2[i];

                    using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, paletteSet, true, false);
                    layer2BG1.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
                }

                if (this.DisplaySettings.ColorMath.HasFlag(CGADSUB.EnableBackground02)) { graphics.Merge(layer2BG1, this.DisplaySettings.ColorMath); }
                else { graphics.Merge(layer2BG1); }
                //graphics.Merge(layer2BG1);
            }

            if (options.DrawLayer1Background01)
            {
                using SuperNintendoGraphics layer1BG1 = this.DrawMapPiece(this.Layer1Background, paletteSet, maxWidth, maxHeight, false, false);
                for (int i = 0; i < this.ObjectTable.Layer1.RowCount; i++)
                {
                    MapObject mapObject = this.ObjectTable.Layer1[i];
                    MapPiece mapPiece = this.Layer1[i];

                    using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, paletteSet, false, false);
                    layer1BG1.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
                }
                if (this.DisplaySettings.ColorMath.HasFlag(CGADSUB.EnableBackground01)) { graphics.Merge(layer1BG1, this.DisplaySettings.ColorMath); }
                else { graphics.Merge(layer1BG1); }
                //graphics.Merge(layer1BG1);
            }

            if (this.ObjectTable.HasLayer2 && options.DrawLayer2Background02)
            {
                using SuperNintendoGraphics layer2BG2 = this.DrawMapPiece(this.Layer2Background, paletteSet, maxWidth, maxHeight, true, true);
                for (int i = 0; i < this.ObjectTable.Layer2.RowCount; i++)
                {
                    MapObject mapObject = this.ObjectTable.Layer2[i];
                    MapPiece mapPiece = this.Layer2[i];

                    using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, paletteSet, true, true);
                    layer2BG2.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
                }
                if (this.DisplaySettings.ColorMath.HasFlag(CGADSUB.EnableBackground02)) { graphics.Merge(layer2BG2, this.DisplaySettings.ColorMath); }
                else { graphics.Merge(layer2BG2); }
                //graphics.Merge(layer2BG2);
            }

            if (options.DrawLayer1Background02)
            {
                using SuperNintendoGraphics layer1BG2 = this.DrawMapPiece(this.Layer1Background, paletteSet, maxWidth, maxHeight, false, true);
                for (int i = 0; i < this.ObjectTable.Layer1.RowCount; i++)
                {
                    MapObject mapObject = this.ObjectTable.Layer1[i];
                    MapPiece mapPiece = this.Layer1[i];

                    using SuperNintendoGraphics piece = this.DrawMapPiece(mapPiece, paletteSet, false, true);
                    layer1BG2.Merge(piece, mapObject.Location.X * 8, mapObject.Location.Y * 8);
                }
                if (this.DisplaySettings.ColorMath.HasFlag(CGADSUB.EnableBackground01)) { graphics.Merge(layer1BG2, this.DisplaySettings.ColorMath); }
                else { graphics.Merge(layer1BG2); }
                //graphics.Merge(layer1BG2);
            }

            return graphics;
        }

        private SuperNintendoGraphics DrawMapPiece(MapPiece piece, DataTable<SpritePalette> paletteSet, bool asLayer2, bool priority)
        {
            return this.DrawMapPiece(piece, paletteSet, piece.Width, piece.Height, asLayer2, priority);
        }

        private SuperNintendoGraphics DrawMapPiece(MapPiece piece, DataTable<SpritePalette> paletteSet, byte width, byte height, bool asLayer2, bool priority)
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, width * 16, height * 16);

            int columnIndex = 0;
            int rowIndex = 0;

            int offset = !asLayer2 ? 0 : 192;
            if (!asLayer2 && this.Header.LayerLoadMode.HasFlag(LayerLoadMode.LoadLayer1AsLayer2))
            {
                offset += 192;
            }
            if (asLayer2 && !this.Header.LayerLoadMode.HasFlag(LayerLoadMode.LoadLayer2AsLayer2))
            {
                offset -= 192;
            }

            for (int i = 0; i < piece.TileIdTable.RowCount; i++)
            {
                Map16x16Tile tile16x16 = this.Tileset16x16[piece.TileIdTable[i] + offset];
                Map16x16TileQuadrant quadrant1 = tile16x16.Quadrants[0];
                Map16x16TileQuadrant quadrant2 = tile16x16.Quadrants[1];
                Map16x16TileQuadrant quadrant3 = tile16x16.Quadrants[2];
                Map16x16TileQuadrant quadrant4 = tile16x16.Quadrants[3];

                GraphicTile tile1 = quadrant1.Priority == priority ? this.Tileset8x8[quadrant1.TileIndex] : GraphicTile4Bpp.Empty;
                GraphicTile tile2 = quadrant2.Priority == priority ? this.Tileset8x8[quadrant2.TileIndex] : GraphicTile4Bpp.Empty;
                GraphicTile tile3 = quadrant3.Priority == priority ? this.Tileset8x8[quadrant3.TileIndex] : GraphicTile4Bpp.Empty;
                GraphicTile tile4 = quadrant4.Priority == priority ? this.Tileset8x8[quadrant4.TileIndex] : GraphicTile4Bpp.Empty;

                graphics.DrawTile(tile1, (columnIndex * 16), (rowIndex * 16), quadrant1.FlipType, quadrant1.ModifiedPaletteIndex);
                graphics.DrawTile(tile2, (columnIndex * 16) + 8, (rowIndex * 16), quadrant2.FlipType, quadrant2.ModifiedPaletteIndex);
                graphics.DrawTile(tile3, (columnIndex * 16), (rowIndex * 16) + 8, quadrant3.FlipType, quadrant3.ModifiedPaletteIndex);
                graphics.DrawTile(tile4, (columnIndex * 16) + 8, (rowIndex * 16) + 8, quadrant4.FlipType, quadrant4.ModifiedPaletteIndex);

                columnIndex++;
                if (columnIndex == piece.Width)
                {
                    // Wrap around to the start of the next row if we have reached the end of the current row.
                    columnIndex = 0;
                    rowIndex++;
                }
            }
            return graphics;
        }
    }

    public readonly struct MapDrawingOptions
    {
        public bool DrawBackground { get; }
        public bool DrawLayer1Background01 { get; }
        public bool DrawLayer1Background02 { get; }
        public bool DrawLayer2Background01 { get; }
        public bool DrawLayer2Background02 { get; }

        public MapDrawingOptions(bool drawBackground, bool drawLayer1BG01, bool drawLayer1BG02, bool drawLayer2BG01, bool drawLayer2BG02)
        {
            this.DrawBackground = drawBackground;
            this.DrawLayer1Background01 = drawLayer1BG01;
            this.DrawLayer1Background02 = drawLayer1BG02;
            this.DrawLayer2Background01 = drawLayer2BG01;
            this.DrawLayer2Background02 = drawLayer2BG02;
        }
    }

}
/*
[65: ]
        T8 PL T6 SI SE DS ?? P2
08AA18: C2 9A 82 00 00 41 00 4A

 */