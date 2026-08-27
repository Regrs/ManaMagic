using System;
using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Represents a set of tiles in Super Nintendo tile based graphics.
    /// </summary>
    public class Tileset : ICloneable
    {
        /// <summary>
        /// Gets a <see cref="Tileset"/> that is empty.
        /// </summary>
        public static Tileset Empty { get; } = new Tileset(0, Array.Empty<GraphicTile>(), TileType.Unknown);

        private readonly List<GraphicTile> tiles = new List<GraphicTile>();

        public int Index { get; }

        public virtual string Name { get; } = "Tileset";

        /// <summary>
        /// Gets the type of tiles contained within this tileset.
        /// </summary>
        public TileType TileType { get; } = TileType.Unknown;

        /// <summary>
        /// Gets a read-only list of the tiles in this tileset.
        /// </summary>
        public IReadOnlyList<GraphicTile> Tiles { get { return tiles; } }

        public bool CanDraw { get { return this.Tiles.Count > 0; } }
        public int Count { get { return this.tiles.Count; } }

        /// <summary>
        /// Gets the <see cref="GraphicTile"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="GraphicTile"/>  to get.</param>
        /// <returns>The <see cref="GraphicTile"/>  at the specified index.</returns>
        public GraphicTile this[int index] { get { return this.Tiles[index]; } }

        /// <summary>
        /// Creates a new instance of the <see cref="Tileset"/> class with the specified type and list of <see cref="GraphicTile"/>s.
        /// </summary>
        /// <param name="index">The index of the tileset.</param>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> that make up the tileset.</param>
        /// <param name="tileType">The type of tiles represented by this tileset.</param>
        public Tileset(int index, IReadOnlyList<GraphicTile> tiles, TileType tileType)
        {
            this.Index = index;
            this.TileType = tileType;
            this.tiles.AddRange(tiles);
        }

        public GraphicTile GetTile(int tileIndex, TileErrorMode errorMode)
        {
            bool hasTile = tileIndex >= 0 && tileIndex < this.Tiles.Count;
            GraphicTile tile = GraphicTile4Bpp.Empty;

            if (hasTile) { tile = this.Tiles[tileIndex]; }
            else if (errorMode == TileErrorMode.TransparentTile) { tile = GraphicTile4Bpp.Empty; }
            else if (errorMode == TileErrorMode.FullTile) { tile = GraphicTile4Bpp.Full; }
            else { ThrowHelper.ThrowInvalidOperationException("Invalid Tile Index"); }

            return tile;
        }

        public List<GraphicTile> GetTile(int tileIndex, TileSize size, TileErrorMode errorMode)
        {
            List<GraphicTile> tileList = new List<GraphicTile>();
            switch (size)
            {
                case TileSize.Size8X8:
                    Get8x8Tile(tileIndex, errorMode, tileList);
                    break;
                case TileSize.Size16X16:
                    Get16x16Tile(tileIndex, errorMode, tileList);
                    break;
                case TileSize.Size32X32:
                    Get32x32Tile(tileIndex, errorMode, tileList);
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Only sizes of 8x8, 16x16, and 32x32 are supported.", nameof(size));
                    break;
            }

            return tileList;

            void Get8x8Tile(int tileIndex, TileErrorMode errorMode, List<GraphicTile> tileList)
            {
                bool hasTile = tileIndex < this.Tiles.Count;
                GraphicTile tile = GraphicTile4Bpp.Empty;

                if (hasTile) { tile = this.Tiles[tileIndex]; }
                else if (errorMode == TileErrorMode.TransparentTile) { tile = GraphicTile4Bpp.Empty; }
                else if (errorMode == TileErrorMode.FullTile) { tile = GraphicTile4Bpp.Full; }
                else { ThrowHelper.ThrowInvalidOperationException("Invalid Tile Index"); }

                tileList.Add(tile);
            }

            void Get16x16Tile(int tileIndex, TileErrorMode errorMode, List<GraphicTile> tileList)
            {
                Get8x8Tile(tileIndex + 0x00, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x01, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x10, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x11, errorMode, tileList);
            }

            void Get32x32Tile(int tileIndex, TileErrorMode errorMode, List<GraphicTile> tileList)
            {
                Get8x8Tile(tileIndex + 0x00, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x01, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x10, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x11, errorMode, tileList);

                Get8x8Tile(tileIndex + 0x02, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x03, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x12, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x13, errorMode, tileList);

                Get8x8Tile(tileIndex + 0x20, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x21, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x30, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x31, errorMode, tileList);

                Get8x8Tile(tileIndex + 0x22, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x23, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x32, errorMode, tileList);
                Get8x8Tile(tileIndex + 0x33, errorMode, tileList);
            }
        }

        /// <summary>
        /// Merges the tiles of the specified <see cref="Tileset"/> into the current instances tileset.
        /// </summary>
        /// <param name="tileset">The <see cref="Tileset"/> whose tiles should be merged into the current instance.</param>
        public void Merge(Tileset tileset)
        {
            this.tiles.AddRange(tileset.tiles);
        }

        /// <summary>
        /// Draws the tileset using the specified sprite palette with 16 columns per row.
        /// </summary>
        /// <param name="palette">The <see cref="SpritePalette"/> to draw the tileset.</param>
        /// <returns>A <see cref="SuperNintendoGraphics"/> containing the drawn tileset.</returns>
        public SuperNintendoGraphics DrawTileset(SpritePalette palette)
        {
            return this.DrawTileset(palette, 16);
        }

        /// <summary>
        /// Draws the tileset using the specified sprite palette with the specified number of columns.
        /// </summary>
        /// <param name="palette">The <see cref="SpritePalette"/> to draw the tileset.</param>
        /// <param name="columnCount">The number of columns per row.</param>
        /// <returns>A <see cref="SuperNintendoGraphics"/> containing the drawn tileset.</returns>
        public SuperNintendoGraphics DrawTileset(SpritePalette palette, int columnCount)
        {
            int tileCount = this.Tiles.Count;
            int rowCount = SuperNintendoGraphics.CalculateRowCount(columnCount, tileCount);

            int width = columnCount * 8;
            int height = rowCount * 8;

            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(palette, width, height);
            graphics.DrawTileset(this, columnCount);
            return graphics;
        }

        /// <inheritdoc/>
        public virtual object Clone()
        {
            return new Tileset(this.Index, this.Tiles, this.TileType);
        }

        protected void Add(GraphicTile tile)
        {
            this.tiles.Add(tile);
        }
    }

    public enum TileErrorMode
    {
        ThrowException,
        TransparentTile,
        FullTile,
    }
}