using ManaMagic.Core.Character;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class CharacterRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public CharacterRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteStatsByLevelTables(CharacterContext context)
        {
            this.WriteStatsByLevelTable((int)Constants.Bank10.RandiStatTableAddress, context.RandiStatsByLevelTable);
            this.WriteStatsByLevelTable((int)Constants.Bank10.PurimStatTableAddress, context.PurimStatsByLevelTable);
            this.WriteStatsByLevelTable((int)Constants.Bank10.PopoieStatTableAddress, context.PopoieStatsByLevelTable);
        }

        public void WriteExperiencePerLevelTable(CharacterContext context)
        {
            this.Seek(Constants.Bank10.ExperiencePerLevelTableAddress);
            foreach (ExperiencePerLevel experienceByLevel in context.ExperiencePerLevelTable)
            {
                this.WriteUInt24(experienceByLevel.Experience);
            }
        }

        public void WriteDefaultCharacterDataTable(CharacterContext context)
        {
            this.Seek(Constants.Bank00.DefaultCharacterDataTableAddress);

            this.WriteUInt16(context.DefaultCharacterDataTable.Gold);
            this.Write(context.DefaultCharacterDataTable.SealedManaSeeds);
            this.Write(context.DefaultCharacterDataTable.SaveLocation);

            this.Write(context.DefaultCharacterDataTable.RandiActionGridQuadrant);
            this.Write(context.DefaultCharacterDataTable.PurimActionGridQuadrant);
            this.Write(context.DefaultCharacterDataTable.PopoieActionGridQuadrant);

            this.Write(context.DefaultCharacterDataTable.RandiActionGridLocation);
            this.Write(context.DefaultCharacterDataTable.PurimActionGridLocation);
            this.Write(context.DefaultCharacterDataTable.PopoieActionGridLocation);

            this.Write((byte)context.DefaultCharacterDataTable.RandiHelmet);
            this.Write((byte)context.DefaultCharacterDataTable.RandiArmor);
            this.Write((byte)context.DefaultCharacterDataTable.RandiAccessory);
            this.Write((byte)context.DefaultCharacterDataTable.RandiWeapon);

            this.Write((byte)context.DefaultCharacterDataTable.PurimHelmet);
            this.Write((byte)context.DefaultCharacterDataTable.PurimArmor);
            this.Write((byte)context.DefaultCharacterDataTable.PurimAccessory);
            this.Write((byte)context.DefaultCharacterDataTable.PurimWeapon);

            this.Write((byte)context.DefaultCharacterDataTable.PopoieHelmet);
            this.Write((byte)context.DefaultCharacterDataTable.PopoieArmor);
            this.Write((byte)context.DefaultCharacterDataTable.PopoieAccessory);
            this.Write((byte)context.DefaultCharacterDataTable.PopoieWeapon);
        }

        private void WriteStatsByLevelTable(int address, DataTable<CharacterStatsByLevel> statTable)
        {
            this.Seek(address);
            foreach (CharacterStatsByLevel statsByLevel in statTable)
            {
                this.WriteUInt16(statsByLevel.HitPoints);
                this.Write(statsByLevel.ManaPoints);
                this.Write(statsByLevel.Strength);
                this.Write(statsByLevel.Agility);
                this.Write(statsByLevel.Constitution);
                this.Write(statsByLevel.Intelligence);
                this.Write(statsByLevel.Wisdom);
            }
        }
    }
}