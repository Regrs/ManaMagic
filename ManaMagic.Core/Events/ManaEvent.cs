using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ManaMagic.Core.Events.OpCodes;

#nullable enable

namespace ManaMagic.Core.Events
{
    /// <summary>
    /// Represents an event in Secret of Mana.
    /// </summary>
    public sealed record ManaEvent : IEnumerable<EventOpCode>, INotifyPropertyChanged
    {
        public static ManaEvent Empty { get; } = new ManaEvent(new List<EventOpCode>());

        private readonly List<EventOpCode> opCodes = new List<EventOpCode>();

        /// <summary>
        /// Gets a value indicating if the event has been modified.
        /// </summary>
        public bool Dirty { get; private set; } = false;

        public bool UserModified { get; } = false;

        /// <summary>
        /// Gets the size of the event (in bytes).
        /// </summary>
        public int Size { get { return this.OpCodes.Sum(p => p.Size); } }

        /// <summary>
        /// Gets a read only list of the operation codes that make up the event.
        /// </summary>
        public IReadOnlyList<EventOpCode> OpCodes { get { return this.opCodes; } }

        /// <summary>
        /// Gets the <see cref="EventOpCode"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="EventOpCode"/> to get.</param>
        /// <returns>The <see cref="EventOpCode"/> at the specified index.</returns>
        public EventOpCode this[int index] { get { return this.opCodes[index]; } }

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Creates a new instance of the <see cref="ManaEvent"/> with the specified list of <see cref="EventOpCode"/>s.
        /// </summary>
        /// <param name="opCodes"></param>
        public ManaEvent(IEnumerable<EventOpCode> opCodes) : this(opCodes, false) { }

        public ManaEvent(IEnumerable<EventOpCode> opCodes, bool userModified)
        {
            foreach (EventOpCode eventOpCode in opCodes)
            {
                eventOpCode.PropertyChanged += this.EventOpCode_PropertyChanged;
                this.opCodes.Add(eventOpCode);
            }
            this.UserModified = userModified;
        }

        public void Insert(int index, EventOpCode opCode)
        {
            opCode.PropertyChanged += this.EventOpCode_PropertyChanged;
            this.opCodes.Insert(index, opCode);
            this.OnPropertyChanged(nameof(this.OpCodes));
        }

        public bool Remove(EventOpCode opCode)
        {
            opCode.PropertyChanged -= this.EventOpCode_PropertyChanged;
            bool removed = this.opCodes.Remove(opCode);
            this.OnPropertyChanged(nameof(this.OpCodes));
            return removed;
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.Dirty = true;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EventOpCode_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.OpCodes));
        }

        /// <inheritdoc />
        public IEnumerator<EventOpCode> GetEnumerator()
        {
            return this.opCodes.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}