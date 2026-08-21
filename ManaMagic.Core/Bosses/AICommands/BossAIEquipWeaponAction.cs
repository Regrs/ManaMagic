namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAIEquipWeaponAction : BossAIOneUShortOperandAction
    {
        public ushort WeaponIndex { get { return this.Operand; } }

        public BossAIEquipWeaponAction(ushort weaponIndex) : base(BossAICommandActionType.EquipBossWeapon, weaponIndex) { }
    }
}