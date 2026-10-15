#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    internal sealed class SMPCountdownTimer : BaseSMPTimer
    {
        public int Counter { get { return this.counter; } }
        public bool Expired { get { return this.counter == 0; } }

        public SMPCountdownTimer() : base() { }

        public SMPCountdownTimer(byte interval, byte target)
        {
            this.Interval = interval;
            this.Target = target;
            this.counter = target;
        }

        public override void Reset()
        {
            this.counter = this.Target;
        }

        public override bool Tick()
        {
            if (!this.Expired)
            {
                this.counter -= this.Interval;
                return this.Expired;
            }
            return false;
        }
    }
}