
namespace ManaMagic.Controls.UserControls.Help
{
    partial class HelpUserControl
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
            this.helpRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // helpRichTextBox
            // 
            this.helpRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.helpRichTextBox.Name = "helpRichTextBox";
            this.helpRichTextBox.ReadOnly = true;
            this.helpRichTextBox.Size = new System.Drawing.Size(573, 473);
            this.helpRichTextBox.TabIndex = 0;
            this.helpRichTextBox.Text = "";
            // 
            // HelpUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpRichTextBox);
            this.Name = "HelpUserControl";
            this.Size = new System.Drawing.Size(573, 473);
            this.Load += new System.EventHandler(this.HelpUserControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox helpRichTextBox;
    }
}
