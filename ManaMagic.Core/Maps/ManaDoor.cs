using ManaMagic.Core.Metadata;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed class ManaDoor
    {
        public ushort Index { get; }
        public string Name { get { return ManaMetadata.GetDoorFriendlyName(this.Index); } }
        public ushort MapId { get; }
        public byte XCoordinate { get; }
        public byte YCoordinate { get; }

        public byte Byte1 { get; }
        public byte Byte2 { get; }
        public byte Byte3 { get; }
        public byte Byte4 { get; }

        public ManaDoor(ushort index, byte mapId, byte xCoordinate, byte yCoordinate, byte b4)
        {
            this.Index = index;
            this.MapId = mapId;
            if ((xCoordinate & 0x01) > 0)
            {
                this.MapId |= 0x0100;
            }
            this.XCoordinate = (byte)(xCoordinate >> 1);
            this.YCoordinate = (byte)(yCoordinate >> 1);

            this.Byte1 = mapId;
            this.Byte2 = xCoordinate;
            this.Byte3 = yCoordinate;
            this.Byte4 = b4;
        }
    }
}