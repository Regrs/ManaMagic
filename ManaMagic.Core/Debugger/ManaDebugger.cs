using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ManaMagic.Core.Bosses.Animations;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ManaMagic.Core.Menu;
using ZwellTech;
using ZwellTech.Logging;

#nullable enable

namespace ManaMagic.Core.Debugger
{
    public static class ManaDebugger
    {
        private static SecretOfManaContext? context = null;

        [MemberNotNullWhen(true, nameof(ManaDebugger.context))]
        public static bool CanDebug { get { return ManaDebugger.context != null; } }

        [Conditional("DEBUG")]
        public static void Initialize(SecretOfManaContext context)
        {
            if (!ManaDebugger.CanDebug)
            {
                ManaDebugger.context = context;
                MapDebugger.Context = ManaDebugger.context;
                TextDebugger.Context = ManaDebugger.context;

                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has already been initialized.");
        }

        [Conditional("DEBUG")]
        public static void RunDebugger(MapDebuggerOption option)
        {
            if (ManaDebugger.CanDebug)
            {
                MapDebugger.RunDebugger(option);
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }

        [Conditional("DEBUG")]
        public static void RunDebugger(RingMenuDebuggerOption option)
        {
            switch (option)
            {
                case RingMenuDebuggerOption.PrintDefinitionOffsets:
                    ManaDebugger.RingMenu_PrintDefinitionOffsets();
                    break;
                case RingMenuDebuggerOption.PrintWeaponDefinitions:
                    ManaDebugger.RingMenu_PrintRingIconWeaponsDefinition();
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Unknown debugger option", nameof(option));
                    break;
            }
        }

        [Conditional("DEBUG")]
        public static void RunDebugger(BossDebuggerOption option)
        {
            switch (option)
            {
                case BossDebuggerOption.PrintAnimationScriptCommandCount:
                    ManaDebugger.Boss_PrintAnimationScriptCommandCount();
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Unknown debugger option", nameof(option));
                    break;
            }
        }

        [Conditional("DEBUG")]
        public static void RunDebugger(TextDebuggerOption option)
        {
            if (ManaDebugger.CanDebug)
            {
                TextDebugger.RunDebugger(option);
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }

        [Conditional("DEBUG")]
        public static void DebugPrint(string message)
        {
            LoggerEngine.Logger.LogDebug(LogComponent.Debug, message);
        }

        [Conditional("DEBUG")]
        private static void RingMenu_PrintDefinitionOffsets()
        {
            if (!ManaDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (ushort offset in ManaDebugger.context.MenuContext.RingIconDefinitionOffsetTable)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.General, $"[{index:X4}]: {offset:X4}");
                index++;
            }
        }

        [Conditional("DEBUG")]
        private static void RingMenu_PrintRingIconWeaponsDefinition()
        {
            if (!ManaDebugger.CanDebug)
            {
                ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
                return;
            }

            int index = 0;
            foreach (RingIconDefinition definition in ManaDebugger.context.MenuContext.RingIconWeaponDefinitionTable)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.General, $"[{index:X4}]: Icon: {definition.IconIndex:X2}, Palette: {definition.PaletteIndex:X2}");
                index++;
            }
        }

        [Conditional("DEBUG")]
        private static void Boss_PrintAnimationScriptCommandCount()
        {
            if (ManaDebugger.CanDebug)
            {
                Dictionary<BossAnimationCommandType, int> commandCounts = new Dictionary<BossAnimationCommandType, int>();
                foreach (BossAnimationCommandType actionType in Enum.GetValues<BossAnimationCommandType>())
                {
                    commandCounts.Add(actionType, 0);
                }

                foreach (BossAnimationScript script in ManaDebugger.context.BossContext.AnimationScriptTable)
                {
                    foreach (BossAnimationCommand action in script.Commands)
                    {
                        commandCounts[action.CommandType]++;
                    }
                }
                foreach (KeyValuePair<BossAnimationCommandType, int> kvp in commandCounts)
                {
                    ManaDebugger.DebugPrint($"[{kvp.Key}]: {kvp.Value}");
                }

                return;
            }


            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }
    }

    internal static class TextDebugger
    {
        public static SecretOfManaContext? Context { get; set; } = null;

        [MemberNotNullWhen(true, nameof(TextDebugger.Context))]
        public static bool CanDebug { get { return TextDebugger.Context != null; } }

        [Conditional("DEBUG")]
        public static void RunDebugger(TextDebuggerOption option)
        {
            switch (option)
            {
                case TextDebuggerOption.PrintLongestEventTextLine:
                    TextDebugger.PrintLongestEventTextLine();
                    break;
                case TextDebuggerOption.PrintMaxSizeForItemStrings:
                    TextDebugger.PrintMaxSizeForItemStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForSpellStrings:
                    TextDebugger.PrintMaxSizeForSpellStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForEnemyStrings:
                    TextDebugger.PrintMaxSizeForEnemyStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForTownNameStrings:
                    TextDebugger.PrintMaxSizeForTownNameStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForItemErrorStrings:
                    TextDebugger.PrintMaxSizeForItemErrorStrings();
                    break;
                case TextDebuggerOption.PrintSizeForWeaponDescriptionStrings:
                    TextDebugger.PrintSizeForWeaponDescriptionStrings();
                    break;
                case TextDebuggerOption.PrintSizeForSpellDescriptionStrings:
                    TextDebugger.PrintSizeForSpellDescriptionStrings();
                    break;
                case TextDebuggerOption.PrintSizeForShopStrings:
                    TextDebugger.PrintSizeForShopStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForStatusEffectStrings:
                    TextDebugger.PrintMaxSizeForStatusEffectStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForElementalFearStrings:
                    TextDebugger.PrintMaxSizeForElementalFearStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForTrapStrings:
                    TextDebugger.PrintMaxSizeForTrapStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForWeaponMessageStrings:
                    TextDebugger.PrintMaxSizeForWeaponMessageStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForBossSkillNameStrings:
                    TextDebugger.PrintMaxSizeForBossSkillNameStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForBuffDebuffStrings:
                    TextDebugger.PrintMaxSizeForBuffDebuffStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForLunarMagicStrings:
                    TextDebugger.PrintMaxSizeForLunarMagicStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForTreasureChestMessageStrings:
                    TextDebugger.PrintMaxSizeForTreasureChestMessageStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForCombatMessageStrings:
                    TextDebugger.PrintMaxSizeForCombatMessageStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForAnalyzerMessageStrings:
                    TextDebugger.PrintMaxSizeForAnalyzerMessageStrings();
                    break;
                case TextDebuggerOption.PrintMaxSizeForLevelUpMessageStrings:
                    TextDebugger.PrintMaxSizeForLevelUpMessageStrings();
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("Unknown debugger option", nameof(option));
                    break;
            }
        }

        [Conditional("DEBUG")]
        private static void PrintLongestEventTextLine()
        {
            if (TextDebugger.CanDebug)
            {
                int maxLineSize = 0;
                foreach (ManaEvent manaEvent in TextDebugger.Context.EventContext.Events)
                {
                    foreach (EventOpCode opCode in manaEvent)
                    {
                        if (opCode is TextDataEventOpCode textOpCode)
                        {
                            int currentLength = 0;
                            foreach (byte b in textOpCode.TextData)
                            {
                                if (b == textOpCode.NewLine)
                                {
                                    maxLineSize = Math.Max(maxLineSize, currentLength);
                                    currentLength = 0;
                                    continue;
                                }

                                currentLength++;
                            }
                        }
                    }
                }
                ManaDebugger.DebugPrint($"Longest Text Line Is: {maxLineSize} bytes.");
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForItemStrings()
        {
            uint numPointers = Constants.Bank0A.WeaponNamesPointerTableSize +
                               Constants.Bank0A.EquipmentNamesPointerTableSize +
                               Constants.Bank0A.ItemNamesPointerTableSize +
                               Constants.Bank0A.RingMenuNamesPointerTableSize;

            TextDebugger.PrintMaxSize("Item Name", numPointers);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForSpellStrings()
        {
            TextDebugger.PrintMaxSize("Spell Name", Constants.Bank0A.SpellNamesPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForEnemyStrings()
        {
            TextDebugger.PrintMaxSize("Enemy Name", Constants.Bank0A.EnemyNamesPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForTownNameStrings()
        {
            TextDebugger.PrintMaxSize("Town Name", Constants.Bank0A.TownNameEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForItemErrorStrings()
        {
            TextDebugger.PrintMaxSize("Item Error", Constants.Bank0A.ItemErrorMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintSizeForWeaponDescriptionStrings()
        {
            if (TextDebugger.CanDebug)
            {
                int maxSize = 0;
                int totalSize = 0;
                foreach (ManaEvent manaEvent in TextDebugger.Context.TextContext.WeaponDescriptionTable)
                {
                    foreach (EventOpCode opCode in manaEvent)
                    {
                        if (opCode is TextDataEventOpCode textOpCode)
                        {
                            maxSize = Math.Max(maxSize, textOpCode.TextData.Count);
                            totalSize += Constants.TextMaxLength;
                        }
                        else { totalSize += opCode.Size; }
                    }
                }
                ManaDebugger.DebugPrint($"Max Size For Weapon Description Strings: {maxSize} bytes.");
                ManaDebugger.DebugPrint($"Total Size For Weapon Description Strings: {totalSize} bytes.");
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }

        [Conditional("DEBUG")]
        private static void PrintSizeForSpellDescriptionStrings()
        {
            if (TextDebugger.CanDebug)
            {
                int maxSize = 0;
                int totalSize = 0;
                foreach (ManaEvent manaEvent in TextDebugger.Context.TextContext.SpellDescriptionTable)
                {
                    foreach (EventOpCode opCode in manaEvent)
                    {
                        if (opCode is TextDataEventOpCode textOpCode)
                        {
                            maxSize = Math.Max(maxSize, textOpCode.TextData.Count);
                            totalSize += Constants.TextMaxLength;
                        }
                        else { totalSize += opCode.Size; }
                    }
                }
                ManaDebugger.DebugPrint($"Max Size For Spell Description Strings: {maxSize} bytes.");
                ManaDebugger.DebugPrint($"Total Size For Spell Description Strings: {totalSize} bytes.");
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }

        [Conditional("DEBUG")]
        private static void PrintSizeForShopStrings()
        {
            if (TextDebugger.CanDebug)
            {
                int maxSize = 0;
                int totalSize = 0;
                foreach (ManaEvent manaEvent in TextDebugger.Context.TextContext.ShopMessageTable)
                {
                    foreach (EventOpCode opCode in manaEvent)
                    {
                        if (opCode is TextDataEventOpCode textOpCode)
                        {
                            maxSize = Math.Max(maxSize, textOpCode.TextData.Count);
                            totalSize += Constants.TextMaxLength;
                        }
                        else { totalSize += opCode.Size; }
                    }
                }
                ManaDebugger.DebugPrint($"Max Size For Shop Strings: {maxSize} bytes.");
                ManaDebugger.DebugPrint($"Total Size For Shop Strings: {totalSize} bytes.");
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForStatusEffectStrings()
        {
            TextDebugger.PrintMaxSize("Status Effect", Constants.Bank00.StatusEffectMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForElementalFearStrings()
        {
            TextDebugger.PrintMaxSize("Elemental Fear", Constants.Bank00.ElementalFearMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForTrapStrings()
        {
            TextDebugger.PrintMaxSize("Trap", Constants.Bank00.TrapMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForWeaponMessageStrings()
        {
            TextDebugger.PrintMaxSize("Weapon Name Message", Constants.Bank00.WeaponNameMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForBossSkillNameStrings()
        {
            TextDebugger.PrintMaxSize("Boss Skill Names", Constants.Bank00.BossSkillNameMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForBuffDebuffStrings()
        {
            TextDebugger.PrintMaxSize("Buff/Debuff Message", Constants.Bank00.BuffDebuffMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForLunarMagicStrings()
        {
            TextDebugger.PrintMaxSize("Lunar Magic", Constants.Bank00.LunarMagicMessageEventsPointerTableSize);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForTreasureChestMessageStrings()
        {
            TextDebugger.PrintMaxSize("Treasure Chest Message", 4);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForCombatMessageStrings()
        {
            TextDebugger.PrintMaxSize("Combat Message", 6);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForAnalyzerMessageStrings()
        {
            TextDebugger.PrintMaxSize("Analyzer", 4);
        }

        [Conditional("DEBUG")]
        private static void PrintMaxSizeForLevelUpMessageStrings()
        {
            TextDebugger.PrintMaxSize("Level Up", 4);
        }

        private static void PrintMaxSize(string name, uint size)
        {
            if (TextDebugger.CanDebug)
            {
                int byteCount = (int)((size * Constants.TextMaxLength) + size + (size * sizeof(ushort)));
                ManaDebugger.DebugPrint($"Max Size For {name} Strings: {byteCount} ({byteCount:X4}) bytes.");
                return;
            }

            ThrowHelper.ThrowInvalidOperationException("Mana debugger has not been initialized.");
        }
    }

        public enum TextDebuggerOption
    {
        PrintLongestEventTextLine,
        PrintMaxSizeForItemStrings,
        PrintMaxSizeForSpellStrings,
        PrintMaxSizeForEnemyStrings,
        PrintMaxSizeForTownNameStrings,
        PrintMaxSizeForItemErrorStrings,
        PrintSizeForWeaponDescriptionStrings,
        PrintSizeForSpellDescriptionStrings,
        PrintSizeForShopStrings,
        PrintMaxSizeForStatusEffectStrings,
        PrintMaxSizeForElementalFearStrings,
        PrintMaxSizeForTrapStrings,
        PrintMaxSizeForWeaponMessageStrings,
        PrintMaxSizeForBossSkillNameStrings,
        PrintMaxSizeForBuffDebuffStrings,
        PrintMaxSizeForLunarMagicStrings,
        PrintMaxSizeForTreasureChestMessageStrings,
        PrintMaxSizeForCombatMessageStrings,
        PrintMaxSizeForAnalyzerMessageStrings,
        PrintMaxSizeForLevelUpMessageStrings,
    }

    public enum RingMenuDebuggerOption
    {
        PrintDefinitionOffsets,
        PrintWeaponDefinitions,
    }

    public enum BossDebuggerOption
    {
        PrintAnimationScriptCommandCount,
    }
}