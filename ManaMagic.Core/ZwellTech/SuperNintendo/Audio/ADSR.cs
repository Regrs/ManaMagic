#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    public readonly struct ADSR
    {
        public static ADSR Empty { get; } = new ADSR(0, 0);

        private readonly byte adsr1;
        private readonly byte adsr2;

        public bool Enabled { get { return (this.adsr1 & 0x80) > 0; } }
        public byte Attack { get { return (byte)(this.adsr1 & 0x0F); } }
        public byte Decay { get { return (byte)((this.adsr1 & 0x70) >> 4); } }
        public byte SustainLevel { get { return (byte)((this.adsr2 & 0xE0) >> 5); } }
        public byte SustainRate { get { return (byte)(this.adsr2 & 0x1F); } }

        public ADSR(byte vxADSR1, byte vxADSR2)
        {
            // Release mode in the S-DSP has a linear decrease at a rate of -8 every sample.
            this.adsr1 = vxADSR1;
            this.adsr2 = vxADSR2;
        }
    }
}