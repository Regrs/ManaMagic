using System;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Represents the SNES TM (Main Screen Layer Enable) and TS (Sub Screen Layer Enable) registers.
    /// </summary>
    /// <remarks>Address: $212C/$212D</remarks>
    public sealed record ScreenLayerEnableRegisterSettings
    {
        public bool EnableBackground01 { get; }
        public bool EnableBackground02 { get; }
        public bool EnableBackground03 { get; }
        public bool EnableBackground04 { get; }
        public bool EnableObjects { get; }

        public ScreenLayerEnableRegisterSettings(byte screenLayerSettings)
        {
            this.EnableBackground01 = (screenLayerSettings & 0x01) > 0;
            this.EnableBackground02 = (screenLayerSettings & 0x02) > 0;
            this.EnableBackground03 = (screenLayerSettings & 0x04) > 0;
            this.EnableBackground04 = (screenLayerSettings & 0x08) > 0;
            this.EnableObjects = (screenLayerSettings & 0x10) > 0;
        }

        public byte ToByte()
        {
            byte screenLayerSettings = 0;
            if (this.EnableBackground01) { screenLayerSettings |= 0x01; }
            if (this.EnableBackground02) { screenLayerSettings |= 0x02; }
            if (this.EnableBackground03) { screenLayerSettings |= 0x04; }
            if (this.EnableBackground04) { screenLayerSettings |= 0x08; }
            if (this.EnableObjects) { screenLayerSettings |= 0x10; }

            return screenLayerSettings;
        }
    }
}