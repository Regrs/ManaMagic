using System.Diagnostics;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed class MapTrigger
    {
        public MapTriggerType TriggerType { get; }
        public ushort Value { get; }

        public bool IsEvent { get { return this.TriggerType == MapTriggerType.Event; } }
        public bool IsDoor { get { return this.TriggerType == MapTriggerType.Door; } }

        public MapTrigger(ushort value)
        {
            // Trigger values are passed unmodified to the event sub-system.
            // Because of this it is possible to set a map trigger to fly on Flammie or get shot out of a cannon.
            // The final game doesn't take advantage of this, all triggers are either events or doors.
            // The idea of opening a door and suddenly getting stuffed into a cannon and shot into the sky is hilarious tho.
            // The event sub system also checks for two additional byte ranges that have no implementation, suggesting other movement options were considered.
            // TODO: Properly handle values > 0xC000.
            if (value <= Constants.Bank0A.EventIdMaximum)
            {
                this.TriggerType = MapTriggerType.Event;
                this.Value = value;
            }
            else if (value < 0x0C00)
            {
                this.TriggerType = MapTriggerType.Door;
                this.Value = (ushort)(value & 0x03FF);
            }
            else if (value < 0x0D00) // Flammie Flight - 0C00-0CFF
            {
                this.TriggerType = MapTriggerType.Unknown;
                this.Value = value;
            }
            else if (value < 0x0D80) // Cannon Travel - 0D00-0D7F
            {
                this.TriggerType = MapTriggerType.Unknown;
                this.Value = value;
            }
            else if (value < 0x0E00) // Unused - 0D80-0DFF
            {
                this.TriggerType = MapTriggerType.Unknown;
                this.Value = value;
            }
            else if (value < 0x0F00) // Unused - 0E00-0EFF
            {
                this.TriggerType = MapTriggerType.Unknown;
                this.Value = value;
            }
            else
            {
                this.TriggerType = MapTriggerType.Unknown;
                this.Value = value;
                Debug.Fail("Map Trigger has a value greater than 0F00.");
            }
        }

        // 0FFF: 0000_1111_1111_1111
        // 03FF: 0000_0011_1111_1111
        // 1800: 0001_1000_0000_0000
        // 1900: 0001_1001_0000_0000
        // 1A00: 0001_1010_0000_0000
        // 1B00: 0001_1011_0000_0000
        // 1C00: 0001_1100_0000_0000 // Flammie
        // 1D00: 0001_1101_0000_0000 // Cannon Travel
        // 083F: 0000_1000_0011_1111
        // 0995: 0000_1001_1001_0101
    }
}