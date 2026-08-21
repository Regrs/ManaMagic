using System;
using System.Collections.Generic;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Bosses.Scripts
{
    public sealed class BossGraphicsScriptParser
    {
        private readonly RomFile rom;

        public BossGraphicsScriptParser(RomFile rom)
        {
            this.rom = rom;
        }

        public void Parse(int index, IReadOnlyList<BossGraphicsTableEntry> graphicsTable, ZzZSecretOfManaBossOld boss)
        {
            int address = Constants.Bank01Offset | rom.ReadUInt16At((int)Constants.Bank02.BossGraphicScriptPointerTableAddress + (index * 2));
            bool continueParsing = true;
            while (continueParsing)
            {
                BossGraphicsScriptOpCode current = (BossGraphicsScriptOpCode)rom.ReadByteAt(address);
                address++;

                ushort graphicsIndex = 0;
                switch (current)
                {
                    case BossGraphicsScriptOpCode.LoadPaletteIntoSlot01:
                        ushort paletteId = rom.ReadUInt16At(address);
                        address += 2;
                        ushort slot = rom.ReadUInt16At(address);
                        address += 2;

                        if (slot == 0x0D) { boss.GraphicsData.PaletteId1 = paletteId; }
                        else if (slot == 0x0E) { boss.GraphicsData.PaletteId2 = paletteId; }
                        else if (slot == 0x0F) { boss.GraphicsData.PaletteId3 = paletteId; }
                        else { throw new InvalidOperationException(); }
                        break;
                    case BossGraphicsScriptOpCode.LoadGraphics11:
                    case BossGraphicsScriptOpCode.LoadHexasGraphics:
                        graphicsIndex = rom.ReadUInt16At(address);
                        boss.GraphicsData.Rows.Add(new BossGraphicsTableRowDetails(graphicsIndex, graphicsTable[graphicsIndex], false, false));
                        address += 2;
                        break;
                    case BossGraphicsScriptOpCode.ClearWRAMSectionBC00AndVRAMWithDMA:
                        break;
                    case BossGraphicsScriptOpCode.CallRoutine:
                        address += 2;
                        break;
                    case BossGraphicsScriptOpCode.LoadAndDecompressBossGraphics:
                        graphicsIndex = rom.ReadUInt16At(address);
                        boss.GraphicsData.Rows.Add(new BossGraphicsTableRowDetails(graphicsIndex, graphicsTable[graphicsIndex], true, false));
                        address += 2;
                        break;
                    case BossGraphicsScriptOpCode.LoadAndDecompressSlimeGraphics:
                        graphicsIndex = rom.ReadUInt16At(address);
                        boss.GraphicsData.Rows.Add(new BossGraphicsTableRowDetails(graphicsIndex, graphicsTable[graphicsIndex], true, true));
                        address += 2;
                        break;
                    case BossGraphicsScriptOpCode.EndScript:
                        continueParsing = false;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public void ParsePaletteScript(int index, ZzZSecretOfManaBossOld boss)
        {
            int address = Constants.Bank01Offset | rom.ReadUInt16At((int)Constants.Bank0A.BossPaletteScriptPointerTableAddress + (index * 2));
            while (true)
            {
                byte currentByte = rom.ReadByteAt(address);
                address++;
                if (currentByte == 0x06 ||
                currentByte == 0x07 ||
                currentByte == 0x0D ||
                currentByte == 0x0E)
                {
                    byte paletteId = rom.ReadByteAt(address);
                    if (boss.GraphicsData.PaletteId1 == -1) { boss.GraphicsData.PaletteId1 = paletteId; }
                    else if (boss.GraphicsData.PaletteId2 == -1) { boss.GraphicsData.PaletteId2 = paletteId; }
                    else if (boss.GraphicsData.PaletteId3 == -1) { boss.GraphicsData.PaletteId3 = paletteId; }
                    else if (boss.GraphicsData.PaletteId4 == -1) { boss.GraphicsData.PaletteId4 = paletteId; }
                    else { throw new InvalidOperationException(); }

                    address++;
                }
                else if (currentByte == 0xFF) { break; }
                else
                {
                    byte prevByte = rom.ReadByteAt(address - 2);
                    if (prevByte == 0xFF) { break; }
                    throw new InvalidOperationException();
                }
            }
        }
    }
}