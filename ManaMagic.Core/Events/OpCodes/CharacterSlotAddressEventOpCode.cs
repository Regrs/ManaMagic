using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for character slot address op-codes in Secret of Mana.
    /// </summary>
    public abstract record CharacterSlotAddressEventOpCode : EventOpCode
    {
        /// <summary>
        /// Gets or sets the sprite slot affected by this op-code.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The sprite slot affected by this op-code.")]
        public EventCharacterSlot Slot
        {
            get { return (EventCharacterSlot)Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        /// <summary>
        /// Gets or sets the address offset that is affected by this op-code.
        /// </summary>
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The address offset that is affected by this op-code.")]
        public byte AddressOffset
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }

        /// <summary>
        /// Gets or sets the value the sprite address will be checked against.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The value the sprite address will be checked against.")]
        public byte Value
        {
            get { return Convert.ToByte(this.Parameters.Parameter3); }
            set { this.Parameters.Parameter3 = value; }
        }

        internal CharacterSlotAddressEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                 ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                 : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }
    }
}