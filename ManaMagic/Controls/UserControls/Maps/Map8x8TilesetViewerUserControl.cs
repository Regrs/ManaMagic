using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Metadata;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class Map8x8TilesetViewerUserControl : UserControl, IManaControl
    {
        //private int tilesetInddex = 0;
        private int paletteSetIndex = 0;
        private int paletteIndex = 0;
        private bool updatingPalettes = false;

        public Map8x8TilesetViewerUserControl()
        {
            InitializeComponent();
            this.tilesetIdNumericUpDown.Maximum = ManaMetadata.Map8x8TilesetMetadata.RowCount - 1;
            this.paletteSetNumericUpDown.Maximum = Constants.Bank0C.MapPaletteSetTableSize - 1;
            this.paletteIndexNumericUpDown.Maximum = Constants.Bank0C.MapPalettesPerSet - 1;
        }

        public void SetIndex(int index)
        {
            this.tilesetIdNumericUpDown.Value = index;
        }

        private void UpdatePalettes()
        {
            this.updatingPalettes = true;

            this.paletteSetNumericUpDown.Value = this.paletteSetIndex;
            this.paletteIndexNumericUpDown.Value = this.paletteIndex;

            this.updatingPalettes = false;
        }

        private void UpdatePalettesFromMetadata()
        {
            int index = (int)this.tilesetIdNumericUpDown.Value;
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[index];
            this.paletteSetIndex = metadata.DefaultPaletteSet;
            this.paletteIndex = metadata.DefaultPaletteIndex;
            this.UpdatePalettes();
        }

        private void UpdateCanvas()
        {
            using SuperNintendoGraphics graphics = this.Draw8x8MapTileset();
            this.map8x8TilesetPictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        private SuperNintendoGraphics Draw8x8MapTileset()
        {
            int index = (int)this.tilesetIdNumericUpDown.Value;
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[index];
            if (!metadata.IsDummiedOut)
            {
                Tileset tileset = ManaMagicContext.Current.Context.MapContext.Map8x8TilesetTable[index];
                SpritePalette palette = ManaMagicContext.Current.Context.MapContext.PaletteSetTable[this.paletteSetIndex][this.paletteIndex];

                return tileset.DrawTileset(palette, 32);
            }
            return SuperNintendoGraphics.CreateGraphics(new SpritePalette(0, new List<Rgb555Color>() { Rgb555Color.Black }, 0), 1, 1);
        }

        private void Map8x8TilesetViewerUserControl_Load(object sender, EventArgs e)
        {
            this.UpdatePalettesFromMetadata();
            this.UpdateCanvas();
        }

        private void TilesetIdNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.UpdatePalettesFromMetadata();
            this.UpdateCanvas();
        }

        private void PaletteSetNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updatingPalettes)
            {
                this.paletteSetIndex = (int)paletteSetNumericUpDown.Value;
                this.UpdatePalettes();
                this.UpdateCanvas();
            }
        }

        private void PaletteIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updatingPalettes)
            {
                this.paletteIndex = (int)paletteIndexNumericUpDown.Value;
                this.UpdatePalettes();
                this.UpdateCanvas();
            }
        }
    }
}
/*
        private void map8x8TilesetNextButton_Click(object sender, EventArgs e)
        {
            //this.tilesetInddex++;
            //this.map8x8TilesetPreviousButton.Enabled = true;
            //if (this.tilesetInddex >= ManaMetadata.Map8x8TilesetMetadata.RowCount - 1)
            //{
            //    this.map8x8TilesetNextButton.Enabled = false;
            //}

            //this.UpdatePalettesFromMetadata();
            //this.UpdateCanvas();
        }

        private void map8x8TilesetPreviousButton_Click(object sender, EventArgs e)
        {
            //this.tilesetInddex--;
            //this.map8x8TilesetNextButton.Enabled = true;
            //if (this.tilesetInddex == 0)
            //{
            //    this.map8x8TilesetPreviousButton.Enabled = false;
            //}

            //this.UpdatePalettesFromMetadata();
            //this.UpdateCanvas();
        }
 */