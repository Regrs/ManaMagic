using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class MapPaletteEditorUserControl : UserControl
    {
        private DataTable<SpritePalette>? paletteSet;
        private bool ignoreEvents = false;

        public MapPaletteEditorUserControl()
        {
            InitializeComponent();

            this.ignoreEvents = true;
            this.indexNumericUpDown.Minimum = Constants.Bank0C.FirstValidMapPaletteIndex;
            this.indexNumericUpDown.Maximum = Constants.Bank0C.MapPaletteSetTableSize - 1;
            this.index0PaletteControl.Tag = 0;
            this.index1PaletteControl.Tag = 1;
            this.index2PaletteControl.Tag = 2;
            this.index3PaletteControl.Tag = 3;
            this.index4PaletteControl.Tag = 4;
            this.index5PaletteControl.Tag = 5;
            this.index6PaletteControl.Tag = 6;

            // Can't edit these until the RGB1555 issue is resolved.
            this.DisablePaletteEditing();
            this.ignoreEvents = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (int)ExtendedWindowStyles.WS_EX_COMPOSITED;
                return cp;
            }
        }

        private void DisablePaletteEditing()
        {
            this.index0PaletteControl.AllowSelection = false;
            this.index1PaletteControl.AllowSelection = false;
            this.index2PaletteControl.AllowSelection = false;
            this.index3PaletteControl.AllowSelection = false;
            this.index4PaletteControl.AllowSelection = false;
            this.index5PaletteControl.AllowSelection = false;
            this.index6PaletteControl.AllowSelection = false;
            this.index0PaletteControl.AllowEditing = false;
            this.index1PaletteControl.AllowEditing = false;
            this.index2PaletteControl.AllowEditing = false;
            this.index3PaletteControl.AllowEditing = false;
            this.index4PaletteControl.AllowEditing = false;
            this.index5PaletteControl.AllowEditing = false;
            this.index6PaletteControl.AllowEditing = false;
        }

        private void SetPalette(int index)
        {
            if (!this.ignoreEvents && ManaMagicContext.Current.Context.Loaded)
            {
                DataTable<SpritePalette> paletteSet = ManaMagicContext.Current.Context.MapContext.PaletteSetTable[index];
                this.SetPalette(paletteSet);
            }
        }

        private void SetPalette(DataTable<SpritePalette> paletteSet)
        {
            this.ignoreEvents = true;

            this.paletteSet = paletteSet;
            this.index0PaletteControl.SetColorPalette(paletteSet[0]);
            this.index1PaletteControl.SetColorPalette(paletteSet[1]);
            this.index2PaletteControl.SetColorPalette(paletteSet[2]);
            this.index3PaletteControl.SetColorPalette(paletteSet[3]);
            this.index4PaletteControl.SetColorPalette(paletteSet[4]);
            this.index5PaletteControl.SetColorPalette(paletteSet[5]);
            this.index6PaletteControl.SetColorPalette(paletteSet[6]);

            this.ignoreEvents = false;
        }

        private void MapPaletteEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetPalette(0);
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetPalette((int)this.indexNumericUpDown.Value);
        }

        private void PaletteControl_ColorChanged(object sender, ColorChanged e)
        {
            if (!this.ignoreEvents && this.paletteSet != null)
            {
                if (sender is PaletteControl paletteControl && paletteControl.Tag is int setIndex)
                {
                    this.paletteSet[setIndex].SetColor(e.Index, paletteControl[e.Index]);
                }
            }
        }
    }
}