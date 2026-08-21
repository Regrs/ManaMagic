using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Party
{
    [EventOpCodeType(EventOpCodeType.RemoveFromParty)]
    [Description("Removes a character from the party.")]
    public sealed record RemoveFromPartyEventOpCode : PartyEventOpCode
    {
        /// <summary>
        /// Gets or sets the character to be removed.
        /// </summary>
        [Description("The character to be removed.")]
        public EventOpCodePartyMember Character
        {
            get { return (EventOpCodePartyMember)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal RemoveFromPartyEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                            ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                            : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Remove {this.Character} From The Party.";
        }
    }
}