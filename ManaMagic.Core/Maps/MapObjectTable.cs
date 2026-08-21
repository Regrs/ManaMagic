using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapObjectTable
    {
        internal static MapObjectTable InvalidObjectTable { get; } = new MapObjectTable(MapObject.Empty, MapObject.Empty, DataTable<MapObject>.Empty, DataTable<MapObject>.Empty, false);

        public MapObject Layer1Background { get; }
        public MapObject Layer2Background { get; }

        public DataTable<MapObject> Layer1 { get; }
        public DataTable<MapObject> Layer2 { get; }

        public bool IsValid { get; }
        public bool HasLayer2 { get { return !this.Layer2Background.Equals(MapObject.Empty); } }

        public MapObjectTable(MapObject layer1Background, MapObject layer2Background, DataTable<MapObject> layer1, DataTable<MapObject> layer2, bool isValid)
        {
            this.Layer1Background = layer1Background;
            this.Layer2Background = layer2Background;
            this.Layer1 = layer1;
            this.Layer2 = layer2;
            this.IsValid = isValid;
        }
    }
}