using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ManaMagic.Controls
{
    [ToolboxItem(false)]
    internal sealed class PaletteLabel : Label
    {
        private bool selected = false;

        public bool Selected
        {
            get { return this.selected; }
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.Invalidate();
                }
            }
        }

        public int PaletteIndex { get; set; } = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Selected)
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, SystemColors.Highlight, ButtonBorderStyle.Outset);
            }
            base.OnPaint(e);
        }
    }
}
