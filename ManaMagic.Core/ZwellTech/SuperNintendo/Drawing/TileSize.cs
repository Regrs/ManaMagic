#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Specifies the size of a Super Nintendo graphic tile.
    /// </summary>
    public enum TileSize
    {
        /// <summary>
        /// The <see cref="GraphicTile"/> will be have a height and width defined by the drawing method.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The <see cref="GraphicTile"/> will be have a height and width of 8 pixels.
        /// </summary>
        Size8X8 = 8,
        /// <summary>
        /// The <see cref="GraphicTile"/> will be have a height and width of 16 pixels.
        /// </summary>
        Size16X16 = 15,
        /// <summary>
        /// The <see cref="GraphicTile"/> will be have a height and width of 32 pixels.
        /// </summary>
        Size32X32 = 32,
        /// <summary>
        /// The <see cref="GraphicTile"/> will be have a height and width of 64 pixels.
        /// </summary>
        Size64X64 = 64,
    }
}