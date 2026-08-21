using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Sprite
{
    [EventOpCodeType(EventOpCodeType.AnimateActorSingle)]
    [Description("Plays an animation on an actor.")]
    public sealed record AnimateActorSingleEventOpCode : AnimateEventOpCode
    {
        internal AnimateActorSingleEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                               ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                               : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Animate Actor: Play Animation {this.AnimationIndex:X2} On Character Slot {this.Slot}.";
        }
    }
}