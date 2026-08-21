
namespace ManaMagic
{
    partial class NewProjectForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.romFileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.romFileBrowseButton = new System.Windows.Forms.Button();
            this.projectPathButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.projectPathTextBox = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.projectNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // romFileTextBox
            // 
            this.romFileTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.romFileTextBox.Location = new System.Drawing.Point(12, 33);
            this.romFileTextBox.Name = "romFileTextBox";
            this.romFileTextBox.ReadOnly = true;
            this.romFileTextBox.Size = new System.Drawing.Size(494, 23);
            this.romFileTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "ROM File";
            // 
            // romFileBrowseButton
            // 
            this.romFileBrowseButton.Location = new System.Drawing.Point(512, 33);
            this.romFileBrowseButton.Name = "romFileBrowseButton";
            this.romFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.romFileBrowseButton.TabIndex = 2;
            this.romFileBrowseButton.Text = "Browse...";
            this.romFileBrowseButton.UseVisualStyleBackColor = true;
            this.romFileBrowseButton.Click += new System.EventHandler(this.RomFileBrowseButton_Click);
            // 
            // projectPathButton
            // 
            this.projectPathButton.Location = new System.Drawing.Point(512, 93);
            this.projectPathButton.Name = "projectPathButton";
            this.projectPathButton.Size = new System.Drawing.Size(75, 23);
            this.projectPathButton.TabIndex = 5;
            this.projectPathButton.Text = "Browse...";
            this.projectPathButton.UseVisualStyleBackColor = true;
            this.projectPathButton.Click += new System.EventHandler(this.ProjectPathButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Project Path";
            // 
            // projectPathTextBox
            // 
            this.projectPathTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.projectPathTextBox.Location = new System.Drawing.Point(12, 93);
            this.projectPathTextBox.Name = "projectPathTextBox";
            this.projectPathTextBox.ReadOnly = true;
            this.projectPathTextBox.Size = new System.Drawing.Size(494, 23);
            this.projectPathTextBox.TabIndex = 3;
            // 
            // createButton
            // 
            this.createButton.Enabled = false;
            this.createButton.Location = new System.Drawing.Point(233, 198);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(158, 23);
            this.createButton.TabIndex = 6;
            this.createButton.Text = "Create Project";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(12, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Project Name";
            // 
            // projectNameTextBox
            // 
            this.projectNameTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.projectNameTextBox.Location = new System.Drawing.Point(12, 149);
            this.projectNameTextBox.Name = "projectNameTextBox";
            this.projectNameTextBox.Size = new System.Drawing.Size(494, 23);
            this.projectNameTextBox.TabIndex = 7;
            this.projectNameTextBox.TextChanged += new System.EventHandler(this.ProjectNameTextBox_TextChanged);
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 233);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.projectNameTextBox);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.projectPathButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.projectPathTextBox);
            this.Controls.Add(this.romFileBrowseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.romFileTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewProjectForm";
            this.Text = "New Project...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox romFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button romFileBrowseButton;
        private System.Windows.Forms.Button projectPathButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox projectPathTextBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox projectNameTextBox;
    }
}