using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents part of a boss background frame.
    /// </summary>
    public sealed record BossBackgroundFramePart : BossFramePart
    {
        /// <summary>
        /// Gets a value indicating if the frame part was compressed.
        /// </summary>
        public bool Compressed { get; init; }

        /// <summary>
        /// Gets the number of 8x8 columns in the frame.
        /// </summary>
        public byte ColumnCount { get; init; }

        /// <summary>
        /// Gets the number of 8x8 rows in the frame.
        /// </summary>
        public byte RowCount { get; init; }

        /// <summary>
        /// Gets the offset into Bank C1 where the tile data is located.
        /// </summary>
        public ushort TileDataOffset { get; init; }

        /// <summary>
        /// Gets the table of parts that make up this background frame part.
        /// </summary>
        public DataTable<BossSpriteFramePart> Parts { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossBackgroundFramePart"/> class.
        /// </summary>
        /// <param name="compressed">A value indicating if the frame part was compressed.</param>
        /// <param name="xCoordinate">The x-coordinate of the frame part.</param>
        /// <param name="yCoordinate">The y-coordinate of the frame part.</param>
        /// <param name="columnCount">The number of 8x8 columns in the frame.</param>
        /// <param name="rowCount">The number of 8x8 rows in the frame.</param>
        /// <param name="tileDataOffset">The pointer to the tile data in Bank C1.</param>
        /// <param name="parts">The table of tile ID/flags that make up the frame part.</param>
        public BossBackgroundFramePart(bool compressed, sbyte xCoordinate, sbyte yCoordinate, byte columnCount, byte rowCount, ushort tileDataOffset, DataTable<BossSpriteFramePart> parts) : base(xCoordinate, yCoordinate)
        {
            this.Compressed = compressed;
            this.ColumnCount = columnCount;
            this.RowCount = rowCount;
            this.TileDataOffset = tileDataOffset;
            this.Parts = parts;
        }
    }
}