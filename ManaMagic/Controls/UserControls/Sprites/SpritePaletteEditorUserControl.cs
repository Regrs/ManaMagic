using System;
using System.Windows.Forms;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Sprites
{
    public partial class SpritePaletteEditorUserControl : UserControl
    {
        private SpritePalette? palette;
        private bool ignoreEvents = false;

        public SpritePaletteEditorUserControl()
        {
            InitializeComponent();
        }

        private void SetPalette(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                SpritePalette palette = ManaMagicContext.Current.Context.SpriteContext.PaletteTable[index];
                this.SetPalette(palette);
            }
        }

        private void SetPalette(SpritePalette palette)
        {
            this.ignoreEvents = true;
            this.palette = palette;
            this.paletteControl.SetColorPalette(palette);
            this.ignoreEvents = false;
        }

        private void SpritePaletteEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetPalette(0);
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetPalette((int)this.indexNumericUpDown.Value);
        }

        private void PaletteControl_ColorChanged(object sender, ColorChanged e)
        {
            if (!this.ignoreEvents && this.palette != null)
            {
                this.palette.SetColor(e.Index, this.paletteControl[e.Index]);
            }
        }
    }
}