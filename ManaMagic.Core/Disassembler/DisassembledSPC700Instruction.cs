using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace ManaMagic.Disassembler
{
    public sealed record DisassembledSPC700Instruction : DisassembledAssemblyInstruction
    {
        public DisassembledSPC700Instruction(int instructionAddress, AssemblyInstruction instruction, IReadOnlyList<byte> operands, int relativeOffset) : base(instructionAddress, instruction, operands, relativeOffset) { }


        public override string ToString()
        {
            int bank = this.InstructionAddress >> 16;
            int offset = this.InstructionAddress & 0x0000FFFF;
            if (this.OperandCount == 1)
            {
                int operand1 = this.ConvertOperandIfRelativeAddressing(offset, this.Operands[0], this.OperandCount);
                return $"{bank:X2}/{offset:X4}: {this.OpCode:X2}{this.Operands[0]:X2}            {this.Mnemonic} {string.Format(this.ArgumentFormatString, operand1)}";
            }
            else if (this.OperandCount == 2)
            {
                (int Operand1, int Operand2) operands = this.GetOperands(offset);
                return $"{bank:X2}/{offset:X4}: {this.OpCode:X2}{this.Operands[0]:X2}{this.Operands[1]:X2}          {this.Mnemonic} {string.Format(this.ArgumentFormatString, operands.Operand1, operands.Operand2)}";
            }
            return $"{bank:X2}/{offset:X4}: {this.OpCode:X2}              {this.Mnemonic} {this.ArgumentFormatString}";
        }

        private (int Operand1, int Operand2) GetOperands(int offset)
        {
            if (SPC700Disassembler.OpCodesWithFirstToLastOperands.Contains(this.OpCode))
            {
                ;
                return (this.Operands[0], this.ConvertOperandIfRelativeAddressing(offset, this.Operands[1], this.OperandCount));
            }
            return (this.Operands[1], this.ConvertOperandIfRelativeAddressing(offset, this.Operands[0], this.OperandCount));
        }
    }
}