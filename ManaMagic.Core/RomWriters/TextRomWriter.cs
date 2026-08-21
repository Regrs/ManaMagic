using System;
using System.Collections.Generic;
using ManaMagic.Core.Events;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class TextRomWriter : EventRomWriter
    {
        private const int StringTableBankByte = 0x2A;
        private const int StringTableOffset = TextRomWriter.StringTableBankByte << 16;

        private const uint ItemNamePointerTableAddress = TextRomWriter.StringTableOffset | 0x0000;
        private const uint SpellNamePointerTableAddress = TextRomWriter.StringTableOffset | 0x3000;
        private const uint EnemyNamePointerTableAddress = TextRomWriter.StringTableOffset | 0x4000;
        private const uint WeaponDescriptionPointerTableAddress = TextRomWriter.StringTableOffset | 0x6000;
        private const uint SpellDescriptionPointerTableAddress = TextRomWriter.StringTableOffset | 0x7000;
        private const uint TownNamePointerTableAddress = TextRomWriter.StringTableOffset | 0x8000;
        private const uint ItemErrorPointerTableAddress = TextRomWriter.StringTableOffset | 0x9000;
        private const uint ShopTextPointerTableAddress = TextRomWriter.StringTableOffset | 0x9300;
        private const uint StatusEffectPointerTableAddress = TextRomWriter.StringTableOffset | 0x9800;
        private const uint ElementalFearPointerTableAddress = TextRomWriter.StringTableOffset | 0xA000;
        private const uint TrapPointerTableAddress = TextRomWriter.StringTableOffset | 0xA200;
        private const uint WeaponNamesPointerTableAddress = TextRomWriter.StringTableOffset | 0xA400;
        private const uint BossSkillNamePointerTableAddress = TextRomWriter.StringTableOffset | 0xA600;
        private const uint BuffDebuffMessagePointerTableAddress = TextRomWriter.StringTableOffset | 0xB000;
        private const uint LunarMagicMessagePointerTableAddress = TextRomWriter.StringTableOffset | 0xB200;
        private const uint TreasureChestMessagePointerTableAddress = TextRomWriter.StringTableOffset | 0xB500;
        private const uint CombatMessagePointerTableAddress = TextRomWriter.StringTableOffset | 0xB600;
        private const uint AnalyzerMessagePointerTableAddress = TextRomWriter.StringTableOffset | 0xB800;
        private const uint LevelUpMessagePointerTableAddress = TextRomWriter.StringTableOffset | 0xB900;

        private const int StringPointerRoutinePatchLocation1 = 0x001929;
        private const int StringPointerRoutinePatchLocation2 = 0x001932;
        private const int ItemNameRoutinePatchLocation = 0x001814;
        private const int SpellNameRoutinePatchLocation = 0x001821;
        private const int EnemyNameRoutinePatchLocation = 0x001807;
        private const int MenuDescriptionRoutinePatchLocation = 0x076AB9;
        private const int WeaponDescriptionRoutinePatchLocation = 0x076490;
        private const int SpellDescriptionRoutinePatchLocation = 0x0764FA;
        private const int TownNameRoutinePatchLocation1 = 0x075A66;
        private const int TownNameRoutinePatchLocation2 = 0x075A6C;
        private const int ItemErrorRoutinePatchLocation1 = 0x00729E;
        private const int ItemErrorRoutinePatchLocation2 = 0x0072A6;
        private const int ItemErrorRoutinePatchLocation3 = 0x0072AB;
        private const int ShopTextThankYouRoutinePatchLocation = 0x007AFB;
        private const int ShopTextSorryNotEnoughRoutinePatchLocation = 0x007B0E;
        private const int ShopTextCantCarryRoutinePatchLocation = 0x007B13;
        private const int ShopTextSorryNotInterestedPatchLocation = 0x007B7B;
        private const int ShopTextLetsForgeItPatchLocation = 0x007C38;
        private const int ShopTextNotEnoughMoneyPatchLocation = 0x007B85;
        private const int ShopTextMoreEnergyOrbPatchLocation = 0x007B8C;
        private const int ShopTextForgedToBestPatchLocation = 0x007B91;
        private const int ShopTextOkayPatchLocation = 0x007E44;
        private const int ShopTextBankLoadPatchLocation1 = 0x007EA7;
        private const int ShopTextBankLoadPatchLocation2 = 0x007FBA;
        private const int StatusEffectPatchLocation1 = 0x005A92;
        private const int StatusEffectPatchLocation2 = 0x005AB6;
        private const int ElementalFearPatchLocation1 = 0x005A3E;
        private const int ElementalFearPatchLocation2 = 0x005A4B;
        private const int TrapPatchLocation = 0x005917;
        private const int WeaponNamesPatchLocation = 0x005B62;
        private const int BossSkillNamesPatchLocation = 0x005BD6;
        private const int BuffDebuffPatchLocation = 0x005942;
        private const int LunarMagicPatchLocation = 0x005BA9;
        private const int MessageRoutinePatchLocation = 0x005C2D;
        private const int TreasureChestGPInsidePatchLocation = 0x0058D5;
        private const int TreasureChestExclaimationPatchLocation = 0x0058EB;
        private const int TreasureChestWontFitPatchLocation = 0x005901;
        private const int TreasureChestStillAlivePatchLocation = 0x005BC4;
        private const int CombatRepelledTheMagicPatchLocation = 0x00598A;
        private const int CombatGetsWhackedPatchLocation = 0x005997;
        private const int CombatMagicFadedPatchLocation = 0x0058C1;
        private const int CombatSpellLevelPatchLocation = 0x005B10;
        private const int CombatRecoveryFailedPatchLocation = 0x0058B3;
        private const int CombatCantUndoWallPatchLocation = 0x005BBA;
        private const int AnalyzerHPPatchLocation = 0x0059AE;
        private const int AnalyzerMPPatchLocation = 0x0059DC;
        private const int AnalyzerExpPatchLocation = 0x005A09;
        private const int AnalyzerGPPatchLocation = 0x005A31;
        private const int LevelUpReachesLvPatchLocation1 = 0x005B29;
        private const int LevelUpReachesLvPatchLocation2 = 0x005B6C;
        private const int LevelUpReachesLvPatchLocation3 = 0x005B99;
        private const int LevelUpPeriodPatchLocation = 0x005B38;
        private const int LevelUpWeaponSkillUpPatchLocation = 0x005B49;
        private const int LevelUpMagicSkillUpPatchLocation = 0x005B7E;

        /// <inheritdoc/>
        public TextRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteStringTables(TextContext context)
        {
            this.WriteRelocatedBank00Tables(context);
            this.WriteRelocatedBank0ATables(context);
            this.WriteRelocatedBank19Tables(context);
        }

        private void WriteRelocatedBank00Tables(TextContext context)
        {
            //[10:33:21] (Debug) Debug: Max Size For Status Effect Strings: 928 (03A0) bytes.
            this.WritePointerAndStringTable(context.StatusEffectMessageTable, TextRomWriter.StatusEffectPointerTableAddress, (int)Constants.Bank00.StatusEffectMessageEventsPointerTableSize);

            //[12:25:50] (Debug) Debug: Max Size For Elemental Fear Strings: 464 (01D0) bytes.
            this.WritePointerAndStringTable(context.ElementalFearMessageTable, TextRomWriter.ElementalFearPointerTableAddress, (int)Constants.Bank00.ElementalFearMessageEventsPointerTableSize);

            //[12:53:02] (Debug) Debug: Max Size For Trap Strings: 464 (01D0) bytes.
            this.WritePointerAndStringTable(context.TrapMessageTable, TextRomWriter.TrapPointerTableAddress, (int)Constants.Bank00.TrapMessageEventsPointerTableSize);

            //[12:57:32] (Debug) Debug: Max Size For Weapon Name Message Strings: 464 (01D0) bytes.
            this.WritePointerAndStringTable(context.WeaponNameMessageTable, TextRomWriter.WeaponNamesPointerTableAddress, (int)Constants.Bank00.WeaponNameMessageEventsPointerTableSize);

            //[01:04:40] (Debug) Debug: Max Size For Boss Skill Names Strings: 1798 (0706) bytes.
            this.WritePointerAndStringTable(context.BossSkillNameMessageTable, TextRomWriter.BossSkillNamePointerTableAddress, (int)Constants.Bank00.BossSkillNameMessageEventsPointerTableSize);

            //[09:38:42] (Debug) Debug: Max Size For Buff/Debuff Message Strings: 464 (01D0) bytes.
            this.WritePointerAndStringTable(context.BuffDebuffMessageTable, TextRomWriter.BuffDebuffMessagePointerTableAddress, (int)Constants.Bank00.BuffDebuffMessageEventsPointerTableSize);

            //[09:53:49] (Debug) Debug: Max Size For Lunar Magic Strings: 522 (020A) bytes.
            this.WritePointerAndStringTable(context.LunarMagicMessageTable, TextRomWriter.LunarMagicMessagePointerTableAddress, (int)Constants.Bank00.LunarMagicMessageEventsPointerTableSize);

            //[12:14:23] (Debug) Debug: Max Size For Treasure Chest Message Strings: 232 (00E8) bytes.
            List<ushort> treasurePointerTable = this.WriteStringTable(context.TreasureChestMessageTable, TextRomWriter.TreasureChestMessagePointerTableAddress, context.TreasureChestMessageTable.RowCount);

            //[12:14:23] (Debug) Debug: Max Size For Combat Message Strings: 348 (015C) bytes.
            List<ushort> combatPointerTable = this.WriteStringTable(context.CombatMessageTable, TextRomWriter.CombatMessagePointerTableAddress, context.CombatMessageTable.RowCount);

            //[12:14:23] (Debug) Debug: Max Size For Analyzer Strings: 232 (00E8) bytes.
            List<ushort> analyzerPointerTable = this.WriteStringTable(context.AnalyzerMessageTable, TextRomWriter.AnalyzerMessagePointerTableAddress, context.AnalyzerMessageTable.RowCount);

            //[12:14:23] (Debug) Debug: Max Size For Level Up Strings: 232 (00E8) bytes.
            List<ushort> levelUpPointerTable = this.WriteStringTable(context.LevelUpMessageTable, TextRomWriter.LevelUpMessagePointerTableAddress, context.LevelUpMessageTable.RowCount);

            // The loader routine for all field messages.
            this.WriteByteAt(TextRomWriter.MessageRoutinePatchLocation, TextRomWriter.StringTableBankByte | 0xC0);

            this.WriteUInt16At(TextRomWriter.StatusEffectPatchLocation1, (ushort)(TextRomWriter.StatusEffectPointerTableAddress & 0xFFFF));
            this.WriteByteAt(TextRomWriter.StatusEffectPatchLocation2, TextRomWriter.StringTableBankByte | 0xC0);

            this.WriteUInt16At(TextRomWriter.ElementalFearPatchLocation1, (ushort)(TextRomWriter.ElementalFearPointerTableAddress & 0xFFFF));
            this.WriteByteAt(TextRomWriter.ElementalFearPatchLocation2, TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.TrapPatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.TrapPointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.WeaponNamesPatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.WeaponNamesPointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.BossSkillNamesPatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.BossSkillNamePointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.BuffDebuffPatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.BuffDebuffMessagePointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.LunarMagicPatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.LunarMagicMessagePointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            // Treasure Chests
            this.WriteUInt16At(TextRomWriter.TreasureChestGPInsidePatchLocation, treasurePointerTable[0]);
            this.WriteUInt16At(TextRomWriter.TreasureChestExclaimationPatchLocation, treasurePointerTable[1]);
            this.WriteUInt16At(TextRomWriter.TreasureChestWontFitPatchLocation, treasurePointerTable[2]);
            this.WriteUInt16At(TextRomWriter.TreasureChestStillAlivePatchLocation, treasurePointerTable[3]);

            // Combat
            this.WriteUInt16At(TextRomWriter.CombatRepelledTheMagicPatchLocation, combatPointerTable[0]);
            this.WriteUInt16At(TextRomWriter.CombatGetsWhackedPatchLocation, combatPointerTable[1]);
            this.WriteUInt16At(TextRomWriter.CombatMagicFadedPatchLocation, combatPointerTable[2]);
            this.WriteUInt16At(TextRomWriter.CombatSpellLevelPatchLocation, combatPointerTable[3]);
            this.WriteUInt16At(TextRomWriter.CombatRecoveryFailedPatchLocation, combatPointerTable[4]);
            this.WriteUInt16At(TextRomWriter.CombatCantUndoWallPatchLocation, combatPointerTable[5]);

            // Analyzer
            this.WriteUInt16At(TextRomWriter.AnalyzerHPPatchLocation, analyzerPointerTable[0]);
            this.WriteUInt16At(TextRomWriter.AnalyzerMPPatchLocation, analyzerPointerTable[1]);
            this.WriteUInt16At(TextRomWriter.AnalyzerExpPatchLocation, analyzerPointerTable[2]);
            this.WriteUInt16At(TextRomWriter.AnalyzerGPPatchLocation, analyzerPointerTable[3]);

            // Level Up
            this.WriteUInt16At(TextRomWriter.LevelUpReachesLvPatchLocation1, levelUpPointerTable[0]);
            this.WriteUInt16At(TextRomWriter.LevelUpReachesLvPatchLocation2, levelUpPointerTable[0]);
            this.WriteUInt16At(TextRomWriter.LevelUpReachesLvPatchLocation3, levelUpPointerTable[0]);
            this.WriteUInt16At(TextRomWriter.LevelUpPeriodPatchLocation, levelUpPointerTable[1]);
            this.WriteUInt16At(TextRomWriter.LevelUpWeaponSkillUpPatchLocation, levelUpPointerTable[2]);
            this.WriteUInt16At(TextRomWriter.LevelUpMagicSkillUpPatchLocation, levelUpPointerTable[3]);

            // This is all the load points for the message tables except for the misc one which is bullshit.
            //005A91: A0BB5D          LDY #$5DBB      ;Load 0x5DBB into Register Y. (Start index for PointerTable_StatusEffectMessages).
            //005AB5: A9C0            LDA #$C0        ;Load 0xC0 into Accumulator.
            //005A3D: A0DB5D          LDY #$5DDB      ;Load start index for a fear message from PointerTable_ElementalFearMessages.
            //005A4A: A9C0            LDA #$C0        ;Load 0xC0 into Accumulator.
            //005916: BFEB5DC0        LDA $C05DEB,X   ;Load the string pointer from PointerTable_TrapMessages, X.
            //005B61: BFFB5DC0        LDA $C05DFB,X   ;Load start index for a weapon name from PointerTable_WeaponNames.
            //005BD5: BF0B5EC0        LDA $C05E0B,X   ;Load start index for skill from PointerTable_EnemySkillMessages.
            //005941: BF495EC0        LDA $C05E49,X   ;Load the string pointer from PointerTable_BuffDebuffMessages, X.
            //005BA8: BF595EC0        LDA $C05E59,X   ;Load start index for lunar magic message from PointerTable_LunarMagicMessages.

            //All field messages end up here, which is where it loads the string from the ROM.
            //005C2A: BF0000C0        LDA $C00000,X   ;Load Current Message Byte into Accumulator.

            // These mother fuckers are just all in a pile and loaded directly by the routines that use them.
            // Treasure Chests
            //0058D4: A29462          LDX #$6294      ;Load 0x6294 into Register X. (Start index for ' GP inside!')
            //0058EA: A2A062          LDX #$62A0      ;Load 0x62A0 into Register X. (Start index for '!')
            //005900: A2C662          LDX #$62C6      ;Load 0x62C6 into Register X. (Start index for ' won't fit!')
            //005BC3: A20363          LDX #$6303      ;Load 0x6303 into Register X. (Index for 'Still alive!' message)

            // Combat
            //005989: A2A362          LDX #$62A3      ;Load 0x62A3 into Register X. (Start index for 'Repelled the magic!')
            //005996: A2B762          LDX #$62B7      ;Load 0x62B7 into Register X. (Start index for ' gets whacked!')
            //0058C0: A2F362          LDX #$62F3      ;Load 0x62F3 into Register X. (Start index for ''s magic faded.')
            //005B0F: A25162          LDX #$6251      ;Load 0x6251 into Register X. (Offset for string 'L v . ').
            //0058B2: A2E262          LDX #$62E2      ;Load 0x62E2 into Register X. (Start index for 'Recovery Failed!')
            //005BB9: A21063          LDX #$6310      ;Load 0x6310 into Register X. (Index for 'Can't undo Wall!' message)

            // Analyzer
            //0059AD: A28C62          LDX #$628C      ;Load Pointer to "HP: " message.
            //0059DB: A29062          LDX #$6290      ;Load Pointer to "MP: " message.
            //005A08: A2D262          LDX #$62D2      ;Load Pointer to "EXP: " message.
            //005A30: A2D762          LDX #$62D7      ;Load Pointer to " GP total." message.

            // Level Up
            //005B28: A25662          LDX #$6256      ;Load 0x6256 into Register X. (Pointer to start index of ' reaches Lv.' message)
            //005B6B: A25662          LDX #$6256      ;Load 0x6256 into Register X. (Pointer to start index of ' reaches Lv.' message)
            //005B98: A25662          LDX #$6256      ;Load 0x6256 into Register X. (Pointer to start index of ' reaches Lv.' message)
            //005B37: A26362          LDX #$6263      ;Load 0x6263 into Register X. (Pointer to start index of '.')
            //005B48: A26562          LDX #$6265      ;Load 0x6265 into Register X. (Pointer to start index of ''s Weapon Skill up!')
            //005B7D: A27962          LDX #$6279      ;Load 0x6279 into Register X. (Pointer to start index of ''s Magic Skill up!')
        }

        private void WriteRelocatedBank0ATables(TextContext context)
        {
            // Various string tables are located in Bank CA and annoyingly blend in with the actual event table.
            // Relocating them to a different bank provides a lot of additional space for event data and allows the strings to grow without taking away from it.
            // Simple patching of a few routines will re-point the tables.

            // At C0/190A is a routine used by the various string op-codes to create a text pointer.
            // This routine needs patched to look at the new bank address rather then bank CA.
            this.Seek(TextRomWriter.StringPointerRoutinePatchLocation1); // Loads the offset from the pointer table.
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);
            this.Seek(TextRomWriter.StringPointerRoutinePatchLocation2); // Sets up event RAM.
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            // The weapon/spell descriptions in Bank C7 go though a common loader that needs its bank byte changed.
            this.Seek(TextRomWriter.MenuDescriptionRoutinePatchLocation);
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            // Methods below will do patching specific to loading that table.

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 9106 (2392) bytes.
            this.WriteItemNameTable(context);

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 2900 (0B54) bytes.
            this.WriteSpellNameTable(context);

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 7424 (1D00) bytes.
            this.WriteEnemyNameTable(context);

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 4032 (0FC0) bytes.
            this.WriteWeaponDescriptionTable(context);

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 2352 (0930) bytes.
            this.WriteSpellDescriptionTable(context);

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 3248 (0CB0) bytes.
            this.WriteTownNameTable(context);

            // Assuming a maximum string length of 55 bytes, then this table will take a maximum of 464 (01D0) bytes.
            this.WriteItemErrorTable(context);
        }

        private void WriteRelocatedBank19Tables(TextContext context)
        {
            //[10:20:37] (Debug) Debug: Total Size For Shop Strings: 1008 (03F0) bytes.
            this.Seek(TextRomWriter.ShopTextPointerTableAddress);
            List<ushort> pointerTable = new List<ushort>(context.ShopMessageTable.RowCount);
            foreach (ManaEvent manaEvent in context.ShopMessageTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }
            // No pointer table for shop text, just hard coded offsets.
            // 00: Thank You
            // 01: Not Enough
            // 02: Can't Carry
            // 03: Not Interested
            // 04: Forge It
            // 05: Not Enough Money
            // 06: More Energy Orb
            // 07: Forged To Best
            // 08: Okay
            this.Seek(TextRomWriter.ShopTextThankYouRoutinePatchLocation);
            this.WriteUInt16(pointerTable[0]);

            this.Seek(TextRomWriter.ShopTextSorryNotEnoughRoutinePatchLocation);
            this.WriteUInt16(pointerTable[1]);

            this.Seek(TextRomWriter.ShopTextCantCarryRoutinePatchLocation);
            this.WriteUInt16(pointerTable[2]);

            this.Seek(TextRomWriter.ShopTextSorryNotInterestedPatchLocation);
            this.WriteUInt16(pointerTable[3]);

            this.Seek(TextRomWriter.ShopTextLetsForgeItPatchLocation);
            this.WriteUInt16(pointerTable[4]);

            this.Seek(TextRomWriter.ShopTextNotEnoughMoneyPatchLocation);
            this.WriteUInt16(pointerTable[5]);

            this.Seek(TextRomWriter.ShopTextMoreEnergyOrbPatchLocation);
            this.WriteUInt16(pointerTable[6]);

            this.Seek(TextRomWriter.ShopTextForgedToBestPatchLocation);
            this.WriteUInt16(pointerTable[7]);

            this.Seek(TextRomWriter.ShopTextOkayPatchLocation);
            this.WriteUInt16(pointerTable[8]);

            this.Seek(TextRomWriter.ShopTextBankLoadPatchLocation1);
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.ShopTextBankLoadPatchLocation2);
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            //C0/7AFA:	A220FE  	LDX #$FE20      ;Load 0xFE20 into Accumulator. (Pointer to ShopTextData_ThankYou)
            //C0/7B0D:	A22DFE  	LDX #$FE2D      ;Load 0xFE2D into Accumulator. (Pointer to ShopTextData_SorryNotEnough)
            //C0/7B12:	A249FE  	LDX #$FE49      ;Load 0xFE49 into Accumulator. (Pointer to ShopTextData_CantCarryAnymore)
            //C0/7B7A:	A265FE  	LDX #$FE65      ;Load 0xFE65 into Accumulator. (Pointer to ShopTextData_SorryNotInterested)
            //C0/7C37:	A27EFE  	LDX #$FE7E      ;Load 0xFE7E into Accumulator. (Pointer to ShopTextData_LetsForgeIt)
            //C0/7B84:	A296FE  	LDX #$FE96      ;Load 0xFE96 into Accumulator. (Pointer to ShopTextData_NotEnoughMoney)
            //C0/7B8B:	A2B5FE  	LDX #$FEB5      ;Load 0xFEB5 into Accumulator. (Pointer to ShopTextData_NeedMoreEnergyOrb)
            //C0/7B90:	A2D1FE  	LDX #$FED1      ;Load 0xFED1 into Accumulator. (Pointer to ShopTextData_ForgedToBest)
            //C0/7E43:	A2ECFE  	LDX #$FEEC      ;Load 0xFEEC into Accumulator. (Pointer to ShopTextData_Okay)

            //C0/7EA6:	A9D9    	LDA #$D9        ;Load 0xD9 into Accumulator.
            //C0/7FB9:	A9D9    	LDA #$D9        ;Load 0xD9 into Accumulator.
        }

        private void WriteItemNameTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.WeaponNamesPointerTableSize +
                               Constants.Bank0A.EquipmentNamesPointerTableSize +
                               Constants.Bank0A.ItemNamesPointerTableSize +
                               Constants.Bank0A.RingMenuNamesPointerTableSize;

            List<ushort> pointerTable = new List<ushort>((int)numPointers);

            // Split this massive table up for easier editing, now have to combine them all back into one in the right order.
            // Weapons > Equipment (Helms > Armor > Accessories) > Consumables > Ring Menu Items (Equip Armor, Trash, etc)
            this.Seek((int)(TextRomWriter.ItemNamePointerTableAddress + (numPointers * sizeof(ushort))));
            foreach (ManaEvent manaEvent in context.WeaponNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }
            foreach (ManaEvent manaEvent in context.EquipmentNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }
            foreach (ManaEvent manaEvent in context.ItemNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }
            foreach (ManaEvent manaEvent in context.RingMenuNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.ItemNamePointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // Patch the event op-code routine that loads the item strings.
            this.Seek(TextRomWriter.ItemNameRoutinePatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.ItemNamePointerTableAddress & 0xFFFF));
        }

        private void WriteSpellNameTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.SpellNamesPointerTableSize;
            this.Seek((int)(TextRomWriter.SpellNamePointerTableAddress + (numPointers * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>((int)numPointers);
            foreach (ManaEvent manaEvent in context.SpellNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.SpellNamePointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // Patch the event op-code routine that loads the strings.
            this.Seek(TextRomWriter.SpellNameRoutinePatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.SpellNamePointerTableAddress & 0xFFFF));
        }

        private void WriteEnemyNameTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.EnemyNamesPointerTableSize;
            this.Seek((int)(TextRomWriter.EnemyNamePointerTableAddress + (numPointers * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>((int)numPointers);
            foreach (ManaEvent manaEvent in context.EnemyNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.EnemyNamePointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // Patch the event op-code routine that loads the strings.
            this.Seek(TextRomWriter.EnemyNameRoutinePatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.EnemyNamePointerTableAddress & 0xFFFF));
        }

        private void WriteWeaponDescriptionTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.WeaponDescriptionsPointerTableSize;
            this.Seek((int)(TextRomWriter.WeaponDescriptionPointerTableAddress + (numPointers * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>((int)numPointers);
            foreach (ManaEvent manaEvent in context.WeaponDescriptionTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.WeaponDescriptionPointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // There's no op-code to display weapon descriptions, instead the equipment screen loads the pointer and invokes the event subsystem.
            // So that routine needs patched instead, a full 3 byte address needs patched.
            this.Seek(TextRomWriter.WeaponDescriptionRoutinePatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.WeaponDescriptionPointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);
        }

        private void WriteSpellDescriptionTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.SpellDescriptionsPointerTableSize;
            this.Seek((int)(TextRomWriter.SpellDescriptionPointerTableAddress + (numPointers * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>((int)numPointers);
            foreach (ManaEvent manaEvent in context.SpellDescriptionTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.SpellDescriptionPointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // There's no op-code to display spell descriptions, instead the spell screen loads the pointer and invokes the event subsystem.
            // So that routine needs patched instead, a full 3 byte address needs patched.
            this.Seek(TextRomWriter.SpellDescriptionRoutinePatchLocation);
            this.WriteUInt16((ushort)(TextRomWriter.SpellDescriptionPointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);
        }

        private void WriteTownNameTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.TownNameEventsPointerTableSize;
            this.Seek((int)(TextRomWriter.TownNamePointerTableAddress + (numPointers * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>((int)numPointers);
            foreach (ManaEvent manaEvent in context.TownNameTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.TownNamePointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // Town names are used by the save screen. Unlike the previous tables, this one is loaded via an indirect DB load.
            // Why? Who knows, the save code is weird. But we have to patch the DB load byte and the offset the load uses.
            this.Seek(TextRomWriter.TownNameRoutinePatchLocation1);
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            this.Seek(TextRomWriter.TownNameRoutinePatchLocation2);
            this.WriteUInt16((ushort)(TextRomWriter.TownNamePointerTableAddress & 0xFFFF));
        }

        private void WriteItemErrorTable(TextContext context)
        {
            uint numPointers = Constants.Bank0A.ItemErrorMessageEventsPointerTableSize;
            this.Seek((int)(TextRomWriter.ItemErrorPointerTableAddress + (numPointers * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>((int)numPointers);
            foreach (ManaEvent manaEvent in context.ItemErrorMessageTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(TextRomWriter.ItemErrorPointerTableAddress);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }

            // Alright, this table is fucking weird, have to focus on the weird that matters here.
            // The routine that loads pointers for this tables starts its indexing at the start of the town name table.
            // It just adds 56 to the index to skip past the entire first table.
            // That won't work now, so in addition to fixing up the offset we need to patch in some NoOps to remove the addition.
            Span<byte> patchCode = stackalloc byte[3] { 0xEA, 0xEA, 0xEA };
            this.Seek(TextRomWriter.ItemErrorRoutinePatchLocation1);
            this.WriteBytes(patchCode);

            // This loads the pointer from our new location.
            this.Seek(TextRomWriter.ItemErrorRoutinePatchLocation2);
            this.WriteUInt16((ushort)(TextRomWriter.ItemErrorPointerTableAddress & 0xFFFF));
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);

            // This sets the bank the event system looks in.
            this.Seek(TextRomWriter.ItemErrorRoutinePatchLocation3);
            this.Write(TextRomWriter.StringTableBankByte | 0xC0);
        }

        private void WritePointerAndStringTable(DataTable<ManaEvent> stringTable, uint address, int size)
        {
            this.Seek((int)(address + (size * sizeof(ushort))));

            List<ushort> pointerTable = new List<ushort>(size);
            foreach (ManaEvent manaEvent in stringTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            // Fill out the pointer table.
            this.Seek(address);
            foreach (ushort textOffset in pointerTable)
            {
                this.WriteUInt16(textOffset);
            }
        }

        private List<ushort> WriteStringTable(DataTable<ManaEvent> stringTable, uint address, int size)
        {
            this.Seek(address);

            List<ushort> pointerTable = new List<ushort>(size);
            foreach (ManaEvent manaEvent in stringTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                this.WriteEvent(manaEvent);
            }

            return pointerTable;
        }
    }
}