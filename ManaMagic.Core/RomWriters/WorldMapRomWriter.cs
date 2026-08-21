using ManaMagic.Core.WorldMap;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class WorldMapRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public WorldMapRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteWorldMapLandingLocationTable(WorldMapContext context)
        {
            this.Seek(Constants.Bank06.WorldMapLandingLocationTableAddress);
            foreach (WorldMapLandingLocation landingLocation in context.WorldMapLandingLocationTable)
            {
                this.Write(landingLocation.MapIndex);
                this.Write(landingLocation.XCoordinate);
                this.Write(landingLocation.YCoordinate);
            }
        }

        public void WriteCannonTravelCoordinateTable(WorldMapContext context)
        {
            this.Seek(Constants.Bank06.CannonTravelCoordinateTableAddress);
            foreach (CannonTravelCoordinateSet coordinateSet in context.CannonTravelCoordinatesTable)
            {
                this.Write(coordinateSet.StartXCoordinate);
                this.Write(coordinateSet.StartYCoordinate);
                this.Write(coordinateSet.EndXCoordinate);
                this.Write(coordinateSet.EndYCoordinate);
            }
        }
    }
}