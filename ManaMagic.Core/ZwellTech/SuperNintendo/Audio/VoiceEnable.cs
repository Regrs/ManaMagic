using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    [Flags]
    public enum VoiceEnable : byte
    {
        Off = 0,
        Voice0 = 0B_0000_0001,
        Voice1 = 0B_0000_0010,
        Voice2 = 0B_0000_0100,
        Voice3 = 0B_0000_1000,
        Voice4 = 0B_0001_0000,
        Voice5 = 0B_0010_0000,
        Voice6 = 0B_0100_0000,
        Voice7 = 0B_1000_0000,
        All = Voice0 | Voice1 | Voice2 | Voice3 | Voice4 | Voice5 | Voice6 | Voice7,
    }
}