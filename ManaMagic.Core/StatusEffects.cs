using System;

#nullable enable

namespace ManaMagic.Core
{
    [Flags]
    public enum StatusEffects : ushort
    {
        None = 0B_0000_0000_0000_0000,
        Stun = 0B_0000_0000_0000_0001,
        Injured = 0B_0000_0000_0000_0010,
        Tangle = 0B_0000_0000_0000_0100,
        Paralyze = 0B_0000_0000_0000_1000,
        KnockedOut = 0B_0000_0000_0001_0000,
        Frosty = 0B_0000_0000_0010_0000,
        Petrify = 0B_0000_0000_0100_0000,
        Confusion = 0B_0000_0000_1000_0000,

        Balloon = 0B_0000_0001_0000_0000,
        Pygmy = 0B_0000_0010_0000_0000,
        Barrel = 0B_0000_0100_0000_0000,
        Transform = 0B_0000_1000_0000_0000,
        Moogle = 0B_0001_0000_0000_0000,
        Poison = 0B_0010_0000_0000_0000,
        Engulf = 0B_0100_0000_0000_0000,
        Ghost = 0B_1000_0000_0000_0000,
    }
}
/*
8000: Ghost
4000: Engulf
2000: Poison
1000: Moogle
0800: Transform
0400: Barrel
0200: Pygmy
0100: Balloon
0080: Confusio
n0040: Petrify
0020: Frosty
0010: Knocked Out
0008: Paralyze (Dummied Out)
0004: Tangle
0002: Injured
0001: Stun
*/