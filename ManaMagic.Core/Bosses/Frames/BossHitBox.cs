#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    public sealed record BossHitBox(BossHitBoxType HitType, sbyte XOffset, sbyte YOffset, byte Width, byte Height)
    {
        public static BossHitBox NullHitBox { get; } = new BossHitBox(BossHitBoxType.Hit, 0, 0, 0, 0);
        public static BossHitBox NullWeaponBox { get; } = new BossHitBox(BossHitBoxType.Weapon, 0, 0, 0, 0);
        public static BossHitBox NullGuardBox { get; } = new BossHitBox(BossHitBoxType.Guard, 0, 0, 0, 0);

        public bool IsValid { get { { return this.Width > 0 && this.Height > 0; } } }
    }
}