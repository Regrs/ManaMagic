using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Inventory
{
    [EventOpCodeType(EventOpCodeType.AddItem)]
    [Description("Adds an item to the players inventory.")]
    public sealed record AddItemEventOpCode : InventoryEventOpCode
    {
        /// <summary>
        /// Gets or sets the ID of the item to be added.
        /// </summary>
        [Description("The ID of the item to be added.")]
        public AddItemType Item
        {
            get { return (AddItemType)Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = (byte)value; }
        }

        internal AddItemEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Add Item To Inventory: {this.Item}.";
        }
    }
}