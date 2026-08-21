#nullable enable

namespace ManaMagic.Core.Events
{
    public enum EventOpCodeDirection : byte
    {
        North = 0x00,
        South = 0x40,
        East = 0x80,
        West = 0xC0,
    }
}