
namespace ManaMagic.Controls.UserControls.Maps
{
    partial class DoorEditorUserControl
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.indexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mapIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xCoordinateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yCoordinateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entranceTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exitTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rawValuesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.indexColumn,
            this.nameColumn,
            this.mapIdColumn,
            this.xCoordinateColumn,
            this.yCoordinateColumn,
            this.entranceTypeColumn,
            this.exitTypeColumn,
            this.rawValuesColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(1321, 581);
            this.dataGridView1.TabIndex = 0;
            // 
            // indexColumn
            // 
            this.indexColumn.HeaderText = "Index";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.Width = 250;
            // 
            // mapIdColumn
            // 
            this.mapIdColumn.HeaderText = "Map ID";
            this.mapIdColumn.Name = "mapIdColumn";
            this.mapIdColumn.ReadOnly = true;
            // 
            // xCoordinateColumn
            // 
            this.xCoordinateColumn.HeaderText = "X-Coordinate";
            this.xCoordinateColumn.Name = "xCoordinateColumn";
            this.xCoordinateColumn.ReadOnly = true;
            // 
            // yCoordinateColumn
            // 
            this.yCoordinateColumn.HeaderText = "Y-Coordinate";
            this.yCoordinateColumn.Name = "yCoordinateColumn";
            this.yCoordinateColumn.ReadOnly = true;
            // 
            // entranceTypeColumn
            // 
            this.entranceTypeColumn.HeaderText = "Entrance Type";
            this.entranceTypeColumn.Name = "entranceTypeColumn";
            this.entranceTypeColumn.ReadOnly = true;
            this.entranceTypeColumn.Width = 150;
            // 
            // exitTypeColumn
            // 
            this.exitTypeColumn.HeaderText = "Exit Type";
            this.exitTypeColumn.Name = "exitTypeColumn";
            this.exitTypeColumn.ReadOnly = true;
            // 
            // rawValuesColumn
            // 
            this.rawValuesColumn.HeaderText = "Raw Values";
            this.rawValuesColumn.Name = "rawValuesColumn";
            this.rawValuesColumn.ReadOnly = true;
            // 
            // DoorEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "DoorEditorUserControl";
            this.Size = new System.Drawing.Size(1321, 581);
            this.Load += new System.EventHandler(this.DoorEditorUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mapIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn xCoordinateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yCoordinateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entranceTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exitTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rawValuesColumn;
    }
}
