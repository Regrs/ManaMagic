using ZwellTech;

#nullable enable

namespace ManaMagic.Core.WorldMap
{
    public sealed record WorldMapLandingLocation : NotifyRecordPropertyChanged
    {
        private byte mapIndex = 0;
        private byte xCoordinate = 0;
        private byte yCoordinate = 0;

        public byte MapIndex
        {
            get { return this.mapIndex; }
            set { this.SetProperty(ref this.mapIndex, value); }
        }

        public byte XCoordinate
        {
            get { return this.xCoordinate; }
            set { this.SetProperty(ref this.xCoordinate, value); }
        }

        public byte YCoordinate
        {
            get { return this.yCoordinate; }
            set { this.SetProperty(ref this.yCoordinate, value); }
        }

        public WorldMapLandingLocation(byte index, byte mapIndex, byte xCoordinate, byte yCoordinate, bool userModified = false) : base(index, userModified)
        {
            this.mapIndex = mapIndex;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }
    }
}