using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.System
{
    /// <summary>
    /// Represents an End Event op-code.
    /// </summary>
    [EventOpCodeType(EventOpCodeType.End)]
    [Description("Ends an event.")]
    public sealed record EndEventOpCode : SystemEventOpCode
    {
        internal EndEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: End Of Event.";
        }
    }
}