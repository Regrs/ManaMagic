using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.PrintPlayerName)]
    [Description("Prints a character's name.")]
    public sealed record PrintPlayerNameEventOpCode : TextEventOpCode
    {
        /// <summary>
        /// Gets or sets the character's name to be printed.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The character's name to be printed.")]
        public EventOpCodePartyMember Character
        {
            get { return (EventOpCodePartyMember)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal PrintPlayerNameEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                            ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                            : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }


        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Print {this.Character}'s Name.";
        }
    }
}