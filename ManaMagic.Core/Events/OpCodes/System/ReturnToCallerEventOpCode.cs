using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.System
{
    /// <summary>
    /// Represents a Return To Caller op-code.
    /// </summary>
    [EventOpCodeType(EventOpCodeType.ReturnToCaller)]
    [Description("Returns event execution to an event that used the CallEvent op-code.")]
    public sealed record ReturnToCallerEventOpCode : SystemEventOpCode
    {
        internal ReturnToCallerEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                           ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                           : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Return To Calling Event.";
        }
    }
}