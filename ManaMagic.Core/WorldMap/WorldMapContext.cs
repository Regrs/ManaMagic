using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.WorldMap
{
    /// <summary>
    /// Represents a container of world map related data in a Secret of Mana ROM file.
    /// </summary>
    public sealed class WorldMapContext
    {
        public DataTable<WorldMapLandingLocation> WorldMapLandingLocationTable { get; private set; } = DataTable<WorldMapLandingLocation>.Empty;
        public DataTable<CannonTravelCoordinateSet> CannonTravelCoordinatesTable { get; private set; } = DataTable<CannonTravelCoordinateSet>.Empty;

        /// <summary>
        /// Initializes the context by reading world map data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            WorldMapRomReader reader = RomReaderFactory.GetRomReader<WorldMapRomReader>();

            this.WorldMapLandingLocationTable = reader.ReadWorldMapLandingLocationTable();
            this.CannonTravelCoordinatesTable = reader.ReadCannonTravelCoordinatesTable();
        }
    }
}
