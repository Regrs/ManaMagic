
namespace ManaMagic.Controls.UserControls.Sprites
{
    partial class SpritePaletteEditorUserControl
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
            this.paletteControl.LabelWidth = 40;
            this.paletteControl.Location = new System.Drawing.Point(3, 50);
            this.paletteControl.Name = "paletteControl";
            this.paletteControl.NumberOfPalettes = 16;
            this.paletteControl.Size = new System.Drawing.Size(652, 331);
            this.paletteControl.TabIndex = 106;
            this.paletteControl.TabStop = false;
            this.paletteControl.ColorChanged += new System.EventHandler<ManaMagic.Controls.ColorChanged>(this.PaletteControl_ColorChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 15);
            this.label6.TabIndex = 105;
            this.label6.Text = "Index";
            // 
            // indexNumericUpDown
            // 
            this.indexNumericUpDown.Hexadecimal = true;
            this.indexNumericUpDown.Location = new System.Drawing.Point(3, 21);
            this.indexNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.indexNumericUpDown.Name = "indexNumericUpDown";
            this.indexNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.indexNumericUpDown.TabIndex = 104;
            this.indexNumericUpDown.ValueChanged += new System.EventHandler(this.IndexNumericUpDown_ValueChanged);
            // 
            // SpritePaletteEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paletteControl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.indexNumericUpDown);
            this.Name = "SpritePaletteEditorUserControl";
            this.Size = new System.Drawing.Size(669, 397);
            this.Load += new System.EventHandler(this.SpritePaletteEditorUserControl_Load);
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
