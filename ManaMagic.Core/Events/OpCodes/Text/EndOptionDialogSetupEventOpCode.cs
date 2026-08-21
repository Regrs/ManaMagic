using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.EndOptionDialogSetup)]
    [Description("Completes the setup of a custom dialog.")]
    public sealed record EndOptionDialogSetupEventOpCode : TextEventOpCode
    {
        /// <summary>
        /// Gets or sets a value indicating if the custom dialog can be canceled out of.
        /// </summary>
        [Description("Indicates if the custom dialog can be canceled out of.")]
        public bool CanCancel
        {
            get { return this.Parameters.Parameter1 != 0; }
            set
            {
                this.Parameters.Parameter1 = (ushort)(value ? 1 : 0);
                this.Parameters.SetParameterInfo(EventOpCodeFactory.GetParameterInfo(this.OperationCode, this.Parameters.Parameter1));
            }
        }

        internal EndOptionDialogSetupEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                 ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                 : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }


        /// <inheritdoc />
        public override string ToString()
        {
            string cancelType = this.CanCancel ? "Allowed" : "Disallowed";
            return $"{this.CommandType} Command: End Option Dialog Setup (Cancel {cancelType}).";
        }
    }
}