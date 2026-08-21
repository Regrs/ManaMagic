using System.Collections.Generic;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents the base class for boss frame data.
    /// </summary>
    public abstract record BossFrame
    {
        /// <summary>
        /// Gets the <see cref="SpritePalette"/> used to color the background of the boss frame.
        /// </summary>
        /// <remarks>Not currently used.</remarks>
        protected static SpritePalette BackgroundPalette { get; } = new SpritePalette(0, new List<Rgb555Color>() { Rgb555Color.Navy, Rgb555Color.Black, Rgb555Color.White });

        /// <summary>
        /// Gets the index of the frame.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// Gets the type of this frame.
        /// </summary>
        public BossFrameType FrameType { get; init; }

        /// <summary>
        /// Gets the name of the frame.
        /// </summary>
        public string DisplayName { get; init; }

        /// <summary>
        /// Gets the ROM address of the frame.
        /// </summary>
        public int Address { get; init; }

        /// <summary>
        /// Gets the length (in frame parts) of the frame.
        /// </summary>
        public int Length { get; init; }

        /// <summary>
        /// Gets a data table of the individual parts of the frame.
        /// </summary>
        public DataTable<BossFramePart> Parts { get; init; }

        public BossHitBox HitBox { get; init; }
        public BossHitBox WeaponBox { get; init; }
        public BossHitBox GuardBox { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossFrame"/> class with the specified index, frame type, displayName, address, length and parts list.
        /// </summary>
        /// <param name="index">The index of the frame.</param>
        /// <param name="frameType">The type of frame.</param>
        /// <param name="displayName">The name of the frame.</param>
        /// <param name="address">The address of the frame.</param>
        /// <param name="length">The number of parts in the frame.</param>
        /// <param name="parts">The list of parts that make up the frame.</param>
        public BossFrame(int index, BossFrameType frameType, string displayName, int address, int length, IReadOnlyList<BossFramePart> parts, BossHitBox hitBox, BossHitBox weaponBox, BossHitBox guardBox)
        {
            this.Index = index;
            this.FrameType = frameType;
            this.DisplayName = displayName;
            this.Address = address;
            this.Length = length;
            this.Parts = new DataTable<BossFramePart>(parts);

            this.HitBox = hitBox;
            this.WeaponBox = weaponBox;
            this.GuardBox = guardBox;
        }

        /// <summary>
        /// Draws the boss frame using the specified <see cref="Tileset"/> and <see cref="SpritePalette"/> table.
        /// </summary>
        /// <param name="bossTileset">The tileset used to draw the frame.</param>
        /// <param name="paletteTable">A list of palettes to color the frame.</param>
        /// <param name="gridlines">If true then draw 8x8 gridlines on the background of the frame.</param>
        /// <returns>A <see cref="SuperNintendoGraphics"/> object containing the drawn boss frame.</returns>
        public abstract SuperNintendoGraphics DrawFrame(Tileset tileset, IReadOnlyList<SpritePalette> paletteTable, FrameDrawingOptions options);
    }
}