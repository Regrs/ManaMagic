#nullable enable

namespace ManaMagic.Core.Events
{
    public enum RestoreEventOpCodeType : byte
    {
        EventActivatorHitPoints = 0x00,
        RandiHitPoints = 0x01,
        PurimHitPoints = 0x02,
        PopoieHitPoints = 0x03,
        PartyHitPoints = 0x04,
        EventActivatorProcessStatusEffects = 0x40,
        RandiProcessStatusEffects = 0x41,
        PurimProcessStatusEffects = 0x42,
        PopoieProcessStatusEffects = 0x43,
        PartyProcessStatusEffects = 0x44,
        EventActivatorManaPoints = 0x80,
        RandiManaPoints = 0x81,
        PurimManaPoints = 0x82,
        PopoieManaPoints = 0x83,
        PartyManaPoints = 0x84,
    }
}