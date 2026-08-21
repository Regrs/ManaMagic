using ZwellTech;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record Map8X8TileInstruction(byte InstructionAddressL, byte InstructionAddressHAndControl, byte InstructionSize)
    {
        public byte InstructionControl { get { return (byte)(this.InstructionAddressHAndControl & 0B_1110_0000); } }
        public byte InstructionAddressH { get { return (byte)(this.InstructionAddressHAndControl & 0B_0001_1111); } }

        public int BaseGraphicsAddressIndex
        {
            get
            {
                if (this.InstructionControl < 0x80) { return 0; }
                if (this.InstructionControl == 0x80) { return 1; } // 80 - 0B_1000_0000
                if (this.InstructionControl == 0xC0) { return 2; } // C0 - 0B_1100_0000
                if (this.InstructionControl == 0xE0) { return 3; } // E0 - 0B_1110_0000
                if (this.InstructionControl == 0xA0) { return 1; } // A0 - 0B_1010_0000
                return (int)ThrowHelper.ThrowInvalidOperationException("Invalid Instruction Bits");
            }
        }

        public uint GetAddressOffset(TileType type)
        {
            switch (type)
            {
                case TileType.ThreeBitsPerPixel:
                    uint baseOffset = (uint)((this.InstructionAddressH << 8) | this.InstructionAddressL);
                    uint offset = (baseOffset * 8) + (baseOffset * 16);
                    return offset;
                case TileType.FourBitsPerPixel:
                    // Expand to a 24-bit offset.
                    return (uint)(((this.InstructionAddressH << 8) | this.InstructionAddressL) << 5);
                default:
                    return (uint)ThrowHelper.ThrowArgumentException("Invalid TileType", nameof(type));
            }
        }
    }
}