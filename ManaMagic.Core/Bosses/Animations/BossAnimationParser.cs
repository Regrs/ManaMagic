using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Bosses.Animations
{
    /// <summary>
    /// Provides methods to parse a boss animation script from a Secret of Mana ROM file.
    /// </summary>
    public sealed class BossAnimationParser
    {
        private readonly RomReader romReader;

        /// <summary>
        /// Create a new instance of the <see cref="BossAnimationParser"/> class.
        /// </summary>
        /// <param name="romReader">The <see cref="RomReader"/> the animation scripts will be read from.</param>
        public BossAnimationParser(RomReader romReader)
        {
            this.romReader = romReader;
        }

        /// <summary>
        /// Parses the boss animation script at the specified index.
        /// </summary>
        /// <param name="index">The index of the animation to parse.</param>
        /// <returns>A <see cref="BossAnimationScript"/> containing the parse animation data.</returns>
        public BossAnimationScript ParseAnimation(uint index)
        {
            ValidationHelper.ThrowIfArgumentLessThan(index, 0u, "Index is out of range.");
            ValidationHelper.ThrowIfArgumentGreaterThanOrEqual(index, Constants.Bank1B.BossAnimationPointerTableSize, "Index is out of range.");

            BossAnimationScript script = new BossAnimationScript();
            this.romReader.Seek((int)(Constants.Bank1B.BossAnimationPointerTableAddress + (sizeof(ushort) * index)));

            // If the offset is zero then the animation script is dummied out and doesn't exist. This is only true for animation index #1.
            ushort offset = this.romReader.ReadUInt16();
            if (offset > 0)
            {
                int pointer = Constants.Bank01Offset | offset;
                this.romReader.Seek(pointer);

                bool done = false;
                while (!done)
                {
                    // An animation script will continue until one of three conditions are met.
                    // 1) A frame with a delay of 0 is loaded.
                    // 2) Command 1B (End Script) is read.
                    // 3) Command 15 (PlayAnimation) is read.
                    // Frame offsets must be aligned to even addresses. Odd values are interpreted as animation commands.
                    byte commandByte = this.romReader.Read();
                    if ((commandByte & 0x01) == 0)
                    {
                        // If the byte is even, then this is a frame address and frame delay. (3 bytes total)
                        byte frameAddressH = this.romReader.Read();
                        byte frameDelay = this.romReader.Read();
                        ushort frameAddress = (ushort)((frameAddressH << 8) | commandByte);

                        LoadFrameBossAnimationCommand command = new LoadFrameBossAnimationCommand(frameAddress, frameDelay);
                        script.Add(command);

                        // If the frame delay is zero then the script is over.
                        // This is the most common way an animation script will end.
                        if (command.FrameDelay == 0) { done = true; }
                    }
                    else
                    {
                        // If the byte is odd then its a command.
                        BossAnimationCommandType commandType = (BossAnimationCommandType)commandByte;
                        done = this.ParseCommand(commandType, script);
                    }
                }
            }

            return script;
        }

        private bool ParseCommand(BossAnimationCommandType commandType, BossAnimationScript script)
        {
            switch (commandType)
            {
                case BossAnimationCommandType.ToggleHorizontalFlip:
                    script.Add(new ToggleHorizontalFlipBossAnimationCommand());
                    break;
                case BossAnimationCommandType.ToggleVerticalFlip:
                    // Unused.
                    script.Add(new ToggleVerticalFlipBossAnimationCommand());
                    break;
                case BossAnimationCommandType.ClearHorizontalFlip:
                    // Unused.
                    script.Add(new ClearHorizontalFlipBossAnimationCommand());
                    break;
                case BossAnimationCommandType.SetHorizontalFlip:
                    script.Add(new SetHorizontalFlipBossAnimationCommand());
                    break;
                case BossAnimationCommandType.ClearVerticalFlip:
                    // Unused.
                    script.Add(new ClearVerticalFlipBossAnimationCommand());
                    break;
                case BossAnimationCommandType.SetVerticalFlip:
                    // Unused.
                    script.Add(new SetVerticalFlipBossAnimationCommand());
                    break;
                case BossAnimationCommandType.SetSpeed:
                    byte xSpeed = this.romReader.Read();
                    byte ySpeed = this.romReader.Read();
                    script.Add(new SetSpeedBossAnimationCommand(xSpeed, ySpeed));
                    break;
                case BossAnimationCommandType.DummiedCommand:
                    // Unused.
                    script.Add(new DummiedCommandBossAnimationCommand());
                    break;
                case BossAnimationCommandType.SetScriptPosition:
                    ushort position = this.romReader.ReadUInt16();
                    script.Add(new SetScriptPositionBossAnimationCommand(position));
                    break;
                case BossAnimationCommandType.SetBossAICommand:
                    // Unused.
                    ushort offset = this.romReader.ReadUInt16();
                    ushort commandPointer = this.romReader.ReadUInt16();
                    script.Add(new SetBossAICommandBossAnimationCommand(offset, commandPointer));
                    break;
                case BossAnimationCommandType.PlayAnimation:
                    ushort bossAnimationID = this.romReader.ReadUInt16();
                    script.Add(new PlayAnimationBossAnimationCommand(bossAnimationID));
                    return true;
                case BossAnimationCommandType.PlaySoundEffect:
                    ushort soundEffect = this.romReader.ReadUInt16();
                    script.Add(new PlaySoundBossAnimationCommand(soundEffect));
                    break;
                case BossAnimationCommandType.SetFrameBank:
                    byte bank = this.romReader.Read();
                    script.Add(new SetFrameBankBossAnimationCommand(bank));
                    break;
                case BossAnimationCommandType.EndScript:
                    script.Add(new EndScriptBossAnimationCommand());
                    return true;
                case BossAnimationCommandType.PlayAnimationOnTarget:
                    byte targetAnimationID = this.romReader.Read();
                    script.Add(new PlayAnimationOnTargetBossAnimationCommand(targetAnimationID));
                    break;
                case BossAnimationCommandType.MoveTargetToCoordinates:
                    sbyte targetXCoord = this.romReader.ReadSByte();
                    sbyte targetYCoord = this.romReader.ReadSByte();
                    script.Add(new MoveTargetToCoordinatesBossAnimationCommand(targetXCoord, targetYCoord));
                    break;
                case BossAnimationCommandType.Unknown2D:
                    ushort unknown2DParameter1 = this.romReader.ReadUInt16();
                    script.Add(new Unknown2DBossAnimationCommand(unknown2DParameter1));
                    break;
                case BossAnimationCommandType.SetTargetAnimationOverride:
                    // Unused.
                    byte animOverrideID = this.romReader.Read();
                    byte animOverrideDuration = this.romReader.Read();
                    script.Add(new SetTargetAnimationOverrideBossAnimationCommand(animOverrideID, animOverrideDuration));
                    break;
                case BossAnimationCommandType.SetTargetSpeed:
                    // Unused.
                    byte xTargetSpeed = this.romReader.Read();
                    byte yTargetSpeed = this.romReader.Read();
                    byte targetDuration = this.romReader.Read();
                    script.Add(new SetTargetSpeedBossAnimationCommand(xTargetSpeed, yTargetSpeed, targetDuration));
                    break;
                case BossAnimationCommandType.SpawnAnimationObject:
                    ushort animObjID = this.romReader.ReadUInt16();
                    sbyte animObjectXOffset = this.romReader.ReadSByte();
                    sbyte animObjectYOffset = this.romReader.ReadSByte();
                    script.Add(new SpawnAnimationObjectBossAnimationCommand(animObjID, animObjectXOffset, animObjectYOffset));
                    break;
                case BossAnimationCommandType.ClearWorkRAM:
                    // Unused.
                    script.Add(new ClearWorkRAMBossAnimationCommand());
                    break;
                case BossAnimationCommandType.EnableFlicker:
                    script.Add(new EnableFlickerBossAnimationCommand());
                    break;
                case BossAnimationCommandType.DisableFlicker:
                    script.Add(new DisableFlickerBossAnimationCommand());
                    break;
                case BossAnimationCommandType.SetScreenYCoordinate:
                    ushort yCoord2 = this.romReader.ReadUInt16();
                    script.Add(new SetScreenYCoordinateBossAnimationCommand(yCoord2));
                    break;
                case BossAnimationCommandType.SetAnimationSyncValue:
                    ushort syncValue = this.romReader.ReadUInt16();
                    script.Add(new SetAnimationSyncValueBossAnimationCommand(syncValue));
                    break;
                case BossAnimationCommandType.SpawnAnimationObjectEx:
                    ushort animObjIDEx = this.romReader.ReadUInt16();
                    BossControlFlags controlFlags = (BossControlFlags)this.romReader.ReadUInt16();
                    sbyte animObjectXOffsetEx = this.romReader.ReadSByte();
                    sbyte animObjectYOffsetEx = this.romReader.ReadSByte();
                    script.Add(new SpawnAnimationObjectExBossAnimationCommand(animObjIDEx, controlFlags, animObjectXOffsetEx, animObjectYOffsetEx));
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("Unknown boss animation command.");
                    break;
            }
            return false;
        }
    }

    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Represents an animation command for a boss animation script.
    /// </summary>
    public enum BossAnimationCommandType : byte
    {
        /// <summary>
        /// An invalid command.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Toggles the bosses 'Horizontal Flip' control flag.
        /// </summary>
        ToggleHorizontalFlip = 0x01,
        /// <summary>
        /// Toggles the bosses 'Vertical Flip' control flag.
        /// </summary>
        /// <remarks>Like all commands relating to Vertical Flip, this command is unused.</remarks>
        ToggleVerticalFlip = 0x03, // Unused.
        /// <summary>
        /// Clears the bosses 'Horizontal Flip' control flag.
        /// </summary>
        /// <remarks>This command is unused.</remarks>
        ClearHorizontalFlip = 0x05, // Unused.
        /// <summary>
        /// Sets the bosses 'Horizontal Flip' control flag.
        /// </summary>
        SetHorizontalFlip = 0x07,
        /// <summary>
        /// Clears the bosses 'Vertical Flip' control flag.
        /// </summary>
        /// <remarks>Like all commands relating to Vertical Flip, this command is unused.</remarks>
        ClearVerticalFlip = 0x09, // Unused.
        /// <summary>
        /// Sets the bosses 'Vertical Flip' control flag.
        /// </summary>
        /// <remarks>Like all commands relating to Vertical Flip, this command is unused.</remarks>
        SetVerticalFlip = 0x0B, // Unused.
        /// <summary>
        /// Sets the bosses X/Y speed.
        /// </summary>
        SetSpeed = 0x0D,
        /// <summary>
        /// This command does nothing.
        /// </summary>
        /// <remarks>It also has no operands.</remarks>
        DummiedCommand = 0x0F, // Unused, No Operands
        /// <summary>
        /// Sets the current position of the animation script.
        /// </summary>
        /// <remarks>Dark Lich's ghetto loop control command.</remarks>
        SetScriptPosition = 0x11,
        /// <summary>
        /// Sets the bosses current AI command.
        /// </summary>
        /// <remarks>This insane command is unused.</remarks>
        SetBossAICommand = 0x13, // Unused.
        /// <summary>
        /// Plays an animation.
        /// </summary>
        /// <remarks>Why would you need this? You don't, but Vampire demanded it.</remarks>
        PlayAnimation = 0x15,
        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        PlaySoundEffect = 0x17,
        /// <summary>
        /// Sets the bank that animations are loaded from.
        /// </summary>
        /// <remarks>CA is the default. Other locations are C7 and DB.</remarks>
        SetFrameBank = 0x19,
        /// <summary>
        /// Ends the animation script.
        /// </summary>
        EndScript = 0x1B,
        /// <summary>
        /// Plays an animation on the bosses current target.
        /// </summary>
        PlayAnimationOnTarget = 0x1D,
        /// <summary>
        /// Moves the bosses current target to an X/Y position.
        /// </summary>
        MoveTargetToCoordinates = 0x1F,
        /// <summary>
        /// Sets the bosses current target's animation override.
        /// </summary>
        /// <remarks>This command is unused.</remarks>
        SetTargetAnimationOverride = 0x21, // Unused.
        /// <summary>
        /// Sets the bosses current target's speed.
        /// </summary>
        /// <remarks>This command is unused.</remarks>
        SetTargetSpeed = 0x23, // Unused.
        /// <summary>
        /// Spawns a boss object that plays an animation.
        /// </summary>
        SpawnAnimationObject = 0x25,
        /// <summary>
        /// Clears the 01/BC00 area of Work RAM.
        /// </summary>
        /// <remarks>This command is unused. Why the hell would you do this?</remarks>
        ClearWorkRAM = 0x27, // Unused.
        /// <summary>
        /// Sets the bosses 'BossFlicker' sprite flag.
        /// </summary>
        EnableFlicker = 0x29,
        /// <summary>
        /// Clears the bosses 'BossFlicker' sprite flag.
        /// </summary>
        DisableFlicker = 0x2B,
        Unknown2D = 0x2D,
        /// <summary>
        /// Sets the bosses screen Y-Coordinate.
        /// </summary>
        SetScreenYCoordinate = 0x2F,
        /// <summary>
        /// Sets the bosses animation sync value.
        /// </summary>
        SetAnimationSyncValue = 0x31,
        /// <summary>
        /// Spawns a boss object, with the specified control flags, that plays an animation.
        /// </summary>
        SpawnAnimationObjectEx = 0x33,

        /// <summary>
        /// Loads an animation frame into memory.
        /// </summary>
        LoadFrame = 0xFF,
    }

    #region Action Classes

    /// <summary>
    /// Represents a boss animation script in Secret of Mana.
    /// </summary>
    [DebuggerDisplay("Count: {Commands.Count}")]
    public sealed class BossAnimationScript : IEnumerable<BossAnimationCommand>
    {
        private readonly List<BossAnimationCommand> commands;

        /// <summary>
        /// Gets a read-only list of <see cref="BossAnimationCommand"/>s in this animation script.
        /// </summary>
        public IReadOnlyList<BossAnimationCommand> Commands { get { return this.commands; } }

        /// <summary>
        /// Gets the <see cref="BossAnimationCommand"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="BossAnimationCommand"/>.</param>
        /// <returns></returns>
        public BossAnimationCommand this[int index] { get { return this.commands[index]; } }

        /// <summary>
        /// Create a new instance of the <see cref="BossAnimationScript"/> class.
        /// </summary>
        public BossAnimationScript()
        {
            this.commands = new List<BossAnimationCommand>();
        }

        /// <summary>
        /// Create a new instance of the <see cref="BossAnimationScript"/> class with the specified command list.
        /// </summary>
        /// <param name="commands">A list of <see cref="BossAnimationCommand"/>s that represent the animation script.</param>
        public BossAnimationScript(List<BossAnimationCommand> commands)
        {
            this.commands = commands;
        }

        /// <summary>
        /// Adds a <see cref="BossAnimationCommand"/> to the animation script.
        /// </summary>
        /// <param name="command">The <see cref="BossAnimationCommand"/> to be added.</param>
        public void Add(BossAnimationCommand command)
        {
            this.commands.Add(command);
        }

        /// <summary>
        /// Inserts a <see cref="BossAnimationCommand"/> at the specified index in the animation script.
        /// </summary>
        /// <param name="index">The index to insert the <see cref="BossAnimationCommand"/>.</param>
        /// <param name="command">The <see cref="BossAnimationCommand"/> to be added.</param>
        public void Insert(int index, BossAnimationCommand command)
        {
            this.commands.Insert(index, command);
        }

        /// <inheritdoc/>
        public IEnumerator<BossAnimationCommand> GetEnumerator()
        {
            return this.commands.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Represents the base class for a boss animation command.
    /// </summary>
    [DebuggerDisplay("{CommandType}")]
    public abstract class BossAnimationCommand
    {
        /// <summary>
        /// Gets the <see cref="BossAnimationCommandType"/> of the command.
        /// </summary>
        public BossAnimationCommandType CommandType { get; }
        /// <summary>
        /// Gets the <see cref="ParameterArray"/> of the command.
        /// </summary>
        public ParameterArray Parameters { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="BossAnimationCommand"/> with the specified <see cref="BossAnimationCommandType"/> and <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="commandType">The <see cref="BossAnimationCommandType"/> of the animation command.</param>
        /// <param name="parameterInfo">A <see cref="ParameterInfo"/> detailing how the command uses its parameters.</param>
        public BossAnimationCommand(BossAnimationCommandType commandType, ParameterInfo parameterInfo)
        {
            this.CommandType = commandType;
            this.Parameters = new ParameterArray(parameterInfo);
        }
    }

    // ------------------------------------------------------------------------------------

    public sealed class LoadFrameBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.Byte);

        public ushort FrameAddress
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }
        public byte FrameDelay
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }

        public LoadFrameBossAnimationCommand(ushort frameAddress, byte frameDelay) : base(BossAnimationCommandType.LoadFrame, LoadFrameBossAnimationCommand.ParameterMetadata)
        {
            this.FrameAddress = frameAddress;
            this.FrameDelay = frameDelay;
        }
    }

    // ------------------------------------------------------------------------------------

    public sealed class ToggleHorizontalFlipBossAnimationCommand : BossAnimationCommand
    {
        public ToggleHorizontalFlipBossAnimationCommand() : base(BossAnimationCommandType.ToggleHorizontalFlip, ParameterInfo.Empty) { }
    }

    public sealed class ToggleVerticalFlipBossAnimationCommand : BossAnimationCommand
    {
        public ToggleVerticalFlipBossAnimationCommand() : base(BossAnimationCommandType.ToggleVerticalFlip, ParameterInfo.Empty) { }
    }

    public sealed class ClearHorizontalFlipBossAnimationCommand : BossAnimationCommand
    {
        public ClearHorizontalFlipBossAnimationCommand() : base(BossAnimationCommandType.ClearHorizontalFlip, ParameterInfo.Empty) { }
    }

    public sealed class SetHorizontalFlipBossAnimationCommand : BossAnimationCommand
    {
        public SetHorizontalFlipBossAnimationCommand() : base(BossAnimationCommandType.SetHorizontalFlip, ParameterInfo.Empty) { }
    }

    public sealed class ClearVerticalFlipBossAnimationCommand : BossAnimationCommand
    {
        public ClearVerticalFlipBossAnimationCommand() : base(BossAnimationCommandType.ClearVerticalFlip, ParameterInfo.Empty) { }
    }

    public sealed class SetVerticalFlipBossAnimationCommand : BossAnimationCommand
    {
        public SetVerticalFlipBossAnimationCommand() : base(BossAnimationCommandType.SetVerticalFlip, ParameterInfo.Empty) { }
    }

    public sealed class SetSpeedBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte);

        public byte XSpeed
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }
        public byte YSpeed
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }

        public SetSpeedBossAnimationCommand(byte xSpeed, byte ySpeed) : base(BossAnimationCommandType.SetSpeed, SetSpeedBossAnimationCommand.ParameterMetadata)
        {
            this.XSpeed = xSpeed;
            this.YSpeed = ySpeed;
        }
    }

    public sealed class DummiedCommandBossAnimationCommand : BossAnimationCommand
    {
        public DummiedCommandBossAnimationCommand() : base(BossAnimationCommandType.DummiedCommand, ParameterInfo.Empty) { }
    }

    public sealed class SetScriptPositionBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort Position
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public SetScriptPositionBossAnimationCommand(ushort position) : base(BossAnimationCommandType.SetScriptPosition, SetScriptPositionBossAnimationCommand.ParameterMetadata)
        {
            this.Position = position;
        }
    }

    public sealed class SetBossAICommandBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.UInt16);

        public ushort Offset
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }
        public ushort AICommandPointer
        {
            get { return this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = value; }
        }

        public SetBossAICommandBossAnimationCommand(ushort offset, ushort aiCommandPointer) : base(BossAnimationCommandType.SetBossAICommand, SetBossAICommandBossAnimationCommand.ParameterMetadata)
        {
            this.Offset = offset;
            this.AICommandPointer = aiCommandPointer;
        }
    }

    public sealed class PlayAnimationBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort AnimationIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public PlayAnimationBossAnimationCommand(ushort animationIndex) : base(BossAnimationCommandType.PlayAnimation, PlayAnimationBossAnimationCommand.ParameterMetadata)
        {
            this.AnimationIndex = animationIndex;
        }
    }

    public sealed class PlaySoundBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort SoundEffectIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public PlaySoundBossAnimationCommand(ushort soundEffectIndex) : base(BossAnimationCommandType.PlaySoundEffect, PlaySoundBossAnimationCommand.ParameterMetadata)
        {
            this.SoundEffectIndex = soundEffectIndex;
        }
    }

    public sealed class SetFrameBankBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.Byte);

        public byte Bank
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        public SetFrameBankBossAnimationCommand(byte bank) : base(BossAnimationCommandType.SetFrameBank, SetFrameBankBossAnimationCommand.ParameterMetadata)
        {
            this.Bank = bank;
        }
    }

    public sealed class EndScriptBossAnimationCommand : BossAnimationCommand
    {
        public EndScriptBossAnimationCommand() : base(BossAnimationCommandType.EndScript, ParameterInfo.Empty) { }
    }

    public sealed class PlayAnimationOnTargetBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.Byte);

        public byte AnimationIndex
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        public PlayAnimationOnTargetBossAnimationCommand(byte animationIndex) : base(BossAnimationCommandType.PlayAnimationOnTarget, PlayAnimationOnTargetBossAnimationCommand.ParameterMetadata)
        {
            this.AnimationIndex = animationIndex;
        }
    }

    public sealed class MoveTargetToCoordinatesBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.SByte, true, ParameterType.SByte);

        public sbyte XCoordinate
        {
            get { return unchecked((sbyte)this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = unchecked((byte)value); }
        }
        public sbyte YCoordinate
        {
            get { return unchecked((sbyte)this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = unchecked((byte)value); }
        }

        public MoveTargetToCoordinatesBossAnimationCommand(sbyte xCoordinate, sbyte yCoordinate) : base(BossAnimationCommandType.MoveTargetToCoordinates, MoveTargetToCoordinatesBossAnimationCommand.ParameterMetadata)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }
    }

    public sealed class SetTargetAnimationOverrideBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte);

        public byte AnimationIndex
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }
        public byte Duration
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }

        public SetTargetAnimationOverrideBossAnimationCommand(byte animationIndex, byte duration) : base(BossAnimationCommandType.SetTargetAnimationOverride, SetTargetAnimationOverrideBossAnimationCommand.ParameterMetadata)
        {
            this.AnimationIndex = animationIndex;
            this.Duration = duration;
        }
    }

    public sealed class SetTargetSpeedBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.Byte, true, ParameterType.Byte, true, ParameterType.Byte);

        public byte XSpeed
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }
        public byte YSpeed
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }
        public byte Duration
        {
            get { return Convert.ToByte(this.Parameters.Parameter3); }
            set { this.Parameters.Parameter3 = value; }
        }

        public SetTargetSpeedBossAnimationCommand(byte xSpeed, byte ySpeed, byte duration) : base(BossAnimationCommandType.SetTargetSpeed, SetTargetSpeedBossAnimationCommand.ParameterMetadata)
        {
            this.XSpeed = xSpeed;
            this.YSpeed = ySpeed;
            this.Duration = duration;
        }
    }

    public sealed class SpawnAnimationObjectBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.SByte, true, ParameterType.SByte);

        public ushort AnimationIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }
        public sbyte XOffset
        {
            get { return unchecked((sbyte)this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = unchecked((byte)value); }
        }
        public sbyte YOffset
        {
            get { return unchecked((sbyte)this.Parameters.Parameter3); }
            set { this.Parameters.Parameter3 = unchecked((byte)value); }
        }

        public SpawnAnimationObjectBossAnimationCommand(ushort animationIndex, sbyte xoffset, sbyte yOffset) : base(BossAnimationCommandType.SpawnAnimationObject, SpawnAnimationObjectBossAnimationCommand.ParameterMetadata)
        {
            this.AnimationIndex = animationIndex;
            this.XOffset = xoffset;
            this.YOffset = yOffset;
        }
    }

    public sealed class ClearWorkRAMBossAnimationCommand : BossAnimationCommand
    {
        public ClearWorkRAMBossAnimationCommand() : base(BossAnimationCommandType.ClearWorkRAM, ParameterInfo.Empty) { }
    }

    public sealed class EnableFlickerBossAnimationCommand : BossAnimationCommand
    {
        public EnableFlickerBossAnimationCommand() : base(BossAnimationCommandType.EnableFlicker, ParameterInfo.Empty) { }
    }

    public sealed class DisableFlickerBossAnimationCommand : BossAnimationCommand
    {
        public DisableFlickerBossAnimationCommand() : base(BossAnimationCommandType.DisableFlicker, ParameterInfo.Empty) { }
    }

    public sealed class Unknown2DBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort Parameter1
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public Unknown2DBossAnimationCommand(ushort parameter1) : base(BossAnimationCommandType.Unknown2D, Unknown2DBossAnimationCommand.ParameterMetadata)
        {
            this.Parameter1 = parameter1;
        }
    }

    public sealed class SetScreenYCoordinateBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort YCoordinate
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public SetScreenYCoordinateBossAnimationCommand(ushort yCoordinate) : base(BossAnimationCommandType.SetScreenYCoordinate, SetScreenYCoordinateBossAnimationCommand.ParameterMetadata)
        {
            this.YCoordinate = yCoordinate;
        }
    }

    public sealed class SetAnimationSyncValueBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort SyncValue
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public SetAnimationSyncValueBossAnimationCommand(ushort syncValue) : base(BossAnimationCommandType.SetAnimationSyncValue, SetAnimationSyncValueBossAnimationCommand.ParameterMetadata)
        {
            this.SyncValue = syncValue;
        }
    }

    public sealed class SpawnAnimationObjectExBossAnimationCommand : BossAnimationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.UInt16, true, ParameterType.SByte, true, ParameterType.SByte);

        public ushort AnimationIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }
        public BossControlFlags ControlFlags
        {
            get { return (BossControlFlags)this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = Convert.ToUInt16(value); }
        }
        public sbyte XOffset
        {
            get { return unchecked((sbyte)this.Parameters.Parameter3); }
            set { this.Parameters.Parameter3 = unchecked((byte)value); }
        }
        public sbyte YOffset
        {
            get { return unchecked((sbyte)this.Parameters.Parameter4); }
            set { this.Parameters.Parameter4 = unchecked((byte)value); }
        }

        public SpawnAnimationObjectExBossAnimationCommand(ushort animationIndex, BossControlFlags controlFlags, sbyte xoffset, sbyte yOffset) : base(BossAnimationCommandType.SpawnAnimationObjectEx, SpawnAnimationObjectExBossAnimationCommand.ParameterMetadata)
        {
            this.AnimationIndex = animationIndex;
            this.ControlFlags = controlFlags;
            this.XOffset = xoffset;
            this.YOffset = yOffset;
        }
    }

    #endregion
}
/*

016B5D: 0D 0A 00                        ;Set Boss Movement Speed: Horizontal: 0A, Vertical: 00.
016BC7: 17 4A00                         ;Play Sound Effect: 004A.
016BB1: 19 C7                           ;Set Bank: C7.
016B4A: 1B                              ;End Script.
016BB3: 1D 24                           ;Play Character Animation 24 On Target.
016BB5: 1F 08 E8                        ;Move Target To Coordinates: X: 08, Y: E8.
017203: 31 06 02                        ;Set Hexas Body Animation: C7.
01723B: 33 CD00                         ;Play Animation: BossAnimationScript_Hexas_SpellSummon.

[1F: BossAnimationScript_Kilroy_Idle]
0167B4: 19 C7                           ;Set Bank: C7.
0167B6: 34CA                            ;Set Frame: BossFrameData_Kilroy_Idle.
0167B8: 00                              ;End Script.

[20: BossAnimationScript_Kilroy_LeftHammerSwing]
0167B9: 19 C7                           ;Set Bank: C7.
0167BB: 76CA                            ;Set Frame: BossFrameData_Kilroy_HammerSwing_01.
0167BD: 03                              ;Set Script Delay: 0x03.
0167BE: B8CA                            ;Set Frame: BossFrameData_Kilroy_HammerSwing_02.
0167C0: 01                              ;Set Script Delay: 0x01.
0167C1: 17 3A00                         ;Play Sound Effect: 003A.
0167C4: FCCA                            ;Set Frame: BossFrameData_Kilroy_HammerSwing_03.
0167C6: 03                              ;Set Script Delay: 0x03.
0167C7: FCCA                            ;Set Frame: BossFrameData_Kilroy_HammerSwing_03.
0167C9: 00                              ;End Script.

[23: BossAnimationScript_Kilroy_WalkDownView]
0167E9: 19 C7                           ;Set Bank: C7.
0167EB: 0D 00 02                        ;Set Boss Movement Speed: Horizontal: 00, Vertical: 02.
0167EE: 34CA                            ;Set Frame: BossFrameData_Kilroy_Idle.
0167F0: 02                              ;Set Script Delay: 0x02.
0167F1: 0D 00 02                        ;Set Boss Movement Speed: Horizontal: 00, Vertical: 02.
0167F4: 7ECC                            ;Set Frame: BossFrameData_Kilroy_WalkDownView_02.
0167F6: 03                              ;Set Script Delay: 0x03.
0167F7: 3CCC                            ;Set Frame: BossFrameData_Kilroy_WalkDownView_01.
0167F9: 01                              ;Set Script Delay: 0x01.
0167FA: 0D 00 02                        ;Set Boss Movement Speed: Horizontal: 00, Vertical: 02.
0167FD: 34CA                            ;Set Frame: BossFrameData_Kilroy_Idle.
0167FF: 02                              ;Set Script Delay: 0x02.
016800: 01                              ;Toggle Horizontal Flip.
016801: 0D 00 02                        ;Set Boss Movement Speed: Horizontal: 00, Vertical: 02.
016804: 7ECC                            ;Set Frame: BossFrameData_Kilroy_WalkDownView_02.
016806: 03                              ;Set Script Delay: 0x03.
016807: 3CCC                            ;Set Frame: BossFrameData_Kilroy_WalkDownView_01.
016809: 01                              ;Set Script Delay: 0x01.
01680A: 01                              ;Toggle Horizontal Flip.
01680B: 1B                              ;End Script.
 */