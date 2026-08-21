using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable enable

// https://stackoverflow.com/questions/18145522/how-to-create-datagridview-numericupdown-column
// https://learn.microsoft.com/en-us/previous-versions/aa730881(v=vs.80)?redirectedfrom=MSDN

namespace ZwellTech.Windows.Forms
{
    /// <summary>
    /// Defines a NumericUpDown cell type for the System.Windows.Forms.DataGridView control
    /// </summary>
    public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    {
        // Used in KeyEntersEditMode function
        [DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern short VkKeyScan(char key);

        internal const int DefaultDecimalPlaces = 0;
        internal const decimal DefaultIncrement = decimal.One;
        internal const decimal DefaultMaximum = 100.0m;
        internal const decimal DefaultMinimum = decimal.Zero;
        internal const bool DefaultThousandsSeparator = false;

        private const int RenderingBitmapWidth = 100;
        private const int RenderingBitmapHeight = 22;
        private const DataGridViewContentAlignment AnyRight = DataGridViewContentAlignment.TopRight | DataGridViewContentAlignment.MiddleRight | DataGridViewContentAlignment.BottomRight;
        private const DataGridViewContentAlignment AnyCenter = DataGridViewContentAlignment.TopCenter | DataGridViewContentAlignment.MiddleCenter | DataGridViewContentAlignment.BottomCenter;

        [ThreadStatic]
        private static Bitmap? renderingBitmap;

        [ThreadStatic]
        private static NumericUpDown? paintingNumericUpDown;

        private int decimalPlaces = DataGridViewNumericUpDownCell.DefaultDecimalPlaces;
        private decimal increment = DataGridViewNumericUpDownCell.DefaultIncrement;
        private decimal minimum = DataGridViewNumericUpDownCell.DefaultMinimum;
        private decimal maximum = DataGridViewNumericUpDownCell.DefaultMaximum;
        private bool thousandsSeparator = DataGridViewNumericUpDownCell.DefaultThousandsSeparator;

        /// <summary>
        /// Gets or sets the number of decimal places to display in the spin box (also known as an up-down control).
        /// </summary>
        [DefaultValue(DataGridViewNumericUpDownCell.DefaultDecimalPlaces)]
        public int DecimalPlaces
        {

            get { return this.decimalPlaces; }
            set
            {
                if (value < 0 || value > 99)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), "The DecimalPlaces property cannot be smaller than 0 or larger than 99.");
                }
                if (this.decimalPlaces != value)
                {
                    this.SetDecimalPlaces(this.RowIndex, value);
                    this.OnCommonChange();  // Assure that the cell or column gets repainted and autosized if needed
                }
            }
        }

        /// <summary>
        /// Gets or sets the value to increment or decrement the spin box (also known as an up-down control) when the up or down buttons are clicked.
        /// </summary>
        public decimal Increment
        {
            get { return this.increment; }
            set
            {
                if (value < 0.0m)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value), "The Increment property cannot be smaller than 0.");
                }
                this.SetIncrement(this.RowIndex, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum value for the spin box (also known as an up-down control).
        /// </summary>
        public decimal Maximum
        {
            get { return this.maximum; }
            set
            {
                if (this.maximum != value)
                {
                    this.SetMaximum(this.RowIndex, value);
                    this.OnCommonChange();
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the spin box (also known as an up-down control).
        /// </summary>
        public decimal Minimum
        {
            get { return this.minimum; }
            set
            {
                if (this.minimum != value)
                {
                    this.SetMinimum(this.RowIndex, value);
                    this.OnCommonChange();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a thousands separator is displayed in the spin box (also known as an up-down control) when appropriate.
        /// </summary>
        [DefaultValue(DataGridViewNumericUpDownCell.DefaultThousandsSeparator)]
        public bool ThousandsSeparator
        {
            get { return this.thousandsSeparator; }
            set
            {
                if (this.thousandsSeparator != value)
                {
                    this.SetThousandsSeparator(this.RowIndex, value);
                    this.OnCommonChange();
                }
            }
        }

        /// <inheritdoc/>
        public override Type ValueType { get { return typeof(decimal); } }

        /// <inheritdoc/>
        public override Type EditType { get { return typeof(DataGridViewNumericUpDownEditingControl); } }

        private DataGridViewNumericUpDownEditingControl EditingNumericUpDown { get { return (DataGridViewNumericUpDownEditingControl)this.DataGridView!.EditingControl; } }

        /// <summary>
        /// Constructor for the DataGridViewNumericUpDownCell cell type
        /// </summary>
        public DataGridViewNumericUpDownCell()
        {
            if (DataGridViewNumericUpDownCell.renderingBitmap == null)
            {
                DataGridViewNumericUpDownCell.renderingBitmap = new Bitmap(DataGridViewNumericUpDownCell.RenderingBitmapWidth, DataGridViewNumericUpDownCell.RenderingBitmapHeight);
            }

            if (DataGridViewNumericUpDownCell.paintingNumericUpDown == null)
            {
                DataGridViewNumericUpDownCell.paintingNumericUpDown = new NumericUpDown();
                DataGridViewNumericUpDownCell.paintingNumericUpDown.BorderStyle = BorderStyle.None;
                DataGridViewNumericUpDownCell.paintingNumericUpDown.Maximum = decimal.MaxValue / 10;
                DataGridViewNumericUpDownCell.paintingNumericUpDown.Minimum = decimal.MinValue / 10;
            }
        }

        /// <inheritdoc/>
        public override object? Clone()
        {
            DataGridViewNumericUpDownCell? dataGridViewCell = (DataGridViewNumericUpDownCell)base.Clone();
            if (dataGridViewCell != null)
            {
                dataGridViewCell.DecimalPlaces = this.DecimalPlaces;
                dataGridViewCell.Increment = this.Increment;
                dataGridViewCell.Maximum = this.Maximum;
                dataGridViewCell.Minimum = this.Minimum;
                dataGridViewCell.ThousandsSeparator = this.ThousandsSeparator;
            }
            return dataGridViewCell;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void DetachEditingControl()
        {
            DataGridView? dataGridView = this.DataGridView;
            if (dataGridView == null || dataGridView.EditingControl == null)
            {
                ThrowHelper.ThrowInvalidOperationException("Cell is detached or its grid has no editing control.");
            }

            if (dataGridView.EditingControl is NumericUpDown numericUpDown)
            {
                if (numericUpDown.Controls[1] is TextBox textBox)
                {
                    textBox.ClearUndo();
                }
            }

            base.DetachEditingControl();
        }

        /// <inheritdoc/>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            if (this.DataGridView?.EditingControl is NumericUpDown numericUpDown)
            {
                numericUpDown.BorderStyle = BorderStyle.None;
                numericUpDown.DecimalPlaces = this.DecimalPlaces;
                numericUpDown.Increment = this.Increment;
                numericUpDown.Maximum = this.Maximum;
                numericUpDown.Minimum = this.Minimum;
                numericUpDown.ThousandsSeparator = this.ThousandsSeparator;

                if (initialFormattedValue is string initialFormattedValueStr) { numericUpDown.Text = initialFormattedValueStr; }
                else { numericUpDown.Text = string.Empty; }
            }
        }

        /// <inheritdoc/>
        public override bool KeyEntersEditMode(KeyEventArgs e)
        {
            //NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            //Keys negativeSignKey = Keys.None;
            //string negativeSignStr = numberFormatInfo.NegativeSign;
            //if (!string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
            //{
            //    negativeSignKey = (Keys)(VkKeyScan(negativeSignStr[0]));
            //}

            //if ((char.IsDigit((char)e.KeyCode) ||
            //     (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
            //     negativeSignKey == e.KeyCode ||
            //     Keys.Subtract == e.KeyCode) &&
            //    !e.Shift && !e.Alt && !e.Control)
            //{
            //    return true;
            //}
            return false;
        }

        /// <inheritdoc/>
        public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle,
                                                    bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
        {
            Rectangle editingControlBounds = PositionEditingPanel(cellBounds,
                                                        cellClip,
                                                        cellStyle,
                                                        singleVerticalBorderAdded,
                                                        singleHorizontalBorderAdded,
                                                        isFirstDisplayedColumn,
                                                        isFirstDisplayedRow);
            editingControlBounds = DataGridViewNumericUpDownCell.GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
            this.EditingNumericUpDown.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
            this.EditingNumericUpDown.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "DataGridViewNumericUpDownCell { ColumnIndex=" + ColumnIndex.ToString(CultureInfo.CurrentCulture) + ", RowIndex=" + RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
        }

        /// <inheritdoc/>
        //protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
        //                              object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
        //                              DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        //{
        //    if (this.DataGridView == null)
        //    {
        //        return;
        //    }

        //    First paint the borders and background of the cell.
        //    base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
        //               paintParts & ~(DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.ContentForeground));

        //    Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
        //    bool cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
        //    bool cellEdited = cellCurrent && this.DataGridView.EditingControl != null;

        //    If the cell is in editing mode, there is nothing else to paint
        //    if (!cellEdited)
        //    {
        //        if (PartPainted(paintParts, DataGridViewPaintParts.ContentForeground))
        //        {
        //            Paint a NumericUpDown control
        //             Take the borders into account
        //            Rectangle borderWidths = BorderWidths(advancedBorderStyle);
        //            Rectangle valBounds = cellBounds;
        //            valBounds.Offset(borderWidths.X, borderWidths.Y);
        //            valBounds.Width -= borderWidths.Right;
        //            valBounds.Height -= borderWidths.Bottom;
        //            Also take the padding into account
        //            if (cellStyle.Padding != Padding.Empty)
        //            {
        //                if (this.DataGridView.RightToLeft == RightToLeft.Yes)
        //                {
        //                    valBounds.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
        //                }
        //                else
        //                {
        //                    valBounds.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
        //                }
        //                valBounds.Width -= cellStyle.Padding.Horizontal;
        //                valBounds.Height -= cellStyle.Padding.Vertical;
        //            }
        //            Determine the NumericUpDown control location
        //           valBounds = DataGridViewNumericUpDownCell.GetAdjustedEditingControlBounds(valBounds, cellStyle);

        //            bool cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;

        //            if (renderingBitmap.Width < valBounds.Width ||
        //                renderingBitmap.Height < valBounds.Height)
        //            {
        //                The static bitmap is too small, a bigger one needs to be allocated.
        //               renderingBitmap.Dispose();
        //                renderingBitmap = new Bitmap(valBounds.Width, valBounds.Height);
        //            }
        //            Make sure the NumericUpDown control is parented to a visible control
        //            if (paintingNumericUpDown.Parent == null || !paintingNumericUpDown.Parent.Visible)
        //            {
        //                paintingNumericUpDown.Parent = this.DataGridView;
        //            }
        //            Set all the relevant properties
        //            paintingNumericUpDown.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(cellStyle.Alignment);
        //            paintingNumericUpDown.DecimalPlaces = this.DecimalPlaces;
        //            paintingNumericUpDown.ThousandsSeparator = this.ThousandsSeparator;
        //            paintingNumericUpDown.Font = cellStyle.Font;
        //            paintingNumericUpDown.Width = valBounds.Width;
        //            paintingNumericUpDown.Height = valBounds.Height;
        //            paintingNumericUpDown.RightToLeft = this.DataGridView.RightToLeft;
        //            paintingNumericUpDown.Location = new Point(0, -paintingNumericUpDown.Height - 100);
        //            paintingNumericUpDown.Text = formattedValue as string;

        //            Color backColor;
        //            if (PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected)
        //            {
        //                backColor = cellStyle.SelectionBackColor;
        //            }
        //            else
        //            {
        //                backColor = cellStyle.BackColor;
        //            }
        //            if (PartPainted(paintParts, DataGridViewPaintParts.Background))
        //            {
        //                if (backColor.A < 255)
        //                {
        //                    The NumericUpDown control does not support transparent back colors
        //                   backColor = Color.FromArgb(255, backColor);
        //                }
        //                paintingNumericUpDown.BackColor = backColor;
        //            }
        //            Finally paint the NumericUpDown control
        //           Rectangle srcRect = new Rectangle(0, 0, valBounds.Width, valBounds.Height);
        //            if (srcRect.Width > 0 && srcRect.Height > 0)
        //            {
        //                paintingNumericUpDown.DrawToBitmap(renderingBitmap, srcRect);
        //                graphics.DrawImage(renderingBitmap, new Rectangle(valBounds.Location, valBounds.Size),
        //                                   srcRect, GraphicsUnit.Pixel);
        //            }
        //        }
        //        if (PartPainted(paintParts, DataGridViewPaintParts.ErrorIcon))
        //        {
        //            Paint the potential error icon on top of the NumericUpDown control
        //            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
        //                       cellStyle, advancedBorderStyle, DataGridViewPaintParts.ErrorIcon);
        //        }
        //    }
        //}

        /// <inheritdoc/>
        protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            const int ButtonsWidth = 16;

            Rectangle errorIconBounds = base.GetErrorIconBounds(graphics, cellStyle, rowIndex);
            if (this.DataGridView != null && this.DataGridView.RightToLeft == RightToLeft.Yes)
            {
                errorIconBounds.X = errorIconBounds.Left + ButtonsWidth;
            }
            else
            {
                errorIconBounds.X = errorIconBounds.Left - ButtonsWidth;
            }
            return errorIconBounds;
        }

        /// <inheritdoc/>
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            // By default, the base implementation converts the Decimal 1234.5 into the string "1234.5"
            object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            string? formattedNumber = formattedValue.ToString();
            if (!string.IsNullOrWhiteSpace(formattedNumber) && value != null)
            {
                Decimal unformattedDecimal = System.Convert.ToDecimal(value);
                Decimal formattedDecimal = System.Convert.ToDecimal(formattedNumber);
                if (unformattedDecimal == formattedDecimal)
                {
                    // The base implementation of GetFormattedValue (which triggers the CellFormatting event) did nothing else than 
                    // the typical 1234.5 to "1234.5" conversion. But depending on the values of ThousandsSeparator and DecimalPlaces,
                    // this may not be the actual string displayed. The real formatted value may be "1,234.500"
                    return formattedDecimal.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());
                }
            }
            return formattedValue;
        }

        /// <inheritdoc/>
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (this.DataGridView == null)
            {
                return new Size(-1, -1);
            }

            Size preferredSize = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
            if (constraintSize.Width == 0)
            {
                const int ButtonsWidth = 16; // Account for the width of the up/down buttons.
                const int ButtonMargin = 8;  // Account for some blank pixels between the text and buttons.
                preferredSize.Width += ButtonsWidth + ButtonMargin;
            }
            return preferredSize;
        }

        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);
                }
                else
                {
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }

        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (this.DataGridView != null && rowIndex > -1)
            {
                if (this.DataGridView.EditingControl is DataGridViewNumericUpDownEditingControl editControl)
                {
                    return editControl.EditingControlRowIndex == rowIndex;
                }
            }
            return false;
        }

        private decimal Constrain(decimal value)
        {
            Debug.Assert(this.minimum <= this.maximum);
            if (value < this.minimum) { value = this.minimum; }
            if (value > this.maximum) { value = this.maximum; }
            return value;
        }

        internal void SetDecimalPlaces(int rowIndex, int value)
        {
            Debug.Assert(value >= 0 && value <= 99);
            this.decimalPlaces = value;
            if (this.OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.DecimalPlaces = value;
            }
        }

        internal void SetIncrement(int rowIndex, decimal value)
        {
            Debug.Assert(value >= 0.0m);
            this.increment = value;
            if (this.OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Increment = value;
            }
        }

        internal void SetMaximum(int rowIndex, decimal value)
        {
            this.maximum = value;
            if (this.minimum > this.maximum) { this.minimum = this.maximum; }

            object cellValue = this.GetValue(rowIndex);
            if (cellValue != null)
            {
                decimal currentValue = Convert.ToDecimal(cellValue);
                decimal constrainedValue = this.Constrain(currentValue);
                if (constrainedValue != currentValue)
                {
                    this.SetValue(rowIndex, constrainedValue);
                }
            }
            Debug.Assert(this.maximum == value);
            if (this.OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Maximum = value;
            }
        }

        internal void SetMinimum(int rowIndex, decimal value)
        {
            this.minimum = value;
            if (this.minimum > this.maximum) { this.maximum = value; }

            object cellValue = this.GetValue(rowIndex);
            if (cellValue != null)
            {
                decimal currentValue = Convert.ToDecimal(cellValue);
                decimal constrainedValue = this.Constrain(currentValue);
                if (constrainedValue != currentValue)
                {
                    this.SetValue(rowIndex, constrainedValue);
                }
            }
            Debug.Assert(this.minimum == value);
            if (this.OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Minimum = value;
            }
        }

        internal void SetThousandsSeparator(int rowIndex, bool value)
        {
            this.thousandsSeparator = value;
            if (this.OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.ThousandsSeparator = value;
            }
        }

        private static Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
        {
            // Add a 1 pixel padding on the left and right of the editing control
            editingControlBounds.X += 1;
            editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);

            // Adjust the vertical location of the editing control:
            int preferredHeight = cellStyle.Font.Height + 3;
            if (preferredHeight < editingControlBounds.Height)
            {
                switch (cellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.MiddleLeft:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.MiddleRight:
                        editingControlBounds.Y += (editingControlBounds.Height - preferredHeight) / 2;
                        break;
                    case DataGridViewContentAlignment.BottomLeft:
                    case DataGridViewContentAlignment.BottomCenter:
                    case DataGridViewContentAlignment.BottomRight:
                        editingControlBounds.Y += editingControlBounds.Height - preferredHeight;
                        break;
                }
            }

            return editingControlBounds;
        }

        internal static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
        {
            if ((align & AnyRight) != 0)
            {
                return HorizontalAlignment.Right;
            }
            else if ((align & AnyCenter) != 0)
            {
                return HorizontalAlignment.Center;
            }
            else
            {
                return HorizontalAlignment.Left;
            }
        }
    }
}