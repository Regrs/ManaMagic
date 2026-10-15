#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    internal sealed class AkaoPanLfo
    {
        public bool Enabled { get; set; } = false; // $0141
        public byte Duration { get; set; } = 0;    // $FCC0
        public short Amplitude { get; set; } = 0;  // $FDA0
        public short Frequency { get; set; } = 0;  // $FDE0
        public sbyte Offset { get; set; } = 0;     // $FDE1
        public SMPCountdownTimer DurationTimer { get; } = new SMPCountdownTimer(1, 0); // $FD00

        public void Enable(PanLFOOnAkaoMessage message)
        {
            this.Duration = (byte)(message.Duration + 1);
            this.DurationTimer.Target = this.Duration > 0 ? (byte)(this.Duration >> 1) : (byte)128;
            this.DurationTimer.Reset();

            byte amplitudeL = message.Amplitude > 0 ? (byte)message.Amplitude : (byte)((message.Amplitude & 0x7F) ^ 0x7F);
            byte amplitudeH = 0;
            this.Amplitude = (short)((amplitudeH << 8) | amplitudeL);
            if (this.DurationTimer.Target > 0)
            {
                amplitudeH = (byte)((amplitudeL / this.DurationTimer.Target) & 0xFF);
                amplitudeL = (byte)((((amplitudeL % this.DurationTimer.Target) << 8) / this.DurationTimer.Target) & 0xFF);
                if (amplitudeL == 0)
                {
                    amplitudeL++;
                }

                this.Amplitude = (short)((amplitudeH << 8) | amplitudeL);
                if (message.Amplitude < 0)
                {
                    this.Amplitude = (short)(this.Amplitude ^ 0xFFFF);
                    this.Amplitude++;
                }
            }

            // The driver will simply write the amplitude value from the message to the enabled byte.
            // Its not used for any other purpose other then making the value not zero.
            this.Enabled = true;
            this.Frequency = 0;
            this.Offset = 0;
        }

        public void Disable()
        {
            // The driver writes zero (which happens to come from the number of parameters this message has) to disable the LFO.
            this.Enabled = false;
            this.Frequency = 0;
            this.Offset = 0;
        }

        public bool Step()
        {
            bool changed = false;
            if (this.Enabled)
            {
                short newFrequency = (short)(this.Amplitude + this.Frequency);
                if ((this.Amplitude >= 0) && (this.Frequency >= 0) && (newFrequency < 0))
                {
                    newFrequency = (short)((newFrequency & 0x00FF) | 0x7F00);
                }
                else if ((this.Amplitude < 0) && (this.Frequency < 0) && (newFrequency >= 0))
                {
                    newFrequency = (short)((newFrequency & 0x00FF) | 0x8000);
                }

                if ((newFrequency & 0xFF00) != (this.Frequency & 0xFF00))
                {
                    this.Offset = (sbyte)((newFrequency >> 8) & 0xFF);
                    this.Frequency = newFrequency;

                    // Pan changed, trigger a DSP volume update at the end of the step.
                    changed = true;
                }

                if (this.DurationTimer.Tick())
                {
                    this.Amplitude = (short)((this.Amplitude ^ 0xFFFF) + 1);
                    this.DurationTimer.Target = this.Duration;
                    this.DurationTimer.Reset();
                }
            }
            return changed;
        }
    }
}