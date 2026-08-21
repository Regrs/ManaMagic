using ZwellTech;

#nullable enable

namespace ManaMagic.Core
{
    public sealed record ManaPoint8 : NotifyRecordPropertyChanged
    {
        private byte x = 0;
        private byte y = 0;

        public byte X
        {
            get { return this.x; }
            set { this.SetProperty(ref this.x, value); }
        }

        public byte Y
        {
            get { return this.y; }
            set { this.SetProperty(ref this.y, value); }
        }

        public ManaPoint8(byte index, byte x, byte y, bool userModified = false) : base(index, userModified)
        {
            this.x = x;
            this.y = y;
        }
    }
}