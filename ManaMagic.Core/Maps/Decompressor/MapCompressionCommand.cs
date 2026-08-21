#nullable enable

namespace ManaMagic.Core.Maps.Decompressor
{
    internal enum MapCompressionCommand
    {
        Tile = 0x00,
        ShortLookBackTileAndRepeat = 0xC0,
        LongLookBackTileAndRepeat = 0xD0,
        CopyFromLookBack = 0xE0,
        FillWithIncrement = 0xF0,

        ReplacePieceIndexA = 0xF8,
        ReplacePieceIndexB = 0xF9,
        ReplacePieceIndexC = 0xFA,
        ReplacePieceIndexD = 0xFB,
        ReplacePieceIndexE = 0xFC,
        ReplacePieceIndexF = 0xFD,
        AdvanceReplacePieceIndex = 0xFE,
        EndReplacePieces = 0xFF,
    }
}