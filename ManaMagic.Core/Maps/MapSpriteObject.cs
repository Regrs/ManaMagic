using System;
using System.ComponentModel;
using ManaMagic.Core.Metadata;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapSpriteObject : NotifyRecordPropertyChanged
    {
        private const byte FlagMinMask = 0B_1111_0000;
        private const byte FlagMaxMask = 0B_0000_1111;
        private const byte CoordinateMask = 0B_0111_1111;
        private const byte AlwaysLoadedFlagMask = 0B_1000_0000;
        private const byte StationaryMask = 0B_1000_0000;
        private const byte DirectionMask = 0B_1100_0000;
        private const byte PaletteIndexMask = 0B_0011_0000;
        private const byte UnknownBitsMask = 0B_0000_1111;
        private const byte InteractionMask = 0B_1111_0000;
        private const byte EventMask = 0B_0000_1111;

        private EventFlag eventFlag = EventFlag.HeadsUpDisplayEnabled;
        private byte eventFlagRange = 0;
        private bool alwaysLoaded = false;
        private bool stationary = false;
        private byte direction = 0;
        private byte spriteIndex = 0;
        private byte eventIdLow = 0;
        private byte eventIdHigh = 0;

        public string Name { get { return ManaMetadata.GetSpriteFriendlyName(this.SpriteIndex); } }

        public EventFlag EventFlag
        {
            get { return this.eventFlag; }
            set { this.SetProperty(ref this.eventFlag, value); }
        }

        public byte EventFlagMinimum
        {
            get { return (byte)((this.eventFlagRange & MapSpriteObject.FlagMinMask) >> 4); }
            set
            {
                value = (byte)((value << 4) & MapSpriteObject.FlagMinMask);
                this.SetProperty(ref this.eventFlagRange, (byte)(this.eventFlagRange | value));
            }
        }

        public byte EventFlagMaximum
        {
            get { return (byte)(eventFlagRange & MapSpriteObject.FlagMaxMask); }
            set
            {
                value = (byte)(value & MapSpriteObject.FlagMaxMask);
                this.SetProperty(ref this.eventFlagRange, (byte)(this.eventFlagRange | value));
            }
        }

        public ManaPoint8 Location { get; }

        public bool AlwaysLoaded
        {
            get { return this.alwaysLoaded; }
            set { this.SetProperty(ref this.alwaysLoaded, value); }
        }

        public bool Stationary
        {
            get { return this.stationary; }
            set { this.SetProperty(ref this.stationary, value); }
        }

        public SpriteDirection Direction
        {
            get { return (SpriteDirection)(this.direction & MapSpriteObject.DirectionMask); }
            set
            {
                byte tmp = (byte)(this.direction & ~MapSpriteObject.DirectionMask);
                this.SetProperty(ref this.direction, (byte)(tmp | (byte)value));
            }
        }

        public byte PaletteIndex
        {
            get { return (byte)((direction & MapSpriteObject.PaletteIndexMask) >> 4); }
            set
            {
                byte tmp = (byte)(this.direction & ~MapSpriteObject.PaletteIndexMask);
                this.SetProperty(ref this.direction, (byte)(tmp | (byte)(value << 4)));
            }
        }

        public byte UnknownBits { get; }

        public byte SpriteIndex
        {
            get { return this.spriteIndex; }
            set { this.SetProperty(ref this.spriteIndex, value); }
        }

        public SpriteInteractTypes Interactions
        {
            get { return (SpriteInteractTypes)(this.eventIdHigh & MapSpriteObject.InteractionMask); }
            set
            {
                byte tmp = (byte)(this.eventIdHigh & ~MapSpriteObject.InteractionMask);
                this.SetProperty(ref this.direction, (byte)(tmp | (byte)value));
            }
        }

        public ushort EventIndex
        {
            get { return (ushort)(((this.eventIdHigh & MapSpriteObject.EventMask) << 8) | this.eventIdLow); }
            set
            {
                byte tmpH = (byte)((byte)(this.eventIdHigh & ~MapSpriteObject.EventMask) | (byte)((value & 0xFF00) >> 8));
                byte tmpL = (byte)(value & 0xFF);
                this.SetProperty(ref this.eventIdHigh, tmpH);
                this.SetProperty(ref this.eventIdLow, tmpL);
            }
        }


        public MapSpriteObject(byte index, byte eventFlag, byte eventFlagRange, byte xCoordinate, byte yCoordinate, byte direction, byte spriteIndex, byte eventIdLow, byte eventIdHigh, bool userModified = false) : base (index, userModified)
        {
            this.eventFlag = (EventFlag)eventFlag;
            this.eventFlagRange = eventFlagRange;
            this.direction = direction;
            this.spriteIndex = spriteIndex;
            this.eventIdLow = eventIdLow;
            this.eventIdHigh = eventIdHigh;

            this.UnknownBits = (byte)(direction & MapSpriteObject.UnknownBitsMask);

            this.Location = new ManaPoint8(0, (byte)(xCoordinate & MapSpriteObject.CoordinateMask), (byte)(yCoordinate & MapSpriteObject.CoordinateMask));
            this.Location.PropertyChanged += this.Location_PropertyChanged;
            
            // If set, the NPC will always be loaded regardless of if they are on screen or not.
            // This reserves the slot so less enemies can spawn. Commonly used for High Steppers.
            // This flag does not override the event flag check, the NPC will not spawn in the event flag check fails.
            this.alwaysLoaded = (xCoordinate & MapSpriteObject.AlwaysLoadedFlagMask) > 0;

            // If this flag is set then the NPC will ignore its AI and not move.
            // Cutscene versions of the PCs have the same AI as the real versions, thus cannot wander and are always stationary.
            this.stationary = (yCoordinate & MapSpriteObject.StationaryMask) > 0;
        }

        public void GetEncodedValues(Span<byte> bytes)
        {
            if (bytes.Length < 8)
            {
                ThrowHelper.ThrowArgumentException("Array must be at least 8 bytes", nameof(bytes));
            }

            byte alwaysLoadedBit = this.AlwaysLoaded ? MapSpriteObject.AlwaysLoadedFlagMask : (byte)0;
            byte stationaryBit = this.Stationary ? MapSpriteObject.StationaryMask : (byte)0;

            bytes.Fill(0);
            bytes[0] = (byte)this.eventFlag;
            bytes[1] = this.eventFlagRange;
            bytes[2] = (byte)(alwaysLoadedBit | this.Location.X);
            bytes[3] = (byte)(stationaryBit | this.Location.Y);
            bytes[4] = this.direction;
            bytes[5] = this.spriteIndex;
            bytes[6] = this.eventIdLow;
            bytes[7] = this.eventIdHigh;
        }

        private void Location_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Location));
        }
    }
}