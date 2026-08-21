using System;
using System.Diagnostics;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Bosses.BossInitialization
{
    /// <summary>
    /// Represents the base class for a boss load command.
    /// </summary>
    [DebuggerDisplay("{CommandType}")]
    public abstract class BossInitializationCommand
    {
        /// <summary>
        /// Gets the <see cref="BossInitializationOpCodeType"/> of the command.
        /// </summary>
        public BossInitializationOpCodeType CommandType { get; }
        /// <summary>
        /// Gets the <see cref="ParameterArray"/> of the command.
        /// </summary>
        public ParameterArray Parameters { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="BossInitializationCommand"/> with the specified <see cref="BossInitializationOpCodeType"/> and <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="commandType">The <see cref="BossInitializationOpCodeType"/> of the boss load command.</param>
        /// <param name="parameterInfo">A <see cref="ParameterInfo"/> detailing how the command uses its parameters.</param>
        public BossInitializationCommand(BossInitializationOpCodeType commandType, ParameterInfo parameterInfo)
        {
            this.CommandType = commandType;
            this.Parameters = new ParameterArray(parameterInfo);
        }
    }

    public sealed class LoadPaletteIntoSlot01BossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.UInt16);

        public ushort PaletteIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }
        public BossPaletteSlot Slot
        {
            get { return (BossPaletteSlot)this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = (ushort)value; }
        }

        public LoadPaletteIntoSlot01BossInitializationCommand(ushort paletteIndex, ushort slot) : base(BossInitializationOpCodeType.LoadPaletteIntoSlot01, LoadPaletteIntoSlot01BossInitializationCommand.ParameterMetadata)
        {
            this.PaletteIndex = paletteIndex;
            this.Slot = (BossPaletteSlot)slot;
            if (!Enum.IsDefined<BossPaletteSlot>(this.Slot))
            {
                ThrowHelper.ThrowInvalidOperationException("Unknown Load Slot");
            }
        }
    }

    public sealed class CreateBossObjectBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.UInt16);

        public BossControlFlags ControlFlags
        {
            get { return (BossControlFlags)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = (ushort)value; }
        }

        public ushort StateMachinePointer
        {
            get { return this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = value; }
        }

        public CreateBossObjectBossInitializationCommand(ushort controlFlags, ushort stateMachinePointer) : base(BossInitializationOpCodeType.CreateBossObject, CreateBossObjectBossInitializationCommand.ParameterMetadata)
        {
            this.ControlFlags = (BossControlFlags)controlFlags;
            this.StateMachinePointer = stateMachinePointer;
        }
    }

    public sealed class LoadGraphics11BossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort GraphicsIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public LoadGraphics11BossInitializationCommand(ushort graphicsIndex) : base(BossInitializationOpCodeType.LoadGraphics11, LoadGraphics11BossInitializationCommand.ParameterMetadata)
        {
            this.GraphicsIndex = graphicsIndex;
        }
    }

    public sealed class LoadHexasGraphicsBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort GraphicsIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public LoadHexasGraphicsBossInitializationCommand(ushort graphicsIndex) : base(BossInitializationOpCodeType.LoadHexasGraphics, LoadHexasGraphicsBossInitializationCommand.ParameterMetadata)
        {
            this.GraphicsIndex = graphicsIndex;
        }
    }

    public sealed class StoreValueAtAddressBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.UInt16);

        public ushort Offset
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }
        public ushort Value
        {
            get { return this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = value; }
        }

        public StoreValueAtAddressBossInitializationCommand(ushort offset, ushort value) : base(BossInitializationOpCodeType.StoreValueAtAddress, StoreValueAtAddressBossInitializationCommand.ParameterMetadata)
        {
            this.Offset = offset;
            this.Value = value;
        }
    }

    public sealed class InitializeCustomStateMachineBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16, true, ParameterType.UInt16);

        public BossControlFlags ControlFlags
        {
            get { return (BossControlFlags)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = (ushort)value; }
        }

        public ushort StateMachinePointer
        {
            get { return this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = value; }
        }

        public InitializeCustomStateMachineBossInitializationCommand(ushort controlFlags, ushort stateMachinePointer) : base(BossInitializationOpCodeType.InitializeCustomStateMachineBoss, InitializeCustomStateMachineBossInitializationCommand.ParameterMetadata)
        {
            this.ControlFlags = (BossControlFlags)controlFlags;
            this.StateMachinePointer = stateMachinePointer;
        }
    }

    public sealed class ClearWRAMSectionBC00BossInitializationCommand : BossInitializationCommand
    {
        public ClearWRAMSectionBC00BossInitializationCommand() : base(BossInitializationOpCodeType.ClearWRAMSectionBC00, ParameterInfo.Empty) { }
    }

    public sealed class InitializeStandardStateMachineBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort AIIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public InitializeStandardStateMachineBossInitializationCommand(ushort aiIndex) : base(BossInitializationOpCodeType.InitializeStandardStateMachineBoss, InitializeStandardStateMachineBossInitializationCommand.ParameterMetadata)
        {
            this.AIIndex = aiIndex;
        }
    }

    public sealed class ClearWRAMSectionBC00AndVRAMWithDMABossInitializationCommand : BossInitializationCommand
    {
        public ClearWRAMSectionBC00AndVRAMWithDMABossInitializationCommand() : base(BossInitializationOpCodeType.ClearWRAMSectionBC00AndVRAMWithDMA, ParameterInfo.Empty) { }
    }

    public sealed class CallRoutineBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort GraphicsIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public CallRoutineBossInitializationCommand(ushort graphicsIndex) : base(BossInitializationOpCodeType.CallRoutine, CallRoutineBossInitializationCommand.ParameterMetadata)
        {
            this.GraphicsIndex = graphicsIndex;
        }
    }

    public sealed class LoadAndDecompressBossGraphicsBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort GraphicsIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public LoadAndDecompressBossGraphicsBossInitializationCommand(ushort graphicsIndex) : base(BossInitializationOpCodeType.LoadAndDecompressBossGraphics, LoadAndDecompressBossGraphicsBossInitializationCommand.ParameterMetadata)
        {
            this.GraphicsIndex = graphicsIndex;
        }
    }

    public sealed class LoadAndDecompressSlimeGraphicsBossInitializationCommand : BossInitializationCommand
    {
        private static readonly ParameterInfo ParameterMetadata = new ParameterInfo(true, ParameterType.UInt16);

        public ushort GraphicsIndex
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        public LoadAndDecompressSlimeGraphicsBossInitializationCommand(ushort graphicsIndex) : base(BossInitializationOpCodeType.LoadAndDecompressSlimeGraphics, LoadAndDecompressSlimeGraphicsBossInitializationCommand.ParameterMetadata)
        {
            this.GraphicsIndex = graphicsIndex;
        }
    }

    public sealed class EndScriptBossInitializationCommand : BossInitializationCommand
    {
        public EndScriptBossInitializationCommand() : base(BossInitializationOpCodeType.End, ParameterInfo.Empty) { }
    }
}