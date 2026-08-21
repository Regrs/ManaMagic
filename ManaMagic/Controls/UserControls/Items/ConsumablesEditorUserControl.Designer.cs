
namespace ManaMagic.Controls.UserControls.Items
{
    partial class ConsumablesEditorUserControl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.paletteIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.amountHealedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.indexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nameChangeTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIndexNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amountHealedNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.paletteIndexNumericUpDown);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.amountHealedNumericUpDown);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nameTextBox);
            this.groupBox2.Controls.Add(this.iconPictureBox);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Location = new System.Drawing.Point(3, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 232);
            this.groupBox2.TabIndex = 90;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // paletteIndexNumericUpDown
            // 
            this.paletteIndexNumericUpDown.Hexadecimal = true;
            this.paletteIndexNumericUpDown.Location = new System.Drawing.Point(6, 143);
            this.paletteIndexNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.paletteIndexNumericUpDown.Name = "paletteIndexNumericUpDown";
            this.paletteIndexNumericUpDown.Size = new System.Drawing.Size(254, 23);
            this.paletteIndexNumericUpDown.TabIndex = 80;
            this.paletteIndexNumericUpDown.ValueChanged += new System.EventHandler(this.PaletteIndexNumericUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 81;
            this.label3.Text = "Palete Index";
            // 
            // amountHealedNumericUpDown
            // 
            this.amountHealedNumericUpDown.Location = new System.Drawing.Point(6, 94);
            this.amountHealedNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.amountHealedNumericUpDown.Name = "amountHealedNumericUpDown";
            this.amountHealedNumericUpDown.Size = new System.Drawing.Size(254, 23);
            this.amountHealedNumericUpDown.TabIndex = 78;
            this.amountHealedNumericUpDown.ValueChanged += new System.EventHandler(this.AmountHealedNumericUpDown_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(6, 76);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(91, 15);
            this.label14.TabIndex = 79;
            this.label14.Text = "Amount Healed";
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
            this.nameTextBox.TextChanged += new System.EventHandler(this.NameTextBox_TextChanged);
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconPictureBox.Location = new System.Drawing.Point(214, 176);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(48, 48);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconPictureBox.TabIndex = 72;
            this.iconPictureBox.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(175, 191);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 15);
            this.label16.TabIndex = 73;
            this.label16.Text = "Icon:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 92;
            this.label2.Text = "Index";
            // 
            // indexNumericUpDown
            // 
            this.indexNumericUpDown.Hexadecimal = true;
            this.indexNumericUpDown.Location = new System.Drawing.Point(3, 25);
            this.indexNumericUpDown.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.indexNumericUpDown.Name = "indexNumericUpDown";
            this.indexNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.indexNumericUpDown.TabIndex = 91;
            this.indexNumericUpDown.ValueChanged += new System.EventHandler(this.IndexNumericUpDown_ValueChanged);
            // 
            // nameChangeTimer
            // 
            this.nameChangeTimer.Interval = 500;
            this.nameChangeTimer.Tick += new System.EventHandler(this.NameChangeTimer_Tick);
            // 
            // ConsumablesEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.indexNumericUpDown);
            this.Controls.Add(this.groupBox2);
            this.Name = "ConsumablesEditorUserControl";
            this.Size = new System.Drawing.Size(275, 292);
            this.Load += new System.EventHandler(this.ConsumablesEditorUserControl_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIndexNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amountHealedNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.indexNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown amountHealedNumericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown indexNumericUpDown;
        private System.Windows.Forms.NumericUpDown paletteIndexNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer nameChangeTimer;
    }
}
