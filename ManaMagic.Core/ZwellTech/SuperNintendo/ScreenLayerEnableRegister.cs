using System;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Represents the SNES TM (Main Screen Layer Enable) and TS (Sub Screen Layer Enable) registers.
    /// </summary>
    /// <remarks>Address: $212C/$212D</remarks>
    [Flags]
    public enum ScreenLayerEnableRegister : byte
    {
        None = 0B_0000_0000,
        EnableBackground01 = 0B_0000_0001,
        EnableBackground02 = 0B_0000_0010,
        EnableBackground03 = 0B_0000_0100,
        EnableBackground04 = 0B_0000_1000,
        EnableObjects = 0B_0001_0000,
        Reserved20 = 0B_0010_0000,
        Reserved40 = 0B_0100_0000,
        Reserved80 = 0B_1000_0000,
    }
}