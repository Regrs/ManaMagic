using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Events
{
    public sealed class EventContext
    {
        public DataTable<ManaEvent> Events { get; private set; } = DataTable<ManaEvent>.Empty;

        /// <summary>
        /// Initializes the context by reading event data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            EventRomReader reader = RomReaderFactory.GetRomReader<EventRomReader>();

            this.Events = reader.ReadAllEvents();
        }
    }
}