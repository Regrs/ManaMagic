
namespace ManaMagic.Controls.UserControls.Items
{
    partial class EquipmentDefinitionEditorUserControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.resistancesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.defenseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.evasionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.magicDefenseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.magicEvasionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.elementComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statAdjustmentTypeFlagCheckBox = new System.Windows.Forms.CheckBox();
            this.wisdomCheckBox = new System.Windows.Forms.CheckBox();
            this.intelligenceCheckBox = new System.Windows.Forms.CheckBox();
            this.constitutionCheckBox = new System.Windows.Forms.CheckBox();
            this.agilityCheckBox = new System.Windows.Forms.CheckBox();
            this.strengthCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.popoieCheckBox = new System.Windows.Forms.CheckBox();
            this.purimCheckBox = new System.Windows.Forms.CheckBox();
            this.randiCheckBox = new System.Windows.Forms.CheckBox();
            this.nameChangeTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defenseNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.evasionNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.magicDefenseNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.magicEvasionNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(6, 43);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(254, 23);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(175, 99);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 15);
            this.label16.TabIndex = 73;
            this.label16.Text = "Icon:";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconPictureBox.Location = new System.Drawing.Point(214, 84);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(48, 48);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconPictureBox.TabIndex = 72;
            this.iconPictureBox.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.resistancesCheckedListBox);
            this.groupBox4.Location = new System.Drawing.Point(550, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 322);
            this.groupBox4.TabIndex = 75;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Resistances";
            // 
            // resistancesCheckedListBox
            // 
            this.resistancesCheckedListBox.FormattingEnabled = true;
            this.resistancesCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.resistancesCheckedListBox.Name = "resistancesCheckedListBox";
            this.resistancesCheckedListBox.Size = new System.Drawing.Size(186, 292);
            this.resistancesCheckedListBox.TabIndex = 67;
            this.resistancesCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ResistancesCheckedListBox_ItemCheck);
            // 
            // defenseNumericUpDown
            // 
            this.defenseNumericUpDown.Location = new System.Drawing.Point(106, 22);
            this.defenseNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.defenseNumericUpDown.Name = "defenseNumericUpDown";
            this.defenseNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.defenseNumericUpDown.TabIndex = 76;
            this.defenseNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(48, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 15);
            this.label14.TabIndex = 77;
            this.label14.Text = "Defense:";
            // 
            // evasionNumericUpDown
            // 
            this.evasionNumericUpDown.Location = new System.Drawing.Point(106, 51);
            this.evasionNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.evasionNumericUpDown.Name = "evasionNumericUpDown";
            this.evasionNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.evasionNumericUpDown.TabIndex = 78;
            this.evasionNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 79;
            this.label2.Text = "Evasion:";
            // 
            // magicDefenseNumericUpDown
            // 
            this.magicDefenseNumericUpDown.Location = new System.Drawing.Point(106, 80);
            this.magicDefenseNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.magicDefenseNumericUpDown.Name = "magicDefenseNumericUpDown";
            this.magicDefenseNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.magicDefenseNumericUpDown.TabIndex = 80;
            this.magicDefenseNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 15);
            this.label3.TabIndex = 81;
            this.label3.Text = "Magic Defense:";
            // 
            // magicEvasionNumericUpDown
            // 
            this.magicEvasionNumericUpDown.Location = new System.Drawing.Point(106, 109);
            this.magicEvasionNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.magicEvasionNumericUpDown.Name = "magicEvasionNumericUpDown";
            this.magicEvasionNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.magicEvasionNumericUpDown.TabIndex = 82;
            this.magicEvasionNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 83;
            this.label4.Text = "Magic Evasion:";
            // 
            // elementComboBox
            // 
            this.elementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.elementComboBox.FormattingEnabled = true;
            this.elementComboBox.Location = new System.Drawing.Point(105, 155);
            this.elementComboBox.Name = "elementComboBox";
            this.elementComboBox.Size = new System.Drawing.Size(155, 23);
            this.elementComboBox.TabIndex = 86;
            this.elementComboBox.SelectedValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 87;
            this.label5.Text = "Element:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.statAdjustmentTypeFlagCheckBox);
            this.groupBox1.Controls.Add(this.wisdomCheckBox);
            this.groupBox1.Controls.Add(this.intelligenceCheckBox);
            this.groupBox1.Controls.Add(this.constitutionCheckBox);
            this.groupBox1.Controls.Add(this.agilityCheckBox);
            this.groupBox1.Controls.Add(this.strengthCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(277, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 172);
            this.groupBox1.TabIndex = 88;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stat Modifiers";
            // 
            // statAdjustmentTypeFlagCheckBox
            // 
            this.statAdjustmentTypeFlagCheckBox.AutoSize = true;
            this.statAdjustmentTypeFlagCheckBox.Enabled = false;
            this.statAdjustmentTypeFlagCheckBox.Location = new System.Drawing.Point(15, 142);
            this.statAdjustmentTypeFlagCheckBox.Name = "statAdjustmentTypeFlagCheckBox";
            this.statAdjustmentTypeFlagCheckBox.Size = new System.Drawing.Size(163, 19);
            this.statAdjustmentTypeFlagCheckBox.TabIndex = 5;
            this.statAdjustmentTypeFlagCheckBox.Text = "Stat Adjustment Type Flag";
            this.statAdjustmentTypeFlagCheckBox.UseVisualStyleBackColor = true;
            // 
            // wisdomCheckBox
            // 
            this.wisdomCheckBox.AutoSize = true;
            this.wisdomCheckBox.Location = new System.Drawing.Point(161, 47);
            this.wisdomCheckBox.Name = "wisdomCheckBox";
            this.wisdomCheckBox.Size = new System.Drawing.Size(70, 19);
            this.wisdomCheckBox.TabIndex = 4;
            this.wisdomCheckBox.Text = "Wisdom";
            this.wisdomCheckBox.UseVisualStyleBackColor = true;
            this.wisdomCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // intelligenceCheckBox
            // 
            this.intelligenceCheckBox.AutoSize = true;
            this.intelligenceCheckBox.Location = new System.Drawing.Point(161, 22);
            this.intelligenceCheckBox.Name = "intelligenceCheckBox";
            this.intelligenceCheckBox.Size = new System.Drawing.Size(87, 19);
            this.intelligenceCheckBox.TabIndex = 3;
            this.intelligenceCheckBox.Text = "Intelligence";
            this.intelligenceCheckBox.UseVisualStyleBackColor = true;
            this.intelligenceCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // constitutionCheckBox
            // 
            this.constitutionCheckBox.AutoSize = true;
            this.constitutionCheckBox.Location = new System.Drawing.Point(15, 72);
            this.constitutionCheckBox.Name = "constitutionCheckBox";
            this.constitutionCheckBox.Size = new System.Drawing.Size(92, 19);
            this.constitutionCheckBox.TabIndex = 2;
            this.constitutionCheckBox.Text = "Constitution";
            this.constitutionCheckBox.UseVisualStyleBackColor = true;
            this.constitutionCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // agilityCheckBox
            // 
            this.agilityCheckBox.AutoSize = true;
            this.agilityCheckBox.Location = new System.Drawing.Point(15, 47);
            this.agilityCheckBox.Name = "agilityCheckBox";
            this.agilityCheckBox.Size = new System.Drawing.Size(60, 19);
            this.agilityCheckBox.TabIndex = 1;
            this.agilityCheckBox.Text = "Agility";
            this.agilityCheckBox.UseVisualStyleBackColor = true;
            this.agilityCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // strengthCheckBox
            // 
            this.strengthCheckBox.AutoSize = true;
            this.strengthCheckBox.Location = new System.Drawing.Point(15, 22);
            this.strengthCheckBox.Name = "strengthCheckBox";
            this.strengthCheckBox.Size = new System.Drawing.Size(71, 19);
            this.strengthCheckBox.TabIndex = 0;
            this.strengthCheckBox.Text = "Strength";
            this.strengthCheckBox.UseVisualStyleBackColor = true;
            this.strengthCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nameTextBox);
            this.groupBox2.Controls.Add(this.elementComboBox);
            this.groupBox2.Controls.Add(this.iconPictureBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 191);
            this.groupBox2.TabIndex = 89;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.defenseNumericUpDown);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.magicEvasionNumericUpDown);
            this.groupBox3.Controls.Add(this.evasionNumericUpDown);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.magicDefenseNumericUpDown);
            this.groupBox3.Location = new System.Drawing.Point(277, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 144);
            this.groupBox3.TabIndex = 90;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Equipment Stats";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.popoieCheckBox);
            this.groupBox5.Controls.Add(this.purimCheckBox);
            this.groupBox5.Controls.Add(this.randiCheckBox);
            this.groupBox5.Location = new System.Drawing.Point(3, 200);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(268, 125);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Equippable By:";
            // 
            // popoieCheckBox
            // 
            this.popoieCheckBox.AutoSize = true;
            this.popoieCheckBox.Location = new System.Drawing.Point(15, 75);
            this.popoieCheckBox.Name = "popoieCheckBox";
            this.popoieCheckBox.Size = new System.Drawing.Size(63, 19);
            this.popoieCheckBox.TabIndex = 5;
            this.popoieCheckBox.Text = "Popoie";
            this.popoieCheckBox.UseVisualStyleBackColor = true;
            this.popoieCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // purimCheckBox
            // 
            this.purimCheckBox.AutoSize = true;
            this.purimCheckBox.Location = new System.Drawing.Point(15, 50);
            this.purimCheckBox.Name = "purimCheckBox";
            this.purimCheckBox.Size = new System.Drawing.Size(58, 19);
            this.purimCheckBox.TabIndex = 4;
            this.purimCheckBox.Text = "Purim";
            this.purimCheckBox.UseVisualStyleBackColor = true;
            this.purimCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // randiCheckBox
            // 
            this.randiCheckBox.AutoSize = true;
            this.randiCheckBox.Location = new System.Drawing.Point(15, 25);
            this.randiCheckBox.Name = "randiCheckBox";
            this.randiCheckBox.Size = new System.Drawing.Size(56, 19);
            this.randiCheckBox.TabIndex = 3;
            this.randiCheckBox.Text = "Randi";
            this.randiCheckBox.UseVisualStyleBackColor = true;
            this.randiCheckBox.CheckedChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // nameChangeTimer
            // 
            this.nameChangeTimer.Interval = 500;
            this.nameChangeTimer.Tick += new System.EventHandler(this.NameChangeTimer_Tick);
            // 
            // EquipmentDefinitionEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Name = "EquipmentDefinitionEditorUserControl";
            this.Size = new System.Drawing.Size(759, 335);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.defenseNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.evasionNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.magicDefenseNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.magicEvasionNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckedListBox resistancesCheckedListBox;
        private System.Windows.Forms.NumericUpDown defenseNumericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown evasionNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown magicDefenseNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown magicEvasionNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox elementComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox strengthCheckBox;
        private System.Windows.Forms.CheckBox wisdomCheckBox;
        private System.Windows.Forms.CheckBox intelligenceCheckBox;
        private System.Windows.Forms.CheckBox constitutionCheckBox;
        private System.Windows.Forms.CheckBox agilityCheckBox;
        private System.Windows.Forms.CheckBox statAdjustmentTypeFlagCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox popoieCheckBox;
        private System.Windows.Forms.CheckBox purimCheckBox;
        private System.Windows.Forms.CheckBox randiCheckBox;
        private System.Windows.Forms.Timer nameChangeTimer;
    }
}
