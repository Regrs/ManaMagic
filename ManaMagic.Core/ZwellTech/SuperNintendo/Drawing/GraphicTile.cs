#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Represents the base class for all Super Nintendo tile based graphics.
    /// </summary>
    public abstract class GraphicTile
    {
        /// <summary>
        /// The 2D-array of bytes that contains the scanlines for this tile.
        /// </summary>
        protected readonly byte[][] scanlines;

        /// <summary>
        /// Gets the type of the tile.
        /// </summary>
        public TileType TileType { get; init; } = TileType.Unknown;

        /// <summary>
        /// Gets the size, in pixels, of the tile.
        /// </summary>
        public TileSize Size { get; init; } = TileSize.Size8X8;

        /// <summary>
        /// Gets the byte at the specified coordinate pair.
        /// </summary>
        /// <param name="x">The x-coordinate of the byte to retrieve.</param>
        /// <param name="y">The y-coordinate of the byte to retrieve.</param>
        /// <returns>The byte at the specified coordinate pair.</returns>
        public byte this[int x, int y]
        {
            get { return this.scanlines[x][y]; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="GraphicTile"/> class with the specified height and width.
        /// </summary>
        /// <param name="width">The width of the tile in bytes.</param>
        /// <param name="height">The height of the tile in bytes.</param>
        protected GraphicTile(int width, int height)
        {
            this.scanlines = new byte[width][];
            for (int i = 0; i < this.scanlines.Length; i++)
            {
                this.scanlines[i] = new byte[height];
            }
        }
    }
}