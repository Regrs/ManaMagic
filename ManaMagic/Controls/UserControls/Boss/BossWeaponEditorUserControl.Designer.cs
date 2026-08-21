
namespace ManaMagic.Controls.UserControls.Boss
{
    partial class BossWeaponEditorUserControl
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
            this.label11 = new System.Windows.Forms.Label();
            this.affinityComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.statusEffectsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.inflictionRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.powerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.accuracyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.elementComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.indexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inflictionRateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accuracyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 15);
            this.label11.TabIndex = 60;
            this.label11.Text = "Affinity:";
            // 
            // affinityComboBox
            // 
            this.affinityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.affinityComboBox.FormattingEnabled = true;
            this.affinityComboBox.Location = new System.Drawing.Point(73, 23);
            this.affinityComboBox.Name = "affinityComboBox";
            this.affinityComboBox.Size = new System.Drawing.Size(155, 23);
            this.affinityComboBox.TabIndex = 59;
            this.affinityComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.statusEffectsCheckedListBox);
            this.groupBox4.Controls.Add(this.inflictionRateNumericUpDown);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(255, 50);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 369);
            this.groupBox4.TabIndex = 75;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Status Effects";
            // 
            // statusEffectsCheckedListBox
            // 
            this.statusEffectsCheckedListBox.FormattingEnabled = true;
            this.statusEffectsCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.statusEffectsCheckedListBox.Name = "statusEffectsCheckedListBox";
            this.statusEffectsCheckedListBox.Size = new System.Drawing.Size(186, 292);
            this.statusEffectsCheckedListBox.TabIndex = 67;
            this.statusEffectsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.StatusEffectsCheckedListBox_ItemCheck);
            // 
            // inflictionRateNumericUpDown
            // 
            this.inflictionRateNumericUpDown.Location = new System.Drawing.Point(6, 335);
            this.inflictionRateNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inflictionRateNumericUpDown.Name = "inflictionRateNumericUpDown";
            this.inflictionRateNumericUpDown.Size = new System.Drawing.Size(186, 23);
            this.inflictionRateNumericUpDown.TabIndex = 65;
            this.inflictionRateNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(6, 317);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 15);
            this.label15.TabIndex = 66;
            this.label15.Text = "Infliction Rate";
            // 
            // powerNumericUpDown
            // 
            this.powerNumericUpDown.Location = new System.Drawing.Point(73, 110);
            this.powerNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.powerNumericUpDown.Name = "powerNumericUpDown";
            this.powerNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.powerNumericUpDown.TabIndex = 78;
            this.powerNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(24, 112);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 15);
            this.label14.TabIndex = 79;
            this.label14.Text = "Power:";
            // 
            // accuracyNumericUpDown
            // 
            this.accuracyNumericUpDown.Location = new System.Drawing.Point(73, 81);
            this.accuracyNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.accuracyNumericUpDown.Name = "accuracyNumericUpDown";
            this.accuracyNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.accuracyNumericUpDown.TabIndex = 76;
            this.accuracyNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 83);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 15);
            this.label13.TabIndex = 77;
            this.label13.Text = "Accuracy:";
            // 
            // elementComboBox
            // 
            this.elementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.elementComboBox.FormattingEnabled = true;
            this.elementComboBox.Location = new System.Drawing.Point(73, 52);
            this.elementComboBox.Name = "elementComboBox";
            this.elementComboBox.Size = new System.Drawing.Size(155, 23);
            this.elementComboBox.TabIndex = 88;
            this.elementComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 89;
            this.label5.Text = "Element:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(11, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 15);
            this.label6.TabIndex = 96;
            this.label6.Text = "Index:";
            // 
            // indexNumericUpDown
            // 
            this.indexNumericUpDown.Hexadecimal = true;
            this.indexNumericUpDown.Location = new System.Drawing.Point(51, 0);
            this.indexNumericUpDown.Maximum = new decimal(new int[] {
            86,
            0,
            0,
            0});
            this.indexNumericUpDown.Name = "indexNumericUpDown";
            this.indexNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.indexNumericUpDown.TabIndex = 95;
            this.indexNumericUpDown.ValueChanged += new System.EventHandler(this.IndexNumericUpDown_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.affinityComboBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.elementComboBox);
            this.groupBox1.Controls.Add(this.accuracyNumericUpDown);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.powerNumericUpDown);
            this.groupBox1.Location = new System.Drawing.Point(3, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 146);
            this.groupBox1.TabIndex = 97;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weapon";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 98;
            this.label1.Text = "Name:";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(51, 26);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(49, 15);
            this.nameLabel.TabIndex = 99;
            this.nameLabel.Text = "[NAME]";
            // 
            // BossWeaponEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.indexNumericUpDown);
            this.Controls.Add(this.groupBox4);
            this.Name = "BossWeaponEditorUserControl";
            this.Size = new System.Drawing.Size(466, 433);
            this.Load += new System.EventHandler(this.BossWeaponEditorUserControl_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inflictionRateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accuracyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox affinityComboBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckedListBox statusEffectsCheckedListBox;
        private System.Windows.Forms.NumericUpDown inflictionRateNumericUpDown;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown powerNumericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown accuracyNumericUpDown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox elementComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown indexNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label nameLabel;
    }
}
