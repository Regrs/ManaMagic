using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.RingMenu
{
    [EventOpCodeType(EventOpCodeType.OpenSellItemRing)]
    [Description("Opens the sell item ring.")]
    public sealed record OpenSellItemRingEventOpCode : RingMenuEventOpCode
    {
        internal OpenSellItemRingEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                             ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                             : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType.GetDisplayName()} Command: Open Sell Item Ring.";
        }
    }
}