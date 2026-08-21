using System.Collections.Generic;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapPiece
    {
        internal static MapPiece Empty { get; } = new MapPiece(0, 1, 1, new List<byte>() { 0 }, false);

        public ushort Index { get; }
        public byte Width { get; }
        public byte Height { get; }
        public DataTable<byte> TileIdTable { get; }
        public bool HasReplaceMapPieceCommand { get; }

        public MapPiece(ushort index, byte width, byte height, IReadOnlyList<byte> tileIds, bool hasReplaceMapPieceCommand)
        {
            this.Index = index;
            this.Width = width;
            this.Height = height;
            this.TileIdTable = new DataTable<byte>(tileIds);
            this.HasReplaceMapPieceCommand = hasReplaceMapPieceCommand;
        }
    }
}