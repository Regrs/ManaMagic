using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.EventFlags
{
    [EventOpCodeType(EventOpCodeType.DecrementEventFlag)]
    [Description("Decrements an event flag.")]
    public sealed record DecrementEventFlagEventOpCode : EventFlagEventOpCode
    {
        /// <summary>
        /// Gets or sets the event flag to be decremented.
        /// </summary>
        [Description("The event flag to be decremented.")]
        public EventFlag Flag
        {
            get { return (EventFlag)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal DecrementEventFlagEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                               ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                               : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType.GetDisplayName()} Command: Decrement Event Flag [{this.Flag.GetDisplayName()}].";
        }
    }
}