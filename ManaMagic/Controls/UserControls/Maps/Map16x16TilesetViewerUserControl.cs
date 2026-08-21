using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class Map16x16TilesetViewerUserControl : UserControl, IManaControl
    {
        private int tileset8x8Index = 0;
        private int tileset16x16Index = 0;
        private int paletteSetIndex = 0;
        private bool updatingUI = false;

        public Map16x16TilesetViewerUserControl()
        {
            InitializeComponent();
        }

        public void SetIndex(int index)
        {
            this.tileset8x8Index = index;
            this.tileset16x16Index = index;
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[this.tileset8x8Index];
            this.paletteSetIndex = metadata.DefaultPaletteSet;

            this.updatingUI = true;
            this.tileset8x8NumericUpDown.Value = this.tileset16x16Index;
            this.tileset16x16NumericUpDown.Value = this.tileset16x16Index;
            this.paletteSetNumericUpDown.Value = this.paletteSetIndex;
            this.updatingUI = false;

            this.UpdateCanvas();
        }
        private void UpdateCanvas()
        {
            using SuperNintendoGraphics layer1 = this.Draw16x16MapTileset(false);
            using SuperNintendoGraphics layer2 = this.Draw16x16MapTileset(true);
            this.layer1PictureBox.Image = new Bitmap(layer1.GetBitmap(true), layer1.Size.Width * 2, layer1.Size.Height * 2);
            this.layer2PictureBox.Image = new Bitmap(layer2.GetBitmap(true), layer2.Size.Width * 2, layer2.Size.Height * 2);
        }

        private SuperNintendoGraphics Draw16x16MapTileset(bool layer2)
        {
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[this.tileset8x8Index];
            if (!metadata.IsDummiedOut)
            {
                Tileset tileset8x8 = ManaMagicContext.Current.Context.MapContext.Map8x8TilesetTable[this.tileset8x8Index];
                DataTable<Map16x16Tile> tileset16x16 = ManaMagicContext.Current.Context.MapContext.Map16x16TilesetTable[this.tileset16x16Index];
                DataTable<SpritePalette> paletteSet = ManaMagicContext.Current.Context.MapContext.PaletteSetTable[this.paletteSetIndex];

                return Map16x16TilesetViewerUserControl.Draw16x16MapTileset(layer2, tileset8x8, tileset16x16, paletteSet);
            }
            return SuperNintendoGraphics.CreateGraphics(new SpritePalette(0, new List<Rgb555Color>() { Rgb555Color.Black }, 0), 1, 1);
        }

        private static SuperNintendoGraphics Draw16x16MapTileset(bool layer2, Tileset map8x8Tileset, DataTable<Map16x16Tile> map16x16Tileset, DataTable<SpritePalette> paletteSet)
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, 32 * 16, 32 * 12);

            int rowIndex = 0;
            int columnIndex = 0;
            int startIndex = !layer2 ? 0 : 192;
            int endIndex = !layer2 ? 192 : 384;
            for (int i = startIndex; i < endIndex; i++)
            {
                Map16x16Tile tile16x16 = map16x16Tileset[i];
                Map16x16TileQuadrant quadrant1 = tile16x16.Quadrants[0];
                Map16x16TileQuadrant quadrant2 = tile16x16.Quadrants[1];
                Map16x16TileQuadrant quadrant3 = tile16x16.Quadrants[2];
                Map16x16TileQuadrant quadrant4 = tile16x16.Quadrants[3];

                GraphicTile tile1 = quadrant1.TileIndex < map8x8Tileset.Count ? map8x8Tileset[quadrant1.TileIndex] : GraphicTile4Bpp.Empty;
                GraphicTile tile2 = quadrant2.TileIndex < map8x8Tileset.Count ? map8x8Tileset[quadrant2.TileIndex] : GraphicTile4Bpp.Empty;
                GraphicTile tile3 = quadrant3.TileIndex < map8x8Tileset.Count ? map8x8Tileset[quadrant3.TileIndex] : GraphicTile4Bpp.Empty;
                GraphicTile tile4 = quadrant4.TileIndex < map8x8Tileset.Count ? map8x8Tileset[quadrant4.TileIndex] : GraphicTile4Bpp.Empty;

                graphics.DrawTile(tile1, (rowIndex * 16), (columnIndex * 16), quadrant1.FlipType, quadrant1.ModifiedPaletteIndex);
                graphics.DrawTile(tile2, (rowIndex * 16) + 8, (columnIndex * 16), quadrant2.FlipType, quadrant2.ModifiedPaletteIndex);
                graphics.DrawTile(tile3, (rowIndex * 16), (columnIndex * 16) + 8, quadrant3.FlipType, quadrant3.ModifiedPaletteIndex);
                graphics.DrawTile(tile4, (rowIndex * 16) + 8, (columnIndex * 16) + 8, quadrant4.FlipType, quadrant4.ModifiedPaletteIndex);

                rowIndex++;
                // Wrap around to the start of the next row if we have reached the end of the current row.
                if (rowIndex == 16)
                {
                    rowIndex = 0;
                    columnIndex++;
                }
            }
            return graphics;
        }

        private void Map16x16TilesetViewerUserControl_Load(object sender, EventArgs e)
        {
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[this.tileset8x8Index];
            this.paletteSetIndex = metadata.DefaultPaletteSet;

            this.UpdateCanvas();

            this.updatingUI = true;
            this.tileset8x8NumericUpDown.Value = this.tileset8x8Index;
            this.tileset16x16NumericUpDown.Value = this.tileset16x16Index;
            this.paletteSetNumericUpDown.Value = this.paletteSetIndex;
            this.updatingUI = false;
        }

        private void Tileset8x8NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.tileset8x8Index = (int)this.tileset8x8NumericUpDown.Value;
                MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[this.tileset8x8Index];
                this.paletteSetIndex = metadata.DefaultPaletteSet;
                this.UpdateCanvas();
            }
        }

        private void Tileset16x16NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.tileset16x16Index = (int)this.tileset16x16NumericUpDown.Value;
                MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[this.tileset8x8Index];
                this.paletteSetIndex = metadata.DefaultPaletteSet;
                this.UpdateCanvas();
            }
        }

        private void PaletteSetNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.paletteSetIndex = (int)this.paletteSetNumericUpDown.Value;
                this.UpdateCanvas();
            }
        }
    }
}