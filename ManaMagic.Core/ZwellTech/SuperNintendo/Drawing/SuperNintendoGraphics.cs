using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Encapsulates a Super Nintendo drawing surface. This class cannot be inherited.
    /// </summary>
    public sealed class SuperNintendoGraphics : IDisposable
    {
        private SpritePalette palette;
        private DataTable<SpritePalette>? paletteTable;
        private int[] bitmapBuffer;
        private GCHandle bufferHandle;

        /// <summary>
        /// Gets if the graphics object supports more than one <see cref="SpritePalette"/>.
        /// </summary>
        [MemberNotNullWhen(true, "paletteTable")]
        public bool MultiPaletteMode { get; } = false;

        /// <summary>
        /// Gets the width and height, in pixels, of this graphics object.
        /// </summary>
        public Size Size { get; } = Size.Empty;

        /// <summary>
        /// Gets a value indicating if this object has been disposed.
        /// </summary>
        public bool Disposed { get; private set; } = false;

        private SuperNintendoGraphics(SpritePalette palette, int width, int height)
        {
            this.palette = palette;
            this.MultiPaletteMode = false;

            this.Size = new Size(width, height);
            this.bitmapBuffer = new int[this.Size.Width * this.Size.Height];
            this.bufferHandle = GCHandle.Alloc(this.bitmapBuffer, GCHandleType.Pinned);
        }

        private SuperNintendoGraphics(DataTable<SpritePalette> paletteList, int width, int height)
        {
            this.palette = paletteList[0];
            this.paletteTable = new DataTable<SpritePalette>(paletteList.Rows);
            this.MultiPaletteMode = true;

            this.Size = new Size(width, height);
            this.bitmapBuffer = new int[this.Size.Width * this.Size.Height];
            this.bufferHandle = GCHandle.Alloc(this.bitmapBuffer, GCHandleType.Pinned);
        }

        /// <summary>
        /// Creates a <see cref="Bitmap"/> object from this <see cref="SuperNintendoGraphics"/>.
        /// </summary>
        /// <returns>A <see cref="Bitmap"/> that represents the <see cref="SuperNintendoGraphics"/>.</returns>
        public Bitmap GetBitmap()
        {
            return this.GetBitmap(false);
        }

        /// <summary>
        /// Creates a <see cref="Bitmap"/> object from this <see cref="SuperNintendoGraphics"/> and optionally makes the color at index 0 transparent.
        /// </summary>
        /// <param name="applyTransparency">If the color at palette index 0 should be made transparent.</param>
        /// <returns>A <see cref="Bitmap"/> that represents the <see cref="SuperNintendoGraphics"/>.</returns>
        public Bitmap GetBitmap(bool applyTransparency)
        {
            Bitmap bitmap = new Bitmap(this.Size.Width, this.Size.Height, this.Size.Width * 4, PixelFormat.Format32bppArgb, this.bufferHandle.AddrOfPinnedObject());
            //Bitmap bitmap = new Bitmap(this.Size.Width, this.Size.Height, this.Size.Width * 4, PixelFormat.Format16bppRgb555, this.bufferHandle.AddrOfPinnedObject());
            if (applyTransparency)
            {
                Rgb555Color color = this.palette[0];
                bitmap.MakeTransparent(Color.FromArgb(color.ToArgb()));
            }
            return bitmap;
        }

        /// <summary>
        /// Fills the graphics object with the transparency color.
        /// </summary>
        public void Fill()
        {
            this.Fill(0);
        }

        /// <summary>
        /// Fills the graphics object with the color at the specified index within the palette.
        /// </summary>
        /// <param name="paletteIndex">The index of the color to use to fill the graphics object.</param>
        public void Fill(int paletteIndex)
        {
            this.Fill(this.palette[paletteIndex]);
        }

        /// <summary>
        /// Fills the graphics object with the color at the specified index within the palette of the specified set.
        /// </summary>
        /// <param name="paletteIndex">The index of the color to use to fill the graphics object.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Fill(int paletteIndex, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            SpritePalette palette = this.paletteTable[paletteSetIndex];
            this.Fill(palette[paletteIndex]);
        }

        /// <summary>
        /// Draws a 8x8 pixel <see cref="GraphicTile"/> at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tile">The <see cref="GraphicTile"/> to be drawn.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        public void DrawTile(GraphicTile tile, int x, int y)
        {
            this.DrawTile(this.palette, tile, x, y, FlipType.None);
        }

        /// <summary>
        /// Draws a 8x8 pixel <see cref="GraphicTile"/> with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tile">The <see cref="GraphicTile"/> to be drawn.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="flipType">The axis used to flip the tile.</param>
        public void DrawTile(GraphicTile tile, int x, int y, FlipType flipType)
        {
            this.DrawTile(this.palette, tile, x, y, flipType);
        }

        /// <summary>
        /// Draws a 8x8, 16x16, or 32x32 pixel <see cref="GraphicTile"/> with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of 1, 4, or 16 8x8 <see cref="GraphicTile"/>s to draw.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="flipType">The axis used to flip the tile.</param>
        /// <param name="size">The size of the tile to draw.</param>
        /// <exception cref="ArgumentException">Thrown when size is set to 64x64.</exception>
        /// <exception cref="ArgumentNullException">Thrown when tiles is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when tiles length is zero.</exception>
        public void DrawTile(ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType, TileSize size)
        {
            ValidationHelper.ThrowIfArgumentNull(tiles);
            ValidationHelper.ThrowIfArgumentLengthIsZero(tiles, "Graphic tile data table cannot be empty.");

            switch (size)
            {
                case TileSize.Size8X8:
                    this.DrawTile(this.palette, tiles[0], x, y, flipType);
                    break;
                case TileSize.Size16X16:
                    this.Draw16x16Tile(this.palette, tiles, x, y, flipType);
                    break;
                case TileSize.Size32X32:
                    this.Draw32x32Tile(this.palette, tiles, x, y, flipType);
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Only sizes of 8x8, 16x16, and 32x32 are supported.", nameof(size));
                    break;
            }
        }

        /// <summary>
        /// Draws a 8x8 pixel <see cref="GraphicTile"/> at a point specified by a coordinate pair with the <see cref="SpritePalette"/> at the specified index.
        /// </summary>
        /// <param name="tile">The <see cref="GraphicTile"/> to be drawn.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void DrawTile(GraphicTile tile, int x, int y, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            this.DrawTile(tile, x, y, FlipType.None, paletteSetIndex);
        }

        /// <summary>
        /// Draws a 8x8 pixel <see cref="GraphicTile"/> with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tile">The <see cref="GraphicTile"/> to be drawn.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the <see cref="GraphicTile"/> to draw.</param>
        /// <param name="flipType">The axis used to flip the tile.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void DrawTile(GraphicTile tile, int x, int y, FlipType flipType, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            SpritePalette palette = this.paletteTable[paletteSetIndex];
            this.DrawTile(palette, tile, x, y, flipType);
        }

        /// <summary>
        /// Draws a 16x16 pixel tile from a span of four <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        public void Draw16x16Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.Draw16x16Tile(this.palette, tiles, x, y, FlipType.None);
        }

        /// <summary>
        /// Draws a 16x16 pixel tile from a read only list of four <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        public void Draw16x16Tile(IReadOnlyList<GraphicTile> tiles, int x, int y)
        {
            Span<GraphicTile> graphicTiles = new GraphicTile[4];
            graphicTiles[0] = tiles[0];
            graphicTiles[1] = tiles[1];
            graphicTiles[2] = tiles[2];
            graphicTiles[3] = tiles[3];
            this.Draw16x16Tile(this.palette, graphicTiles, x, y, FlipType.None);
        }

        /// <summary>
        /// Draws a 16x16 pixel tile from a span of four <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="flipType">If the tile is rotated and the axis used to flip the tile.</param>
        public void Draw16x16Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType)
        {
            this.Draw16x16Tile(this.palette, tiles, x, y, flipType);
        }

        /// <summary>
        /// Draws a 16x16 pixel tile from a span of four <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw16x16Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y, int paletteSetIndex)
        {
            this.Draw16x16Tile(tiles, x, y, FlipType.None, paletteSetIndex);
        }

        /// <summary>
        /// Draws a 16x16 pixel tile from a span of four <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="flipType">The axis used to flip the tile.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw16x16Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            SpritePalette palette = this.paletteTable[paletteSetIndex];
            this.Draw16x16Tile(palette, tiles, x, y, flipType);
        }

        /// <summary>
        /// Draws a 32x32 pixel tile from a span of sixteen <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        public void Draw32x32Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.Draw32x32Tile(this.palette, tiles, x, y, FlipType.None);
        }

        /// <summary>
        /// Draws a 32x32 pixel tile from a span of sixteen <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="flipType">If the tile is rotated and the axis used to flip the tile.</param>
        public void Draw32x32Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType)
        {
            this.Draw32x32Tile(this.palette, tiles, x, y, flipType);
        }

        /// <summary>
        /// Draws a 32x32 pixel tile from a span of sixteen <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only span of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw32x32Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y, int paletteSetIndex)
        {
            this.Draw32x32Tile(tiles, x, y, FlipType.None, paletteSetIndex);
        }

        /// <summary>
        /// Draws a 32x32 pixel tile from an array of sixteen <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="flipType">If the tile is rotated and the axis used to flip the tile.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw32x32Tile(ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            SpritePalette palette = this.paletteTable[paletteSetIndex];
            this.Draw32x32Tile(palette, tiles, x, y, flipType);
        }

        /// <summary>
        /// Draws a <see cref="Tileset"/> into the graphics object.
        /// </summary>
        /// <param name="tileset">The <see cref="Tileset"/> to be drawn.</param>
        /// <param name="columnCount">The number of 8x8 <see cref="GraphicTile"/>s to draw per row.</param>
        public void DrawTileset(Tileset tileset, int columnCount)
        {
            int tileCount = tileset.Tiles.Count;
            int rowCount = SuperNintendoGraphics.CalculateRowCount(columnCount, tileCount);
            bool mode7 = tileset.TileType == TileType.Mode7;
            GraphicTile blankTile = !mode7 ? GraphicTile4Bpp.Empty : GraphicTileMode7.Empty;
            this.Fill();

            int rowIndex = 0;
            int columnIndex = 0;
            for (int i = 0; i < tileCount; i++)
            {
                // If we still have tiles to draw, then draw the requested tile.
                // If the tile count was over stated, then draw default blank tiles in the remaining spaces.
                GraphicTile tileToDraw = i < tileCount ? tileset.Tiles[i] : blankTile;
                this.DrawTile(tileToDraw, rowIndex * 8, columnIndex * 8);

                if (!mode7)
                {
                    rowIndex++;
                    // Wrap around to the start of the next row if we have reached the end of the current row.
                    if (rowIndex == columnCount)
                    {
                        rowIndex = 0;
                        columnIndex++;
                    }
                }
                else
                {
                    columnIndex++;
                    // Wrap around to the start of the next column if we have reached the end of the current column.
                    if (columnIndex == rowCount)
                    {
                        columnIndex = 0;
                        rowIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a line connecting the two points specified by the coordinate pairs.
        /// </summary>
        /// <param name="paletteIndex">The index of the palette to use to color the line.</param>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        public void DrawLine(int paletteIndex, int x1, int y1, int x2, int y2)
        {
            this.DrawLine(this.palette[paletteIndex], x1, y1, x2, y2);
        }

        /// <summary>
        /// Draws a rectangle specified by a coordinate pair, a width, and a height.
        /// </summary>
        /// <param name="paletteIndex">The index of the palette to use to color the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        public void DrawRectangle(int paletteIndex, int x, int y, int width, int height)
        {
            this.DrawRectangle(this.palette[paletteIndex], x, y, width, height);
        }

        /// <summary>
        /// Draws a rectangle specified by a <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="paletteIndex">The index of the palette to use to color the rectangle.</param>
        /// <param name="rectangle">A <see cref="Rectangle"/> structure that represents the rectangle to draw.</param>
        public void DrawRectangle(int paletteIndex, Rectangle rectangle)
        {
            this.DrawRectangle(this.palette[paletteIndex], rectangle);
        }

        public void DrawRectangle(Rgb555Color color, int x, int y, int width, int height)
        {
            this.DrawLine(color, x, y, x + width, y);                   // - TOP
            this.DrawLine(color, x, y + height, x + width, y + height); // - BOTTOM
            this.DrawLine(color, x, y, x, y + height);                  // - LEFT
            this.DrawLine(color, x + width, y, x + width, y + height);  // - RIGHT
        }

        /// <summary>
        /// Draws 8x8 grid lines into the graphics object for the specified number of row and columns.
        /// </summary>
        /// <param name="paletteIndex">The index of the palette to use to color the grid lines.</param>
        /// <param name="rowCount">The number of rows in the grid.</param>
        /// <param name="columnCount">The number of columns in the grid.</param>
        public void DrawGridlines(int paletteIndex, int rowCount, int columnCount)
        {
            this.DrawGridlines(this.palette[paletteIndex], rowCount, columnCount);
        }

        /// <summary>
        /// Draws 8x8 grid lines into the graphics object for the specified number of row and columns.
        /// </summary>
        /// <param name="color">The color of the grid lines.</param>
        /// <param name="rowCount">The number of rows in the grid.</param>
        /// <param name="columnCount">The number of columns in the grid.</param>
        public void DrawGridlines(Rgb555Color color, int rowCount, int columnCount)
        {
            this.DrawLine(color, 0, 0, this.Size.Width - 1, 0);                                       // Top
            this.DrawLine(color, 0, this.Size.Height - 1, this.Size.Width - 1, this.Size.Height - 1); // Bottom
            this.DrawLine(color, 0, 0, 0, this.Size.Height - 1);                                      // Left
            this.DrawLine(color, this.Size.Width - 1, 0, this.Size.Width - 1, this.Size.Height - 1);  // Right

            for (int i = 0; i < columnCount; i++)
            {
                this.DrawLine(color, (i * 8) + 0, 0, (i * 8) + 0, this.Size.Height);
            }
            for (int i = 0; i < rowCount; i++)
            {
                this.DrawLine(color, 0, (i * 8) + 0, this.Size.Width, (i * 8) + 0);
            }
        }

        /// <summary>
        /// Merges the pixel data of the specified <see cref="SuperNintendoGraphics"/> into the current instance.
        /// </summary>
        /// <param name="source">The <see cref="SuperNintendoGraphics"/> whose pixel data will be merged into the current instance.</param>
        public void Merge(SuperNintendoGraphics source)
        {
            int transparent = source.palette[0].ToArgb();
            for (int i = 0; i < source.bitmapBuffer.Length; i++)
            {
                int color = source.bitmapBuffer[i];
                if (color != transparent)
                {
                    this.bitmapBuffer[i] = color;
                }
            }
        }

        public void Merge(SuperNintendoGraphics source, CGADSUB colorMath)
        {
            // https://snes.nesdev.org/wiki/Color_math
            int transparent = source.palette[0].ToArgb();
            Dictionary<int, bool> blendDict = new Dictionary<int, bool>();
            for (int i = 0; i < source.bitmapBuffer.Length; i++)
            {
                int sourceColor = source.bitmapBuffer[i];
                if (sourceColor != transparent)
                {
                    int blendColor = sourceColor;
                    int destColor = this.bitmapBuffer[i];
                    if (CanBlend(destColor, blendDict))
                    {
                        blendColor = colorMath.HasFlag(CGADSUB.Subtractive) ? (destColor - sourceColor) : (destColor + sourceColor);
                        if (colorMath.HasFlag(CGADSUB.HalfColorMath))
                        {
                            blendColor /= 2;
                        }
                    }
                    this.bitmapBuffer[i] = blendColor;
                }
            }
        }

        private bool CanBlend(int color, Dictionary<int, bool> blendDict)
        {
            int colorIndex = -1;
            if (!blendDict.TryGetValue(color, out bool canBlend))
            {
                for (int i = 0; i < this.paletteTable!.RowCount; i++)
                {
                    SpritePalette current = this.paletteTable[i];
                    for (int j = 4; j < current.NumberOfColors; j++)
                    {
                        if (color == current[j].ToArgb())
                        {
                            colorIndex = i;
                            break;
                        }
                    }
                }

                canBlend = colorIndex >= 6;
                blendDict.Add(color, canBlend);
            }

            //if (colorIndex > -1) { System.Diagnostics.Debugger.Break(); }
            return canBlend;
            //return colorIndex > -1 && colorIndex <= 7;
        }

#pragma warning disable IDE0051 // Remove unused private members
        private static bool CanBlend(SpritePalette palette, int color)
#pragma warning restore IDE0051 // Remove unused private members
        {
            int colorIndex = -1;
            for (int i = 0; i < palette.NumberOfColors; i++)
            {
                if (color == palette[i].ToArgb())
                {
                    colorIndex = i;
                    break;
                }
            }

            if (colorIndex > -1) { System.Diagnostics.Debugger.Break(); }
            return colorIndex >= 4;// && colorIndex <= 7;
        }

        /// <summary>
        /// Merges the pixel data of the specified <see cref="SuperNintendoGraphics"/> into the current instance at the specified coordinate pair.
        /// </summary>
        /// <param name="source">The <see cref="SuperNintendoGraphics"/> whose pixel data will be merged into the current instance.</param>
        /// <param name="x">The x-coordinate in this instance at which the merge will begin.</param>
        /// <param name="y">The y-coordinate in this instance at which the merge will begin.</param>
        public void Merge(SuperNintendoGraphics source, int x, int y)
        {
            int transparent = source.palette[0].ToArgb();
            for (int sourceX = 0; sourceX < source.Size.Width; sourceX++)
            {
                for (int sourceY = 0; sourceY < source.Size.Height; sourceY++)
                {
                    int color = source.bitmapBuffer[sourceX + (sourceY * source.Size.Width)];
                    if (color != transparent)
                    {
                        this.bitmapBuffer[(sourceX + x) + ((sourceY + y) * this.Size.Width)] = color;
                    }
                }
            }
        }

        /// <summary>
        /// Releases all resources used by this <see cref="SuperNintendoGraphics"/>.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    if (this.MultiPaletteMode)
                    {
                        this.paletteTable = null;
                    }

                    this.palette = null!;
                    this.paletteTable = null;
                    this.bitmapBuffer = null!;
                }

                if (this.bufferHandle.IsAllocated)
                {
                    this.bufferHandle.Free();
                }
                this.Disposed = true;
            }
        }

        private void SetPixel(int x, int y, Rgb555Color color)
        {
            int index = Math.Abs(x + (y * this.Size.Width));
            if (index >= this.bitmapBuffer.Length) { return; }
            //if (this.bitmapBuffer[index] == color.ToArgb()) { return; }
            this.bitmapBuffer[index] = color.ToArgb();
            //this.bitmapBuffer[index] = color.ToRgb555();
        }

        public void Fill(Rgb555Color color)
        {
            for (int i = 0; i < this.bitmapBuffer.Length; i++)
            {
                this.bitmapBuffer[i] = color.ToArgb();
            }
        }

        private void DrawLine(Rgb555Color color, int x1, int y1, int x2, int y2)
        {
            // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
            int xDelta = x2 - x1;
            int yDelta = y2 - y1;

            int stepX1 = 0;
            int stepY1 = 0;
            int stepX2 = 0;
            int stepY2 = 0;

            if (xDelta != 0)
            {
                stepX1 = xDelta > 0 ? -1 : -1;
                stepX2 = xDelta > 0 ? 1 : -1;
            }

            if (yDelta != 0)
            {
                stepY1 = yDelta > 0 ? 1 : -1;
                stepY2 = yDelta > 0 ? 1 : -1;
            }

            xDelta = Math.Abs(xDelta);
            yDelta = Math.Abs(yDelta);
            if (xDelta < yDelta)
            {
                int tmp = xDelta;
                xDelta = yDelta;
                yDelta = tmp;
                stepX2 = 0;
            }

            int nextDelta = xDelta >> 1;
            for (int i = 0; i <= xDelta; i++)
            {

                if (x1 >= this.Size.Width) { break; }
                if (y1 >= this.Size.Height) { break; }
                this.SetPixel(x1, y1, color);

                nextDelta += yDelta;
                if (nextDelta > xDelta)
                {
                    nextDelta -= xDelta;
                    x1 += stepX1;
                    y1 += stepY1;
                }
                else
                {
                    x1 += stepX2;
                    y1 += stepY2;
                }
            }
        }

        private void DrawRectangle(Rgb555Color color, Rectangle rectangle)
        {
            this.DrawRectangle(color, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        private void DrawTile(SpritePalette palette, GraphicTile tile, int x, int y)
        {
            for (int yOffset = 0; yOffset < 8; yOffset++)
            {
                for (int xOffset = 0; xOffset < 8; xOffset++)
                {
                    // Get the color index for this pixel.
                    byte colorIndex = tile[xOffset, yOffset];

                    // Draw the pixel.
                    this.SetPixel(x + xOffset, y + yOffset, palette[colorIndex]);
                }
            }
        }

        private void DrawTile(SpritePalette palette, GraphicTile tile, int x, int y, FlipType flipType)
        {
            switch (flipType)
            {
                case FlipType.None:
                    this.DrawTile(palette, tile, x, y);
                    break;
                case FlipType.HorizontalFlip:
                    this.DrawTileHorizontalFlip(palette, tile, x, y);
                    break;
                case FlipType.VerticalFlip:
                    this.DrawTileVerticalFlip(palette, tile, x, y);
                    break;
                case FlipType.HorizontalVerticalFlip:
                    this.DrawTileHVFlip(palette, tile, x, y);
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Invalid flipType.", nameof(flipType));
                    break;
            }
        }

        private void DrawTileHorizontalFlip(SpritePalette palette, GraphicTile tile, int x, int y)
        {
            for (int yOffset = 0; yOffset < 8; yOffset++)
            {
                for (int xOffset = 7; xOffset >= 0; xOffset--)
                {
                    // Get the color index for this pixel.
                    byte active = tile[xOffset, yOffset];

                    // Draw the pixel.
                    this.SetPixel(x + (7 - xOffset), y + yOffset, palette[active]);
                }
            }
        }

        private void DrawTileVerticalFlip(SpritePalette palette, GraphicTile tile, int x, int y)
        {
            for (int yOffset = 7; yOffset >= 0; yOffset--)
            {
                for (int xOffset = 0; xOffset < 8; xOffset++)
                {
                    // Get the color index for this pixel.
                    byte active = tile[xOffset, yOffset];

                    // Draw the pixel.
                    this.SetPixel(x + xOffset, y + (7 - yOffset), palette[active]);
                }
            }
        }

        private void DrawTileHVFlip(SpritePalette palette, GraphicTile tile, int x, int y)
        {
            for (int yOffset = 7; yOffset >= 0; yOffset--)
            {
                for (int xOffset = 7; xOffset >= 0; xOffset--)
                {
                    // Get the color index for this pixel.
                    byte active = tile[xOffset, yOffset];

                    // Draw the pixel.
                    this.SetPixel(x + (7 - xOffset), y + (7 - yOffset), palette[active]);
                }
            }
        }

        private void Draw16x16Tile(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.DrawTile(palette, tiles[0], x, y);
            this.DrawTile(palette, tiles[1], x + 8, y);
            this.DrawTile(palette, tiles[2], x, y + 8);
            this.DrawTile(palette, tiles[3], x + 8, y + 8);
        }

        private void Draw16x16Tile(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType)
        {
            switch (flipType)
            {
                case FlipType.None:
                    this.Draw16x16Tile(palette, tiles, x, y);
                    break;
                case FlipType.HorizontalFlip:
                    this.Draw16x16TileHorizontalFlip(palette, tiles, x, y, 0);
                    break;
                case FlipType.VerticalFlip:
                    this.Draw16x16TileVerticalFlip(palette, tiles, x, y, 0);
                    break;
                case FlipType.HorizontalVerticalFlip:
                    this.Draw16x16TileHVFlip(palette, tiles, x, y, 0);
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Invalid flipType.", nameof(flipType));
                    break;
            }
        }

        private void Draw16x16TileHorizontalFlip(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y, int skip)
        {
            this.DrawTileHorizontalFlip(palette, tiles[1 + skip], x, y);
            this.DrawTileHorizontalFlip(palette, tiles[0 + skip], x + 8, y);
            this.DrawTileHorizontalFlip(palette, tiles[3 + skip], x, y + 8);
            this.DrawTileHorizontalFlip(palette, tiles[2 + skip], x + 8, y + 8);
        }

        private void Draw16x16TileVerticalFlip(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y, int skip)
        {
            this.DrawTileVerticalFlip(palette, tiles[2 + skip], x, y);
            this.DrawTileVerticalFlip(palette, tiles[3 + skip], x + 8, y);
            this.DrawTileVerticalFlip(palette, tiles[0 + skip], x, y + 8);
            this.DrawTileVerticalFlip(palette, tiles[1 + skip], x + 8, y + 8);
        }

        private void Draw16x16TileHVFlip(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y, int skip)
        {
            this.DrawTileHVFlip(palette, tiles[3 + skip], x, y);
            this.DrawTileHVFlip(palette, tiles[2 + skip], x + 8, y);
            this.DrawTileHVFlip(palette, tiles[1 + skip], x, y + 8);
            this.DrawTileHVFlip(palette, tiles[0 + skip], x + 8, y + 8);
        }

        private void Draw32x32Tile(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.Draw16x16Tile(palette, tiles.Slice(0, 4), x, y);
            this.Draw16x16Tile(palette, tiles.Slice(4, 4), x + 16, y);
            this.Draw16x16Tile(palette, tiles.Slice(8, 4), x, y + 16);
            this.Draw16x16Tile(palette, tiles.Slice(12, 4), x + 16, y + 16);
        }

        private void Draw32x32Tile(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y, FlipType flipType)
        {
            switch (flipType)
            {
                case FlipType.None:
                    this.Draw32x32Tile(palette, tiles, x, y);
                    break;
                case FlipType.HorizontalFlip:
                    this.Draw32x32TileHorizontalFlip(palette, tiles, x, y);
                    break;
                case FlipType.VerticalFlip:
                    this.Draw32x32TileVerticalFlip(palette, tiles, x, y);
                    break;
                case FlipType.HorizontalVerticalFlip:
                    this.Draw32x32TileHVFlip(palette, tiles, x, y);
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Invalid flipType.", nameof(flipType));
                    break;
            }
        }

        private void Draw32x32TileHorizontalFlip(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.Draw16x16TileHorizontalFlip(palette, tiles, x, y, 4);
            this.Draw16x16TileHorizontalFlip(palette, tiles, x + 16, y, 0);
            this.Draw16x16TileHorizontalFlip(palette, tiles, x, y + 16, 12);
            this.Draw16x16TileHorizontalFlip(palette, tiles, x + 16, y + 16, 8);
        }

        private void Draw32x32TileVerticalFlip(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.Draw16x16TileVerticalFlip(palette, tiles, x, y, 8);
            this.Draw16x16TileVerticalFlip(palette, tiles, x + 16, y, 12);
            this.Draw16x16TileVerticalFlip(palette, tiles, x, y + 16, 0);
            this.Draw16x16TileVerticalFlip(palette, tiles, x + 16, y + 16, 4);
        }

        private void Draw32x32TileHVFlip(SpritePalette palette, ReadOnlySpan<GraphicTile> tiles, int x, int y)
        {
            this.Draw16x16TileHVFlip(palette, tiles, x, y, 12);
            this.Draw16x16TileHVFlip(palette, tiles, x + 16, y, 8);
            this.Draw16x16TileHVFlip(palette, tiles, x, y + 16, 4);
            this.Draw16x16TileHVFlip(palette, tiles, x + 16, y + 16, 0);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SuperNintendoGraphics"/> class with the specified size and palette.
        /// </summary>
        /// <param name="palette">The <see cref="SpritePalette"/> used to draw the graphics.</param>
        /// <param name="width">The width of the graphics, in pixels.</param>
        /// <param name="height">The height of the graphics, in pixels.</param>
        /// <returns>A new <see cref="SuperNintendoGraphics"/> object with the specified parameters.</returns>
        public static SuperNintendoGraphics CreateGraphics(SpritePalette palette, int width, int height)
        {
            if (palette == null) { ThrowHelper.ThrowArgumentNullException(nameof(palette)); }
            return new SuperNintendoGraphics(palette, width, height);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SuperNintendoGraphics"/> class with the specified size and palette set.
        /// </summary>
        /// <param name="paletteSet">A set of <see cref="SpritePalette"/>s used to draw the graphics.</param>
        /// <param name="width">The width of the graphics, in pixels.</param>
        /// <param name="height">The height of the graphics, in pixels.</param>
        /// <returns>A new <see cref="SuperNintendoGraphics"/> object with the specified parameters.</returns>
        public static SuperNintendoGraphics CreateGraphics(DataTable<SpritePalette> paletteSet, int width, int height)
        {
            if (paletteSet == null) { ThrowHelper.ThrowArgumentNullException("paletteList"); }
            if (paletteSet.RowCount == 0) { ThrowHelper.ThrowArgumentException("Palette List is empty.", "paletteList"); }
            if (paletteSet[0] == null) { ThrowHelper.ThrowArgumentNullException("paletteList"); }
            return new SuperNintendoGraphics(paletteSet, width, height);
        }

        /// <summary>
        /// Returns the number of rows required for a tile set of a given size with the specified number of columns.
        /// </summary>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="tileCount">The number of tiles.</param>
        /// <returns>The number of rows.</returns>
        public static int CalculateRowCount(int columnCount, int tileCount)
        {
            // If they're are less tiles than the number of columns, then everything goes on one row.
            int rowCount = 1;
            if (tileCount > columnCount)
            {
                // Divide the number of tiles by the number of columns to get the number of rows.
                rowCount = tileCount / columnCount;

                // If the columns don't divide evenly into the tile count then add an extra row to catch the spillover.
                if ((tileCount % columnCount) != 0) { rowCount++; }
            }

            return rowCount;
        }
    }
}
/*
        public void DrawTile(List<GraphicTile> tiles, int x, int y, FlipType flipType, TileSize size)
        {
            ValidationHelper.ThrowIfArgumentNull(tiles);
            ValidationHelper.ThrowIfArgumentLengthIsZero(tiles, "Graphic tile data table cannot be empty.");

            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.DrawTile(span, x, y, flipType, size);
        }
        /// <summary>
        /// Draws a 16x16 pixel tile from an array of four <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        public void Draw16x16Tile(List<GraphicTile> tiles, int x, int y)
        {
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw16x16Tile(this.palette, span, x, y, FlipType.None);
        }
        /// <summary>
        /// Draws a 16x16 pixel tile from an array of four <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="flipType">If the tile is rotated and the axis used to flip the tile.</param>
        public void Draw16x16Tile(List<GraphicTile> tiles, int x, int y, FlipType flipType)
        {
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw16x16Tile(this.palette, span, x, y, flipType);
        }
        /// <summary>
        /// Draws a 16x16 pixel tile from an array of four <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw16x16Tile(List<GraphicTile> tiles, int x, int y, int paletteSetIndex)
        {
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw16x16Tile(span, x, y, FlipType.None, paletteSetIndex);
        }
        /// <summary>
        /// Draws a 32x32 pixel tile from an array of sixteen <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw32x32Tile(List<GraphicTile> tiles, int x, int y, int paletteSetIndex)
        {
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw32x32Tile(span, x, y, FlipType.None, paletteSetIndex);
        }
        /// <summary>
        /// Draws a 32x32 pixel tile from an array of sixteen <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="flipType">If the tile is rotated and the axis used to flip the tile.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw32x32Tile(List<GraphicTile> tiles, int x, int y, FlipType flipType, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            SpritePalette palette = this.paletteTable[paletteSetIndex];
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw32x32Tile(palette, span, x, y, flipType);
        }
        /// <summary>
        /// Draws a 32x32 pixel tile from an array of sixteen <see cref="GraphicTile"/>s at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        public void Draw32x32Tile(List<GraphicTile> tiles, int x, int y)
        {
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw32x32Tile(this.palette, span, x, y, FlipType.None);
        }
        /// <summary>
        /// Draws a 32x32 pixel tile from an array of sixteen <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 32x32 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 32x32 tile to draw.</param>
        /// <param name="flipType">If the tile is rotated and the axis used to flip the tile.</param>
        public void Draw32x32Tile(List<GraphicTile> tiles, int x, int y, FlipType flipType)
        {
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw32x32Tile(this.palette, span, x, y, flipType);
        }
        /// <summary>
        /// Draws a 16x16 pixel tile from an array of four <see cref="GraphicTile"/>s with the specified rotate flip type, at a point specified by a coordinate pair.
        /// </summary>
        /// <param name="tiles">A read-only list of <see cref="GraphicTile"/> to create the 16x16 tile from.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the 16x16 tile to draw.</param>
        /// <param name="flipType">The axis used to flip the tile.</param>
        /// <param name="paletteSetIndex">The index of the <see cref="SpritePalette"/> to use to draw the tile.</param>
        /// <exception cref="InvalidOperationException"><see cref="MultiPaletteMode"/> is false.</exception>
        public void Draw16x16Tile(List<GraphicTile> tiles, int x, int y, FlipType flipType, int paletteSetIndex)
        {
            if (!this.MultiPaletteMode)
            {
                ThrowHelper.ThrowInvalidOperationException("Graphics is not in multi palette mode.");
            }

            SpritePalette palette = this.paletteTable[paletteSetIndex];
            ReadOnlySpan<GraphicTile> span = CollectionsMarshal.AsSpan<GraphicTile>(tiles);
            this.Draw16x16Tile(palette, span, x, y, flipType);
        }
        private void Draw16x16Tile(SpritePalette palette, IReadOnlyList<GraphicTile> tiles, int x, int y)
        {
            this.DrawTile(palette, tiles[0], x, y);
            this.DrawTile(palette, tiles[1], x + 8, y);
            this.DrawTile(palette, tiles[2], x, y + 8);
            this.DrawTile(palette, tiles[3], x + 8, y + 8);
        }
        private void Draw32x32Tile(SpritePalette palette, IReadOnlyList<GraphicTile> tiles, int x, int y)
        {
            this.Draw16x16Tile(palette, tiles.Take(4).ToList(), x, y);
            this.Draw16x16Tile(palette, tiles.Skip(4).Take(4).ToList(), x + 16, y);
            this.Draw16x16Tile(palette, tiles.Skip(8).Take(4).ToList(), x, y + 16);
            this.Draw16x16Tile(palette, tiles.Skip(12).Take(4).ToList(), x + 16, y + 16);
        }
 */