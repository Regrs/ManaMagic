using System.Drawing;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for conditional character slot address op-codes in Secret of Mana.
    /// </summary>
    [EventOpCodeCommandType(EventOpCodeCommandType.Conditional)]
    public abstract record IfCharacterSlotAddressEventOpCode : CharacterSlotAddressEventOpCode
    {
        /// <inheritdoc />
        public override Color Color { get { return Color.Yellow; } }

        internal IfCharacterSlotAddressEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                   ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                   : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }
    }
}