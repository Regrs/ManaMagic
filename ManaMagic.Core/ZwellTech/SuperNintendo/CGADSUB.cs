using System;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Represents the SNES CGADSUB (Color Math Designation) register.
    /// </summary>
    /// <remarks>Address: $2131</remarks>
    [Flags]
    public enum CGADSUB : byte
    {
        None = 0B_0000_0000,
        EnableBackground01 = 0B_0000_0001,
        EnableBackground02 = 0B_0000_0010,
        EnableBackground03 = 0B_0000_0100,
        EnableBackground04 = 0B_0000_1000,
        EnableObjects = 0B_0001_0000,
        EnableBackdrop = 0B_0010_0000,
        HalfColorMath = 0B_0100_0000,
        Subtractive = 0B_1000_0000,
    }
}
/*
    7  bit  0
    ---- ----
    MHBO 4321
    |||| ||||
    |||| |||+- BG1 color math enable
    |||| ||+-- BG2 color math enable
    |||| |+--- BG3 color math enable
    |||| +---- BG4 color math enable
    |||+------ OBJ color math enable (palettes 4-7 only)
    ||+------- Backdrop color math enable
    |+-------- Half color math
    +--------- Operator type (0 = add, 1 = subtract)
*/