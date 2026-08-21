#nullable enable

namespace ManaMagic.Core.Menu
{
    public readonly struct RingIconDefinition
    {
        public byte IconIndex { get; }
        public byte PaletteIndex { get; }

        public RingIconDefinition(byte iconIndex, byte paletteIndex)
        {
            this.IconIndex = iconIndex;
            this.PaletteIndex = paletteIndex;
        }
    }
}