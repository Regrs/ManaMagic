
namespace ManaMagic.Controls.UserControls.Items.Shops
{
    partial class ShopPriceEditorUserControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.consumableDataGridView = new System.Windows.Forms.DataGridView();
            this.consumableColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consumablePriceColumn = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.consumableCanSellColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.weaponUpgradesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn1 = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.helmetsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn2 = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.armorDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn3 = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.accessoriesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn4 = new ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.consumableDataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weaponUpgradesDataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helmetsDataGridView)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.armorDataGridView)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accessoriesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.consumableDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(14, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 353);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consumable Items";
            // 
            // consumableDataGridView
            // 
            this.consumableDataGridView.AllowUserToAddRows = false;
            this.consumableDataGridView.AllowUserToDeleteRows = false;
            this.consumableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.consumableDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.consumableColumn,
            this.consumablePriceColumn,
            this.consumableCanSellColumn});
            this.consumableDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consumableDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.consumableDataGridView.Location = new System.Drawing.Point(3, 19);
            this.consumableDataGridView.Name = "consumableDataGridView";
            this.consumableDataGridView.RowTemplate.Height = 25;
            this.consumableDataGridView.Size = new System.Drawing.Size(428, 331);
            this.consumableDataGridView.TabIndex = 2;
            this.consumableDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            this.consumableDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            // 
            // consumableColumn
            // 
            this.consumableColumn.HeaderText = "Item";
            this.consumableColumn.Name = "consumableColumn";
            this.consumableColumn.ReadOnly = true;
            this.consumableColumn.Width = 200;
            // 
            // consumablePriceColumn
            // 
            this.consumablePriceColumn.HeaderText = "Price";
            this.consumablePriceColumn.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.consumablePriceColumn.Name = "consumablePriceColumn";
            // 
            // consumableCanSellColumn
            // 
            this.consumableCanSellColumn.HeaderText = "Can Sell";
            this.consumableCanSellColumn.Name = "consumableCanSellColumn";
            this.consumableCanSellColumn.Width = 75;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.weaponUpgradesDataGridView);
            this.groupBox2.Location = new System.Drawing.Point(454, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(434, 350);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Weapon Upgrades";
            // 
            // weaponUpgradesDataGridView
            // 
            this.weaponUpgradesDataGridView.AllowUserToAddRows = false;
            this.weaponUpgradesDataGridView.AllowUserToDeleteRows = false;
            this.weaponUpgradesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.weaponUpgradesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewNumericUpDownColumn1});
            this.weaponUpgradesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weaponUpgradesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.weaponUpgradesDataGridView.Location = new System.Drawing.Point(3, 19);
            this.weaponUpgradesDataGridView.Name = "weaponUpgradesDataGridView";
            this.weaponUpgradesDataGridView.RowTemplate.Height = 25;
            this.weaponUpgradesDataGridView.Size = new System.Drawing.Size(428, 328);
            this.weaponUpgradesDataGridView.TabIndex = 2;
            this.weaponUpgradesDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            this.weaponUpgradesDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewNumericUpDownColumn1
            // 
            this.dataGridViewNumericUpDownColumn1.HeaderText = "Price";
            this.dataGridViewNumericUpDownColumn1.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn1.Name = "dataGridViewNumericUpDownColumn1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.helmetsDataGridView);
            this.groupBox3.Location = new System.Drawing.Point(14, 375);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(434, 673);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Helmets";
            // 
            // helmetsDataGridView
            // 
            this.helmetsDataGridView.AllowUserToAddRows = false;
            this.helmetsDataGridView.AllowUserToDeleteRows = false;
            this.helmetsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.helmetsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewNumericUpDownColumn2,
            this.dataGridViewCheckBoxColumn1});
            this.helmetsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helmetsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.helmetsDataGridView.Location = new System.Drawing.Point(3, 19);
            this.helmetsDataGridView.Name = "helmetsDataGridView";
            this.helmetsDataGridView.RowTemplate.Height = 25;
            this.helmetsDataGridView.Size = new System.Drawing.Size(428, 651);
            this.helmetsDataGridView.TabIndex = 2;
            this.helmetsDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            this.helmetsDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Item";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewNumericUpDownColumn2
            // 
            this.dataGridViewNumericUpDownColumn2.HeaderText = "Price";
            this.dataGridViewNumericUpDownColumn2.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn2.Name = "dataGridViewNumericUpDownColumn2";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Can Sell";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 75;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.armorDataGridView);
            this.groupBox4.Location = new System.Drawing.Point(454, 375);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(434, 673);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Armor";
            // 
            // armorDataGridView
            // 
            this.armorDataGridView.AllowUserToAddRows = false;
            this.armorDataGridView.AllowUserToDeleteRows = false;
            this.armorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.armorDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewNumericUpDownColumn3,
            this.dataGridViewCheckBoxColumn2});
            this.armorDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.armorDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.armorDataGridView.Location = new System.Drawing.Point(3, 19);
            this.armorDataGridView.Name = "armorDataGridView";
            this.armorDataGridView.RowTemplate.Height = 25;
            this.armorDataGridView.Size = new System.Drawing.Size(428, 651);
            this.armorDataGridView.TabIndex = 2;
            this.armorDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            this.armorDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Item";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // dataGridViewNumericUpDownColumn3
            // 
            this.dataGridViewNumericUpDownColumn3.HeaderText = "Price";
            this.dataGridViewNumericUpDownColumn3.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn3.Name = "dataGridViewNumericUpDownColumn3";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.HeaderText = "Can Sell";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Width = 75;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.accessoriesDataGridView);
            this.groupBox5.Location = new System.Drawing.Point(894, 375);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(434, 673);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Accessories";
            // 
            // accessoriesDataGridView
            // 
            this.accessoriesDataGridView.AllowUserToAddRows = false;
            this.accessoriesDataGridView.AllowUserToDeleteRows = false;
            this.accessoriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accessoriesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewNumericUpDownColumn4,
            this.dataGridViewCheckBoxColumn3});
            this.accessoriesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accessoriesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.accessoriesDataGridView.Location = new System.Drawing.Point(3, 19);
            this.accessoriesDataGridView.Name = "accessoriesDataGridView";
            this.accessoriesDataGridView.RowTemplate.Height = 25;
            this.accessoriesDataGridView.Size = new System.Drawing.Size(428, 651);
            this.accessoriesDataGridView.TabIndex = 2;
            this.accessoriesDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            this.accessoriesDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Item";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 200;
            // 
            // dataGridViewNumericUpDownColumn4
            // 
            this.dataGridViewNumericUpDownColumn4.HeaderText = "Price";
            this.dataGridViewNumericUpDownColumn4.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn4.Name = "dataGridViewNumericUpDownColumn4";
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.HeaderText = "Can Sell";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            this.dataGridViewCheckBoxColumn3.Width = 75;
            // 
            // ShopPriceEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ShopPriceEditorUserControl";
            this.Size = new System.Drawing.Size(1361, 1086);
            this.Load += new System.EventHandler(this.ShopPriceEditorUserControl_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.consumableDataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.weaponUpgradesDataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.helmetsDataGridView)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.armorDataGridView)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accessoriesDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView consumableDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn consumableColumn;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn consumablePriceColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn consumableCanSellColumn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView weaponUpgradesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView helmetsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView armorDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView accessoriesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private ZwellTech.Windows.Forms.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
    }
}
