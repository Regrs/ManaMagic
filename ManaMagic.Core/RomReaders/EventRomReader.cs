using System.Collections.Generic;
using ManaMagic.Core.Events;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class EventRomReader : RomReader
    {
        private readonly EventParser parser;

        public EventRomReader(RomFile rom) : base(rom) { this.parser = new EventParser(this); }

        public DataTable<ManaEvent> ReadAllEvents()
        {
            List<ManaEvent> events = new List<ManaEvent>((int)Constants.EventCount);
            for (ushort i = 0; i < Constants.EventCount; i++)
            {
                ManaEvent manaEvent = this.parser.ParseEvent(i);
                events.Add(manaEvent);
            }
            return new DataTable<ManaEvent>(events);
        }
    }
}