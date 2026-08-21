
namespace ManaMagic.Controls.UserControls.Maps
{
    partial class Map16x16TilesetViewerUserControl
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
            this.layer1PictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.layer2PictureBox = new System.Windows.Forms.PictureBox();
            this.paletteSetNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tileset8x8NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tileset16x16NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layer1PictureBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layer2PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSetNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileset8x8NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileset16x16NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.layer1PictureBox);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(539, 423);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layer 1";
            // 
            // layer1PictureBox
            // 
            this.layer1PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer1PictureBox.Location = new System.Drawing.Point(6, 22);
            this.layer1PictureBox.Name = "layer1PictureBox";
            this.layer1PictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.layer1PictureBox.Size = new System.Drawing.Size(527, 395);
            this.layer1PictureBox.TabIndex = 5;
            this.layer1PictureBox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.layer2PictureBox);
            this.groupBox2.Location = new System.Drawing.Point(545, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(539, 423);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layer 2";
            // 
            // layer2PictureBox
            // 
            this.layer2PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer2PictureBox.Location = new System.Drawing.Point(6, 22);
            this.layer2PictureBox.Name = "layer2PictureBox";
            this.layer2PictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.layer2PictureBox.Size = new System.Drawing.Size(527, 395);
            this.layer2PictureBox.TabIndex = 5;
            this.layer2PictureBox.TabStop = false;
            // 
            // paletteSetNumericUpDown
            // 
            this.paletteSetNumericUpDown.Hexadecimal = true;
            this.paletteSetNumericUpDown.Location = new System.Drawing.Point(84, 487);
            this.paletteSetNumericUpDown.Name = "paletteSetNumericUpDown";
            this.paletteSetNumericUpDown.Size = new System.Drawing.Size(94, 23);
            this.paletteSetNumericUpDown.TabIndex = 30;
            this.paletteSetNumericUpDown.ValueChanged += new System.EventHandler(this.PaletteSetNumericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 490);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 31;
            this.label1.Text = "Palette Set:";
            // 
            // tileset8x8NumericUpDown
            // 
            this.tileset8x8NumericUpDown.Hexadecimal = true;
            this.tileset8x8NumericUpDown.Location = new System.Drawing.Point(84, 429);
            this.tileset8x8NumericUpDown.Name = "tileset8x8NumericUpDown";
            this.tileset8x8NumericUpDown.Size = new System.Drawing.Size(94, 23);
            this.tileset8x8NumericUpDown.TabIndex = 32;
            this.tileset8x8NumericUpDown.ValueChanged += new System.EventHandler(this.Tileset8x8NumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 432);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "8x8 Tileset:";
            // 
            // tileset16x16NumericUpDown
            // 
            this.tileset16x16NumericUpDown.Hexadecimal = true;
            this.tileset16x16NumericUpDown.Location = new System.Drawing.Point(84, 458);
            this.tileset16x16NumericUpDown.Name = "tileset16x16NumericUpDown";
            this.tileset16x16NumericUpDown.Size = new System.Drawing.Size(94, 23);
            this.tileset16x16NumericUpDown.TabIndex = 34;
            this.tileset16x16NumericUpDown.ValueChanged += new System.EventHandler(this.Tileset16x16NumericUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 460);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 35;
            this.label3.Text = "16x16 Tileset:";
            // 
            // Map16x16TilesetViewerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tileset16x16NumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tileset8x8NumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.paletteSetNumericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Map16x16TilesetViewerUserControl";
            this.Size = new System.Drawing.Size(1089, 531);
            this.Load += new System.EventHandler(this.Map16x16TilesetViewerUserControl_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layer1PictureBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layer2PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteSetNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileset8x8NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileset16x16NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox layer1PictureBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox layer2PictureBox;
        private System.Windows.Forms.NumericUpDown paletteSetNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown tileset8x8NumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown tileset16x16NumericUpDown;
        private System.Windows.Forms.Label label3;
    }
}
