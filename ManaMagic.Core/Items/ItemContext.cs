using System.Collections.Generic;
using ManaMagic.Core.Events;
using ManaMagic.Core.Menu;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Items
{
    public sealed class ItemContext
    {
        public const HelmetType FirstHelmet = HelmetType.BareHead;
        public const HelmetType LastHelmet = HelmetType.FaerieCrown;
        public const ArmorType FirstArmor = ArmorType.No;
        public const ArmorType LastArmor = ArmorType.FaerieCloak;
        public const AccessoryType FirstAccessory = AccessoryType.Nothing;
        public const AccessoryType LastAccessory = AccessoryType.DummiedItem;
        public const ushort CannotSellItem = ushort.MaxValue;

        public DataTable<ManaWeaponDefinition> WeaponDefinitionTable { get; private set; } = DataTable<ManaWeaponDefinition>.Empty;
        public DataTable<ManaEquipmentDefinition> EquipmentDefinitionTable { get; private set; } = DataTable<ManaEquipmentDefinition>.Empty;
        public DataTable<ManaItemDefinition> ItemDefinitionTable { get; private set; } = DataTable<ManaItemDefinition>.Empty;

        public DataTable<UShortValue> ConsumableItemPriceTable { get; private set; } = DataTable<UShortValue>.Empty;
        public DataTable<UShortValue> HelmetPriceTable { get; private set; } = DataTable<UShortValue>.Empty;
        public DataTable<UShortValue> ArmorPriceTable { get; private set; } = DataTable<UShortValue>.Empty;
        public DataTable<UShortValue> AccessoryPriceTable { get; private set; } = DataTable<UShortValue>.Empty;
        public DataTable<UShortValue> WeaponUpgradePriceTable { get; private set; } = DataTable<UShortValue>.Empty;
        public DataTable<ManaShop> ShopTable { get; private set; } = DataTable<ManaShop>.Empty;

        public DataTable<ManaWeapon> Weapons { get; private set; } = DataTable<ManaWeapon>.Empty;
        public DataTable<ManaEquipment> Helmets { get; private set; } = DataTable<ManaEquipment>.Empty;
        public DataTable<ManaEquipment> Armor { get; private set; } = DataTable<ManaEquipment>.Empty;
        public DataTable<ManaEquipment> Accessories { get; private set; } = DataTable<ManaEquipment>.Empty;
        public DataTable<ManaItem> Items { get; private set; } = DataTable<ManaItem>.Empty;

        /// <summary>
        /// Initializes the context by reading item data from the ROM file.
        /// </summary>
        public void Initialize(TextContext textContext, MenuContext menuContext)
        {
            ItemRomReader reader = RomReaderFactory.GetRomReader<ItemRomReader>();

            this.WeaponDefinitionTable = reader.ReadWeaponDefinitionTable();
            this.EquipmentDefinitionTable = reader.ReadEquipmentDefinitionTable();
            this.ItemDefinitionTable = reader.ReadItemDefinitionTable();

            this.ConsumableItemPriceTable = reader.ReadConsumableItemPriceTable();
            this.HelmetPriceTable = reader.ReadHelmetPriceTable();
            this.ArmorPriceTable = reader.ReadArmorPriceTable();
            this.AccessoryPriceTable = reader.ReadAccessoryPriceTable();
            this.WeaponUpgradePriceTable = reader.ReadWeaponUpgradePriceTable();
            this.ShopTable = reader.ReadShopTable();

            this.Weapons = this.CreateFullWeaponTable(textContext, menuContext);
            this.Helmets = this.CreateFullEquipmentTable((int)ItemContext.FirstHelmet, (int)ItemContext.LastHelmet, textContext, menuContext.HelmetRingIcons);
            this.Armor = this.CreateFullEquipmentTable((int)ItemContext.FirstArmor, (int)ItemContext.LastArmor, textContext, menuContext.ArmorRingIcons);
            this.Accessories = this.CreateFullEquipmentTable((int)ItemContext.FirstAccessory, (int)ItemContext.LastAccessory, textContext, menuContext.AccessoryRingIcons);
            this.Items = this.CreateFullItemTable(textContext, menuContext);
        }

        private DataTable<ManaWeapon> CreateFullWeaponTable(TextContext textContext, MenuContext menuContext)
        {
            int index = 0;
            List<ManaWeapon> weaponList = new List<ManaWeapon>(this.WeaponDefinitionTable.RowCount);
            foreach (ManaWeaponDefinition definition in this.WeaponDefinitionTable)
            {
                ManaEvent name = ManaEvent.Empty;
                ManaEvent description = ManaEvent.Empty;
                RingIcon icon = RingIcon.Empty;
                if (index < Constants.Bank0A.WeaponNamesPointerTableSize)
                {
                    name = textContext.WeaponNameTable[index];
                    description = textContext.WeaponDescriptionTable[index];
                    icon = menuContext.WeaponRingIcons[index];
                }

                ManaWeapon weapon = new ManaWeapon((byte)index, name, description, icon, definition);
                weapon.SetListeners(textContext.WeaponNameTable, textContext.WeaponDescriptionTable, this.WeaponDefinitionTable);
                weaponList.Add(weapon);
                index++;
            }

            return new DataTable<ManaWeapon>(weaponList);
        }

        private DataTable<ManaEquipment> CreateFullEquipmentTable(int minIndex, int maxIndex, TextContext textContext, DataTable<RingIcon> icons)
        {
            int iconIndex = 0;
            List<ManaEquipment> equipmentList = new List<ManaEquipment>(this.WeaponDefinitionTable.RowCount);
            for (int index = minIndex; index <= maxIndex; index++)
            {
                ManaEquipmentDefinition definition = this.EquipmentDefinitionTable[index];
                ManaEvent name = textContext.EquipmentNameTable[index];
                RingIcon icon = iconIndex < icons.RowCount ? icons[iconIndex] : RingIcon.Empty; // "Max Candy" does not have an icon defined.

                ManaEquipment equipment = new ManaEquipment((byte)index, name, icon, definition);
                equipment.SetListeners(textContext.EquipmentNameTable, this.EquipmentDefinitionTable);
                equipmentList.Add(equipment);

                iconIndex++;
            }

            return new DataTable<ManaEquipment>(equipmentList);
        }

        private DataTable<ManaItem> CreateFullItemTable(TextContext textContext, MenuContext menuContext)
        {
            int index = 0;
            List<ManaItem> itemList = new List<ManaItem>(this.ItemDefinitionTable.RowCount);
            foreach (ManaItemDefinition definition in this.ItemDefinitionTable)
            {
                ManaEvent name = textContext.ItemNameTable[definition.Index];
                RingIcon icon = menuContext.ItemRingIcons[definition.Index];

                ManaItem item = new ManaItem((byte)index, name, icon, definition);
                item.SetListeners(textContext.ItemNameTable, this.ItemDefinitionTable);
                itemList.Add(item);
                index++;
            }

            return new DataTable<ManaItem>(itemList);
        }
    }
}