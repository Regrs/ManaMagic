using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Bosses.BossInitialization
{
    public sealed class BossInitializationScriptParser
    {
        private readonly RomReader romReader;

        /// <summary>
        /// Create a new instance of the <see cref="BossInitializationScriptParser"/> class.
        /// </summary>
        /// <param name="romReader">The <see cref="RomReader"/> the load scripts will be read from.</param>
        public BossInitializationScriptParser(RomReader romReader)
        {
            this.romReader = romReader;
        }

        public BossInitializationScript ParseScript(uint index)
        {
            ValidationHelper.ThrowIfArgumentLessThan(index, 0u, "Index is out of range.");
            ValidationHelper.ThrowIfArgumentGreaterThanOrEqual(index, Constants.Bank02.BossInitializationScriptPointerTableTableSize, "Index is out of range.");

            int pointerOffset = sizeof(ushort) * 3; // 3 invalid pointers at the start of this table. The loader subtracts 0x54 instead of 0x57.
            this.romReader.Seek((int)(Constants.Bank02.BossInitializationScriptPointerTableAddress + (sizeof(ushort) * index) + pointerOffset));

            ushort offset = this.romReader.ReadUInt16();
            int pointer = Constants.Bank01Offset | offset;
            this.romReader.Seek(pointer);

            bool done = false;
            List<BossInitializationCommand> commands = new List<BossInitializationCommand>();
            while (!done)
            {
                BossInitializationOpCodeType opCode = (BossInitializationOpCodeType)this.romReader.Read(); ;
                done = this.ParseCommand(opCode, commands);
            }

            return new BossInitializationScript(commands);
        }

        private bool ParseCommand(BossInitializationOpCodeType commandType, List<BossInitializationCommand> script)
        {
            ushort parameter1 = 0;
            ushort parameter2 = 0;
            switch (commandType)
            {
                case BossInitializationOpCodeType.LoadPaletteIntoSlot01:
                    parameter1 = this.romReader.ReadUInt16();
                    parameter2 = this.romReader.ReadUInt16();
                    script.Add(new LoadPaletteIntoSlot01BossInitializationCommand(parameter1, parameter2));
                    break;
                case BossInitializationOpCodeType.CreateBossObject:
                    parameter1 = this.romReader.ReadUInt16();
                    parameter2 = this.romReader.ReadUInt16();
                    script.Add(new CreateBossObjectBossInitializationCommand(parameter1, parameter2));
                    break;
                case BossInitializationOpCodeType.LoadGraphics11:
                    parameter1 = this.romReader.ReadUInt16();
                    script.Add(new LoadGraphics11BossInitializationCommand(parameter1));
                    break;
                case BossInitializationOpCodeType.LoadHexasGraphics:
                    parameter1 = this.romReader.ReadUInt16();
                    script.Add(new LoadHexasGraphicsBossInitializationCommand(parameter1));
                    break;
                case BossInitializationOpCodeType.StoreValueAtAddress:
                    parameter1 = this.romReader.ReadUInt16();
                    parameter2 = this.romReader.ReadUInt16();
                    script.Add(new StoreValueAtAddressBossInitializationCommand(parameter1, parameter2));
                    break;
                case BossInitializationOpCodeType.InitializeCustomStateMachineBoss:
                    parameter1 = this.romReader.ReadUInt16();
                    parameter2 = this.romReader.ReadUInt16();
                    script.Add(new InitializeCustomStateMachineBossInitializationCommand(parameter1, parameter2));
                    break;
                case BossInitializationOpCodeType.ClearWRAMSectionBC00:
                    script.Add(new ClearWRAMSectionBC00BossInitializationCommand());
                    break;
                case BossInitializationOpCodeType.InitializeStandardStateMachineBoss:
                    parameter1 = this.romReader.ReadUInt16();
                    script.Add(new InitializeStandardStateMachineBossInitializationCommand(parameter1));
                    break;
                case BossInitializationOpCodeType.ClearWRAMSectionBC00AndVRAMWithDMA:
                    script.Add(new ClearWRAMSectionBC00AndVRAMWithDMABossInitializationCommand());
                    break;
                case BossInitializationOpCodeType.CallRoutine:
                    parameter1 = this.romReader.ReadUInt16();
                    script.Add(new CallRoutineBossInitializationCommand(parameter1));
                    break;
                case BossInitializationOpCodeType.LoadAndDecompressBossGraphics:
                    parameter1 = this.romReader.ReadUInt16();
                    script.Add(new LoadAndDecompressBossGraphicsBossInitializationCommand(parameter1));
                    break;
                case BossInitializationOpCodeType.LoadAndDecompressSlimeGraphics:
                    parameter1 = this.romReader.ReadUInt16();
                    script.Add(new LoadAndDecompressSlimeGraphicsBossInitializationCommand(parameter1));
                    break;
                case BossInitializationOpCodeType.End:
                    script.Add(new EndScriptBossInitializationCommand());
                    return true;
                default:
                    ThrowHelper.ThrowInvalidOperationException("Unknown boss load command.");
                    break;
            }
            return false;
        }
    }
}