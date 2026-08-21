using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.EventFlags
{
    [EventOpCodeType(EventOpCodeType.SetEventFlag)]
    [Description("Sets an event flag to a value.")]
    public sealed record SetEventFlagEventOpCode : EventFlagEventOpCode
    {
        /// <summary>
        /// Gets or sets the event flag to be set.
        /// </summary>
        [Description("The event flag to be set.")]
        public EventFlag Flag
        {
            get { return (EventFlag)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        /// <summary>
        /// Gets or sets the value the event flag will be set to.
        /// </summary>
        [Description("The value the event flag will be set to.")]
        public byte Value
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = Math.Min(value, (byte)0x0F); }
        }

        internal SetEventFlagEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                         ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                         : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType.GetDisplayName()} Command: Set Event Flag [{this.Flag.GetDisplayName()}] to {this.Value:X2}.";
        }
    }
}