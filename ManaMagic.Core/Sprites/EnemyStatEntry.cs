using System;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed record EnemyStatEntry : NotifyRecordPropertyChanged
    {
        private byte level = 0;
        private ushort hitPoints = 0;
        private byte manaPoints = 0;
        private byte strength = 0;
        private byte agility = 0;
        private byte intelligence = 0;
        private byte wisdom = 0;
        private byte evasion = 0;
        private ushort defense = 0;
        private byte magicEvasion = 0;
        private ushort magicDefense = 0;
        private MonsterType monsterType = MonsterType.None;
        private ElementalType element = ElementalType.None;
        private ushort experienceAward = 0;
        private byte blackMagicPower = 0;
        private byte whiteMagicPower = 0;
        private StatusEffects immunities = StatusEffects.None;
        private byte meleeWeapon = 0;
        private byte rangedWeapon = 0;
        private byte deathStyle = 0;
        private byte weaponMagicLevel = 0;
        private ushort goldAward = 0;

        public byte Level
        {
            get { return this.level; }
            set { this.SetProperty(ref this.level, value); }
        }

        public ushort HitPoints
        {
            get { return this.hitPoints; }
            set { this.SetProperty(ref this.hitPoints, value); }
        }

        public byte ManaPoints
        {
            get { return this.manaPoints; }
            set { this.SetProperty(ref this.manaPoints, value); }
        }

        public byte Strength
        {
            get { return this.strength; }
            set { this.SetProperty(ref this.strength, value); }
        }

        public byte Agility
        {
            get { return this.agility; }
            set { this.SetProperty(ref this.agility, value); }
        }

        public byte Intelligence
        {
            get { return this.intelligence; }
            set { this.SetProperty(ref this.intelligence, value); }
        }

        public byte Wisdom
        {
            get { return this.wisdom; }
            set { this.SetProperty(ref this.wisdom, value); }
        }

        public byte Evasion
        {
            get { return this.evasion; }
            set { this.SetProperty(ref this.evasion, value); }
        }

        public ushort Defense
        {
            get { return this.defense; }
            set { this.SetProperty(ref this.defense, value); }
        }

        public byte MagicEvasion
        {
            get { return this.magicEvasion; }
            set { this.SetProperty(ref this.magicEvasion, value); }
        }

        public ushort MagicDefense
        {
            get { return this.magicDefense; }
            set { this.SetProperty(ref this.magicDefense, value); }
        }

        public MonsterType MonsterType
        {
            get { return this.monsterType; }
            set { this.SetProperty(ref this.monsterType, value); }
        }

        public ElementalType Element
        {
            get { return this.element; }
            set { this.SetProperty(ref this.element, value); }
        }

        public ushort ExperienceAward
        {
            get { return this.experienceAward; }
            set { this.SetProperty(ref this.experienceAward, value); }
        }

        public byte BlackMagicPower
        {
            get { return this.blackMagicPower; }
            set { this.SetProperty(ref this.blackMagicPower, value); }
        }

        public byte WhiteMagicPower
        {
            get { return this.whiteMagicPower; }
            set { this.SetProperty(ref this.whiteMagicPower, value); }
        }

        public StatusEffects Immunities
        {
            get { return this.immunities; }
            set { this.SetProperty(ref this.immunities, value); }
        }

        public byte Unused { get; }

        public byte MeleeWeapon
        {
            get { return this.meleeWeapon; }
            set { this.SetProperty(ref this.meleeWeapon, value); }
        }

        public byte RangedWeapon
        {
            get { return this.rangedWeapon; }
            set { this.SetProperty(ref this.rangedWeapon, value); }
        }

        public DeathStyle DeathStyle
        {
            // Are any of these other bits used for anything? I don't think so.
            // Some are set sometimes though, be weird to store it this way for no reason.
            // Also this gives a death style range of 0-F, but adds 0xF9 to the style index.
            // Some of my other notes say one of these flags goes into MISC_FLAGS ($E1B1)
            get { return (DeathStyle)((this.deathStyle & 0x1E) >> 1); }
            set
            {
                byte unknownBits = (byte)(this.deathStyle & 0xE1);
                byte deathBits = (byte)((((byte)value) & 0x0F) << 1);
                this.SetProperty(ref this.deathStyle, (byte)(unknownBits | deathBits));
            }
        }

        public bool UnknownDeathBit01 { get { return (this.deathStyle & 0x01) > 0; } }
        public bool UnknownDeathBit20 { get { return (this.deathStyle & 0x20) > 0; } }
        public bool UnknownDeathBit40 { get { return (this.deathStyle & 0x40) > 0; } }
        public bool UnknownDeathBit80 { get { return (this.deathStyle & 0x80) > 0; } }

        public byte DeathStyleAsByte { get { return this.deathStyle; } }

        public byte WeaponLevel
        {
            get { return (byte)((this.weaponMagicLevel >> 4) & 0x0F); }
            set
            {
                value = (byte)((value & 0x0F) << 4);
                byte magicLevel = (byte)(this.weaponMagicLevel & 0x0F);
                this.SetProperty(ref this.weaponMagicLevel, (byte)(value | magicLevel));
            }
        }

        public byte MagicLevel
        {
            get { return (byte)(this.weaponMagicLevel & 0x0F); }
            set
            {
                value = (byte)(value & 0x0F);
                byte weaponLevel = (byte)(this.weaponMagicLevel & 0xF0);
                this.SetProperty(ref this.weaponMagicLevel, (byte)(value | weaponLevel));
            }
        }

        public byte WeaponMagicLevel { get { return this.weaponMagicLevel; } }

        public ushort GoldAward
        {
            get { return this.goldAward; }
            set { this.SetProperty(ref this.goldAward, value); }
        }

        public EnemyStatEntry(byte index) : base(index, false) { }

        public EnemyStatEntry(byte index,
                              byte level,
                              ushort hitPoints,
                              byte manaPoints,
                              byte strength,
                              byte agility,
                              byte intelligence,
                              byte wisdom,
                              byte evasion,
                              ushort defense,
                              byte magicEvasion,
                              ushort magicDefense,
                              byte monsterType,
                              byte element,
                              ushort experienceAward,
                              byte blackMagicPower,
                              byte whiteMagicPower,
                              ushort immunities,
                              byte unused,
                              byte meleeWeapon,
                              byte rangedWeapon,
                              byte deathStyle,
                              byte weaponMagicLevel,
                              ushort goldAward,
                              bool userModified = false) : base(index, userModified)
        {
            this.level = level;
            this.hitPoints = hitPoints;
            this.manaPoints = manaPoints;
            this.strength = strength;
            this.agility = agility;
            this.intelligence = intelligence;
            this.wisdom = wisdom;
            this.evasion = evasion;
            this.defense = defense;
            this.magicEvasion = magicEvasion;
            this.magicDefense = magicDefense;
            this.monsterType = (MonsterType)monsterType;
            this.element = (ElementalType)element;
            this.experienceAward = experienceAward;
            this.blackMagicPower = blackMagicPower;
            this.whiteMagicPower = whiteMagicPower;
            this.immunities = (StatusEffects)immunities;
            this.Unused = unused;
            this.meleeWeapon = meleeWeapon;
            this.rangedWeapon = rangedWeapon;
            this.deathStyle = deathStyle;
            this.weaponMagicLevel = weaponMagicLevel;
            this.goldAward = goldAward;

            // The ? enemy has 100 evasion, 100 magic evasion, 65,535 defense and 65,535 magic defense.
            // These stats are never used, so we'll cap them at something reasonable so the UI doesn't have to deal with it.
            this.evasion = Math.Min(this.evasion, (byte)99);
            this.defense = Math.Min(this.defense, (ushort)999);
            this.magicEvasion = Math.Min(this.magicEvasion, (byte)99);
            this.magicDefense = Math.Min(this.MagicDefense, (ushort)999);
        }
    }
}