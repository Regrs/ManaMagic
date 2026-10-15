#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public abstract class AkaoMessage
    {
        public AkaoMessageType MessageType { get; }

        public AkaoMessage(AkaoMessageType type)
        {
            this.MessageType = type;
        }

        public override string ToString()
        {
            return this.MessageType.ToString();
        }
    }

    public abstract class BaseNoteAkaoMessage : AkaoMessage
    {
        public byte Note { get; }
        public byte NoteIndex { get { return (byte)(this.Note / 15); } }
        public byte DurationIndex { get { return (byte)(this.Note % 15); } }

        public bool IsNote { get { return this.MessageType == AkaoMessageType.Note; } }
        public bool IsTie { get { return this.MessageType == AkaoMessageType.Tie; } }
        public bool IsRest { get { return this.MessageType == AkaoMessageType.Rest; } }

        public BaseNoteAkaoMessage(byte note, AkaoMessageType type) : base(type)
        {
            this.Note = note;
        }
    }

    public sealed class NoteAkaoMessage : BaseNoteAkaoMessage
    {
        public NoteAkaoMessage(byte note) : base(note, AkaoMessageType.Note) { }

        public override string ToString()
        {
            return $"{base.ToString()} - Note: {this.Note:X2}";
        }
    }

    public sealed class TieAkaoMessage : BaseNoteAkaoMessage
    {
        public TieAkaoMessage(byte tie) : base(tie, AkaoMessageType.Tie) { }

        public override string ToString()
        {
            return $"{base.ToString()} - Tie: {this.Note:X2}";
        }
    }

    public sealed class RestAkaoMessage : BaseNoteAkaoMessage
    {
        public RestAkaoMessage(byte rest) : base(rest, AkaoMessageType.Rest) { }

        public override string ToString()
        {
            return $"{base.ToString()} - Rest: {this.Note:X2}";
        }
    }

    public sealed class ChannelVolumeAkaoMessage : AkaoMessage
    {
        public byte Volume { get; }

        public ChannelVolumeAkaoMessage(byte volume) : base(AkaoMessageType.ChannelVolume)
        {
            this.Volume = volume;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Volume: {this.Volume:X2}";
        }
    }

    public sealed class ChannelVolumeFadeAkaoMessage : AkaoMessage
    {
        public byte Duration { get; }
        public byte Volume { get; }

        public ChannelVolumeFadeAkaoMessage(byte duration, byte volume) : base(AkaoMessageType.ChannelVolumeFade)
        {
            this.Duration = duration;
            this.Volume = volume;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Duration: {this.Duration:X2}, Target Volume: {this.Volume:X2}";
        }
    }

    public sealed class PanAkaoMessage : AkaoMessage
    {
        public byte Position { get; }//; b1: pan value (01 = left, 40 = center, 7f = right, top bit inactive)

        public PanAkaoMessage(byte position) : base(AkaoMessageType.Pan)
        {
            this.Position = position;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Position: {this.Position:X2}";
        }
    }

    public sealed class PanFadeAkaoMessage : AkaoMessage
    {
        public byte Duration { get; }
        public byte Position { get; }

        public PanFadeAkaoMessage(byte duration, byte position) : base(AkaoMessageType.PanFade)
        {
            this.Duration = duration;
            this.Position = position;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Duration: {this.Duration:X2}, Target Position: {this.Position:X2}";
        }
    }

    public sealed class PitchSlideAkaoMessage : AkaoMessage
    {
        public byte Duration { get; }
        public sbyte Pitch { get; }

        public PitchSlideAkaoMessage(byte duration, sbyte pitch) : base(AkaoMessageType.PitchSlide)
        {
            this.Duration = duration;
            this.Pitch = pitch;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Duration: {this.Duration:X2}, Target Pitch: {this.Pitch:X2}";
        }
    }

    public sealed class VibratoOnAkaoMessage : AkaoMessage
    {
        public byte Delay { get; }
        public byte Duration { get; }
        public byte Amplitude { get; }

        public VibratoOnAkaoMessage(byte delay, byte duration, byte amplitude) : base(AkaoMessageType.VibratoOn)
        {
            this.Delay = delay;
            this.Duration = duration;
            this.Amplitude = amplitude;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Delay: {this.Delay:X2}, Duration: {this.Duration:X2}, Amplitude: {this.Amplitude:X2}";
        }
    }

    public sealed class VibratoOffAkaoMessage : AkaoMessage
    {
        public VibratoOffAkaoMessage() : base(AkaoMessageType.VibratoOff) { }
    }

    public sealed class TremoloOnAkaoMessage : AkaoMessage
    {
        public byte Delay { get; }
        public byte Rate { get; }
        public byte Depth { get; }

        public TremoloOnAkaoMessage(byte delay, byte rate, byte depth) : base(AkaoMessageType.TremoloOn)
        {
            this.Delay = delay;
            this.Rate = rate;
            this.Depth = depth;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Delay: {this.Delay:X2}, Rate: {this.Rate:X2}, Depth: {this.Depth:X2}";
        }
    }

    public sealed class TremoloOffAkaoMessage : AkaoMessage
    {
        public TremoloOffAkaoMessage() : base(AkaoMessageType.TremoloOff) { }
    }

    public sealed class PanLFOOnAkaoMessage : AkaoMessage
    {
        public byte Duration { get; }
        public sbyte Amplitude { get; }

        public PanLFOOnAkaoMessage(byte duration, sbyte amplitude) : base(AkaoMessageType.PanLFOOn)
        {
            this.Duration = duration;
            this.Amplitude = amplitude;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Duration: {this.Duration:X2}, Amplitude: {this.Amplitude:X2}";
        }
    }

    public sealed class PanLFOOffAkaoMessage : AkaoMessage
    {
        public PanLFOOffAkaoMessage() : base(AkaoMessageType.PanLFOOff) { }
    }

    public sealed class NoiseFrequencyAkaoMessage : AkaoMessage
    {
        public byte Frequency { get; }

        public NoiseFrequencyAkaoMessage(byte frequency) : base(AkaoMessageType.NoiseFrequency)
        {
            this.Frequency = frequency;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Frequency: {this.Frequency:X2}";
        }
    }

    public sealed class NoiseOnAkaoMessage : AkaoMessage
    {
        public NoiseOnAkaoMessage() : base(AkaoMessageType.NoiseOn) { }
    }

    public sealed class NoiseOffAkaoMessage : AkaoMessage
    {
        public NoiseOffAkaoMessage() : base(AkaoMessageType.NoiseOff) { }
    }

    public sealed class PitchModOnAkaoMessage : AkaoMessage
    {
        public PitchModOnAkaoMessage() : base(AkaoMessageType.PitchModOn) { }
    }

    public sealed class PitchModOffAkaoMessage : AkaoMessage
    {
        public PitchModOffAkaoMessage() : base(AkaoMessageType.PitchModOff) { }
    }

    public sealed class EchoOnAkaoMessage : AkaoMessage
    {
        public EchoOnAkaoMessage() : base(AkaoMessageType.EchoOn) { }
    }

    public sealed class EchoOffAkaoMessage : AkaoMessage
    {
        public EchoOffAkaoMessage() : base(AkaoMessageType.EchoOff) { }
    }

    public sealed class OctaveAkaoMessage : AkaoMessage
    {
        public byte Octave { get; }

        public OctaveAkaoMessage(byte octave) : base(AkaoMessageType.Octave)
        {
            this.Octave = octave;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Octave: {this.Octave:X2}";
        }
    }

    public sealed class OctaveUpAkaoMessage : AkaoMessage
    {
        public OctaveUpAkaoMessage() : base(AkaoMessageType.OctaveUp) { }
    }

    public sealed class OctaveDownAkaoMessage : AkaoMessage
    {
        public OctaveDownAkaoMessage() : base(AkaoMessageType.OctaveDown) { }
    }

    public sealed class TransposeAkaoMessage : AkaoMessage
    {
        public byte Step { get; }

        public TransposeAkaoMessage(byte step) : base(AkaoMessageType.Transpose)
        {
            this.Step = step;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Step: {this.Step:X2}";
        }
    }

    public sealed class TransposeAdjustAkaoMessage : AkaoMessage
    {
        public byte StepAdjustment { get; }

        public TransposeAdjustAkaoMessage(byte stepAdjustment) : base(AkaoMessageType.TransposeAdjust)
        {
            this.StepAdjustment = stepAdjustment;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Step: {this.StepAdjustment:X2}";
        }
    }

    public sealed class DetuneAkaoMessage : AkaoMessage
    {
        public byte Detune { get; }

        public DetuneAkaoMessage(byte detune) : base(AkaoMessageType.Detune)
        {
            this.Detune = detune;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Detune: {this.Detune:X2}";
        }
    }

    public sealed class InstrumentChangeAkaoMessage : AkaoMessage
    {
        public byte Index { get; }

        public InstrumentChangeAkaoMessage(byte index) : base(AkaoMessageType.InstrumentChange)
        {
            this.Index = index;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Instrument: {this.Index:X2}";
        }
    }

    public sealed class EnvelopeAttackAkaoMessage : AkaoMessage
    {
        public byte Rate { get; }

        public EnvelopeAttackAkaoMessage(byte rate) : base(AkaoMessageType.EnvelopeAttack)
        {
            this.Rate = rate;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Rate: {this.Rate:X2}";
        }
    }

    public sealed class EnvelopeDecayAkaoMessage : AkaoMessage
    {
        public byte Rate { get; }

        public EnvelopeDecayAkaoMessage(byte rate) : base(AkaoMessageType.EnvelopeDecay)
        {
            this.Rate = rate;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Rate: {this.Rate:X2}";
        }
    }

    public sealed class EnvelopeSustainAkaoMessage : AkaoMessage
    {
        public byte Level { get; }

        public EnvelopeSustainAkaoMessage(byte level) : base(AkaoMessageType.EnvelopeSustain)
        {
            this.Level = level;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Level: {this.Level:X2}";
        }
    }

    public sealed class EnvelopeReleaseAkaoMessage : AkaoMessage
    {
        public byte Rate { get; }

        public EnvelopeReleaseAkaoMessage(byte rate) : base(AkaoMessageType.EnvelopeRelease)
        {
            this.Rate = rate;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Rate: {this.Rate:X2}";
        }
    }

    public sealed class EnvelopeDefaultAkaoMessage : AkaoMessage
    {
        public EnvelopeDefaultAkaoMessage() : base(AkaoMessageType.EnvelopeDefault) { }
    }

    public sealed class LoopStartAkaoMessage : AkaoMessage
    {
        public byte LoopCount { get; }

        public LoopStartAkaoMessage(byte loopCount) : base(AkaoMessageType.LoopStart)
        {
            this.LoopCount = loopCount;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Loop Count: {this.LoopCount:X2}";
        }
    }

    public sealed class LoopEndAkaoMessage : AkaoMessage
    {
        public LoopEndAkaoMessage() : base(AkaoMessageType.LoopEnd) { }
    }

    public sealed class EndSequenceAkaoMessage : AkaoMessage
    {
        public EndSequenceAkaoMessage() : base(AkaoMessageType.EndSequence) { }
    }

    public sealed class TempoAkaoMessage : AkaoMessage
    {
        public byte Tempo { get; }

        public TempoAkaoMessage(byte tempo) : base(AkaoMessageType.Tempo)
        {
            this.Tempo = tempo;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Tempo: {this.Tempo:X2}";
        }
    }

    public sealed class TempoFadeAkaoMessage : AkaoMessage
    {
        public byte Duration { get; }
        public byte Tempo { get; }

        public TempoFadeAkaoMessage(byte duration, byte tempo) : base(AkaoMessageType.TempoFade)
        {
            this.Duration = duration;
            this.Tempo = tempo;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Duration: {this.Duration:X2}, Target Tempo: {this.Tempo:X2}";
        }
    }

    public sealed class TrackVolumeAkaoMessage : AkaoMessage
    {
        public byte Volume { get; }

        public TrackVolumeAkaoMessage(byte volume) : base(AkaoMessageType.TrackVolume)
        {
            this.Volume = volume;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Volume: {this.Volume:X2}";
        }
    }

    public sealed class VoltaRepeatAkaoMessage : AkaoMessage
    {
        public byte Count { get; }
        public ushort Offset { get; }

        public VoltaRepeatAkaoMessage(byte count, ushort offset) : base(AkaoMessageType.VoltaRepeat)
        {
            this.Count = count;
            this.Offset = offset;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Count: {this.Count:X2}, Offset: {this.Offset:X4}";
        }
    }

    public sealed class GotoAkaoMessage : AkaoMessage
    {
        public ushort Offset { get; }

        public GotoAkaoMessage(ushort offset) : base(AkaoMessageType.Goto)
        {
            this.Offset = offset;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Offset: {this.Offset:X4}";
        }
    }

    public sealed class LoopRestartAkaoMessage : AkaoMessage
    {
        public LoopRestartAkaoMessage() : base(AkaoMessageType.LoopRestart) { }
    }

    public sealed class IgnoreTrackVolumeAkaoMessage : AkaoMessage
    {
        public byte InstrumentIndex { get; }

        public IgnoreTrackVolumeAkaoMessage(byte index) : base(AkaoMessageType.IgnoreTrackVolume)
        {
            this.InstrumentIndex = index;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Instrument: {this.InstrumentIndex:X2}";
        }
    }
}