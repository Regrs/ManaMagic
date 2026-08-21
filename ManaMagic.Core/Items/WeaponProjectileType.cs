#nullable enable

namespace ManaMagic.Core.Items
{
    public enum WeaponProjectileType : byte
    {
        None = 0x00,
        Arrow = 0x01,
        Javelin = 0x02,
        Boomerang = 0x03,
        UpsideDownChakram = 0x04, // No idea, maybe used for the charkam's flight back?
        Chakram = 0x05,
    }
}