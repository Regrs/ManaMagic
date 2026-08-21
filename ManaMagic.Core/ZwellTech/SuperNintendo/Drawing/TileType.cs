#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Specifies the type of a Super Nintendo graphic tile.
    /// </summary>
    public enum TileType
    {
        /// <summary>
        /// The <see cref="GraphicTile"/> has an unknown type.
        /// </summary>
        Unknown,
        /// <summary>
        /// The <see cref="GraphicTile"/> was encoded as a 3BPP tile.
        /// </summary>
        ThreeBitsPerPixel,
        /// <summary>
        /// The <see cref="GraphicTile"/> was encoded as a 4BPP tile.
        /// </summary>
        FourBitsPerPixel,
        /// <summary>
        /// The <see cref="GraphicTile"/> is a Mode7 tile.
        /// </summary>
        Mode7,
    }
}