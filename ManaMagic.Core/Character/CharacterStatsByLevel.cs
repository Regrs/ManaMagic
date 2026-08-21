using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Character
{
    public sealed record CharacterStatsByLevel : NotifyRecordPropertyChanged
    {
        private ushort hitPoints = 0;
        private byte manaPoints = 0;
        private byte strength = 0;
        private byte agility = 0;
        private byte constitution = 0;
        private byte intelligence = 0;
        private byte wisdom = 0;

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

        public byte Constitution
        {
            get { return this.constitution; }
            set { this.SetProperty(ref this.constitution, value); }
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

        public CharacterStatsByLevel(byte index, ushort hitPoints, byte manaPoints, byte strength, byte agility, byte constitution, byte intelligence, byte wisdom, bool userModified = false) : base(index, userModified)
        {
            this.hitPoints = hitPoints;
            this.manaPoints = manaPoints;
            this.strength = strength;
            this.agility = agility;
            this.constitution = constitution;
            this.intelligence = intelligence;
            this.wisdom = wisdom;
        }
    }
}
/*
[DataTable_Randi_StatsByLevel]
        MXHP MP ST AG CO IN WS
        0001 02 03 04 05 06 07
104210:	3200 00 0F 0F 0D 05 05 [Level 01]
 */