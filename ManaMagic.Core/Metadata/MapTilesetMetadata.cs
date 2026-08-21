#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Represents metadata about a map tileset in Secret Of Mana.
    /// </summary>
    public sealed record MapTilesetMetadata
    {
        /// <summary>
        /// Gets the index of the tileset.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// Gets the name of the tileset.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the default palette index in the set for the tileset.
        /// </summary>
        public byte DefaultPaletteIndex { get; init; }

        /// <summary>
        /// Gets the default palette set Id for the tileset.
        /// </summary>
        public byte DefaultPaletteSet { get; init; }

        /// <summary>
        /// Gets the metadata flags for the tileset.
        /// </summary>
        public MapTilesetMetadataFlags Flags { get; init; }

        /// <summary>
        /// Gets if this tileset is dummied out and not used in-game.
        /// </summary>
        public bool IsDummiedOut { get { return this.Flags.HasFlag(MapTilesetMetadataFlags.DummiedOut); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicMetadata"/> class with the specified index, name, default palette index and set Ids and metadata flags.
        /// </summary>
        /// <param name="index">The index of the tileset.</param>
        /// <param name="name">The name of the tileset.</param>
        /// <param name="defaultPaletteIndex">The default palette index of the tileset.</param>
        /// <param name="defaultPaletteSet">The default palette set of the tileset.</param>
        /// <param name="flags">The metadata flags for the tileset.</param>
        public MapTilesetMetadata(int index, string name, byte defaultPaletteSet, byte defaultPaletteIndex, MapTilesetMetadataFlags flags)
        {
            this.Index = index;
            this.Name = name;
            this.DefaultPaletteSet = defaultPaletteSet;
            this.DefaultPaletteIndex = defaultPaletteIndex;
            this.Flags = flags;
        }
    }
}