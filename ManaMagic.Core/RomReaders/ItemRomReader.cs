using System.Collections.Generic;
using ManaMagic.Core.Items;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class ItemRomReader : RomReader
    {
        public ItemRomReader(RomFile rom) : base(rom) { }

        public DataTable<ManaWeaponDefinition> ReadWeaponDefinitionTable()
        {
            this.Seek((int)Constants.Bank10.WeaponDefinitionTableAddress);
            List<ManaWeaponDefinition> dataList = new List<ManaWeaponDefinition>((int)Constants.Bank10.WeaponDefinitionTableSize);
            for (int i = 0; i < Constants.Bank10.WeaponDefinitionTableSize; i++)
            {
                byte weaponClass = this.Read();
                ushort statModifiers = this.ReadUInt16();
                byte projectileType = this.Read();
                byte palette = this.Read();
                byte monsterAffinity = this.Read();
                byte critChance = this.Read();
                byte accuracy = this.Read();
                byte power = this.Read();
                ushort statusEffects = this.ReadUInt16();
                byte inflictionRate = this.Read();

                dataList.Add(new ManaWeaponDefinition((byte)i, weaponClass, statModifiers, projectileType, palette, monsterAffinity, critChance, accuracy, power, statusEffects, inflictionRate));
            }
            return new DataTable<ManaWeaponDefinition>(dataList);
        }

        public DataTable<ManaEquipmentDefinition> ReadEquipmentDefinitionTable()
        {
            this.Seek((int)Constants.Bank10.ArmorDefinitionTableAddress);
            List<ManaEquipmentDefinition> dataList = new List<ManaEquipmentDefinition>((int)Constants.Bank10.ArmorDefinitionTableSize);
            for (int i = 0; i < Constants.Bank10.ArmorDefinitionTableSize; i++)
            {
                byte statModifiers = this.Read();
                byte defense = this.Read();
                byte evasion = this.Read();
                byte magicDefense = this.Read();
                byte magicEvasion = this.Read();
                byte equippableBy = this.Read();
                byte element = this.Read();
                ushort resistances = this.ReadUInt16();
                byte unknown = this.Read();

                dataList.Add(new ManaEquipmentDefinition((byte)i, statModifiers, defense, evasion, magicDefense, magicEvasion, equippableBy, element, resistances, unknown));
            }
            return new DataTable<ManaEquipmentDefinition>(dataList);
        }

        public DataTable<ManaItemDefinition> ReadItemDefinitionTable()
        {
            this.Seek((int)Constants.Bank10.ItemDefinitionTableAddress);
            List<ManaItemDefinition> dataList = new List<ManaItemDefinition>((int)Constants.Bank10.ItemDefinitionTableSize);
            for (int i = 0; i < Constants.Bank10.ItemDefinitionTableSize; i++)
            {
                byte unused00 = this.Read();
                byte unused01 = this.Read();
                byte amountHealed = this.Read();
                byte unused03 = this.Read();
                byte unused04 = this.Read();
                byte unused05 = this.Read();
                byte unused06 = this.Read();
                byte paletteIndex = this.Read();
                ushort probablyItemGraphicsPointer = this.ReadUInt16();
                ushort probablyPlayerGraphicsPointer = this.ReadUInt16();
                ushort animationPointer = this.ReadUInt16();
                byte unused0E = this.Read();
                byte unused0F = this.Read();

                dataList.Add(new ManaItemDefinition((byte)i, 
                                                    unused00,
                                                    unused01,
                                                    amountHealed,
                                                    unused03,
                                                    unused04,
                                                    unused05,
                                                    unused06,
                                                    paletteIndex,
                                                    probablyItemGraphicsPointer,
                                                    probablyPlayerGraphicsPointer,
                                                    animationPointer,
                                                    unused0E,
                                                    unused0F));
            }
            return new DataTable<ManaItemDefinition>(dataList);
        }

        public DataTable<UShortValue> ReadConsumableItemPriceTable()
        {
            return this.ReadPriceTable((int)Constants.Bank18.ConsumableItemPriceTableAddress, Constants.Bank18.ConsumableItemPriceTableSize);
        }

        public DataTable<UShortValue> ReadHelmetPriceTable()
        {
            return this.ReadPriceTable((int)Constants.Bank18.HelmetPriceTableAddress, Constants.Bank18.HelmetPriceTableSize);
        }

        public DataTable<UShortValue> ReadArmorPriceTable()
        {
            return this.ReadPriceTable((int)Constants.Bank18.ArmorPriceTableAddress, Constants.Bank18.ArmorPriceTableSize);
        }

        public DataTable<UShortValue> ReadAccessoryPriceTable()
        {
            return this.ReadPriceTable((int)Constants.Bank18.AccessoryPriceTableAddress, Constants.Bank18.AccessoryPriceTableSize);
        }

        public DataTable<UShortValue> ReadWeaponUpgradePriceTable()
        {
            return this.ReadPriceTable((int)Constants.Bank18.WeaponUpgradePriceTableAddress, Constants.Bank18.WeaponUpgradePriceTableSize);
        }

        public DataTable<ManaShop> ReadShopTable()
        {
            List<ManaShop> dataList = new List<ManaShop>((int)Constants.Bank18.ShopPointerTableSize);
            for (int i = 0; i < Constants.Bank18.ShopPointerTableSize; i++)
            {
                this.Seek((int)Constants.Bank18.ShopPointerTableAddress + (i * sizeof(ushort)));
                ushort offset = this.ReadUInt16();
                this.Seek(Constants.Bank18Offset | offset);

                List<ShopItem> shopData = new List<ShopItem>();
                byte index = 0;
                while (true)
                {
                    byte item = this.Read();
                    if (item == Constants.Bank18.ShopTableEndMarker) { break; }

                    shopData.Add((ShopItem)item);
                    index++;
                }
                dataList.Add(new ManaShop((byte)i, shopData));
            }
            return new DataTable<ManaShop>(dataList);
        }

        private DataTable<UShortValue> ReadPriceTable(int address, int size)
        {
            this.Seek(address);
            List<UShortValue> dataList = new List<UShortValue>(size);
            for (int i = 0; i < size; i++)
            {
                ushort price = this.ReadUInt16();

                dataList.Add(new UShortValue((byte)i, price));
            }
            return new DataTable<UShortValue>(dataList);
        }
    }
}