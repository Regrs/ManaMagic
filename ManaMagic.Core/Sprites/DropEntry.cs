using ManaMagic.Core.Items;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed record DropEntry : NotifyRecordPropertyChanged
    {
        private LootDropType dropType = LootDropType.Gold;
        private byte value = 0;

        public LootDropType DropType
        {
            get { return this.dropType; }
            set
            {
                this.Value = DropEntry.GetDefaultValue(value);
                this.SetProperty(ref this.dropType, value);
            }
        }

        public byte Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value); }
        }

        public bool IsGold { get { return this.DropType == LootDropType.Gold; } }
        public bool IsConsumable { get { return this.DropType == LootDropType.Consumable; } }
        public bool IsHelmet { get { return this.DropType == LootDropType.Helmet; } }
        public bool IsArmor { get { return this.DropType == LootDropType.Armor; } }
        public bool IsAccessory { get { return this.DropType == LootDropType.Accessory; } }
        public bool IsWeaponOrb { get { return this.DropType == LootDropType.WeaponOrb; } }

        public DropEntry(LootDropType dropType, byte value, bool userModified = false) : base(0, userModified)
        {
            this.dropType = dropType;
            this.value = value;
        }

        public int GetValueAsIndex()
        {
            switch (this.DropType)
            {
                case LootDropType.Armor: return this.Value - 0x15;
                case LootDropType.Accessory: return this.Value - 0x2A;
                case LootDropType.Consumable: return this.Value - 0x40;
                case LootDropType.WeaponOrb: return this.Value - 0x80;
                case LootDropType.Helmet:
                case LootDropType.Gold:
                default: return this.Value;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            switch (this.DropType)
            {
                case LootDropType.Consumable: return ((ConsumableItemType)(this.Value - 0x40)).ToString();
                case LootDropType.Helmet: return ((HelmetType)this.Value).ToString();
                case LootDropType.Armor: return ((ArmorType)this.Value).ToString();
                case LootDropType.Accessory: return ((AccessoryType)this.Value).ToString();
                case LootDropType.WeaponOrb: return ((WeaponOrbType)this.Value).ToString();
                case LootDropType.Gold: return this.Value.ToString();
                default: return base.ToString();
            }
        }

        private static byte GetDefaultValue(LootDropType dropType)
        {
            switch (dropType)
            {
                case LootDropType.Consumable: return (byte)default(ConsumableItemType);
                case LootDropType.Helmet: return (byte)default(HelmetType);
                case LootDropType.Armor: return (byte)default(ArmorType);
                case LootDropType.Accessory: return (byte)default(AccessoryType);
                case LootDropType.WeaponOrb: return (byte)default(WeaponOrbType);
                case LootDropType.Gold:
                default: return 0;
            }
        }
    }
}