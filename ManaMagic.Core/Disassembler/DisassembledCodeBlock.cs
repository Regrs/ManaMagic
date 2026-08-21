using System.Collections.Generic;
using System.Text;

#nullable enable

namespace ManaMagic.Disassembler
{
    public sealed record DisassembledCodeBlock(AssemblyInstructionSetType InstructionSetType, IReadOnlyList<DisassembledAssemblyInstruction> Instructions)
    {
        public bool SpaceAfterReturn { get; set; } = true;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(128);
            foreach (DisassembledAssemblyInstruction instruction in this.Instructions)
            {
                sb.AppendLine(instruction.ToString());
                if (instruction.OpCode == 0x6F) { sb.AppendLine(); }
            }
            return sb.ToString();
        }
    }
}