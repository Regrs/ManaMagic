using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Items
{
    public sealed record ManaEquipmentDefinition : NotifyRecordPropertyChanged
    {
        private EquipmentStatModifiers statModifiers = EquipmentStatModifiers.StatAdjustmentTypeFlag;
        private byte defense = 0;
        private byte evasion = 0;
        private byte magicDefense = 0;
        private byte magicEvasion = 0;
        private EquipmentEquippableSettings equippableBy = EquipmentEquippableSettings.None;
        private ElementalType element = ElementalType.None;
        private StatusEffects resistances = StatusEffects.None;
        private byte unknown = 0;

        public EquipmentStatModifiers StatModifiers
        {
            get { return this.statModifiers; }
            set { this.SetProperty(ref this.statModifiers, value); }
        }

        public byte Defense
        {
            get { return this.defense; }
            set { this.SetProperty(ref this.defense, value); }
        }

        public byte Evasion
        {
            get { return this.evasion; }
            set { this.SetProperty(ref this.evasion, value); }
        }

        public byte MagicDefense
        {
            get { return this.magicDefense; }
            set { this.SetProperty(ref this.magicDefense, value); }
        }

        public byte MagicEvasion
        {
            get { return this.magicEvasion; }
            set { this.SetProperty(ref this.magicEvasion, value); }
        }

        public EquipmentEquippableSettings EquippableBy
        {
            get { return this.equippableBy; }
            set { this.SetProperty(ref this.equippableBy, value); }
        }

        public ElementalType Element
        {
            get { return this.element; }
            set { this.SetProperty(ref this.element, value); }
        }

        public StatusEffects Resistances
        {
            get { return this.resistances; }
            set { this.SetProperty(ref this.resistances, value); }
        }

        public byte Unknown
        {
            get { return this.unknown; }
            set { this.SetProperty(ref this.unknown, value); }
        }

        public ManaEquipmentDefinition(byte index, byte statModifiers, byte defense, byte evasion, byte magicDefense, byte magicEvasion, byte equippableBy, byte element, ushort resistances, byte unknown, bool userModified = false) : base(index, userModified)
        {
            this.statModifiers = (EquipmentStatModifiers)statModifiers;
            this.defense = defense;
            this.evasion = evasion;
            this.magicDefense = magicDefense;
            this.magicEvasion = magicEvasion;
            this.equippableBy = (EquipmentEquippableSettings)equippableBy;
            this.element = (ElementalType)element;
            this.resistances = (StatusEffects)resistances;
            this.unknown = unknown;
        }
    }
}
/*
[Table_HeadArmor]
;     Legend
;     SB: Stat Boost
;         0x40: Always Set
;         0x10: Strength
;         0x08: Agility
;         0x04: Constitution
;         0x02: Intelligence
;         0x01: Wisdom
;     DF: Defense
;     EV: Evasion
;     MD: Magic Defense
;     MV: Magic Evasion
;     EQ: Equippable By
;         0xE0: Everyone
;         0xA0: Randi & Popoie
;         0x60: Purim & Popoie
;         0x80: Randi
;         0x40: Purim
;         0x20: Popoie
;     EL: Elemental Resistance/Weakness (Dummied Out, but functional if you alter the table)
;     R1: Status Resist 1
;     R2: Status Resist 2
;     ??: ???
        SB DF EV MD MV EQ EL R1 R2 ??
        00 01 02 03 04 05 06 07 08 09
103ED0: 40 00 00 00 00 E0 00 00 00 00 [00: Bare Head]
 */