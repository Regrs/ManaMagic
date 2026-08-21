using System;

#nullable enable

namespace ManaMagic.Core.Maps
{
    [Flags]
    public enum LayerLoadMode : byte
    {
        LoadLayer1AsLayer2 = 0x40,
        LoadLayer2AsLayer2 = 0x80,
    }
}