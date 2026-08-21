using System.Windows.Forms.Design;
//using Microsoft.DotNet.DesignTools.Designers;

namespace ManaMagic.Controls
{
    internal sealed class PaletteControlControlDesigner : ControlDesigner
    {
        //public override SelectionRules SelectionRules
        //{
        //    get { return SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.Moveable; }
        //}

        public PaletteControlControlDesigner()
        {
            base.AutoResizeHandles = true;
        }
    }
}