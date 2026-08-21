using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Transport
{
    [EventOpCodeType(EventOpCodeType.CannonTravel)]
    [Description("Initiates a Cannon Travel flight sequence.")]
    public sealed record CannonTravelEventOpCode : TransportEventOpCode
    {
        /// <summary>
        /// Gets or sets the set of starting/ending coordinates used by the cannon travel flight.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The set of starting/ending coordinates used by the cannon travel flight.")]
        public CannonTravelCoordinate CoordinateIndex
        {
            get { return (CannonTravelCoordinate)Convert.ToByte(this.Parameters.Parameter1 & 0x3F); }
            set { this.Parameters.Parameter1 = (byte)value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the cannon travel flight should use the flyover effect instead of the normal PC animation.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("Indicates if the cannon travel flight should use the flyover effect instead of the normal PC animation.")]
        public bool FlyOverEffect
        {
            get { return (this.Parameters.Parameter1 & 0x40) > 0; }
            set
            {
                if (value) { this.Parameters.Parameter1 |= 0x40; }
                else { this.Parameters.Parameter1 &= 0xBF; }
            }
        }

        internal CannonTravelEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                         ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                         : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Cannon Travel Flight (Coordinate Set: {this.CoordinateIndex.GetDisplayName()}).";
        }
    }
}