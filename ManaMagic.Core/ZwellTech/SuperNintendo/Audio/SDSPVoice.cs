using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    internal sealed class SDSPVoice
    {
        private readonly BrrDecoder decoder;
        private readonly short[] sampleBuffer = new short[12];
        private int sourceNumber = -1;
        private int pitchCounter = 0;

        public ReadOnlySpan<short> RingBuffer { get { return this.sampleBuffer; } }
        public int BufferIndex { get; set; } = 0;

        public DSPRegisters.VoiceDSPRegisters Registers { get; }
        public VoiceEnable VoiceEnable { get; }
        public int KeyOnDelay { get; set; } = 0;

        public short Envelope { get; set; } = 0;
        public EnvelopeMode EnvelopeMode { get; set; } = EnvelopeMode.Attack;

        public int PCMIndex { get { return (this.pitchCounter >> 12) + this.BufferIndex; } }
        public int InterpolationIndex { get { return (this.pitchCounter >> 4) & 0xFF; } }
        public short PitchModulator { get; set; } = 0;

        public short MonoOut { get; set; } = 0;
        public short StereoOutL { get; set; } = 0;
        public short StereoOutR { get; set; } = 0;

        public SDSPVoice(BitRateReductionSample sample, VoiceEnable voiceEnable, DSPRegisters.VoiceDSPRegisters registers)
        {
            this.VoiceEnable = voiceEnable;
            this.decoder = new BrrDecoder(sample);
            this.Registers = registers;
            this.InitializeBuffer();
        }

        public void Tick(DSPRegisters dspRegisters)
        {
            if (this.sourceNumber != this.Registers.VxSRCN)
            {
                this.sourceNumber = this.Registers.VxSRCN;
                this.decoder.QueueSample(dspRegisters.DIR[this.Registers.VxSRCN]);
            }

            int pitch = this.Registers.VxPITCH & 0x3FFF;
            if (dspRegisters.PMON.HasFlag(this.VoiceEnable))
            {
                pitch += ((this.PitchModulator >> 5) * pitch) >> 10;
            }
            this.Tick(pitch);
        }

        private void Tick(int pitch)
        {
            if (this.KeyOnDelay > 0)
            {
                if (this.KeyOnDelay == 5)
                {
                    this.decoder.Reset();
                    this.InitializeBuffer();
                }

                pitch = 0;
                this.pitchCounter = 0;
                this.Envelope = 0;
                this.KeyOnDelay--;
            }

            if (this.pitchCounter >= 0x4000)
            {
                this.AdvanceBuffer();
            }

            this.pitchCounter = (this.pitchCounter & 0x3FFF) + pitch;
            if (this.pitchCounter > 0x7FFF)
            {
                this.pitchCounter = 0x7FFF;
            }
        }

        private void AdvanceBuffer()
        {
            this.sampleBuffer[this.BufferIndex] = this.decoder.Next();
            this.sampleBuffer[this.BufferIndex + 1] = this.decoder.Next();
            this.sampleBuffer[this.BufferIndex + 2] = this.decoder.Next();
            this.sampleBuffer[this.BufferIndex + 3] = this.decoder.Next();

            this.BufferIndex += 4;
            if (this.BufferIndex >= this.sampleBuffer.Length)
            {
                this.BufferIndex = 0;
            }
        }

        private void InitializeBuffer()
        {
            this.sampleBuffer[0] = this.decoder.Next();
            this.sampleBuffer[1] = this.decoder.Next();
            this.sampleBuffer[2] = this.decoder.Next();
            this.sampleBuffer[3] = this.decoder.Next();
            this.sampleBuffer[4] = this.decoder.Next();
            this.sampleBuffer[5] = this.decoder.Next();
            this.sampleBuffer[6] = this.decoder.Next();
            this.sampleBuffer[7] = this.decoder.Next();
            this.sampleBuffer[8] = this.decoder.Next();
            this.sampleBuffer[9] = this.decoder.Next();
            this.sampleBuffer[10] = this.decoder.Next();
            this.sampleBuffer[11] = this.decoder.Next();
            this.BufferIndex = 0;
        }

        public static int WrapIndex(int pcmIndex)
        {
            if (pcmIndex < 0) { return pcmIndex + 12; }
            if (pcmIndex >= 12) { return pcmIndex - 11; }
            return pcmIndex;
        }
    }
}