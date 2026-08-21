using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Inventory
{
    [EventOpCodeType(EventOpCodeType.SubtractGold)]
    [Description("Removes gold from the players inventory.")]
    public sealed record SubtractGoldEventOpCode : InventoryEventOpCode
    {
        /// <summary>
        /// Gets or sets the amount of gold to remove.
        /// </summary>
        [Description("The amount of gold to remove.")]
        public ushort Amount
        {
            get { return this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = value; }
        }

        internal SubtractGoldEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                         ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                         : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Subtract {this.Amount} Gold.";
        }
    }
}