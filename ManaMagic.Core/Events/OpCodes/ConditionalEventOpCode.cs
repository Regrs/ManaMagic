using System.Drawing;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for all event system Conditional op-codes in Secret of Mana.
    /// </summary>
    [EventOpCodeCommandType(EventOpCodeCommandType.Conditional)]
    public abstract record ConditionalEventOpCode : EventOpCode
    {
        /// <inheritdoc />
        public override Color Color { get { return Color.Yellow; } }

        internal ConditionalEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                        ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                        : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }
    }
}