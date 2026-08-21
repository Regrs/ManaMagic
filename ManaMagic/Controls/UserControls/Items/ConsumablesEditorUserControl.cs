using System;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Items;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Items
{
    public partial class ConsumablesEditorUserControl : UserControl
    {
        private ManaItem? item = null;
        private bool ignoreEvents = false;

        public ConsumablesEditorUserControl()
        {
            InitializeComponent();
            this.indexNumericUpDown.Maximum = Constants.Bank10.ItemDefinitionTableSize - 1;
        }

        private void SetConsumable(ManaItem item)
        {
            this.nameChangeTimer.Stop();
            this.ignoreEvents = true;

            this.item = item;
            this.nameTextBox.Text = ManaUtil.GetNameFromEvent(item.Name, out bool canEditName);
            this.nameTextBox.Enabled = canEditName;

            this.amountHealedNumericUpDown.Value = item.Definition.AmountHealed;
            this.paletteIndexNumericUpDown.Value = item.Definition.PaletteIndex;

            this.iconPictureBox.Image?.Dispose();
            this.iconPictureBox.Image = null;
            if (!item.Icon.IsEmpty)
            {
                using SuperNintendoGraphics graphics = item.Icon.Draw();
                this.iconPictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }

            this.ignoreEvents = false;
        }

        private void ConsumablesEditorUserControl_Load(object sender, EventArgs e)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaItem item = ManaMagicContext.Current.Context.ItemContext.Items[(int)this.indexNumericUpDown.Value];
                this.SetConsumable(item);
            }
        }

        private void NameChangeTimer_Tick(object sender, EventArgs e)
        {
            this.nameChangeTimer.Stop();
            if (this.ignoreEvents)
            {
                this.nameChangeTimer.Start();
                return;
            }

            if (this.item != null)
            {
                this.item.SetName(this.nameTextBox.Text);
            }
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.nameChangeTimer.Stop();
            this.nameChangeTimer.Start();
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaItem item = ManaMagicContext.Current.Context.ItemContext.Items[(int)this.indexNumericUpDown.Value];
                this.SetConsumable(item);
            }
        }

        private void AmountHealedNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.ignoreEvents && this.item != null)
            {
                this.item.Definition.AmountHealed = (byte)this.amountHealedNumericUpDown.Value;
            }
        }

        private void PaletteIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this.ignoreEvents && this.item != null)
            {
                this.item.Definition.PaletteIndex = (byte)this.paletteIndexNumericUpDown.Value;
            }
        }
    }
}