#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    internal sealed class SMPTimer : BaseSMPTimer
    {
        private byte value = 0;

        public byte Out
        {
            get
            {
                // Reading the timer will reset it.
                byte current = this.value;
                this.value = 0;
                return current;
            }
        }

        public SMPTimer() { this.Interval = 1; }

        public override void Reset()
        {
            ThrowHelper.ThrowNotImplementedException("Core SMP Timers cannot be reset.");
        }

        public override bool Tick()
        {
            this.counter += this.Interval;
            if (this.counter >= this.Target)
            {
                this.counter = 0;
                this.value++;
                this.value &= 0x0F;
                return true;
            }
            return false;
        }
    }
}