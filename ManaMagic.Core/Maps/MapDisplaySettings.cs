using ZwellTech;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapDisplaySettings : NotifyRecordPropertyChanged
    {
        public static MapDisplaySettings Empty { get; } = new MapDisplaySettings(0, 0, 0, 0, 0, 0, 0, 0, Rgb555Color.Black);

        private byte mosaicSettings = 0;
        private ScreenLayerEnableRegister mainScreenLayersEnabled = 0;
        private ScreenLayerEnableRegister subScreenLayersEnabled = 0;
        private CGADSUB colorMath = 0;
        private Rgb555Color backgroundColor = Rgb555Color.White;

        public byte MosaicSettings { get { return this.mosaicSettings; } }

        public bool EnableOnBackground01
        {
            get { return (this.mosaicSettings & 0x01) > 0; }
            set
            {
                byte newSettings = this.mosaicSettings;
                if (value) { newSettings |= 0x01; }
                else { newSettings &= 0xFE; }
                this.SetProperty(ref this.mosaicSettings, newSettings);
            }
        }

        public bool EnableOnBackground02
        {
            get { return (this.mosaicSettings & 0x02) > 0; }
            set
            {
                byte newSettings = this.mosaicSettings;
                if (value) { newSettings |= 0x02; }
                else { newSettings &= 0xFD; }
                this.SetProperty(ref this.mosaicSettings, newSettings);
            }
        }

        public bool EnableOnBackground03
        {
            get { return (this.mosaicSettings & 0x04) > 0; }
            set
            {
                byte newSettings = this.mosaicSettings;
                if (value) { newSettings |= 0x04; }
                else { newSettings &= 0xFB; }
                this.SetProperty(ref this.mosaicSettings, newSettings);
            }
        }

        public bool EnableOnBackground04
        {
            get { return (this.mosaicSettings & 0x08) > 0; }
            set
            {
                byte newSettings = this.mosaicSettings;
                if (value) { newSettings |= 0x08; }
                else { newSettings &= 0xF7; }
                this.SetProperty(ref this.mosaicSettings, newSettings);
            }
        }

        public MosaicRegisterSize MosaicSize
        {
            get { return (MosaicRegisterSize)((this.mosaicSettings & 0xF0) >> 4); }
            set
            {
                byte newSettings = (byte)(((byte)((((byte)value) & 0x0F) << 4)) | (this.mosaicSettings & 0xF0));
                this.SetProperty(ref this.mosaicSettings, newSettings);
            }
        }

        public ScreenLayerEnableRegister MainScreenLayersEnabled
        {
            get { return this.mainScreenLayersEnabled; }
            set { this.SetProperty(ref this.mainScreenLayersEnabled, value); }
        }

        public ScreenLayerEnableRegister SubScreenLayersEnabled
        {
            get { return this.subScreenLayersEnabled; }
            set { this.SetProperty(ref this.subScreenLayersEnabled, value); }
        }

        public CGADSUB ColorMath
        {
            get { return this.colorMath; }
            set { this.SetProperty(ref this.colorMath, value); }
        }

        public byte UnusedValue01 { get; }
        public byte UnusedValue02 { get; }
        public byte UnusedValue03 { get; }

        public Rgb555Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set { this.SetProperty(ref this.backgroundColor, value); }
        }


        public MapDisplaySettings(byte index, byte mosaicSettings, byte mainScreenLayersEnabled, byte subScreenLayersEnabled, byte colorMath, byte unusedValue01, byte unusedValue02, byte unusedValue03, Rgb555Color backgroundColor, bool userModified = false) : base(index, userModified)
        {
            this.mosaicSettings = mosaicSettings;
            this.mainScreenLayersEnabled = (ScreenLayerEnableRegister)mainScreenLayersEnabled;
            this.subScreenLayersEnabled = (ScreenLayerEnableRegister)subScreenLayersEnabled;
            this.colorMath = (CGADSUB)colorMath;
            this.UnusedValue01 = unusedValue01;
            this.UnusedValue02 = unusedValue02;
            this.UnusedValue03 = unusedValue03;
            this.backgroundColor = backgroundColor;
        }
    }
}