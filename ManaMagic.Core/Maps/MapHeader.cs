using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapHeader
    {
        internal static DataTable<MapSpriteObject> EmptyObjectTable = DataTable<MapSpriteObject>.Empty;
        internal static MapHeader InvalidHeader { get; } = new MapHeader(0, 0, 0, 0, 0, 0, 0, 0, MapHeader.EmptyObjectTable, false);

        private const byte CombatMapMarker = 0xFF;

        public byte Tileset8x8Index { get; }
        public byte PaletteSetIndex { get; }
        public byte Tileset16x16Index { get; }
        public MapEventOptions EventOptions { get; }
        public MapSpecialItemsOptions SpecialItemsOptions { get; }
        public byte LayerScrollSettingsIndex { get; }
        public byte DisplaySettingsIndex { get; }
        public byte Unknown { get; }
        public byte NpcPaletteIndex { get; }
        public MapAnimationFlags AnimationFlags { get; }
        public MapCollisionFlags CollisionFlags { get; }
        public LayerLoadMode LayerLoadMode { get; }
        public DataTable<MapSpriteObject> ObjectTable { get; }
        public bool IsValid { get; }
        public bool IsCombatMap { get { return this.NpcPaletteIndex == MapHeader.CombatMapMarker; } }

        public MapHeader(byte tileset8x8Index, byte paletteSetIndex, byte tileset16x16Index, byte eventOptions, byte specialItemsOptions, byte displaySettingsIndex, byte unknown, byte npcPaletteIndex, DataTable<MapSpriteObject> objectTable, bool isValid)
        {
            this.Tileset8x8Index = (byte)(tileset8x8Index & 0x3F);
            this.PaletteSetIndex = (byte)(paletteSetIndex & 0x7F);
            this.Tileset16x16Index = (byte)(tileset16x16Index & 0x3F);
            this.EventOptions = (MapEventOptions)eventOptions;
            this.SpecialItemsOptions = (MapSpecialItemsOptions)specialItemsOptions;
            this.LayerScrollSettingsIndex = (byte)(specialItemsOptions & 0x1F);
            this.DisplaySettingsIndex = (byte)(displaySettingsIndex & 0x3F);
            this.Unknown = unknown;
            this.NpcPaletteIndex = npcPaletteIndex;
            this.AnimationFlags = (MapAnimationFlags)(tileset8x8Index & 0xC0);
            this.CollisionFlags = (MapCollisionFlags)(paletteSetIndex & 0xC0);
            this.LayerLoadMode = (LayerLoadMode)(tileset16x16Index & 0xC0);
            this.ObjectTable = objectTable;
            this.IsValid = isValid;
        }
    }

    public enum MapAnimationFlags : byte
    {
        AnimationFlag40 = 0x40,
        AnimationFlag80 = 0x80,
    }

    public enum MapCollisionFlags : byte
    {
        CollisionFlag40 = 0x40,
        CollisionFlag80 = 0x80,
    }
}