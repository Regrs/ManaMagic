#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    internal abstract class BaseSMPTimer
    {
        protected int counter = 0;

        public byte Interval { get; set; } = 0;
        public byte Target { get; set; } = 0;

        public abstract void Reset();
        public abstract bool Tick();
    }
}