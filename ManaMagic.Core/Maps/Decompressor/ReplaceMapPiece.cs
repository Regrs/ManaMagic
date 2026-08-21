using System.Collections.Generic;

#nullable enable

namespace ManaMagic.Core.Maps.Decompressor
{
    internal sealed class ReplaceMapPiece
    {
        public byte Height { get; set; } = 0;
        public byte Width { get; set; } = 0;
        public List<byte> Instructions { get; } = new List<byte>();
        public List<int> InsertionIndexes { get; } = new List<int>();
        public bool Initialized { get { return this.Height > 0 && this.Width > 0; } }
    }
}