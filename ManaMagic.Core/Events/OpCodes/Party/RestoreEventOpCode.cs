using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Party
{
    [EventOpCodeType(EventOpCodeType.Restore)]
    [Description("Restores HP or MP to one or more party members.")]
    public sealed record RestoreEventOpCode : PartyEventOpCode
    {
        /// <summary>
        /// Gets or sets the restore action that will be taken.
        /// </summary>
        [Description("The restore action that will be taken.")]
        public RestoreEventOpCodeType RestoreType
        {
            get { return (RestoreEventOpCodeType)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = (byte)value; }
        }

        internal RestoreEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                    ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                    : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            switch (this.RestoreType)
            {
                case RestoreEventOpCodeType.EventActivatorHitPoints: return $"{this.CommandType} Command: Restore Event Activator's Hit Points.";
                case RestoreEventOpCodeType.RandiHitPoints: return $"{this.CommandType} Command: Restore Randi's Hit Points.";
                case RestoreEventOpCodeType.PurimHitPoints: return $"{this.CommandType} Command: Restore Purim's Hit Points.";
                case RestoreEventOpCodeType.PopoieHitPoints: return $"{this.CommandType} Command: Restore Popoie's Hit Points.";
                case RestoreEventOpCodeType.PartyHitPoints: return $"{this.CommandType} Command: Restore Party's Hit Points.";
                case RestoreEventOpCodeType.EventActivatorProcessStatusEffects: return $"{this.CommandType} Command: Process Event Activator's Status Effects.";
                case RestoreEventOpCodeType.RandiProcessStatusEffects: return $"{this.CommandType} Command: Process Randi's Status Effects.";
                case RestoreEventOpCodeType.PurimProcessStatusEffects: return $"{this.CommandType} Command: Process Purim's Status Effects.";
                case RestoreEventOpCodeType.PopoieProcessStatusEffects: return $"{this.CommandType} Command: Process Popoie's Status Effects.";
                case RestoreEventOpCodeType.PartyProcessStatusEffects: return $"{this.CommandType} Command: Process Party's Status Effects.";
                case RestoreEventOpCodeType.EventActivatorManaPoints: return $"{this.CommandType} Command: Restore Event Activator's Mana Points.";
                case RestoreEventOpCodeType.RandiManaPoints: return $"{this.CommandType} Command: Restore Randi's Mana Points.";
                case RestoreEventOpCodeType.PurimManaPoints: return $"{this.CommandType} Command: Restore Purim's Mana Points.";
                case RestoreEventOpCodeType.PopoieManaPoints: return $"{this.CommandType} Command: Restore Popoie's Mana Points.";
                case RestoreEventOpCodeType.PartyManaPoints: return $"{this.CommandType} Command: Restore Party's Mana Points.";
                default: return (string)ThrowHelper.ThrowInvalidOperationException("Unknown Restore Op-Code");
            }
        }
    }
}