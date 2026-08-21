using System;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    public sealed class RowReplacedEventArgs<T> : EventArgs
    {
        public int Index { get; }
        public T OldItem { get; private set; }
        public T NewItem { get; private set; }

        public RowReplacedEventArgs(int index, T oldItem, T newItem)
        {
            this.Index = index;
            this.OldItem = oldItem;
            this.NewItem = newItem;
        }

        internal void Clear()
        {
            this.OldItem = default(T)!;
            this.NewItem = default(T)!;
        }
    }
}