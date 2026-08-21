using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace ZwellTech
{
    public abstract record NotifyRecordPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating if the record has been modified.
        /// </summary>
        public bool Dirty { get; private set; } = false;
        public bool UserModified { get; } = false;
        public byte Index { get; }

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        public NotifyRecordPropertyChanged(byte index, bool userModified)
        {
            this.Index = index;
            this.UserModified = userModified;
        }

        protected void SetProperty<T>(ref T lValueRef, T rValue, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(lValueRef, rValue))
            {
                lValueRef = rValue;
                this.OnPropertyChanged(propertyName);
            }
        }

        internal void OnPropertyChanged(string? propertyName = null)
        {
            this.Dirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}