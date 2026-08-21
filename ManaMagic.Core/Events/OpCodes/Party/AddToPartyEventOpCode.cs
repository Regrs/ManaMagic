using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Party
{
    [EventOpCodeType(EventOpCodeType.AddToParty)]
    [Description("Adds a character to the party.")]
    public sealed record AddToPartyEventOpCode : PartyEventOpCode
    {
        /// <summary>
        /// Gets or sets the character to be added.
        /// </summary>
        [Description("The character to be added.")]
        public EventOpCodePartyMember Character
        {
            get { return (EventOpCodePartyMember)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal AddToPartyEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                       ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                       : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Add {this.Character} To The Party.";
        }
    }
}