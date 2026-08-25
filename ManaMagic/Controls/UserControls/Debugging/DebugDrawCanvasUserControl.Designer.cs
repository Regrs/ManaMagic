
namespace ManaMagic.Controls.UserControls.Debug
{
    partial class DebugDrawCanvasUserControl
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.previousCanvas1Button = new System.Windows.Forms.Button();
            this.canvas1PictureBox = new System.Windows.Forms.PictureBox();
            this.nextCanvas1Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.c2PrevButton = new System.Windows.Forms.Button();
            this.canvas2PictureBox = new System.Windows.Forms.PictureBox();
            this.c2NextButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas1PictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas2PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.previousCanvas1Button);
            this.groupBox2.Controls.Add(this.canvas1PictureBox);
            this.groupBox2.Controls.Add(this.nextCanvas1Button);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(552, 662);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Canvas #1";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Hexadecimal = true;
            this.numericUpDown1.Location = new System.Drawing.Point(168, 633);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(117, 23);
            this.numericUpDown1.TabIndex = 17;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericUpDown1_ValueChanged);
            // 
            // previousCanvas1Button
            // 
            this.previousCanvas1Button.Location = new System.Drawing.Point(6, 631);
            this.previousCanvas1Button.Name = "previousCanvas1Button";
            this.previousCanvas1Button.Size = new System.Drawing.Size(75, 23);
            this.previousCanvas1Button.TabIndex = 16;
            this.previousCanvas1Button.Text = "Previous";
            this.previousCanvas1Button.UseVisualStyleBackColor = true;
            this.previousCanvas1Button.Click += new System.EventHandler(this.PreviousCanvas1Button_Click);
            // 
            // canvas1PictureBox
            // 
            this.canvas1PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvas1PictureBox.Location = new System.Drawing.Point(6, 22);
            this.canvas1PictureBox.Name = "canvas1PictureBox";
            this.canvas1PictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.canvas1PictureBox.Size = new System.Drawing.Size(534, 603);
            this.canvas1PictureBox.TabIndex = 5;
            this.canvas1PictureBox.TabStop = false;
            // 
            // nextCanvas1Button
            // 
            this.nextCanvas1Button.Location = new System.Drawing.Point(87, 631);
            this.nextCanvas1Button.Name = "nextCanvas1Button";
            this.nextCanvas1Button.Size = new System.Drawing.Size(75, 23);
            this.nextCanvas1Button.TabIndex = 15;
            this.nextCanvas1Button.Text = "Next";
            this.nextCanvas1Button.UseVisualStyleBackColor = true;
            this.nextCanvas1Button.Click += new System.EventHandler(this.NextCanvas1Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.c2PrevButton);
            this.groupBox1.Controls.Add(this.canvas2PictureBox);
            this.groupBox1.Controls.Add(this.c2NextButton);
            this.groupBox1.Location = new System.Drawing.Point(558, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(806, 826);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Canvas #2";
            // 
            // c2PrevButton
            // 
            this.c2PrevButton.Location = new System.Drawing.Point(6, 797);
            this.c2PrevButton.Name = "c2PrevButton";
            this.c2PrevButton.Size = new System.Drawing.Size(75, 23);
            this.c2PrevButton.TabIndex = 16;
            this.c2PrevButton.Text = "Previous";
            this.c2PrevButton.UseVisualStyleBackColor = true;
            this.c2PrevButton.Click += new System.EventHandler(this.C2PrevButton_Click);
            // 
            // canvas2PictureBox
            // 
            this.canvas2PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvas2PictureBox.Location = new System.Drawing.Point(6, 22);
            this.canvas2PictureBox.Name = "canvas2PictureBox";
            this.canvas2PictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.canvas2PictureBox.Size = new System.Drawing.Size(600, 600);
            this.canvas2PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.canvas2PictureBox.TabIndex = 5;
            this.canvas2PictureBox.TabStop = false;
            // 
            // c2NextButton
            // 
            this.c2NextButton.Location = new System.Drawing.Point(87, 797);
            this.c2NextButton.Name = "c2NextButton";
            this.c2NextButton.Size = new System.Drawing.Size(75, 23);
            this.c2NextButton.TabIndex = 15;
            this.c2NextButton.Text = "Next";
            this.c2NextButton.UseVisualStyleBackColor = true;
            this.c2NextButton.Click += new System.EventHandler(this.C2NextButton_Click);
            // 
            // DebugDrawCanvasUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "DebugDrawCanvasUserControl";
            this.Size = new System.Drawing.Size(1367, 832);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas1PictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas2PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button previousCanvas1Button;
        private System.Windows.Forms.PictureBox canvas1PictureBox;
        private System.Windows.Forms.Button nextCanvas1Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button c2PrevButton;
        private System.Windows.Forms.PictureBox canvas2PictureBox;
        private System.Windows.Forms.Button c2NextButton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}
