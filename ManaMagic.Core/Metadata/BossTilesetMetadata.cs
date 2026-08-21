using System;
using System.Collections.Generic;
using ManaMagic.Core.Bosses;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Represents metadata about a boss tileset in Secret Of Mana.
    /// </summary>
    public sealed record BossTilesetMetadata
    {
        public static BossTilesetMetadata Empty { get; } = new BossTilesetMetadata(0, string.Empty, (0xFF, 0xFF, 0xFF), (0, 0), BossFamily.Plant, BossTilesetMetadataFlags.None);

        /// <summary>
        /// Gets the index of the tileset.
        /// </summary>
        public byte Index { get; init; }

        /// <summary>
        /// Gets the name of the tileset.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the palette Ids used to draw the tileset.
        /// </summary>
        public (byte Primary, byte Secondary, byte Tertiary) PaletteIds { get; init; }

        /// <summary>
        /// Gets the Ids of the auxiliaryIds needed to draw the tileset.
        /// </summary>
        public (byte Primary, byte Secondary) AuxiliaryIds { get; init; }

        /// <summary>
        /// Gets the family of the tileset.
        /// </summary>
        public BossFamily Family { get; init; }

        /// <summary>
        /// Gets the metadata flags for the tileset.
        /// </summary>
        public BossTilesetMetadataFlags Flags { get; init; }

        /// <summary>
        /// Gets if the tileset is compressed with both the boss and general use LZ77 compressors.
        /// </summary>
        public bool IsDualCompressed { get { return this.Flags.HasFlag(BossTilesetMetadataFlags.DualCompressed); } }

        /// <summary>
        /// Gets if the tileset consists of Mode 7 tiles.
        /// </summary>
        public bool IsMode7 { get { return this.Flags.HasFlag(BossTilesetMetadataFlags.Mode7); } }

        /// <summary>
        /// Gets if frames drawn with this tileset are offset by the length of the boss shadow tiles.
        /// </summary>
        public bool IsOffsetByShadowTiles { get { return this.Flags.HasFlag(BossTilesetMetadataFlags.OffsetByShadowTiles); } }

        /// <summary>
        /// Gets if this tileset is dummied out and not used in-game.
        /// </summary>
        public bool IsDummiedOut { get { return this.Flags.HasFlag(BossTilesetMetadataFlags.DummiedOut); } }

        /// <summary>
        /// Gets a value indicating if this boss tileset can be drawn with multiple auxiliary tilesets.
        /// </summary>
        public bool SwitchableAuxiliaryTileset { get { return this.AuxiliaryIds.Primary != ManaMetadata.NullBossTilesetAuxiliaryId && this.AuxiliaryIds.Secondary != ManaMetadata.NullBossTilesetAuxiliaryId; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicMetadata"/> class with the specified index, name, paletteId list, auxiliary Ids, boss family and metadata flags.
        /// </summary>
        /// <param name="index">The index of the tileset.</param>
        /// <param name="name">The name of the tileset.</param>
        /// <param name="paletteIds">The palette Ids used to draw the tileset.</param>
        /// <param name="auxiliaryIds">The Ids of additional tilesets required for this tileset.</param>
        /// <param name="family">The <see cref="BossFamily"/> that this tileset belongs.</param>
        /// <param name="flags">The metadata flags for the tileset.</param>
        public BossTilesetMetadata(byte index, string name, (byte Primary, byte Secondary, byte Tertiary) paletteIds, (byte Primary, byte Secondary) auxiliaryIds, BossFamily family, BossTilesetMetadataFlags flags)
        {
            this.Index = index;
            this.Name = name;
            this.PaletteIds = paletteIds;
            this.AuxiliaryIds = auxiliaryIds;
            this.Family = family;
            this.Flags = flags;

        }
    }
}