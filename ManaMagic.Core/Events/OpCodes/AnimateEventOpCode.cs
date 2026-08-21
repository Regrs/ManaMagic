using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes
{
    /// <summary>
    /// Represents the base class for sprite animation op-codes in Secret of Mana.
    /// </summary>
    public abstract record AnimateEventOpCode : SpriteEventOpCode
    {
        /// <summary>
        /// Gets or sets the sprite slot affected by this op-code.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The sprite slot affected by this op-code.")]
        public EventCharacterSlot Slot
        {
            get { return (EventCharacterSlot)Convert.ToByte(this.Parameters.Parameter1); }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        /// <summary>
        /// Gets the ID of the animation to be played on the actor.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The ID of the animation to be played on the actor.")]
        public byte AnimationIndex
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }

        internal AnimateEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }
    }
}