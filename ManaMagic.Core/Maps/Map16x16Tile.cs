using System.Collections.Generic;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record Map16x16Tile
    {
        public IReadOnlyList<Map16x16TileQuadrant> Quadrants { get; }

        public Map16x16Tile(IReadOnlyList<Map16x16TileQuadrant> quadrants)
        {
            this.Quadrants = new List<Map16x16TileQuadrant>(quadrants);
        }
    }
}