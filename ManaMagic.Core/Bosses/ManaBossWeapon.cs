using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Bosses
{
    public sealed record ManaBossWeapon : NotifyRecordPropertyChanged
    {
        public static ManaBossWeapon Empty { get; } = new ManaBossWeapon(0, MonsterType.None, ElementalType.None, 0, 0, StatusEffects.None, 0);

        private MonsterType monsterAffinity = MonsterType.None;
        private ElementalType element = ElementalType.None;
        private byte accuracy = 0;
        private byte power = 0;
        private StatusEffects statusEffects = StatusEffects.None;
        private byte inflictionRate = 0;

        public MonsterType MonsterAffinity
        {
            get { return this.monsterAffinity; }
            set { this.SetProperty(ref this.monsterAffinity, value); }
        }

        public ElementalType Element
        {
            get { return this.element; }
            set { this.SetProperty(ref this.element, value); }
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

        public ManaBossWeapon(byte index, MonsterType affinity, ElementalType element, byte accuracy, byte power, StatusEffects statusEffects, byte inflictionRate, bool userModified = false) : base(index, userModified)
        {
            this.monsterAffinity = affinity;
            this.element = element;
            this.accuracy = accuracy;
            this.power = power;
            this.statusEffects = statusEffects;
            this.inflictionRate = inflictionRate;
        }
    }
}