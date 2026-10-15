using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    /// <summary>
    /// Represents an emulator of the Super Nintendo's S-DSP audio chip.
    /// </summary>
    /// <remarks>
    /// References:
    /// https://github.com/akatsuki105/snes-docs-ja/blob/main/others/anomie/apudsp_jwdonal.txt
    /// https://github.com/snes9xgit/snes9x/blob/master/apu/bapu/dsp/SPC_DSP.cpp
    /// https://github.com/nesdev-org/MesenCE/blob/master/Core/SNES/DSP/Dsp.cpp
    /// https://github.com/nesdev-org/MesenCE/blob/master/Core/SNES/DSP/DspVoice.cpp
    /// https://snes.nesdev.org/wiki/S-DSP_registers
    /// </remarks>
    public sealed class SDSP
    {
        private const ushort GlobalTimerMax = 0x77FF;

        private readonly short[,] echoRingBuffer = new short[1024, 2];
        private readonly int echoBufferMaxSize = 1024;
        private int echoBufferIndex = 0;

        private bool latchKONOFF = false;
        private VoiceEnable internalKON = VoiceEnable.Off;
        private VoiceEnable internalKOFF = VoiceEnable.All;

        private readonly SDSPVoice voice0;
        private readonly SDSPVoice voice1;
        private readonly SDSPVoice voice2;
        private readonly SDSPVoice voice3;
        private readonly SDSPVoice voice4;
        private readonly SDSPVoice voice5;
        private readonly SDSPVoice voice6;
        private readonly SDSPVoice voice7;

        private ushort globalTimer = 0;
        private short echoOutL = 0;
        private short echoOutR = 0;

        public DSPRegisters Registers { get; } = new DSPRegisters();
        public short MainStereoOutL { get; private set; } = 0;
        public short MainStereoOutR { get; private set; } = 0;

        public SDSP()
        {
            this.voice0 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice0, this.Registers.Voice0);
            this.voice1 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice1, this.Registers.Voice1);
            this.voice2 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice2, this.Registers.Voice2);
            this.voice3 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice3, this.Registers.Voice3);
            this.voice4 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice4, this.Registers.Voice4);
            this.voice5 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice5, this.Registers.Voice5);
            this.voice6 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice6, this.Registers.Voice6);
            this.voice7 = new SDSPVoice(BitRateReductionSample.Empty, VoiceEnable.Voice7, this.Registers.Voice7);
        }

        public void Step()
        {
            this.Tick();

            // Voice 0 cannot have pitch modulation.
            this.Registers.PMON &= ~VoiceEnable.Voice0;

            this.voice1.PitchModulator = this.StepVoice(this.voice0);
            this.voice2.PitchModulator = this.StepVoice(this.voice1);
            this.voice3.PitchModulator = this.StepVoice(this.voice2);
            this.voice4.PitchModulator = this.StepVoice(this.voice3);
            this.voice5.PitchModulator = this.StepVoice(this.voice4);
            this.voice6.PitchModulator = this.StepVoice(this.voice5);
            this.voice7.PitchModulator = this.StepVoice(this.voice6);
            this.StepVoice(this.voice7);

            // Mix left & right audio channels.
            this.MainStereoOutL = (short)Math.Clamp(this.voice0.StereoOutL + this.voice1.StereoOutL, short.MinValue, short.MaxValue);
            this.MainStereoOutL = (short)Math.Clamp(this.MainStereoOutL + this.voice2.StereoOutL, short.MinValue, short.MaxValue);
            this.MainStereoOutL = (short)Math.Clamp(this.MainStereoOutL + this.voice3.StereoOutL, short.MinValue, short.MaxValue);
            this.MainStereoOutL = (short)Math.Clamp(this.MainStereoOutL + this.voice4.StereoOutL, short.MinValue, short.MaxValue);
            this.MainStereoOutL = (short)Math.Clamp(this.MainStereoOutL + this.voice5.StereoOutL, short.MinValue, short.MaxValue);
            this.MainStereoOutL = (short)Math.Clamp(this.MainStereoOutL + this.voice6.StereoOutL, short.MinValue, short.MaxValue);
            this.MainStereoOutL = (short)Math.Clamp(this.MainStereoOutL + this.voice7.StereoOutL, short.MinValue, short.MaxValue);

            this.MainStereoOutR = (short)Math.Clamp(this.voice0.StereoOutR + this.voice1.StereoOutR, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp(this.MainStereoOutR + this.voice2.StereoOutR, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp(this.MainStereoOutR + this.voice3.StereoOutR, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp(this.MainStereoOutR + this.voice4.StereoOutR, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp(this.MainStereoOutR + this.voice5.StereoOutR, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp(this.MainStereoOutR + this.voice6.StereoOutR, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp(this.MainStereoOutR + this.voice7.StereoOutR, short.MinValue, short.MaxValue);

            // Apply main volume.
            this.MainStereoOutL = (short)Math.Clamp((this.MainStereoOutL * this.Registers.MVOLL) >> 7, short.MinValue, short.MaxValue);
            this.MainStereoOutR = (short)Math.Clamp((this.MainStereoOutR * this.Registers.MVOLR) >> 7, short.MinValue, short.MaxValue);

            // TODO: Fix Echo, pretty sure this is the reason for the bad horn distortion in the back half of Danger.
            this.StepEcho();
            this.MainStereoOutL += this.echoOutL;
            this.MainStereoOutR += this.echoOutR;

            this.latchKONOFF = !this.latchKONOFF;
            if (this.latchKONOFF)
            {
                this.Registers.KON &= ~internalKON;
                this.internalKON = this.Registers.KON;
                this.internalKOFF = this.Registers.KOFF;
            }
        }

        private short StepVoice(SDSPVoice voice)
        {
            // Update the pitch counter and decode samples if needed.
            voice.Tick(this.Registers);

            // Gaussian interpolation to smooth out the sample.
            voice.MonoOut = SDSP.StepInterpolate(voice);

            // Add the current ADSR envelope and clamp the sample.
            voice.MonoOut = (short)(Math.Clamp((voice.MonoOut * voice.Envelope) >> 11, short.MinValue, short.MaxValue) & ~1);
            voice.StereoOutL = (short)Math.Clamp((voice.MonoOut * voice.Registers.VxVOLL) >> 7, short.MinValue, short.MaxValue);
            voice.StereoOutR = (short)Math.Clamp((voice.MonoOut * voice.Registers.VxVOLR) >> 7, short.MinValue, short.MaxValue);

            //voice.StereoOutL = voice.MonoOut;
            //voice.StereoOutR = voice.MonoOut;

            if (this.latchKONOFF)
            {
                if (this.internalKOFF.HasFlag(voice.VoiceEnable))
                {
                    voice.EnvelopeMode = EnvelopeMode.Release;
                }

                if (this.internalKON.HasFlag(voice.VoiceEnable))
                {
                    voice.KeyOnDelay = 5;
                    voice.EnvelopeMode = EnvelopeMode.Attack;
                }
            }

            if (voice.KeyOnDelay == 0)
            {
                // Advance the ADSR envelope for the voice.
                this.StepEnvelope(voice);
            }

            voice.Registers.VxENVX = (byte)(voice.Envelope >> 4);
            voice.Registers.VxOUTX = (sbyte)(voice.MonoOut >> 4);

            return voice.MonoOut;
        }

        private void StepEnvelope(SDSPVoice voice)
        {
            if (!voice.Registers.VxADSR.Enabled)
            {
                ThrowHelper.ThrowNotImplementedException("GAIN mode not implemented.");
            }

            // The volume envelope is 11 bits unsigned
            if (voice.EnvelopeMode == EnvelopeMode.Release)
            {
                voice.Envelope -= 8;
                if (voice.Envelope < 0)
                {
                    voice.Envelope = 0;
                }
            }
            else
            {
                int envelope = voice.Envelope;
                int rate;
                if (voice.EnvelopeMode == EnvelopeMode.Attack)
                {
                    if (voice.Registers.VxADSR.Attack == 15)
                    {
                        rate = 31;
                        envelope += 1024;
                    }
                    else
                    {
                        rate = (voice.Registers.VxADSR.Attack * 2) + 1;
                        envelope += 32;
                    }
                }
                else if (voice.EnvelopeMode == EnvelopeMode.Decay)
                {
                    envelope -= ((envelope - 1) >> 8) + 1;
                    rate = (voice.Registers.VxADSR.Decay * 2) + 16;

                    if ((envelope >> 8) == voice.Registers.VxADSR.SustainLevel)
                    {
                        voice.EnvelopeMode = EnvelopeMode.Sustain;
                    }
                }
                else // EnvelopeMode.Sustain
                {
                    envelope -= ((envelope - 1) >> 8) + 1;
                    rate = voice.Registers.VxADSR.SustainRate;
                }


                if (envelope < 0 || envelope > 0x07FF)
                {
                    envelope = Math.Clamp(envelope, 0, 0x07FF);
                    if (voice.EnvelopeMode == EnvelopeMode.Attack)
                    {
                        voice.EnvelopeMode = EnvelopeMode.Decay;
                    }
                }

                if (this.CheckTimer(rate))
                {
                    voice.Envelope = (short)envelope;
                }
            }
        }

        private void StepEcho()
        {
            this.UpdateEcho();

            int filterL = 0;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 7, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR0) >> 6;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 6, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR1) >> 6;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 5, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR2) >> 6;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 4, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR3) >> 6;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 3, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR4) >> 6;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 2, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR5) >> 6;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 1, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR6) >> 6;
            filterL = (short)filterL;
            filterL += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex, this.echoBufferMaxSize), 0] >> 1) * this.Registers.FIR7) >> 6;
            filterL = (short)(Math.Clamp(filterL, short.MinValue, short.MaxValue) & ~0x01);

            int filterR = 0;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 7, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR0) >> 6;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 6, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR1) >> 6;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 5, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR2) >> 6;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 4, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR3) >> 6;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 3, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR4) >> 6;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 2, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR5) >> 6;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex - 1, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR6) >> 6;
            filterR = (short)filterR;
            filterR += ((this.echoRingBuffer[SDSP.WrapIndex(this.echoBufferIndex, this.echoBufferMaxSize), 1] >> 1) * this.Registers.FIR7) >> 6;
            filterR = (short)(Math.Clamp(filterR, short.MinValue, short.MaxValue) & ~0x01);

            int efbL = (filterL * this.Registers.EFB) >> 7;
            int efbR = (filterR * this.Registers.EFB) >> 7;

            this.echoBufferIndex = SDSP.WrapIndex(this.echoBufferIndex++, this.echoBufferMaxSize);
            this.echoRingBuffer[this.echoBufferIndex, 0] = (short)(Math.Clamp(this.echoOutL + efbL, short.MinValue, short.MaxValue) & ~0x01);
            this.echoRingBuffer[this.echoBufferIndex, 1] = (short)(Math.Clamp(this.echoOutR + efbR, short.MinValue, short.MaxValue) & ~0x01);

            this.echoOutL = (short)(Math.Clamp(this.echoOutL + ((filterL * this.Registers.EVOLL) >> 7), short.MinValue, short.MaxValue) & ~0x01);
            this.echoOutR = (short)(Math.Clamp(this.echoOutR + ((filterR * this.Registers.EVOLR) >> 7), short.MinValue, short.MaxValue) & ~0x01);
        }

        private void UpdateEcho()
        {
            this.echoOutL = 0;
            this.echoOutR = 0;
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice0))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice0.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice0.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice1))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice1.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice1.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice2))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice2.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice2.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice3))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice3.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice3.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice4))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice4.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice4.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice5))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice5.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice5.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice6))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice6.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice6.StereoOutR, short.MinValue, short.MaxValue);
            }
            if (this.Registers.EON.HasFlag(VoiceEnable.Voice7))
            {
                this.echoOutL = (short)Math.Clamp(this.echoOutL + this.voice7.StereoOutL, short.MinValue, short.MaxValue);
                this.echoOutR = (short)Math.Clamp(this.echoOutR + this.voice7.StereoOutR, short.MinValue, short.MaxValue);
            }
        }

        private void Tick()
        {
            if (this.globalTimer == 0)
            {
                this.globalTimer = SDSP.GlobalTimerMax;
                return;
            }
            this.globalTimer--;
        }

        private bool CheckTimer(int rate)
        {
            return (this.globalTimer + SDSP.GlobalTimerOffsets[rate]) % SDSP.GlobalTimerRates[rate] == 0;
        }

        private static short StepInterpolate(SDSPVoice voice)
        {
            Span<short> pcms = stackalloc short[4];
            pcms[0] = voice.RingBuffer[SDSPVoice.WrapIndex(voice.PCMIndex)];
            pcms[1] = voice.RingBuffer[SDSPVoice.WrapIndex(voice.PCMIndex + 1)];
            pcms[2] = voice.RingBuffer[SDSPVoice.WrapIndex(voice.PCMIndex + 2)];
            pcms[3] = voice.RingBuffer[SDSPVoice.WrapIndex(voice.PCMIndex + 3)];

            int InterpolatedPcm = 0;
            InterpolatedPcm += (SDSP.GaussianInterpolationTable[255 - voice.InterpolationIndex] * pcms[0]) >> 11;
            InterpolatedPcm += (SDSP.GaussianInterpolationTable[511 - voice.InterpolationIndex] * pcms[1]) >> 11;
            InterpolatedPcm += (SDSP.GaussianInterpolationTable[256 + voice.InterpolationIndex] * pcms[2]) >> 11;
            InterpolatedPcm = (short)InterpolatedPcm;
            InterpolatedPcm += (SDSP.GaussianInterpolationTable[000 + voice.InterpolationIndex] * pcms[3]) >> 11;

            short pcm = (short)(Math.Clamp(InterpolatedPcm, short.MinValue, short.MaxValue) & ~1);
            return pcm;
        }

        private static int WrapIndex(int echoBufferIndex, int bufferSize)
        {
            if (echoBufferIndex < 0) { return echoBufferIndex + bufferSize; }
            if (echoBufferIndex >= bufferSize) { return echoBufferIndex - bufferSize - 1; }
            return echoBufferIndex;
        }

        private static readonly ushort[] GaussianInterpolationTable = new ushort[]
        {
            // https://problemkaputt.de/fullsnes.htm#snesapudspbrrpitch
            0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000, 0x0000,
            0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0001, 0x0002, 0x0002, 0x0002, 0x0002, 0x0002,
            0x0002, 0x0002, 0x0003, 0x0003, 0x0003, 0x0003, 0x0003, 0x0004, 0x0004, 0x0004, 0x0004, 0x0004, 0x0005, 0x0005, 0x0005, 0x0005,
            0x0006, 0x0006, 0x0006, 0x0006, 0x0007, 0x0007, 0x0007, 0x0008, 0x0008, 0x0008, 0x0009, 0x0009, 0x0009, 0x000A, 0x000A, 0x000A,
            0x000B, 0x000B, 0x000B, 0x000C, 0x000C, 0x000D, 0x000D, 0x000E, 0x000E, 0x000F, 0x000F, 0x000F, 0x0010, 0x0010, 0x0011, 0x0011,
            0x0012, 0x0013, 0x0013, 0x0014, 0x0014, 0x0015, 0x0015, 0x0016, 0x0017, 0x0017, 0x0018, 0x0018, 0x0019, 0x001A, 0x001B, 0x001B,
            0x001C, 0x001D, 0x001D, 0x001E, 0x001F, 0x0020, 0x0020, 0x0021, 0x0022, 0x0023, 0x0024, 0x0024, 0x0025, 0x0026, 0x0027, 0x0028,
            0x0029, 0x002A, 0x002B, 0x002C, 0x002D, 0x002E, 0x002F, 0x0030, 0x0031, 0x0032, 0x0033, 0x0034, 0x0035, 0x0036, 0x0037, 0x0038,
            0x003A, 0x003B, 0x003C, 0x003D, 0x003E, 0x0040, 0x0041, 0x0042, 0x0043, 0x0045, 0x0046, 0x0047, 0x0049, 0x004A, 0x004C, 0x004D,
            0x004E, 0x0050, 0x0051, 0x0053, 0x0054, 0x0056, 0x0057, 0x0059, 0x005A, 0x005C, 0x005E, 0x005F, 0x0061, 0x0063, 0x0064, 0x0066,
            0x0068, 0x006A, 0x006B, 0x006D, 0x006F, 0x0071, 0x0073, 0x0075, 0x0076, 0x0078, 0x007A, 0x007C, 0x007E, 0x0080, 0x0082, 0x0084,
            0x0086, 0x0089, 0x008B, 0x008D, 0x008F, 0x0091, 0x0093, 0x0096, 0x0098, 0x009A, 0x009C, 0x009F, 0x00A1, 0x00A3, 0x00A6, 0x00A8,
            0x00AB, 0x00AD, 0x00AF, 0x00B2, 0x00B4, 0x00B7, 0x00BA, 0x00BC, 0x00BF, 0x00C1, 0x00C4, 0x00C7, 0x00C9, 0x00CC, 0x00CF, 0x00D2,
            0x00D4, 0x00D7, 0x00DA, 0x00DD, 0x00E0, 0x00E3, 0x00E6, 0x00E9, 0x00EC, 0x00EF, 0x00F2, 0x00F5, 0x00F8, 0x00FB, 0x00FE, 0x0101,
            0x0104, 0x0107, 0x010B, 0x010E, 0x0111, 0x0114, 0x0118, 0x011B, 0x011E, 0x0122, 0x0125, 0x0129, 0x012C, 0x0130, 0x0133, 0x0137,
            0x013A, 0x013E, 0x0141, 0x0145, 0x0148, 0x014C, 0x0150, 0x0153, 0x0157, 0x015B, 0x015F, 0x0162, 0x0166, 0x016A, 0x016E, 0x0172,
            0x0176, 0x017A, 0x017D, 0x0181, 0x0185, 0x0189, 0x018D, 0x0191, 0x0195, 0x019A, 0x019E, 0x01A2, 0x01A6, 0x01AA, 0x01AE, 0x01B2,
            0x01B7, 0x01BB, 0x01BF, 0x01C3, 0x01C8, 0x01CC, 0x01D0, 0x01D5, 0x01D9, 0x01DD, 0x01E2, 0x01E6, 0x01EB, 0x01EF, 0x01F3, 0x01F8,
            0x01FC, 0x0201, 0x0205, 0x020A, 0x020F, 0x0213, 0x0218, 0x021C, 0x0221, 0x0226, 0x022A, 0x022F, 0x0233, 0x0238, 0x023D, 0x0241,
            0x0246, 0x024B, 0x0250, 0x0254, 0x0259, 0x025E, 0x0263, 0x0267, 0x026C, 0x0271, 0x0276, 0x027B, 0x0280, 0x0284, 0x0289, 0x028E,
            0x0293, 0x0298, 0x029D, 0x02A2, 0x02A6, 0x02AB, 0x02B0, 0x02B5, 0x02BA, 0x02BF, 0x02C4, 0x02C9, 0x02CE, 0x02D3, 0x02D8, 0x02DC,
            0x02E1, 0x02E6, 0x02EB, 0x02F0, 0x02F5, 0x02FA, 0x02FF, 0x0304, 0x0309, 0x030E, 0x0313, 0x0318, 0x031D, 0x0322, 0x0326, 0x032B,
            0x0330, 0x0335, 0x033A, 0x033F, 0x0344, 0x0349, 0x034E, 0x0353, 0x0357, 0x035C, 0x0361, 0x0366, 0x036B, 0x0370, 0x0374, 0x0379,
            0x037E, 0x0383, 0x0388, 0x038C, 0x0391, 0x0396, 0x039B, 0x039F, 0x03A4, 0x03A9, 0x03AD, 0x03B2, 0x03B7, 0x03BB, 0x03C0, 0x03C5,
            0x03C9, 0x03CE, 0x03D2, 0x03D7, 0x03DC, 0x03E0, 0x03E5, 0x03E9, 0x03ED, 0x03F2, 0x03F6, 0x03FB, 0x03FF, 0x0403, 0x0408, 0x040C,
            0x0410, 0x0415, 0x0419, 0x041D, 0x0421, 0x0425, 0x042A, 0x042E, 0x0432, 0x0436, 0x043A, 0x043E, 0x0442, 0x0446, 0x044A, 0x044E,
            0x0452, 0x0455, 0x0459, 0x045D, 0x0461, 0x0465, 0x0468, 0x046C, 0x0470, 0x0473, 0x0477, 0x047A, 0x047E, 0x0481, 0x0485, 0x0488,
            0x048C, 0x048F, 0x0492, 0x0496, 0x0499, 0x049C, 0x049F, 0x04A2, 0x04A6, 0x04A9, 0x04AC, 0x04AF, 0x04B2, 0x04B5, 0x04B7, 0x04BA,
            0x04BD, 0x04C0, 0x04C3, 0x04C5, 0x04C8, 0x04CB, 0x04CD, 0x04D0, 0x04D2, 0x04D5, 0x04D7, 0x04D9, 0x04DC, 0x04DE, 0x04E0, 0x04E3,
            0x04E5, 0x04E7, 0x04E9, 0x04EB, 0x04ED, 0x04EF, 0x04F1, 0x04F3, 0x04F5, 0x04F6, 0x04F8, 0x04FA, 0x04FB, 0x04FD, 0x04FF, 0x0500,
            0x0502, 0x0503, 0x0504, 0x0506, 0x0507, 0x0508, 0x050A, 0x050B, 0x050C, 0x050D, 0x050E, 0x050F, 0x0510, 0x0511, 0x0511, 0x0512,
            0x0513, 0x0514, 0x0514, 0x0515, 0x0516, 0x0516, 0x0517, 0x0517, 0x0517, 0x0518, 0x0518, 0x0518, 0x0518, 0x0518, 0x0519, 0x0519,
        };
        private static readonly ushort[] GlobalTimerRates = new ushort[32]
        {
           SDSP.GlobalTimerMax + 1, 2048, 1536, 1280, 1024, 768, 640, 512, 384, 320, 256, 192, 160, 128, 96, 80, 64, 48, 40, 32, 24, 20, 16, 12, 10, 8, 6, 5, 4, 3, 2, 1
        };
        private static readonly short[] GlobalTimerOffsets = new short[32]
        {
            1, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 536, 0, 1040, 0, 0
        };
    }
}