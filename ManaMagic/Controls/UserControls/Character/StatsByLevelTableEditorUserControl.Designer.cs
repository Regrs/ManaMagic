
namespace ManaMagic.Controls.UserControls.Character
{
    partial class StatsByLevelTableEditorUserControl
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
            this.statsByLevelDataGridView = new System.Windows.Forms.DataGridView();
            this.levelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hitPointsColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.manaPointsColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.strengthColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.agilityColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.constitutionColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.intelligenceColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.wisdomColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            ((System.ComponentModel.ISupportInitialize)(this.statsByLevelDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // statsByLevelDataGridView
            // 
            this.statsByLevelDataGridView.AllowUserToAddRows = false;
            this.statsByLevelDataGridView.AllowUserToDeleteRows = false;
            this.statsByLevelDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statsByLevelDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.levelColumn,
            this.hitPointsColumn,
            this.manaPointsColumn,
            this.strengthColumn,
            this.agilityColumn,
            this.constitutionColumn,
            this.intelligenceColumn,
            this.wisdomColumn});
            this.statsByLevelDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsByLevelDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.statsByLevelDataGridView.Location = new System.Drawing.Point(0, 0);
            this.statsByLevelDataGridView.Name = "statsByLevelDataGridView";
            this.statsByLevelDataGridView.RowTemplate.Height = 25;
            this.statsByLevelDataGridView.Size = new System.Drawing.Size(933, 688);
            this.statsByLevelDataGridView.TabIndex = 0;
            this.statsByLevelDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.StatsByLevelDataGridView_CellValueChanged);
            // 
            // levelColumn
            // 
            this.levelColumn.HeaderText = "Level";
            this.levelColumn.Name = "levelColumn";
            this.levelColumn.ReadOnly = true;
            this.levelColumn.Width = 50;
            // 
            // hitPointsColumn
            // 
            this.hitPointsColumn.HeaderText = "Hit Points";
            this.hitPointsColumn.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.hitPointsColumn.Name = "hitPointsColumn";
            this.hitPointsColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.hitPointsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // manaPointsColumn
            // 
            this.manaPointsColumn.HeaderText = "Mana Points";
            this.manaPointsColumn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.manaPointsColumn.Name = "manaPointsColumn";
            this.manaPointsColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.manaPointsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // strengthColumn
            // 
            this.strengthColumn.HeaderText = "Strength";
            this.strengthColumn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.strengthColumn.Name = "strengthColumn";
            this.strengthColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.strengthColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // agilityColumn
            // 
            this.agilityColumn.HeaderText = "Agility";
            this.agilityColumn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.agilityColumn.Name = "agilityColumn";
            this.agilityColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.agilityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // constitutionColumn
            // 
            this.constitutionColumn.HeaderText = "Constitution";
            this.constitutionColumn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.constitutionColumn.Name = "constitutionColumn";
            this.constitutionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.constitutionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // intelligenceColumn
            // 
            this.intelligenceColumn.HeaderText = "Intelligence";
            this.intelligenceColumn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.intelligenceColumn.Name = "intelligenceColumn";
            this.intelligenceColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.intelligenceColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // wisdomColumn
            // 
            this.wisdomColumn.HeaderText = "Wisdom";
            this.wisdomColumn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.wisdomColumn.Name = "wisdomColumn";
            this.wisdomColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.wisdomColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // StatsByLevelTableEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statsByLevelDataGridView);
            this.Name = "StatsByLevelTableEditorUserControl";
            this.Size = new System.Drawing.Size(933, 688);
            this.Load += new System.EventHandler(this.StatsByLevelTableEditorUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statsByLevelDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView statsByLevelDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn levelColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn hitPointsColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn manaPointsColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn strengthColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn agilityColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn constitutionColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn intelligenceColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn wisdomColumn;
    }
}
