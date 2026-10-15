using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    public sealed class DSPRegisters
    {
        internal static readonly IReadOnlyList<BitRateReductionSample> NullDirectory = new List<BitRateReductionSample>();

        public VoiceDSPRegisters Voice0 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice1 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice2 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice3 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice4 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice5 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice6 { get; } = new VoiceDSPRegisters();
        public VoiceDSPRegisters Voice7 { get; } = new VoiceDSPRegisters();

        public sbyte MVOLL { get; set; } = 0; // $0C
        public sbyte MVOLR { get; set; } = 0; // $1C
        public sbyte EVOLL { get; set; } = 0; // $2C
        public sbyte EVOLR { get; set; } = 0; // $3C
        public VoiceEnable KON { get; set; } = VoiceEnable.Off;  // $4C
        public VoiceEnable KOFF { get; set; } = VoiceEnable.All; // $5C
        public byte FLG { get; set; } = 0; // $6C
        public VoiceEnable ENDX { get; set; } = VoiceEnable.Off; // $7C

        public sbyte EFB { get; set; } = 0; // $0D
        public VoiceEnable PMON { get; set; } = VoiceEnable.Off; // $2D
        public VoiceEnable NON { get; set; } = VoiceEnable.Off;  // $3D
        public VoiceEnable EON { get; set; } = VoiceEnable.Off;  // $4D

        public IReadOnlyList<BitRateReductionSample> DIR { get; set; } = DSPRegisters.NullDirectory; // $5D - Pointer to the sample source directory page at $DD00. (DDDD DDDD)
        public byte ESA { get; set; } = 0; // $6D - Pointer to the start of the echo memory region at $EE00. (EEEE EEEE)
        public byte EDL { get; set; } = 0; // $7D - EDL controls the size (DDDD * 2048 bytes) and length (DDDD * 512 samples or 16ms @ 32000Hz) of the echo buffer.

        public sbyte FIR0 { get; set; } = 0; // $0F
        public sbyte FIR1 { get; set; } = 0; // $1F
        public sbyte FIR2 { get; set; } = 0; // $2F
        public sbyte FIR3 { get; set; } = 0; // $3F
        public sbyte FIR4 { get; set; } = 0; // $4F
        public sbyte FIR5 { get; set; } = 0; // $5F
        public sbyte FIR6 { get; set; } = 0; // $6F
        public sbyte FIR7 { get; set; } = 0; // $7F

        internal DSPRegisters() { }

        public sealed class VoiceDSPRegisters
        {
            public sbyte VxVOLL { get; set; } = 0;         // $X0
            public sbyte VxVOLR { get; set; } = 0;         // $X1
            public ushort VxPITCH { get; set; } = 0;       // $X2 & $X3
            public byte VxSRCN { get; set; } = 0;          // $X4
            public ADSR VxADSR { get; set; } = ADSR.Empty; // $X5 & $X6
            public byte VxGAIN { get; set; } = 0;          // $X7
            public byte VxENVX { get; set; } = 0;          // $X8
            public sbyte VxOUTX { get; set; } = 0;         // $X9

            internal VoiceDSPRegisters() { }
        }
    }
}