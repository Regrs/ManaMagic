using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Sprite
{
    [EventOpCodeType(EventOpCodeType.UnlockSpriteBehavior)]
    [Description("Unlocks all sprites, allowing normal behavior.")]
    public sealed record UnlockSpriteBehaviorEventOpCode : SpriteEventOpCode
    {
        internal UnlockSpriteBehaviorEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                 ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                 : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Unlock All Sprite Behavior.";
        }
    }
}