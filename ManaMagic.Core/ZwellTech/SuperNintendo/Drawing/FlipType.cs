using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Specifies the axis used to flip the <see cref="GraphicTile"/>.
    /// </summary>
    [Flags]
    public enum FlipType
    {
        /// <summary>
        /// Specifies no flipping.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Specifies a horizontal flip.
        /// </summary>
        HorizontalFlip = 0x01,
        /// <summary>
        /// Specifies a vertical flip.
        /// </summary>
        VerticalFlip = 0x02,
        /// <summary>
        /// Specifies a horizontal and vertical flip.
        /// </summary>
        HorizontalVerticalFlip = HorizontalFlip | VerticalFlip,
    }
}