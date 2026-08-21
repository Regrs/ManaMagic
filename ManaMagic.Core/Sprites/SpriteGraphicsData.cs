#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed class SpriteGraphicsData
    {
        public uint GraphicsAddress { get; }
        public ushort FrameIndexOffset { get; }
        public ushort Unknown02 { get; }
        public ushort Unknown03 { get; }
        public ushort Unknown04 { get; }
        public ushort Unknown05 { get; }
        public ushort Unknown06 { get; }
        public ushort Unknown07 { get; }

        public SpriteGraphicsData(ushort encodedGfxOffset, ushort frameIndexOffset, ushort unknown2, ushort unknown3, ushort unknown4, ushort unknown5, ushort unknown6, ushort unknown7)
        {
            // These bits being discarded are probably used for something.
            this.GraphicsAddress = (uint)((encodedGfxOffset & 0x1FFF) << 6) + 0x150000;

            // This is an offset into the frame construction pointer table and is the first frame for this sprite.
            this.FrameIndexOffset = frameIndexOffset;
            this.Unknown02 = unknown2;
            this.Unknown03 = unknown3;
            this.Unknown04 = unknown4;
            this.Unknown05 = unknown5;
            this.Unknown06 = unknown6;
            this.Unknown07 = unknown7;
        }
    }
}