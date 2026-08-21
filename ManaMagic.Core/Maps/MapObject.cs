using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapObject
    {
        internal static MapObject Empty { get; } = new MapObject((EventFlag)0, 0, 0, 0, 0);

        public ushort Index { get; }
        public Point8 Location { get; }
        public EventFlag EventFlag { get; }
        public Bounds8 EventFlagRange { get; }

        public MapObject(EventFlag eventFlag, byte eventFlagRange, byte pieceIndex, byte xCoordinate, byte yCoordinate)
        {
            ushort ninthBit = (ushort)((xCoordinate & 0x01) << 8);
            ushort tenthBit = (ushort)((yCoordinate & 0x01) << 9);

            this.Index = (ushort)(tenthBit | ninthBit | pieceIndex);
            this.Location = new Point8((byte)(xCoordinate & 0xFE), (byte)(yCoordinate & 0xFE));
            this.EventFlag = eventFlag;
            this.EventFlagRange = new Bounds8((byte)((eventFlagRange & 0xF0) >> 4), (byte)(eventFlagRange & 0x0F));
        }
    }
}