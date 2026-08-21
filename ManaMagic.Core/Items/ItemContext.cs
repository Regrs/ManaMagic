using System.Collections;
using System.Collections.Generic;
using ManaMagic.Core.Events;
using ManaMagic.Core.Menu;
using ManaMagic.Core.RomReaders;
using ZwellTech;
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

    public enum ShopItem : byte
    {
        BareHead = 0x7B,
        Bandanna = 0x7C,
        HairRibbon = 0x7D,
        RabiteCap = 0x7E,
        HeadGear = 0x7F,
        QuillCap = 0x80,
        SteelCap = 0x81,
        GoldenTiara = 0x82,
        RaccoonCap = 0x83,
        QuiltedHood = 0x84,
        TigerCap = 0x85,
        Circlet = 0x86,
        RubyArmet = 0x87,
        UnicornHelm = 0x88,
        DragonHelm = 0x89,
        DuckHelm = 0x8A,
        NeedleHelm = 0x8B,
        CockatriceCap = 0x8C,
        AmuletHelm = 0x8D,
        GriffinHelm = 0x8E,
        FaerieCrown = 0x8F,

        No = 0x90,
        Overalls = 0x91,
        KungFuSuit = 0x92,
        MidgeRobe = 0x93,
        ChainVest = 0x94,
        SpikySuit = 0x95,
        KungFuDress = 0x96,
        FancyOveralls = 0x97,
        ChestGuard = 0x98,
        GoldenVest = 0x99,
        RubyVest = 0x9A,
        TigerSuit = 0x9B,
        TigerBikini = 0x9C,
        MagicalArmor = 0x9D,
        TortoiseMail = 0x9E,
        FlowerSuit = 0x9F,
        BattleSuit = 0xA0,
        Vestguard = 0xA1,
        VampireCape = 0xA2,
        PowerSuit = 0xA3,
        FaerieCloak = 0xA4,

        Nothing = 0xA5,
        FaeriesRing = 0xA6,
        ElbowPad = 0xA7,
        PowerWrist = 0xA8,
        CobraBracelet = 0xA9,
        WolfsBand = 0xAA,
        SilverBand = 0xAB,
        GolemRing = 0xAC,
        FrostyRing = 0xAD,
        IvyAmulet = 0xAE,
        GoldBracelet = 0xAF,
        ShieldRing = 0xB0,
        LazuriRing = 0xB1,
        GuardianRing = 0xB2,
        Gauntlet = 0xB3,
        NinjaGloves = 0xB4,
        DragonRing = 0xB5,
        WatcherRing = 0xB6,
        ImpsRing = 0xB7,
        AmuletRing = 0xB8,
        Wristband = 0xB9,

        Candy = 0xBA,
        Chocolate = 0xBB,
        RoyalJam = 0xBC,
        FaerieWalnut = 0xBD,
        MedicalHerb = 0xBE,
        CupOfWishes = 0xBF,
        MagicRope = 0xC0,
        FlammieDrum = 0xC1,
        MoogleBelt = 0xC2,
        MidgeMallet = 0xC3,
        Barrel = 0xC4,
        QuestionMark = 0xC5,
    }

    public sealed record ManaShop : NotifyRecordPropertyChanged, IEnumerable<ShopItem>
    {
        public static ManaShop Empty { get; } = new ManaShop(0, new List<ShopItem>());

        private readonly List<ShopItem> items;

        public IReadOnlyList<ShopItem> Items { get { return this.items; } }
        public ShopItem this[int index]
        {
            get { return this.items[index]; }
            set
            {
                this.items[index] = value;
                this.OnPropertyChanged(nameof(this.Items));
            }
        }

        public ManaShop(byte index, List<ShopItem> shopItems, bool userModified = false) : base(index, userModified)
        {
            this.items = shopItems;
        }

        public void Add(ShopItem item)
        {
            this.items.Add(item);
            this.OnPropertyChanged(nameof(this.Items));
        }

        public void Insert(int index, ShopItem item)
        {
            this.items.Insert(index, item);
            this.OnPropertyChanged(nameof(this.Items));
        }

        public bool Remove(ShopItem item)
        {
            bool removed = this.items.Remove(item);
            this.OnPropertyChanged(nameof(this.Items));
            return removed;
        }

        /// <inheritdoc />
        public IEnumerator<ShopItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public sealed record UShortValue : GenericValue<ushort>
    {
        public UShortValue(byte index, ushort value, bool userModified = false) : base(index, value, userModified) { }
    }

    public record GenericValue<T> : NotifyRecordPropertyChanged
    {
        private T value;

        public T Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value); }
        }

        public GenericValue(byte index, T value, bool userModified = false) : base(index, userModified)
        {
            this.value = value;
        }
    }
}