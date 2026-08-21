using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Conditional
{
    [EventOpCodeType(EventOpCodeType.IfEventFlagInRange)]
    [Description("Executes the next two bytes of the event if the value of an event flag is between the provided minimum and maximum (Inclusive).")]
    public sealed record IfEventFlagInRangeEventOpCode : ConditionalEventOpCode
    {
        /// <summary>
        /// Gets or sets the event flag to be checked.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The event flag to be checked.")]
        public EventFlag Flag
        {
            get { return (EventFlag)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        /// <summary>
        /// Gets or sets the lower bound of the range to be checked.
        /// </summary>
        [Range(0x00, 0x0F)]
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The lower bound of the range to be checked.")]
        public byte Minimum
        {
            get { return (byte)((this.Parameters.Parameter2 >> 4) & 0x0F); }
            set { this.Parameters.Parameter2 = (byte)(((Math.Min(value, (byte)0x0F) & 0x0F) << 4) | (this.Parameters.Parameter2 & 0x0F)); }
        }

        /// <summary>
        /// Gets or sets the upper bound of the range to be checked.
        /// </summary>
        [Range(0x00, 0x0F)]
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The upper bound of the range to be checked.")]
        public byte Maximum
        {
            get { return (byte)(this.Parameters.Parameter2 & 0x0F); }
            set
            {
                byte t = Math.Min(value, (byte)0x0F);
                this.Parameters.Parameter2 = (byte)((Math.Min(value, (byte)0x0F) & 0x0F) | (this.Parameters.Parameter2 & 0xF0));
            }
        }

        internal IfEventFlagInRangeEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                               ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                               : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Execute Next Command If [{this.Flag.GetDisplayName()}] Is Between {this.Minimum:X2} And {this.Maximum:X2}.";
        }
    }
}