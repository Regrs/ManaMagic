using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.System
{
    /// <summary>
    /// Represents a No Operation op-code.
    /// </summary>
    [EventOpCodeType(EventOpCodeType.NoOperation)]
    [Description("Performs no operation. Used as padding for condition op-codes.")]
    public sealed record NoOperationEventOpCode : SystemEventOpCode
    {
        internal NoOperationEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                        ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                        : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: No Operation.";
        }
    }
}