using System.Collections.Generic;
using System.Diagnostics;
using ZwellTech.SuperNintendo;

#nullable enable
//https://emudev.de/q00-snes/spc700-the-audio-processor/
namespace ManaMagic.Disassembler
{
    public sealed class SPC700Disassembler
    {
        private readonly static Dictionary<byte, AssemblyInstruction> SPC700InstructionSet = new Dictionary<byte, AssemblyInstruction>()
        {
            // Move Operators
            { 0xE8, new AssemblyInstruction(0xE8, "MOV", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0xE4, new AssemblyInstruction(0xE4, "MOV", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0xF4, new AssemblyInstruction(0xF4, "MOV", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0xE5, new AssemblyInstruction(0xE5, "MOV", 2, AssemblyAddressingMode.Absolute, "A, ${1:X2}{0:X2}") },
            { 0xF5, new AssemblyInstruction(0xF5, "MOV", 2, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}{1:X2}+X") },
            { 0xF6, new AssemblyInstruction(0xF6, "MOV", 2, AssemblyAddressingMode.AbsoluteYIndexed, "A, ${0:X2}{1:X2}+Y") },
            { 0xE6, new AssemblyInstruction(0xE6, "MOV", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0xBF, new AssemblyInstruction(0xBF, "MOV", 0, AssemblyAddressingMode.IndirectAutoIncrement, "A, (X)+") },
            { 0xE7, new AssemblyInstruction(0xE7, "MOV", 1, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}+X]") },
            { 0xF7, new AssemblyInstruction(0xF7, "MOV", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}]+Y") },

            { 0xF8, new AssemblyInstruction(0xF8, "MOV", 1, AssemblyAddressingMode.DirectPage, "X, ${0:X2}") },
            { 0xF9, new AssemblyInstruction(0xF9, "MOV", 1, AssemblyAddressingMode.DirectPageYIndexed, " X, ${0:X2}+Y") },
            { 0xFA, new AssemblyInstruction(0xFA, "MOV", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0xFB, new AssemblyInstruction(0xFB, "MOV", 1, AssemblyAddressingMode.DirectPageXIndexed, "Y, ${0:X2}+X") },
            { 0xBD, new AssemblyInstruction(0xBD, "MOV", 0, AssemblyAddressingMode.Implied, "SP, X") },
            { 0x8D, new AssemblyInstruction(0x8D, "MOV", 1, AssemblyAddressingMode.Immediate, "Y, #${0:X2}") },
            { 0xC5, new AssemblyInstruction(0xC5, "MOV", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}, A") },
            { 0xCB, new AssemblyInstruction(0xCB, "MOV", 1, AssemblyAddressingMode.DirectPage, "${0:X2}, Y") },
            { 0xCC, new AssemblyInstruction(0xCC, "MOV", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}, Y") },
            { 0xCD, new AssemblyInstruction(0xCD, "MOV", 1, AssemblyAddressingMode.Immediate, "X, #${0:X2}") },
            { 0xD4, new AssemblyInstruction(0xD4, "MOV", 1, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X, A") },
            { 0xD5, new AssemblyInstruction(0xD5, "MOV", 2, AssemblyAddressingMode.AbsoluteXIndexed, "${0:X2}{1:X2}+X, A") },
            { 0xD6, new AssemblyInstruction(0xD6, "MOV", 2, AssemblyAddressingMode.AbsoluteYIndexed, "${0:X2}{1:X2}+Y, A") },
            { 0xD7, new AssemblyInstruction(0xD7, "MOV", 1, AssemblyAddressingMode.IndirectYIndexed, "[${0:X2}]+Y, A") },
            { 0xD8, new AssemblyInstruction(0xD8, "MOV", 1, AssemblyAddressingMode.DirectPage, "${0:X2}, X") },
            { 0xD9, new AssemblyInstruction(0xD9, "MOV", 1, AssemblyAddressingMode.DirectPageYIndexed, "${0:X2}+Y, X") },
            { 0xE9, new AssemblyInstruction(0xE9, "MOV", 2, AssemblyAddressingMode.Absolute, " X, ${0:X2}{1:X2}") },
            { 0xEB, new AssemblyInstruction(0xEB, "MOV", 1, AssemblyAddressingMode.DirectPage, "Y, ${0:X2}") },
            { 0xEC, new AssemblyInstruction(0xEC, "MOV", 2, AssemblyAddressingMode.Absolute, " Y, ${0:X2}{1:X2}") },
            { 0xFD, new AssemblyInstruction(0xFD, "MOV", 0, AssemblyAddressingMode.Implied, "Y, A") },
            { 0xDD, new AssemblyInstruction(0xDD, "MOV", 0, AssemblyAddressingMode.Implied, "A, Y") },
            { 0xC9, new AssemblyInstruction(0xC9, "MOV", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}, X") },
            { 0xAF, new AssemblyInstruction(0xAF, "MOV", 0, AssemblyAddressingMode.IndirectAutoIncrement, "(X)+, A") },
            { 0x5D, new AssemblyInstruction(0x5D, "MOV", 0, AssemblyAddressingMode.Implied, "X, A") },
            { 0x7D, new AssemblyInstruction(0x7D, "MOV", 0, AssemblyAddressingMode.Implied, "A, X") },
            { 0xDB, new AssemblyInstruction(0xDB, "MOV", 1, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X, Y") },
            { 0xC7, new AssemblyInstruction(0xC7, "MOV", 1, AssemblyAddressingMode.IndirectXIndexed, "[${0:X2}+X], A") },
            { 0x8F, new AssemblyInstruction(0x8F, "MOV", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0xC4, new AssemblyInstruction(0xC4, "MOV", 1, AssemblyAddressingMode.DirectPage, "${0:X2}, A") },
            { 0xAA, new AssemblyInstruction(0xAA, "MOV1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "C, ${0:X2}.#${1:X2}") },
            { 0xCA, new AssemblyInstruction(0xCA, "MOV1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "${0:X2}.#${1:X2}, C") },
            { 0xBA, new AssemblyInstruction(0xBA, "MOVW", 1, AssemblyAddressingMode.DirectPage, "YA, ${0:X2}") },
            { 0xDA, new AssemblyInstruction(0xDA, "MOVW", 1, AssemblyAddressingMode.DirectPage, "${0:X2}, YA") },

            // CMP Operators
            { 0x3E, new AssemblyInstruction(0x3E, "CMP", 1, AssemblyAddressingMode.DirectPage, "X, ${0:X2}") },
            { 0x64, new AssemblyInstruction(0x64, "CMP", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0x65, new AssemblyInstruction(0x65, "CMP", 2, AssemblyAddressingMode.Absolute, "A, ${0:X2}{1:X2}") },
            { 0x66, new AssemblyInstruction(0x66, "CMP", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0x67, new AssemblyInstruction(0x67, "CMP", 1, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}+X]") },
            { 0x68, new AssemblyInstruction(0x68, "CMP", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0x69, new AssemblyInstruction(0x69, "CMP", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0x74, new AssemblyInstruction(0x74, "CMP", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0x75, new AssemblyInstruction(0x75, "CMP", 2, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}{1:X2}+X") },
            { 0x76, new AssemblyInstruction(0x76, "CMP", 2, AssemblyAddressingMode.AbsoluteYIndexed, "A, ${0:X2}{1:X2}+Y") },
            { 0x77, new AssemblyInstruction(0x77, "CMP", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}]+Y") },
            { 0x78, new AssemblyInstruction(0x78, "CMP", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0x79, new AssemblyInstruction(0x79, "CMP", 0, AssemblyAddressingMode.IndirectToIndirect, "(X), (Y)") },
            { 0xC8, new AssemblyInstruction(0xC8, "CMP", 1, AssemblyAddressingMode.Immediate, "X, #${0:X2}") },
            { 0x1E, new AssemblyInstruction(0x1E, "CMP", 2, AssemblyAddressingMode.Absolute, "X, ${0:X2}{1:X2}") },
            { 0x5E, new AssemblyInstruction(0x5E, "CMP", 2, AssemblyAddressingMode.Absolute, "Y, ${0:X2}{1:X2}") },
            { 0x7E, new AssemblyInstruction(0x7E, "CMP", 1, AssemblyAddressingMode.DirectPage, "Y, ${0:X2}") },
            { 0xAD, new AssemblyInstruction(0xAD, "CMP", 1, AssemblyAddressingMode.Immediate, "Y, #${0:X2}") },
            { 0x5A, new AssemblyInstruction(0x5A, "CMPW", 1, AssemblyAddressingMode.DirectPage, "YA, ${0:X2}") },

            // AND
            { 0x24, new AssemblyInstruction(0x24, "AND", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0x25, new AssemblyInstruction(0x25, "AND", 2, AssemblyAddressingMode.Absolute, "A, ${0:X2}{1:X2}") },
            { 0x28, new AssemblyInstruction(0x28, "AND", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0x29, new AssemblyInstruction(0x29, "AND", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0x34, new AssemblyInstruction(0x34, "AND", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0x38, new AssemblyInstruction(0x38, "AND", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0x4A, new AssemblyInstruction(0x4A, "AND1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "C, ${0:X2}.#${1:X2}") },

            // ADC
            { 0x84, new AssemblyInstruction(0x84, "ADC", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0x85, new AssemblyInstruction(0x85, "ADC", 1, AssemblyAddressingMode.Absolute, "A, ${0:X2}") },
            { 0x86, new AssemblyInstruction(0x86, "ADC", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0x87, new AssemblyInstruction(0x87, "ADC", 1, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}+X]") },
            { 0x88, new AssemblyInstruction(0x88, "ADC", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0x89, new AssemblyInstruction(0x89, "ADC", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0x95, new AssemblyInstruction(0x95, "ADC", 2, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}{1:X2}+X") },
            { 0x96, new AssemblyInstruction(0x96, "ADC", 2, AssemblyAddressingMode.AbsoluteYIndexed, "A, ${0:X2}{1:X2}+Y") },
            { 0x97, new AssemblyInstruction(0x97, "ADC", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}+Y]") },
            { 0x98, new AssemblyInstruction(0x98, "ADC", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0x99, new AssemblyInstruction(0x99, "ADC", 0, AssemblyAddressingMode.IndirectToIndirect, "(X), (Y)") },
            { 0x7A, new AssemblyInstruction(0x7A, "ADDW", 1, AssemblyAddressingMode.DirectPage, "YA, ${0:X2}") },
            { 0xEA, new AssemblyInstruction(0xEA, "NOT1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "${0:X2}.#${1:X2}") },

            // SBC
            { 0xA4, new AssemblyInstruction(0xA4, "SBC", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0xA5, new AssemblyInstruction(0xA5, "SBC", 2, AssemblyAddressingMode.Absolute, "A, ${0:X2}{1:X2}") },
            { 0xA6, new AssemblyInstruction(0xA6, "SBC", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0xA7, new AssemblyInstruction(0xA7, "SBC", 1, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}+X]") },
            { 0xA8, new AssemblyInstruction(0xA8, "SBC", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0xA9, new AssemblyInstruction(0xA9, "SBC", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0xB4, new AssemblyInstruction(0xB4, "SBC", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0xB5, new AssemblyInstruction(0xB5, "SBC", 1, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}+X") },
            { 0xB6, new AssemblyInstruction(0xB6, "SBC", 1, AssemblyAddressingMode.AbsoluteYIndexed, "A, ${0:X2}+Y") },
            { 0xB7, new AssemblyInstruction(0xB7, "SBC", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}]+Y") },
            { 0xB8, new AssemblyInstruction(0xB8, "SBC", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0xB9, new AssemblyInstruction(0xB9, "SBC", 0, AssemblyAddressingMode.IndirectToIndirect, "(X), (Y)") },
            { 0x9A, new AssemblyInstruction(0x9A, "SUBW", 1, AssemblyAddressingMode.DirectPage, "YA, ${0:X2}") },

            // OR
            { 0x04, new AssemblyInstruction(0x04, "OR", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0x05, new AssemblyInstruction(0x05, "OR", 2, AssemblyAddressingMode.Absolute, "A, ${0:X2}{1:X2}") },
            { 0x06, new AssemblyInstruction(0x06, "OR", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0x07, new AssemblyInstruction(0x07, "OR", 1, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}+X] ") },
            { 0x08, new AssemblyInstruction(0x08, "OR", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0x09, new AssemblyInstruction(0x09, "OR", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0x14, new AssemblyInstruction(0x14, "OR", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, {0:X2}+X") },
            { 0x15, new AssemblyInstruction(0x15, "OR", 2, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}{1:X2}+X") },
            { 0x16, new AssemblyInstruction(0x16, "OR", 2, AssemblyAddressingMode.AbsoluteYIndexed, "A, ${0:X2}{1:X2}+Y") },
            { 0x17, new AssemblyInstruction(0x17, "OR", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}]+Y") },
            { 0x18, new AssemblyInstruction(0x18, "OR", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0x0A, new AssemblyInstruction(0x0A, "OR1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "C, ${0:X2}.#${1:X2}") },

            // EOR
            { 0x44, new AssemblyInstruction(0x44, "EOR", 1, AssemblyAddressingMode.DirectPage, "A, ${0:X2}") },
            { 0x45, new AssemblyInstruction(0x45, "EOR", 2, AssemblyAddressingMode.Absolute, "A, ${0:X2}{1:X2}") },
            { 0x46, new AssemblyInstruction(0x46, "EOR", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0x47, new AssemblyInstruction(0x47, "EOR", 1, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}+X]") },
            { 0x48, new AssemblyInstruction(0x48, "EOR", 1, AssemblyAddressingMode.Immediate, "A, #${0:X2}") },
            { 0x49, new AssemblyInstruction(0x49, "EOR", 2, AssemblyAddressingMode.DirectPageToDirectPage, "${0:X2}, ${1:X2}") },
            { 0x54, new AssemblyInstruction(0x54, "EOR", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0x55, new AssemblyInstruction(0x55, "EOR", 2, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}{1:X2}+X") },
            { 0x56, new AssemblyInstruction(0x56, "EOR", 2, AssemblyAddressingMode.AbsoluteYIndexed, " A, ${0:X2}{1:X2}+Y") },
            { 0x57, new AssemblyInstruction(0x57, "EOR", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}]+Y") },
            { 0x58, new AssemblyInstruction(0x58, "EOR", 2, AssemblyAddressingMode.ImmediateToDirectPage, "${0:X2}, #${1:X2}") },
            { 0x59, new AssemblyInstruction(0x59, "EOR", 0, AssemblyAddressingMode.DirectPageToDirectPage, "(X), (Y)") },
            { 0x8A, new AssemblyInstruction(0x8A, "EOR1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "C, ${0:X2}.#${1:X2}") },

            // Branch
            { 0x2F, new AssemblyInstruction(0x2F, "BRA", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0x10, new AssemblyInstruction(0x10, "BPL", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0x30, new AssemblyInstruction(0x30, "BMI", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0xF0, new AssemblyInstruction(0xF0, "BEQ", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0xD0, new AssemblyInstruction(0xD0, "BNE", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0x90, new AssemblyInstruction(0x90, "BCC", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0xB0, new AssemblyInstruction(0xB0, "BCS", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0x50, new AssemblyInstruction(0x50, "BVC", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0x70, new AssemblyInstruction(0x70, "BVS", 1, AssemblyAddressingMode.Relative, "${0:X4}") },
            { 0x03, new AssemblyInstruction(0x03, "BBS0", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x23, new AssemblyInstruction(0x23, "BBS1", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x43, new AssemblyInstruction(0x43, "BBS2", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x63, new AssemblyInstruction(0x63, "BBS3", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x83, new AssemblyInstruction(0x83, "BBS4", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0xA3, new AssemblyInstruction(0xA3, "BBS5", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0xC3, new AssemblyInstruction(0xC3, "BBS6", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0xE3, new AssemblyInstruction(0xE3, "BBS7", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x13, new AssemblyInstruction(0x13, "BBC0", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x33, new AssemblyInstruction(0x33, "BBC1", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x53, new AssemblyInstruction(0x53, "BBC2", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x73, new AssemblyInstruction(0x73, "BBC3", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x93, new AssemblyInstruction(0x93, "BBC4", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0xB3, new AssemblyInstruction(0xB3, "BBC5", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0xD3, new AssemblyInstruction(0xD3, "BBC6", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0xF3, new AssemblyInstruction(0xF3, "BBC7", 2, AssemblyAddressingMode.DirectPageBitRelative, "${0:X2}{1:X2}") },
            { 0x2E, new AssemblyInstruction(0x2E, "CBNE", 2, AssemblyAddressingMode.Relative, "${0:X2}, ${1:X4}") },
            { 0xDE, new AssemblyInstruction(0xDE, "CBNE", 2, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X, ${1:X4}") },
            { 0x6E, new AssemblyInstruction(0x6E, "DBNZ", 2, AssemblyAddressingMode.Relative, "${0:X2}, ${1:X4}") },
            { 0xFE, new AssemblyInstruction(0xFE, "DBNZ", 1, AssemblyAddressingMode.Relative, "Y, ${0:X4}") },

            // Set
            { 0x0E, new AssemblyInstruction(0x0E, "TSET1", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x02, new AssemblyInstruction(0x02, "SET0", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x22, new AssemblyInstruction(0x22, "SET1", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x42, new AssemblyInstruction(0x42, "SET2", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x62, new AssemblyInstruction(0x62, "SET3", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x82, new AssemblyInstruction(0x82, "SET4", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0xA2, new AssemblyInstruction(0xA2, "SET5", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0xC2, new AssemblyInstruction(0xC2, "SET6", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0xE2, new AssemblyInstruction(0xE2, "SET7", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },

            // Clear
            { 0x4E, new AssemblyInstruction(0x4E, "TCLR1", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x12, new AssemblyInstruction(0x12, "CLR0", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x32, new AssemblyInstruction(0x32, "CLR1", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x52, new AssemblyInstruction(0x52, "CLR2", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x72, new AssemblyInstruction(0x72, "CLR3", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0x92, new AssemblyInstruction(0x92, "CLR4", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0xB2, new AssemblyInstruction(0xB2, "CLR5", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0xD2, new AssemblyInstruction(0xD2, "CLR6", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },
            { 0xF2, new AssemblyInstruction(0xF2, "CLR7", 1, AssemblyAddressingMode.DirectPageBit, "${0:X2}") },

            // Inc/Dec
            { 0x3A, new AssemblyInstruction(0x3A, "INCW", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0x3D, new AssemblyInstruction(0x3D, "INC", 0, AssemblyAddressingMode.Implied, "X") },
            { 0xAB, new AssemblyInstruction(0xAB, "INC", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0xAC, new AssemblyInstruction(0xAC, "INC", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0xBB, new AssemblyInstruction(0xBB, "INC", 1, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X") },
            { 0xBC, new AssemblyInstruction(0xBC, "INC", 0, AssemblyAddressingMode.Implied, "A") },
            { 0xFC, new AssemblyInstruction(0xFC, "INC", 0, AssemblyAddressingMode.Implied, "Y") },
            { 0x1D, new AssemblyInstruction(0x1D, "DEC", 0, AssemblyAddressingMode.Implied, "X") },
            { 0xDC, new AssemblyInstruction(0xDC, "DEC", 0, AssemblyAddressingMode.Implied, "Y") },
            { 0x8B, new AssemblyInstruction(0x8B, "DEC", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0x9B, new AssemblyInstruction(0x9B, "DEC", 1, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X") },
            { 0x9C, new AssemblyInstruction(0x9C, "DEC", 0, AssemblyAddressingMode.Absolute, "A") },
            { 0x1A, new AssemblyInstruction(0x1A, "DECW", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },

            // Shifting
            { 0x1C, new AssemblyInstruction(0x1C, "ASL", 0, AssemblyAddressingMode.Implied, "A") },
            { 0x0B, new AssemblyInstruction(0x0B, "ASL", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0x0C, new AssemblyInstruction(0x0C, "ASL", 2, AssemblyAddressingMode.Absolute, "${1:X2}{0:X2}") },
            { 0x4B, new AssemblyInstruction(0x4B, "LSR", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0x5B, new AssemblyInstruction(0x5B, "LSR", 0, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X") },
            { 0x5C, new AssemblyInstruction(0x5C, "LSR", 0, AssemblyAddressingMode.DirectPageYIndexed, "A") },

            // Rolling
            { 0x2B, new AssemblyInstruction(0x2B, "ROL", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0x2C, new AssemblyInstruction(0x2C, "ROL", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x3B, new AssemblyInstruction(0x3B, "ROL", 1, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X") },
            { 0x3C, new AssemblyInstruction(0x3C, "ROL", 0, AssemblyAddressingMode.Implied, "A") },
            { 0x6B, new AssemblyInstruction(0x6B, "ROR", 1, AssemblyAddressingMode.DirectPage, "${0:X2}") },
            { 0x6C, new AssemblyInstruction(0x6C, "ROR", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x7B, new AssemblyInstruction(0x7B, "ROR", 1, AssemblyAddressingMode.DirectPageXIndexed, "${0:X2}+X") },
            { 0x7C, new AssemblyInstruction(0x7C, "ROR", 0, AssemblyAddressingMode.Implied, "A") },
            { 0x9F, new AssemblyInstruction(0x9F, "XCN", 0, AssemblyAddressingMode.Implied, "A") },

            // Push/Pop
            { 0x2D, new AssemblyInstruction(0x2D, "PUSH", 0, AssemblyAddressingMode.Implied, "A") },
            { 0x4D, new AssemblyInstruction(0x4D, "PUSH", 0, AssemblyAddressingMode.Implied, "X") },
            { 0x6D, new AssemblyInstruction(0x6D, "PUSH", 0, AssemblyAddressingMode.Implied, "Y") },
            { 0x0D, new AssemblyInstruction(0x0D, "PUSH", 0, AssemblyAddressingMode.Implied, "PSW") },
            { 0xAE, new AssemblyInstruction(0xAE, "POP", 0, AssemblyAddressingMode.Implied, "A") },
            { 0xCE, new AssemblyInstruction(0xCE, "POP", 0, AssemblyAddressingMode.Implied, "X") },
            { 0xEE, new AssemblyInstruction(0xEE, "POP", 0, AssemblyAddressingMode.Implied, "Y") },
            { 0x8E, new AssemblyInstruction(0x8E, "POP", 0, AssemblyAddressingMode.Implied, "PSW") },

            // Subroutine Operations
            { 0x3F, new AssemblyInstruction(0x3F, "CALL", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x5F, new AssemblyInstruction(0x5F, "JMP", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x1F, new AssemblyInstruction(0x1F, "JMP", 2, AssemblyAddressingMode.AbsoluteXIndexed, "(${0:X2}+X)") },
            { 0x4F, new AssemblyInstruction(0x4F, "PCALL", 1, AssemblyAddressingMode.Implied, "$FF{0:X2}") },
            { 0x01, new AssemblyInstruction(0x01, "TCALL", 0, AssemblyAddressingMode.Implied, "0") },
            { 0x11, new AssemblyInstruction(0x11, "TCALL", 0, AssemblyAddressingMode.Implied, "1") },
            { 0x21, new AssemblyInstruction(0x21, "TCALL", 0, AssemblyAddressingMode.Implied, "2") },
            { 0x31, new AssemblyInstruction(0x31, "TCALL", 0, AssemblyAddressingMode.Implied, "3") },
            { 0x41, new AssemblyInstruction(0x41, "TCALL", 0, AssemblyAddressingMode.Implied, "4") },
            { 0x51, new AssemblyInstruction(0x51, "TCALL", 0, AssemblyAddressingMode.Implied, "5") },
            { 0x61, new AssemblyInstruction(0x61, "TCALL", 0, AssemblyAddressingMode.Implied, "6") },
            { 0x71, new AssemblyInstruction(0x71, "TCALL", 0, AssemblyAddressingMode.Implied, "7") },
            { 0x81, new AssemblyInstruction(0x81, "TCALL", 0, AssemblyAddressingMode.Implied, "8") },
            { 0x91, new AssemblyInstruction(0x91, "TCALL", 0, AssemblyAddressingMode.Implied, "9") },
            { 0xA1, new AssemblyInstruction(0xA1, "TCALL", 0, AssemblyAddressingMode.Implied, "10") },
            { 0xB1, new AssemblyInstruction(0xB1, "TCALL", 0, AssemblyAddressingMode.Implied, "11") },
            { 0xC1, new AssemblyInstruction(0xC1, "TCALL", 0, AssemblyAddressingMode.Implied, "12") },
            { 0xD1, new AssemblyInstruction(0xD1, "TCALL", 0, AssemblyAddressingMode.Implied, "13") },
            { 0xE1, new AssemblyInstruction(0xE1, "TCALL", 0, AssemblyAddressingMode.Implied, "14") },
            { 0xF1, new AssemblyInstruction(0xF1, "TCALL", 0, AssemblyAddressingMode.Implied, "15") },
            { 0x6F, new AssemblyInstruction(0x6F, "RET", 0, AssemblyAddressingMode.Implied, string.Empty) },

            // Multiplication/Division Operations
            { 0xCF, new AssemblyInstruction(0xCF, "MUL", 0, AssemblyAddressingMode.Implied, "YA") },
            { 0x9E, new AssemblyInstruction(0x9E, "DIV", 0, AssemblyAddressingMode.Implied, "YA, X") },
            { 0xBE, new AssemblyInstruction(0xBE, "DAS", 0, AssemblyAddressingMode.Implied, "A") },

            // PSW Operations
            { 0x60, new AssemblyInstruction(0x60, "CLRC", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0x20, new AssemblyInstruction(0x20, "CLRP", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0x40, new AssemblyInstruction(0x40, "SETP", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0x80, new AssemblyInstruction(0x80, "SETC", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0xA0, new AssemblyInstruction(0xA0, "EI", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0xC0, new AssemblyInstruction(0xC0, "DI", 0, AssemblyAddressingMode.Implied, string.Empty) },

            // Misc
            { 0x0F, new AssemblyInstruction(0x0F, "BRK", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0x00, new AssemblyInstruction(0x00, "NOP", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0xEF, new AssemblyInstruction(0xEF, "SLEEP", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0xFF, new AssemblyInstruction(0xFF, "STOP", 0, AssemblyAddressingMode.Implied, string.Empty) },

            // ---------
            
            { 0x19, new AssemblyInstruction(0x19, "OR", 0, AssemblyAddressingMode.IndirectToIndirect, "(X), (Y)") },
            { 0x1B, new AssemblyInstruction(0x1B, "AND", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0x26, new AssemblyInstruction(0x26, "AND", 0, AssemblyAddressingMode.Indirect, "A, (X)") },
            { 0x27, new AssemblyInstruction(0x27, "AND", 0, AssemblyAddressingMode.IndirectXIndexed, "A, [${0:X2}]+X") },
            { 0x2A, new AssemblyInstruction(0x2A, "OR1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "C, /${0:X2}.#${1:X2}") },
            { 0x35, new AssemblyInstruction(0x35, "AND", 2, AssemblyAddressingMode.AbsoluteXIndexed, "A, ${0:X2}{1:X2}+X") },
            { 0x36, new AssemblyInstruction(0x36, "AND", 2, AssemblyAddressingMode.DirectPage, "A, [${0:X2}]+Y") },
            { 0x37, new AssemblyInstruction(0x37, "AND", 1, AssemblyAddressingMode.IndirectYIndexed, "A, [${0:X2}]+Y") },
            { 0x39, new AssemblyInstruction(0x39, "AND", 0, AssemblyAddressingMode.IndirectToIndirect, "(X), (Y)") },
            { 0x4C, new AssemblyInstruction(0x4C, "LSR", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x6A, new AssemblyInstruction(0x6A, "AND1", 2, AssemblyAddressingMode.AbsoluteBooleanBit, "C, /${0:X2}.#${1:X2}") },
            { 0x7F, new AssemblyInstruction(0x7F, "RETI", 0, AssemblyAddressingMode.Implied, "") },
            { 0x8C, new AssemblyInstruction(0x8C, "DEC", 2, AssemblyAddressingMode.Absolute, "${0:X2}{1:X2}") },
            { 0x94, new AssemblyInstruction(0x94, "ADC", 1, AssemblyAddressingMode.DirectPageXIndexed, "A, ${0:X2}+X") },
            { 0x9D, new AssemblyInstruction(0x9D, "MOV", 0, AssemblyAddressingMode.Implied, "X, SP") },
            { 0xC6, new AssemblyInstruction(0xC6, "MOV", 0, AssemblyAddressingMode.Indirect, "(X), A") },
            { 0xDF, new AssemblyInstruction(0xDF, "DAA", 0, AssemblyAddressingMode.Implied, "A") },
            { 0xE0, new AssemblyInstruction(0xE0, "CLRV", 0, AssemblyAddressingMode.Implied, string.Empty) },
            { 0xED, new AssemblyInstruction(0xED, "NOTC", 0, AssemblyAddressingMode.Implied, string.Empty) },
        };

        public static IReadOnlyList<byte> OpCodesWithFirstToLastOperands { get; } = new List<byte>()
        {
            0x03, 0x23, 0x43, 0x63, 0x83, 0xA3, 0xC3, 0xE3, // BBS
            0x13, 0x33, 0x53, 0x73, 0x93, 0xB3, 0xD3, 0xF3, // BBC
            0x2E, 0xD3,                                     // CBNE
            0x6E, 0xFE,                                     // DBNZ
        };

        public DisassembledCodeBlock Disassemble(RomFile romFile, int startAddress, ushort length)
        {
            return this.Disassemble(romFile, startAddress, length, startAddress);
        }

        public DisassembledCodeBlock Disassemble(RomFile romFile, int startAddress, ushort length, int spcStartAddress)
        {
            ushort pc = 0;

            List<byte> emptyOperandList = new List<byte>();
            List<DisassembledSPC700Instruction> parsedCodeList = new List<DisassembledSPC700Instruction>(64);
            while (pc < length)
            {
                int opCodeAddress = startAddress + pc;
                int spcOpCodeAddress = spcStartAddress + pc;
                byte opCode = romFile.ReadByteAt(startAddress + pc);
                pc++;
                if (!SPC700Disassembler.SPC700InstructionSet.ContainsKey(opCode)) { Debugger.Break(); }

                AssemblyInstruction current = SPC700Disassembler.SPC700InstructionSet[opCode];
                IReadOnlyList<byte> currentOperands = emptyOperandList;
                int relativeOffset = 0;
                if (current.OperandCount > 0)
                {
                    List<byte> operandList = new List<byte>(current.OperandCount);
                    for (int i = 0; i < current.OperandCount; i++)
                    {
                        operandList.Add(romFile.ReadByteAt(startAddress + pc));
                        pc++;
                    }
                    if (current.AddressingMode == AssemblyAddressingMode.Relative ||
                        current.AddressingMode == AssemblyAddressingMode.DirectPageBitRelative)
                    {
                        byte offset = current.OperandCount == 1 ? operandList[0] : operandList[1];
                        if (offset <= 127) { relativeOffset = offset; }
                        else
                        {
                            relativeOffset = -((offset ^ 0xFF) + 1);
                        }
                    }
                    currentOperands = operandList;
                }
                parsedCodeList.Add(new DisassembledSPC700Instruction(spcOpCodeAddress, current, currentOperands, relativeOffset));
            }

            return new DisassembledCodeBlock(AssemblyInstructionSetType.SPC700, parsedCodeList);
        }
    }
}