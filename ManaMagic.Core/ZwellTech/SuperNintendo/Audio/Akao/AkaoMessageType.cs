#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public enum AkaoMessageType : byte
    {
        Note = 0xB3, // 00-B3
        Tie = 0xB4,  // B4-C2
        Rest = 0xD1, // C3-D1

        ChannelVolume = 0xD2,
        ChannelVolumeFade = 0xD3,
        Pan = 0xD4,
        PanFade = 0xD5,
        PitchSlide = 0xD6,
        VibratoOn = 0xD7,
        VibratoOff = 0xD8,
        TremoloOn = 0xD9,
        TremoloOff = 0xDA,
        PanLFOOn = 0xDB,
        PanLFOOff = 0xDC,
        NoiseFrequency = 0xDD,
        NoiseOn = 0xDE,
        NoiseOff = 0xDF,
        PitchModOn = 0xE0,
        PitchModOff = 0xE1,
        EchoOn = 0xE2,
        EchoOff = 0xE3,
        Octave = 0xE4,
        OctaveUp = 0xE5,
        OctaveDown = 0xE6,
        Transpose = 0xE7,
        TransposeAdjust = 0xE8,
        Detune = 0xE9,
        InstrumentChange = 0xEA,
        EnvelopeAttack = 0xEB,
        EnvelopeDecay = 0xEC,
        EnvelopeSustain = 0xED,
        EnvelopeRelease = 0xEE,
        EnvelopeDefault = 0xEF,
        LoopStart = 0xF0,
        LoopEnd = 0xF1,
        EndSequence = 0xF2,
        Tempo = 0xF3,
        TempoFade = 0xF4,
        ReservedF5 = 0xF5,                   // No operation, but takes the parameters of EchoVolume like other Akao V3 Drivers.
        ReservedF6 = 0xF6,                   // No operation, but takes the parameters of EchoVolumeFade like other Akao V3 Drivers.
        ReservedF7 = 0xF7,                   // No operation, but takes the parameters of EchoFeedbackFilter like other Akao V3 Drivers.
        TrackVolume = 0xF8,
        VoltaRepeat = 0xF9,
        Goto = 0xFA,
        GotoConditional = 0xFB,              // Only used by sound effects.
        LoopRestart = 0xFC,
        IgnoreTrackVolume = 0xFD,
        ReservedFE = 0xFE,                   // Points to EndSequence.
        ReservedFF = 0xFF,                   // Points to EndSequence.
    }
}