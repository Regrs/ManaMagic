#nullable enable

namespace ManaMagic.Core
{
    public sealed record UShortValue : GenericValue<ushort>
    {
        public UShortValue(byte index, ushort value, bool userModified = false) : base(index, value, userModified) { }
    }
}