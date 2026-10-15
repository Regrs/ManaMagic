#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    public sealed class BitRateReductionBlock
    {
        public static BitRateReductionBlock Empty { get; } = new BitRateReductionBlock(1, 0, 0, 0, 0, 0, 0, 0, 0);

        public byte Control { get; } = 0;
        public byte Range { get { return (byte)(this.Control >> 4); } }
        public byte Filter { get { return (byte)((this.Control >> 2) & 0x03); } }
        public bool Loop { get { return (this.Control & 0x02) > 0; } }
        public bool End { get { return (this.Control & 0x01) > 0; } }

        public byte Sample01 { get; } = 0;
        public byte Sample02 { get; } = 0;
        public byte Sample03 { get; } = 0;
        public byte Sample04 { get; } = 0;
        public byte Sample05 { get; } = 0;
        public byte Sample06 { get; } = 0;
        public byte Sample07 { get; } = 0;
        public byte Sample08 { get; } = 0;

        public BitRateReductionBlock(byte control, byte sample1, byte sample2, byte sample3, byte sample4, byte sample5, byte sample6, byte sample7, byte sample8)
        {
            this.Control = control;
            this.Sample01 = sample1;
            this.Sample02 = sample2;
            this.Sample03 = sample3;
            this.Sample04 = sample4;
            this.Sample05 = sample5;
            this.Sample06 = sample6;
            this.Sample07 = sample7;
            this.Sample08 = sample8;
        }

        public byte GetRawSample(int index, bool highNibble)
        {
            byte sample = this.GetSampleByIndex(index);
            if (highNibble)
            {
                return (byte)((sample >> 4) & 0x0F);
            }
            return (byte)(sample & 0x0F);
        }

        public byte GetSampleByIndex(int index)
        {
            switch (index)
            {
                case 0: return this.Sample01;
                case 1: return this.Sample02;
                case 2: return this.Sample03;
                case 3: return this.Sample04;
                case 4: return this.Sample05;
                case 5: return this.Sample06;
                case 6: return this.Sample07;
                case 7: return this.Sample08;
            }
            return (byte)ThrowHelper.ThrowArgumentException("Invalid sample index.", nameof(index));
        }
    }
}