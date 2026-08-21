
namespace ManaMagic.Controls.UserControls.Character
{
    partial class ExperiencePerLevelEditorUserControl
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
            this.experienceByLevelDataGridView = new System.Windows.Forms.DataGridView();
            this.levelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            ((System.ComponentModel.ISupportInitialize)(this.experienceByLevelDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // experienceByLevelDataGridView
            // 
            this.experienceByLevelDataGridView.AllowUserToAddRows = false;
            this.experienceByLevelDataGridView.AllowUserToDeleteRows = false;
            this.experienceByLevelDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.experienceByLevelDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.levelColumn,
            this.expColumn});
            this.experienceByLevelDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.experienceByLevelDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.experienceByLevelDataGridView.Location = new System.Drawing.Point(0, 0);
            this.experienceByLevelDataGridView.Name = "experienceByLevelDataGridView";
            this.experienceByLevelDataGridView.RowTemplate.Height = 25;
            this.experienceByLevelDataGridView.Size = new System.Drawing.Size(770, 568);
            this.experienceByLevelDataGridView.TabIndex = 1;
            this.experienceByLevelDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ExperienceByLevelDataGridView_CellValueChanged);
            // 
            // levelColumn
            // 
            this.levelColumn.HeaderText = "Level";
            this.levelColumn.Name = "levelColumn";
            this.levelColumn.ReadOnly = true;
            this.levelColumn.Width = 50;
            // 
            // expColumn
            // 
            this.expColumn.HeaderText = "Experience Needed";
            this.expColumn.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.expColumn.Name = "expColumn";
            this.expColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.expColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.expColumn.Width = 150;
            // 
            // ExperiencePerLevelEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.experienceByLevelDataGridView);
            this.Name = "ExperiencePerLevelEditorUserControl";
            this.Size = new System.Drawing.Size(770, 568);
            this.Load += new System.EventHandler(this.ExperiencePerLevelEditorUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.experienceByLevelDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView experienceByLevelDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn levelColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn expColumn;
    }
}
