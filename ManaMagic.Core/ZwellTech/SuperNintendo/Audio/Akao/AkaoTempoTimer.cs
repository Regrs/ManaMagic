#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    internal sealed class AkaoTempoTimer : BaseSMPTimer
    {
        public AkaoTempoTimer() { this.Target = AkaoDriver.TempoTarget; }

        public override void Reset()
        {
            // When a new track is set to play the tempo rate will be set to 1 and the counter will be set to 255.
            // This insures it is able to tick once to read the initial sequence message for each voice.
            // One of the voices is expected to set the tempo immediately, otherwise the next message won't be processed for over a second.
            this.Interval = AkaoDriver.DefaultTempo;
            this.Target = AkaoDriver.TempoTarget;
            this.counter = this.Target;
        }

        public override bool Tick()
        {
            this.counter += this.Interval;
            if (this.counter > this.Target)
            {
                this.counter &= this.Target;
                return true;
            }
            return false;
        }
    }
}