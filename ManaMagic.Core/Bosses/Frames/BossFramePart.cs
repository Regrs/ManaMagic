#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents the base class for all boss frame parts.
    /// </summary>
    public abstract record BossFramePart
    {
        /// <summary>
        /// Gets the X-Coordinate of the frame part. The coordinate is relative to the bosses origin.
        /// </summary>
        public sbyte XCoordinate { get; init; }

        /// <summary>
        /// Gets the Y-Coordinate of the frame part. The coordinate is relative to the bosses origin.
        /// </summary>
        public sbyte YCoordinate { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossFramePart"/> class.
        /// </summary>
        public BossFramePart() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossFramePart"/> class with the specified coordinates.
        /// </summary>
        /// <param name="xCoordinate">The X-Coordinate of the frame part, relative to the bosses origin.</param>
        /// <param name="yCoordinate">The Y-Coordinate of the frame part, relative to the bosses origin.</param>
        public BossFramePart(sbyte xCoordinate, sbyte yCoordinate)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }
    }
}