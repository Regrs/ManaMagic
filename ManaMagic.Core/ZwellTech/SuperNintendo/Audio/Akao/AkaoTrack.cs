using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public sealed class AkaoTrack
    {
        public uint Address { get; }
        public ushort Size { get; }
        public ushort StartOffset { get; }
        public ushort EndOffset { get; }
        public AkaoChannel Channel01 { get; }
        public AkaoChannel Channel02 { get; }
        public AkaoChannel Channel03 { get; }
        public AkaoChannel Channel04 { get; }
        public AkaoChannel Channel05 { get; }
        public AkaoChannel Channel06 { get; }
        public AkaoChannel Channel07 { get; }
        public AkaoChannel Channel08 { get; }
        public IReadOnlyList<AkaoInstrument> Instruments { get; }

        public int ChannelsUsed
        {
            get
            {
                int count = 0;
                if (Channel01.Enabled) { count++; }
                if (Channel02.Enabled) { count++; }
                if (Channel03.Enabled) { count++; }
                if (Channel04.Enabled) { count++; }
                if (Channel05.Enabled) { count++; }
                if (Channel06.Enabled) { count++; }
                if (Channel07.Enabled) { count++; }
                if (Channel08.Enabled) { count++; }
                return count;
            }
        }

        public AkaoTrack(uint address,
                         ushort size,
                         ushort startOffset,
                         ushort endoffset,
                         AkaoChannel channel01,
                         AkaoChannel channel02,
                         AkaoChannel channel03,
                         AkaoChannel channel04,
                         AkaoChannel channel05,
                         AkaoChannel channel06,
                         AkaoChannel channel07,
                         AkaoChannel channel08,
                         IReadOnlyList<AkaoInstrument> instruments)
        {
            this.Address = address;
            this.Size = size;
            this.StartOffset = startOffset;
            this.EndOffset = endoffset;
            this.Channel01 = channel01;
            this.Channel02 = channel02;
            this.Channel03 = channel03;
            this.Channel04 = channel04;
            this.Channel05 = channel05;
            this.Channel06 = channel06;
            this.Channel07 = channel07;
            this.Channel08 = channel08;
            this.Instruments = instruments;
        }
    }
}