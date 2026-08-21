using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.SetOptionDialogOption)]
    [Description("Sets a line of text as a custom dialog option.")]
    public sealed record SetOptionDialogOptionEventOpCode : TextEventOpCode
    {
        /// <summary>
        /// Gets or sets the number of padding spaces to add to the left of the dialog option.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The number of padding spaces to add to the left of the dialog option.")]
        public byte PaddingAmount
        {
            get { return Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = value; }
        }

        internal SetOptionDialogOptionEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                  ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                  : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Set Option Dialog Option (Offset: {this.PaddingAmount:X2}).";
        }
    }
}