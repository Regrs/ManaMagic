using System;

#nullable enable

namespace ZwellTech.Windows.Forms
{
    /// <summary>
    /// Provides a bitwise combination of flags to set a TreeView's style.
    /// </summary>
    [Flags]
    public enum TreeViewStyles
    {
        /// <summary>
        /// Specifies how the background is erased or filled.
        /// </summary>
        DoubleBuffer = 0x0004,
        /// <summary>
        /// Do not indent the tree view for the expando buttons.
        /// </summary>
        NoIndent = 0x0008,
        /// <summary>
        /// Remove the horizontal scroll bar and auto-scroll depending on mouse position.
        /// </summary>
        AutoHorizontalScroll = 0x0020,
        /// <summary>
        /// Fade expando buttons in or out when the mouse moves away or into a state of hovering over the control.
        /// </summary>
        ExpandoFadeEffects = 0x0040,
        /// <summary>
        /// Causes the item being selected to expand and the item being unselected to collapse upon selection in the tree view. If the user holds down the CTRL key while selecting an item, the item being unselected will not be collapsed.
        /// </summary>
        SingleExpand = 0x0400,
    }
}