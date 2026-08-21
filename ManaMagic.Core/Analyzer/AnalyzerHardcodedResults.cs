using System.Collections.Generic;

#nullable enable

namespace ManaMagic.Core.Analyzer
{
    internal static class AnalyzerHardcodedResults
    {
        private static readonly AnalyzerResult Event7F8_01 = new AnalyzerResult(AnalyzerReferenceType.HardcodedEvent, 0x020B96, 0, null, "Loaded and invoked during the generic boss death handling.");
        private static readonly AnalyzerResult Event7F8_02 = new AnalyzerResult(AnalyzerReferenceType.HardcodedEvent, 0x0272CC, 0, null, "Loaded and invoked during the special death handling for Mech Rider I/II.");

        private static readonly AnalyzerResult Event7FF_01 = new AnalyzerResult(AnalyzerReferenceType.HardcodedEvent, 0x019458, 0, null, "Loaded and invoked when the game detects all active party members have the 'Dead' status effect.");

        private static readonly AnalyzerResult EventFlagF7_01 = new AnalyzerResult(AnalyzerReferenceType.HardcodedEventFlag, 0x0000D3, 0, null, "FLAG_DEBUG_CRASH_GAME is explicily set to zero just before the main menu.");
        private static readonly AnalyzerResult EventFlagF7_02 = new AnalyzerResult(AnalyzerReferenceType.HardcodedEventFlag, 0x001E87, 0, null, "FLAG_DEBUG_CRASH_GAME is checked just after naming Randi. If set, the game jumps to C7/D000, which does not contain code.");

        public static void AddHardcodedEvents(ushort eventIndex, List<AnalyzerResult> results)
        {
            if (eventIndex == 0x07F8)
            {
                results.Add(AnalyzerHardcodedResults.Event7F8_01);
                results.Add(AnalyzerHardcodedResults.Event7F8_02);
            }
            else if (eventIndex == 0x07FF)
            {
                results.Add(AnalyzerHardcodedResults.Event7FF_01);
            }
        }

        public static void AddHardcodedEventFlags(EventFlag eventFlag, List<AnalyzerResult> results)
        {
            if (eventFlag == EventFlag.DebugCrashGame)
            {
                results.Add(AnalyzerHardcodedResults.EventFlagF7_01);
                results.Add(AnalyzerHardcodedResults.EventFlagF7_02);
            }
        }
    }
}
// New Game Door
// C7/51FC:	A975    	LDA #$75

// C0/00D2:	8FF6CF7E	STA $7ECFF6	;Store 0x00 into [FLAG_DEBUG_CRASH_GAME].
// C0/1E86:	AFF6CF7E	LDA $7ECFF6
// 003A22: 8D0FCF          STA $CF0F       ;Store 0x01 in [FLAG_CAN_HOLD_ITEM].
// 0043DA: BDB0CF          LDA $CFB0,X     ;Load [FLAG_TOTAL_UPGRADES_WEAPON, X] into Accumulator.
// C0/BCE8:	AFF7CF7E	LDA $7ECFF7	[FLAG_SNOWFALL]
// C0/BEAC:	AF00CF7E	LDA $7ECF00	;Load [FLAG_HUD_ENABLED] into Accumulator.

// C0/3FF7: A90800          LDA #$0008      ;Load 0x0008 into Accumulator. (Mana Sword Revival Event)
// C1/9458:	A9FF07  	LDA #$07FF    ;Game Over Event
// 00658F:	8F0FCF7E        STA $7ECF0F     ;Store 0x00 (GaveItem), 0x01 (EquipmentRingFull), or 0x02 (ConsumableRingFull or ConsumableCountMax) into [FLAG_ADD_ITEM_RESULT].