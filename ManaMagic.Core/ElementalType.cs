using System;

#nullable enable

namespace ManaMagic.Core
{
    [Flags]
    public enum ElementalType : byte
    {
        None = 0B_0000_0000,
        Gnome = 0B_0000_0001,
        Sylphid = 0B_0000_0010,
        Undine = 0B_0000_0100,
        Salamando = 0B_0000_1000,
        Shade = 0B_0001_0000,
        Lumina = 0B_0010_0000,
        Luna = 0B_0100_0000,
        Dryad = 0B_1000_0000,
    }
}