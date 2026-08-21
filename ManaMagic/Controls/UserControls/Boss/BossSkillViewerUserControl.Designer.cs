
namespace ManaMagic.Controls.UserControls.Boss
{
    partial class BossSkillViewerUserControl
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
            this.tileSheetPictureBox = new System.Windows.Forms.PictureBox();
            this.previousButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.paletteSelector1UserControl = new ManaMagic.Controls.UserControls.PaletteSelectorUserControl();
            this.skillPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.nextFrameButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.showGridlinesCheckBox = new System.Windows.Forms.CheckBox();
            this.previousFrameButton = new System.Windows.Forms.Button();
            this.framePictureBox = new System.Windows.Forms.PictureBox();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.propertyGridSplitContainer = new System.Windows.Forms.SplitContainer();
            this.framePropertyGrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.tileSheetPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridSplitContainer)).BeginInit();
            this.propertyGridSplitContainer.Panel1.SuspendLayout();
            this.propertyGridSplitContainer.Panel2.SuspendLayout();
            this.propertyGridSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tileSheetPictureBox
            // 
            this.tileSheetPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tileSheetPictureBox.Location = new System.Drawing.Point(6, 22);
            this.tileSheetPictureBox.Name = "tileSheetPictureBox";
            this.tileSheetPictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.tileSheetPictureBox.Size = new System.Drawing.Size(280, 440);
            this.tileSheetPictureBox.TabIndex = 4;
            this.tileSheetPictureBox.TabStop = false;
            // 
            // previousButton
            // 
            this.previousButton.Location = new System.Drawing.Point(6, 468);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(75, 23);
            this.previousButton.TabIndex = 7;
            this.previousButton.Text = "Previous";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(87, 468);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 8;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // paletteSelector1UserControl
            // 
            this.paletteSelector1UserControl.Location = new System.Drawing.Point(724, 3);
            this.paletteSelector1UserControl.Name = "paletteSelector1UserControl";
            this.paletteSelector1UserControl.PaletteMaximum = 255;
            this.paletteSelector1UserControl.PaletteMinimum = 0;
            this.paletteSelector1UserControl.Size = new System.Drawing.Size(304, 164);
            this.paletteSelector1UserControl.TabIndex = 9;
            // 
            // skillPropertyGrid
            // 
            this.skillPropertyGrid.DisabledItemForeColor = System.Drawing.SystemColors.ControlText;
            this.skillPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skillPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.skillPropertyGrid.Name = "skillPropertyGrid";
            this.skillPropertyGrid.Size = new System.Drawing.Size(317, 349);
            this.skillPropertyGrid.TabIndex = 14;
            // 
            // nextFrameButton
            // 
            this.nextFrameButton.Location = new System.Drawing.Point(87, 428);
            this.nextFrameButton.Name = "nextFrameButton";
            this.nextFrameButton.Size = new System.Drawing.Size(75, 23);
            this.nextFrameButton.TabIndex = 15;
            this.nextFrameButton.Text = "Next";
            this.nextFrameButton.UseVisualStyleBackColor = true;
            this.nextFrameButton.Click += new System.EventHandler(this.nextFrameButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tileSheetPictureBox);
            this.groupBox1.Controls.Add(this.previousButton);
            this.groupBox1.Controls.Add(this.nextButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 503);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tileset";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.showGridlinesCheckBox);
            this.groupBox2.Controls.Add(this.previousFrameButton);
            this.groupBox2.Controls.Add(this.framePictureBox);
            this.groupBox2.Controls.Add(this.nextFrameButton);
            this.groupBox2.Location = new System.Drawing.Point(303, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 504);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Frame";
            // 
            // showGridlinesCheckBox
            // 
            this.showGridlinesCheckBox.AutoSize = true;
            this.showGridlinesCheckBox.Checked = true;
            this.showGridlinesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridlinesCheckBox.Location = new System.Drawing.Point(6, 457);
            this.showGridlinesCheckBox.Name = "showGridlinesCheckBox";
            this.showGridlinesCheckBox.Size = new System.Drawing.Size(72, 19);
            this.showGridlinesCheckBox.TabIndex = 18;
            this.showGridlinesCheckBox.Text = "Gridlines";
            this.showGridlinesCheckBox.UseVisualStyleBackColor = true;
            this.showGridlinesCheckBox.CheckedChanged += new System.EventHandler(this.gridlinesCheckBox_CheckedChanged);
            // 
            // previousFrameButton
            // 
            this.previousFrameButton.Location = new System.Drawing.Point(6, 428);
            this.previousFrameButton.Name = "previousFrameButton";
            this.previousFrameButton.Size = new System.Drawing.Size(75, 23);
            this.previousFrameButton.TabIndex = 16;
            this.previousFrameButton.Text = "Previous";
            this.previousFrameButton.UseVisualStyleBackColor = true;
            this.previousFrameButton.Click += new System.EventHandler(this.previousFrameButton_Click);
            // 
            // framePictureBox
            // 
            this.framePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.framePictureBox.Location = new System.Drawing.Point(6, 22);
            this.framePictureBox.Name = "framePictureBox";
            this.framePictureBox.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.framePictureBox.Size = new System.Drawing.Size(400, 400);
            this.framePictureBox.TabIndex = 5;
            this.framePictureBox.TabStop = false;
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.groupBox1);
            this.mainSplitContainer.Panel1.Controls.Add(this.groupBox2);
            this.mainSplitContainer.Panel1.Controls.Add(this.paletteSelector1UserControl);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.propertyGridSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(1352, 709);
            this.mainSplitContainer.SplitterDistance = 1025;
            this.mainSplitContainer.TabIndex = 20;
            // 
            // propertyGridSplitContainer
            // 
            this.propertyGridSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.propertyGridSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.propertyGridSplitContainer.Name = "propertyGridSplitContainer";
            this.propertyGridSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // propertyGridSplitContainer.Panel1
            // 
            this.propertyGridSplitContainer.Panel1.Controls.Add(this.skillPropertyGrid);
            // 
            // propertyGridSplitContainer.Panel2
            // 
            this.propertyGridSplitContainer.Panel2.Controls.Add(this.framePropertyGrid);
            this.propertyGridSplitContainer.Size = new System.Drawing.Size(321, 707);
            this.propertyGridSplitContainer.SplitterDistance = 353;
            this.propertyGridSplitContainer.TabIndex = 15;
            // 
            // framePropertyGrid
            // 
            this.framePropertyGrid.DisabledItemForeColor = System.Drawing.SystemColors.ControlText;
            this.framePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framePropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.framePropertyGrid.Name = "framePropertyGrid";
            this.framePropertyGrid.Size = new System.Drawing.Size(317, 346);
            this.framePropertyGrid.TabIndex = 15;
            // 
            // BossSkillViewerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "BossSkillViewerUserControl";
            this.Size = new System.Drawing.Size(1352, 709);
            this.Load += new System.EventHandler(this.BossSkillViewerUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tileSheetPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framePictureBox)).EndInit();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.propertyGridSplitContainer.Panel1.ResumeLayout(false);
            this.propertyGridSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridSplitContainer)).EndInit();
            this.propertyGridSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox tileSheetPictureBox;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button nextButton;
        private PaletteSelectorUserControl paletteSelector1UserControl;
        private System.Windows.Forms.PropertyGrid skillPropertyGrid;
        private System.Windows.Forms.Button nextFrameButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox framePictureBox;
        private System.Windows.Forms.Button previousFrameButton;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer propertyGridSplitContainer;
        private System.Windows.Forms.CheckBox showGridlinesCheckBox;
        private System.Windows.Forms.PropertyGrid framePropertyGrid;
    }
}
