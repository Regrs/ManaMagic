using System.Collections.Generic;

#nullable enable

namespace ManaMagic.Disassembler
{
    public record DisassembledAssemblyInstruction : AssemblyInstruction
    {
        public int InstructionAddress { get; }
        public IReadOnlyList<byte> Operands { get; }
        public int RelativeOffset { get; }

        public DisassembledAssemblyInstruction(int instructionAddress, AssemblyInstruction instruction, IReadOnlyList<byte> operands, int relativeOffset) : base(instruction.OpCode, instruction.Mnemonic, instruction.OperandCount, instruction.AddressingMode, instruction.ArgumentFormatString)
        {
            this.InstructionAddress = instructionAddress;
            this.Operands = operands;
            this.RelativeOffset = relativeOffset;
        }

        protected int ConvertOperandIfRelativeAddressing(int instructionOffset, int operand, int operandCount)
        {
            if (this.AddressingMode == AssemblyAddressingMode.Relative ||
                this.AddressingMode == AssemblyAddressingMode.DirectPageBitRelative)
            {
                operand = (instructionOffset + 1 + operandCount) + this.RelativeOffset;
            }
            return operand;
        }
    }
}