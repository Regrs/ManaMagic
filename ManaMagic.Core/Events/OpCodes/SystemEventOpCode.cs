using System.Drawing;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for all event system System op-codes in Secret of Mana.
    /// </summary>
    [EventOpCodeCommandType(EventOpCodeCommandType.System)]
    public abstract record SystemEventOpCode : EventOpCode
    {
        /// <inheritdoc />
        public override Color Color { get { return Color.DarkGray; } }

        internal SystemEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                   ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                   : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }
    }
}