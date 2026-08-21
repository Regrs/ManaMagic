#nullable enable

namespace ManaMagic.Disassembler
{
    public record AssemblyInstruction(byte OpCode, string Mnemonic, byte OperandCount, AssemblyAddressingMode AddressingMode, string ArgumentFormatString);
}