#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public sealed class AkaoChannel
    {
        public bool Enabled { get; }
        public ushort Offset { get; }
        public ushort Size { get; }
        public AkaoSequence Sequence { get; }

        public AkaoChannel(bool enabled, ushort offset, ushort size, AkaoSequence sequence)
        {
            this.Enabled = enabled;
            this.Offset = offset;
            this.Size = size;
            this.Sequence = sequence;
        }
    }
}