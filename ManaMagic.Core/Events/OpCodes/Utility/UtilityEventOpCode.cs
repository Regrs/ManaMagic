using System;
using System.ComponentModel;
using System.Drawing;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Utility
{
    [EventOpCodeType(EventOpCodeType.Utility)]
    [EventOpCodeCommandType(EventOpCodeCommandType.Utility)]
    [Description("A utility command with various different effects.")]
    public sealed record UtilityEventOpCode : EventOpCode
    {
        /// <inheritdoc />
        public override Color Color { get { return Color.Ivory; } }

        /// <summary>
        /// Gets or sets the utility effect of the op-code.
        /// </summary>
        [Description("The utility effect of the op-code.")]
        public UtilityEventOpCodeType UtilityType
        {
            get { return (UtilityEventOpCodeType)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToByte(value); }
        }

        internal UtilityEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            switch (this.UtilityType)
            {
                case UtilityEventOpCodeType.OpenNameRandiDialog: return $"{this.CommandType} Command: Open Randi's Naming Dialog.";
                case UtilityEventOpCodeType.OpenNamePurimDialog: return $"{this.CommandType} Command: Open Purim's Naming Dialog.";
                case UtilityEventOpCodeType.OpenNamePopoieDialog: return $"{this.CommandType} Command: Open Popoie's Naming Dialog.";
                case UtilityEventOpCodeType.PlayCutsceneSunkenContinentRises: return $"{this.CommandType} Command: Play Cutscene: Sunken Continent Rises.";
                case UtilityEventOpCodeType.PlayCutsceneManaFortressRises: return $"{this.CommandType} Command: Play Cutscene: Mana Fortress Rises.";
                case UtilityEventOpCodeType.DummiedUtility05: return $"{this.CommandType} Command: Dummied: Does Nothing.";
                case UtilityEventOpCodeType.OpenSaveGameDialog: return $"{this.CommandType} Command: Open Save Game Dialog.";
                case UtilityEventOpCodeType.ForceGameReset: return $"{this.CommandType} Command: Force Game Reset.";
                case UtilityEventOpCodeType.UpdateEquippedWeapons: return $"{this.CommandType} Command: Update Equipped Weapons.";
                case UtilityEventOpCodeType.ProcessChestContents: return $"{this.CommandType} Command: Process Chest Contents.";
                case UtilityEventOpCodeType.PrepareForWhipPostJump: return $"{this.CommandType} Command: Prepare For Whip Post Jump.";
                case UtilityEventOpCodeType.DisplayEndGameSlide: return $"{this.CommandType} Command: Display End Game Slide.";
                default: return (string)ThrowHelper.ThrowInvalidOperationException("Unknown Utility Op-Code");
            }
        }
    }
}