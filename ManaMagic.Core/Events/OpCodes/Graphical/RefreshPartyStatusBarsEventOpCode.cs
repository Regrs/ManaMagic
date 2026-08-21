using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Graphical
{
    [EventOpCodeType(EventOpCodeType.RefreshPartyStatusBars)]
    [Description("Refreshes the status bars.")]
    public sealed record RefreshPartyStatusBarsEventOpCode : GraphicalEventOpCode
    {
        internal RefreshPartyStatusBarsEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                   ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                   : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Refresh Party Status Bars.";
        }
    }
}