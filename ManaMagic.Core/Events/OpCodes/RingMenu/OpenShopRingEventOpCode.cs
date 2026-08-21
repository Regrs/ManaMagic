using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.RingMenu
{
    [EventOpCodeType(EventOpCodeType.OpenShopRing)]
    [Description("Opens the shop ring.")]
    public sealed record OpenShopRingEventOpCode : RingMenuEventOpCode
    {
        /// <summary>
        /// Gets or sets the ID of the shop ring to open..
        /// </summary>
        [Description("The ID of the shop ring to open.")]
        public ShopRingType ShopRing
        {
            get { return (ShopRingType)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal OpenShopRingEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                         ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                         : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType.GetDisplayName()} Command: Open Shop Ring: {this.ShopRing}.";
        }
    }
}