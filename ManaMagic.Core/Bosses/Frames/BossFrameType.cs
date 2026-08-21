#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents a boss frame's type.
    /// </summary>
    public enum BossFrameType
    {
        /// <summary>
        /// The frame is a sprite frame.
        /// </summary>
        Sprite,
        /// <summary>
        /// The frame is a background frame.
        /// </summary>
        Background,
        /// <summary>
        /// The frame is a Mode 7 background frame.
        /// </summary>
        Mode7,
    }
}