using ZwellTech;

#nullable enable

namespace ManaMagic.Core
{
    public record GenericValue<T> : NotifyRecordPropertyChanged
    {
        private T value;

        public T Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value); }
        }

        public GenericValue(byte index, T value, bool userModified = false) : base(index, userModified)
        {
            this.value = value;
        }
    }
}