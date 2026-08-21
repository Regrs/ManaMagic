using System;
using System.Drawing;
using System.Windows.Forms;
using ZwellTech.Interop.User32;

#nullable enable

namespace ZwellTech.Windows.Forms
{
    /// <summary>
    /// Defines the editing control for the <see cref="DataGridViewNumericUpDownCell"/>.
    /// </summary>
    public sealed class DataGridViewNumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        /// <inheritdoc/>
        public DataGridView? EditingControlDataGridView { get; set; }

        /// <inheritdoc/>
        public object EditingControlFormattedValue
        {
            get { return this.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting); }
            set { this.Text = value.ToString(); }
        }

        /// <inheritdoc/>
        public int EditingControlRowIndex { get; set; }

        /// <inheritdoc/>
        public bool EditingControlValueChanged { get; set; }

        /// <inheritdoc/>
        public Cursor EditingPanelCursor { get { return Cursors.Default; } }

        /// <inheritdoc/>
        public bool RepositionEditingControlOnValueChange { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewNumericUpDownEditingControl"/> class.
        /// </summary>
        public DataGridViewNumericUpDownEditingControl()
        {
            // The editing control must not be part of the tabbing loop
            this.TabStop = false;
        }

        /// <inheritdoc/>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 255)
            {
                // The NumericUpDown control does not support transparent back colors
                Color opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
                this.BackColor = opaqueBackColor;
                if (this.EditingControlDataGridView != null)
                {
                    this.EditingControlDataGridView.EditingPanel.BackColor = opaqueBackColor;
                }
            }
            else
            {
                this.BackColor = dataGridViewCellStyle.BackColor;
            }
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(dataGridViewCellStyle.Alignment);
        }

        /// <inheritdoc/>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            TextBox? textBox = this.Controls[1] as TextBox;
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                    if (textBox != null)
                    {
                        // If the end of the selection is at the end of the string,
                        // let the DataGridView treat the key message
                        if ((this.RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.Text.Length)) ||
                            (this.RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0)))
                        {
                            return true;
                        }
                    }
                    break;
                case Keys.Left:
                    if (textBox != null)
                    {
                        // If the end of the selection is at the begining of the string
                        // or if the entire text is selected and we did not start editing,
                        // send this character to the dataGridView, else process the key message
                        if ((this.RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0)) ||
                            (this.RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.Text.Length)))
                        {
                            return true;
                        }
                    }
                    break;
                case Keys.Down:
                    // If the current value hasn't reached its minimum yet, handle the key. Otherwise let
                    // the grid handle it.
                    if (this.Value > this.Minimum)
                    {
                        return true;
                    }
                    break;

                case Keys.Up:
                    // If the current value hasn't reached its maximum yet, handle the key. Otherwise let
                    // the grid handle it.
                    if (this.Value < this.Maximum)
                    {
                        return true;
                    }
                    break;

                case Keys.Home:
                case Keys.End:
                    if (textBox != null)
                    {
                        if (textBox.SelectionLength != textBox.Text.Length)
                        {
                            return true;
                        }
                    }
                    break;
                case Keys.Delete:
                    if (textBox != null)
                    {
                        if (textBox.SelectionLength > 0 ||
                            textBox.SelectionStart < textBox.Text.Length)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return !dataGridViewWantsInputKey;
        }

        /// <inheritdoc/>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            bool userEdit = this.UserEdit;
            try
            {   
                // Prevent the Value from being set to Maximum or Minimum when the cell is being painted.
                this.UserEdit = (context & DataGridViewDataErrorContexts.Display) == 0;
                return this.Value.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());
            }
            finally
            {
                this.UserEdit = userEdit;
            }
        }

        /// <inheritdoc/>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (this.Controls[1] is TextBox textBox)
            {
                if (selectAll)
                {
                    textBox.SelectAll();
                    return;
                }

                // Do not select all the text, but position the caret at the end of the text
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // The value changes when a digit, the decimal separator, the group separator or
            // the negative sign is pressed.
            bool notifyValueChange = false;
            if (char.IsDigit(e.KeyChar))
            {
                notifyValueChange = true;
            }
            else
            {
                System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                string decimalSeparatorStr = numberFormatInfo.NumberDecimalSeparator;
                string groupSeparatorStr = numberFormatInfo.NumberGroupSeparator;
                string negativeSignStr = numberFormatInfo.NegativeSign;
                if (!string.IsNullOrEmpty(decimalSeparatorStr) && decimalSeparatorStr.Length == 1)
                {
                    notifyValueChange = decimalSeparatorStr[0] == e.KeyChar;
                }
                if (!notifyValueChange && !string.IsNullOrEmpty(groupSeparatorStr) && groupSeparatorStr.Length == 1)
                {
                    notifyValueChange = groupSeparatorStr[0] == e.KeyChar;
                }
                if (!notifyValueChange && !string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
                {
                    notifyValueChange = negativeSignStr[0] == e.KeyChar;
                }
            }

            if (notifyValueChange)
            {
                // Let the DataGridView know about the value change
                NotifyDataGridViewOfValueChange();
            }
        }

        /// <inheritdoc/>
        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            if (this.Focused)
            {
                // Let the DataGridView know about the value change
                NotifyDataGridViewOfValueChange();
            }
        }

        /// <inheritdoc/>
        protected override bool ProcessKeyEventArgs(ref Message m)
        {
            if (this.Controls[1] is TextBox textBox)
            {
                NativeMethods.SendMessage(textBox.Handle, (uint)m.Msg, m.WParam, m.LParam);
                return true;
            }
            return base.ProcessKeyEventArgs(ref m);
        }

        private void NotifyDataGridViewOfValueChange()
        {
            if (!this.EditingControlValueChanged)
            {
                this.EditingControlValueChanged = true;
                this.EditingControlDataGridView?.NotifyCurrentCellDirty(true);
            }
        }
    }
}