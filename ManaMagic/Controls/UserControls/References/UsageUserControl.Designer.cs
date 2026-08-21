
namespace ManaMagic.Controls.UserControls.References
{
    partial class UsageUserControl
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
            this.usageListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // usageListView
            // 
            this.usageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.usageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usageListView.FullRowSelect = true;
            this.usageListView.HideSelection = false;
            this.usageListView.Location = new System.Drawing.Point(0, 0);
            this.usageListView.Name = "usageListView";
            this.usageListView.Size = new System.Drawing.Size(1037, 695);
            this.usageListView.TabIndex = 0;
            this.usageListView.UseCompatibleStateImageBehavior = false;
            this.usageListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 450;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Usage";
            // 
            // UsageUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.usageListView);
            this.Name = "UsageUserControl";
            this.Size = new System.Drawing.Size(1037, 695);
            this.Load += new System.EventHandler(this.UsageUserControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView usageListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
