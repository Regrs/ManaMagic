using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Menu
{
    public readonly struct RingIcon
    {
        public static RingIcon Empty { get; } = new RingIcon();

        private readonly bool initialized;

        public byte Index { get; }
        public DataTable<GraphicTile4Bpp> Tiles { get; }
        public SpritePalette Palette { get; }
        public RingIconDefinition Definition { get; }

        public bool IsEmpty { get { return !this.initialized; } }

        public RingIcon(byte index, DataTable<GraphicTile4Bpp> tiles, SpritePalette palette, RingIconDefinition definition)
        {
            this.Index = index;
            this.Tiles = tiles;
            this.Palette = palette;
            this.Definition = definition;

            this.initialized = true;
        }

        public SuperNintendoGraphics Draw()
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(this.Palette, 16, 16);
            graphics.Draw16x16Tile(this.Tiles.Rows, 0, 0);

            return graphics;
        }
    }
}