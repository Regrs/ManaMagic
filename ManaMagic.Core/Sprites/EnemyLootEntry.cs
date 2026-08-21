using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed record EnemyLootEntry : NotifyRecordPropertyChanged
    {
        private byte dropRate = 0;
        private byte trapInfo = 0;
        private byte rareDropChance = 0;

        public byte DropRate
        {
            get { return (byte)(this.dropRate & 0x3F); }
            set
            {
                value &= 0x3F;
                value |= (byte)(this.dropRate & 0xC0);

                this.SetProperty(ref this.dropRate, value);
            }
        }

        public byte RareDropChance
        {
            get { return (byte)((this.rareDropChance & 0x38) >> 3); }
            set
            {
                value = (byte)((value & 0x07) << 3);
                value |= (byte)(this.rareDropChance & 0xC7);

                this.SetProperty(ref this.rareDropChance, value);
            }
        }

        public bool Flees
        {
            get { return (this.dropRate & 0x80) > 0; }
            set
            {
                byte newValue = this.dropRate;
                if (value) { newValue |= 0x80; }
                else { newValue &= 0x7F; }
                this.SetProperty(ref this.dropRate, newValue);
            }
        }

        public bool AlwaysDrops
        {
            get { return (this.dropRate & 0x40) > 0; }
            set
            {
                byte newValue = this.dropRate;
                if (value) { newValue |= 0x40; }
                else { newValue &= 0xBF; }
                this.SetProperty(ref this.dropRate, newValue);
            }
        }

        public byte AgilityMultiplier
        {
            get { return (byte)((byte)((this.trapInfo & 0xF0) >> 1) / 8); }
            set
            {
                value &= 0x0F;
                value *= 8;
                value <<= 1;
                value |= (byte)(this.trapInfo & 0x0F);
                this.SetProperty(ref this.trapInfo, value);
            }
        }

        public byte DisarmLevel
        {
            get { return (byte)(this.trapInfo & 0x0F); }
            set
            {
                value &= 0x0F;
                value |= (byte)(this.trapInfo & 0xF0);
                this.SetProperty(ref this.trapInfo, value);
            }
        }

        public DropEntry CommonLoot { get; }
        public DropEntry RareLoot { get; }

        public double DropRatePercent { get { return this.DropRate / 64d; } }
        public double RareDropChancePercent { get { return this.RareDropChance / 64d; } }
        public byte DodgeThreshold { get { return (byte)(this.AgilityMultiplier * 8); } }

        public EnemyLootEntry(byte index, byte dropRate, byte trapInfo, byte rareDropChance, byte commonDrop, byte rareDrop, bool userModified = false) : base(index, userModified)
        {
            this.dropRate = dropRate;
            this.trapInfo = trapInfo;
            this.rareDropChance = rareDropChance;

            LootDropType commonDropType = (rareDropChance & 0x40) > 0 ? EnemyLootEntry.GetLootDropType(commonDrop) : LootDropType.Gold;
            LootDropType rareDropType = (rareDropChance & 0x80) > 0 ? LootDropType.Gold : EnemyLootEntry.GetLootDropType(rareDrop);

            this.CommonLoot = new DropEntry(commonDropType, commonDrop);
            this.RareLoot = new DropEntry(rareDropType, rareDrop);

            this.CommonLoot.PropertyChanged += this.CommonLoot_PropertyChanged;
            this.RareLoot.PropertyChanged += this.RareLoot_PropertyChanged;
        }

        public void GetEncodedValues(Span<byte> bytes)
        {
            if (bytes.Length < 5)
            {
                ThrowHelper.ThrowArgumentException("Array must be at least 5 bytes", nameof(bytes));
            }

            bytes.Fill(0);
            bytes[0] = this.dropRate;
            bytes[1] = this.trapInfo;
            bytes[2] = this.rareDropChance;
            bytes[3] = this.CommonLoot.Value;
            bytes[4] = this.RareLoot.Value;
        }

        private void CommonLoot_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DropEntry.DropType))
            {
                if (this.CommonLoot.IsGold) { this.rareDropChance &= 0xBF; }
                else { this.rareDropChance |= 0x40; }
            }

            this.OnPropertyChanged(nameof(this.CommonLoot));
        }

        private void RareLoot_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DropEntry.DropType))
            {
                if (this.CommonLoot.IsGold) { this.rareDropChance |= 0x80; }
                else { this.rareDropChance &= 0x7F; }
            }

            this.OnPropertyChanged(nameof(this.RareLoot));
        }

        private static LootDropType GetLootDropType(byte value)
        {
            if (value <= 0x14) { return LootDropType.Helmet; }
            else if (value <= 0x29) { return LootDropType.Armor; }
            else if (value <= 0x3E) { return LootDropType.Accessory; }
            if (value <= 0x4B) { return LootDropType.Consumable; }
            else if (value >= 0x80 && value <= 0x87) { return LootDropType.WeaponOrb; }

            // Invalid entries become gold. Invalid entries aren't used anyway.
            return LootDropType.Gold;
        }
    }
}