using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Maps
{
    public sealed record Map16x16TileQuadrant(bool VerticalFlip, bool HorizontalFlip, bool Priority, byte PaletteIndex, ushort TileIndex)
    {
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

        public byte ModifiedPaletteIndex
        {
            get
            {
                if (this.PaletteIndex > 0) { return (byte)(this.PaletteIndex - 1); }
                return this.PaletteIndex;
            }
        }
    }
}