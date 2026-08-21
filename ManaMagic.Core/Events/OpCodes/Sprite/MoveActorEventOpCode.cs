using System;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Sprite
{
    [EventOpCodeType(EventOpCodeType.MoveActor)]
    [Description("Moves an actor in the specified direction.")]
    public sealed record MoveActorEventOpCode : SpriteEventOpCode
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
        /// Gets or sets the distance the actor will move in pixels.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The distance the actor will move in pixels.")]
        public byte Distance
        {
            get { return Convert.ToByte(this.Parameters.Parameter2 & 0x3F); }
            set
            {
                this.Parameters.Parameter2 &= 0x3F;
                this.Parameters.Parameter2 |= (ushort)Math.Min((value & 0x3F), 0x3F);
            }
        }

        /// <summary>
        /// Gets or sets the direction the actor will move.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The direction the actor will move.")]
        public EventOpCodeDirection Direction
        {
            get { return (EventOpCodeDirection)Convert.ToByte(this.Parameters.Parameter2 & 0xC0); }
            set
            {
                this.Parameters.Parameter2 &= 0xC0;
                this.Parameters.Parameter2 |= (byte)value;
            }
        }

        internal MoveActorEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                      ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                      : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Animate Actor: Move Sprite In Character Slot {this.Slot} A Distance Of {this.Distance:X2} Pixels {this.Direction}.";
        }
    }
}