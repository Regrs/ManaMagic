using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZwellTech.SuperNintendo.Drawing;

#pragma warning disable IDE1006 // Naming Styles

namespace ManaMagic.Controls.UserControls
{
    public partial class PaletteSelectorUserControl : UserControl
    {
        private bool updatingUI = false;

        public event EventHandler SelectedIndexChanged;

        public int PaletteMinimum
        {
            get { return (int)this.paletteSelectorNumericUpDown.Minimum; }
            set { this.paletteSelectorNumericUpDown.Minimum = value; }
        }

        public int PaletteMaximum
        {
            get { return (int)this.paletteSelectorNumericUpDown.Maximum; }
            set { this.paletteSelectorNumericUpDown.Maximum = value; }
        }

        public int SelectedIndex
        {
            get { return (int)this.paletteSelectorNumericUpDown.Value; }
        }

        public PaletteSelectorUserControl()
        {
            InitializeComponent();
        }

        public void SetColorPalette(int index, IEnumerable<Rgb555Color> newColors)
        {
            this.updatingUI = true;
            this.paletteControl.SetColorPalette(newColors.Select(color => Color.FromArgb(color.ToArgb())));
            this.paletteSelectorNumericUpDown.Value = index;
            this.updatingUI = false;
        }

        public void SetSelectionState(bool enabled)
        {
            this.paletteSelectorNumericUpDown.Enabled = enabled;
            this.changePaletteButton.Enabled = enabled;
        }

        private void changePaletteButton_Click(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.OnSelectedIndexChanged();
            }
        }

        private void paletteSelectorNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.OnSelectedIndexChanged();
            }
        }

        private void OnSelectedIndexChanged()
        {
            this.SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
