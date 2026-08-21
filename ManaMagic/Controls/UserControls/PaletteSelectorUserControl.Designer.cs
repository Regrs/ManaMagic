
namespace ManaMagic.Controls.UserControls
{
    partial class PaletteSelectorUserControl
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
            this.paletteControl = new ManaMagic.Controls.PaletteControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.paletteSelectorNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.changePaletteButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSelectorNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // paletteControl
            // 
            this.paletteControl.Location = new System.Drawing.Point(6, 22);
            this.paletteControl.Name = "paletteControl";
            this.paletteControl.NumberOfPalettes = 16;
            this.paletteControl.Size = new System.Drawing.Size(284, 100);
            this.paletteControl.TabIndex = 0;
            this.paletteControl.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.paletteSelectorNumericUpDown);
            this.groupBox1.Controls.Add(this.changePaletteButton);
            this.groupBox1.Controls.Add(this.paletteControl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 164);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Palette";
            // 
            // paletteSelectorNumericUpDown
            // 
            this.paletteSelectorNumericUpDown.Hexadecimal = true;
            this.paletteSelectorNumericUpDown.Location = new System.Drawing.Point(119, 128);
            this.paletteSelectorNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.paletteSelectorNumericUpDown.Name = "paletteSelectorNumericUpDown";
            this.paletteSelectorNumericUpDown.Size = new System.Drawing.Size(56, 23);
            this.paletteSelectorNumericUpDown.TabIndex = 2;
            this.paletteSelectorNumericUpDown.ValueChanged += new System.EventHandler(this.paletteSelectorNumericUpDown_ValueChanged);
            // 
            // changePaletteButton
            // 
            this.changePaletteButton.Location = new System.Drawing.Point(6, 128);
            this.changePaletteButton.Name = "changePaletteButton";
            this.changePaletteButton.Size = new System.Drawing.Size(107, 23);
            this.changePaletteButton.TabIndex = 1;
            this.changePaletteButton.Text = "Change Palette";
            this.changePaletteButton.UseVisualStyleBackColor = true;
            this.changePaletteButton.Click += new System.EventHandler(this.changePaletteButton_Click);
            // 
            // PaletteSelectorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PaletteSelectorUserControl";
            this.Size = new System.Drawing.Size(304, 164);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paletteSelectorNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ManaMagic.Controls.PaletteControl paletteControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown paletteSelectorNumericUpDown;
        private System.Windows.Forms.Button changePaletteButton;
    }
}
