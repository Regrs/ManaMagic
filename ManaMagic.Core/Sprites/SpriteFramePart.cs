using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed class SpriteFramePart
    {
        public ushort TopLeftOffset { get; }
        public ushort TopRightOffset { get; }
        public ushort BottomLeftOffset { get; }
        public ushort BottomRightOffset { get; }

        public sbyte XCoordinate { get; }
        public sbyte YCoordinate { get; }

        public FlipType FlipFlags { get; }

        public SpriteFramePart(ushort topLeft, ushort topRight, ushort bottomLeft, ushort bottomRight, sbyte xCoordinate, sbyte yCoordinate, FlipType flags)
        {
            this.TopLeftOffset = topLeft;
            this.TopRightOffset = topRight;
            this.BottomLeftOffset = bottomLeft;
            this.BottomRightOffset = bottomRight;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
            this.FlipFlags = flags;
        }
    }
}