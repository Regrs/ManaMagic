#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed class SpriteGraphicsData
    {
        public ushort EncodedGraphicsOffset { get; }
        public ushort FrameIndexOffset { get; }
        public byte Unknown01 { get; }
        public ushort Unknown02 { get; }
        public ushort Unknown03 { get; }
        public ushort AIOffset { get; }
        public ushort Unknown05 { get; }
        public ushort Unknown06 { get; }
        public byte Unknown07 { get; }

        public uint FullGraphicsAddress { get { return (uint)((this.EncodedGraphicsOffset & 0x1FFF) << 6) + 0x150000; } }

        public SpriteGraphicsData(ushort encodedGfxOffset, ushort frameIndexOffset, byte unknown1, ushort unknown2, ushort unknown3, ushort aiOffset, ushort unknown5, ushort unknown6, byte unknown7)
        {
            this.EncodedGraphicsOffset = encodedGfxOffset; // These bits being discarded are probably used for something.
            this.FrameIndexOffset = frameIndexOffset;      // This is an offset into the frame construction pointer table and is the first frame for this sprite.
            this.Unknown01 = unknown1;
            this.Unknown02 = unknown2;
            this.Unknown03 = unknown3;
            this.AIOffset = aiOffset;
            this.Unknown05 = unknown5;
            this.Unknown06 = unknown6;
            this.Unknown07 = unknown7;
        }
    }
}