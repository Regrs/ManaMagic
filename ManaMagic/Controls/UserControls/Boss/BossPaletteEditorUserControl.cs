using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Boss
{
    public partial class BossPaletteEditorUserControl : UserControl
    {
        private SpritePalette? palette;
        private bool ignoreEvents = false;

        public BossPaletteEditorUserControl()
        {
            InitializeComponent();
            this.indexNumericUpDown.Maximum = Constants.Bank02.BossPaletteTableAddressSize - 1;
        }

        private void BossPaletteEditorUserControl_Load(object sender, EventArgs e)
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

        private void SetPalette(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                SpritePalette palette = ManaMagicContext.Current.Context.BossContext.PaletteTable[index];
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
    }
}
