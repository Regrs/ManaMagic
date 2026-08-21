using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Sprite
{
    [EventOpCodeType(EventOpCodeType.ToggleInvisibilityA)]
    [Description("Toggles the partys invisible flag on or off. This op-code is identical to ToggleInvisibilityB.")]
    public sealed record ToggleInvisibilityAEventOpCode : SpriteEventOpCode
    {
        internal ToggleInvisibilityAEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Toggle Party Invisibility A.";
        }
    }
}