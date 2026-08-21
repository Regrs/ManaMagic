
namespace ManaMagic.Controls.UserControls.Items
{
    partial class WeaponDefinitionEditorUserControl
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.weaponClassComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.strengthModifierComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.agilityModifierComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.constitutionModifierComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.intelligenceModifierComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.wisdomModifierComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.projectileTypeComboBox = new System.Windows.Forms.ComboBox();
            this.paletteIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.affinityComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.critChanceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.accuracyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.powerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.inflictionRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.statusEffectsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label17 = new System.Windows.Forms.Label();
            this.unknownIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nameChangeTimer = new System.Windows.Forms.Timer(this.components);
            this.descriptionChangeTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.paletteIndexNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.critChanceNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accuracyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inflictionRateNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknownIndexNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(11, 35);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(254, 23);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(11, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // descriptionRichTextBox
            // 
            this.descriptionRichTextBox.Location = new System.Drawing.Point(11, 82);
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.Size = new System.Drawing.Size(254, 96);
            this.descriptionRichTextBox.TabIndex = 2;
            this.descriptionRichTextBox.Text = "";
            this.descriptionRichTextBox.TextChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(11, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 272);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 15);
            this.label10.TabIndex = 42;
            this.label10.Text = "Weapon Class:";
            // 
            // weaponClassComboBox
            // 
            this.weaponClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.weaponClassComboBox.FormattingEnabled = true;
            this.weaponClassComboBox.Location = new System.Drawing.Point(110, 268);
            this.weaponClassComboBox.Name = "weaponClassComboBox";
            this.weaponClassComboBox.Size = new System.Drawing.Size(155, 23);
            this.weaponClassComboBox.TabIndex = 41;
            this.weaponClassComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 15);
            this.label3.TabIndex = 44;
            this.label3.Text = "Strength Modifier:";
            // 
            // strengthModifierComboBox
            // 
            this.strengthModifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.strengthModifierComboBox.FormattingEnabled = true;
            this.strengthModifierComboBox.Location = new System.Drawing.Point(139, 22);
            this.strengthModifierComboBox.Name = "strengthModifierComboBox";
            this.strengthModifierComboBox.Size = new System.Drawing.Size(155, 23);
            this.strengthModifierComboBox.TabIndex = 43;
            this.strengthModifierComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 15);
            this.label4.TabIndex = 46;
            this.label4.Text = "Agility Modifier:";
            // 
            // agilityModifierComboBox
            // 
            this.agilityModifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.agilityModifierComboBox.FormattingEnabled = true;
            this.agilityModifierComboBox.Location = new System.Drawing.Point(139, 51);
            this.agilityModifierComboBox.Name = "agilityModifierComboBox";
            this.agilityModifierComboBox.Size = new System.Drawing.Size(155, 23);
            this.agilityModifierComboBox.TabIndex = 45;
            this.agilityModifierComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 15);
            this.label5.TabIndex = 48;
            this.label5.Text = "Constitution Modifier:";
            // 
            // constitutionModifierComboBox
            // 
            this.constitutionModifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.constitutionModifierComboBox.FormattingEnabled = true;
            this.constitutionModifierComboBox.Location = new System.Drawing.Point(139, 80);
            this.constitutionModifierComboBox.Name = "constitutionModifierComboBox";
            this.constitutionModifierComboBox.Size = new System.Drawing.Size(155, 23);
            this.constitutionModifierComboBox.TabIndex = 47;
            this.constitutionModifierComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 15);
            this.label6.TabIndex = 50;
            this.label6.Text = "Intelligence Modifier:";
            // 
            // intelligenceModifierComboBox
            // 
            this.intelligenceModifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.intelligenceModifierComboBox.FormattingEnabled = true;
            this.intelligenceModifierComboBox.Location = new System.Drawing.Point(139, 109);
            this.intelligenceModifierComboBox.Name = "intelligenceModifierComboBox";
            this.intelligenceModifierComboBox.Size = new System.Drawing.Size(155, 23);
            this.intelligenceModifierComboBox.TabIndex = 49;
            this.intelligenceModifierComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 15);
            this.label7.TabIndex = 52;
            this.label7.Text = "Wisdom Modifier:";
            // 
            // wisdomModifierComboBox
            // 
            this.wisdomModifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wisdomModifierComboBox.FormattingEnabled = true;
            this.wisdomModifierComboBox.Location = new System.Drawing.Point(139, 140);
            this.wisdomModifierComboBox.Name = "wisdomModifierComboBox";
            this.wisdomModifierComboBox.Size = new System.Drawing.Size(155, 23);
            this.wisdomModifierComboBox.TabIndex = 51;
            this.wisdomModifierComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 15);
            this.label8.TabIndex = 54;
            this.label8.Text = "Projectile Type:";
            // 
            // projectileTypeComboBox
            // 
            this.projectileTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectileTypeComboBox.FormattingEnabled = true;
            this.projectileTypeComboBox.Location = new System.Drawing.Point(139, 110);
            this.projectileTypeComboBox.Name = "projectileTypeComboBox";
            this.projectileTypeComboBox.Size = new System.Drawing.Size(155, 23);
            this.projectileTypeComboBox.TabIndex = 53;
            this.projectileTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // paletteIndexNumericUpDown
            // 
            this.paletteIndexNumericUpDown.Hexadecimal = true;
            this.paletteIndexNumericUpDown.Location = new System.Drawing.Point(110, 297);
            this.paletteIndexNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.paletteIndexNumericUpDown.Name = "paletteIndexNumericUpDown";
            this.paletteIndexNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.paletteIndexNumericUpDown.TabIndex = 55;
            this.paletteIndexNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 301);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 15);
            this.label9.TabIndex = 56;
            this.label9.Text = "Palette Index:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(84, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 15);
            this.label11.TabIndex = 58;
            this.label11.Text = "Affinity:";
            // 
            // affinityComboBox
            // 
            this.affinityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.affinityComboBox.FormattingEnabled = true;
            this.affinityComboBox.Location = new System.Drawing.Point(139, 139);
            this.affinityComboBox.Name = "affinityComboBox";
            this.affinityComboBox.Size = new System.Drawing.Size(155, 23);
            this.affinityComboBox.TabIndex = 57;
            this.affinityComboBox.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(43, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 15);
            this.label12.TabIndex = 60;
            this.label12.Text = "Critical Chance:";
            // 
            // critChanceNumericUpDown
            // 
            this.critChanceNumericUpDown.Location = new System.Drawing.Point(139, 81);
            this.critChanceNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.critChanceNumericUpDown.Name = "critChanceNumericUpDown";
            this.critChanceNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.critChanceNumericUpDown.TabIndex = 59;
            this.critChanceNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(74, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 15);
            this.label13.TabIndex = 62;
            this.label13.Text = "Accuracy:";
            // 
            // accuracyNumericUpDown
            // 
            this.accuracyNumericUpDown.Location = new System.Drawing.Point(139, 52);
            this.accuracyNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.accuracyNumericUpDown.Name = "accuracyNumericUpDown";
            this.accuracyNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.accuracyNumericUpDown.TabIndex = 61;
            this.accuracyNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(90, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 15);
            this.label14.TabIndex = 64;
            this.label14.Text = "Power:";
            // 
            // powerNumericUpDown
            // 
            this.powerNumericUpDown.Location = new System.Drawing.Point(139, 22);
            this.powerNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.powerNumericUpDown.Name = "powerNumericUpDown";
            this.powerNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.powerNumericUpDown.TabIndex = 63;
            this.powerNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
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
            // statusEffectsCheckedListBox
            // 
            this.statusEffectsCheckedListBox.FormattingEnabled = true;
            this.statusEffectsCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.statusEffectsCheckedListBox.Name = "statusEffectsCheckedListBox";
            this.statusEffectsCheckedListBox.Size = new System.Drawing.Size(186, 292);
            this.statusEffectsCheckedListBox.TabIndex = 67;
            this.statusEffectsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.StatusEffectsCheckedListBox_ItemCheck);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 329);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(93, 15);
            this.label17.TabIndex = 70;
            this.label17.Text = "Unknown Index:";
            // 
            // unknownIndexNumericUpDown
            // 
            this.unknownIndexNumericUpDown.Hexadecimal = true;
            this.unknownIndexNumericUpDown.Location = new System.Drawing.Point(110, 326);
            this.unknownIndexNumericUpDown.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.unknownIndexNumericUpDown.Name = "unknownIndexNumericUpDown";
            this.unknownIndexNumericUpDown.Size = new System.Drawing.Size(155, 23);
            this.unknownIndexNumericUpDown.TabIndex = 69;
            this.unknownIndexNumericUpDown.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.strengthModifierComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.agilityModifierComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.constitutionModifierComboBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.intelligenceModifierComboBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.wisdomModifierComboBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(287, 187);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 185);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stat Modifiers";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.powerNumericUpDown);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.accuracyNumericUpDown);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.critChanceNumericUpDown);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.projectileTypeComboBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.affinityComboBox);
            this.groupBox2.Location = new System.Drawing.Point(287, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 178);
            this.groupBox2.TabIndex = 72;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Weapon Stats";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.iconPictureBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.nameTextBox);
            this.groupBox3.Controls.Add(this.descriptionRichTextBox);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.unknownIndexNumericUpDown);
            this.groupBox3.Controls.Add(this.weaponClassComboBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.paletteIndexNumericUpDown);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(278, 369);
            this.groupBox3.TabIndex = 73;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(178, 209);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 15);
            this.label16.TabIndex = 71;
            this.label16.Text = "Icon:";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconPictureBox.Location = new System.Drawing.Point(217, 194);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(48, 48);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconPictureBox.TabIndex = 65;
            this.iconPictureBox.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.statusEffectsCheckedListBox);
            this.groupBox4.Controls.Add(this.inflictionRateNumericUpDown);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(597, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 369);
            this.groupBox4.TabIndex = 74;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Status Effects";
            // 
            // nameChangeTimer
            // 
            this.nameChangeTimer.Interval = 500;
            this.nameChangeTimer.Tick += new System.EventHandler(this.NameChangeTimer_Tick);
            // 
            // descriptionChangeTimer
            // 
            this.descriptionChangeTimer.Interval = 500;
            this.descriptionChangeTimer.Tick += new System.EventHandler(this.DescriptionChangeTimer_Tick);
            // 
            // WeaponDefinitionEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WeaponDefinitionEditorUserControl";
            this.Size = new System.Drawing.Size(808, 378);
            ((System.ComponentModel.ISupportInitialize)(this.paletteIndexNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.critChanceNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accuracyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inflictionRateNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknownIndexNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox descriptionRichTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox weaponClassComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox strengthModifierComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox agilityModifierComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox constitutionModifierComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox intelligenceModifierComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox wisdomModifierComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox projectileTypeComboBox;
        private System.Windows.Forms.NumericUpDown paletteIndexNumericUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox affinityComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown critChanceNumericUpDown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown accuracyNumericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown powerNumericUpDown;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown inflictionRateNumericUpDown;
        private System.Windows.Forms.CheckedListBox statusEffectsCheckedListBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown unknownIndexNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Timer nameChangeTimer;
        private System.Windows.Forms.Timer descriptionChangeTimer;
    }
}
