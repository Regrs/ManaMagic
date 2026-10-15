#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    internal sealed class AkaoVibrato
    {
        public bool Enabled { get { return this.Amplitude > 0; } }
        public byte Amplitude { get; set; } = 0;  // $0121
        public byte Delay { get; set; } = 0;      // $0160
        public byte Duration { get; set; } = 0;   // $FCA0
        public byte Multiplier { get; set; } = 0; // $FDC0
        public byte Envelope { get; set; } = 0;   // $FE00
        public ushort Amount { get; set; } = 0;   // $FE40
        public SMPCountdownTimer DelayTimer { get; } = new SMPCountdownTimer(1, 0);    // $0100
        public SMPCountdownTimer DurationTimer { get; } = new SMPCountdownTimer(1, 1); // $FCE0

        public void Enable(VibratoOnAkaoMessage message)
        {
            this.Delay = message.Delay;
            this.Duration = message.Duration;

            // Duration initially set to a single tick.
            this.DurationTimer.Target = 1;
            this.DurationTimer.Reset();

            this.Amplitude = message.Amplitude;
        }

        public void Disable()
        {
            this.Amplitude = 0;
        }

        public void Reset()
        {
            this.Amount = 0; // Driver will unconditionally set to zero when a new note is prepared.
            if (this.Enabled)
            {
                this.Multiplier = (byte)(this.Amplitude >= 0xC0 ? 0x80 : 0x00);

                this.DelayTimer.Target = this.Delay;
                this.DelayTimer.Reset();

                // Duration initially set to a single tick.
                this.DurationTimer.Target = 1;
                this.DurationTimer.Reset();

                this.Envelope = (byte)(this.DelayTimer.Target > 0x00 ? 0x07 : 0x00);
            }
        }

        public bool Step(ushort pitch)
        {
            bool changed = false;
            if (this.Enabled && this.DelayTimer.Expired)
            {
                if (this.DurationTimer.Tick())
                {
                    // $34 = Envelope
                    // $35 = Amplitude
                    // $36 = Multiplier
                    (byte Envelope, byte Amplitude, byte Multiplier) result = AkaoVibrato.Calculate(this.Envelope, this.Amplitude, this.Multiplier);

                    this.Multiplier = result.Multiplier;
                    this.Amount = 0;

                    byte localMultiplier = (byte)(this.Multiplier << 1);
                    if (localMultiplier > 0)
                    {
                        this.Envelope = result.Envelope;
                        this.Amount = (ushort)((((pitch * 0x0F) >> 8) * localMultiplier) >> 8);

                        // Driver keeps the shifted bit from above in the carry flag for this check.
                        if ((this.Multiplier & 0x80) > 0)
                        {
                            this.Amount ^= 0xFFFF;
                        }
                    }

                    changed = true;
                }

                if (this.DurationTimer.Expired)
                {
                    this.DurationTimer.Target = this.Duration;
                    this.DurationTimer.Reset();
                }
            }
            return changed;
        }

        private static (byte Envelope, byte Amplitude, byte Multiplier) Calculate(byte envelope, byte amplitude, byte multiplier)
        {
            if ((amplitude & 0xC0) == 0xC0)
            {
                if ((multiplier & 0x80) == 0)
                {
                    return (envelope, amplitude, (byte)(multiplier | 0x80));
                }
                multiplier = 0;
            }

            byte fraction = 0;
            if ((multiplier << 1) == 0)
            {
                fraction = (byte)(((amplitude & 0x3F) << 1) + 1);
                if (envelope > 0)
                {
                    if ((envelope & 0x01) > 0)
                    {
                        envelope &= 0xFE;
                        fraction >>= 2;
                    }
                    else if ((envelope & 0x02) > 0)
                    {
                        envelope &= 0xFD;
                        fraction >>= 1;
                    }
                    else
                    {
                        envelope &= 0xFB;

                        fraction >>= 1;
                        fraction = (byte)(((fraction >> 1) + fraction) & 0xFF);
                    }

                    if (fraction == 0)
                    {
                        fraction++;
                    }
                }
            }

            multiplier = (byte)((((amplitude & 0x40) << 1) ^ 0x80) | fraction);
            return (envelope, amplitude, multiplier);
        }
    }
}