using System;
using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Graphical
{
    [EventOpCodeType(EventOpCodeType.ScreenEffect)]
    [Description("Invokes a screen wide effect.")]
    public sealed record ScreenEffectEventOpCode : GraphicalEventOpCode
    {
        /// <summary>
        /// Gets or sets the screen effect to invoke.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The screen effect to invoke.")]
        public ScreenEffectEventOpCodeType Effect
        {
            get { return (ScreenEffectEventOpCodeType)this.Parameters.Parameter1; }
            set
            { 
                this.Parameters.Parameter1 = Convert.ToByte(value);
                this.Parameters.SetParameterInfo(EventOpCodeFactory.GetParameterInfo(this.OperationCode, this.Parameters.Parameter1));
            }
        }

        /// <summary>
        /// Gets or sets the palette used for the PaletteFilter and PaletteFlash effects.
        /// </summary>
        [Category(EventOpCode.ParametersCategoryName)]
        [Description("The palette used for the PaletteFilter and PaletteFlash effects.")]
        public ushort Palette
        {
            get { return this.Parameters.Parameter2; }
            set { this.Parameters.Parameter2 = value; }
        }

        internal ScreenEffectEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                        ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                        : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            switch (this.Effect)
            {
                case ScreenEffectEventOpCodeType.WhiteStrobeEffect: return $"{this.CommandType} Command: Begin White Strobe Flash Effect.";
                case ScreenEffectEventOpCodeType.EndWhiteStrobeEffect: return $"{this.CommandType} Command: End White Strobe Flash Effect.";
                case ScreenEffectEventOpCodeType.VerticalEarthquakeEffect: return $"{this.CommandType} Command: Vertical Earthquake Effect.";
                case ScreenEffectEventOpCodeType.HorizontalEarthquakeEffect: return $"{this.CommandType} Command: Horizontal Earthquake Effect.";
                case ScreenEffectEventOpCodeType.EndEarthquakeEffect: return $"{this.CommandType} Command: End Earthquake Effect.";
                case ScreenEffectEventOpCodeType.PaletteFilter: return $"{this.CommandType} Command: Palette Filter: {this.Palette:X4}.";
                case ScreenEffectEventOpCodeType.PaletteFlash: return $"{this.CommandType} Command: Palette Flash: {this.Palette:X4}.";
                case ScreenEffectEventOpCodeType.RestoreMapDefaultPalette: return $"{this.CommandType} Command: Restore Map Default Palette.";
                case ScreenEffectEventOpCodeType.CenterCameraOnEventActivator: return $"{this.CommandType} Command: Center Camera On Event Activator.";
                default: return (string)ThrowHelper.ThrowInvalidOperationException("Unknown Screen Effect Op-Code");
            }
        }

        internal static bool HasAdditionalOperand(ScreenEffectEventOpCodeType opCode)
        {
            return opCode == ScreenEffectEventOpCodeType.PaletteFilter || opCode == ScreenEffectEventOpCodeType.PaletteFlash;
        }
    }
}