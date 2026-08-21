using System;
using System.ComponentModel;
using ManaMagic.Core.Metadata;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.System
{
    /// <summary>
    /// Represents a Jump To Event op-code.
    /// </summary>
    [EventOpCodeType(EventOpCodeType.JumpToEvent1)]
    [Description("Jumps to an event. The current event will end.")]
    public sealed record JumpToEventOpCode : SystemEventOpCode
    {
        /// <summary>
        /// Gets the low byte of the Event ID.
        /// </summary>
        [Browsable(false)]
        public byte EventIdLow
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [RefreshProperties(RefreshProperties.All)]
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The ID of the event to call.")]
        public ushort EventId
        {
            get { return (ushort)((((byte)this.OperationCode & 0x07) << 8) | this.EventIdLow); }
            set
            {
                this.OperationCode = (EventOpCodeType)(Convert.ToByte((value & 0xFF00) >> 8) | 0x10);
                this.EventIdLow = Convert.ToByte(value & 0xFF);
            }
        }

        internal JumpToEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                   ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                   : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Jump To Event: {ManaMetadata.GetEventFriendlyName(this.EventId)}.";
        }
    }
}