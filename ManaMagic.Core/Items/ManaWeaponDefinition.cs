using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Items
{
    public sealed record ManaWeaponDefinition : NotifyRecordPropertyChanged
    {
        private WeaponClassType weaponClass = WeaponClassType.PlayerGlove;
        private byte unknownStatIndex = 0;
        private WeaponStatModifier strengthModifier = WeaponStatModifier.None;
        private WeaponStatModifier agilityModifier = WeaponStatModifier.None;
        private WeaponStatModifier constitutionModifier = WeaponStatModifier.None;
        private WeaponStatModifier intelligenceModifier = WeaponStatModifier.None;
        private WeaponStatModifier wisdomModifier = WeaponStatModifier.None;
        private WeaponProjectileType projectileType = WeaponProjectileType.None;
        private byte paletteIndex = 0;
        private MonsterType affinity = MonsterType.None;
        private byte criticalChance = 0;
        private byte accuracy = 0;
        private byte power = 0;
        private StatusEffects statusEffects = StatusEffects.None;
        private byte inflictionRate = 0;

        public WeaponClassType WeaponClass
        {
            get { return this.weaponClass; }
            set { this.SetProperty(ref this.weaponClass, value); }
        }

        public ushort StatModifiers
        {
            get
            {
                return (ushort)((ushort)(this.UnknownStatIndex << 14) |
                       (ushort)((ushort)this.StrengthModifier << 8) |
                       (ushort)((ushort)this.AgilityModifier << 6) |
                       (ushort)((ushort)this.ConstitutionModifier << 4) |
                       (ushort)((ushort)this.IntelligenceModifier << 2) |
                                (ushort)this.WisdomModifier);
            }
        }

        public byte UnknownStatIndex
        {
            get { return this.unknownStatIndex; }
            set { this.SetProperty(ref this.unknownStatIndex, value); }
        }

        public WeaponStatModifier StrengthModifier
        {
            get { return this.strengthModifier; }
            set { this.SetProperty(ref this.strengthModifier, value); }
        }

        public WeaponStatModifier AgilityModifier
        {
            get { return this.agilityModifier; }
            set { this.SetProperty(ref this.agilityModifier, value); }
        }

        public WeaponStatModifier ConstitutionModifier
        {
            get { return this.constitutionModifier; }
            set { this.SetProperty(ref this.constitutionModifier, value); }
        }

        public WeaponStatModifier IntelligenceModifier
        {
            get { return this.intelligenceModifier; }
            set { this.SetProperty(ref this.intelligenceModifier, value); }
        }

        public WeaponStatModifier WisdomModifier
        {
            get { return this.wisdomModifier; }
            set { this.SetProperty(ref this.wisdomModifier, value); }
        }

        public WeaponProjectileType ProjectileType
        {
            get { return this.projectileType; }
            set { this.SetProperty(ref this.projectileType, value); }
        }

        public byte PaletteIndex
        {
            get { return this.paletteIndex; }
            set { this.SetProperty(ref this.paletteIndex, value); }
        }

        public MonsterType Affinity
        {
            get { return this.affinity; }
            set { this.SetProperty(ref this.affinity, value); }
        }

        public byte CriticalChance
        {
            get { return this.criticalChance; }
            set { this.SetProperty(ref this.criticalChance, value); }
        }

        public byte Accuracy
        {
            get { return this.accuracy; }
            set { this.SetProperty(ref this.accuracy, value); }
        }

        public byte Power
        {
            get { return this.power; }
            set { this.SetProperty(ref this.power, value); }
        }

        public StatusEffects StatusEffects
        {
            get { return this.statusEffects; }
            set { this.SetProperty(ref this.statusEffects, value); }
        }

        public byte InflictionRate
        {
            get { return this.inflictionRate; }
            set { this.SetProperty(ref this.inflictionRate, value); }
        }

        public ManaWeaponDefinition(byte index, byte weaponClass, ushort statModifiers, byte projectileType, byte palette, byte monsterAffinity, byte critChance, byte accuracy, byte power, ushort statusEffects, byte inflictionRate, bool userModified = false) : base(index, userModified)
        {
            this.weaponClass = (WeaponClassType)weaponClass;

            this.unknownStatIndex = (byte)((statModifiers & 0xC000) >> 14);
            this.strengthModifier = (WeaponStatModifier)((statModifiers & 0x0300) >> 8);
            this.agilityModifier = (WeaponStatModifier)((statModifiers & 0x00C0) >> 6);
            this.constitutionModifier = (WeaponStatModifier)((statModifiers & 0x0030) >> 4);
            this.intelligenceModifier = (WeaponStatModifier)((statModifiers & 0x000C) >> 2);
            this.wisdomModifier = (WeaponStatModifier)(statModifiers & 0x0003);

            this.projectileType = (WeaponProjectileType)projectileType;
            this.paletteIndex = palette;
            this.affinity = (MonsterType)monsterAffinity;
            this.criticalChance = critChance;
            this.accuracy = accuracy;
            this.power = power;
            this.statusEffects = (StatusEffects)statusEffects;
            this.inflictionRate = inflictionRate;
        }
    }
}
/*
[Table_PlayerEnemyWeapons]
;     -Legend-
;     TP:   Weapon Type
;     STAT: Stat Modifiers Provided By The Weapon
;     PJ:   Projectile Type
;     PL:   Palette
;     MA:   Monster Affinity
;     CC:   Critical Chance
;     AC:   Accuracy
;     PW:   Power
;     S1S2: Status Effect
;     SR:   Status Effect Rate

        TP STAT PJ PL MA CC AC PW S1S2 SR
101000: 00 00C0 00 02 00 00 4B 02 0000 50 [00: Spike Knuckle]
*/