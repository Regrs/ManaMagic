using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ManaMagic.Core.Bosses.AICommands;
using ManaMagic.Core.Bosses.Animations;
using ManaMagic.Core.Bosses.Frames;
using ManaMagic.Core.Sprites;
using ZwellTech;
using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses
{
    /* TODO:
     * 
     * Aegagropilon
     * - N/A
     * 
     * Axe Beak
     * - Equips the correct weapon for the "Sonic Pulse" ability.
     * - Equips the correct weapon for the "Sleep Gas" abiility.
     * 
     * Blue Dragon
     * - N/A
     * 
     * Boreal Face
     * - N/A
     * 
     * Chamber's Eye
     * - N/A
     * 
     * Chamber's Wall
     * - N/A
     * 
     * Dark Lich
     * - Fix sleeve sprite priorty.
     * 
     * Death Machine
     * - Reenabled the "Doom Beam" ability.
     * - Use Death Machine's drilling in the ground animation during short circuit.
     * 
     * Doom's Eye
     * - N/A
     * 
     * Doom's Wall
     * - N/A
     * 
     * Dragon (All)
     * - N/A
     * 
     * Dread Slime
     * - Improve Spell Casting
     * 
     * Gorgon Bull
     * - Fix Petrify Gas usage.
     * 
     * Hexas
     * - N/A
     * 
     * Kettle Kin
     * - Reenabled the "Doom Beam" ability.
     * 
     * Lime Slime
     * - Improve Spell Casting
     * 
     * Mana Beast
     * - Reenabled Buff Spell Casting
     * 
     * Mantis Ant
     * - N/A
     * 
     * Mech Rider (All)
     * - N/A
     * 
     * Mech Rider I
     * - N/A
     * 
     * Mech Rider II
     * - N/A
     * 
     * Mech Rider III
     * - N/A
     * 
     * Metal Mantis
     * - N/A
     * 
     * Minotaur
     * - Implement Jump attack.
     * 
     * Red Dragon
     * - Implement fireball ability from unused graphics.
     * - Increase attack range.
     * 
     * Snow Dragon
     * - N/A
     * 
     * Spring Beak
     * - N/A
     * 
     * Tonpole
     * - Cannot overkill to prevent the fight with Biting Lizard.
     * 
     * Tropicallo
     * - N/A
     */
    /* DONE
     * 
     * Aegagropilon
     * - (Bugfix) Devour can now be used.
     * - Relaxed Devour targeting requirements.
     * - Changed the animation for eating the target.
     * 
     * Blue Dragon
     * - Increased attack range.
     * 
     * Brambler
     * - (Bugfix) No longer has a chance of spawning out side the map boundries or in other invalid positions.
     * - (Bugfix) Bramblers spawned by Boreal Face now equip the correct weapon.
     * 
     * Dark Lich
     * - (Bugfix) Can now attack while underground with his head sticking out.
     * - Can now move horizontally while hiding underground.
     * - Has a 50% chance to cast super magic.
     * 
     * Doom's Wall
     * - (Bugfix) Fixed invalid character in the name of the "Cave-In" ability.
     * 
     * Dragon (All)
     * - (Bugfix) Skills now target all opponents.
     * 
     * Hexas
     * - Barrier Change will now occur after enough time has passed or after taking damage enough times.
     * --- Will no longer advance on his target.
     * --- (Bugfix) Barrier Change can now use it's Sylphid Form.
     * --- (Bugfix) Barrier Change will now change Hexas's weakness and resistance elements.
     * --- Can Barrier Change into Shade (Spell List: Dark Force, Evil Gate, Dark Forcex2).
     * - (Bugfix) Will now spawn with the Luna element instead of with no element.
     * - Luna Barrier can now use the skill 'Moogle Bubbles'.
     * - Has infinite MP.
     * - Increased magic level to 8 and has a 25% chance to cast super magic.
     * 
     * Kettle Kin
     * - Restored Death Machine.
     * --- Death Machine's drill will spin while moving.
     * 
     * Mech Rider (All)
     * - (Bugfix) Will now properly set the AI Damage Lock routine instead of attempting to freeze the current action for 13,497 ticks.
     * - Can now attack with skills/spells/missiles while aligning with it target.
     * - Doubled vertical movement speed.
     * 
     * Mech Rider I
     * - Increased agility.
     * 
     * Mech Rider II
     * - Increased strength, agility, physical defense, magic defense and white magic power.
     * 
     * Mech Rider III
     * - (Bugfix) Fixed AI logic so spell and cannon attacks work properly.
     * - (Bugfix) Diffuser Cannon now properly targets all opponents.
     * - Increased the power of Diffuser Cannon.
     * - Increased agility, magic level and white magic power.
     * 
     * Metal Mantis
     * - (Bugfix) Equips a properly leveled melee and projectile weapon instead of reusing Mantis Ant's.
     * - Increased the power of the 'Fire Beam' skill.
     * - Can now use a more powerful version of the 'Acid Breath' skill.
     * 
     * Red Dragon
     * - Increased attack range.
     * 
     * Snow Dragon
     * - Replaced 'Breath Wing' skill with new skill 'Frost Wing' from unused graphics.
     * - Increased attack range.
     * 
     * Tropicallo
     * - (Bugfix) Will now spawn exactly his limit of Bramblers (Default: 2) instead of a semi-random amount.
     * - Brambler limit can be configured.
     */
    public sealed class BossAIUpgradeWriter
    {
        private readonly RomFile romFile;
        private readonly WritableRomFile writableRomFile;
        private readonly BossAIRomReader romReader;
        private readonly BossAIRomWriter romWriter;
        private readonly BossAICommandParser commandParser;
        private readonly List<ushort> palettePointerTable;

        private uint BossAIAuxiliaryBankOffset { get { return (uint)(this.BossAIAuxiliaryBank << 16); } }
        private uint PalettePointerTableAddress { get { return this.BossAIAuxiliaryBankOffset | AIConstants.PaletteTablePointerTableAddressOffset; } }
        private uint PaletteTableAddress { get { return this.BossAIAuxiliaryBankOffset | AIConstants.PaletteTableAddressOffset; } }

        public byte BossAIAuxiliaryBank { get; set; } = 0x22;
        public string JapaneseRomFilePath { get; set; } = string.Empty;

        public BossAIUpgradeWriter(RomFile romFile, bool extendRom)
        {
            this.romFile = romFile;
            this.writableRomFile = new WritableRomFile(this.romFile, extendRom);
            this.romReader = new BossAIRomReader(this.romFile);
            this.romWriter = new BossAIRomWriter(this.writableRomFile);
            this.commandParser = new BossAICommandParser(this.romReader);
            this.palettePointerTable = new List<ushort>(Constants.Bank02.BossPaletteTableAddressSize);
        }

        public void Run(BossAIUpgradeOptions options)
        {
            this.PrepareRomFile();

            this.WriteAegagropilonUpgrades(options);
            this.WriteBlueDragonUpgrades(options);
            this.WriteBramblerUpgrades(options);
            this.WriteDarkLichUpgrades(options);
            this.WriteDoomsWallUpgrades(options);
            this.WriteDragonAllUpgrades(options);
            this.WriteKettleKinUpdates(options);
            this.WriteHexasUpgrades(options);
            this.WriteMetalMantisUpgrades(options);
            this.WriteMechRiderAllUpgrades(options);
            this.WriteMechRiderIUpgrades(options);
            this.WriteMechRiderIIUpgrades(options);
            this.WriteMechRiderIIIUpgrades(options);
            this.WriteRedDragonUpgrades(options);
            this.WriteSnowDragonUpgrades(options);
            this.WriteTropicalloUpgrades(options);

            this.DebugWriteSaveEventWarp();
        }

        public void SaveToFile(string filePath, BossAIUpgradeOptions options)
        {
            //const string romFileName = "SecretOfManaTestROM.sfc";
            const string romFileName = "Secret Of Mana (Boss AI Patch).sfc";
            const string logFileName = "Secret Of Mana (Boss AI Patch) Log.txt";

            this.writableRomFile.CorrectChecksum();
            this.writableRomFile.SaveToFile(Path.Combine(filePath, romFileName));
            File.WriteAllText(Path.Combine(filePath, logFileName), this.ToString(options));
        }

        [Conditional("DEBUG")]
        public void DebugSaveToFile(BossAIUpgradeOptions options)
        {
            File.WriteAllText(@"D:\SOM Stuff\SecretOfManaTestROM.txt", this.ToString(options));
            this.writableRomFile.DebugSaveToFile();
        }

        public string ToString(BossAIUpgradeOptions options)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Source US ROM Path: ").AppendLine(romFile.FilePath);
            sb.Append("Source JP ROM Path: ").AppendLine(this.JapaneseRomFilePath);
            sb.Append("BossAIAuxiliaryBank: ").AppendLine(this.BossAIAuxiliaryBank.ToString("X2"));
            sb.AppendLine();
            sb.Append(options.ToString());
            return sb.ToString();
        }

        [Conditional("DEBUG")]
        private void DebugWriteSaveEventWarp()
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            const byte EndOfEvent = 0x00;
            const byte UseDoor01 = 0x18;
            const byte UseDoor02 = 0x19;
            const byte UseDoor03 = 0x1A;
            const byte UseDoor04 = 0x1B;
            const byte SetEventFlag = 0x30;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            // Event 50: Save Dialogue
            this.romWriter.Seek(0x0910A4);

            // Play 'Danger'.
            //this.romWriter.WriteUInt16(0xC427);

            // Fade Out Music
            this.romWriter.WriteUInt16(0x3E27);

            // Disable collision to chase annoying bosses like snakes and dragons.
            this.romWriter.Write(SetEventFlag);
            this.romWriter.WriteUInt16(0x0101);

            // Aegagropilon - Door 358, Requires Event Flag 45 to be set to 1.
            //this.romWriter.Write(SetEventFlag);
            //this.romWriter.WriteUInt16(0x0145);
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0x58);
            //this.romWriter.Write(EndOfEvent);

            // Blue Dragon - Door 3AD, Requires Event Flag 46 to be less than or equal to 5.
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0xAD);
            //this.romWriter.Write(EndOfEvent);

            // Dark Lich - Door 2F, Requires Event Flag 4E to be set to 6.
            //this.romWriter.Write(SetEventFlag);
            //this.romWriter.WriteUInt16(0x064E);
            //this.romWriter.Write(UseDoor01);
            //this.romWriter.Write(0x2F);
            //this.romWriter.Write(EndOfEvent);
            // Dark Lich always goes under ground.
            //this.romWriter.Seek(0x026E09);
            //this.romWriter.Write(0x80);

            // Great Viper - Doors 14C, 16D, Requires Event Flag 24 to be less than or equal to 4.
            this.romWriter.Write(UseDoor02);
            this.romWriter.Write(0x6D);
            this.romWriter.Write(EndOfEvent);

            // Hexas - Door 3DF, Requires Event Flag 44 to be set to 5.
            //this.romWriter.Write(SetEventFlag);
            //this.romWriter.WriteUInt16(0x0544);
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0xDF);
            //this.romWriter.Write(EndOfEvent);

            // Kettle Kin - Door 3C7, Requires Event Flag 47 to be set to 2.
            //this.romWriter.Write(SetEventFlag);
            //this.romWriter.WriteUInt16(0x0247);
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0xC7);
            //this.romWriter.Write(EndOfEvent);

            // Mech Rider I - Door 193, Event Flag 28 at any value.
            //this.romWriter.Write(UseDoor02);
            //this.romWriter.Write(0x93);
            //this.romWriter.Write(EndOfEvent);

            // Mech Rider II - Door 35D, Requires Event Flag 35 to be less than or equal to 5.
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0x5D);
            //this.romWriter.Write(EndOfEvent);

            // Mech Rider III - Door 3F6, Event Flag 00 at any value.
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0xF6);
            //this.romWriter.Write(EndOfEvent);

            // Metal Mantis - Door 345, Requires Event Flag 34 to be less than or equal to 3.
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0x45);
            //this.romWriter.Write(EndOfEvent);

            // Red Dragon - Door 395, Requires Event Flag 46 to be less than or equal to 3.
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0x95);
            //this.romWriter.Write(EndOfEvent);

            // Snow Dragon - Door 37F, Requires Event Flag 46 to be less than or equal to 1.
            //this.romWriter.Write(UseDoor04);
            //this.romWriter.Write(0x7F);
            //this.romWriter.Write(EndOfEvent);
        }

        private void PrepareRomFile()
        {
            // 3,683
            this.RelocatePaletteTable();
            this.WriteSetSuperMagicRateRoutine();

            this.romWriter.Seek(0x017E27);
            while (this.romWriter.Position < 0x018000)
            {
                this.romWriter.Write(0xFF);
            }
        }

        private void RelocatePaletteTable()
        {
            // Relocating this table is a little more annoying then a straight copy since their are more pointers then palettes.
            this.romReader.Seek((int)Constants.Bank10.BossPalettePointerTableAddress);

            // Read the original pointer table.
            List<ushort> originalPointerTable = new List<ushort>(Constants.Bank02.BossPaletteTableAddressSize);
            for (int paletteIndex = 0; paletteIndex < Constants.Bank02.BossPaletteTableAddressSize; paletteIndex++)
            {
                originalPointerTable.Add(this.romReader.ReadUInt16());
            }

            Dictionary<ushort, ushort> writtenPointers = new Dictionary<ushort, ushort>();
            this.romWriter.Seek((int)this.PaletteTableAddress);

            foreach (ushort offset in originalPointerTable)
            {
                // If we haven't seen this pointer before then copy the palette it points to to the new location then add the updated pointer to the new pointer list.
                // If we have seen it before then just add the new pointer in the current position.
                if (!writtenPointers.TryGetValue(offset, out ushort newOffset))
                {
                    newOffset = (ushort)(this.romWriter.Position & 0xFFFF);

                    this.romReader.Seek(Constants.Bank02Offset | offset);
                    for (int colorIndex = 0; colorIndex < Constants.Bank02.BossPaletteColorCount; colorIndex++)
                    {
                        this.romWriter.WriteUInt16(this.romReader.ReadUInt16());
                    }
                    writtenPointers.Add(offset, newOffset);
                }
                this.palettePointerTable.Add(newOffset);
            }

            // Write out the new pointer table.
            this.romWriter.Seek((int)this.PalettePointerTableAddress);
            foreach (ushort palettePointer in this.palettePointerTable)
            {
                this.romWriter.WriteUInt16(palettePointer);
            }

            // There are three sets of code that reference the palette table that need to be updated.
            // The first two are general palette loading routines, while the third is a invisible palette cycling object.
            UpdateCodeInstructions();

            // Fill the old palette area with FF.
            this.romWriter.Seek((int)Constants.Bank02.BossPaletteTableAddress);
            while (this.romWriter.Position < 0x030000) { this.romWriter.Write(0xFF); }

            void UpdateCodeInstructions()
            {
                // Addressing fixup.
                byte dataTableBank = (byte)(((this.PaletteTableAddress & 0x00FF0000) >> 16) | 0xC0);
                uint pointerTableAddress = this.PalettePointerTableAddress | 0xC00000;

                // [LoadBossPalette_ToWRAMWithDMA]
                const uint loadBossPaletteWithDMATableAddressLocation = 0x0226C8;
                const uint loadBossPaletteWithDMABankByteAddressLocation = 0x0226EB;

                this.romWriter.Seek((int)loadBossPaletteWithDMATableAddressLocation);
                this.romWriter.WriteUInt24(pointerTableAddress);

                this.romWriter.Seek((int)loadBossPaletteWithDMABankByteAddressLocation);
                this.romWriter.Write(dataTableBank);

                // [LoadBossPalette_ToRAM]
                const uint loadBossPaletteBankByteAddressLocation = 0x022711;
                const uint loadBossPaletteTableAddressLocation = 0x022718;

                this.romWriter.Seek((int)loadBossPaletteBankByteAddressLocation);
                this.romWriter.Write(dataTableBank);

                this.romWriter.Seek((int)loadBossPaletteTableAddressLocation);
                this.romWriter.WriteUInt24(pointerTableAddress);

                // [Palette Cycler Boss Object AI]
                // C2/2768:	7F24BBD0	   ADC $D0BB24,X   ;PointerTable_BossPalettes
                this.romWriter.Seek(0x022769);
                this.romWriter.WriteUInt24(pointerTableAddress);

                // C2/2784:	A9C2    	   LDA #$C2        ;Load 0xC2 (The Bank byte for boss palette addressing) into Accumulator.
                this.romWriter.Seek(0x022785);
                this.romWriter.Write(dataTableBank);

                // C2/27A2:	A900C2  	   LDA #$C200      ;Load 0x00C2 (The Bank byte for boss palette addressing) into Accumulator.
                this.romWriter.Seek(0x0227A4);
                this.romWriter.Write(dataTableBank);

                // C2/27A7:	BF24BBD0	   LDA $D0BB24,X   ;Load a pointer from PointerTable_BossPalettes.
                this.romWriter.Seek(0x0227A8);
                this.romWriter.WriteUInt24(pointerTableAddress);

                // C2/281F:	A900C2  	   LDA #$C200      ;Load 0x00C2 (The Bank byte for boss palette addressing) into Accumulator.
                this.romWriter.Seek(0x022821);
                this.romWriter.Write(dataTableBank);

                // C2/2829:	BF24BBD0	   LDA $D0BB24,X   ;Load a pointer from PointerTable_BossPalettes.
                this.romWriter.Seek(0x02282A);
                this.romWriter.WriteUInt24(pointerTableAddress);

                // C2/289C:	7F24BBD0	   ADC $D0BB24,X   ;PointerTable_BossPalettes
                this.romWriter.Seek(0x02289D);
                this.romWriter.WriteUInt24(pointerTableAddress);

                // C2/28A7:	547EC2  	   MVN $7E,$C2
                this.romWriter.Seek(0x0228A9);
                this.romWriter.Write(dataTableBank);

                // [LoadBossPalette_ToWRAMWithDMA]
                // 0226C7: BF24BBD0        LDA $D0BB24,X   ;Load a pointer from PointerTable_BossPalettes.
                // 0226EA: A9C2            LDA #$C2        ;Load 0xC2 into Accumulator.

                // [LoadBossPalette_ToRAM]
                // 02270F: A900C2          LDA #$C200      ;Load 0xC200 into Accumulator.
                // 022717: BF24BBD0        LDA $D0BB24,X   ;Load a pointer from PointerTable_BossPalettes.

                // [Palette Cycler Boss Object AI]
                // C2/2768:	7F24BBD0	   ADC $D0BB24,X   ;PointerTable_BossPalettes
                // C2/2784:	A9C2    	   LDA #$C2        ;Load 0xC2 (The Bank byte for boss palette addressing) into Accumulator.
                // C2/27A2:	A900C2  	   LDA #$C200      ;Load 0x00C2 (The Bank byte for boss palette addressing) into Accumulator.
                // C2/27A7:	BF24BBD0	   LDA $D0BB24,X   ;Load a pointer from PointerTable_BossPalettes.
                // C2/281F:	A900C2  	   LDA #$C200      ;Load 0x00C2 (The Bank byte for boss palette addressing) into Accumulator.
                // C2/2829:	BF24BBD0	   LDA $D0BB24,X   ;Load a pointer from PointerTable_BossPalettes.
                // C2/289C:	7F24BBD0	   ADC $D0BB24,X   ;PointerTable_BossPalettes
                // C2/28A7:	547EC2  	   MVN $7E,$C2
            }
        }

        private void WriteSetSuperMagicRateRoutine()
        {
            ReadOnlySpan<byte> setSuperMagicRate = stackalloc byte[]
            {
                0xE2, 0x20,             // SEP #$20        ;Disable 16-Bit Accumulator.
                0x9D, 0xD8, 0x01,       // STA $01D8,X     ;Store Accumulator into [CURRENT_EXPERIENCE_GNOME, X].
                0x9D, 0xD9, 0x01,       // STA $01D9,X     ;Store Accumulator into [CURRENT_EXPERIENCE_UNDINE, X].
                0x9D, 0xDA, 0x01,       // STA $01DA,X     ;Store Accumulator into [CURRENT_EXPERIENCE_SALAMANDO, X].
                0x9D, 0xDB, 0x01,       // STA $01DB,X     ;Store Accumulator into [CURRENT_EXPERIENCE_SYLPHID, X].
                0x9D, 0xDC, 0x01,       // STA $01DC,X     ;Store Accumulator into [CURRENT_EXPERIENCE_LUNA, X].
                0x9D, 0xDD, 0x01,       // STA $01DD,X     ;Store Accumulator into [CURRENT_EXPERIENCE_DRYAD, X].
                0x9D, 0xDE, 0x01,       // STA $01DE,X     ;Store Accumulator into [CURRENT_EXPERIENCE_SHADE, X].
                0x9D, 0xDF, 0x01,       // STA $01DF,X     ;Store Accumulator into [CURRENT_EXPERIENCE_LUMINA, X].
                0xC2, 0x20,             // REP #$20        ;Enable 16-Bit Accumulator.
                0x60,                   // RTS             ;Return.
            };
            this.romWriter.Seek((int)AIConstants.SetSuperMagicRateRoutineLocation);
            this.romWriter.WriteBytes(setSuperMagicRate);
        }

        private void WriteAegagropilonUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Aegagropilon);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.AegagropilonOptions.EnableDevourAttackFix)
            {
                // Aegagropilon is supposed to use this attack once everytime he transforms back into his legged form.
                // His ball form correctly sets the flag, however his transform command resets it back to zero.
                BossAICommand command = commandSet.Commands[AIConstants.Aegagropilon.Commands.LeggedFormTransformBegin];
                command.Replace(1, new BossAISetBossDataAction(0x007E, 0x8000));

                this.romWriter.Seek(Constants.Bank02Offset | commandSet.PointerTable[AIConstants.Aegagropilon.Commands.LeggedFormTransformBegin]);
                this.romWriter.WriteAICommand(command);

                if (options.AegagropilonOptions.ImprovedDevourTargeting)
                {
                    // The parameters for Aegagropilon to use Devour are pretty tight. The target must be south of him and within 32 total pixels.
                    // This removes the direction requirement and doubles the range to 64 total pixels.
                    this.romWriter.Seek((int)AIConstants.Aegagropilon.DevourDirectionPatchLocation);
                    this.romWriter.WriteUInt16(0xEAEA);

                    this.romWriter.Seek((int)AIConstants.Aegagropilon.DevourDistancePatchLocation);
                    this.romWriter.Write(0x04);
                }

                if (options.AegagropilonOptions.AlternateDevourEatAnimation)
                {
                    // The animation for eating his target is just a static frame of his mouth open while upside down.
                    // There is an unused animation for his mouth being upside down while opening and closing, which looks better.
                    BossAICommand eatTargetCommand = commandSet.Commands[AIConstants.Aegagropilon.Commands.EatTarget];
                    eatTargetCommand.Replace(0, new BossAIPlayAnimationAction(0x0081));

                    this.romWriter.Seek(Constants.Bank02Offset | commandSet.PointerTable[AIConstants.Aegagropilon.Commands.EatTarget]);
                    this.romWriter.WriteAICommand(eatTargetCommand);
                }
            }

            if (options.AegagropilonOptions.BallFormAlwaysAttacks)
            {
                // Then Ball Form will attack once and then return to Legged Form.
                // However, 50% of Ball Forms attack roulette also returns him to Legged Form.
                this.romWriter.Seek((int)AIConstants.Aegagropilon.BallFormAttackPatchLocation);
                this.romWriter.WriteUInt16(0xD90D);
                this.romWriter.WriteUInt16(0xD90F);
            }
        }

        private void WriteBlueDragonUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Dragon);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.BlueDragonOptions.IncreasedAttackRange)
            {
                this.romWriter.Seek(AIConstants.BlueDragon.AttackRangePatchLocation);
                this.romWriter.Write(0x0C); // 192 total pixels, up from 64.
            }
        }

        private void WriteBramblerUpgrades(BossAIUpgradeOptions options)
        {
            if (options.BramblerOptions.OutOfBoundsAndCountFix)
            {
                // Located at C2/45D4 in the Brambler's init routine are some out of order op-codes.
                // This causes a hardcoded value of 0x0001 that it loaded a couple instructions before to be loaded into Register X instead of the parent bosses offset.
                // So when it goes to load [BOSS_ATTACK_PATTERN] to determine if it should fetch Tropicallo or Boreal Face's coordinates it instead reads
                // from 7E/0043, the contents of which are random, which causes the coordinate table to be indexed out of bounds.
                // Additionally instead of increasing the counter of the number of Bramblers spawned it increases the value of memory at address 7E/00B1.
                // This allows Tropicallo to spawn a random number of Bramblers based off which ones are killed, instead of just two.
                // To fix both bugs these instructions need flipped around so the correct value gets loaded into Register X.
                ReadOnlySpan<byte> patchCode = stackalloc byte[]
                {
                    0xA9, 0x01, 0x00, // LDA #$0001      ;Load 0x0001 into Accumulator.
                    0x9D, 0x82, 0x01, // STA $0182,X     ;Store 0x0001 into [CURRENT_HIT_POINTS, X].
                    0xA5, 0x87,       // LDA $87         ;Load {TempVar:BossOffset} into Accumulator.
                    0x9D, 0xB0, 0x00, // STA $00B0,X     ;Store {TempVar:BossOffset} into [BOSS_OBJECT_MASTER_OFFSET, X].
                };

                this.romWriter.Seek((int)AIConstants.Brambler.OutOfBoundsFixLocation);
                this.romWriter.WriteBytes(patchCode);
            }
            if (options.BramblerOptions.WeaponFix)
            {
                // Bramblers spawned by Boreal Face have their own weapon coded in, however they do not use it, using the ones used by Tropicallo instead.
                // This makes them very weak. A small patch to calculate the weapon ID will allow both types of Bramblers to equip the correct weapon.
                // The code being overwritten to jump to the patch is the 'LDA #$001E' to load the weapon ID right before the long jump to the weapon equip code.
                // The patch will return with the updated weapon ID.
                ReadOnlySpan<byte> jumpCode = stackalloc byte[]
                {
                    0x20, 0x7C, 0xF3,       // JSR $F37C       ;Jump to BramblerWeaponFixPatch.
                };
                ReadOnlySpan<byte> patchCode = stackalloc byte[]
                {
                    //[BramblerWeaponFixPatch]
                    0xBD, 0xB0, 0x00,       // LDA $00B0,X     ;Load the parent bosses offset from [BOSS_OBJECT_MASTER_OFFSET, X].
                    0xAA,                   // TAX             ;Transfer the boss offset to Register X.
                    0xA9, 0x1E, 0x00,       // LDA #$001E      ;Load 0x001E into Accumulator. (Base value of Bramblers weapon)
                    0x18,                   // CLC             ;Clear Carry Flag.
                    0x7D, 0x42, 0x00,       // ADC $0042,X     ;Add [BOSS_ATTACK_PATTERN, X] to 0x001E.
                    0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                    0x60,                   // RTS             ;Return.
                };

                this.romWriter.Seek((int)AIConstants.Brambler.WeaponPatchJumpLocation);
                this.romWriter.WriteBytes(jumpCode);

                this.romWriter.Seek((int)AIConstants.Brambler.WeaponPatchLocation);
                this.romWriter.WriteBytes(patchCode);
            }
        }

        private void WriteDarkLichUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Lich);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.DarkLichOptions.UseSuperMagic)
            {
                // The instruction being over written here is after the creation of the body object.
                // The X-Register contains the offset to the body, so the patch routine needs to finish initializing it
                // and then load Dark Lich's offset before setting the super magic rate.
                byte rate = options.DarkLichOptions.SuperMagicRate;
                byte adrH = (byte)((AIConstants.SetSuperMagicRateRoutineLocation >> 8) & 0xFF);
                byte adrL = (byte)(AIConstants.SetSuperMagicRateRoutineLocation & 0xFF);
                ReadOnlySpan<byte> superMagicPatch = stackalloc byte[]
                {
                    0x9E, 0x7E, 0x00, // STZ $007E,X     ;Store Zero into[BOSS_STATE_FLAGS, X].
                    0xA6, 0x87,       // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                    0xA9, rate, 0x00, // LDA #$SuperRate ;Load SuperRate into Accumulator.
                    0x20, adrL, adrH, // JSR $FF00       ;Jump to SetSuperMagicRate.
                    0x60,             // RTS             ;Return.
                };
                this.romWriter.Seek((int)AIConstants.DarkLich.SetSuperMagicRateRoutineLocation);
                this.romWriter.WriteBytes(superMagicPatch);

                this.romWriter.Seek((int)AIConstants.DarkLich.InitializationRoutinePatchLocation);
                this.romWriter.Write(0x20);
                this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.SetSuperMagicRateRoutineLocation & 0xFFFF));
            }

            if (options.DarkLichOptions.HatesHeavyMetalMusic)
            {
                // So Dark Lich can neither cast spells nor use skills while underground.
                // He cannot use skills because his sprite sheet is to large and bleeds into the space used for skills.
                // Using one will corrupt his head graphics.
                // Trying to cast a spell will result in his AI locking up indefinitely, unsure why.
                // Only thing to do is remove the branch after the "Is My Head Above Ground?" check and allow him to physically attack regardless.
                this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadAttackRoutinePatchLocation);
                this.romWriter.WriteUInt16(0xEAEA);
            }

            if (options.DarkLichOptions.UndergroundHorizontalMovement)
            {
                // Command actions for the movement commands.
                BossAIPlayAnimationAction playHandAnimationAction = new BossAIPlayAnimationAction(0xF4);
                BossAIPlayAnimationAction playHandAndHeadAnimationAction = new BossAIPlayAnimationAction(0xF6);
                BossAISetBossCoordinateSpeedAction setCoordinateSpeedEastAction = new BossAISetBossCoordinateSpeedAction(0x0000, 0x0001, 0x0000, 0x0000, 0x0000, 0x0000);
                BossAISetBossCoordinateSpeedAction setCoordinateSpeedWestAction = new BossAISetBossCoordinateSpeedAction(0x0000, 0xFFFF, 0x0000, 0x0000, 0x0000, 0x0000);
                BossAISetBossCoordinateRoutineAction coordinateRoutineAction = new BossAISetBossCoordinateRoutineAction(AIConstants.GenericCoordinateUpdateRoutineOffset);
                BossAIEquipWeaponAction equipWeaponAction = new BossAIEquipWeaponAction(0x006E);
                BossAIFreezeAnimationAction freezeAnimationAction = new BossAIFreezeAnimationAction(0x0005);
                BossAIEndCommandAction endCommandAction = new BossAIEndCommandAction();

                BossAICommand handMoveEastCommand = new BossAICommand();
                handMoveEastCommand.Add(playHandAnimationAction);
                handMoveEastCommand.Add(setCoordinateSpeedEastAction);
                handMoveEastCommand.Add(coordinateRoutineAction);
                handMoveEastCommand.Add(equipWeaponAction);
                handMoveEastCommand.Add(freezeAnimationAction);
                handMoveEastCommand.Add(endCommandAction);

                BossAICommand handMoveWestCommand = new BossAICommand();
                handMoveWestCommand.Add(playHandAnimationAction);
                handMoveWestCommand.Add(setCoordinateSpeedWestAction);
                handMoveWestCommand.Add(coordinateRoutineAction);
                handMoveWestCommand.Add(equipWeaponAction);
                handMoveWestCommand.Add(freezeAnimationAction);
                handMoveWestCommand.Add(endCommandAction);

                BossAICommand handHeadMoveEastCommand = new BossAICommand();
                handHeadMoveEastCommand.Add(playHandAndHeadAnimationAction);
                handHeadMoveEastCommand.Add(setCoordinateSpeedEastAction);
                handHeadMoveEastCommand.Add(coordinateRoutineAction);
                handHeadMoveEastCommand.Add(equipWeaponAction);
                handHeadMoveEastCommand.Add(freezeAnimationAction);
                handHeadMoveEastCommand.Add(endCommandAction);

                BossAICommand handHeadMoveWestCommand = new BossAICommand();
                handHeadMoveWestCommand.Add(playHandAndHeadAnimationAction);
                handHeadMoveWestCommand.Add(setCoordinateSpeedWestAction);
                handHeadMoveWestCommand.Add(coordinateRoutineAction);
                handHeadMoveWestCommand.Add(equipWeaponAction);
                handHeadMoveWestCommand.Add(freezeAnimationAction);
                handHeadMoveWestCommand.Add(endCommandAction);

                // Write out the new movement commands and collect the pointers.
                Span<ushort> commandPointers = stackalloc ushort[4];
                this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadMoveCommandLocation);
                commandPointers[0] = (ushort)(this.romWriter.Position & 0xFFFF);
                commandPointers[1] = this.romWriter.WriteAICommand(handMoveEastCommand);
                commandPointers[2] = this.romWriter.WriteAICommand(handMoveWestCommand);
                commandPointers[3] = this.romWriter.WriteAICommand(handHeadMoveEastCommand);
                this.romWriter.WriteAICommand(handHeadMoveWestCommand);

                // Write the command pointers into the command table.
                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.DarkLich.MovementCommandIndex));
                this.romWriter.WriteUInt16(commandPointers[0]);
                this.romWriter.WriteUInt16(commandPointers[1]);
                this.romWriter.WriteUInt16(commandPointers[2]);
                this.romWriter.WriteUInt16(commandPointers[3]);

                // AI Scripts to use the movement commands.
                ReadOnlySpan<byte> undergroundAIScripts = stackalloc byte[]
                {
                    0x28, 0xFF,
                    0x29, 0xFF,
                    0x2A, 0xFF,
                    0x2B, 0xFF,
                };
                this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadMoveScriptsLocation);
                this.romWriter.WriteBytes(undergroundAIScripts);

                // Dark Lich's body continues to sync it's animation with the main boss while it's hiding underground.
                // The animation table does not have space to add more, so the whole table has to be moved.
                int address = (this.BossAIAuxiliaryBank << 16) | AIConstants.DarkLich.NewBodyAnimationSyncTableOffset;
                uint fixupAddress = (uint)(((this.BossAIAuxiliaryBank | 0xC0) << 16) | AIConstants.DarkLich.NewBodyAnimationSyncTableOffset);

                this.romReader.Seek((int)Constants.Bank1C.DarkLichBodyAnimationIdSyncTable);
                this.romWriter.Seek(address);
                for (int i = 0; i < Constants.Bank1C.DarkLichBodyAnimationIdSyncTableSize; i++)
                {
                    this.romWriter.WriteUInt16(this.romReader.ReadUInt16());
                }

                // Animations to make the body invisible while the hands move underground.
                this.romWriter.WriteUInt16(AIConstants.DarkLich.LichBodyInvisibleAnimationIndex);
                this.romWriter.WriteUInt16(AIConstants.DarkLich.LichBodyInvisibleAnimationIndex);
                this.romWriter.WriteUInt16(AIConstants.DarkLich.LichBodyInvisibleAnimationIndex);
                this.romWriter.WriteUInt16(AIConstants.DarkLich.LichBodyInvisibleAnimationIndex);

                // Patch the routine that loads the body animations.
                this.romWriter.Seek((int)AIConstants.DarkLich.LoadBodyAnimationIdPatchLocation);
                this.romWriter.WriteUInt24(fixupAddress);

                // Add pointers to the new AI Scripts to Dark Lich's Underground movement AI tables.
                // TODO: This is broken somehow is is overwriting the move north pointers.
                this.romWriter.Seek((int)Constants.Bank1C.DarkLichUndergroundHandMovementPointerTable);
                this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadMoveScriptsLocation & 0xFFFF) + (sizeof(ushort) * 0));
                this.romWriter.Seek(this.romWriter.Position + 2);
                this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadMoveScriptsLocation & 0xFFFF) + (sizeof(ushort) * 1));

                this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadMoveScriptsLocation & 0xFFFF) + (sizeof(ushort) * 2));
                this.romWriter.Seek(this.romWriter.Position + 2);
                this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadMoveScriptsLocation & 0xFFFF) + (sizeof(ushort) * 3));
            }
        }

        private void WriteDoomsWallUpgrades(BossAIUpgradeOptions options)
        {
            if (options.DoomsWallOptions.CaveInNameFix)
            {
                // Replace the invalid character ('2A') with a dash ('C6').
                this.romWriter.Seek((int)AIConstants.DoomsWall.CaveInNamePatchLocation);
                this.romWriter.Write(0xC6);
            }
        }

        private void WriteDragonAllUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Dragon);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.DragonAllOptions.SkillTargetingFix)
            {
                // Dragon skills are supposed to hit all opponents, however they set the targeting parameter to late.
                // All skills uses a common exit point that does a few things, one of which is setting the target parameter.
                // However, the targeting parameter needs to be set before spawning the skill object, not after.
                // Breath Wing does not have this issue and correctly sets the targeting parameter before creating the skill object.

                BossAICommand freezeBreathCommand = commandSet.Commands[AIConstants.Dragon.Commands.FreezeBreathIndex];
                BossAICommand fireBreathCommand = commandSet.Commands[AIConstants.Dragon.Commands.FireBreathIndex];
                BossAICommand blitzBreathCommand = commandSet.Commands[AIConstants.Dragon.Commands.BlitzBreathIndex];
                BossAICommand balloonRingCommand = commandSet.Commands[AIConstants.Dragon.Commands.BalloonRingIndex];
                BossAICommand sleepRingCommand = commandSet.Commands[AIConstants.Dragon.Commands.SleepRingIndex];
                BossAICommand confuseHoopsCommand = commandSet.Commands[AIConstants.Dragon.Commands.ConfuseHoopsIndex];

                // Confuse Hoops contains the common script data, so the targeting action just needs moved to the top of the command.
                confuseHoopsCommand.Move(2, 0);

                // For the other skills, we need to change all the script jump commands to jump to one command later then before.
                BossAIScriptJumpAction jumpToScriptAddressAction = new BossAIScriptJumpAction(0xF11A);
                freezeBreathCommand.Replace(2, jumpToScriptAddressAction);
                fireBreathCommand.Replace(2, jumpToScriptAddressAction);
                blitzBreathCommand.Replace(2, jumpToScriptAddressAction);
                balloonRingCommand.Replace(2, jumpToScriptAddressAction);
                sleepRingCommand.Replace(2, jumpToScriptAddressAction);

                // Then insert a targeting action at the start of each command.
                BossAISetBossDataAction targetAllAction = new BossAISetBossDataAction(0x00A9, 0x0004);
                freezeBreathCommand.Insert(0, targetAllAction);
                fireBreathCommand.Insert(0, targetAllAction);
                blitzBreathCommand.Insert(0, targetAllAction);
                balloonRingCommand.Insert(0, targetAllAction);
                sleepRingCommand.Insert(0, targetAllAction);

                // The pointer for Confuse Hoops doesn't have to change, the rest do.
                this.romWriter.Seek(AIConstants.Dragon.DragonSkillCommandsPatchLocation);
                Span<ushort> commandPointers = stackalloc ushort[5];
                commandPointers[0] = (ushort)(this.romWriter.Position & 0xFFFF);
                commandPointers[1] = this.romWriter.WriteAICommand(freezeBreathCommand);
                commandPointers[2] = this.romWriter.WriteAICommand(fireBreathCommand);
                commandPointers[3] = this.romWriter.WriteAICommand(blitzBreathCommand);
                commandPointers[4] = this.romWriter.WriteAICommand(balloonRingCommand);
                this.romWriter.WriteAICommand(sleepRingCommand);

                // Update the command pointer table.
                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.Dragon.Commands.FreezeBreathIndex));
                this.romWriter.WriteUInt16(commandPointers[0]);

                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.Dragon.Commands.FireBreathIndex));
                this.romWriter.WriteUInt16(commandPointers[1]);

                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.Dragon.Commands.BlitzBreathIndex));
                this.romWriter.WriteUInt16(commandPointers[2]);

                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.Dragon.Commands.BalloonRingIndex));
                this.romWriter.WriteUInt16(commandPointers[3]);

                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.Dragon.Commands.SleepRingIndex));
                this.romWriter.WriteUInt16(commandPointers[4]);
            }
        }

        private void WriteGreatViperUpdates(BossAIUpgradeOptions options)
        {
            this.romWriter.Seek(0x02);
        }

        private void WriteKettleKinUpdates(BossAIUpgradeOptions options)
        {
            if (options.KettleKinOptions.RestoreDeathMachine)
            {
                // Death Machine was removed in a lazy manner, which is good for us.
                // Most of his data still exists, the AI routines and commands are almost identical between Death Machine and Kettle Kin.
                // His graphics and sound effects also still exist in the US ROM and can be loaded without issue.
                // What is missing is his animation scripts and frame data, these were removed in localization.
                // We need to load them from the JP ROM and transplant them into the US ROM.

                // This is the wrong encoding, but it doesn't matter as we aren't reading string data.
                RomFile jpRom = new RomFile(this.JapaneseRomFilePath, this.romFile.Encoding);
                BossAIRomReader jpReader = new BossAIRomReader(jpRom);

                // Get Kettle Kin's & Death Machine's header.
                BossSetupHeader kettleKinHeader = this.romReader.ReadSetupHeader(BossFamily.KettleKin);
                BossSetupHeader deathMachineHeader = jpReader.ReadSetupHeader(BossFamily.DeathMachine);

                // The new name is longer then the original so he has to be relocated.
                // There's some free space at the end of the bank where it will fit.
                this.romWriter.Seek((int)AIConstants.KettleKin.NamePatchLocation);
                this.romWriter.WriteBytes(this.romFile.Encoding.GetBytes("Death Machine"));
                this.romWriter.Write(0x00);

                // Update the name pointer.
                this.romWriter.Seek((int)(Constants.Bank0A.EnemyNamesPointerTableAddress + (sizeof(ushort) * AIConstants.KettleKin.SpriteIndex)));
                this.romWriter.WriteUInt16((ushort)(AIConstants.KettleKin.NamePatchLocation & 0xFFFF));

                // Change Kettle Kins graphics loader script to load the chainsaw/drill graphics instead of the hammer/wheel.
                this.romWriter.Seek((int)AIConstants.KettleKin.ChainsawGraphicsLoaderPatchLocation);
                this.romWriter.Write(0x05);

                // Copy the animation scripts over to the US ROM.
                WriteDeathMachineAnimationScripts(jpReader);

                // Here is where I'd like to be fancy and have a frame parser to be able to pull out the data and alter it.
                // But I still haven't bothered to figure out the collision data portion of the frame.
                // So instead we'll just leverage the fact that the frame are all in a block and just copy the whole block to the US ROM.
                // We'll copy the frames to the same offsets somewhere in the extended ROM area and 
                // just add a call to the start of each animation to switch the frame bank.
                // One important detail is that the frame data has to start at an even address. Odd bytes are considered animation commands.
                jpReader.Seek((int)AIConstants.DeathMachine.FrameDataBlockStart);
                this.romWriter.Seek((int)((this.BossAIAuxiliaryBank << 16) | (AIConstants.DeathMachine.FrameDataBlockStart & 0xFFFF)));
                while (jpReader.Position < AIConstants.DeathMachine.FrameDataBlockEnd)
                {
                    this.romWriter.Write(jpReader.Read());
                }

                // Copy and re-write Death Machine's command set.
                WriteDeathMachineCommands(jpReader, kettleKinHeader, deathMachineHeader);

                // Robot Legs AI plays an animation indexed by the command being executed by the main boss.
                // Death Machine's drill animation table needs copied over Kettle Kin's.
                this.romWriter.Seek((int)Constants.Bank1C.KettleKinLegAnimationIdSyncTable);
                jpReader.Seek((int)Constants.Bank1C.KettleKinLegAnimationIdSyncTable);

                Span<byte> legSyncData = stackalloc byte[AIConstants.DeathMachine.MaxValidCommandIndex];
                legSyncData.Fill(0);

                jpReader.ReadBytes(legSyncData);
                if (options.KettleKinOptions.DrillSpinsDuringMovement)
                {
                    // Change the animation for the drill to the unused spinning drill animation for Death Machine's movement commands.
                    // This looks much nicer then the static drill.
                    legSyncData[AIConstants.KettleKin.Commands.WheelMoveSouth] = AIConstants.DeathMachine.DrillSpinAnimationIndex;
                    legSyncData[AIConstants.KettleKin.Commands.WheelMoveNorth] = AIConstants.DeathMachine.DrillSpinAnimationIndex;
                    legSyncData[AIConstants.KettleKin.Commands.WheelMoveWest] = AIConstants.DeathMachine.DrillSpinAnimationIndex;
                    legSyncData[AIConstants.KettleKin.Commands.WheelMoveEast] = AIConstants.DeathMachine.DrillSpinAnimationIndex;

                    //legSyncData[AIConstants.KettleKin.Commands.WheelShortCircuit] = AIConstants.DeathMachine.DrillInGroundAnimationIndex;
                }
                this.romWriter.WriteBytes(legSyncData);

                // The next set of bytes is for the Horizontal/Vertical Flip flags for the above leg animations.
                legSyncData.Fill(0);
                jpReader.ReadBytes(legSyncData);
                this.romWriter.WriteBytes(legSyncData);

                // Death Machine needs to be raised up higher then Kettle Kin when form changing to account for the extra height of the drill.
                // JP verison uses a z-coordinate of 0x28, we'll use 0x2A instead to raise him up a little higher so there is less lip between his body and the drill.
                this.romWriter.Seek((int)AIConstants.KettleKin.FormChangeZCoordinatePatchLocation);
                this.romWriter.Write(AIConstants.DeathMachine.FormChangeZCoordinate);
            }

            void WriteDeathMachineAnimationScripts(BossAIRomReader jpReader)
            {
                // Death Machine uses animations 41-4D. They need to be modified to switch frame banks.
                // Death Machine actually has a lot fewer animations then Kilroy/Kettle Kin (13 vs 21).
                // These animations are grouped by direction (Down > Side > Back).
                // So the unuused animations were probably originally for damage animations in different directions.
                BossAnimationScript script41 = jpReader.ReadAnimationScript(0x41); // BossAnimationScript_DeathMachine_Idle
                BossAnimationScript script42 = jpReader.ReadAnimationScript(0x42); // BossAnimationScript_DeathMachine_ChainsawThrust_DownView
                BossAnimationScript script43 = jpReader.ReadAnimationScript(0x43); // BossAnimationScript_DeathMachine_ShortCircuit
                BossAnimationScript script44 = jpReader.ReadAnimationScript(0x44); // BossAnimationScript_DeathMachine_WalkDownView
                BossAnimationScript script45 = jpReader.ReadAnimationScript(0x45); // BossAnimationScript_DeathMachine_ChainsawGoRound
                BossAnimationScript script46 = jpReader.ReadAnimationScript(0x46); // BossAnimationScript_DeathMachine_IdleSideView
                BossAnimationScript script47 = jpReader.ReadAnimationScript(0x47); // BossAnimationScript_DeathMachine_ChainsawThrust_SideView
                BossAnimationScript script48 = jpReader.ReadAnimationScript(0x48); // BossAnimationScript_DeathMachine_Unused_01
                BossAnimationScript script49 = jpReader.ReadAnimationScript(0x49); // BossAnimationScript_DeathMachine_WalkSideView
                BossAnimationScript script4A = jpReader.ReadAnimationScript(0x4A); // BossAnimationScript_DeathMachine_IdleBackView
                BossAnimationScript script4B = jpReader.ReadAnimationScript(0x4B); // BossAnimationScript_DeathMachine_ChainsawThrust_BackView
                BossAnimationScript script4C = jpReader.ReadAnimationScript(0x4C); // BossAnimationScript_DeathMachine_Unused_02
                BossAnimationScript script4D = jpReader.ReadAnimationScript(0x4D); // BossAnimationScript_DeathMachine_WalkBackView

                // Insert a new action at the start of each animation to switch the frame bank to where ever we put Death Machine's frame data.
                byte frameBank = (byte)(this.BossAIAuxiliaryBank | 0xC0);
                SetFrameBankBossAnimationCommand setFrameBankAction = new SetFrameBankBossAnimationCommand(frameBank);
                script41.Insert(0, setFrameBankAction);
                script42.Insert(0, setFrameBankAction);
                script43.Insert(0, setFrameBankAction);
                script44.Insert(0, setFrameBankAction);
                script45.Insert(0, setFrameBankAction);
                script46.Insert(0, setFrameBankAction);
                script47.Insert(0, setFrameBankAction);
                script48.Insert(0, setFrameBankAction);
                script49.Insert(0, setFrameBankAction);
                script4A.Insert(0, setFrameBankAction);
                script4B.Insert(0, setFrameBankAction);
                script4C.Insert(0, setFrameBankAction);
                script4D.Insert(0, setFrameBankAction);

                //script43.Insert(4, new BossToggleHorizontalFlipAnimationAction());
                //script43.Insert(6, new BossToggleHorizontalFlipAnimationAction());

                // Write the animation scripts to the US ROM and collect a list of pointers to them.
                Span<ushort> animPointers = stackalloc ushort[13];
                this.romWriter.Seek((int)AIConstants.KettleKin.AnimationScriptLocation);
                animPointers[0] = (ushort)(this.romWriter.Position & 0xFFFF);
                animPointers[1] = this.romWriter.WriteAnimationScript(script41);
                animPointers[2] = this.romWriter.WriteAnimationScript(script42);
                animPointers[3] = this.romWriter.WriteAnimationScript(script43);
                animPointers[4] = this.romWriter.WriteAnimationScript(script44);
                animPointers[5] = this.romWriter.WriteAnimationScript(script45);
                animPointers[6] = this.romWriter.WriteAnimationScript(script46);
                animPointers[7] = this.romWriter.WriteAnimationScript(script47);
                animPointers[8] = this.romWriter.WriteAnimationScript(script48);
                animPointers[9] = this.romWriter.WriteAnimationScript(script49);
                animPointers[10] = this.romWriter.WriteAnimationScript(script4A);
                animPointers[11] = this.romWriter.WriteAnimationScript(script4B);
                animPointers[12] = this.romWriter.WriteAnimationScript(script4C);
                this.romWriter.WriteAnimationScript(script4D);

                // Death Machine's section in the animation table still exists, though they all currently point to Minotaur's idle animation.
                this.romWriter.Seek((int)(Constants.Bank1B.BossAnimationPointerTableAddress + (sizeof(ushort) * AIConstants.DeathMachine.AnimationScriptStartIndex)));
                foreach (ushort animPointer in animPointers)
                {
                    this.romWriter.WriteUInt16(animPointer);
                }
            }

            void WriteDeathMachineCommands(BossAIRomReader jpReader, BossSetupHeader kettleKinHeader, BossSetupHeader deathMachineHeader)
            {
                // Death Machine actually has more commands defined then Kettle Kin does.
                // Commands 10-17 for Kettle Kin all just point to command 18.
                // For Death Machine these commands contain duplicates of his 4 Idle animations.
                // These unused commands need to be removed from the command set.
                // Removing those commands still won't allow Death Machine's commands to fit in Kettle Kin's space.
                // Kettle Kin controls the speed of his movement in Wheel Form via his animation scripts.
                // Where has Death Machine does it manually in the Drill Form movement commands.
                // So each movement command for Death Machine is 16 bytes longer then the one for Kettle Kin. (DM: 23, KK: 7)
                // There are more unused commands that can be removed for space.
                // Commands 01, 02, and 03 are idle animations for left, right and back. All of these are unused (7 bytes per command, 21 total).
                // Commands 08, 09, 0A, and 0B are yet another set of unused idle animation commands.  (7/8 bytes per command, 30 total).
                // That's still not enough bytes. So we need to do some minor re-wiring of the command set.
                // All verisons of the robot bosses contain two commands for each attack depending on which form they are in.
                // The commands for both forms are identical though. So the Drill Mode attacks can be re-pointed to the Leg Mode attacks
                // and the space used for the Drill Mode attacks can be reclaimed.
                // Wheel/Drill Mode Commands:
                // 23: Short Circuit.
                // 28, 29, 2A, 2B: Hammer Smash/Chainsaw Thurst (Down, Up, Left, Right).
                // 2C, 2D, 2E, 2F are unused pointers that point to the above attacks.
                // 30: Hammer-Go-Round/Chainsaw-Go-Round.
                // 31, 32: Lunar Boast, Lucid Barrier.
                BossAICommandSet kettleKinCommandSet = this.romReader.ReadAICommandSet(kettleKinHeader.AICommandSetPointer);
                BossAICommandSet deathMachineCommandSet = jpReader.ReadAICommandSet(deathMachineHeader.AICommandSetPointer);

                // Drill movement commands need repointed to areas with enough space to contain them.
                // South can stay where it is while the others need repointed.
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.WheelMoveSouth, AIConstants.KettleKin.Commands.WheelMoveSouth);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.IdleV2, AIConstants.KettleKin.Commands.WheelMoveNorth);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.DoomBeam, AIConstants.KettleKin.Commands.WheelMoveWest);
                kettleKinCommandSet.SetPointer(AIConstants.KettleKin.Commands.WheelMoveEast, (ushort)(kettleKinCommandSet.PointerTable[AIConstants.KettleKin.Commands.WheelMoveWest] + 25));

                // Repoint all the Drill attack commands and the unused idle commands to free up the space they were using.
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.Idle, AIConstants.KettleKin.Commands.IdleV2);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.IdleBackView, AIConstants.KettleKin.Commands.IdleBackViewV2);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.IdleLeftSideView, AIConstants.KettleKin.Commands.IdleLeftSideViewV2);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.IdleRightSideView, AIConstants.KettleKin.Commands.IdleRightSideViewV2);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.ShortCircuit, AIConstants.KettleKin.Commands.WheelShortCircuit);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerDown, AIConstants.KettleKin.Commands.WheelHammerDown);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerUp, AIConstants.KettleKin.Commands.WheelHammerUp);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerLeft, AIConstants.KettleKin.Commands.WheelHammerLeft);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerRight, AIConstants.KettleKin.Commands.WheelHammerRight);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerDown, AIConstants.KettleKin.Commands.WheelHammerDownDummied);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerUp, AIConstants.KettleKin.Commands.WheelHammerUpDummied);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerLeft, AIConstants.KettleKin.Commands.WheelHammerLeftDummied);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerRight, AIConstants.KettleKin.Commands.WheelHammerRightDummied);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.HammerGoRound, AIConstants.KettleKin.Commands.WheelHammerGoRound);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.LunarBoast, AIConstants.KettleKin.Commands.WheelLunarBoast);
                kettleKinCommandSet.CopyPointer(AIConstants.KettleKin.Commands.LucidBarrier, AIConstants.KettleKin.Commands.WheelLucidBarrier);

                // The generic coordinate update routine has different offsets in the JP and US ROMs.
                // So the set coordinate routine action needs replaced with one that points to the correct offset.
                BossAISetBossCoordinateRoutineAction coordinateRoutineAction = new BossAISetBossCoordinateRoutineAction(AIConstants.GenericCoordinateUpdateRoutineOffset);
                deathMachineCommandSet.Commands[AIConstants.KettleKin.Commands.WheelMoveSouth].Replace(2, coordinateRoutineAction);
                deathMachineCommandSet.Commands[AIConstants.KettleKin.Commands.WheelMoveNorth].Replace(2, coordinateRoutineAction);
                deathMachineCommandSet.Commands[AIConstants.KettleKin.Commands.WheelMoveWest].Replace(3, coordinateRoutineAction);
                deathMachineCommandSet.Commands[AIConstants.KettleKin.Commands.WheelMoveEast].Replace(3, coordinateRoutineAction);

                BossAISetCommandLockRoutineAction lockRoutine = new BossAISetCommandLockRoutineAction(0x34B9);
                deathMachineCommandSet.Commands[AIConstants.KettleKin.Commands.ShortCircuit].Replace(1, lockRoutine);
                //02/D5FC: 04 B934         ;Set Command Lock Routine: BossCommandLock_TakingDamage. (US)
                //C2/D627: 04 9634         ;Set Command Lock Routine: BossCommandLock_TakingDamage. (JP)

                // Fill the AI command area with FF.
                this.romWriter.Seek((int)AIConstants.KettleKin.FillSpaceStart);
                while (this.romWriter.Position < AIConstants.KettleKin.FillSpaceEnd) { this.romWriter.Write(0xFF); }

                // Write out the modified command pointer table.
                this.romWriter.Seek(Constants.Bank02Offset | kettleKinHeader.AICommandSetPointer);
                foreach (ushort pointer in kettleKinCommandSet.PointerTable)
                {
                    this.romWriter.WriteUInt16(pointer);
                }

                // Don't write any of these commands.
                ReadOnlySpan<byte> ignoreList = stackalloc byte[]
                {
                    AIConstants.KettleKin.Commands.IdleV2,
                    AIConstants.KettleKin.Commands.IdleBackViewV2,
                    AIConstants.KettleKin.Commands.IdleLeftSideViewV2,
                    AIConstants.KettleKin.Commands.IdleRightSideViewV2,
                    AIConstants.KettleKin.Commands.IdleV3,
                    AIConstants.KettleKin.Commands.IdleBackViewV3,
                    AIConstants.KettleKin.Commands.IdleLeftSideViewV3,
                    AIConstants.KettleKin.Commands.IdleRightSideViewV3,
                    AIConstants.KettleKin.Commands.IdleV4,
                    AIConstants.KettleKin.Commands.IdleBackViewV4,
                    AIConstants.KettleKin.Commands.IdleLeftSideViewV4,
                    AIConstants.KettleKin.Commands.IdleRightSideViewV4,
                    AIConstants.KettleKin.Commands.HammerDownDummied,
                    AIConstants.KettleKin.Commands.HammerUpDummied,
                    AIConstants.KettleKin.Commands.HammerLeftDummied,
                    AIConstants.KettleKin.Commands.HammerRightDummied,
                    AIConstants.KettleKin.Commands.WheelShortCircuit,
                    AIConstants.KettleKin.Commands.LucidBarrierDummied01,
                    AIConstants.KettleKin.Commands.LucidBarrierDummied02,
                    AIConstants.KettleKin.Commands.DoomBeam,
                    AIConstants.KettleKin.Commands.WheelHammerDownDummied,
                    AIConstants.KettleKin.Commands.WheelHammerUpDummied,
                    AIConstants.KettleKin.Commands.WheelHammerLeftDummied,
                    AIConstants.KettleKin.Commands.WheelHammerRightDummied,
                    AIConstants.KettleKin.Commands.WheelHammerGoRound,
                    AIConstants.KettleKin.Commands.WheelLunarBoast,
                    AIConstants.KettleKin.Commands.WheelLucidBarrier,
                };

                int commandIndex = 0;
                for (byte i = 0; i < AIConstants.DeathMachine.MaxValidCommandIndex; i++)
                {
                    if (!ignoreList.Contains(i))
                    {
                        BossAICommand command = deathMachineCommandSet.Commands[commandIndex];

                        this.romWriter.Seek(Constants.Bank02Offset | kettleKinCommandSet.PointerTable[i]);
                        this.romWriter.WriteAICommand(command);
                    }
                    commandIndex++;
                }
            }
        }

        private void WriteHexasUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Hexas);
            EnemyStatEntry statEntry = this.romReader.ReadStatEntry(AIConstants.Hexas.SpriteIndex);

            if (options.HexasOptions.ImprovedBarrierChange)
            {
                // How does Hexas Barrier Change ability work? Mostly not at all.
                // Both the movement and attack routines contain code to control it, which would probably be problematic if it worked properly.
                // Attack Routine:   Contains some simple code that with a 33% chance, checks if bit 0x8000 is set in Hexas state flags, and if it is
                //                   then it picks a random element and changes to it if it isn't already in that element.
                //                   The problem with this code? Nothing ever sets bit 0x8000 in the state flags! Making this code unused.
                // Movement Routine: This routine ultimately controls the Barrier Change due to the attack code being unused.
                //                   However, the fact it controls it at all seems more like a bug then a feature.
                //                   If Hexas's target is more then 128 total pixels away then he will attempt to move closer to them.
                //                   Hexas can only move cardinally, but his direction is calculated ordinally.
                //                   This causes it out out of bounds index his movement pointer table, and the table after it happens to be Barrier Change pointers.
                //                   This means you can control his element by which direction you are from him.
                //                   West == Luna, North-West == Undine, North == Gnome, North-East == Salamando. His Sylphid form is inaccessible.
                //                   There is also no check to see if he is already this element, so he will change over and over again as long as you stand in the right spot.
                // The problem with Barrier Change doesn't end there though. While the commands that control the ability correctly update his element,
                // they do not update the elements he is strong and weak to, resulting in him remaining neutral to all elements at all times.
                //
                // So to fix this we'll throw out the original code and move the entire Barrier Change control to Hexas movement routine.
                // Using the movement routine allows for the ability to fire more often and doesn't eat up one of Hexas's attack calls.
                // Hexas will change his barrier under two conditions:
                // 1) Hexas has taken damage 3 times.
                // 2) After 21 AI calls Hexas will have a 66% chance of changing his Barrier every time he moves.
                // Hexas will also no longer attempt to move closer to his target. He will only run away, idle or barrier change.
                // The method of counting used here isn't frame accurate. Many times the AI routines will not be caused because the
                // system is paused due to something happening (most likely one of Hexas's own spells). Since Hexas is a structured boss
                // that runs though the main state machine we don't have full control over the AI Call Timer address ($0094) and it never gets high enough to be useful.
                // With this method of timing he generally casts 2-3 spells before changing elements.

                this.romWriter.Seek((int)AIConstants.Hexas.FillSpaceStart);
                while (this.romWriter.Position < AIConstants.Hexas.FillSpaceEnd) { this.romWriter.Write(0xFF); }

                // First we need to patch the initialize routine to set the new hit counter variable.
                this.romWriter.Seek((int)AIConstants.Hexas.InitRoutinePatchLocation);
                ReadOnlySpan<byte> initializationRoutinePatchPrefix = stackalloc byte[]
                {
                    0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                };
                this.romWriter.WriteBytes(initializationRoutinePatchPrefix);
                if (options.HexasOptions.UseSuperMagic)
                {
                    // If we are granting Hexas super magic then we need to patch in a call to the routine to set the rate.
                    byte rate = options.HexasOptions.SuperMagicRate;
                    byte adrH = (byte)((AIConstants.SetSuperMagicRateRoutineLocation >> 8) & 0xFF);
                    byte adrL = (byte)(AIConstants.SetSuperMagicRateRoutineLocation & 0xFF);
                    ReadOnlySpan<byte> superMagicPatch = stackalloc byte[]
                    {
                        0xA9, rate, 0x00, // LDA #$SuperRate ;Load SuperRate into Accumulator.
                        0x20, adrL, adrH, // JSR $FF00       ;Jump to SetSuperMagicRate.
                    };
                    this.romWriter.WriteBytes(superMagicPatch);

                    // Magic level also needs to be 8 to cast super magic.
                    statEntry.MagicLevel = 8;
                }
                ReadOnlySpan<byte> initializationRoutinePatch = stackalloc byte[]
                {
                    //0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                    0xA9, 0x05, 0x00,       // LDA #$0005      ;Load 0x0005 into Accumulator.
                    0x9D, 0xD2, 0x01,       // STA $01D2,X     ;Store 0x0005 into [BARRIER_CHANGE_HIT_COUNT, X].
                    0x60,                   // RTS             ;Return.
                };
                this.romWriter.WriteBytes(initializationRoutinePatch);

                // Next the new movement routine, this controls the barrier change ability.
                // Optionally, add the infinite MP patch as well.
                WriteMovementRoutine(options.HexasOptions);

                // All the unused barrier change stuff in the attack routine is wasted space that can used for better things.
                // Need to add some code to the top of the element attack jump routine to select the target and increment the counter.
                ReadOnlySpan<byte> attackRoutinePatch = stackalloc byte[]
                {
                    0x20, 0x41, 0x3A,      // JSR $3A41       ;Jump to TargetClosestOpponentAndSetOrdinalDirection.
                    0xFE, 0xD4, 0x01,      // INC $01D4,X     ;Increment [BARRIER_CHANGE_TIMER, X].
                };
                this.romWriter.Seek((int)AIConstants.Hexas.AttackRoutinePatchLocation);
                this.romWriter.WriteBytes(attackRoutinePatch);

                // Lastly a damage routine needs created to decrement the hit counter.
                ReadOnlySpan<byte> damagedRoutine = stackalloc byte[]
                {
                    0xBD, 0xD2, 0x01,      // LDA $01D2,X     ;Load [BARRIER_CHANGE_HIT_COUNT, X] into Accumulator.
                    0xF0, 0x04,            // BEQ $04         ;Branch and set the animation if the hit counter is zero.
                    0x3A,                  // DEC A           ;Decrement the hit counter.
                    0x9D, 0xD2, 0x01,      // STA $01D2,X     ;Store the value back into [BARRIER_CHANGE_HIT_COUNT, X].
                    0xA9, 0x3C, 0xE1,      // LDA #$E13C      ;Load 0xE146 into Accumulator. (Set Animation: Damaged)
                    0x60,                  // RTS             ;Return.
                };
                this.romWriter.Seek((int)AIConstants.Hexas.DamagedRoutinePatchLocation);
                this.romWriter.WriteBytes(damagedRoutine);

                // Because the set data action writes a ushort we also have to write to the weak/strong monster type fields.
                // Since they are unused, we'll just write zero there for each action.
                List<BossAICommand> commands = this.commandParser.ParseCommands(header.AICommandSetPointer);
                ushort shadePointer = WriteBarrierChangeCommands(commands);

                // Next, optionally add support for the Shade element.
                if (options.HexasOptions.BarrierChangeShade)
                {
                    WriteShadeBarrierChange(shadePointer);
                }

                // Finally update the header with pointers to the new routines.
                //header.MovementRoutinePointer = (ushort)(AIConstants.Hexas.MoveRoutinePatchLocation & 0xFFFF);
                //header.AttackRoutinePointer = (ushort)(AIConstants.Hexas.AttackRoutinePatchLocation & 0xFFFF);
                //header.DamagedRoutinePointer = (ushort)(AIConstants.Hexas.DamagedRoutinePatchLocation & 0xFFFF);
            }
            else
            {
                int offset = 0;
                if (options.HexasOptions.InfiniteMP)
                {
                    // If not doing AI rewrites then just create a small routine to set MP and target,
                    // then patch the jump at the top the move routine to call it.
                    ReadOnlySpan<byte> infiniteMPPatch = stackalloc byte[]
                    {
                        0xA9, 0x63, 0x63,      // LDA #$6363      ;Load 0x6363 into Accumulator.
                        0x9D, 0x86, 0x01,      // STA $0186,X     ;Store 0x6363 into [CURRENT_MANA_POINTS, X] and [MAX_MANA_POINTS, X].
                        0x20, 0x41, 0x3A,      // JSR $3A41       ;Jump to TargetClosestOpponentAndSetOrdinalDirection.
                        0x60,                  // RTS             ;Return.
                    };
                    this.romWriter.Seek((int)AIConstants.Hexas.MoveRoutinePatchLocation);
                    this.romWriter.WriteBytes(infiniteMPPatch);

                    this.romWriter.Seek((Constants.Bank02Offset | header.MovementRoutinePointer) + 1);
                    this.romWriter.WriteUInt16((ushort)(AIConstants.Hexas.MoveRoutinePatchLocation & 0xFFFF));

                    offset = infiniteMPPatch.Length;
                }

                if (options.HexasOptions.UseSuperMagic)
                {
                    // If we are granting Hexas super magic then we need to patch in a call to the routine to set the rate.
                    byte rate = options.HexasOptions.SuperMagicRate;
                    byte adrH = (byte)((AIConstants.SetSuperMagicRateRoutineLocation >> 8) & 0xFF);
                    byte adrL = (byte)(AIConstants.SetSuperMagicRateRoutineLocation & 0xFF);
                    ReadOnlySpan<byte> superMagicPatch = stackalloc byte[]
                    {
                        0xA9, rate, 0x00, // LDA #$SuperRate ;Load SuperRate into Accumulator.
                        0x20, adrL, adrH, // JSR $FF00       ;Jump to SetSuperMagicRate.
                        0xA9, 0x3F, 0x00, // LDA #$003F      ;Load 0x003F into Accumulator.
                        0x60,             // RTS             ;Return.
                    };

                    int superMagicPatchPointer = (int)AIConstants.Hexas.MoveRoutinePatchLocation + offset;
                    this.romWriter.Seek(superMagicPatchPointer);
                    this.romWriter.WriteBytes(superMagicPatch);

                    // The first instruction in the init routine is LDA #$003F, which is Hexas's Z-Coordinate.
                    // Our patch will load the value instead, and the 3 bytes for the instruction turned into a JSR.
                    adrH = (byte)((superMagicPatchPointer >> 8) & 0xFF);
                    adrL = (byte)(superMagicPatchPointer & 0xFF);
                    ReadOnlySpan<byte> initRoutinePatch = stackalloc byte[]
                    {
                        0x20, adrL, adrH,      // JSR $????       ;Jump to HexasSuperMagicInitPatch.
                    };
                    this.romWriter.Seek(Constants.Bank02Offset | header.InitializeRoutinePointer);
                    this.romWriter.WriteBytes(initRoutinePatch);

                    // Magic level also needs to be 8 to cast super magic.
                    statEntry.MagicLevel = 8;
                }
            }

            if (options.HexasOptions.LunaMoogleBubbles)
            {
                // Moogle Bubbles is an unused boss skill, the weapon for it is unfinished, having zero power and not actually inflicting Moogle.
                ManaBossWeapon moogleBubbles = this.romReader.ReadWeapon(AIConstants.Hexas.MoogleBubblesWeaponIndex);
                moogleBubbles.Power = 0x47;
                moogleBubbles.StatusEffects = StatusEffects.Moogle;
                moogleBubbles.InflictionRate = 0x63;
                this.romWriter.WriteWeapon(moogleBubbles, AIConstants.Hexas.MoogleBubblesWeaponIndex);

                // Note: Hexas keeps the palette for his spell rings in the palette slot used for skills.
                // So we need to hold the boss in place long enough with the freeze animation command for the bubble animation to finish.
                // Otherwise command 1D will be invoked to soon and restore the spell palette while the bubbles are still on screen.
                BossAICommand useSkillMoogleBubbles = new BossAICommand();
                useSkillMoogleBubbles.Add(new BossAIPlayAnimationAction(0x00C1));        // Play Animation: BossAnimationScript_Hexas_Idle.
                useSkillMoogleBubbles.Add(new BossAIPlaySkillCommand(0x06, 0x00, 0xE6)); // Play Boss Skill Animation: Moogle Bubbles, X: 00, Y: E6.
                useSkillMoogleBubbles.Add(new BossAIEquipWeaponAction(0x0006));          // Equip Boss Weapon Skill_MoogleBubbles.
                useSkillMoogleBubbles.Add(new BossAIFreezeAnimationAction(0x0022));      // Freeze Animation for 0x0022 Ticks.
                useSkillMoogleBubbles.Add(new BossAIEndCommandAction());                 // End Command Subset.

                // Write out the command to the ROM.
                this.romWriter.Seek(Constants.Bank02Offset | AIConstants.Hexas.MoogleBubblesCommandOffset);
                ushort scriptOffset = this.romWriter.WriteAICommand(useSkillMoogleBubbles);

                // Write an AI script to invoke the Moogle Bubbles command.
                // Commands 1E-2F are all dummied out and unused. Shade will use 1E-22.
                this.romWriter.Write(0x25);
                this.romWriter.Write(0x1D); // Command 1D reloads the correct palette for the spell casting animation.
                this.romWriter.Write(0xFF);

                int lunaAIRouletteTableLocation = this.romWriter.Position;

                // Update command 0x25 to the new Moogle Bubbles command.
                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * 0x25));
                this.romWriter.WriteUInt16(AIConstants.Hexas.MoogleBubblesCommandOffset);

                // Move the Luna AI Roulette Pointer Table and add the pointer for Moogle Bubbles.
                this.romReader.Seek((int)AIConstants.Hexas.LunaElementPointerTableLocation);
                this.romWriter.Seek(lunaAIRouletteTableLocation);
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16());
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16());
                this.romWriter.WriteUInt16(scriptOffset);

                // Make the RNG call in the Luna attack routine get a number between 0-2 instead of 0-1.
                this.romWriter.Seek((int)AIConstants.Hexas.LunaAttackRngPatchLocation);
                this.romWriter.Write(0x02);

                // Change the attack loader instruction to point to the new AI Roulette table.
                this.romWriter.Seek((int)AIConstants.Hexas.LunaAttackLoadPointerPatchLocation);
                this.romWriter.WriteUInt24((uint)(lunaAIRouletteTableLocation | 0xC00000));
            }

            if (options.HexasOptions.SpawnElementFix)
            {
                // Hexas spawns with no element, but his barrier change state is Luna.
                statEntry.Element = ElementalType.Luna;
            }

            this.romWriter.WriteSetupHeader(BossFamily.Hexas, header);
            this.romWriter.WriteStatEntry(statEntry, AIConstants.Hexas.SpriteIndex);

            void WriteMovementRoutine(BossAIUpgradeOptions.HexasAIOptions hexasOptions)
            {
                byte numElement = (byte)(hexasOptions.BarrierChangeShade ? 0x05 : 0x04);
                byte bcH = (byte)(hexasOptions.BarrierChangeShade ? 0xC2 : 0xDC);
                byte bcM = (byte)(hexasOptions.BarrierChangeShade ? 0xF3 : 0xE3);
                byte bcL = (byte)(hexasOptions.BarrierChangeShade ? 0x90 : 0x24);
                ReadOnlySpan<byte> movementRoutine = stackalloc byte[]
                {
                    //[Hexas_Movement]
                    0x20, 0x41, 0x3A,      // JSR $3A41       ;Jump to TargetClosestOpponentAndSetOrdinalDirection.
                    0xFE, 0xD4, 0x01,      // INC $01D4,X     ;Increment [BARRIER_CHANGE_TIMER, X].
                    0xBD, 0xD2, 0x01,      // LDA $01D2,X     ;Load [BARRIER_CHANGE_HIT_COUNT, X] into Accumulator.
                    0xF0, 0x10,            // BEQ $10         ;Branch to Hexas_Movement_BarrierChange if the hit counter is zero.
                    0xBD, 0xD4, 0x01,      // LDA $0096,X     ;Load [BOSS_AI_CALL_TIMER, X] into Accumulator.
                    0xC9, 0x15, 0x00,      // CMP #$0015      ;Compare [BARRIER_CHANGE_TIMER, X] against 0x0015.
                    0x90, 0x37,            // BCC $37         ;Branch to Hexas_Movement_RunAway if enough frames haven't passed.
                    0xA9, 0x02, 0x00,      // LDA #$0002      ;Load 0x0002 into Accumulator.
                    0x20, 0x0B, 0x30,      // JSR $300B       ;Jump to GetBossRandomNumberInRange.
                    0xF0, 0x2C,            // BEQ $2C         ;Branch to Hexas_Movement_RunAway if the random value is zero. (66% chance of Barrier Change)

                    //[Hexas_Movement_BarrierChange]
                    0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                    0xBD, 0x7E, 0x00,       // LDA $007E,X     ;Load [BOSS_STATE_FLAGS, X] into Accumulator.
                    0x29, 0xFF, 0x00,       // AND #$00FF      ;Drop the high byte.
                    0x85, 0x00,             // STA $00         ;Store value into {TempVar:CurrentForm}.
                    0xA9, numElement, 0x00, // LDA #$0004/5    ;Load 0x0004/5 into Accumulator. (Get a new random element)
                    0x20, 0x0B, 0x30,       // JSR $300B       ;Jump to GetBossRandomNumberInRange.
                    0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                    0xC5, 0x00,             // CMP $00         ;Compare the result against {TempVar:CurrentForm}.
                    0xF0, 0x16,             // BEQ $16         ;Branch to Hexas_Movement_RunAway if we picked the element we currently are.
                    0x0A,                   // ASL A           ;Multiply By 2.
                    0xAA,                   // TAX             ;Transfer the index to Register X.
                    0xBF,  bcL, bcM, bcH,   // LDA $DCE324,X   ;Load a Barrier Change pointer from PointerTable_HexasBarrierChange.
                    0x85, 0x00,             // STA $00         ;Store value into {TempVar:BarrierChangePointer}.
                    0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
                    0x9E, 0xD4, 0x01,       // STZ $01D4,X     ;Store Zero into [BARRIER_CHANGE_TIMER, X].
                    0xA9, 0x03, 0x00,       // LDA #$0003      ;Load 0x0003 into Accumulator.
                    0x9D, 0xD2, 0x01,       // STA $01D2,X     ;Store 0x0003 into [BARRIER_CHANGE_HIT_COUNT, X].
                    0xA5, 0x00,             // LDA $00         ;Load {TempVar:BarrierChangePointer} into Accumulator.
                    0x60,                   // RTS             ;Return.

                    //[Hexas_Movement_RunAway]
                    0xBD, 0xA9, 0x00,       // LDA $00A9,X     ;Load [BOSS_CURRENT_TARGET, X] into Accumulator.
                    0x20, 0xD9, 0x08,       // JSR $08D9       ;Jump to GetRelativeUncappedDistanceFromTarget.
                    0xC9, 0x06, 0x00,       // CMP #$0006      ;Compare result against 0x0006.
                    0xB0, 0x0A,             // BCS $0A         ;Branch to Hexas_Movement_Idle if Hexas is far enough away from his target.
                    0xBD, 0xAB, 0x00,       // LDA $00AB,X     ;Load [BOSS_DIRECTION, X] into Accumulator.
                    0x0A,                   // ASL A           ;Multiply By 2.
                    0xAA,                   // TAX             ;Transfer the pointer to Register X.
                    0xBF, 0x14, 0xE3, 0xDC, // LDA $DCE314,X   ;Load a movement pointer into Accumulator. (Hexas will run from his target)
                    0x60,                   // RTS             ;Return.

                    //[Hexas_Movement_Idle]
                    0xA9, 0x46, 0xE1,       // LDA #$E146      ;Load 0xE146 into Accumulator. (Set Animation: Idle)
                    0x60,                   // RTS             ;Return.
                };

                this.romWriter.Seek((int)AIConstants.Hexas.MoveRoutinePatchLocation);
                if (hexasOptions.InfiniteMP)
                {
                    // If we are also giving Hexas infinite MP then add that patch to the top of the new movement routine.
                    ReadOnlySpan<byte> infiniteMPPatch = stackalloc byte[]
                    {
                        0xA9, 0x63, 0x63,      // LDA #$6363      ;Load 0x6363 into Accumulator.
                        0x9D, 0x86, 0x01,      // STA $0186,X     ;Store 0x6363 into [CURRENT_MANA_POINTS, X] and [MAX_MANA_POINTS, X].
                    };
                    this.romWriter.WriteBytes(infiniteMPPatch);
                }
                this.romWriter.WriteBytes(movementRoutine);
            }

            ushort WriteBarrierChangeCommands(IReadOnlyList<BossAICommand> commands)
            {
                BossAICommand lunaBarrierCommand = commands[0x06];
                lunaBarrierCommand.Insert(2, new BossAISetBossDataAction(0x01A0, 0x0000)); // Weak:   Neutral
                lunaBarrierCommand.Insert(3, new BossAISetBossDataAction(0x01A2, 0x0000)); // Resist: Neutral

                BossAICommand undineBarrierCommand = commands[0x07];
                undineBarrierCommand.Insert(2, new BossAISetBossDataAction(0x01A0, 0x0800)); // Weak:   Salamando
                undineBarrierCommand.Insert(3, new BossAISetBossDataAction(0x01A2, 0x0400)); // Resist: Undine

                BossAICommand gnomeBarrierCommand = commands[0x08];
                gnomeBarrierCommand.Insert(2, new BossAISetBossDataAction(0x01A0, 0x0200)); // Weak:   Sylphid
                gnomeBarrierCommand.Insert(3, new BossAISetBossDataAction(0x01A2, 0x0100)); // Resist: Gnome

                BossAICommand salamandoBarrierCommand = commands[0x09];
                salamandoBarrierCommand.Insert(2, new BossAISetBossDataAction(0x01A0, 0x0400)); // Weak:   Undine
                salamandoBarrierCommand.Insert(3, new BossAISetBossDataAction(0x01A2, 0x0800)); // Resist: Salamando

                BossAICommand sylphidBarrierCommand = commands[0x0A];
                sylphidBarrierCommand.Insert(2, new BossAISetBossDataAction(0x01A0, 0x0100)); // Weak:   Gnome
                sylphidBarrierCommand.Insert(3, new BossAISetBossDataAction(0x01A2, 0x0200)); // Resist: Sylphid

                List<ushort> newCommandPointers = new List<ushort>(5);
                this.romWriter.Seek(Constants.Bank02Offset | AIConstants.Hexas.BarrierChangeCommandOffset);
                newCommandPointers.Add((ushort)(this.romWriter.Position & 0xFFFF));

                newCommandPointers.Add(this.romWriter.WriteAICommand(lunaBarrierCommand));
                newCommandPointers.Add(this.romWriter.WriteAICommand(undineBarrierCommand));
                newCommandPointers.Add(this.romWriter.WriteAICommand(gnomeBarrierCommand));
                newCommandPointers.Add(this.romWriter.WriteAICommand(salamandoBarrierCommand));
                ushort shadePointer = this.romWriter.WriteAICommand(sylphidBarrierCommand);

                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * 0x06));
                foreach (ushort pointer in newCommandPointers)
                {
                    this.romWriter.WriteUInt16(pointer);
                }

                return shadePointer;
            }

            void WriteShadeBarrierChange(ushort shadePointer)
            {
                BossAICommand barrierChangeShade = new BossAICommand();
                barrierChangeShade.Add(new BossAISetBossDataAction(0x007E, 0x0005)); // Set 0005 to [BOSS_STATE_FLAGS]. (Shade Barrier)
                barrierChangeShade.Add(new BossAISetBossDataAction(0x0192, 0x2010)); // Set 2040 to [MONSTER_TYPE] & [ELEMENT]. (Type: Evil/Dead. Element: Shade)
                barrierChangeShade.Add(new BossAISetBossDataAction(0x01A0, 0x2000)); // Weak:   Lumina
                barrierChangeShade.Add(new BossAISetBossDataAction(0x01A2, 0x1000)); // Resist: Shade
                barrierChangeShade.Add(new BossAIScriptJumpAction(0xE26B));          // Jump to Address: $E26B.

                BossAICommand shadeDarkForce = new BossAICommand();
                shadeDarkForce.Add(new BossAICastSpellAction(ManaSpell.DarkForce, 0x00)); // Cast Spell: Dark Force. Target: Current Target.
                shadeDarkForce.Add(new BossAIScriptJumpAction(0xE28D));                   // Jump to Script Address: $E28D. (Lower Arm)

                BossAICommand shadeEvilGate = new BossAICommand();
                shadeEvilGate.Add(new BossAICastSpellAction(ManaSpell.EvilGate, 0x00)); // Cast Spell: Evil Gate. Target: Current Target.
                shadeEvilGate.Add(new BossAIScriptJumpAction(0xE2A6));                  // Jump to Script Address: $E2A6. (Upper Arm)

                BossAICommand shadeDarkForceDoubleCast1 = new BossAICommand();
                shadeDarkForceDoubleCast1.Add(new BossAICastSpellAction(ManaSpell.DarkForce, 0x00)); // Cast Spell: Dark Force. Target: Current Target.
                shadeDarkForceDoubleCast1.Add(new BossAIScriptJumpAction(0xE2BF));                   // Jump to Script Address: $E2BF. (Lower Arm)

                BossAICommand shadeDarkForceDoubleCast2 = new BossAICommand();
                shadeDarkForceDoubleCast2.Add(new BossAICastSpellAction(ManaSpell.DarkForce, 0x00)); // Cast Spell: Dark Force. Target: Current Target.
                shadeDarkForceDoubleCast2.Add(new BossAIScriptJumpAction(0xE2D8));                   // Jump to Script Address: $E2D8. (Upper Arm)

                this.romWriter.Seek(Constants.Bank02Offset | shadePointer);

                // Write the Shade AI commands and build the pointer table.
                List<ushort> commandPointers = new List<ushort>(5);
                commandPointers.Add(shadePointer);
                commandPointers.Add(this.romWriter.WriteAICommand(barrierChangeShade));
                commandPointers.Add(this.romWriter.WriteAICommand(shadeDarkForce));
                commandPointers.Add(this.romWriter.WriteAICommand(shadeEvilGate));
                commandPointers.Add(this.romWriter.WriteAICommand(shadeDarkForceDoubleCast1));
                ushort scriptPointer = this.romWriter.WriteAICommand(shadeDarkForceDoubleCast2);

                // Commands 1E-2F are all dummied out, so the can be reused for Shade commands.
                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * 0x1E));
                foreach (ushort pointer in commandPointers)
                {
                    this.romWriter.WriteUInt16(pointer);
                }

                // AI scripts to invoke the Shade commands.
                this.romWriter.Seek(Constants.Bank02Offset | scriptPointer);
                ReadOnlySpan<byte> shadeAIScripts = stackalloc byte[]
                {
                    0x1E, 0xFF,       // Action: Elemental Change: Shade.
                    0x1F, 0xFF,       // Cast Spell: 'Dark Force' On 'Current Target'.
                    0x20, 0xFF,       // Cast Spell: 'Evil Gate' On 'Current Target'.
                    0x21, 0x22, 0xFF, // Double Cast: 'Dark Force' And 'Dark Force' On 'Current Target'.
                };
                this.romWriter.WriteBytes(shadeAIScripts);

                // Shade's AI Roulette Pointer Table.
                ushort attackPointerTableOffset = (ushort)(this.romWriter.Position & 0xFFFF);
                this.romWriter.WriteUInt16((ushort)(scriptPointer + (sizeof(ushort) * 1)));
                this.romWriter.WriteUInt16((ushort)(scriptPointer + (sizeof(ushort) * 2)));
                this.romWriter.WriteUInt16((ushort)(scriptPointer + (sizeof(ushort) * 3)));

                // The Barrier Change AI Roulette Pointer Table has to be relocated since there isn't space to add Shade.
                this.romReader.Seek((int)AIConstants.Hexas.BarrierChangePointerTableLocation);
                this.romWriter.Seek((int)AIConstants.Hexas.BarrierChangePointerTablePatchLocation);
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Luna
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Undine
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Gnome
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Salamando
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Sylphid
                this.romWriter.WriteUInt16(scriptPointer);               // Shade

                // The attack state routine for Shade.
                byte atkH = (byte)((attackPointerTableOffset >> 8) & 0xFF);
                byte atkL = (byte)(attackPointerTableOffset & 0xFF);
                ReadOnlySpan<byte> shadeAttackRoutine = stackalloc byte[]
                {
                    //[Hexas_ShadeElementalAttack]
                    0xA9, 0x02, 0x00,       // LDA #$0002      ;Load 0x0002 into Accumulator.
                    0x20, 0x0B, 0x30,       // JSR $300B       ;Jump to GetBossRandomNumberInRange.
                    0x0A,                   // ASL A           ;Multiply By 2.
                    0xAA,                   // TAX             ;Transfer the index to Register X.
                    0xBF, atkL, atkH, 0xC2, // LDA $C2????,X   ;Load an attack pointer from PointerTable_HexasShadeBarrierAbility.
                    0x18,                   // CLC             ;Clear Carry Flag.
                    0x60,                   // RTS             ;Return.
                };
                this.romWriter.Seek((int)AIConstants.Hexas.ShadeAttackRoutineLocation);
                this.romWriter.WriteBytes(shadeAttackRoutine);

                // Like the Barrier Change pointer table above, the element attack state pointers have to be relocated to add Shade.
                ushort elementStatePointerTableOffset = (ushort)(this.romWriter.Position & 0xFFFF);
                ushort shadeStateOffset = (ushort)(AIConstants.Hexas.ShadeAttackRoutineLocation & 0xFFFF);
                this.romReader.Seek((int)AIConstants.Hexas.ElementStatePointerTableLocation);
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Luna
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Undine
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Gnome
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Salamando
                this.romWriter.WriteUInt16(this.romReader.ReadUInt16()); // Sylphid
                this.romWriter.WriteUInt16(shadeStateOffset);            // Shade

                // Finally, patch the state machine to load from the new address.
                this.romWriter.Seek((int)AIConstants.Hexas.ElementStateJumpPatchLocation);
                this.romWriter.WriteUInt16(elementStatePointerTableOffset);

                // For reasons, Shade's palette index has to be 0x7D. This index is used by Thunder Gigas.
                // Thankfully Thunder Gigas doesn't do any special palette tricks, so changing the index he uses his simple.
                // For Shade's palette we'll use the palette 0x81, which is the 4th palette in the slime crystal palette cycle.
                // This give the tail a nice black and blue color.
                // A runner up is palette 0x60, which belongs to Dread Slime, this will make the tail black and red instead.
                ushort thunderGigasPaletteOffset = this.palettePointerTable[0x7D];
                ushort shadePaletteOffset = this.palettePointerTable[0x81];

                this.romWriter.Seek((int)this.PalettePointerTableAddress + (sizeof(ushort) * 0x7D));
                this.romWriter.WriteUInt16(shadePaletteOffset);

                // Palette Index 0x26 is one of several unused indexes that points to Aegagropilon's palette.
                this.romWriter.Seek((int)this.PalettePointerTableAddress + (sizeof(ushort) * 0x26));
                this.romWriter.WriteUInt16(thunderGigasPaletteOffset);

                // Update Thunder Gigas's loader script to use palette 0x26 instead of 0x7D.
                this.romWriter.Seek((int)AIConstants.ThunderGigas.PalettePatchLocation);
                this.romWriter.Write(0x26);
            }
        }

        private void WriteMechRiderAllUpgrades(BossAIUpgradeOptions options)
        {
            if (options.MechRiderAllOptions.AILocksOnDamageFix)
            {
                // Mech Rider's damage/death command attempts to call a routine to lock his AI state until the sequence is complete.
                // However the wrong op code is used resulting in him attempting to freeze in his current action for 13,497 ticks (67,485 frames).
                // He somehow gets out of this situation, I believe its due to how the frame delay in the animation script is handled.
                // Patching in the correct op code allows it to function correctly.
                this.romWriter.Seek((int)AIConstants.MechRider.DeathCommandPatchLocation);
                this.romWriter.Write((byte)BossAICommandActionType.SetCommandLockRoutine);
            }

            if (options.MechRiderAllOptions.TargetAlignmentDoesntLockAI)
            {
                // This isn't a bug, but a questionable design decision.
                // Mech Rider primarily attacks via running you over with his bike, so his primary objective is to align himself with his target.
                // His AI will lock and prevent him from doing anything until the alignment requirement is met.
                // This is why he can be tricked into doing nothing in the second fight. 
                // This patch will always unlock the state machine while letting the lock code still run.
                // So Mech Rider will try to align until its time for him to attack, and if he can't drive over his target then
                // he will cast spells or fire his cannon instead.
                this.romWriter.Seek((int)AIConstants.MechRider.MovementLockPatchLocation);
                this.romWriter.Write(0x38); // SEC
            }

            if (options.MechRiderAllOptions.DoubleVerticalMovement)
            {
                // Default values: 2, -2
                this.romWriter.Seek((int)AIConstants.MechRider.MovementLockPositiveSpeedPatchLocation);
                this.romWriter.Write(0x04);
                this.romWriter.Seek((int)AIConstants.MechRider.MovementLockNegativeSpeedPatchLocation);
                this.romWriter.Write(0xFC);
            }
        }

        private void WriteMechRiderIUpgrades(BossAIUpgradeOptions options)
        {
            EnemyStatEntry statEntry = this.romReader.ReadStatEntry(AIConstants.MechRider.MechRiderISpriteIndex);

            if (options.MechRiderIOptions.IncreasedStatistics)
            {
                statEntry.Agility = 0x15;
            }

            this.romWriter.WriteStatEntry(statEntry, AIConstants.MechRider.MechRiderISpriteIndex);
        }

        private void WriteMechRiderIIUpgrades(BossAIUpgradeOptions options)
        {
            EnemyStatEntry statEntry = this.romReader.ReadStatEntry(AIConstants.MechRider.MechRiderIISpriteIndex);

            if (options.MechRiderIIOptions.IncreasedStatistics)
            {
                // Mech Rider II's stats are weird. They are mostly lower then Mech Rider I.
                // His black magic power is insanely high, but he has no black magic spells he can cast.
                statEntry.Strength = 0x25;
                statEntry.Agility = 0x25;
                statEntry.Defense = 0x0030;
                statEntry.MagicDefense = 0x0080;
                statEntry.WhiteMagicPower = 0x25;
            }

            this.romWriter.WriteStatEntry(statEntry, AIConstants.MechRider.MechRiderIISpriteIndex);
        }

        private void WriteMechRiderIIIUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.MechRider);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);
            EnemyStatEntry statEntry = this.romReader.ReadStatEntry(AIConstants.MechRider.MechRiderIIISpriteIndex);

            if (options.MechRiderIIIOptions.SpellAndSkillUseFix)
            {
                // Mech Rider III's AI looks like this:
                // If Mech Rider doesn't have Wall Then cast Wall.
                // Else If Mech Rider has no buffs Then cast Speed Up.
                // Else Use Wave/Diffuser Cannon.
                // So basically once he has Wall he'll start buffing the PCs and never firing his cannons.
                // New AI looks like this:
                // If Mech Rider has Wall Then Use Wave/Diffuser Cannon
                // Else If Mech Rider has no buffs then Cast Speed Up.
                // Else cast Wall.
                ReadOnlySpan<byte> spellAndSkillPatch = stackalloc byte[]
                {
                    //[MechRiderIII_CastSpellOrUseSkill]
                    0xBD, 0xB1, 0x01, // LDA $01B1,X     ;Load [MISC_FLAGS, X] into Accumulator.
                    0x89, 0x40, 0x00, // BIT #$0040      ;Test to see if Mech Rider III has 'Wall' status.
                    0xD0, 0x12,       // BNE $12         ;Branch to MechRiderIII_UseSkill if Mech Rider III has Wall status.
                    0xBD, 0xB0, 0x01, // LDA $01B0,X     ;Load [BUFF_EFFECTS, X] into Accumulator.
                    0x29, 0xFF, 0x00, // AND #$00FF      ;Drop the high byte.
                    0xD0, 0x05,       // BNE $05         ;Branch to MechRiderIII_CastWall if Mech Rider III has any buff effect.
                    0xA9, 0x99, 0xEB, // LDA #$EB99      ;Load 0xEB99 into Accumulator. (Cast Spell: Speed Up. Target: Self)
                    0x80, 0x03,       // BRA $03         ;Branch to MechRiderIII_AbilityExit.

                    //[MechRiderIII_CastWall]
                    0xA9, 0x9B, 0xEB, // LDA #$EB9B      ;Load 0xEB9B into Accumulator. (Cast Spell: Wall. Target: Self)

                    //[MechRiderIII_AbilityExit]
                    0x18,             // CLC             ;Clear Carry Flag.
                    0x60,             // RTS             ;Return.
                };
                this.romWriter.Seek((int)AIConstants.MechRider.MechRiderIIISpellPatchLocation);
                this.romWriter.WriteBytes(spellAndSkillPatch);
            }

            if (options.MechRiderIIIOptions.DiffuserCannonTargetingFix)
            {
                // Diffuser Cannon is supposted to hit all PCs, however the actions in the command are out of order, so the target gets set to late.
                // Diffuser Cannon will instead hit whoever his current target is.
                // There is a secondary problem of the command not waiting for long enough so the palette gets swapped out to soon.
                BossAICommand diffuserCannon = commandSet.Commands[AIConstants.MechRider.Commands.DiffuserCannon];
                diffuserCannon.Move(0x00, 0x02);
                diffuserCannon.Replace(0x04, new BossAIFreezeAnimationAction(0x0036));

                ushort pointer = commandSet.PointerTable[AIConstants.MechRider.Commands.DiffuserCannon];
                this.romWriter.Seek(Constants.Bank02Offset | pointer);
                this.romWriter.WriteAICommand(diffuserCannon);
            }

            if (options.MechRiderIIIOptions.IncreasedDiffuserCannonDamage)
            {
                ManaBossWeapon weapon = this.romReader.ReadWeapon(AIConstants.MechRider.DiffuserCannonWeaponIndex);
                weapon.Power = 0x6D;
                this.romWriter.WriteWeapon(weapon, AIConstants.MechRider.DiffuserCannonWeaponIndex);
            }

            if (options.MechRiderIIIOptions.IncreasedStatistics)
            {
                statEntry.Agility = 0x35;
                statEntry.WhiteMagicPower = 0x30;
                statEntry.MagicLevel = 0x06;
            }

            this.romWriter.WriteStatEntry(statEntry, AIConstants.MechRider.MechRiderIIISpriteIndex);
        }

        private void WriteMetalMantisUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Ant);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.MetalMantisOptions.UsefulWeapons)
            {
                // Metal Mantis uses the same weapons and commands for melee and projectile attacks as Mantis Ant.
                // These weapons specifically:
                // D0/BF9D:	00 00 63 07 0000 32		[44: MantisAnt_Kama]
                // D0/C076:	00 00 63 0A 1000 63		[63: MantisAnt_ThrowingKama]
                // Considering by the time you fight him your not an unarmored boy with a rusty sword you found in the creek his damage is rather pathetic.
                //
                // To beef him up we'll start with two unused boss weapons, one of which is already a duplicate of Mantis Ant's melee attack.
                // D0/BFA4:	00 00 63 07 0000 32		[45: Duplicate of MantisAnt_Kama]
                // D0/C08B:	00 00 63 51 0000 32		[66: Duplicate of 65]

                // Most of the stats of the new melee weapon will remain the same, but with increased power.
                ManaBossWeapon meleeWeapon = this.romReader.ReadWeapon(AIConstants.MetalMantis.NewMeleeWeaponIndex);
                ManaBossWeapon projectileWeapon = this.romReader.ReadWeapon(AIConstants.MetalMantis.NewProjectileWeaponIndex);

                meleeWeapon.Power = 0x43;
                meleeWeapon.StatusEffects = StatusEffects.KnockedOut;

                projectileWeapon.Power = 0x47;
                projectileWeapon.StatusEffects = StatusEffects.KnockedOut;
                projectileWeapon.InflictionRate = 0x63;

                this.romWriter.WriteWeapon(meleeWeapon, AIConstants.MetalMantis.NewMeleeWeaponIndex);
                this.romWriter.WriteWeapon(projectileWeapon, AIConstants.MetalMantis.NewProjectileWeaponIndex);

                // Now that there are weapons, Metal Mantis needs AI commands that use them. There are 3 melee attacks and 1 projectile attack.
                // Commands 18-1B are dummied commands that do nothing, so we'll be replacing them with Metal Mantis's new commands.
                BossAICommand doubleKamaSwing = new BossAICommand();
                doubleKamaSwing.Add(new BossAIClearHorizontalFlipAction()); // Clear Horizontal Flip.
                doubleKamaSwing.Add(new BossAIPlayAnimationAction(0x000F)); // Play Animation: BossAnimationScript_Ant_DoubleKamaSwing.
                doubleKamaSwing.Add(new BossAIEquipWeaponAction(0x0045));   // Equip Boss Weapon: MetalMantis_Kama.
                doubleKamaSwing.Add(new BossAIEndCommandAction());          // End Command Subset.

                BossAICommand rightKamaSwing = new BossAICommand();
                rightKamaSwing.Add(new BossAISetHorizontalFlipAction()); // Set Horizontal Flip.

                BossAICommand leftKamaSwing = new BossAICommand();
                leftKamaSwing.Add(new BossAIPlayAnimationAction(0x000F)); // Play Animation: BossAnimationScript_Ant_DoubleKamaSwing.
                leftKamaSwing.Add(new BossAIEquipWeaponAction(0x0045));   // Equip Boss Weapon: MetalMantis_Kama.
                leftKamaSwing.Add(new BossAIEndCommandAction());          // End Command Subset.

                BossAICommand kamaThrow = new BossAICommand();
                kamaThrow.Add(new BossAICallRoutineAction(0x3E7F));     // Call Routine: CreateKamaProjectile.
                kamaThrow.Add(new BossAIFreezeAnimationAction(0x0003)); // Freeze Animation for 0x0003 Frames.
                kamaThrow.Add(new BossAIEquipWeaponAction(0x0066));     // Equip Boss Weapon: MetalMantis_ThrowingKama.
                kamaThrow.Add(new BossAIEndCommandAction());            // End Command Subset.

                // Write out the new attack commands and remember their pointers.
                Span<ushort> commandPointers = stackalloc ushort[4];
                this.romWriter.Seek((int)AIConstants.MetalMantis.CommandLocation);
                commandPointers[0] = (ushort)(this.romWriter.Position & 0xFFFF);
                commandPointers[1] = this.romWriter.WriteAICommand(doubleKamaSwing);
                commandPointers[2] = this.romWriter.WriteAICommand(rightKamaSwing);
                commandPointers[3] = this.romWriter.WriteAICommand(leftKamaSwing);
                this.romWriter.WriteAICommand(kamaThrow);

                // Update the pointer table with the new attack commands.
                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * 0x18));
                this.romWriter.WriteUInt16(commandPointers[0]);
                this.romWriter.WriteUInt16(commandPointers[1]);
                this.romWriter.WriteUInt16(commandPointers[2]);
                this.romWriter.WriteUInt16(commandPointers[3]);

                // Next we need AI Scripts to run the new commands.
                ReadOnlySpan<byte> aiScripts = stackalloc byte[]
                {
                    0x18, 0xFF,       //         ;Attack: Metal Mantis Double Kama Swing.
                    0x19, 0xFF,       //         ;Attack: Metal Mantis Right Kama Swing.
                    0x1A, 0xFF,       //         ;Attack: Metal Mantis Left Kama Swing.
                    0x18, 0x1B, 0xFF, //         ;Attack: Metal Mantis Kama Throw.
                };

                this.romWriter.Seek((int)AIConstants.MetalMantis.WeaponAIScriptLocation);
                this.romWriter.WriteBytes(aiScripts);

                // Next Step: Create a new attack pointer table for Metal Mantis to use.
                this.romWriter.Seek((int)AIConstants.MetalMantis.AttackCommandPointerTableLocation);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.RightKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.RightKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.DoubleKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.LeftKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.LeftKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.DoubleKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.DoubleKamaSwingCommandOffset);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.DoubleKamaSwingCommandOffset);

                // Finally update Metal Mantis's attack AI routine to use the new attack scripts.
                this.romWriter.Seek((int)AIConstants.MetalMantis.KamaThrowPatchLocation);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.KamaThrowCommandOffset);

                this.romWriter.Seek((int)AIConstants.MetalMantis.MeleePatchLocation);
                this.romWriter.WriteUInt24(AIConstants.MetalMantis.AttackCommandPointerTableLocation);
            }

            if (options.MetalMantisOptions.IncreasedFireBeamDamage)
            {
                // Metal Mantis's Fire Beam ability has a comically weak power of 11.
                // D0/BE0E:	00 00 63 0B 0040 63		[0B: Skill_FireBeam] (Metal Mantis)
                ManaBossWeapon weapon = this.romReader.ReadWeapon(AIConstants.MetalMantis.FireBeamWeaponIndex);
                weapon.Power = 0x45;
                this.romWriter.WriteWeapon(weapon, AIConstants.MetalMantis.FireBeamWeaponIndex);
            }

            if (options.MetalMantisOptions.HasImprovedAcidBreathCommand)
            {
                // Metal Mantis has a command for Flash Beam in his AI set.
                // It is unused because his attack roulette table is filled with two instances of Fire Beam instead.
                // Re-enabling Flash Beam is less interesting since he already has a beam attack,
                // so instead we will replace that command with a new one that uses Acid Breath instead.

                // We need to repurpose another unused boss weapon for this.
                // D0/C084:	00 00 63 51 0000 32		[65: ]
                ManaBossWeapon weapon = this.romReader.ReadWeapon(AIConstants.MetalMantis.AcidBreathWeaponIndex);
                weapon.Power = 0x45;
                weapon.StatusEffects = StatusEffects.Poison;
                weapon.InflictionRate = 0x4B;
                this.romWriter.WriteWeapon(weapon, AIConstants.MetalMantis.AcidBreathWeaponIndex);

                BossAICommand acidBreath = new BossAICommand();
                acidBreath.Add(new BossAIPlayAnimationAction(0x000C));        // Play Animation: BossAnimationScript_Ant_SkillUse.
                acidBreath.Add(new BossAIPlaySkillCommand(0x03, 0x00, 0xE0)); // Play Skill Animation: Acid Breath, X-Coordinate: 0x00, Y-Coordinate: 0xE0.
                acidBreath.Add(new BossAIEquipWeaponAction(0x0065));          // Equip Boss Weapon: Skill_MantisAnt_AcidBreath.
                acidBreath.Add(new BossAIFreezeAnimationAction(0x0024));      // Freeze Animation for 0x0024 Ticks.
                acidBreath.Add(new BossAIEndCommandAction());                 // End Command Subset.

                this.romWriter.Seek((int)AIConstants.MetalMantis.AcidBreathCommandLocation);
                this.romWriter.WriteAICommand(acidBreath);

                // This table has two entries, both of which are Fire Beam.
                // Replace one of the Fire Beam pointers with the new pointer to Acid Breath. Giving a 50% chance of either skill.
                this.romWriter.Seek((int)AIConstants.MetalMantis.SkillUseAIRoulettePointerTable);
                this.romWriter.WriteUInt16(AIConstants.MetalMantis.AcidBreathCommandScriptOffset);
            }

            this.romWriter.WriteSetupHeader(BossFamily.Ant, header);
        }

        private void WriteRedDragonUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Dragon);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.RedDragonOptions.IncreasedAttackRange)
            {
                this.romWriter.Seek(AIConstants.RedDragon.AttackRangePatchLocation);
                this.romWriter.Write(0x0C); // 192 total pixels, up from 64.
            }

            if (options.RedDragonOptions.ImprovedSleepRing)
            {
                ManaBossWeapon sleepRingWeapon = this.romReader.ReadWeapon(Constants.BossWeapons.SkillSleepRing2);
                sleepRingWeapon.Power = 0x60;
                sleepRingWeapon.InflictionRate = 0x63;
                this.romWriter.WriteWeapon(sleepRingWeapon, Constants.BossWeapons.SkillSleepRing2);
            }
        }

        private void WriteSnowDragonUpgrades(BossAIUpgradeOptions options)
        {
            BossSetupHeader header = this.romReader.ReadSetupHeader(BossFamily.Dragon);
            BossAICommandSet commandSet = this.romReader.ReadAICommandSet(header.AICommandSetPointer);

            if (options.SnowDragonOptions.NewFrostWingSkill)
            {
#if DEBUG
                // Replace Snow Dragons attack roulette table with pointers to the new skill.
                this.romWriter.Seek(0x1CE5C5);
                this.romWriter.WriteUInt16((ushort)(AIConstants.SnowDragon.FrostWingCommandScriptLocation & 0xFFFF));
                this.romWriter.WriteUInt16((ushort)(AIConstants.SnowDragon.FrostWingCommandScriptLocation & 0xFFFF));
                this.romWriter.WriteUInt16((ushort)(AIConstants.SnowDragon.FrostWingCommandScriptLocation & 0xFFFF));
#endif

                // The dragon tileset contains graphics for a special attack for each dragon.
                // Icicles for Snow Dragon.
                // Fireballs for Red Dragon.
                // Gas clouds for Blue Dragon.
                // Of these, only the gas clouds were used for the skill 'Breath Wing' and all three dragons use it.
                // The fireballs aren't super useful as they only contain southward facing graphics.
                // The icicles however contain a full set of graphics of it progressively breaking.
                // Skill Index 18 contains a copy of the skill 'Moogle Glare' that is unused, so we'll be replacing that with the new skill.

                // No frame or animation data exists for these graphics so they have to be created from scratch.
                WriteFrostWingAnimationFrameData();
                WriteFrostWingBuilderTableIndexes();
                WriteFrostWingCommandData();

                // 'Frost Wing' is less characters then 'Moogle Glare', so the string can be replaced at the current location.
                this.romWriter.Seek(AIConstants.SnowDragon.FrostWingNameLocation);
                this.romWriter.WriteBytes(this.romFile.Encoding.GetBytes("Frost Wing"));
                this.romWriter.Write(0x00);

                // The weapon being replaced inflicts both Moogle and Pygmy, which while comical, is not appropriate for this skill.
                ManaBossWeapon frostWingWeapon = this.romReader.ReadWeapon(AIConstants.SnowDragon.FrostWingSkillIndex);
                frostWingWeapon.Power = 0x7D;
                frostWingWeapon.StatusEffects = StatusEffects.Frosty | StatusEffects.Confusion;
                frostWingWeapon.InflictionRate = 0x4B;
                this.romWriter.WriteWeapon(frostWingWeapon, AIConstants.SnowDragon.FrostWingSkillIndex);

                // Make the dragon's tail attack when the skill is used.
                this.romWriter.Seek(Constants.Bank02.DragonFamilyTailAnimationIdSyncTable + (sizeof(ushort) * AIConstants.SnowDragon.Commands.NewFrostWingIndex));
                this.romWriter.WriteUInt16(Constants.BossAnimations.DragonTailAttack);
            }

            if (options.SnowDragonOptions.IncreasedAttackRange)
            {
                this.romWriter.Seek(AIConstants.SnowDragon.AttackRangePatchLocation);
                this.romWriter.Write(0x0C); // 192 total pixels, up from 96.
            }

            void WriteFrostWingAnimationFrameData()
            {
                // 66 - Full Icicle Top
                // 68 - Full Icicle Bottom
                // 6A - Partial Break Icicle Top
                // 6C - Partial Break Icicle Bottom
                // 6E - Broken Icicle
                // 80 = Small Down Fireball
                // 82 = Medium Down Fireball
                // 84 = Large Down Fireball

                // Full Icicle - Max Height
                List<BossSpriteFramePart> partList = new List<BossSpriteFramePart>(2);
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xD0), 0x66, BossFrameFlags.UsePalette3));
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xE0), 0x68, BossFrameFlags.UsePalette3));
                BossSpriteFrame frame1 = new BossSpriteFrame(0, "Frost Wing (Player 01)", (int)(this.BossAIAuxiliaryBankOffset | AIConstants.SnowDragon.FrostWingFrame1Offset), partList, BossHitBox.NullHitBox, BossHitBox.NullWeaponBox, BossHitBox.NullGuardBox);

                // Full Icicle - Falling
                partList = new List<BossSpriteFramePart>(2);
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xD4), 0x66, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xE4), 0x68, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                BossSpriteFrame frame2 = new BossSpriteFrame(1, "Frost Wing (Player 02)", (int)(this.BossAIAuxiliaryBankOffset | AIConstants.SnowDragon.FrostWingFrame2Offset), partList, BossHitBox.NullHitBox, BossHitBox.NullWeaponBox, BossHitBox.NullGuardBox);

                // Partial Broken Icicle - Max Height
                partList = new List<BossSpriteFramePart>(2);
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xD6), 0x6A, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xE6), 0x6C, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                BossSpriteFrame frame3 = new BossSpriteFrame(2, "Frost Wing (Player 03)", (int)(this.BossAIAuxiliaryBankOffset | AIConstants.SnowDragon.FrostWingFrame3Offset), partList, BossHitBox.NullHitBox, BossHitBox.NullWeaponBox, BossHitBox.NullGuardBox);

                // Partial Broken Icicle - Falling
                partList = new List<BossSpriteFramePart>(2);
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xD8), 0x6A, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xE8), 0x6C, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                BossSpriteFrame frame4 = new BossSpriteFrame(3, "Frost Wing (Player 04)", (int)(this.BossAIAuxiliaryBankOffset | AIConstants.SnowDragon.FrostWingFrame4Offset), partList, BossHitBox.NullHitBox, BossHitBox.NullWeaponBox, BossHitBox.NullGuardBox);

                // Broken Icicle
                partList = new List<BossSpriteFramePart>(2);
                partList.Add(new BossSpriteFramePart(unchecked((sbyte)0xF5), unchecked((sbyte)0xE8), 0x6E, BossFrameFlags.UsePalette3 | BossFrameFlags.Unknown08));
                BossSpriteFrame frame5 = new BossSpriteFrame(4, "Frost Wing (Player 05)", (int)(this.BossAIAuxiliaryBankOffset | AIConstants.SnowDragon.FrostWingFrame5Offset), partList, BossHitBox.NullHitBox, BossHitBox.NullWeaponBox, BossHitBox.NullGuardBox);

                this.romWriter.WriteFrame(frame1);
                this.romWriter.WriteFrame(frame2);
                this.romWriter.WriteFrame(frame3);
                this.romWriter.WriteFrame(frame4);
                this.romWriter.WriteFrame(frame5);

                // Create the animation from the frames.
                BossAnimationScript script = new BossAnimationScript();
                script.Add(new SetFrameBankBossAnimationCommand((byte)(this.BossAIAuxiliaryBank | 0xC0)));
                script.Add(new LoadFrameBossAnimationCommand((ushort)(frame1.Address & 0xFFFF), 0x01));
                script.Add(new LoadFrameBossAnimationCommand((ushort)(frame2.Address & 0xFFFF), 0x01));
                script.Add(new LoadFrameBossAnimationCommand((ushort)(frame3.Address & 0xFFFF), 0x01));
                script.Add(new LoadFrameBossAnimationCommand((ushort)(frame4.Address & 0xFFFF), 0x01));
                script.Add(new LoadFrameBossAnimationCommand((ushort)(frame5.Address & 0xFFFF), 0x01));
                script.Add(new LoadFrameBossAnimationCommand((ushort)(frame5.Address & 0xFFFF), 0x00));

                this.romWriter.Seek(AIConstants.SnowDragon.FrostWingAnimationScriptLocation);
                this.romWriter.WriteAnimationScript(script);
            }
            void WriteFrostWingBuilderTableIndexes()
            {
                // Skill AI
                this.romWriter.Seek(Constants.Bank1C.BossSkillAIPointerTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingSkillIndex));
                this.romWriter.WriteUInt16(0x99AD);

                // Palette ID
                this.romWriter.Seek(Constants.Bank1C.BossSkillPaletteIdTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingSkillIndex));
                this.romWriter.WriteUInt16(0x0077);

                // Boss Animation ID
                this.romWriter.Seek(Constants.Bank1C.BossSkillBossAnimationIdTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingSkillIndex));
                this.romWriter.WriteUInt16(0x0000);

                // Player Animation ID
                this.romWriter.Seek(Constants.Bank1C.BossSkillPlayerAnimationIdTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingSkillIndex));
                this.romWriter.WriteUInt16(AIConstants.SnowDragon.FrostWingAnimationIndex);

                // Boss Sound Effect
                this.romWriter.Seek(Constants.Bank1C.BossSkillBossSoundEffectIdTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingSkillIndex));
                this.romWriter.WriteUInt16(Constants.SoundEffects.None);

                // Player Sound Effect
                this.romWriter.Seek(Constants.Bank1C.BossSkillPlayerSoundEffectIdTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingSkillIndex));
                this.romWriter.WriteUInt16(Constants.SoundEffects.BossSkillFreezeBreathPlayer);

                // The new animation needs to be indexed in the boss animation table.
                // 0x159 is an unused duplicate pointer to one of the dragon body animations.
                this.romWriter.Seek(Constants.Bank1B.BossAnimationPointerTableAddress + (sizeof(ushort) * AIConstants.SnowDragon.FrostWingAnimationIndex));
                this.romWriter.WriteUInt16((ushort)(AIConstants.SnowDragon.FrostWingAnimationScriptLocation & 0xFFFF));
            }
            void WriteFrostWingCommandData()
            {
                // Write the command script for the Frost Wing skill.
                this.romWriter.Seek(AIConstants.SnowDragon.FrostWingCommandScriptLocation);
                this.romWriter.Write(AIConstants.SnowDragon.Commands.NewFrostWingIndex);
                this.romWriter.Write(0xFF);

                // Dragon bosses have several unused commands that have valid indexes in their child sync tables.
                // This means we can repurpose one of those commands without having to extend the body and tail animation sync tables.
                // 0x14 is a dummied command that points to Breath Wing and has sync values matching Breath Wing.
                BossAICommand frostWingCommand = commandSet.Commands[AIConstants.SnowDragon.Commands.NewFrostWingIndex];
                frostWingCommand.Replace(0, new BossAIPlayAnimationAction(0x156));
                frostWingCommand.Replace(3, new BossAIEquipWeaponAction(AIConstants.SnowDragon.FrostWingSkillIndex));
                frostWingCommand.Replace(5, new BossAIPlaySkillCommand(AIConstants.SnowDragon.FrostWingSkillIndex, 0x00, 0x00));

                // Capture the command offset and write out the command.
                ushort frostWingCommandOffset = (ushort)(this.romWriter.Position & 0xFFFF);
                this.romWriter.WriteAICommand(frostWingCommand);

                // Write a new AI roulette table for the previously shared common attack table.
                int snowDragonAttackRouletteTableAddress = this.romWriter.Position;
                this.romWriter.WriteUInt16((ushort)(AIConstants.SnowDragon.FrostWingCommandScriptLocation & 0xFFFF));
                this.romWriter.WriteUInt16(0xF054); // Attack: Tail Chain

                // Update the command pointer to point to the new Frost Wing command.
                this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.SnowDragon.Commands.NewFrostWingIndex));
                this.romWriter.WriteUInt16(frostWingCommandOffset);

                // Update Snow Dragon's AI to use the new AI Roulette Table.
                this.romWriter.Seek(AIConstants.SnowDragon.FrostWingAttackRoutinePatchLocation);
                this.romWriter.WriteUInt24((uint)(snowDragonAttackRouletteTableAddress | 0xC00000));
            }
        }

        private void WriteTropicalloUpgrades(BossAIUpgradeOptions options)
        {
            if (options.TropicalloOptions.InfiniteBramblerSpawns)
            {
                // For infinite respawns we just need to change the BNE to BRA to skip the count check.
                this.romWriter.Seek((int)AIConstants.Tropicallo.TropicalloInfiniteBramblerPatchLocation);
                this.romWriter.Write(0x80);
            }
            else
            {
                // Not doing infinite respawns, write the total number of Bramblers Tropicallo is allowed into the CMP instruction. (Vanilla: 2)
                this.romWriter.Seek((int)AIConstants.Tropicallo.TropicalloBramblerCountPatchLocation);
                this.romWriter.WriteUInt16(options.TropicalloOptions.MaxBramblerSpawns);
            }
        }

        private static class AIConstants
        {
            public const ushort PaletteTablePointerTableAddressOffset = 0x0000;
            public const ushort PaletteTableAddressOffset = 0x0110;

            public const uint SetSuperMagicRateRoutineLocation = Constants.Bank02Offset | 0xFF00;
            public const ushort GenericCoordinateUpdateRoutineOffset = 0x36BC;

            public static class Aegagropilon
            {
                public const uint DevourDirectionPatchLocation = Constants.Bank02Offset | 0x51ED;
                public const uint DevourDistancePatchLocation = Constants.Bank02Offset | 0x51F6;
                public const uint BallFormAttackPatchLocation = Constants.Bank02Offset | 0xDAE2;

                public static class Commands
                {
                    public const byte LeggedFormTransformBegin = 0x0F;
                    public const byte EatTarget = 0x14;
                }
            }

            public static class BlueDragon
            {
                public const uint AttackRangePatchLocation = Constants.Bank02Offset | 0x8CBD;
            }

            public static class Brambler
            {
                public const uint OutOfBoundsFixLocation = Constants.Bank02Offset | 0x45D4;
                public const uint WeaponPatchJumpLocation = Constants.Bank02Offset | 0x4B74;
                public const uint WeaponPatchLocation = Constants.Bank02Offset | 0xF37C;
            }

            public static class DarkLich
            {
                public const uint UndergroundHeadAttackRoutinePatchLocation = Constants.Bank02Offset | 0x6EB1;
                public const uint InitializationRoutinePatchLocation = Constants.Bank02Offset | 0x6DF7;
                public const uint LoadBodyAnimationIdPatchLocation = Constants.Bank02Offset | 0x7009;
                public const uint UndergroundHeadMoveScriptsLocation = Constants.Bank02Offset | 0xF3A0;
                public const uint UndergroundHeadMoveCommandLocation = Constants.Bank02Offset | 0xF3B0;
                public const uint SetSuperMagicRateRoutineLocation = Constants.Bank02Offset | 0xF420;
                public const ushort NewBodyAnimationSyncTableOffset = 0x4400;
                public const ushort LichBodyInvisibleAnimationIndex = 0x0133;
                public const byte MovementCommandIndex = 0x28;
            }

            public static class DeathMachine
            {
                public const uint FrameDataBlockStart = Constants.Bank0AOffset | 0x8E10;
                public const uint FrameDataBlockEnd = Constants.Bank0AOffset | 0x9638;
                public const byte SpriteIndex = 0x70;
                public const byte DrillSpinAnimationIndex = 0x3F;
                public const byte DrillInGroundAnimationIndex = 0x40;
                public const byte AnimationScriptStartIndex = 0x41;
                public const byte MaxValidCommandIndex = 0x33;
                public const byte FormChangeZCoordinate = 0x2A;
            }

            public static class DoomsWall
            {
                public const uint CaveInNamePatchLocation = Constants.Bank00Offset | 0x60DE;
            }

            public static class Dragon
            {
                public const uint DragonSkillCommandsPatchLocation = Constants.Bank02Offset | 0xF460;

                public static class Commands
                {
                    public const byte FreezeBreathIndex = 0x04;
                    public const byte FireBreathIndex = 0x06;
                    public const byte BlitzBreathIndex = 0x08;
                    public const byte BalloonRingIndex = 0x0A;
                    public const byte SleepRingIndex = 0x0B;
                    public const byte ConfuseHoopsIndex = 0x0C;
                }
            }

            public static class Hexas
            {
                public const uint FillSpaceStart = Constants.Bank02Offset | 0x6520;
                public const uint FillSpaceEnd = Constants.Bank02Offset | 0x657C;
                public const uint InitRoutinePatchLocation = Constants.Bank02Offset | 0x651F;
                public const uint MoveRoutinePatchLocation = Constants.Bank02Offset | 0xF300;
                public const uint AttackRoutinePatchLocation = Constants.Bank02Offset | 0x6576;
                public const uint DamagedRoutinePatchLocation = Constants.Bank02Offset | 0x6534; //0x6529
                public const uint ShadeAttackRoutineLocation = Constants.Bank02Offset | 0x6541; //0x6536
                public const uint ElementStateJumpPatchLocation = Constants.Bank02Offset | 0x6587;
                public const uint ElementStatePointerTableLocation = Constants.Bank02Offset | 0x6589;
                public const uint BarrierChangePointerTablePatchLocation = Constants.Bank02Offset | 0xF390;
                public const uint BarrierChangePointerTableLocation = Constants.Bank1COffset | 0xE324;
                public const uint LunaElementPointerTableLocation = Constants.Bank1COffset | 0xE32E;
                public const uint LunaAttackRngPatchLocation = Constants.Bank02Offset | 0x6594;
                public const uint LunaAttackLoadPointerPatchLocation = Constants.Bank02Offset | 0x659C;
                public const ushort BarrierChangeCommandOffset = 0xF200;
                public const ushort MoogleBubblesCommandOffset = 0xF2BC;
                public const byte MoogleBubblesWeaponIndex = 0x06;
                public const byte SpriteIndex = 0x6F;
            }

            public static class KettleKin
            {
                public const uint FillSpaceStart = Constants.Bank02Offset | 0xD54F;
                public const uint FillSpaceEnd = Constants.Bank02Offset | 0xD657;
                public const uint NamePatchLocation = Constants.Bank0AOffset | 0xFF70;
                public const uint AnimationScriptLocation = Constants.Bank01Offset | 0x7DEB;
                public const uint ChainsawGraphicsLoaderPatchLocation = Constants.Bank01Offset | 0x6245;
                public const uint FormChangeZCoordinatePatchLocation = Constants.Bank02Offset | 0x3FD2;
                public const uint DrillMoveCommandPatchLocation = Constants.Bank02Offset | 0xD611;
                public const byte SpriteIndex = 0x70;

                public static class Commands
                {
                    public const byte Idle = 0x00;
                    public const byte IdleBackView = 0x01;
                    public const byte IdleLeftSideView = 0x02;
                    public const byte IdleRightSideView = 0x03;
                    public const byte IdleV2 = 0x08;
                    public const byte IdleBackViewV2 = 0x09;
                    public const byte IdleLeftSideViewV2 = 0x0A;
                    public const byte IdleRightSideViewV2 = 0x0B;
                    public const byte WheelMoveSouth = 0x0C;
                    public const byte WheelMoveNorth = 0x0D;
                    public const byte WheelMoveWest = 0x0E;
                    public const byte WheelMoveEast = 0x0F;
                    public const byte IdleV3 = 0x10;
                    public const byte IdleBackViewV3 = 0x11;
                    public const byte IdleLeftSideViewV3 = 0x12;
                    public const byte IdleRightSideViewV3 = 0x13;
                    public const byte IdleV4 = 0x14;
                    public const byte IdleBackViewV4 = 0x15;
                    public const byte IdleLeftSideViewV4 = 0x16;
                    public const byte IdleRightSideViewV4 = 0x17;
                    public const byte HammerDown = 0x18;
                    public const byte HammerUp = 0x19;
                    public const byte HammerLeft = 0x1A;
                    public const byte HammerRight = 0x1B;
                    public const byte HammerDownDummied = 0x1C;
                    public const byte HammerUpDummied = 0x1D;
                    public const byte HammerLeftDummied = 0x1E;
                    public const byte HammerRightDummied = 0x1F;
                    public const byte HammerGoRound = 0x20;
                    public const byte LunarBoast = 0x21;
                    public const byte ShortCircuit = 0x22;
                    public const byte WheelShortCircuit = 0x23;
                    public const byte LucidBarrierDummied01 = 0x24;
                    public const byte LucidBarrierDummied02 = 0x25;
                    public const byte LucidBarrier = 0x26;
                    public const byte DoomBeam = 0x27;
                    public const byte WheelHammerDown = 0x28;
                    public const byte WheelHammerUp = 0x29;
                    public const byte WheelHammerLeft = 0x2A;
                    public const byte WheelHammerRight = 0x2B;
                    public const byte WheelHammerDownDummied = 0x2C;
                    public const byte WheelHammerUpDummied = 0x2D;
                    public const byte WheelHammerLeftDummied = 0x2E;
                    public const byte WheelHammerRightDummied = 0x2F;
                    public const byte WheelHammerGoRound = 0x30;
                    public const byte WheelLunarBoast = 0x31;
                    public const byte WheelLucidBarrier = 0x32;
                }
            }

            public static class MechRider
            {
                public const uint MechRiderIIISpellPatchLocation = Constants.Bank02Offset | 0x7227;
                public const uint MovementLockPositiveSpeedPatchLocation = Constants.Bank02Offset | 0x736F;
                public const uint MovementLockNegativeSpeedPatchLocation = Constants.Bank02Offset | 0x7377;
                public const uint MovementLockPatchLocation = Constants.Bank02Offset | 0x7384;
                public const uint DeathCommandPatchLocation = Constants.Bank02Offset | 0xEC9F;
                public const byte DiffuserCannonWeaponIndex = 0x1A;
                public const byte MechRiderISpriteIndex = 0x60;
                public const byte MechRiderIISpriteIndex = 0x64;
                public const byte MechRiderIIISpriteIndex = 0x72;

                public static class Commands
                {
                    public const byte DiffuserCannon = 0x0B;
                }
            }

            public static class MetalMantis
            {
                public const uint KamaThrowPatchLocation = Constants.Bank02Offset | 0x3DF4;
                public const uint MeleePatchLocation = Constants.Bank02Offset | 0x3E21;
                public const uint WeaponAIScriptLocation = Constants.Bank02Offset | 0xF19C;
                public const uint CommandLocation = Constants.Bank02Offset | 0xF1A5;
                public const uint AcidBreathCommandLocation = Constants.Bank02Offset | 0xD1D9;
                public const uint AttackCommandPointerTableLocation = Constants.Bank02Offset | 0xF1BF;
                public const uint SkillUseAIRoulettePointerTable = Constants.Bank1COffset | 0xE024;
                public const ushort AcidBreathCommandScriptOffset = 0xD0C1;
                public const ushort DoubleKamaSwingCommandOffset = 0xF19C;
                public const ushort RightKamaSwingCommandOffset = 0xF19E;
                public const ushort LeftKamaSwingCommandOffset = 0xF1A0;
                public const ushort KamaThrowCommandOffset = 0xF1A2;
                public const byte FireBeamWeaponIndex = 0x0B;
                public const byte NewMeleeWeaponIndex = 0x45;
                public const byte AcidBreathWeaponIndex = 0x65;
                public const byte NewProjectileWeaponIndex = 0x66;
            }

            public static class RedDragon
            {
                public const uint AttackRangePatchLocation = Constants.Bank02Offset | 0x8C75;
            }

            public static class SnowDragon
            {
                public const uint FrostWingNameLocation = Constants.Bank00Offset | 0x609C;
                public const uint FrostWingAnimationScriptLocation = Constants.Bank01Offset | 0x7EF0;
                public const uint FrostWingAttackRoutinePatchLocation = Constants.Bank02Offset | 0x8C54;
                public const uint FrostWingCommandScriptLocation = Constants.Bank02Offset | 0xF430;
                public const uint AttackRangePatchLocation = Constants.Bank02Offset | 0x8C3A;
                public const ushort FrostWingFrame1Offset = 0x1300;
                public const ushort FrostWingFrame2Offset = 0x130A;
                public const ushort FrostWingFrame3Offset = 0x1324;
                public const ushort FrostWingFrame4Offset = 0x1314;
                public const ushort FrostWingFrame5Offset = 0x131E;
                public const ushort FrostWingAnimationIndex = 0x0159;
                public const byte FrostWingSkillIndex = 0x18;

                public static class Commands
                {
                    public const byte NewFrostWingIndex = 0x14;
                }
            }

            public static class ThunderGigas
            {
                public const uint PalettePatchLocation = Constants.Bank01Offset | 0x6344;
            }

            public static class Tropicallo
            {
                public const uint TropicalloInfiniteBramblerPatchLocation = Constants.Bank02Offset | 0x459B;
                public const uint TropicalloBramblerCountPatchLocation = Constants.Bank02Offset | 0x45A1;
            }
        }
    }

    public sealed class BossAIUpgradeOptions
    {
        public AegagropilonAIOptions AegagropilonOptions { get; } = new AegagropilonAIOptions();
        public BlueDragonAIOptions BlueDragonOptions { get; } = new BlueDragonAIOptions();
        public BramblerAIOptions BramblerOptions { get; } = new BramblerAIOptions();
        public DarkLichAIOptions DarkLichOptions { get; } = new DarkLichAIOptions();
        public DoomsWallAIOptions DoomsWallOptions { get; } = new DoomsWallAIOptions();
        public DragonAllAIOptions DragonAllOptions { get; } = new DragonAllAIOptions();
        public HexasAIOptions HexasOptions { get; } = new HexasAIOptions();
        public KettleKinAIOptions KettleKinOptions { get; } = new KettleKinAIOptions();
        public MechRiderAllAIOptions MechRiderAllOptions { get; } = new MechRiderAllAIOptions();
        public MechRiderIAIOptions MechRiderIOptions { get; } = new MechRiderIAIOptions();
        public MechRiderIIAIOptions MechRiderIIOptions { get; } = new MechRiderIIAIOptions();
        public MechRiderIIIAIOptions MechRiderIIIOptions { get; } = new MechRiderIIIAIOptions();
        public MetalMantisAIOptions MetalMantisOptions { get; } = new MetalMantisAIOptions();
        public RedDragonAIOptions RedDragonOptions { get; } = new RedDragonAIOptions();
        public SnowDragonAIOptions SnowDragonOptions { get; } = new SnowDragonAIOptions();
        public TropicalloAIOptions TropicalloOptions { get; } = new TropicalloAIOptions();

        public sealed class AegagropilonAIOptions : AIOptions
        {
            public bool EnableDevourAttackFix { get; set; } = true;
            public bool ImprovedDevourTargeting { get; set; } = true;
            public bool AlternateDevourEatAnimation { get; set; } = true;
            public bool BallFormAlwaysAttacks { get; set; } = true;
        }

        public sealed class BlueDragonAIOptions : AIOptions
        {
            public bool IncreasedAttackRange { get; set; } = true;
        }

        public sealed class BramblerAIOptions : AIOptions
        {
            public bool OutOfBoundsAndCountFix { get; set; } = true;
            public bool WeaponFix { get; set; } = true;
        }

        public sealed class DarkLichAIOptions : AIOptions
        {
            public bool HatesHeavyMetalMusic { get; set; } = true;
            public bool UndergroundHorizontalMovement { get; set; } = true;
            public bool UseSuperMagic { get; set; } = true;
            public byte SuperMagicRate { get; set; } = 64;
        }

        public sealed class DoomsWallAIOptions : AIOptions
        {
            public bool CaveInNameFix { get; set; } = true;
        }

        public sealed class DragonAllAIOptions : AIOptions
        {
            public bool SkillTargetingFix { get; set; } = true;
        }

        public sealed class HexasAIOptions : AIOptions
        {
            public bool ImprovedBarrierChange { get; set; } = true;
            public bool BarrierChangeShade { get; set; } = true;
            public bool InfiniteMP { get; set; } = true;
            public bool LunaMoogleBubbles { get; set; } = true;
            public bool UseSuperMagic { get; set; } = true;
            public byte SuperMagicRate { get; set; } = 32;
            public bool SpawnElementFix { get; set; } = true;
        }

        public sealed class KettleKinAIOptions : AIOptions
        {
            public bool RestoreDeathMachine { get; set; } = true;
            public bool DrillSpinsDuringMovement { get; set; } = true;
        }

        public sealed class MechRiderAllAIOptions : AIOptions
        {
            public bool AILocksOnDamageFix { get; set; } = true;
            public bool TargetAlignmentDoesntLockAI { get; set; } = true;
            public bool DoubleVerticalMovement { get; set; } = true;
        }

        public sealed class MechRiderIAIOptions : AIOptions
        {
            public bool IncreasedStatistics { get; set; } = true;
        }

        public sealed class MechRiderIIAIOptions : AIOptions
        {
            public bool IncreasedStatistics { get; set; } = true;
        }

        public sealed class MechRiderIIIAIOptions : AIOptions
        {
            public bool SpellAndSkillUseFix { get; set; } = true;
            public bool DiffuserCannonTargetingFix { get; set; } = true;
            public bool IncreasedDiffuserCannonDamage { get; set; } = true;
            public bool IncreasedStatistics { get; set; } = true;
        }

        public sealed class MetalMantisAIOptions : AIOptions
        {
            public bool UsefulWeapons { get; set; } = true;
            public bool IncreasedFireBeamDamage { get; set; } = true;
            public bool HasImprovedAcidBreathCommand { get; set; } = true;
        }

        public sealed class RedDragonAIOptions : AIOptions
        {
            public bool IncreasedAttackRange { get; set; } = true;
            public bool ImprovedSleepRing { get; set; } = true;
        }

        public sealed class SnowDragonAIOptions : AIOptions
        {
            public bool NewFrostWingSkill { get; set; } = true;
            public bool IncreasedAttackRange { get; set; } = true;
        }

        public sealed class TropicalloAIOptions : AIOptions
        {
            public bool InfiniteBramblerSpawns { get; set; } = false;
            public ushort MaxBramblerSpawns { get; set; } = 0x0002;
        }

        public abstract class AIOptions
        {
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append('[').Append(this.GetType().Name).AppendLine("]");
                foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
                {
                    sb.Append(propertyInfo.Name).Append(": ").AppendLine(propertyInfo.GetValue(this).ToString());
                }
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsAssignableTo(typeof(AIOptions)))
                {
                    sb.AppendLine(propertyInfo.GetValue(this).ToString());//.AppendLine();
                }
            }
            return sb.ToString();
        }
    }

    internal sealed class BossAIRomReader : RomReader
    {
        private readonly BossAICommandParser commandParser;
        private readonly BossAnimationParser animationParser;

        public BossAIRomReader(RomFile rom) : base(rom)
        {
            this.commandParser = new BossAICommandParser(this);
            this.animationParser = new BossAnimationParser(this);
        }

        public BossSetupHeader ReadSetupHeader(BossFamily family)
        {
            this.Seek((int)BossReaderWriterUtil.GetBossSetupHeaderAddress(family));

            //BossSetupHeader header = new BossSetupHeader();
            //header.ControlFlags = (BossControlFlags)this.ReadUInt16();
            //header.SpriteFlags = this.ReadUInt16();
            //header.InitializeRoutinePointer = this.ReadUInt16();
            //header.MovementRoutinePointer = this.ReadUInt16();
            //header.AttackRoutinePointer = this.ReadUInt16();
            //header.DamagedRoutinePointer = this.ReadUInt16();
            //header.DefaultCommandScriptPointer = this.ReadUInt16();
            //header.DeathCommandScriptPointer = this.ReadUInt16();
            //header.AICommandSetPointer = this.ReadUInt16();
            //header.BossShadowSize = this.ReadUInt16();
            //header.PostDeathHandlerPointer = this.ReadUInt16();
            //header.UnknownValue = this.ReadUInt16();
            //header.SpecialDeathHandlerPointer = this.ReadUInt16();
            return null!;
        }

        public ManaBossWeapon ReadWeapon(int index)
        {
            this.Seek((int)(Constants.Bank10.BossWeaponTableAddress + (Constants.Bank10.BossWeaponTableRowSize * index)));

            MonsterType affinity = (MonsterType)this.Read();
            ElementalType element = (ElementalType)this.Read();
            byte accuracy = this.Read();
            byte power = this.Read();
            StatusEffects statusEffects = (StatusEffects)this.ReadUInt16();
            byte inflictionRate = this.Read();

            return new ManaBossWeapon((byte)index, affinity, element, accuracy, power, statusEffects, inflictionRate);
        }

        public EnemyStatEntry ReadStatEntry(int spriteIndex)
        {
            if (spriteIndex < 0x57 || spriteIndex > 0x7F)
            {
                return (EnemyStatEntry)ThrowHelper.ThrowArgumentException("Sprite Index out of range.", nameof(spriteIndex));
            }

            this.Seek((int)(Constants.Bank10.BossStatTableAddress + (Constants.Bank10.SpriteStatisticsTableRowSize * (spriteIndex - 0x57))));

            EnemyStatEntry entry = new EnemyStatEntry((byte)spriteIndex);
            //entry.Level = this.Read();
            //entry.HitPoints = this.ReadUInt16();
            //entry.ManaPoints = this.Read();
            //entry.Strength = this.Read();
            //entry.Agility = this.Read();
            //entry.Intelligence = this.Read();
            //entry.Wisdom = this.Read();
            //entry.Evasion = this.Read();
            //entry.Defense = this.ReadUInt16();
            //entry.MagicEvasion = this.Read();
            //entry.MagicDefense = this.ReadUInt16();
            //entry.MonsterType = (MonsterType)this.Read();
            //entry.Element = (ElementalType)this.Read();
            //entry.ExperienceAward = this.ReadUInt16();
            //entry.BlackMagicPower = this.Read();
            //entry.WhiteMagicPower = this.Read();
            //entry.Immunities = (StatusEffects)this.ReadUInt16();
            //_ = this.Read();
            //entry.Weapon01 = this.Read();
            //entry.Weapon02 = this.Read();
            //_ = this.Read();

            //byte wmLevel = this.Read();
            //entry.WeaponLevel = (byte)((wmLevel >> 4) & 0x0F);
            //entry.MagicLevel = (byte)(wmLevel & 0x0F);

            //entry.GoldAward = this.ReadUInt16();

            return entry;
        }

        public BossAICommandSet ReadAICommandSet(ushort commandPointerTableAddress)
        {
            List<ushort> pointerTable = this.commandParser.ParseCommandPointerTable(commandPointerTableAddress);
            List<BossAICommand> commands = this.commandParser.ParseCommands(pointerTable);
            return new BossAICommandSet(pointerTable, commands);
        }

        public BossAnimationScript ReadAnimationScript(int index)
        {
            return this.animationParser.ParseAnimation((uint)index);
        }
    }

    internal sealed class BossAIRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public BossAIRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteSetupHeader(BossFamily family, BossSetupHeader header)
        {
            this.Seek((int)BossReaderWriterUtil.GetBossSetupHeaderAddress(family));

            this.WriteUInt16((ushort)header.ControlFlags);
            this.WriteUInt16(header.SpriteFlags);
            this.WriteUInt16(header.InitializeRoutinePointer);
            this.WriteUInt16(header.MovementRoutinePointer);
            this.WriteUInt16(header.AttackRoutinePointer);
            this.WriteUInt16(header.DamagedRoutinePointer);
            this.WriteUInt16(header.DefaultCommandScriptPointer);
            this.WriteUInt16(header.DeathCommandScriptPointer);
            this.WriteUInt16(header.AICommandSetPointer);
            this.WriteUInt16(header.ShadowSize);
            this.WriteUInt16(header.PostDeathHandlerPointer);
            this.WriteUInt16(header.UnknownValue);
            this.WriteUInt16(header.SpecialDeathHandlerPointer);
        }

        public ushort WriteAICommand(BossAICommand command)
        {
            foreach (BossAICommandAction action in command)
            {
                action.Write(this);
            }
            return (ushort)(this.Position & 0xFFFF);
        }

        public ushort WriteAnimationScript(BossAnimationScript script)
        {
            //script.Write(this);
            _ = script;
            return (ushort)(this.Position & 0xFFFF);
        }

        public void WriteWeapon(ManaBossWeapon weapon, int index)
        {
            this.Seek((int)(Constants.Bank10.BossWeaponTableAddress + (Constants.Bank10.BossWeaponTableRowSize * index)));

            this.Write((byte)weapon.MonsterAffinity);
            this.Write((byte)weapon.Element);
            this.Write(weapon.Accuracy);
            this.Write(weapon.Power);
            this.WriteUInt16((ushort)weapon.StatusEffects);
            this.Write(weapon.InflictionRate);
        }

        public void WriteStatEntry(EnemyStatEntry entry, int spriteIndex)
        {
            if (spriteIndex < 0x57 || spriteIndex > 0x7F)
            {
                ThrowHelper.ThrowArgumentException("Sprite Index out of range.", nameof(spriteIndex));
            }

            this.Seek((int)(Constants.Bank10.BossStatTableAddress + (Constants.Bank10.SpriteStatisticsTableRowSize * (spriteIndex - 0x57))));

            this.Write(entry.Level);
            this.WriteUInt16(entry.HitPoints);
            this.Write(entry.ManaPoints);
            this.Write(entry.Strength);
            this.Write(entry.Agility);
            this.Write(entry.Intelligence);
            this.Write(entry.Wisdom);
            this.Write(entry.Evasion);
            this.WriteUInt16(entry.Defense);
            this.Write(entry.MagicEvasion);
            this.WriteUInt16(entry.MagicDefense);
            this.Write((byte)entry.MonsterType);
            this.Write((byte)entry.Element);
            this.WriteUInt16(entry.ExperienceAward);
            this.Write(entry.BlackMagicPower);
            this.Write(entry.WhiteMagicPower);
            this.WriteUInt16((ushort)entry.Immunities);
            this.Write(entry.Unused);
            this.Write(entry.MeleeWeapon);
            this.Write(entry.RangedWeapon);
            this.Write((byte)entry.DeathStyle);
            byte b = (byte)(((entry.WeaponLevel & 0x0F) << 4) | (entry.MagicLevel & 0x0F));
            this.Write((byte)(((entry.WeaponLevel & 0x0F) << 4) | (entry.MagicLevel & 0x0F)));
            this.WriteUInt16(entry.GoldAward);
        }

        public void WriteFrame(BossSpriteFrame frame)
        {
            this.Seek(frame.Address);
            this.WriteUInt16((ushort)(frame.Length & 0xFFFF));
            foreach (BossSpriteFramePart framePart in frame.Parts)
            {
                this.Write((byte)framePart.XCoordinate);
                this.Write((byte)framePart.YCoordinate);
                this.Write(framePart.TileId);
                this.Write((byte)framePart.Flags);
            }
        }

        public void WriteByteAt(int index, byte value)
        {
            this.RomFile.WriteByteAt(index, value);
        }

        public void WriteUInt16At(int index, ushort value)
        {
            this.RomFile.WriteUInt16At(index, value);
        }
    }

    internal static class BossReaderWriterUtil
    {
        public static uint GetBossSetupHeaderAddress(BossFamily family)
        {
            switch (family)
            {
                case BossFamily.Aegagropilon:
                    return Constants.Bank02.AegagropilonSetupHeaderAddress;
                case BossFamily.Ant:
                    return Constants.Bank02.AntFamilySetupHeaderAddress;
                case BossFamily.Lich:
                    return Constants.Bank02.DarkLichSetupHeaderAddress;
                case BossFamily.DeathMachine:
                    return Constants.Bank02.DeathMachineSetupHeaderAddress;
                case BossFamily.Dragon:
                    return Constants.Bank02.DragonFamilySetupHeaderAddress;
                case BossFamily.MechRider:
                    return Constants.Bank02.MechRiderFamilySetupHeaderAddress;
                case BossFamily.KettleKin:
                    return Constants.Bank02.KettleKinSetupHeaderAddress;
                case BossFamily.Hexas:
                    return Constants.Bank02.HexasFamilySetupHeaderAddress;
                default:
                    return (uint)ThrowHelper.ThrowArgumentException("Boss family does not have a setup header.", nameof(family));
            }
        }
    }
}
/*

 */
/*

public const uint UndergroundHeadAttackScriptsLocation = Constants.Bank02Offset | 0xF3A0;
public const uint UndergroundHeadAttackCommandLocation = Constants.Bank02Offset | 0xF3B0;
public const uint UndergroundHeadAttackRoutineLocation = Constants.Bank02Offset | 0xF3E0;
public const uint UndergroundHeadAttackRoutinePatchLocation = Constants.Bank02Offset | 0x6EAE;
public const byte UndergroundDamageAnimationIndex = 0xF7;
public const byte UndergroundHeadAttackCommandIndex = 0x28;

// Shared actions between commands.
// Using the damage animation for his underground attack animation, it always looked like he was laughing to me, which fits.
BossAIPlayAnimationAction playAnimationAction = new BossAIPlayAnimationAction(AIConstants.DarkLich.UndergroundDamageAnimationIndex);
//BossAIPlayAnimationAction playAnimationAction = new BossAIPlayAnimationAction(0xF6);
BossAIEndCommandAction endCommandAction = new BossAIEndCommandAction();

// Create the attack commands for the new attacks. Two spells and two skills.
BossAICommand undergroundCastEvilGateOnTarget = new BossAICommand();
undergroundCastEvilGateOnTarget.Add(playAnimationAction);
undergroundCastEvilGateOnTarget.Add(new BossAICastSpellAction(ManaSpell.EvilGate, 0x00));
undergroundCastEvilGateOnTarget.Add(endCommandAction);

BossAICommand undergroundCastHPAbsorbOnTarget = new BossAICommand();
undergroundCastHPAbsorbOnTarget.Add(playAnimationAction);
undergroundCastHPAbsorbOnTarget.Add(new BossAICastSpellAction(ManaSpell.EnergyAbsorb, 0x00));
undergroundCastHPAbsorbOnTarget.Add(endCommandAction);

BossAICommand undergroundUsePygmusGlareOnTarget = new BossAICommand();
undergroundUsePygmusGlareOnTarget.Add(playAnimationAction);
undergroundUsePygmusGlareOnTarget.Add(new BossAIPlaySkillCommand(0x16, 0x00, 0x00));
undergroundUsePygmusGlareOnTarget.Add(new BossAIFreezeAnimationAction(0x0024));
undergroundUsePygmusGlareOnTarget.Add(new BossAIEquipWeaponAction(0x004A));
undergroundUsePygmusGlareOnTarget.Add(endCommandAction);

BossAICommand undergroundUseConfuseHoopsOnTarget = new BossAICommand();
undergroundUseConfuseHoopsOnTarget.Add(playAnimationAction);
undergroundUseConfuseHoopsOnTarget.Add(new BossAIPlaySkillCommand(0x10, 0x00, 0x00));
undergroundUseConfuseHoopsOnTarget.Add(new BossAIFreezeAnimationAction(0x0024));
undergroundUseConfuseHoopsOnTarget.Add(new BossAIEquipWeaponAction(0x0047));
undergroundUseConfuseHoopsOnTarget.Add(endCommandAction);

// Write out the commands to the ROM and collect the offsets to them.
Span<ushort> commandPointers = stackalloc ushort[4];
this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadAttackCommandLocation);
commandPointers[0] = (ushort)(this.romWriter.Position & 0xFFFF);
commandPointers[1] = this.romWriter.WriteAICommand(undergroundCastEvilGateOnTarget);
commandPointers[2] = this.romWriter.WriteAICommand(undergroundCastHPAbsorbOnTarget);
commandPointers[3] = this.romWriter.WriteAICommand(undergroundUsePygmusGlareOnTarget);
this.romWriter.WriteAICommand(undergroundUseConfuseHoopsOnTarget);

// Update the command pointers to point to the new commands.
// Unlike most bosses only has 8 unused command pointers and we are using half of them here.
this.romWriter.Seek((Constants.Bank02Offset | header.AICommandSetPointer) + (sizeof(ushort) * AIConstants.DarkLich.UndergroundHeadAttackCommandIndex));
this.romWriter.WriteUInt16(commandPointers[0]);
this.romWriter.WriteUInt16(commandPointers[1]);
this.romWriter.WriteUInt16(commandPointers[2]);
this.romWriter.WriteUInt16(commandPointers[3]);

// AI Scripts to execute the new commands. (28-2B)
ReadOnlySpan<byte> undergroundAIScripts = stackalloc byte[]
{
    0x2A, 0xFF, // Cast Spell: 'Evil Gate' On 'Current Target'.
    0x2A, 0xFF, // Cast Spell: 'Energy Absorb' On 'Current Target'.
    0x2A, 0xFF, // Use Skill: 'Pygmus Glare' On 'Current Target'.
    0x2A, 0xFF, // Use Skill: 'Confuse Hoops' On 'Current Target'.
};
this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadAttackScriptsLocation);
this.romWriter.WriteBytes(undergroundAIScripts);

// Write out a AI Roulette table for the underground head attacks.
int attackTableLocation = this.romWriter.Position;
this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadAttackScriptsLocation & 0xFFFF) + (sizeof(ushort) * 0));
this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadAttackScriptsLocation & 0xFFFF) + (sizeof(ushort) * 1));
this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadAttackScriptsLocation & 0xFFFF) + (sizeof(ushort) * 2));
this.romWriter.WriteUInt16((ushort)(AIConstants.DarkLich.UndergroundHeadAttackScriptsLocation & 0xFFFF) + (sizeof(ushort) * 3));

// Need a new attack routine to allow his head to attack instead of just canceling the attack frame.
byte atkH = (byte)((attackTableLocation >> 8) & 0xFF);
byte atkL = (byte)(attackTableLocation & 0xFF);
ReadOnlySpan<byte> undergroundAtackRoutine = stackalloc byte[]
{
    //[DarkLich_UndergroundAttack]
    0x89, 0x00, 0x80,       // BIT #$8000      ;Test to see if Dark Lich's head is above ground.
    0xD0, 0x05,             // BNE $05         ;Branch to DarkLich_UndergroundAttack_Head if it is.
    0xA9, 0x94, 0xE8,       // LDA #$E894      ;Load 0xE894 into Accumulator. (Attack: Skeletal Squeeze)
    0x18,                   // CLC             ;Clear Carry Flag.
    0x60,                   // RTS             ;Return.

    //[DarkLich_UndergroundAttack_Head]
    0xA9, 0x03, 0x00,       // LDA #$0003      ;Load 0x0003 into Accumulator.
    0x20, 0x0B, 0x30,       // JSR $300B       ;Jump to GetBossRandomNumberInRange.
    0x0A,                   // ASL A           ;Multiply By 2.
    0xAA,                   // TAX             ;Transfer the pointer to Register X.
    0xBF, atkL, atkH, 0xC2, // LDA $C2????,X   ;Load the X-Coordinate from DataTable_DarkLich_UndergroundAttack.
    0x18,                   // CLC             ;Clear Carry Flag.
    0x60,                   // RTS             ;Return..
};
this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadAttackRoutineLocation);
this.romWriter.WriteBytes(undergroundAtackRoutine);

// Lastly we have to patch the section of the routine that controls Dark Lich's underground attack options.
atkH = (byte)((AIConstants.DarkLich.UndergroundHeadAttackRoutineLocation >> 8) & 0xFF);
atkL = (byte)(AIConstants.DarkLich.UndergroundHeadAttackRoutineLocation & 0xFF);
ReadOnlySpan<byte> undergroundAtackRoutinePatch = stackalloc byte[]
{
    0x20, atkL, atkH, // JSR $????       ;Jump to DarkLich_UndergroundAttack.
    0x60,             // RTS             ;Return.
    0xEA,             // NOP             ;No Operation. (Just to clear out the entire instruction)
};
this.romWriter.Seek((int)AIConstants.DarkLich.UndergroundHeadAttackRoutinePatchLocation);
this.romWriter.WriteBytes(undergroundAtackRoutinePatch);

this.romWriter.Seek(0x1CE4F9);
this.romWriter.WriteUInt16(0x0133);
this.romWriter.WriteUInt16(0x0133);
this.romWriter.WriteUInt16(0x0133);
this.romWriter.WriteUInt16(0x0133);
 */

/*
//[Hexas_ShadeElementalAttack]
0xA9, 0x02, 0x00,       // LDA #$0002      ;Load 0x0002 into Accumulator.
0x20, 0x0B, 0x30,       // JSR $300B       ;Jump to GetBossRandomNumberInRange.
0x0A,                   // ASL A           ;Multiply By 2.
0xAA,                   // TAX             ;Transfer the index to Register X.
0xBF, 0x32, 0xE3, 0xDC, // LDA $DCE332,X   ;Load an attack pointer from PointerTable_HexasShadeBarrierAbility.
0x18,                   // CLC             ;Clear Carry Flag.
0x60,                   // RTS             ;Return.
        //[Hexas_Movement]
        0x20, 0x41, 0x3A,      // JSR $3A41       ;Jump to TargetClosestOpponentAndSetOrdinalDirection.
        0xBD, 0xD2, 0x01,      // LDA $01D2,X     ;Load [CURRENT_EXPERIENCE_AXE, X] into Accumulator.
        0xF0, 0x10,            // BEQ $10         ;Branch to Hexas_Movement_BarrierChange if the hit counter is zero.
        0xBD, 0x96, 0x00,      // LDA $0096,X     ;Load [BOSS_AI_CALL_TIMER, X] into Accumulator.
        0xC9, 0x48, 0x00,      // CMP #$0048      ;Compare [BOSS_AI_CALL_TIMER, X] against 0x0048. (360 Frames)
        0x90, 0x35,            // BCC $35         ;Branch to Hexas_Movement_RunAway if enough frames haven't passed.
        0xA9, 0x02, 0x00,      // LDA #$0002      ;Load 0x0002 into Accumulator.
        0x20, 0x0B, 0x30,      // JSR $300B       ;Jump to GetBossRandomNumberInRange.
        0xF0, 0x2D,            // BEQ $2D         ;Branch to Hexas_Movement_RunAway if the random value is zero. (66% chance of Barrier Change)

        //[Hexas_Movement_BarrierChange]
        0xBD, 0x7E, 0x00,       // LDA $007E,X     ;Load [BOSS_STATE_FLAGS, X] into Accumulator.
        0x29, 0xFF, 0x00,       // AND #$00FF      ;Drop the high byte.
        0x85, 0x00,             // STA $00         ;Store value into {TempVar:CurrentForm}.
        0xA9, 0x04, 0x00,       // LDA #$0004      ;Load 0x0004 into Accumulator. (Get a new random element)
        0x20, 0x0B, 0x30,       // JSR $300B       ;Jump to GetBossRandomNumberInRange.
        0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
        0xC5, 0x00,             // CMP $00         ;Compare the result against {TempVar:CurrentForm}.
        0xF0, 0x17,             // BEQ $17         ;Branch to Hexas_Movement_BarrierChange_Failure if we picked the element we currently are.
        0x0A,                   // ASL A           ;Multiply By 2.
        0xAA,                   // TAX             ;Transfer the index to Register X.
        0xBF, 0x24, 0xE3, 0xDC, // LDA $DCE324,X   ;Load a Barrier Change pointer from PointerTable_HexasBarrierChange.
        0x85, 0x00,             // STA $00         ;Store value into {TempVar:BarrierChangePointer}.
        0xA6, 0x87,             // LDX $87         ;Load {TempVar:BossOffset} into Register X.
        0x9E, 0x96, 0x00,       // STZ $0096,X     ;Store Zero into [BOSS_AI_CALL_TIMER, X].
        0xA9, 0x05, 0x00,       // LDA #$0005      ;Load 0x0005 into Accumulator.
        0x9D, 0xD2, 0x01,       // STA $01D2,X     ;Store 0x0005 into [CURRENT_EXPERIENCE_AXE, X].
        0xA5, 0x00,             // LDA $00         ;Load {TempVar:BarrierChangePointer} into Accumulator.
        0x18,                   // CLC             ;Clear Carry Flag.
        0x60,                   // RTS             ;Return.

        //[Hexas_Movement_BarrierChange_Failure]
        0x38,                   // SEC             ;Set Carry Flag.
        0x60,                   // RTS             ;Return.

        //[Hexas_Movement_RunAway]
        0xBD, 0xA9, 0x00,       // LDA $00A9,X     ;Load [BOSS_CURRENT_TARGET, X] into Accumulator.
        0x20, 0xD9, 0x08,       // JSR $08D9       ;Jump to GetRelativeUncappedDistanceFromTarget.
        0xC9, 0x06, 0x00,       // CMP #$0006      ;Compare result against 0x0006.
        0xB0, 0x0A,             // BCS $0A         ;Branch to Hexas_Movement_Idle if Hexas is far enough away from his target.
        0xBD, 0xAB, 0x00,       // LDA $00AB,X     ;Load [BOSS_DIRECTION, X] into Accumulator.
        0x0A,                   // ASL A           ;Multiply By 2.
        0xAA,                   // TAX             ;Transfer the pointer to Register X.
        0xBF, 0x14, 0xE3, 0xDC, // LDA $DCE314,X   ;Load a movement pointer into Accumulator. (Hexas will run from his target)
        0x60,                   // RTS             ;Return.

        //[Hexas_Movement_Idle]
        0xA9, 0x46, 0xE1,       // LDA #$E146      ;Load 0xE146 into Accumulator. (Set Animation: Idle)
        0x60,                   // RTS             ;Return.

 */