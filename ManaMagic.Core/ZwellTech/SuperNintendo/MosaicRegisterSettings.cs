#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Represents the SNES MOSAIC (Screen Pixelation) register.
    /// </summary>
    /// <remarks>Address: $2106</remarks>
    public sealed record MosaicRegisterSettings
    {
        public bool EnableOnBackground01 { get; set; }
        public bool EnableOnBackground02 { get; set; }
        public bool EnableOnBackground03 { get; set; }
        public bool EnableOnBackground04 { get; set; }
        public MosaicRegisterSize Size { get; set; }

        public MosaicRegisterSettings(byte mosaicSettings)
        {
            this.EnableOnBackground01 = (mosaicSettings & 0x01) > 0;
            this.EnableOnBackground02 = (mosaicSettings & 0x02) > 0;
            this.EnableOnBackground03 = (mosaicSettings & 0x04) > 0;
            this.EnableOnBackground04 = (mosaicSettings & 0x08) > 0;
            this.Size = (MosaicRegisterSize)((mosaicSettings & 0xF0) >> 4);
        }

        public MosaicRegisterSettings(MosaicRegisterSettings settings)
        {
            this.EnableOnBackground01 = settings.EnableOnBackground01;
            this.EnableOnBackground02 = settings.EnableOnBackground02;
            this.EnableOnBackground03 = settings.EnableOnBackground03;
            this.EnableOnBackground04 = settings.EnableOnBackground04;
            this.Size = settings.Size;
        }

        public byte ToByte()
        {
            byte mosaicSettings = (byte)(((byte)this.Size) << 4);
            if (this.EnableOnBackground01) { mosaicSettings |= 0x01; }
            if (this.EnableOnBackground02) { mosaicSettings |= 0x02; }
            if (this.EnableOnBackground03) { mosaicSettings |= 0x04; }
            if (this.EnableOnBackground04) { mosaicSettings |= 0x08; }

            return mosaicSettings;
        }
    }
}