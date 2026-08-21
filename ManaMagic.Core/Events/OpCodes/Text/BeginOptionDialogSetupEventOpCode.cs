using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.BeginOptionDialogSetup)]
    [Description("Begins creating an options dialog and places the event parser in Options mode.")]
    public sealed record BeginOptionDialogSetupEventOpCode : TextEventOpCode
    {
        internal BeginOptionDialogSetupEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                   ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                   : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Begin Option Dialog Setup.";
        }
    }
}