using ManaMagic.Core.Events;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core
{
    public sealed class TextContext
    {
        // Bank CA Tables
        public DataTable<ManaEvent> SpellNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> WeaponNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> EquipmentNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> ItemNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> RingMenuNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> EnemyNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> WeaponDescriptionTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> SpellDescriptionTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> TownNameTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> ItemErrorMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;

        // Bank 00 Tables
        public DataTable<ManaEvent> StatusEffectMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> ElementalFearMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> TrapMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> WeaponNameMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> BossSkillNameMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> BuffDebuffMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> LunarMagicMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> MiscellaneousMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> TreasureChestMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> CombatMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> AnalyzerMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;
        public DataTable<ManaEvent> LevelUpMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;

        // Bank D9 Tables
        public DataTable<ManaEvent> ShopMessageTable { get; private set; } = DataTable<ManaEvent>.Empty;

        /// <summary>
        /// Initializes the context by reading text data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            TextRomReader reader = RomReaderFactory.GetRomReader<TextRomReader>();

            this.SpellNameTable = reader.ReadSpellNameTable();
            this.WeaponNameTable = reader.ReadWeaponNameTable();
            this.EquipmentNameTable = reader.ReadEquipmentNameTable();
            this.ItemNameTable = reader.ReadItemNameTable();
            this.RingMenuNameTable = reader.ReadRingMenuNameTable();
            this.EnemyNameTable = reader.ReadEnemyNameTable();
            this.WeaponDescriptionTable = reader.ReadWeaponDescriptionTable();
            this.SpellDescriptionTable = reader.ReadSpellDescriptionTable();
            this.TownNameTable = reader.ReadTownNameTable();
            this.ItemErrorMessageTable = reader.ReadItemErrorMessageTable();

            this.StatusEffectMessageTable = reader.ReadStatusEffectMessageTable();
            this.ElementalFearMessageTable = reader.ReadElementalFearMessageTable();
            this.TrapMessageTable = reader.ReadTrapMessageTable();
            this.WeaponNameMessageTable = reader.ReadWeaponNameMessageTable();
            this.BossSkillNameMessageTable = reader.ReadBossSkillNameMessageTable();
            this.BuffDebuffMessageTable = reader.ReadBuffDebuffMessageTable();
            this.LunarMagicMessageTable = reader.ReadLunarMagicMessageTable();
            //this.MiscellaneousMessageTable = reader.ReadMiscellaneousMessageTable();
            this.TreasureChestMessageTable = reader.ReadTreasureChestMessageTable();
            this.CombatMessageTable = reader.ReadCombatMessageTable();
            this.AnalyzerMessageTable = reader.ReadAnalyzerMessageTable();
            this.LevelUpMessageTable = reader.ReadLevelUpMessageTable();

            this.ShopMessageTable = reader.ReadShopMessageTable();
        }
    }
}