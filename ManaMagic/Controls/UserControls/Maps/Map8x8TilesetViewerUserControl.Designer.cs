
namespace ManaMagic.Controls.UserControls.Maps
{
    partial class Map8x8TilesetViewerUserControl
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
            this.paletteIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tilesetIdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.map8x8TilesetPreviousButton = new System.Windows.Forms.Button();
            this.paletteSetNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.map8x8TilesetPictureBox = new System.Windows.Forms.PictureBox();
            this.map8x8TilesetNextButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIndexNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetIdNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSetNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.map8x8TilesetPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.paletteIndexNumericUpDown);
            this.groupBox1.Controls.Add(this.tilesetIdNumericUpDown);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.map8x8TilesetPreviousButton);
            this.groupBox1.Controls.Add(this.paletteSetNumericUpDown);
            this.groupBox1.Controls.Add(this.map8x8TilesetPictureBox);
            this.groupBox1.Controls.Add(this.map8x8TilesetNextButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(613, 693);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tileset";
            // 
            // paletteIndexNumericUpDown
            // 
            this.paletteIndexNumericUpDown.Hexadecimal = true;
            this.paletteIndexNumericUpDown.Location = new System.Drawing.Point(512, 659);
            this.paletteIndexNumericUpDown.Name = "paletteIndexNumericUpDown";
            this.paletteIndexNumericUpDown.Size = new System.Drawing.Size(94, 23);
            this.paletteIndexNumericUpDown.TabIndex = 30;
            this.paletteIndexNumericUpDown.ValueChanged += new System.EventHandler(this.PaletteIndexNumericUpDown_ValueChanged);
            // 
            // tilesetIdNumericUpDown
            // 
            this.tilesetIdNumericUpDown.Hexadecimal = true;
            this.tilesetIdNumericUpDown.Location = new System.Drawing.Point(6, 628);
            this.tilesetIdNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tilesetIdNumericUpDown.Name = "tilesetIdNumericUpDown";
            this.tilesetIdNumericUpDown.Size = new System.Drawing.Size(76, 23);
            this.tilesetIdNumericUpDown.TabIndex = 19;
            this.tilesetIdNumericUpDown.ValueChanged += new System.EventHandler(this.TilesetIdNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 662);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 31;
            this.label2.Text = "Palette Index:";
            // 
            // map8x8TilesetPreviousButton
            // 
            this.map8x8TilesetPreviousButton.Enabled = false;
            this.map8x8TilesetPreviousButton.Location = new System.Drawing.Point(88, 628);
            this.map8x8TilesetPreviousButton.Name = "map8x8TilesetPreviousButton";
            this.map8x8TilesetPreviousButton.Size = new System.Drawing.Size(75, 23);
            this.map8x8TilesetPreviousButton.TabIndex = 16;
            this.map8x8TilesetPreviousButton.Text = "Previous";
            this.map8x8TilesetPreviousButton.UseVisualStyleBackColor = true;
            this.map8x8TilesetPreviousButton.Visible = false;
            // 
            // paletteSetNumericUpDown
            // 
            this.paletteSetNumericUpDown.Hexadecimal = true;
            this.paletteSetNumericUpDown.Location = new System.Drawing.Point(512, 630);
            this.paletteSetNumericUpDown.Name = "paletteSetNumericUpDown";
            this.paletteSetNumericUpDown.Size = new System.Drawing.Size(94, 23);
            this.paletteSetNumericUpDown.TabIndex = 28;
            this.paletteSetNumericUpDown.ValueChanged += new System.EventHandler(this.PaletteSetNumericUpDown_ValueChanged);
            // 
            // map8x8TilesetPictureBox
            // 
            this.map8x8TilesetPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map8x8TilesetPictureBox.Location = new System.Drawing.Point(6, 22);
            this.map8x8TilesetPictureBox.Name = "map8x8TilesetPictureBox";
            this.map8x8TilesetPictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.map8x8TilesetPictureBox.Size = new System.Drawing.Size(600, 600);
            this.map8x8TilesetPictureBox.TabIndex = 5;
            this.map8x8TilesetPictureBox.TabStop = false;
            // 
            // map8x8TilesetNextButton
            // 
            this.map8x8TilesetNextButton.Location = new System.Drawing.Point(169, 628);
            this.map8x8TilesetNextButton.Name = "map8x8TilesetNextButton";
            this.map8x8TilesetNextButton.Size = new System.Drawing.Size(75, 23);
            this.map8x8TilesetNextButton.TabIndex = 15;
            this.map8x8TilesetNextButton.Text = "Next";
            this.map8x8TilesetNextButton.UseVisualStyleBackColor = true;
            this.map8x8TilesetNextButton.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 633);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 29;
            this.label1.Text = "Palette Set:";
            // 
            // Map8x8TilesetViewerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "Map8x8TilesetViewerUserControl";
            this.Size = new System.Drawing.Size(618, 705);
            this.Load += new System.EventHandler(this.Map8x8TilesetViewerUserControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIndexNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetIdNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSetNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.map8x8TilesetPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button map8x8TilesetPreviousButton;
        private System.Windows.Forms.PictureBox map8x8TilesetPictureBox;
        private System.Windows.Forms.Button map8x8TilesetNextButton;
        private System.Windows.Forms.NumericUpDown paletteSetNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown paletteIndexNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown tilesetIdNumericUpDown;
    }
}
