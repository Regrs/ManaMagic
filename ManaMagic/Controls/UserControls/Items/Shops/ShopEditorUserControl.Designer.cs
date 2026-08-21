
namespace ManaMagic.Controls.UserControls.Items.Shops
{
    partial class ShopEditorUserControl
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
            this.components = new System.ComponentModel.Container();
            this.shopTypeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.shopDataGridView = new System.Windows.Forms.DataGridView();
            this.itemColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addItemToShopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeItemFromShopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shopDataGridView)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // shopTypeComboBox
            // 
            this.shopTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shopTypeComboBox.FormattingEnabled = true;
            this.shopTypeComboBox.Location = new System.Drawing.Point(6, 3);
            this.shopTypeComboBox.Name = "shopTypeComboBox";
            this.shopTypeComboBox.Size = new System.Drawing.Size(193, 23);
            this.shopTypeComboBox.TabIndex = 87;
            this.shopTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ShopTypeComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.shopDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(3, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 413);
            this.groupBox1.TabIndex = 88;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shop Data";
            // 
            // shopDataGridView
            // 
            this.shopDataGridView.AllowUserToAddRows = false;
            this.shopDataGridView.AllowUserToDeleteRows = false;
            this.shopDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.shopDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemColumn});
            this.shopDataGridView.ContextMenuStrip = this.contextMenuStrip;
            this.shopDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shopDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.shopDataGridView.Location = new System.Drawing.Point(3, 19);
            this.shopDataGridView.Name = "shopDataGridView";
            this.shopDataGridView.RowTemplate.Height = 25;
            this.shopDataGridView.Size = new System.Drawing.Size(428, 391);
            this.shopDataGridView.TabIndex = 2;
            this.shopDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ShopDataGridView_CellValueChanged);
            this.shopDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.ShopDataGridView_CurrentCellDirtyStateChanged);
            // 
            // itemColumn
            // 
            this.itemColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemColumn.HeaderText = "Item";
            this.itemColumn.Name = "itemColumn";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemToShopToolStripMenuItem,
            this.removeItemFromShopToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(206, 48);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // addItemToShopToolStripMenuItem
            // 
            this.addItemToShopToolStripMenuItem.Name = "addItemToShopToolStripMenuItem";
            this.addItemToShopToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.addItemToShopToolStripMenuItem.Text = "Add Item To Shop";
            this.addItemToShopToolStripMenuItem.Click += new System.EventHandler(this.AddItemToShopToolStripMenuItem_Click);
            // 
            // removeItemFromShopToolStripMenuItem
            // 
            this.removeItemFromShopToolStripMenuItem.Name = "removeItemFromShopToolStripMenuItem";
            this.removeItemFromShopToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.removeItemFromShopToolStripMenuItem.Text = "Remove Item From Shop";
            this.removeItemFromShopToolStripMenuItem.Click += new System.EventHandler(this.RemoveItemFromShopToolStripMenuItem_Click);
            // 
            // ShopEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.shopTypeComboBox);
            this.Name = "ShopEditorUserControl";
            this.Size = new System.Drawing.Size(647, 506);
            this.Load += new System.EventHandler(this.ShopEditorUserControl_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shopDataGridView)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox shopTypeComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView shopDataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn itemColumn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addItemToShopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeItemFromShopToolStripMenuItem;
    }
}
