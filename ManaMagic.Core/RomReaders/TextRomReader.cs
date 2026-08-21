using System.Collections.Generic;
using ManaMagic.Core.Events;
using ZwellTech.SuperNintendo;

#nullable enable

// Bank 0A Pointer Table Order:
// 1) Events (D)
// 2) Spell Names (D)
// 3) Weapon Names (D)
// 4) Equipment Names (D)
// 5) Item Names (D)
// 6) Ring Menu Text (D)
// 7) Enemy Names (D)
// 8) Weapon Descriptions (D)
// 9) Spell Descriptions (D)
// 10) Town Names (D)
// 11) Error Messages (D)

namespace ManaMagic.Core.RomReaders
{
    public sealed class TextRomReader : RomReader
    {
        private readonly EventParser parser;

        public TextRomReader(RomFile rom) : base(rom) { this.parser = new EventParser(this); }

        public DataTable<ManaEvent> ReadSpellNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.SpellNamesPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.SpellNamesPointerTableSize);
        }

        public DataTable<ManaEvent> ReadWeaponNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.WeaponNamesPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.WeaponNamesPointerTableSize);
        }

        public DataTable<ManaEvent> ReadEquipmentNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.EquipmentNamesPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.EquipmentNamesPointerTableSize);
        }

        public DataTable<ManaEvent> ReadItemNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.ItemNamesPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.ItemNamesPointerTableSize);
        }

        public DataTable<ManaEvent> ReadRingMenuNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.RingMenuNamesPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.RingMenuNamesPointerTableSize);
        }

        public DataTable<ManaEvent> ReadEnemyNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.EnemyNamesPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.EnemyNamesPointerTableSize);
        }

        public DataTable<ManaEvent> ReadWeaponDescriptionTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.WeaponDescriptionsPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.WeaponDescriptionsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadSpellDescriptionTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.SpellDescriptionsPointerTableAddress, Constants.Bank0AOffset, Constants.Bank0A.SpellDescriptionsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadTownNameTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.TownNameEventsPointerTableAddress, Constants.Bank0AOffset, (int)Constants.Bank0A.TownNameEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadItemErrorMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank0A.ItemErrorMessageEventsPointerTableAddress, Constants.Bank0AOffset, (int)Constants.Bank0A.ItemErrorMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadStatusEffectMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.StatusEffectMessageEventsPointerTableAddress, Constants.Bank00Offset, Constants.Bank00.StatusEffectMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadElementalFearMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.ElementalFearMessageEventsPointerTableAddress, Constants.Bank00Offset, Constants.Bank00.ElementalFearMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadTrapMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.TrapMessageEventsPointerTableAddress, Constants.Bank00Offset, Constants.Bank00.TrapMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadWeaponNameMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.WeaponNameMessageEventsPointerTableAddress, Constants.Bank00Offset, Constants.Bank00.WeaponNameMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadBossSkillNameMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.BossSkillNameMessageEventsPointerTableAddress, Constants.Bank00Offset, Constants.Bank00.BossSkillNameMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadLunarMagicMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.LunarMagicMessageEventsPointerTableAddress, Constants.Bank00Offset, (int)Constants.Bank00.LunarMagicMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadBuffDebuffMessageTable()
        {
            return this.ReadStringEventTable(Constants.Bank00.BuffDebuffMessageEventsPointerTableAddress, Constants.Bank00Offset, (int)Constants.Bank00.BuffDebuffMessageEventsPointerTableSize);
        }

        public DataTable<ManaEvent> ReadMiscellaneousMessageTable()
        {
            List<ManaEvent> miscMessages = new List<ManaEvent>(16);

            // This last pile of field messages are in a random order and have no pointer table.
            // The routines that use these messages load their pointers directly, because fuck me right?
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.SpellLevelMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.LevelUpMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.LevelUpPeriodMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.WeaponSkillUpMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.MagicSkillUpMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerHPMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerMPMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.GPInsideMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.ChestItemExclamationMessageAddress));

            // No real point in loading this one, nothing references it so editing it gains nothing.
            //miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.DummiedOutMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.RepelledTheMagicMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.GetsWhackedMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.WontFitMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerExpMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerGPMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.RecoveryFailedMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.MagicFadedMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.StillAliveMessageAddress));
            miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.CantUndoWallMessageAddress));

            return new DataTable<ManaEvent>(miscMessages);
            //Treasure Chests
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.GPInsideMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.ChestItemExclamationMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.WontFitMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.StillAliveMessageAddress));

            // Combat
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.RepelledTheMagicMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.GetsWhackedMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.MagicFadedMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.SpellLevelMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.RecoveryFailedMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.CantUndoWallMessageAddress));

            // Analyzer
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerHPMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerMPMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerExpMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerGPMessageAddress));

            // Level Up
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.LevelUpMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.LevelUpPeriodMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.WeaponSkillUpMessageAddress));
            //messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.MagicSkillUpMessageAddress));

            // Dummied Out
            //miscMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.DummiedOutMessageAddress));
        }

        public DataTable<ManaEvent> ReadTreasureChestMessageTable()
        {
            List<ManaEvent> messages = new List<ManaEvent>(4);

            //Treasure Chests
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.GPInsideMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.ChestItemExclamationMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.WontFitMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.StillAliveMessageAddress));

            return new DataTable<ManaEvent>(messages);
        }

        public DataTable<ManaEvent> ReadCombatMessageTable()
        {
            List<ManaEvent> messages = new List<ManaEvent>(6);

            // Combat
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.RepelledTheMagicMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.GetsWhackedMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.MagicFadedMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.SpellLevelMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.RecoveryFailedMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.CantUndoWallMessageAddress));

            return new DataTable<ManaEvent>(messages);
        }

        public DataTable<ManaEvent> ReadAnalyzerMessageTable()
        {
            List<ManaEvent> messages = new List<ManaEvent>(4);

            // Analyzer
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerHPMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerMPMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerExpMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.AnalyzerGPMessageAddress));

            return new DataTable<ManaEvent>(messages);
        }

        public DataTable<ManaEvent> ReadLevelUpMessageTable()
        {
            List<ManaEvent> messages = new List<ManaEvent>(4);

            // Level Up
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.LevelUpMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.LevelUpPeriodMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.WeaponSkillUpMessageAddress));
            messages.Add(this.parser.ParseEventAtAddress(Constants.Bank00.MagicSkillUpMessageAddress));

            return new DataTable<ManaEvent>(messages);
        }

        public DataTable<ManaEvent> ReadShopMessageTable()
        {
            List<ManaEvent> shopMessages = new List<ManaEvent>(16);

            // As the fuck me continues, all the shop related messages are also directly indexed by the routines that use them.
            // Need to track down all the locations. They are used around C0/7AFA.
            // Also these events are slightly more complex then just strings.
            // They start with a new line and then clear the text window.
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopThankYouMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopNotEnoughMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopCantCarryMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopNotInterestedMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopForgeItMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopNotEnoughMoneyMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopMoreCrystalOrbMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopForgedToBestMessageAddress));
            shopMessages.Add(this.parser.ParseEventAtAddress(Constants.Bank19.ShopOkayMessageAddress));

            return new DataTable<ManaEvent>(shopMessages);
        }

        private DataTable<ManaEvent> ReadStringEventTable(uint pointerTableAddress, int stringBankOffset, uint size)
        {
            List<ManaEvent> stringList = new List<ManaEvent>((int)size);
            for (int i = 0; i < size; i++)
            {
                this.Seek((int)(pointerTableAddress + (i * 2)));
                ushort pointer = this.ReadUInt16();

                ManaEvent stringEvent = this.parser.ParseEventAtAddress(stringBankOffset | pointer);
                stringList.Add(stringEvent);
            }
            return new DataTable<ManaEvent>(stringList);
        }
    }
}