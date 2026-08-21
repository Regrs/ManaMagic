using System;
using System.Text;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Encapsulates a Super Nintendo 4BPP graphic tile, which consists of pixel data.
    /// </summary>
    public sealed class GraphicTile4Bpp : GraphicTile
    {
        private static byte[] FullPixelPlaneBuffer = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

        /// <summary>
        /// The size (in bytes) of an 8x8 4BPP Graphics Tile.
        /// </summary>
        public const int Tile4Bpp8x8Size = 32;

        /// <summary>
        /// Represents an empty 4BPP tile, with all bit planes set to zero.
        /// </summary>
        public static GraphicTile4Bpp Empty { get; } = new GraphicTile4Bpp();
        /// <summary>
        /// Represents an full 4BPP tile, with all bit planes set to 0xFF.
        /// </summary>
        public static GraphicTile4Bpp Full { get; } = new GraphicTile4Bpp(FullPixelPlaneBuffer, FullPixelPlaneBuffer, FullPixelPlaneBuffer, FullPixelPlaneBuffer);

        private readonly byte[][] bitplanes = new byte[4][];

        private GraphicTile4Bpp() : base(8, 8)
        {
            this.TileType = TileType.FourBitsPerPixel;
            this.bitplanes[0] = new byte[8];
            this.bitplanes[1] = this.bitplanes[0];
            this.bitplanes[2] = this.bitplanes[0];
            this.bitplanes[3] = this.bitplanes[0];
            this.CreateScanlines();
        }

        private GraphicTile4Bpp(byte[] bitplane1, byte[] bitplane2, byte[] bitplane3, byte[] bitplane4) : base(8, 8)
        {
            this.TileType = TileType.FourBitsPerPixel;
            this.bitplanes[0] = bitplane1;
            this.bitplanes[1] = bitplane2;
            this.bitplanes[2] = bitplane3;
            this.bitplanes[3] = bitplane4;
            this.CreateScanlines();
        }

        private GraphicTile4Bpp(TileType tileType, bool mirrorPalette, byte[] bitplane1, byte[] bitplane2, byte[] bitplane3, byte[] bitplane4) : base(8, 8)
        {
            this.TileType = tileType;
            this.bitplanes[0] = bitplane1;
            this.bitplanes[1] = bitplane2;
            this.bitplanes[2] = bitplane3;
            this.bitplanes[3] = bitplane4;
            this.CreateScanlines(mirrorPalette);
        }

        /// <summary>
        /// Creates a new <see cref="GraphicTile4Bpp"/> object using the specified 3BPP compressed byte array.
        /// </summary>
        /// <param name="tileBuffer">A compressed 3BPP array of bytes making up the image data. Expected Length is 24 bytes.</param>
        /// <returns>A <see cref="GraphicTile4Bpp"/> object containing the decompressed image data.</returns>
        public static GraphicTile4Bpp From3BppTile(Span<byte> tileBuffer)
        {
            byte[] bitplane1 = new byte[8];
            byte[] bitplane2 = new byte[8];
            byte[] bitplane3 = new byte[8];
            byte[] bitplane4 = new byte[8];

            for (byte i = 0; i < 8; i++)
            {
                // Fetch 3 bytes to make up the color indexes for the current scanline.
                bitplane1[i] = tileBuffer[(i * 3)];
                bitplane2[i] = tileBuffer[(i * 3) + 1];
                bitplane3[i] = tileBuffer[(i * 3) + 2];
                bitplane4[i] = 0x00;
            }
            GraphicTile4Bpp tile = new GraphicTile4Bpp(TileType.ThreeBitsPerPixel, true, bitplane1, bitplane2, bitplane3, bitplane4);
            return tile;
            //throw new NotImplementedException();
            //byte[] bitplane1 = new byte[8];
            //byte[] bitplane2 = new byte[8];
            //byte[] bitplane3 = new byte[8];
            //byte[] bitplane4 = new byte[8];

            //for (byte i = 0; i < 8; i++)
            //{
            //    // Fetch 4 bytes to make up the color indexes for the current scanline.
            //    bitplane1[i] = tileBuffer[(i * 3)];
            //    bitplane2[i] = tileBuffer[(i * 3) + 1];
            //    bitplane3[i] = tileBuffer[(i * 3) + 2];
            //    bitplane4[i] = 0x00;
            //}
            //GraphicTile4Bpp tile = new GraphicTile4Bpp(bitplane1, bitplane2, bitplane3, bitplane4);
            //return tile;

            //// Create a tile object.
            //GraphicTile4Bpp tile = new GraphicTile4Bpp(tileBuffer);

            //// Loop though the tile bytes and create color indexes.
            //for (byte i = 0; i < 8; i++)
            //{
            //    // Fetch 3 bytes to make up the color indexes for the current scanline.
            //    byte b1 = tile.buffer[i * 3];
            //    byte b2 = tile.buffer[i * 3 + 1];
            //    byte b3 = tile.buffer[i * 3 + 2];

            //    // Decompressing from a 3BPP tile, so byte 4 will always be zero.
            //    tile.CreateScanLine(i, false, b1, b2, b3, 0x00);
            //}

            //return tile;
        }

        /// <summary>
        /// Creates a new <see cref="GraphicTile4Bpp"/> object using the specified 4BPP byte array.
        /// </summary>
        /// <param name="tileBuffer">An array of bytes making up 4BPP image data.</param>
        /// <returns>A <see cref="GraphicTile4Bpp"/> object containing the decompressed image data.</returns>
        public static GraphicTile4Bpp From4BppTile(Span<byte> tileBuffer)
        {
            byte[] bitplane1 = new byte[8];
            byte[] bitplane2 = new byte[8];
            byte[] bitplane3 = new byte[8];
            byte[] bitplane4 = new byte[8];

            for (byte i = 0; i < 8; i++)
            {
                // Fetch 4 bytes to make up the color indexes for the current scanline.
                bitplane1[i] = tileBuffer[i * 2];
                bitplane2[i] = tileBuffer[i * 2 + 1];
                bitplane3[i] = tileBuffer[i * 2 + 0x10];
                bitplane4[i] = tileBuffer[i * 2 + 0x11];
            }
            GraphicTile4Bpp tile = new GraphicTile4Bpp(bitplane1, bitplane2, bitplane3, bitplane4);
            return tile;
        }

        /// <summary>
        /// Returns a string that represents the color matrix of the tile.
        /// </summary>
        /// <returns>A string that represents the color matrix of the tile.</returns>
        public override string ToString()
        {
            if (this.scanlines.Length == 0) { return string.Empty; }
            // String Builder for the entire string object.
            StringBuilder sb = new StringBuilder();
            // String Builder for the current scanline.
            StringBuilder currentLine = new StringBuilder();

            // Loop through the scanlines.
            for (int i = 0; i < 8; i++)
            {
                // Clear the scanline string builder.
                currentLine.Clear();

                // Loop though each pixel of the scanline.
                for (int j = 0; j < 8; j++)
                {
                    // Add a hexidecimal representation of the pixel to the scanline string.
                    currentLine.Append(this.scanlines[i][j].ToString("X2"));

                    // Add a space if we haven't reached the end of the scanline yet.
                    if (j < 7) { currentLine.Append(" "); }
                }

                // Add the scanline to the string object.
                sb.AppendLine(currentLine.ToString());
            }

            return sb.ToString();
        }

        private void CreateScanlines(bool mirrorPalette = false)
        {
            // Loop through each bit in the bitplanes.
            // The MSB of each byte added together will create a color index for each pixel on the current scanline.
            // Shift right one on each byte to obtain the color index of the next pixel drawn on the line.
            for (int i = 0; i < 8; i++) { this.scanlines[i] = new byte[8]; }
            for (byte rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                for (byte columnIndex = 0; columnIndex < 8; columnIndex++)
                {
                    // Get the active color index bit from each byte of the scanline.
                    byte bit1 = (byte)((this.bitplanes[0][columnIndex] >> 7 - rowIndex) & 0x01);
                    byte bit2 = (byte)((this.bitplanes[1][columnIndex] >> 7 - rowIndex) & 0x01);
                    byte bit3 = (byte)((this.bitplanes[2][columnIndex] >> 7 - rowIndex) & 0x01);
                    byte bit4 = (byte)((this.bitplanes[3][columnIndex] >> 7 - rowIndex) & 0x01);

                    // Create the color index.
                    byte colorIndex = (byte)(bit1 + (bit2 << 1) + (bit3 << 2) + (bit4 << 3));

                    // If mirroring is enabled, change indexes with the value of 7 to the value of 0. (Transparency)
                    if (mirrorPalette && colorIndex == 7) { colorIndex = 0; }

                    this.scanlines[rowIndex][columnIndex] = colorIndex;
                }
            }
        }

        public byte[][] GetScanlines()
        {
            return this.scanlines;
        }
    }
}