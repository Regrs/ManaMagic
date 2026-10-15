using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    [Flags]
    public enum SMPControlSettings : byte
    {
        None = 0,
        EnableTimer0 = 0B_0000_0001,
        EnableTimer1 = 0B_0000_0010,
        EnableTimer2 = 0B_0000_0100,

        ClearCPUInput1 = 0B_0001_0000,
        ClearCPUInput2 = 0B_0010_0000,

        EnableBootRom = 0B_1000_0000,
    }
}