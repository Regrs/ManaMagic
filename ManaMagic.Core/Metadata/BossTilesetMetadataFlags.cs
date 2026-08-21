using System;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Represents metadata flags for a <see cref="BossTilesetMetadataFlags"/> object.
    /// </summary>
    [Flags]
    public enum BossTilesetMetadataFlags
    {
        /// <summary>
        /// No metadata flags.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// The tileset is compressed with the boss LZ77 and then with the general use LZ77.
        /// </summary>
        DualCompressed = 0x01,
        /// <summary>
        /// The tileset is made up of Mode 7 tiles.
        /// </summary>
        Mode7 = 0x02,
        /// <summary>
        /// Frame offsets into this tileset are offset by the total size of the boss shadow tiles (0x20).
        /// </summary>
        OffsetByShadowTiles = 0x04,
        /// <summary>
        /// The metadata entry is dummied out and not used in-game.
        /// </summary>
        DummiedOut = 0x08,
    }
}