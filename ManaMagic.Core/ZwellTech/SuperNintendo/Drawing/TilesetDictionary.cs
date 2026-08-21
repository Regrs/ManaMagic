using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Represents a read-only collection of Super Nintendo tilesets.
    /// </summary>
    public sealed class TilesetDictionary : ReadOnlyDictionary<byte, Tileset>
    {
        /// <summary>
        /// Represents an empty <see cref="TilesetDictionary"/>.
        /// </summary>
        public static TilesetDictionary Empty { get; } = new TilesetDictionary(new Dictionary<byte, Tileset>());

        /// <summary>
        /// Creates a new instance of the <see cref="TilesetDictionary"/> class that is a wrapper around the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to wrap.</param>
        public TilesetDictionary(IDictionary<byte, Tileset> dictionary) : base(dictionary) { }
    }
}