using System.ComponentModel;
using System.Drawing;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Sprite
{
    [EventOpCodeType(EventOpCodeType.SetCharacterSlotAddress)]
    [EventOpCodeCommandType(EventOpCodeCommandType.Sprite)]
    [Description("Sets an address in the specified character slot to the specified value.")]
    public sealed record SetCharacterSlotAddressEventOpCode : CharacterSlotAddressEventOpCode
    {
        /// <inheritdoc />
        public override Color Color { get { return Color.MediumSlateBlue; } }

        internal SetCharacterSlotAddressEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Set Address E1{this.AddressOffset:X2} Of {this.Slot} To Value {this.Value:X2}.";
        }
    }
}