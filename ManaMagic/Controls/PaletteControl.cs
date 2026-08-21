using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZwellTech;
using ZwellTech.SuperNintendo.Drawing;

namespace ManaMagic.Controls
{
    /// <summary>
    /// Represents a Windows control that displays a listing of palette colors.
    /// </summary>
    //[Designer(typeof(PaletteControlControlDesigner))]
    public sealed class PaletteControl : GroupBox
    {
        private const int DefaultLabelWidth = 17;
        private const int LabelXOffset = 6;
        private const int LabelYOffset = 12;
        private const AnchorStyles LabelStyle = AnchorStyles.Top | AnchorStyles.Bottom;
        private const BorderStyle LabelBorder = BorderStyle.Fixed3D;

        private int numPalettes = 8;
        private bool isMultiLine = false;
        private int labelWidth = PaletteControl.DefaultLabelWidth;
        private readonly List<Color> colors;

        /// <summary>
        /// Gets or sets the number of palettes to be displayed within the control.
        /// </summary>
        [Category("Layout")]
        [Description("Specifies the number of palettes to be displayed in the control.")]
        [DefaultValue(8)]
        public int NumberOfPalettes
        {
            get { return this.numPalettes; }
            set
            {
                if (this.numPalettes != value)
                {
                    this.numPalettes = value;
                    this.CreateLabelControls();
                }
            }
        }

        /// <summary>
        /// Gets or sets if the palette displays should be drawn on multiple lines.
        /// </summary>
        [Category("Layout")]
        [Description("Determines if a palette array of more than 8 colors should be displayed on multiple lines.")]
        [DefaultValue(false)]
        public bool MultiLine
        {
            get { return this.isMultiLine; }
            set
            {
                if (this.isMultiLine != value)
                {
                    this.isMultiLine = value;
                    this.CreateLabelControls();
                }
            }
        }

        /// <summary>
        /// Gets or sets if the individual palettes can be selected.
        /// </summary>
        [Category("Behavior")]
        [Description("Determines if the individual palettes can be selected.")]
        [DefaultValue(false)]
        public bool AllowSelection { get; set; } = false;

        /// <summary>
        /// Gets or sets if the individual palettes can be edited.
        /// </summary>
        [Category("Behavior")]
        [Description("Determines if the individual palettes can be edited.")]
        [DefaultValue(false)]
        public bool AllowEditing { get; set; } = false;

        [DefaultValue(PaletteControl.DefaultLabelWidth)]
        public int LabelWidth
        {
            get { return this.labelWidth; }
            set
            {
                this.labelWidth = value;
                this.RefreshLabelControls();
            }
        }

        /// <summary>
        /// Gets the list of colors this control will display.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<Color> Colors { get { return this.colors; } }

        public Color this[int index] { get { return this.Colors[index]; } }

        /// <summary>
        /// Occurs when one of the colors is edited.
        /// </summary>
        [Category("Property Changed")]
        [Description("Occurs when one of the colors is edited.")]
        public event EventHandler<ColorChanged> ColorChanged;

        /// <summary>
        /// Creates a new instance of the <see cref="PaletteControl"/> class.
        /// </summary>
        public PaletteControl()
        {
            this.colors = new List<Color>();
            this.CreateLabelControls();
        }

        /// <summary>
        /// Sets the color palette for this control.
        /// </summary>
        /// <param name="newColors">A collection of colors to be displayed within the control.</param>
        public void SetColorPalette(IEnumerable<Color> newColors)
        {
            ValidationHelper.ThrowIfArgumentNull(newColors);

            this.colors.Clear();
            this.colors.AddRange(newColors);

            this.RefreshLabelControls();
        }

        /// <summary>
        /// Sets the color palette for this control.
        /// </summary>
        /// <param name="palette">A <see cref="SpritePalette"/> containing the colors to display.</param>
        public void SetColorPalette(SpritePalette palette)
        {
            ValidationHelper.ThrowIfArgumentNull(palette);

            this.colors.Clear();

            foreach (Rgb555Color color in palette.Colors)
            {
                this.colors.Add(Color.FromArgb(color.ToArgb()));
            }

            this.RefreshLabelControls();
        }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        public override void Refresh()
        {
            this.RefreshLabelControls();
            base.Refresh();
        }

        /// <summary>
        /// Raises the <see cref="Control.Resize"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.RefreshLabelControls();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.RefreshLabelControls();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            int labelWidth = this.MultiLine ? (this.LabelWidth * (this.numPalettes / 2)) : (this.LabelWidth * this.numPalettes);
            width = (labelWidth) + (PaletteControl.LabelXOffset * 2);
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void OnColorChanged(int index)
        {
            this.ColorChanged?.Invoke(this, new ColorChanged(index));
        }

        private void CreateLabelControls()
        {
            this.SuspendLayout();
            this.Controls.Clear();

            for (int i = 0; i < this.numPalettes; i++)
            {
                PaletteLabel label = new PaletteLabel();

                label.PaletteIndex = i;
                label.Click += this.PaletteLabel_Click;
                label.DoubleClick += this.PaletteLabel_DoubleClick;

                this.Controls.Add(label);
            }

            this.RefreshLabelControls();
            this.ResumeLayout();
        }

        private void RefreshLabelControls()
        {
            this.SuspendLayout();
            this.SetBoundsCore(this.Location.X, this.Location.Y, 0, this.Height, BoundsSpecified.All);


            int xValue = PaletteControl.LabelXOffset;
            int yValue = PaletteControl.LabelYOffset + (string.IsNullOrWhiteSpace(this.Text) ? 0 : 6);
            int heightOffset = string.IsNullOrWhiteSpace(this.Text) ? 18 : 24;
            bool lineBreak = false;
            bool drawMultiLine = this.numPalettes > 8 && this.MultiLine;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is PaletteLabel label)
                {
                    label.Width = this.LabelWidth;
                    label.Height = drawMultiLine ? (this.Height - heightOffset) / 2 : this.Height - heightOffset;
                    label.Anchor = PaletteControl.LabelStyle;
                    label.BorderStyle = PaletteControl.LabelBorder;
                    label.Location = new Point(xValue, yValue);
                    label.BackColor = label.PaletteIndex < this.colors.Count ? this.colors[label.PaletteIndex] : SystemColors.Control;

                    xValue += this.LabelWidth;

                    if ((i >= 7) & drawMultiLine & !lineBreak)
                    {
                        xValue = PaletteControl.LabelXOffset;
                        yValue += label.Height;
                        lineBreak = true;
                    }
                }
            }

            this.ResumeLayout();
        }

        private void PaletteLabel_Click(object sender, EventArgs e)
        {
            if (this.AllowSelection)
            {
                if (sender is PaletteLabel clickedLabel)
                {
                    foreach (Control control in this.Controls)
                    {
                        if (control is PaletteLabel label)
                        {
                            label.Selected = label.PaletteIndex == clickedLabel.PaletteIndex;
                        }
                    }
                }
            }
        }

        private void PaletteLabel_DoubleClick(object sender, EventArgs e)
        {
            if (this.AllowSelection && this.AllowEditing && sender is PaletteLabel label)
            {
                using ColorDialog colorDialog = new ColorDialog();
                colorDialog.FullOpen = true;
                colorDialog.SolidColorOnly = true;
                colorDialog.Color = this.colors[label.PaletteIndex];
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Rgb555Color rgb555Color = Rgb555Color.FromArgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                    this.colors[label.PaletteIndex] = Color.FromArgb(rgb555Color.ToArgb());
                    label.BackColor = this.colors[label.PaletteIndex];
                    this.OnColorChanged(label.PaletteIndex);
                }
            }
        }
    }
}