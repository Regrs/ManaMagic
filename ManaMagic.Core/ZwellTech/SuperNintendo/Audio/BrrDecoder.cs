using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    public sealed class BrrDecoder
    {
        private BitRateReductionSample sample;
        private BitRateReductionSample? queuedSample = null;
        private int blockIndex = 0;
        private int sampleIndex = 0;
        private bool highNibble = true;
        private short olderSample = 0;
        private short oldestSample = 0;

        public bool UseFloatMath { get; set; } = false;

        public BrrDecoder(BitRateReductionSample sample)
        {
            this.sample = sample;
        }

        public short Next()
        {
            return this.UseFloatMath ? this.NextDouble() : this.NextShift();
        }

        public void Reset()
        {
            this.blockIndex = 0;
            this.sampleIndex = 0;

            this.highNibble = true;
            if (this.queuedSample != null)
            {
                this.sample = this.queuedSample;
                this.queuedSample = null;
            }
        }

        public void QueueSample(BitRateReductionSample sample)
        {
            // If DIR or VxSRCN is changed while a voice is playing it has no immediate effect.
            // The next time the voice reaches a BRR end block, the new DIR and/or VxSRCN is used to fetch the loop address. 
            this.queuedSample = sample;
        }

        private short NextShift()
        {
            BitRateReductionBlock block = this.sample[this.blockIndex];
            int sample = block.GetRawSample(this.sampleIndex, this.highNibble);

            if (block.Range >= 0x0D) { ThrowHelper.ThrowNotImplementedException("A shift range greater than 12 is not implemented."); }

            if (sample >= 8) { sample -= 16; }
            sample = (sample << block.Range) >> 1;

            // Filter 0 (Direct): S(x) = RD
            switch (block.Filter)
            {
                case 1:
                    // Filter 1 (15/16): S(x) = RD + S(x-1) + ((-S(x-1))>>4)
                    sample += this.olderSample + (-this.olderSample >> 4);
                    break;
                case 2:
                    // Filter 2 (61/32-15/16): S(x) = RD + (S(x-1)<<1) + ((-((S(x-1)<<1)+S(x-1)))>>5) - S(x - 2) + (S(x - 2) >> 4)
                    sample += (this.olderSample << 1) + ((-((this.olderSample << 1) + this.olderSample)) >> 5) - this.oldestSample + (this.oldestSample >> 4);
                    break;
                case 3:
                    // Filter 3 (115/64-13/16): S(x) = RD + (S(x-1)<<1) + ((-(S(x-1)+(S(x-1)<<2)+(S(x-1)<<3)))>>6) - S(x - 2) + (((S(x - 2) << 1) + S(x - 2)) >> 4)
                    sample += (this.olderSample << 1) + ((-(this.olderSample + (this.olderSample << 2) + (this.olderSample << 3))) >> 6) - this.oldestSample + (((this.oldestSample << 1) + this.oldestSample) >> 4);
                    break;
            }

            sample = Math.Clamp(sample, short.MinValue, short.MaxValue) * 2;

            this.oldestSample = this.olderSample;
            this.olderSample = (short)(sample >> 1);
            this.Update();

            return this.olderSample;
        }

        private short NextDouble()
        {
            BitRateReductionBlock block = this.sample[this.blockIndex];
            int sample = block.GetRawSample(this.sampleIndex, this.highNibble);

            if (block.Range >= 0x0D) { ThrowHelper.ThrowNotImplementedException("A shift range greater than 12 is not implemented."); }

            if (sample >= 8) { sample -= 16; }
            sample = (sample << block.Range) >> 1;

            switch (block.Filter)
            {
                case 1:
                    sample += (short)((double)this.olderSample * (15 / 16));
                    break;
                case 2:
                    sample += (short)(((double)this.olderSample * (61 / 32)) - ((double)this.oldestSample * (15 / 16)));
                    break;
                case 3:
                    sample += (short)(((double)this.olderSample * (115 / 64)) - ((double)this.oldestSample * (13 / 16)));
                    break;
            }

            sample = Math.Clamp(sample, short.MinValue, short.MaxValue) * 2;

            this.oldestSample = this.olderSample;
            this.olderSample = (short)(sample >> 1);
            this.Update();

            return this.olderSample;
        }

        private void Update()
        {
            if (!this.highNibble)
            {
                this.sampleIndex++;
                if (this.sampleIndex >= 8)
                {
                    this.sampleIndex = 0;
                    if (this.sample[this.blockIndex].End)
                    {
                        this.Reset();

                        // Restart from the loopback address rather then the beginning.
                        // TODO: Loop bit, just assuming they always loop because track instruments always do.
                        this.blockIndex = this.sample.LoopbackIndex - 1;
                        return;
                    }
                    this.blockIndex++;
                }
            }
            this.highNibble = !this.highNibble;
        }
    }
}