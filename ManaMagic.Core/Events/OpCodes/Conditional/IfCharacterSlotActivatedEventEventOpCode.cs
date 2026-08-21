using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Conditional
{
    [EventOpCodeType(EventOpCodeType.IfCharacterSlotActivatedEvent)]
    [Description("Executes the next two bytes of the event if the given character slot is the one that activated the event.")]
    public sealed record IfCharacterSlotActivatedEventEventOpCode : ConditionalEventOpCode
    {
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The sprite slot affected by this op-code.")]
        public EventOpCodePartyMember Slot
        {
            get { return (EventOpCodePartyMember)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal IfCharacterSlotActivatedEventEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                      ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                      : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Execute Next Command If {this.Slot} Activated The Event.";
        }
    }
}