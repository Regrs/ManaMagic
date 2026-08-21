using System;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Options for drawing a boss frame part.
    /// </summary>
    [Flags]
    public enum BossFrameFlags : byte
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Add 0x0100 to the tile Id for the part.
        /// </summary>
        ExtendTileId = 0x01,
        /// <summary>
        /// Draw the frame part using palette index 01.
        /// </summary>
        UsePalette1 = 0x02,
        /// <summary>
        /// Draw the frame part using palette index 02.
        /// </summary>
        UsePalette2 = 0x04,
        /// <summary>
        /// Draw the frame part using palette index 03.
        /// </summary>
        UsePalette3 = BossFrameFlags.UsePalette1 | BossFrameFlags.UsePalette2,
        /// <summary>
        /// This bits purpose is unknown. Palette related, needed to correctly display palette 3 on a frame.
        /// </summary>
        Unknown08 = 0x08,
        /// <summary>
        /// This bit purpose is unknown.
        /// </summary>
        Unknown10 = 0x10,
        /// <summary>
        /// Draw the frame part as a 32x32 tile.
        /// </summary>
        Draw32x32Tile = 0x20,
        /// <summary>
        /// Flip the frame part horizontally.
        /// </summary>
        HorizontalFlip = 0x40,
        /// <summary>
        /// Flip the frame part vertically.
        /// </summary>
        VerticalFlip = 0x80,
    }
}