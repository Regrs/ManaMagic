using System;
using ManaMagic.Core.Sprites;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class SpriteRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public SpriteRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteEnemyStatisticsTable(SpriteContext context)
        {
            this.Seek(Constants.Bank10.SpriteStatisticsTableAddress);
            foreach (EnemyStatEntry entry in context.EnemyStatisticsTable)
            {
                this.Write(entry.Level);
                this.WriteUInt16(entry.HitPoints);
                this.Write(entry.ManaPoints);
                this.Write(entry.Strength);
                this.Write(entry.Agility);
                this.Write(entry.Intelligence);
                this.Write(entry.Wisdom);
                this.Write(entry.Evasion);
                this.WriteUInt16(entry.Defense);
                this.Write(entry.MagicEvasion);
                this.WriteUInt16(entry.MagicDefense);
                this.Write((byte)entry.MonsterType);
                this.Write((byte)entry.Element);
                this.WriteUInt16(entry.ExperienceAward);
                this.Write(entry.BlackMagicPower);
                this.Write(entry.WhiteMagicPower);
                this.WriteUInt16((ushort)entry.Immunities);
                this.Write(entry.Unused);
                this.Write(entry.MeleeWeapon);
                this.Write(entry.RangedWeapon);
                this.Write(entry.DeathStyleAsByte);
                this.Write(entry.WeaponMagicLevel);
                this.WriteUInt16(entry.GoldAward);
            }
        }

        public void WriteLootTable(SpriteContext context)
        {
            this.Seek(Constants.Bank10.EnemyLootTableAddress);

            Span<byte> lootTableValues = stackalloc byte[5];
            foreach (EnemyLootEntry entry in context.LootTable)
            {
                entry.GetEncodedValues(lootTableValues);
                this.Write(lootTableValues[0]);
                this.Write(lootTableValues[1]);
                this.Write(lootTableValues[2]);
                this.Write(lootTableValues[3]);
                this.Write(lootTableValues[4]);
            }
        }

        public void WriteSpritePaletteTable(SpriteContext context)
        {
            this.Seek(Constants.Bank08.SpritePaletteTableAddress);
            for (int i = 0; i < context.PaletteTable.RowCount; i++)
            {
                // Skip the first palette, this the transparency color, which the game will automatically add for every normal sprite.
                SpritePalette palette = context.PaletteTable[i];
                for (int colorIndex = 1; colorIndex < palette.NumberOfColors; colorIndex++)
                {
                    this.WriteUInt16(palette[colorIndex].ToRgb555(Rgb555Format.BGR));
                }
            }
        }
    }
}