#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Represents metadata about a music track in Secret Of Mana.
    /// </summary>
    public sealed record MusicMetadata
    {
        /// <summary>
        /// Gets the index of the music track.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// Gets the name of the music track.
        /// </summary>
        public string TrackName { get; init; }

        /// <summary>
        /// Gets the metadata flags for the music track.
        /// </summary>
        public MusicMetadataFlags Flags { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicMetadata"/> class with the specified index, track name and metadata flags.
        /// </summary>
        /// <param name="index">The index of the music track.</param>
        /// <param name="trackName">The name of the music track.</param>
        /// <param name="flags">The metadata flags for the music track.</param>
        public MusicMetadata(int index, string trackName, MusicMetadataFlags flags)
        {
            this.Index = index;
            this.TrackName = trackName;
            this.Flags = flags;
        }
    }
}