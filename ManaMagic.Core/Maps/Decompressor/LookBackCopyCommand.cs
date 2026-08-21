#nullable enable

namespace ManaMagic.Core.Maps.Decompressor
{
    internal enum LookBackCopyCommand
    {
        RowLargeCopy = 0x00,
        RowSmallCopy = 0x01,
        LookBackCopy = 0x02,
    }
}