using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    internal sealed class AkaoVoice
    {
        private readonly Stack<LoopTracker> loopStack = new Stack<LoopTracker>();

        public VoiceEnable VoiceEnable { get; }
        public DSPRegisters.VoiceDSPRegisters Registers { get; }
        public bool HasActiveLoops { get { return this.loopStack.Count != 0; } }
        public int WaveIndexAtGoto { get; set; } = 0;

        [MemberNotNullWhen(false, nameof(AkaoVoice.Channel))]
        public bool EndOfSequence { get { return this.Channel == null || this.SequenceIndex >= this.Channel.Sequence.Count; } }

        [MemberNotNullWhen(true, nameof(AkaoVoice.Channel))]
        public bool CanRead { get { return this.Channel != null && this.Channel.Enabled && !this.EndOfSequence; } }
        public AkaoChannel? Channel { get; private set; }
        public int SequenceIndex { get; set; } = 0;

        public byte InstrumentIndex { get; set; } = 0;
        public byte Octave { get; set; } = 0;
        public byte Transpose { get; set; } = 0;
        public sbyte Volume { get; set; } = 0;
        public byte Detune { get; set; } = 0;
        public byte Pan { get; set; } = 0;
        public AkaoVibrato Vibrato { get; } = new AkaoVibrato();
        public AkaoPanLfo PanLFO { get; } = new AkaoPanLfo();

        public ushort Pitch { get; set; } = 0;
        public SMPCountdownTimer DurationTimer { get; } = new SMPCountdownTimer(1, 0);
        public bool Playing { get { return !this.DurationTimer.Expired; } }

        public AkaoVoice(VoiceEnable voiceEnable, DSPRegisters.VoiceDSPRegisters registers)
        {
            this.VoiceEnable = voiceEnable;
            this.Registers = registers;
        }

        public void Initialize(AkaoChannel channel)
        {
            this.Channel = channel;
            this.SequenceIndex = 0;

            this.InstrumentIndex = 0;
            this.Octave = 0;
            this.Transpose = 0;
            this.Volume = 0;
            this.Detune = 0;
            this.Pan = 0;

            this.Pitch = 0;

            // If this voice is enabled then its initial duration will be set to 1.
            // This will force the sequence parser to run for the voice on the first tempo tick and queue a note.
            this.DurationTimer.Target = (byte)(this.CanRead ? 1 : 0);
            this.DurationTimer.Reset();
        }

        public void StartLoop(int loopCount)
        {
            if (loopCount == 0) { ThrowHelper.ThrowNotImplementedException("Infinite Akao Loops are not implemented."); }

            LoopTracker loopTracker = new LoopTracker();
            loopTracker.LoopIndex = this.SequenceIndex;
            loopTracker.LoopCount = loopCount;
            this.loopStack.Push(loopTracker);
        }

        public void EndLoop()
        {
            LoopTracker loopTracker = this.loopStack.Peek();
            loopTracker.LoopCount--;

            if (loopTracker.LoopCount == 0)
            {
                this.loopStack.Pop();
                return;
            }
            this.SequenceIndex = loopTracker.LoopIndex;
        }

        private sealed class LoopTracker
        {
            public int LoopIndex { get; set; }
            public int LoopCount { get; set; }
        }
    }
}