using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManaMagic.Core.WorldMap;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    /// <summary>
    /// Provides a Secret of Mana ROM reader that encapsulates methods to read map related data.
    /// </summary>
    public sealed class WorldMapRomReader : RomReader
    {
        /// <inheritdoc/>
        public WorldMapRomReader(RomFile rom) : base(rom) { }

        public DataTable<WorldMapLandingLocation> ReadWorldMapLandingLocationTable()
        {
            this.Seek((int)Constants.Bank06.WorldMapLandingLocationTableAddress);

            List<WorldMapLandingLocation> coordinateList = new List<WorldMapLandingLocation>((int)Constants.Bank06.WorldMapLandingLocationSize);
            for (int i = 0; i < Constants.Bank06.WorldMapLandingLocationSize; i++)
            {
                byte mapIndex = this.Read();
                byte xCoordinate = this.Read();
                byte yCoordinate = this.Read();
                coordinateList.Add(new WorldMapLandingLocation((byte)i, mapIndex, xCoordinate, yCoordinate));
            }
            return new DataTable<WorldMapLandingLocation>(coordinateList);
        }

        public DataTable<CannonTravelCoordinateSet> ReadCannonTravelCoordinatesTable()
        {
            this.Seek((int)Constants.Bank06.CannonTravelCoordinateTableAddress);

            List<CannonTravelCoordinateSet> coordinateList = new List<CannonTravelCoordinateSet>((int)Constants.Bank06.CannonTravelCoordinateTableSize);
            for (int i = 0; i < Constants.Bank06.CannonTravelCoordinateTableSize; i++)
            {
                byte startX = this.Read();
                byte startY = this.Read();
                byte endX = this.Read();
                byte endY = this.Read();
                coordinateList.Add(new CannonTravelCoordinateSet((byte)i, startX, startY, endX, endY));
            }
            return new DataTable<CannonTravelCoordinateSet>(coordinateList);
        }
    }


}
