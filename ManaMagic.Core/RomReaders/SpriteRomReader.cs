using System.Collections.Generic;
using ManaMagic.Core.Sprites;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class SpriteRomReader : RomReader
    {
        public SpriteRomReader(RomFile rom) : base(rom) { }

        public DataTable<SpritePalette> ReadSpritePaletteTable()
        {
            this.Seek((int)Constants.Bank08.SpritePaletteTableAddress);

            List<SpritePalette> paletteList = new List<SpritePalette>((int)Constants.Bank08.SpritePaletteTableSize);
            for (int i = 0; i < Constants.Bank08.SpritePaletteTableSize; i++)
            {
                List<Rgb555Color> colorList = new List<Rgb555Color>((int)Constants.Bank08.NumberOfColorsPerSpritePalette + 1);

                // The game adds the color white as the tranparency color for every sprite palette.
                colorList.Add(Rgb555Color.White);
                for (int colorIndex = 0; colorIndex < Constants.Bank08.NumberOfColorsPerSpritePalette; colorIndex++)
                {
                    colorList.Add(Rgb555Color.FromRgb(this.ReadUInt16(), Rgb555Format.BGR));
                }
                paletteList.Add(new SpritePalette((byte)i, colorList));
            }
            return new DataTable<SpritePalette>(paletteList);
        }

        public DataTable<SpriteGraphicsData> ReadSpriteGraphicsDataTable()
        {
            this.Seek((int)Constants.Bank10.SpriteGraphicsTableAddress);
            List<SpriteGraphicsData> dataList = new List<SpriteGraphicsData>((int)Constants.Bank10.SpriteGraphicsTableSize);
            for (int i = 0; i <= Constants.Bank10.SpriteGraphicsTableSize; i++)
            {
                ushort encodedGfxOffset = this.ReadUInt16(); // 0001
                ushort frameIndexOffset = this.ReadUInt16(); // 0203
                ushort unknown2 = this.ReadUInt16();         // 0405
                ushort unknown3 = this.ReadUInt16();         // 0607
                ushort unknown4 = this.ReadUInt16();         // 0809
                ushort unknown5 = this.ReadUInt16();         // 0A0B
                ushort unknown6 = this.ReadUInt16();         // 0C0D
                ushort unknown7 = this.ReadUInt16();         // 0E0F

                dataList.Add(new SpriteGraphicsData(encodedGfxOffset, frameIndexOffset, unknown2, unknown3, unknown4, unknown5, unknown6, unknown7));
            }
            return new DataTable<SpriteGraphicsData>(dataList);
        }

        public DataTable<EnemyStatEntry> ReadEnemyStatisticsTable()
        {
            this.Seek((int)Constants.Bank10.SpriteStatisticsTableAddress);
            List<EnemyStatEntry> dataList = new List<EnemyStatEntry>((int)Constants.Bank10.SpriteStatisticsTableSize);
            for (int i = 0; i < Constants.Bank10.SpriteStatisticsTableSize; i++)
            {
                byte level = this.Read();
                ushort hitPoints = this.ReadUInt16();
                byte manaPoints = this.Read();
                byte strength = this.Read();
                byte agility = this.Read();
                byte intelligence = this.Read();
                byte wisdom = this.Read();
                byte evasion = this.Read();
                ushort defense = this.ReadUInt16();
                byte magicEvasion = this.Read();
                ushort magicDefense = this.ReadUInt16();
                byte monsterType = this.Read();
                byte element = this.Read();
                ushort experienceAward = this.ReadUInt16();
                byte blackMagicPower = this.Read();
                byte whiteMagicPower = this.Read();
                ushort immunities = this.ReadUInt16();
                byte unused = this.Read();
                byte meleeWeapon = this.Read();
                byte rangedWeapon = this.Read();
                byte deathStyle = this.Read();
                byte wmLevel = this.Read();
                ushort goldAward = this.ReadUInt16();

                EnemyStatEntry entry = new EnemyStatEntry((byte)i,
                                                          level,
                                                          hitPoints,
                                                          manaPoints,
                                                          strength,
                                                          agility,
                                                          intelligence,
                                                          wisdom,
                                                          evasion,
                                                          defense,
                                                          magicEvasion,
                                                          magicDefense,
                                                          monsterType,
                                                          element,
                                                          experienceAward,
                                                          blackMagicPower,
                                                          whiteMagicPower,
                                                          immunities,
                                                          unused,
                                                          meleeWeapon,
                                                          rangedWeapon,
                                                          deathStyle,
                                                          wmLevel,
                                                          goldAward);
                dataList.Add(entry);
            }
            return new DataTable<EnemyStatEntry>(dataList);
        }

        public DataTable<EnemyLootEntry> ReadEnemyLootTable()
        {
            this.Seek((int)Constants.Bank10.EnemyLootTableAddress);
            List<EnemyLootEntry> dataList = new List<EnemyLootEntry>((int)Constants.Bank10.EnemyLootTableAddressSize);
            for (int i = 0; i < Constants.Bank10.EnemyLootTableAddressSize; i++)
            {
                byte dropRate = this.Read();
                byte trapData = this.Read();
                byte rareDropChance = this.Read();
                byte commonDrop = this.Read();
                byte rareDrop = this.Read();

                dataList.Add(new EnemyLootEntry((byte)i, dropRate, trapData, rareDropChance, commonDrop, rareDrop));
            }
            return new DataTable<EnemyLootEntry>(dataList);
        }
    }
}