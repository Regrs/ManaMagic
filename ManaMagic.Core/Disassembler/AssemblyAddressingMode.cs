#nullable enable

namespace ManaMagic.Disassembler
{
    public enum AssemblyAddressingMode
    {
        Implied = 0,            // NOP
        Relative,               // r
        Immediate,              // #imm
        ImmediateToDirectPage,  // dp,#imm
        Indirect,               // (X)
        IndirectAutoIncrement,  // (X)+
        IndirectXIndexed,       // [dp]+X
        IndirectYIndexed,       // [dp]+Y
        IndirectToIndirect,     // (X),(Y)
        Absolute,               // labs
        AbsoluteXIndexed,       // labs+X
        AbsoluteYIndexed,       // labs+Y
        AbsoluteBooleanBit,     // mem.bit
        DirectPage,             // dp
        DirectPageXIndexed,     // dp+X
        DirectPageYIndexed,     // dp+Y
        DirectPageBit,          // dp.bit
        DirectPageBitRelative,  // dp.bit,r
        DirectPageToDirectPage, // dp,dp
    }
}