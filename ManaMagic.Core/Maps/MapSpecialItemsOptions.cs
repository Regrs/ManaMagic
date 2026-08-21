using System;

#nullable enable

namespace ManaMagic.Core.Maps
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 00: 86 - None
    /// 01: 31
    /// 02: 32
    /// 04: 27
    /// 08: 26
    /// 10: 4
    /// 20: 213
    /// 40: 137 - MagicRopeAllowed
    /// 80: 103 - FlammieDrumAllowed
    /// </remarks>
    [Flags]
    public enum MapSpecialItemsOptions : byte
    {
        None = 0x00,
        //Unknown01 = 0x01,
        //MagicRopeAllowed = 0x02,
        //Unknown04 = 0x04,
        //Unknown08 = 0x08,
        //Unknown10 = 0x10,
        Unknown20 = 0x20,
        MagicRopeAllowed = 0x40,
        FlammieDrumAllowed = 0x80,
        //All = Unknown01 | Unknown02 | Unknown04 | Unknown08 | Unknown10 | MagicRopeAllowed | Unknown40 | FlammieDrumAllowed,
    }
}