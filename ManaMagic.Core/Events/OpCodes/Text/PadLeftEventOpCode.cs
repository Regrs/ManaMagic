using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.PadLeft)]
    [Description("Right-aligns text by adding padding spaces to the left.")]
    public sealed record PadLeftEventOpCode : TextEventOpCode
    {
        /// <summary>
        /// Gets or sets the number of padding spaces to add.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The number of padding spaces to add.")]
        public byte PaddingAmount
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        internal PadLeftEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Pad Text {this.PaddingAmount:X2}'s Spaces To The Right.";
        }
    }
}