using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

#nullable enable

namespace ZwellTech.Windows.Forms
{
    /// <summary>
    /// Represents a numeric updown column in a <see cref="DataGridView"/> control.
    /// </summary>
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {
        /// <inheritdoc/>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value is DataGridViewNumericUpDownCell dataGridViewNumericUpDownCell)
                {
                    base.CellTemplate = value;
                    return;
                }
                ThrowHelper.ThrowInvalidOperationException("Value provided for CellTemplate must be of type DataGridViewNumericUpDownElements.DataGridViewNumericUpDownCell or derive from it.");
            }
        }

        /// <summary>
        /// Gets or sets the number of decimal places to display.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(DataGridViewNumericUpDownCell.DefaultDecimalPlaces)]
        [Description("Indicates the number of decimal places to display.")]
        public int DecimalPlaces
        {
            get
            {
                this.ValidateCellTemplate();
                return this.NumericUpDownCellTemplate.DecimalPlaces;
            }
            set
            {
                this.ValidateCellTemplate();

                // Update the template cell so that subsequent cloned cells use the new value.
                this.NumericUpDownCellTemplate.DecimalPlaces = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing DataGridViewNumericUpDownCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily. 
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            // Call the internal SetDecimalPlaces method instead of the property to avoid invalidation 
                            // of each cell. The whole column is invalidated later in a single operation for better performance.
                            dataGridViewCell.SetDecimalPlaces(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Gets or sets the value to increment or decrement the spin box (also known as an up-down control) when the up or down buttons are clicked.
        /// </summary>
        [Category("Data")]
        [Description("Indicates the amount to increment or decrement on each button click.")]
        public decimal Increment
        {
            get
            {
                this.ValidateCellTemplate();
                return this.NumericUpDownCellTemplate.Increment;
            }
            set
            {
                this.ValidateCellTemplate();

                this.NumericUpDownCellTemplate.Increment = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetIncrement(rowIndex, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum value for the spin box (also known as an up-down control).
        /// </summary>
        [Category("Data")]
        [Description("Indicates the maximum value for the numeric up-down cells.")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal Maximum
        {
            get
            {
                this.ValidateCellTemplate();
                return this.NumericUpDownCellTemplate.Maximum;
            }
            set
            {
                this.ValidateCellTemplate();

                this.NumericUpDownCellTemplate.Maximum = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetMaximum(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows, 
                    //       column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the spin box (also known as an up-down control).
        /// </summary>
        [Category("Data")]
        [Description("Indicates the minimum value for the numeric up-down cells.")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal Minimum
        {
            get
            {
                this.ValidateCellTemplate();
                return this.NumericUpDownCellTemplate.Minimum;
            }
            set
            {
                this.ValidateCellTemplate();

                this.NumericUpDownCellTemplate.Minimum = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetMinimum(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows, 
                    //       column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a thousands separator is displayed in the spin box (also known as an up-down control) when appropriate.
        /// </summary>
        [Category("Data")]
        [DefaultValue(DataGridViewNumericUpDownCell.DefaultThousandsSeparator)]
        [Description("Indicates whether the thousands separator will be inserted between every three decimal digits.")]
        public bool ThousandsSeparator
        {
            get
            {
                this.ValidateCellTemplate();
                return this.NumericUpDownCellTemplate.ThousandsSeparator;
            }
            set
            {
                this.ValidateCellTemplate();

                this.NumericUpDownCellTemplate.ThousandsSeparator = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetThousandsSeparator(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows, 
                    //       column headers / row headers as needed.
                }
            }
        }

        private DataGridViewNumericUpDownCell NumericUpDownCellTemplate { get { return (DataGridViewNumericUpDownCell)this.CellTemplate; } }

        /// <summary>
        /// Constructor for the DataGridViewNumericUpDownColumn class.
        /// </summary>
        public DataGridViewNumericUpDownColumn() : base(new DataGridViewNumericUpDownCell())
        {
        }

        /// <summary>
        /// Returns a standard compact string representation of the column.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("DataGridViewNumericUpDownColumn { Name=");
            sb.Append(this.Name);
            sb.Append(", Index=");
            sb.Append(this.Index.ToString(CultureInfo.CurrentCulture));
            sb.Append(" }");
            return sb.ToString();
        }

        private bool ShouldSerializeIncrement()
        {
            return !this.Increment.Equals(DataGridViewNumericUpDownCell.DefaultIncrement);
        }

        private bool ShouldSerializeMaximum()
        {
            return !this.Maximum.Equals(DataGridViewNumericUpDownCell.DefaultMaximum);
        }

        private bool ShouldSerializeMinimum()
        {
            return !this.Minimum.Equals(DataGridViewNumericUpDownCell.DefaultMinimum);
        }

        private void ValidateCellTemplate()
        {
            if (this.NumericUpDownCellTemplate == null)
            {
                ThrowHelper.ThrowInvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
        }
    }
}