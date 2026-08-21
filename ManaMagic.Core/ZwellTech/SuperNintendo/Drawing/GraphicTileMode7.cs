using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Encapsulates a Super Nintendo Mode7 graphic tile, which consists of pixel data.
    /// </summary>
    public sealed class GraphicTileMode7 : GraphicTile
    {
        /// <summary>
        /// Represents an empty Mode7 tile, with all bit planes set to zero.
        /// </summary>
        public static GraphicTileMode7 Empty { get; } = new GraphicTileMode7();

        private GraphicTileMode7() : base(8, 8)
        {
            this.TileType = TileType.Mode7;
        }

        private GraphicTileMode7(Span<byte> tileBuffer) : base(8, 8)
        {
            this.TileType = TileType.Mode7;
            for (int scanline = 0; scanline < 8; scanline++)
            {
                for (int y = 0; y < 8; y++)
                {
                    byte current = tileBuffer[y + (8 * scanline)];
                    this.scanlines[scanline][y] = current;

                    //byte current = tileBuffer[scanline + (8 * y)];
                    //this.scanlines[y][scanline] = current;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="GraphicTileMode7"/> object using the specified Mode7 byte array.
        /// </summary>
        /// <param name="tileBuffer">An array of bytes making up Mode7 image data.</param>
        /// <returns>A <see cref="GraphicTileMode7"/> object containing the decompressed image data.</returns>
        public static GraphicTileMode7 FromMode7Tile(Span<byte> tileBuffer)
        {
            return new GraphicTileMode7(tileBuffer);
        }
    }
}