using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Maps
{
    [DebuggerDisplay("Count: {Triggers.Count}")]
    public sealed record MapTriggerList : NotifyRecordPropertyChanged, IEnumerable<MapTrigger>
    {
        private readonly List<MapTrigger> triggers;

        /// <summary>
        /// Gets a read-only list of <see cref="MapTrigger"/>s in this list.
        /// </summary>
        public IReadOnlyList<MapTrigger> Triggers { get { return this.triggers; } }

        /// <summary>
        /// Gets the <see cref="MapTrigger"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="MapTrigger"/>.</param>
        /// <returns></returns>
        public MapTrigger this[int index] { get { return this.triggers[index]; } }

        /// <summary>
        /// Create a new instance of the <see cref="MapTriggerList"/> class with the specified trigger list.
        /// </summary>
        /// <param name="triggers">A list of <see cref="MapTrigger"/>s that represent the maps trigger list.</param>
        public MapTriggerList(byte index, List<MapTrigger> triggers, bool userModified = false) : base(index, userModified)
        {
            triggers.ForEach((trigger) => trigger.PropertyChanged += this.Trigger_PropertyChanged);
            this.triggers = triggers;
        }

        /// <inheritdoc/>
        public IEnumerator<MapTrigger> GetEnumerator()
        {
            return this.triggers.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void Trigger_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Triggers));
        }
    }
}