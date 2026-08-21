using System;

#nullable enable

namespace ManaMagic.Core.Maps
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 00: 201 - None
    /// 01: 33  - SpecialTile
    /// 02: 3
    /// 04: 172 - OnEnter
    /// 08: 4
    /// 10: 10
    /// 20: 10
    /// 40: 10
    /// 80: 10
    /// </remarks>
    [Flags]
    public enum MapEventOptions : byte
    {
        None = 0x00,
        SpecialTile = 0x01,
        Unknown02 = 0x02,
        OnEnter = 0x04,
        Unknown08 = 0x08,
        Unknown10 = 0x10,
        Unknown20 = 0x20,
        Unknown40 = 0x40,
        Unknown80 = 0x80,
        //All = SpecialTile | Unknown02 | OnEnter | Unknown08 | Unknown10 | Unknown20 | Unknown40 | Unknown80,
    }
}