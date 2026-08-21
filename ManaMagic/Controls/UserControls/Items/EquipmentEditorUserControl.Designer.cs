
namespace ManaMagic.Controls.UserControls.Items
{
    partial class EquipmentEditorUserControl
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
            this.equipmentTabControl = new System.Windows.Forms.TabControl();
            this.helmetsTabPage = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.helmetsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.armorTabPage = new System.Windows.Forms.TabPage();
            this.accessoriesTabPage = new System.Windows.Forms.TabPage();
            this.helmetDefinitionEditorUserControl = new ManaMagic.Controls.UserControls.Items.EquipmentDefinitionEditorUserControl();
            this.armorDefinitionEditorUserControl = new ManaMagic.Controls.UserControls.Items.EquipmentDefinitionEditorUserControl();
            this.label1 = new System.Windows.Forms.Label();
            this.armorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.accessoryDefinitionEditorUserControl = new ManaMagic.Controls.UserControls.Items.EquipmentDefinitionEditorUserControl();
            this.label3 = new System.Windows.Forms.Label();
            this.accessoryNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.equipmentTabControl.SuspendLayout();
            this.helmetsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helmetsNumericUpDown)).BeginInit();
            this.armorTabPage.SuspendLayout();
            this.accessoriesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.armorNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accessoryNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // equipmentTabControl
            // 
            this.equipmentTabControl.Controls.Add(this.helmetsTabPage);
            this.equipmentTabControl.Controls.Add(this.armorTabPage);
            this.equipmentTabControl.Controls.Add(this.accessoriesTabPage);
            this.equipmentTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equipmentTabControl.Location = new System.Drawing.Point(0, 0);
            this.equipmentTabControl.Name = "equipmentTabControl";
            this.equipmentTabControl.SelectedIndex = 0;
            this.equipmentTabControl.Size = new System.Drawing.Size(972, 692);
            this.equipmentTabControl.TabIndex = 0;
            // 
            // helmetsTabPage
            // 
            this.helmetsTabPage.Controls.Add(this.helmetDefinitionEditorUserControl);
            this.helmetsTabPage.Controls.Add(this.label2);
            this.helmetsTabPage.Controls.Add(this.helmetsNumericUpDown);
            this.helmetsTabPage.Location = new System.Drawing.Point(4, 24);
            this.helmetsTabPage.Name = "helmetsTabPage";
            this.helmetsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.helmetsTabPage.Size = new System.Drawing.Size(964, 664);
            this.helmetsTabPage.TabIndex = 0;
            this.helmetsTabPage.Text = "Helmets";
            this.helmetsTabPage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Index";
            // 
            // helmetsNumericUpDown
            // 
            this.helmetsNumericUpDown.Hexadecimal = true;
            this.helmetsNumericUpDown.Location = new System.Drawing.Point(6, 24);
            this.helmetsNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.helmetsNumericUpDown.Name = "helmetsNumericUpDown";
            this.helmetsNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.helmetsNumericUpDown.TabIndex = 10;
            // 
            // armorTabPage
            // 
            this.armorTabPage.Controls.Add(this.armorDefinitionEditorUserControl);
            this.armorTabPage.Controls.Add(this.label1);
            this.armorTabPage.Controls.Add(this.armorNumericUpDown);
            this.armorTabPage.Location = new System.Drawing.Point(4, 24);
            this.armorTabPage.Name = "armorTabPage";
            this.armorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.armorTabPage.Size = new System.Drawing.Size(964, 664);
            this.armorTabPage.TabIndex = 1;
            this.armorTabPage.Text = "Armor";
            this.armorTabPage.UseVisualStyleBackColor = true;
            // 
            // accessoriesTabPage
            // 
            this.accessoriesTabPage.Controls.Add(this.accessoryDefinitionEditorUserControl);
            this.accessoriesTabPage.Controls.Add(this.label3);
            this.accessoriesTabPage.Controls.Add(this.accessoryNumericUpDown);
            this.accessoriesTabPage.Location = new System.Drawing.Point(4, 24);
            this.accessoriesTabPage.Name = "accessoriesTabPage";
            this.accessoriesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.accessoriesTabPage.Size = new System.Drawing.Size(964, 664);
            this.accessoriesTabPage.TabIndex = 2;
            this.accessoriesTabPage.Text = "Accessories";
            this.accessoriesTabPage.UseVisualStyleBackColor = true;
            // 
            // helmetDefinitionEditorUserControl
            // 
            this.helmetDefinitionEditorUserControl.Location = new System.Drawing.Point(6, 53);
            this.helmetDefinitionEditorUserControl.Name = "helmetDefinitionEditorUserControl";
            this.helmetDefinitionEditorUserControl.Size = new System.Drawing.Size(759, 335);
            this.helmetDefinitionEditorUserControl.TabIndex = 12;
            // 
            // armorDefinitionEditorUserControl
            // 
            this.armorDefinitionEditorUserControl.Location = new System.Drawing.Point(6, 53);
            this.armorDefinitionEditorUserControl.Name = "armorDefinitionEditorUserControl";
            this.armorDefinitionEditorUserControl.Size = new System.Drawing.Size(759, 335);
            this.armorDefinitionEditorUserControl.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Index";
            // 
            // armorNumericUpDown
            // 
            this.armorNumericUpDown.Hexadecimal = true;
            this.armorNumericUpDown.Location = new System.Drawing.Point(6, 24);
            this.armorNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.armorNumericUpDown.Name = "armorNumericUpDown";
            this.armorNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.armorNumericUpDown.TabIndex = 13;
            // 
            // accessoryDefinitionEditorUserControl
            // 
            this.accessoryDefinitionEditorUserControl.Location = new System.Drawing.Point(6, 53);
            this.accessoryDefinitionEditorUserControl.Name = "accessoryDefinitionEditorUserControl";
            this.accessoryDefinitionEditorUserControl.Size = new System.Drawing.Size(759, 335);
            this.accessoryDefinitionEditorUserControl.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "Index";
            // 
            // accessoryNumericUpDown
            // 
            this.accessoryNumericUpDown.Hexadecimal = true;
            this.accessoryNumericUpDown.Location = new System.Drawing.Point(6, 24);
            this.accessoryNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.accessoryNumericUpDown.Name = "accessoryNumericUpDown";
            this.accessoryNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.accessoryNumericUpDown.TabIndex = 16;
            // 
            // EquipmentEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.equipmentTabControl);
            this.Name = "EquipmentEditorUserControl";
            this.Size = new System.Drawing.Size(972, 692);
            this.Load += new System.EventHandler(this.EquipmentEditorUserControl_Load);
            this.equipmentTabControl.ResumeLayout(false);
            this.helmetsTabPage.ResumeLayout(false);
            this.helmetsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helmetsNumericUpDown)).EndInit();
            this.armorTabPage.ResumeLayout(false);
            this.armorTabPage.PerformLayout();
            this.accessoriesTabPage.ResumeLayout(false);
            this.accessoriesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.armorNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accessoryNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl equipmentTabControl;
        private System.Windows.Forms.TabPage helmetsTabPage;
        private System.Windows.Forms.TabPage armorTabPage;
        private System.Windows.Forms.TabPage accessoriesTabPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown helmetsNumericUpDown;
        private EquipmentDefinitionEditorUserControl helmetDefinitionEditorUserControl;
        private EquipmentDefinitionEditorUserControl armorDefinitionEditorUserControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown armorNumericUpDown;
        private EquipmentDefinitionEditorUserControl accessoryDefinitionEditorUserControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown accessoryNumericUpDown;
    }
}
