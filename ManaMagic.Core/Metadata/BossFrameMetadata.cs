using ManaMagic.Core.Bosses;
using ManaMagic.Core.Bosses.Frames;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    public sealed record BossFrameMetadata(byte Bank, ushort Address, BossFrameType FrameType, string DisplayName, bool HasHitBoxData = true)
    {
        public int FullAddress { get { return (this.Bank << 16) | this.Address; } }
    }
}