using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Sprite
{
    [EventOpCodeType(EventOpCodeType.LockSpriteBehavior)]
    [Description("Locks all sprites into their current animations.")]
    public sealed record LockSpriteBehaviorEventOpCode : SpriteEventOpCode
    {
        internal LockSpriteBehaviorEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                               ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                               : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Halt PC Movement And Lock All Sprite Behavior.";
        }
    }
}