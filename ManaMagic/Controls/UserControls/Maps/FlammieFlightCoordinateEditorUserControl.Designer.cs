
namespace ManaMagic.Controls.UserControls.Maps
{
    partial class FlammieFlightCoordinateEditorUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.indexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.yColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.indexColumn,
            this.xColumn,
            this.yColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(812, 692);
            this.dataGridView.TabIndex = 99;
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            // 
            // indexColumn
            // 
            this.indexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.indexColumn.HeaderText = "Map";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            // 
            // xColumn
            // 
            this.xColumn.HeaderText = "X Coordinate";
            this.xColumn.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.xColumn.Name = "xColumn";
            this.xColumn.Width = 125;
            // 
            // yColumn
            // 
            this.yColumn.HeaderText = "Y Coordinate";
            this.yColumn.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.yColumn.Name = "yColumn";
            this.yColumn.Width = 125;
            // 
            // FlammieFlightCoordinateEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView);
            this.Name = "FlammieFlightCoordinateEditorUserControl";
            this.Size = new System.Drawing.Size(812, 692);
            this.Load += new System.EventHandler(this.FlammieFlightCoordinateEditorUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn xColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn yColumn;
    }
}
