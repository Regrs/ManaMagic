using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Inventory
{
    [EventOpCodeType(EventOpCodeType.AddGold)]
    [Description("Adds gold to the players inventory.")]
    public sealed record AddGoldEventOpCode : InventoryEventOpCode
    {
        /// <summary>
        /// Gets or sets the amount of gold to add.
        /// </summary>
        [Description("The amount of gold to add.")]
        public ushort Amount
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        internal AddGoldEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Add {this.Amount} Gold.";
        }
    }
}