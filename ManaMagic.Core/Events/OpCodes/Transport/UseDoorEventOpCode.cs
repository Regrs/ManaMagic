using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManaMagic.Core.Metadata;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Transport
{
    [EventOpCodeType(EventOpCodeType.UseDoor1)]
    [Description("Uses a door.")]
    public sealed record UseDoorEventOpCode : TransportEventOpCode // 421
    {
        /// <summary>
        /// Gets or sets the low byte of the door ID.
        /// </summary>
        [Browsable(false)]
        public byte DoorIdLow
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the door to be used.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The ID of the door to be used.")]
        [RefreshProperties(RefreshProperties.All)]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort DoorId
        {
            get { return (ushort)((((byte)this.OperationCode & 0x03) << 8) | this.DoorIdLow); }
            set
            {
                this.OperationCode = (EventOpCodeType)(Convert.ToByte((value & 0xFF00) >> 8) | 0x18);//fc00
                this.DoorIdLow = Convert.ToByte(value & 0xFF);
            }
        }

        internal UseDoorEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Use Door: {ManaMetadata.GetDoorFriendlyName(this.DoorId)}.";
        }
    }
}