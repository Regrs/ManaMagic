using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.PrintCurrency)]
    [Description("Prints the current gold value to the currency dialog window.")]
    public sealed record PrintCurrencyEventOpCode : TextEventOpCode
    {
        internal PrintCurrencyEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                          ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                         : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Print Currency To Currency Window.";
        }
    }
}