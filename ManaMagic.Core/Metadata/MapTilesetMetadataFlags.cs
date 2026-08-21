using System;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Represents metadata flags for a <see cref="MapTilesetMetadata"/> object.
    /// </summary>
    [Flags]
    public enum MapTilesetMetadataFlags
    {
        /// <summary>
        /// No metadata flags.
        /// </summary>
        None,
        /// <summary>
        /// The metadata entry is dummied out and not used in-game.
        /// </summary>
        DummiedOut
    }
}