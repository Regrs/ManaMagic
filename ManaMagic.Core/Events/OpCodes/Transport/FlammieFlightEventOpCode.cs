using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Transport
{
    [EventOpCodeType(EventOpCodeType.FlammieFlight)]
    [Description("Transitions to Flammie Flight Mode, with the starting point at the specified coordinate set.")]
    public sealed record FlammieFlightEventOpCode : TransportEventOpCode
    {
        [Category(EventOpCode.ParametersCategoryName)]
        public byte CoordinateSet
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        internal FlammieFlightEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                          ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                          : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Flammie Flight (Coordinate Set: {this.CoordinateSet:X2}).";
        }
    }
}