using System.Collections.Generic;
using ManaMagic.Core.Character;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class CharacterRomReader : RomReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterRomReader"/> class with the specified <see cref="RomFile"/>.
        /// </summary>
        /// <param name="rom">A <see cref="RomFile"/> from which data will be read.</param>
        public CharacterRomReader(RomFile rom) : base(rom) { }

        public DataTable<CharacterStatsByLevel> ReadRandiStatsByLevelTable()
        {
            this.Seek((int)Constants.Bank10.RandiStatTableAddress);
            List<CharacterStatsByLevel> dataList = new List<CharacterStatsByLevel>((int)Constants.Bank10.RandiStatTableAddressSize);
            for (int i = 0; i < Constants.Bank10.RandiStatTableAddressSize; i++)
            {
                ushort hitPoints = this.ReadUInt16();
                byte manaPoints = this.Read();
                byte strength = this.Read();
                byte agility = this.Read();
                byte constitution = this.Read();
                byte intelligence = this.Read();
                byte wisdom = this.Read();

                dataList.Add(new CharacterStatsByLevel((byte)i, hitPoints, manaPoints, strength, agility, constitution, intelligence, wisdom));
            }
            return new DataTable<CharacterStatsByLevel>(dataList);
        }

        public DataTable<CharacterStatsByLevel> ReadPurimStatsByLevelTable()
        {
            this.Seek((int)Constants.Bank10.PurimStatTableAddress);
            List<CharacterStatsByLevel> dataList = new List<CharacterStatsByLevel>((int)Constants.Bank10.PurimStatTableAddressSize);
            for (int i = 0; i < Constants.Bank10.PurimStatTableAddressSize; i++)
            {
                ushort hitPoints = this.ReadUInt16();
                byte manaPoints = this.Read();
                byte strength = this.Read();
                byte agility = this.Read();
                byte constitution = this.Read();
                byte intelligence = this.Read();
                byte wisdom = this.Read();

                dataList.Add(new CharacterStatsByLevel((byte)i, hitPoints, manaPoints, strength, agility, constitution, intelligence, wisdom));
            }
            return new DataTable<CharacterStatsByLevel>(dataList);
        }

        public DataTable<CharacterStatsByLevel> ReadPopoieStatsByLevelTable()
        {
            this.Seek((int)Constants.Bank10.PopoieStatTableAddress);
            List<CharacterStatsByLevel> dataList = new List<CharacterStatsByLevel>((int)Constants.Bank10.PopoieStatTableAddressSize);
            for (int i = 0; i < Constants.Bank10.PopoieStatTableAddressSize; i++)
            {
                ushort hitPoints = this.ReadUInt16();
                byte manaPoints = this.Read();
                byte strength = this.Read();
                byte agility = this.Read();
                byte constitution = this.Read();
                byte intelligence = this.Read();
                byte wisdom = this.Read();

                dataList.Add(new CharacterStatsByLevel((byte)i, hitPoints, manaPoints, strength, agility, constitution, intelligence, wisdom));
            }
            return new DataTable<CharacterStatsByLevel>(dataList);
        }

        public DataTable<ExperiencePerLevel> ReadExperiencePerLevelTable()
        {
            this.Seek((int)Constants.Bank10.ExperiencePerLevelTableAddress);
            List<ExperiencePerLevel> dataList = new List<ExperiencePerLevel>((int)Constants.Bank10.ExperiencePerLevelTableAddressSize);
            for (byte i = 0; i < Constants.Bank10.ExperiencePerLevelTableAddressSize; i++)
            {
                uint exp = this.ReadUInt24();
                dataList.Add(new ExperiencePerLevel(i, exp));
            }
            return new DataTable<ExperiencePerLevel>(dataList);
        }

        public ManaDefaultCharacterData ReadDefaultCharacterDataTable()
        {
            this.Seek((int)Constants.Bank00.DefaultCharacterDataTableAddress);

            ushort gold = this.ReadUInt16();
            byte sealedManaSeeds = this.Read();
            byte saveLocation = this.Read(); // This feels like an off-by-one error and should be setting player & enemy sealed seeds since this does nothing.
            byte randiActionGridQuadrant = this.Read();
            byte purimActionGridQuadrant = this.Read();
            byte popoieActionGridQuadrant = this.Read();
            byte randiActionGridLocation = this.Read();
            byte purimActionGridLocation = this.Read();
            byte popoieActionGridLocation = this.Read();

            byte randiHelmet = this.Read();
            byte randiArmor = this.Read();
            byte randiAccessory = this.Read();
            byte randiWeapon = this.Read();

            byte purimHelmet = this.Read();
            byte purimArmor = this.Read();
            byte purimAccessory = this.Read();
            byte purimWeapon = this.Read();

            byte popoieHelmet = this.Read();
            byte popoieArmor = this.Read();
            byte popoieAccessory = this.Read();
            byte popoieWeapon = this.Read();

            ManaDefaultCharacterData defaultData = new ManaDefaultCharacterData(0,
                                                                                gold,
                                                                                sealedManaSeeds,
                                                                                saveLocation,
                                                                                randiActionGridQuadrant,
                                                                                purimActionGridQuadrant,
                                                                                popoieActionGridQuadrant,
                                                                                randiActionGridLocation,
                                                                                purimActionGridLocation,
                                                                                popoieActionGridLocation,
                                                                                randiHelmet,
                                                                                randiArmor,
                                                                                randiAccessory,
                                                                                randiWeapon,
                                                                                purimHelmet,
                                                                                purimArmor,
                                                                                purimAccessory,
                                                                                purimWeapon,
                                                                                popoieHelmet,
                                                                                popoieArmor,
                                                                                popoieAccessory,
                                                                                popoieWeapon);

            return defaultData;
        }
    }
}