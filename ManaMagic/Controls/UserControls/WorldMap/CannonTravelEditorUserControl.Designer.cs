
namespace ManaMagic.Controls.UserControls.WorldMap
{
    partial class CannonTravelEditorUserControl
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
            this.startXColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.startYColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.endXColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.endYColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
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
            this.startXColumn,
            this.startYColumn,
            this.endXColumn,
            this.endYColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(770, 516);
            this.dataGridView.TabIndex = 97;
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            // 
            // indexColumn
            // 
            this.indexColumn.HeaderText = "Index";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            this.indexColumn.Width = 50;
            // 
            // startXColumn
            // 
            this.startXColumn.HeaderText = "Start X Coordinate";
            this.startXColumn.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.startXColumn.Name = "startXColumn";
            this.startXColumn.Width = 125;
            // 
            // startYColumn
            // 
            this.startYColumn.HeaderText = "Start Y Coordinate";
            this.startYColumn.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.startYColumn.Name = "startYColumn";
            this.startYColumn.Width = 125;
            // 
            // endXColumn
            // 
            this.endXColumn.HeaderText = "End X Coordinate";
            this.endXColumn.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.endXColumn.Name = "endXColumn";
            this.endXColumn.Width = 125;
            // 
            // endYColumn
            // 
            this.endYColumn.HeaderText = "End Y Coordinate";
            this.endYColumn.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.endYColumn.Name = "endYColumn";
            this.endYColumn.Width = 125;
            // 
            // CannonTravelEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView);
            this.Name = "CannonTravelEditorUserControl";
            this.Size = new System.Drawing.Size(770, 516);
            this.Load += new System.EventHandler(this.CannonTravelEditorUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn startXColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn startYColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn endXColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn endYColumn;
    }
}
