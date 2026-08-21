using System.Drawing;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for all event system Transport op-codes in Secret of Mana.
    /// </summary>
    [EventOpCodeCommandType(EventOpCodeCommandType.Transport)]
    public abstract record TransportEventOpCode : EventOpCode
    {
        /// <inheritdoc />
        public override Color Color { get { return Color.Green; } }

        internal TransportEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                      ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                      : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }
    }
}