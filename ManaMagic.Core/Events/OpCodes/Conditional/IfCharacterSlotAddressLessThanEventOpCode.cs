using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Conditional
{
    [EventOpCodeType(EventOpCodeType.IfCharacterSlotAddressLessThanOrEqual)]
    [Description("Executes the next two bytes of the event if the value at a given character address is less than or equal to the provided value.")]
    public sealed record IfCharacterSlotAddressLessThanEventOpCode : IfCharacterSlotAddressEventOpCode
    {
        internal IfCharacterSlotAddressLessThanEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                           ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                           : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Execute Next Command If Address E1{this.AddressOffset:X2} Of {this.Slot} Is Less Than {this.Value:X2}.";
        }
    }
}
