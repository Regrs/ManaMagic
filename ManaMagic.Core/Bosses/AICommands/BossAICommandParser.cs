using System;
using System.Collections.Generic;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAICommandParser
    {
        private RomReader romReader;

        public BossAICommandParser(RomReader romReader)
        {
            this.romReader = romReader;
        }

        public List<ushort> ParseCommandPointerTable(ushort commandPointerTableAddress)
        {
            List<ushort> commandPointers = new List<ushort>();
            this.romReader.Seek(Constants.Bank02Offset | commandPointerTableAddress);

            ushort firstPointer = this.romReader.ReadUInt16();
            commandPointers.Add(firstPointer);
            while (this.romReader.Position != (Constants.Bank02Offset | firstPointer))
            {
                commandPointers.Add(this.romReader.ReadUInt16());
            }
            return commandPointers;
        }

        public List<BossAICommand> ParseCommands(ushort commandPointerTableAddress)
        {
            List<ushort> commandPointers = this.ParseCommandPointerTable(commandPointerTableAddress);
            return this.ParseCommands(commandPointers);
        }

        public List<BossAICommand> ParseCommands(IReadOnlyList<ushort> commandPointers)
        {
            List<BossAICommand> commands = new List<BossAICommand>(commandPointers.Count);

            for (int i = 0; i < commandPointers.Count; i++)
            {
                ushort commandPointer = commandPointers[i];
                this.romReader.Seek(Constants.Bank02Offset | commandPointer);

                BossAICommandActionType actionType = default(BossAICommandActionType);
                List<BossAICommandAction> actions = new List<BossAICommandAction>(commandPointers.Count);
                do
                {
                    actionType = (BossAICommandActionType)this.romReader.Read();
                    switch (actionType)
                    {
                        case BossAICommandActionType.PlayAnimation:
                            ushort animationIndex = this.romReader.ReadUInt16();
                            actions.Add(new BossAIPlayAnimationAction(animationIndex));
                            break;
                        case BossAICommandActionType.SetBossCoordinateSpeed:
                            ushort xSubPixelSpeed = this.romReader.ReadUInt16();
                            ushort xSpeed = this.romReader.ReadUInt16();
                            ushort ySubPixelSpeed = this.romReader.ReadUInt16();
                            ushort ySpeed = this.romReader.ReadUInt16();
                            ushort zSubPixelSpeed = this.romReader.ReadUInt16();
                            ushort zSpeed = this.romReader.ReadUInt16();
                            actions.Add(new BossAISetBossCoordinateSpeedAction(xSubPixelSpeed, xSpeed, ySubPixelSpeed, ySpeed, zSubPixelSpeed, zSpeed));
                            break;
                        case BossAICommandActionType.SetBossCoordinateRoutine:
                            ushort coordinateRoutine = this.romReader.ReadUInt16();
                            actions.Add(new BossAISetBossCoordinateRoutineAction(coordinateRoutine));
                            break;
                        case BossAICommandActionType.SetGenericRoutine:
                            ushort genericRoutine = this.romReader.ReadUInt16();
                            actions.Add(new BossAISetGenericRoutineAction(genericRoutine));
                            break;
                        case BossAICommandActionType.SetCommandLockRoutine:
                            ushort commandLockRoutine = this.romReader.ReadUInt16();
                            actions.Add(new BossAISetCommandLockRoutineAction(commandLockRoutine));
                            break;
                        case BossAICommandActionType.FreezeAnimation:
                            ushort freezeTime = this.romReader.ReadUInt16();
                            actions.Add(new BossAIFreezeAnimationAction(freezeTime));
                            break;
                        case BossAICommandActionType.SetHorizontalFlip:
                            actions.Add(new BossAISetHorizontalFlipAction());
                            break;
                        case BossAICommandActionType.ClearHorizontalFlip:
                            actions.Add(new BossAIClearHorizontalFlipAction());
                            break;
                        case BossAICommandActionType.CastSpell:
                            ManaSpell spellID = (ManaSpell)this.romReader.Read();
                            byte spellTargetID = this.romReader.Read();
                            actions.Add(new BossAICastSpellAction(spellID, spellTargetID));
                            break;
                        case BossAICommandActionType.PlaySkillAnimation:
                            byte skillID = this.romReader.Read();
                            byte skillXCoordinate = this.romReader.Read();
                            byte skillYCoordinate = this.romReader.Read();
                            actions.Add(new BossAIPlaySkillCommand(skillID, skillXCoordinate, skillYCoordinate));
                            break;
                        case BossAICommandActionType.CallRoutine:
                            ushort routinePointer = this.romReader.ReadUInt16();
                            actions.Add(new BossAICallRoutineAction(routinePointer));
                            break;
                        case BossAICommandActionType.SetBossData:
                            ushort addressToSet = this.romReader.ReadUInt16();
                            ushort valueToSet = this.romReader.ReadUInt16();
                            actions.Add(new BossAISetBossDataAction(addressToSet, valueToSet));
                            break;
                        case BossAICommandActionType.EquipBossWeapon:
                            ushort bossWeaponID = this.romReader.ReadUInt16();
                            actions.Add(new BossAIEquipWeaponAction(bossWeaponID));
                            break;
                        case BossAICommandActionType.PlaySoundEffect:
                            ushort soundEffectIndex = this.romReader.ReadUInt16();
                            actions.Add(new BossAIPlaySoundEffectAction(soundEffectIndex));
                            break;
                        case BossAICommandActionType.JumpToScriptAddress:
                            ushort scriptAddress = this.romReader.ReadUInt16();
                            actions.Add(new BossAIScriptJumpAction(scriptAddress));
                            break;
                        case BossAICommandActionType.EndCommand:
                            actions.Add(new BossAIEndCommandAction());
                            break;
                        default:
                            throw new Exception("Unimplemented Action Type");
                    }

                    if ((i + 1) < commandPointers.Count)
                    {
                        if (this.romReader.Position == (Constants.Bank02Offset | commandPointers[i + 1]) ||
                            actionType == BossAICommandActionType.JumpToScriptAddress)
                        {
                            actionType = BossAICommandActionType.EndCommand;
                        }
                    }
                } while (actionType != BossAICommandActionType.EndCommand);

                commands.Add(new BossAICommand(actions));
            }
            return commands;
        }
    }


    public sealed class BossAICommandSet
    {
        public List<ushort> PointerTable { get; }
        public List<BossAICommand> Commands { get; }

        public BossAICommandSet(List<ushort> pointerTable, List<BossAICommand> commands)
        {
            this.PointerTable = pointerTable;
            this.Commands = commands;
        }

        public void SetPointer(int index, ushort pointer)
        {
            this.PointerTable[index] = pointer;
        }

        public void CopyPointer(int sourceIndex, int destinationIndex)
        {
            this.PointerTable[destinationIndex] = this.PointerTable[sourceIndex];
        }
    }

}

/*
[Command Usage Examples]
02D10A: 00 0A00         ;Play Animation: BossAnimationScript_Ant_Idle.
02D164: 01 0000 0000	;Set Boss Coordinate Speed: X-SubPixel Speed: 0x0000, X-Speed: 0x0000,
02D169:    0000 FEFF	;                           Y-SubPixel Speed: 0x0000, Y-Speed: 0xFFFE, 
02D16D:    0000 1400	;                           Z-SubPixel Speed: 0x0000, Z-Speed: 0x0014.
02D1C4: 02 BC36         ;Set Boss Object Coordinate Routine: $36BC.
02D1C7: 04 6637         ;Set Command Lock Routine: BossCommandLock_Airborne.
02D10D: 05 0C00         ;Freeze Animation for 0x000C Frames.
02D111: 06              ;Set Horizontal Flip.
02D109: 0A              ;Clear Horizontal Flip
02D14E: 0E 01 00        ;Cast Spell: Gem Missile. Target: Current Target.
02D1CE: 0F 03 00 E0     ;Play Skill Animation: Skill: Acid Breath, X-Coordinate: 0x00, Y-Coordinate: 0xE0.
02D18D: 10 7F3E         ;Call Routine: CreateKamaProjectile.
02D182: 11 6600 0000    ;Set Boss Data: Set 0x0000 to [BOSS_Z_COORDINATE].
02D144: 13 4400         ;Equip Boss Weapon: MantisAnt_Kama.
C2F140:	14 8300         ;Play Sound Effect: 0083.
02D801: 15 15D8         ;Jump to Script Address: $D815.
02D110: FF              ;End Command Subset.
 */