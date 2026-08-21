using ZwellTech;

#nullable enable

namespace ManaMagic.Core.WorldMap
{
    public sealed record CannonTravelCoordinateSet : NotifyRecordPropertyChanged
    {
        private byte startX = 0;
        private byte startY = 0;
        private byte endX = 0;
        private byte endY = 0;

        public byte StartXCoordinate
        {
            get { return this.startX; }
            set { this.SetProperty(ref this.startX, value); }
        }

        public byte StartYCoordinate
        {
            get { return this.startY; }
            set { this.SetProperty(ref this.startY, value); }
        }

        public byte EndXCoordinate
        {
            get { return this.endX; }
            set { this.SetProperty(ref this.endX, value); }
        }

        public byte EndYCoordinate
        {
            get { return this.endY; }
            set { this.SetProperty(ref this.endY, value); }
        }

        public CannonTravelCoordinateSet(byte index, byte startXCoordinate, byte startYCoordinate, byte endXCoordinate, byte endYCoordinate, bool userModified = false) : base(index, userModified)
        {
            this.startX = startXCoordinate;
            this.startY = startYCoordinate;
            this.endX = endXCoordinate;
            this.endY = endYCoordinate;
        }
    }
}