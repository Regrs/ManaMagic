using System.Collections.Generic;
using System.Linq;
using ManaMagic.Core.Events;

#nullable enable

namespace ManaMagic.Core
{
    public sealed class ManaValidator
    {
        private const string SuccessMessage = "Success";
        private readonly List<ManaValidationRecord> results = new List<ManaValidationRecord>();

        public IReadOnlyList<ManaValidationRecord> Results { get { return this.results; } }
        public bool HasErrors { get { return this.Results.Any(p => p.Result == ManaValidationResult.Error); } }

        public void Validate(SecretOfManaContext context)
        {
            this.ValidateBank09Events(context.EventContext);
            this.ValidateBank0AEvents(context.EventContext);
        }

        private void ValidateBank09Events(EventContext context)
        {
            bool hasErrors = false;
            ushort bankSpace = ushort.MaxValue;

            // Reserve the size of the event pointer table.
            bankSpace -= (ushort)(Constants.Bank09.EventIdMaximum + 1) * sizeof(ushort);

            // Starting at C9/FD7C are some routines of an unknown purpose, so we cannot overwrite those.
            // If these bytes end up being valuable then the routines could be relocated.
            // Note: The JP ending is located at C9/F2D7, we can safely overrite this though, leaving these bytes as the only (non-event) reserved ones in the bank.
            bankSpace -= 643;

            int sizeUsed = 0;
            for (int i = (int)Constants.Bank09.EventIdMinimum; i <= Constants.Bank09.EventIdMaximum; i++)
            {
                ManaEvent manaEvent = context.Events[i];
                sizeUsed += manaEvent.Size;

                bool eventEnds = manaEvent.Any(p => p.OperationCode == EventOpCodeType.End);
                if (!eventEnds)
                {
                    this.results.Add(new ManaValidationRecord(ManaValidationType.Bank09Events, ManaValidationResult.Error, $"Event Index: {i:X4} does not contain an 'End' Op-Code."));
                    hasErrors = true;
                }
            }

            if (sizeUsed > bankSpace)
            {
                this.results.Add(new ManaValidationRecord(ManaValidationType.Bank09Events, ManaValidationResult.Error, $"Bank overrun detected. Available Space: {bankSpace}. Used Space: {sizeUsed}"));
                hasErrors = true;
            }

            if (!hasErrors)
            {
                this.results.Add(new ManaValidationRecord(ManaValidationType.Bank09Events, ManaValidationResult.Success, ManaValidator.SuccessMessage));
            }
        }

        private void ValidateBank0AEvents(EventContext context)
        {
            bool hasErrors = false;
            ushort bankSpace = ushort.MaxValue;

            // Reserve the size of the event pointer table.
            bankSpace -= (ushort)(Constants.Bank09.EventIdMaximum + 1) * sizeof(ushort);

            // The other string tables will be moved from this bank so we can reclaim all that space for events.
            // The space remaining is reserved by boss frames. In theory these could be moved too, but it would be very tedious.
            // So for now they remain, but should revisit, this is a lot of space that could be freed up.
            // CA/BE00 - CA/FF5F
            bankSpace -= 16895;

            int sizeUsed = 0;
            for (int i = (int)Constants.Bank0A.EventIdMinimum; i <= Constants.Bank0A.EventIdMaximum; i++)
            {
                ManaEvent manaEvent = context.Events[i];
                sizeUsed += manaEvent.Size;

                bool eventEnds = manaEvent.Any(p => p.OperationCode == EventOpCodeType.End);
                if (!eventEnds)
                {
                    this.results.Add(new ManaValidationRecord(ManaValidationType.Bank0AEvents, ManaValidationResult.Error, $"Event Index: {i:X4} does not contain an 'End' Op-Code."));
                    hasErrors = true;
                }
            }

            if (sizeUsed > bankSpace)
            {
                this.results.Add(new ManaValidationRecord(ManaValidationType.Bank0AEvents, ManaValidationResult.Error, $"Bank overrun detected. Available Space: {bankSpace}. Used Space: {sizeUsed}"));
                hasErrors = true;
            }

            if (!hasErrors)
            {
                this.results.Add(new ManaValidationRecord(ManaValidationType.Bank0AEvents, ManaValidationResult.Success, ManaValidator.SuccessMessage));
            }
        }
    }

    public enum ManaValidationType
    {
        Unknown = 0,
        Bank09Events,
        Bank0AEvents,
    }

    public enum ManaValidationResult
    {
        Success = 0,
        Warning,
        Error,
    }

    public sealed record ManaValidationRecord
    {
        public ManaValidationType ValidationType { get; }
        public ManaValidationResult Result { get; }
        public string Message { get; }

        public ManaValidationRecord(ManaValidationType type, ManaValidationResult result, string message)
        {
            this.ValidationType = type;
            this.Result = result;
            this.Message = message;
        }
    }
}