#nullable enable

namespace ManaMagic.Core.Bosses.BossInitialization
{
    public sealed record BossPaletteScript(int Palette1Index,
                                           int Palette2Index,
                                           int Palette3Index,
                                           int ManaBeastSlot1,
                                           int ManaBeastSlot2,
                                           int BackgroundPaletteSlot1,
                                           int BackgroundPaletteSlot2);
}