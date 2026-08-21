#nullable enable

namespace ManaMagic.Core.Events
{
    public enum ScreenEffectEventOpCodeType : byte
    {
        WhiteStrobeEffect = 0x00,
        EndWhiteStrobeEffect = 0x01,
        VerticalEarthquakeEffect = 0x02,
        HorizontalEarthquakeEffect = 0x03,
        EndEarthquakeEffect = 0x04,
        PaletteFilter = 0x05,
        PaletteFlash = 0x06,
        RestoreMapDefaultPalette = 0x07,
        CenterCameraOnEventActivator = 0x08,
    }
}