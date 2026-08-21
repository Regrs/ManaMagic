using ManaMagic.Core.Metadata;
using ZwellTech;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents a single 16x16 tile of a boss sprite frame.
    /// </summary>
    public sealed record BossSpriteFramePart : BossFramePart
    {
        /// <summary>
        /// Gets the tile Id for this frame part.
        /// </summary>
        public byte TileId { get; init; }

        /// <summary>
        /// Gets the drawing flags for this frame part.
        /// </summary>
        public BossFrameFlags Flags { get; init; }

        /// <summary>
        /// Gets if the tile Id should be extended.
        /// </summary>
        public bool ExtendTileId { get { return this.Flags.HasFlag(BossFrameFlags.ExtendTileId); } }

        /// <summary>
        /// Gets if the frame part should be drawn using palette index 1.
        /// </summary>
        public bool UsePalette1 { get { return this.Flags.HasFlag(BossFrameFlags.UsePalette1) && !this.Flags.HasFlag(BossFrameFlags.UsePalette2); } }

        /// <summary>
        /// Gets if the frame part should be drawn using palette index 2.
        /// </summary>
        public bool UsePalette2 { get { return this.Flags.HasFlag(BossFrameFlags.UsePalette2) && !this.Flags.HasFlag(BossFrameFlags.UsePalette1); } }

        /// <summary>
        /// Gets if the frame part should be drawn using palette index 3.
        /// </summary>
        public bool UsePalette3 { get { return this.Flags.HasFlag(BossFrameFlags.UsePalette3); } }

        /// <summary>
        /// Gets if the frame part should be drawn as a 32x32 tile instead of 16x16.
        /// </summary>
        public bool Draw32x32Tile { get { return this.Flags.HasFlag(BossFrameFlags.Draw32x32Tile); } }

        /// <summary>
        /// Gets if the frame part should be flipped horizontally.
        /// </summary>
        public bool HorizontalFlip { get { return this.Flags.HasFlag(BossFrameFlags.HorizontalFlip); } }

        /// <summary>
        /// Gets if the frame part should be flipped vertically.
        /// </summary>
        public bool VerticalFlip { get { return this.Flags.HasFlag(BossFrameFlags.VerticalFlip); } }

        /// <summary>
        /// Gets the palette index for the frame part based off the palette flags in the <see cref="Flags"/> property.
        /// </summary>
        public int PaletteIndex
        {
            get
            {
                if (this.UsePalette1) { return 0; }
                else if (this.UsePalette2) { return 1; }
                else if (this.UsePalette3) { return 2; }

                //return 0;
                return (int)ThrowHelper.ThrowInvalidOperationException("Unknown palette bits");
            }
        }

        /// <summary>
        /// Gets the <see cref="FlipType"/> for the frame part based off the flip flags in the <see cref="Flags"/> property.
        /// </summary>
        public FlipType FlipType
        {
            get
            {
                FlipType flipType = FlipType.None;
                if (this.HorizontalFlip) { flipType |= FlipType.HorizontalFlip; }
                if (this.VerticalFlip) { flipType |= FlipType.VerticalFlip; }
                return flipType;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BossSpriteFramePart"/> class.
        /// </summary>
        public BossSpriteFramePart() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossFramePart"/> class with the specified coordinates, tile Id, and frame flags.
        /// </summary>
        /// <param name="xCoordinate">The X-Coordinate of the frame part, relative to the bosses origin.</param>
        /// <param name="yCoordinate">The Y-Coordinate of the frame part, relative to the bosses origin.</param>
        /// <param name="tileId">The tile Id of the frame part.</param>
        /// <param name="flags">The drawing flags for the frame part.</param>
        public BossSpriteFramePart(sbyte xCoordinate, sbyte yCoordinate, byte tileId, BossFrameFlags flags) : base(xCoordinate, yCoordinate)
        {
            this.TileId = tileId;
            this.Flags = flags;
        }

        /// <summary>
        /// Calculates the true tile Id for the frame part for the boss tileset specified by the index.
        /// </summary>
        /// <param name="index">The index of the boss tileset that will be used to draw the frame part.</param>
        /// <returns>The calculated tile Id for the frame part.</returns>
        /// <remarks>
        /// Most tile Ids in the boss frame data come with offsets pre-baked in.
        /// These offsets need to be removed as all boss tilesets will start with index 0.
        /// 
        /// Shadow Tiles: The most common offset. The tiles for the shadow graphic take up tile Ids 0x00-0x20.
        /// Crystal Tileset: This tileset is loaded into memory starting at index 0x40. Unknown why.
        /// Explosion Tileset: This tileset is loaded into memory starting at index 0x80. Unknown why.
        /// Robot Auxiliary Tilesets: Auxiliary frames are offset by the size of the robot tile set (96) and the shadow tiles (32).
        /// Gigas Auxiliary Tilesets: Auxiliary frames are offset by the size of the shadow tiles (32) and ignoring the Extend Tile ID flag.
        /// 
        /// Skill Tilesets: All skill tilesets are loaded starting at index 0x80.
        /// 
        /// Lastly, an extension rather then an offset, if ExtendTileId is set, then add 0x100 to the tile Id to access the upper half of the tiles.
        /// Do not do this for the Crystal and Explosion tilesets.
        /// </remarks>
        public ushort CalculateTileId(int index, bool isSkill = false)
        {
            ushort tileId = this.TileId;
            if (!isSkill)
            {
                BossTilesetMetadata metadata = ManaMetadata.BossTilesetMetadata[index];
                if (metadata.IsOffsetByShadowTiles) { tileId -= ManaMetadata.BossShadowTileOffset; }

                if (metadata.Family == BossFamily.Crystal) { tileId -= ManaMetadata.BossCrystalTilesetOffset; }
                else if (metadata.Family == BossFamily.Explosion) { tileId -= ManaMetadata.BossExplosionTilesetOffset; }
                else if (metadata.Family == BossFamily.RobotAuxiliaryHammer) { tileId -= ManaMetadata.BossRobotAuxiliaryTilesetOffset; }
                else if (metadata.Family == BossFamily.RobotAuxiliaryChainsaw) { tileId -= ManaMetadata.BossRobotAuxiliaryTilesetOffset; }
                else if (metadata.Family == BossFamily.GigasAuxiliaryDiamond) { tileId -= ManaMetadata.BossGigasAuxiliaryTilesetOffset; }
                else if (metadata.Family == BossFamily.GigasAuxiliaryOrb) { tileId -= ManaMetadata.BossGigasAuxiliaryTilesetOffset; }
                else if (this.ExtendTileId) { tileId += ManaMetadata.BossExtendTileIdAmount; }

                return tileId;
            }
            tileId -= ManaMetadata.BossSkillTilesetOffset;
            return tileId;
        }
    }
}