using System;
using ManaMagic.Core.Metadata;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record MapSpriteObject
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

        public string Name { get { return ManaMetadata.GetSpriteFriendlyName(this.SpriteIndex); } }
        public EventFlag EventFlag { get; }
        public Bounds8 EventFlagRange { get; }
        public Point8 Location { get; }
        public bool AlwaysLoaded { get; }
        public bool Stationary { get; }
        public SpriteDirection Direction { get; }
        public byte PaletteIndex { get; }
        public byte UnknownBits { get; }
        public byte SpriteIndex { get; }
        public SpriteInteractTypes Interactions { get; }
        public ushort EventIndex { get; }

        public MapSpriteObject(EventFlag eventFlag, byte eventFlagRange, byte xCoordinate, byte yCoordinate, byte direction, byte spriteIndex, byte eventIdLow, byte eventIdHigh)
        {
            this.EventFlag = eventFlag;
            this.EventFlagRange = new Bounds8((byte)((eventFlagRange & MapSpriteObject.FlagMinMask) >> 4), (byte)(eventFlagRange & MapSpriteObject.FlagMaxMask));
            this.Location = new Point8((byte)(xCoordinate & MapSpriteObject.CoordinateMask), (byte)(yCoordinate & MapSpriteObject.CoordinateMask));
            
            // If set, the NPC will always be loaded regardless of if they are on screen or not.
            // This reserves the slot so less enemies can spawn. Commonly used for High Steppers.
            // This flag does not override the event flag check, the NPC will not spawn in the event flag check fails.
            this.AlwaysLoaded = (xCoordinate & MapSpriteObject.AlwaysLoadedFlagMask) > 0;

            // If this flag is set then the NPC will ignore its AI and not move.
            // Cutscene versions of the PC have the same AI as the real versions, thus cannot wander and are always stationary.
            this.Stationary = (yCoordinate & MapSpriteObject.StationaryMask) > 0;
            this.Direction = (SpriteDirection)(direction & MapSpriteObject.DirectionMask);
            this.PaletteIndex = (byte)((direction & PaletteIndexMask) >> 4);
            this.UnknownBits = (byte)(direction & MapSpriteObject.UnknownBitsMask);
            this.SpriteIndex = spriteIndex;
            this.Interactions = (SpriteInteractTypes)(eventIdHigh & MapSpriteObject.InteractionMask);
            this.EventIndex = (ushort)(((eventIdHigh & MapSpriteObject.EventMask) << 8) | eventIdLow);
        }
    }

    [Flags]
    public enum SpriteInteractTypes : byte
    {
        None = 0x00,
        EventInRadius = 0x10,
        DoNotFaceOnInteract = 0x20,
        EventOnInteract = 0x40,
        Pushable = 0x80,
    }

    public enum SpriteDirection : byte
    {
        North = 0x00,
        South = 0x40,
        West = 0x80,
        East = 0xC0,
    }
}