using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Timing
{
    [EventOpCodeType(EventOpCodeType.Wait)]
    [Description("Waits a specified number of ticks before continuing the event.")]
    public sealed record WaitEventOpCode : TimingEventOpCode
    {
        /// <summary>
        /// Gets or sets the number of ticks (a tick is 5 frames) to wait before continuing. A tick count of zero means 'Until Joypad Input'.
        /// </summary>
        [Description("The number of ticks (a tick is 5 frames) to wait before continuing. A tick count of zero means 'Until Joypad Input'.")]
        public byte TickCount
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        internal WaitEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                 ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                 : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.TickCount == 0)
            {
                return $"{this.CommandType} Command: Halt Event Processing Until Joypad Input.";
            }
            return $"{this.CommandType} Command: Halt Event Processing For {this.TickCount:X2} Ticks.";
        }
    }
}