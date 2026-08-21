using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.OpenCurrencyWindow)]
    [Description("Opens the currency dialog window.")]
    public sealed record OpenCurrencyWindowEventOpCode : TextEventOpCode
    {
        internal OpenCurrencyWindowEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                               ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                               : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Open Currency Window.";
        }
    }
}