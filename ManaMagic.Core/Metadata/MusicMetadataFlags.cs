using System;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Represents metadata flags for a <see cref="MusicMetadata"/> object.
    /// </summary>
    [Flags]
    public enum MusicMetadataFlags
    {
        /// <summary>
        /// No metadata flags.
        /// </summary>
        None,
        /// <summary>
        /// The music track has no official name.
        /// </summary>
        UnnamedTrack,
        /// <summary>
        /// The metadata entry is dummied out and not used in-game.
        /// </summary>
        DummiedOut
    }
}