using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Timing
{
    [EventOpCodeType(EventOpCodeType.WaitForAnimations)]
    [Description("Waits until all actors finish their current animations before continuing the event.")]
    public sealed record WaitForAnimationsEventOpCode : TimingEventOpCode
    {
        internal WaitForAnimationsEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                              ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                              : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Halt Event Processing Until Animations Are Complete.";
        }
    }
}