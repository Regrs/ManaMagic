
namespace ManaMagic.Controls.UserControls.Boss
{
    partial class BossPaletteEditorUserControl
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
            this.label6 = new System.Windows.Forms.Label();
            this.indexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.indexNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // paletteControl
            // 
            this.paletteControl.AllowEditing = true;
            this.paletteControl.AllowSelection = true;
            this.paletteControl.LabelWidth = 35;
            this.paletteControl.Location = new System.Drawing.Point(3, 35);
            this.paletteControl.Name = "paletteControl";
            this.paletteControl.NumberOfPalettes = 16;
            this.paletteControl.Size = new System.Drawing.Size(572, 223);
            this.paletteControl.TabIndex = 98;
            this.paletteControl.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 15);
            this.label6.TabIndex = 100;
            this.label6.Text = "Index";
            // 
            // indexNumericUpDown
            // 
            this.indexNumericUpDown.Hexadecimal = true;
            this.indexNumericUpDown.Location = new System.Drawing.Point(3, 18);
            this.indexNumericUpDown.Maximum = new decimal(new int[] {
            86,
            0,
            0,
            0});
            this.indexNumericUpDown.Name = "indexNumericUpDown";
            this.indexNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.indexNumericUpDown.TabIndex = 99;
            // 
            // BossPaletteEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.indexNumericUpDown);
            this.Controls.Add(this.paletteControl);
            this.Name = "BossPaletteEditorUserControl";
            this.Size = new System.Drawing.Size(721, 540);
            ((System.ComponentModel.ISupportInitialize)(this.indexNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PaletteControl paletteControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown indexNumericUpDown;
    }
}
