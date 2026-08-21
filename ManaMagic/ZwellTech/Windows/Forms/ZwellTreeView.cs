using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using ZwellTech.Interop.UxTheme;
using ZwellTech.Interop.Win32;

#nullable enable

namespace ZwellTech.Windows.Forms
{
    /// <summary>
    /// Displays a hierachical colection of labeled items, each represented by a <see cref="System.Windows.Forms.TreeNode"/>.
    /// </summary>
    [ToolboxBitmap(typeof(TreeView))]
    [DefaultEvent("AfterSelect"), DefaultProperty("Nodes")]
    [Description("Displays a hierachical colection of labeled items to the user that optionally contain an image.")]
    public class ZwellTreeView : TreeView
    {
        private bool m_fullRowSelect = true;
        private bool m_fadeEffects = true;
        private bool m_singleClickExpand = false;

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight spans the width of the tree view control.
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether the selection highlight spans the width of the tree view control.")]
        public new bool FullRowSelect
        {
            get { return this.m_fullRowSelect; }
            set
            {
                this.m_fullRowSelect = value;
                base.FullRowSelect = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether fade effects are applyed to the expando buttons of the tree view control.
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether fade effects are applyed to the expando buttons of the tree view control.")]
        public bool FadeEffects
        {
            get { return this.m_fadeEffects; }
            set
            {
                if (this.FadeEffects != value)
                {
                    this.m_fadeEffects = value;
                    this.UpdateStyleSettings();
                }
            }
        }
        
        /// <summary>
        /// Indicates whether a single click will expand the selected node and collapse all others.
        /// </summary>
        [Category("Behavior"), DefaultValue(false)]
        [Description("Indicates whether a single click will expand the selected node and collapse all others.")]
        public bool SingleClickExpand
        {
            get { return this.m_singleClickExpand; }
            set
            {
                if (this.SingleClickExpand != value)
                {
                    this.m_singleClickExpand = value;
                    this.UpdateStyleSettings();
                }
            }
        }

        /// <summary>
        /// Indicates whether lines are displayed between sibling nodes and between parent and child nodes.
        /// </summary>
        [Browsable(false), Bindable(false), DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool ShowLines
        {
            get { return base.ShowLines; }
            set { base.ShowLines = value; }
        }

        /// <summary>
        /// Removes highlight from the selected node when control does not have focus.
        /// </summary>
        [Browsable(false), Bindable(false), DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool HideSelection
        {
            get { return base.HideSelection; }
            set { base.HideSelection = value; }
        }

        /// <summary>
        /// Indicates whether nodes give feedback when the mouse is moved over them.
        /// </summary>
        [Browsable(false), Bindable(false), DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool HotTracking
        {
            get { return base.HotTracking; }
            set { base.HotTracking = value; }
        }

        /// <summary>
        /// Gets if the control can apply its visual styles.
        /// </summary>
        protected static bool CanApplyStyles
        {
            [SecurityCritical]
            get
            {
                if (Environment.OSVersion.Version.Major < 6) { return false; }
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DotSlash.Windows.Forms.TreeViewEx" />.
        /// </summary>
        public ZwellTreeView()
        {
            this.ShowLines = false;
            this.HideSelection = false;
            this.HotTracking = true;
            this.FullRowSelect = true;

            this.UpdateStyleSettings();
        }

        /// <summary>
        /// Creates a handle for the control.
        /// </summary>
        [SecuritySafeCritical]
        protected override void CreateHandle()
        {
            base.CreateHandle();

            if (ZwellTreeView.CanApplyStyles)
            {
                UXThemeManager.SetWindowTheme(this.Handle, "explorer", null);

                IntPtr lParam = UXThemeManager.SendMessage(base.Handle, TreeViewMessage.GetExtendedStyle, IntPtr.Zero, IntPtr.Zero);
                UXThemeManager.SendMessage(base.Handle, TreeViewMessage.SetExtendedStyle, IntPtr.Zero, lParam);
            }
        }

        private void UpdateStyleSettings()
        {
            TreeViewStyles extendedStyles = (TreeViewStyles)UXThemeManager.SendMessage(this.Handle, TreeViewMessage.GetExtendedStyle, IntPtr.Zero, IntPtr.Zero).ToInt32();
            extendedStyles |= TreeViewStyles.DoubleBuffer;
            extendedStyles |= TreeViewStyles.AutoHorizontalScroll;
            extendedStyles = this.FadeEffects ? (extendedStyles | TreeViewStyles.ExpandoFadeEffects) : (extendedStyles & ~TreeViewStyles.ExpandoFadeEffects);

            UXThemeManager.SendMessage(this.Handle, TreeViewMessage.SetExtendedStyle, IntPtr.Zero, new IntPtr((int)extendedStyles));

            TreeViewStyles windowAttribute = (TreeViewStyles)UXThemeManager.GetWindowAttribute(new HandleRef(this, this.Handle), new IntPtr(-16)).ToInt32();
            windowAttribute |= TreeViewStyles.NoIndent;
            windowAttribute = this.SingleClickExpand ? (windowAttribute | TreeViewStyles.SingleExpand) : (windowAttribute & ~TreeViewStyles.SingleExpand);

            UXThemeManager.SetWindowAttribute(new HandleRef(this, this.Handle), new IntPtr(-16), new IntPtr((int)windowAttribute));

            this.Invalidate();
        }
    }
}