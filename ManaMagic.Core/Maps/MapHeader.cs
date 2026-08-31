using System;
using System.ComponentModel;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapHeader : NotifyRecordPropertyChanged
    {
        internal static DataTable<MapSpriteObject> EmptyObjectTable = DataTable<MapSpriteObject>.Empty;
        internal static MapHeader InvalidHeader { get; } = new MapHeader(0, 0, 0, 0, 0, 0, 0, 0, 0, MapHeader.EmptyObjectTable, false);

        private byte tileset8x8Index = 0;
        private byte paletteSetIndex = 0;
        private byte tileset16x16Index = 0;
        private byte eventOptions = 0;
        private byte specialItemsOptions = 0;
        private byte displaySettingsIndex = 0;
        private byte npcPaletteIndex = 0;

        public byte Tileset8x8Index
        {
            get { return (byte)(this.tileset8x8Index & 0x3F); }
            set
            {
                value = (byte)(value & 0x3F);
                this.SetProperty(ref this.tileset8x8Index, (byte)(value | (byte)this.AnimationFlags));
            }
        }

        public byte PaletteSetIndex
        {
            get { return (byte)(this.paletteSetIndex & 0x7F); }
            set
            {
                value = (byte)(value & 0x3F);
                this.SetProperty(ref this.paletteSetIndex, (byte)(value | (byte)this.CollisionFlags));
            }
        }

        public byte Tileset16x16Index
        {
            get { return (byte)(this.tileset16x16Index & 0x3F); }
            set
            {
                value = (byte)(value & 0x3F);
                this.SetProperty(ref this.tileset16x16Index, (byte)(value | (byte)this.LayerLoadMode));
            }
        }

        public MapEventOptions EventOptions
        {
            get { return (MapEventOptions)this.eventOptions; }
            set { this.SetProperty(ref this.eventOptions, (byte)value); }
        }

        public byte LayerScrollSettingsIndex
        {
            get { return (byte)(this.specialItemsOptions & 0x1F); }
            set
            {
                value = (byte)(value & 0x1F);
                this.SetProperty(ref this.specialItemsOptions, (byte)(value | (byte)this.SpecialItemsOptions));
            }
        }

        public MapSpecialItemsOptions SpecialItemsOptions
        {
            get { return (MapSpecialItemsOptions)(this.specialItemsOptions & 0xE0); }
            set { this.SetProperty(ref this.specialItemsOptions, (byte)((byte)value | this.LayerScrollSettingsIndex)); }
        }

        public byte DisplaySettingsIndex
        {
            get { return (byte)(this.displaySettingsIndex & 0x3F); }
            set
            {
                value = (byte)(value & 0x3F);
                this.SetProperty(ref this.displaySettingsIndex, (byte)(value | (byte)this.UnknownDSBits));
            }
        }

        public MapDisplaySettingsFlags UnknownDSBits
        {
            get { return (MapDisplaySettingsFlags)(this.displaySettingsIndex & 0xC0); }
            set { this.SetProperty(ref this.displaySettingsIndex, (byte)((byte)value | this.DisplaySettingsIndex)); }
        }


        public byte Unused { get; }

        public byte NpcPaletteIndex
        {
            get { return this.npcPaletteIndex; }
            set { this.SetProperty(ref this.npcPaletteIndex, value); }
        }

        public MapAnimationFlags AnimationFlags
        {
            get { return (MapAnimationFlags)(this.tileset8x8Index & 0xC0); }
            set { this.SetProperty(ref this.tileset8x8Index, (byte)((byte)value | this.Tileset8x8Index)); }
        }

        public MapCollisionFlags CollisionFlags
        {
            get { return (MapCollisionFlags)(this.paletteSetIndex & 0x80); }
            set { this.SetProperty(ref this.paletteSetIndex, (byte)((byte)value | this.paletteSetIndex)); }
        }

        public LayerLoadMode LayerLoadMode
        {
            get { return (LayerLoadMode)(this.tileset16x16Index & 0xC0); }
            set { this.SetProperty(ref this.tileset16x16Index, (byte)((byte)value | this.tileset16x16Index)); }
        }

        public DataTable<MapSpriteObject> ObjectTable { get; }

        public bool IsValid { get; }
        public bool IsCombatMap { get { return this.NpcPaletteIndex == Constants.CombatMapMarker; } }

        public MapHeader(byte index,
                         byte tileset8x8Index,
                         byte paletteSetIndex,
                         byte tileset16x16Index,
                         byte eventOptions,
                         byte specialItemsOptions,
                         byte displaySettingsIndex,
                         byte unused,
                         byte npcPaletteIndex,
                         DataTable<MapSpriteObject> objectTable,
                         bool isValid,
                         bool userModified = false) : base(index, userModified)
        {
            this.tileset8x8Index = tileset8x8Index;
            this.paletteSetIndex = paletteSetIndex;
            this.tileset16x16Index = tileset16x16Index;
            this.eventOptions = eventOptions;
            this.specialItemsOptions = specialItemsOptions;
            this.displaySettingsIndex = displaySettingsIndex;
            this.Unused = unused;
            this.npcPaletteIndex = npcPaletteIndex;
            this.ObjectTable = objectTable;
            this.IsValid = isValid;

            foreach (MapSpriteObject spriteObject in this.ObjectTable)
            {
                spriteObject.PropertyChanged += this.SpriteObject_PropertyChanged;
            }
        }

        public void GetEncodedValues(Span<byte> bytes)
        {
            if (bytes.Length < 8)
            {
                ThrowHelper.ThrowArgumentException("Array must be at least 8 bytes", nameof(bytes));
            }

            bytes.Fill(0);
            bytes[0] = this.tileset8x8Index;
            bytes[1] = this.paletteSetIndex;
            bytes[2] = this.tileset16x16Index;
            bytes[3] = this.eventOptions;
            bytes[4] = this.specialItemsOptions;
            bytes[5] = this.displaySettingsIndex;
            bytes[6] = this.Unused;
            bytes[7] = this.npcPaletteIndex;
        }

        private void SpriteObject_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.ObjectTable));
        }
    }

    public enum MapDisplaySettingsFlags : byte
    {
        // This is the only bit that is ever used. And only by two maps. Potos Field 1 and the end credits version of that map.
        MapDisplaySetting40 = 0x40,
        MapDisplaySetting80 = 0x80,
    }

    public enum MapAnimationFlags : byte
    {
        AnimationFlag40 = 0x40,
        AnimationFlag80 = 0x80,
    }

    public enum MapCollisionFlags : byte
    {
        //CollisionFlag40 = 0x40,
        CollisionFlag80 = 0x80,
    }
}