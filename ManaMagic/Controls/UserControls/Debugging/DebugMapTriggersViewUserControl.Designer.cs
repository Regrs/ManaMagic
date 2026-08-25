
namespace ManaMagic.Controls.UserControls.Debug
{
    partial class DebugMapTriggersViewUserControl
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
            this.noTriggersListBox = new System.Windows.Forms.ListBox();
            this.debugDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.debugDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // noTriggersListBox
            // 
            this.noTriggersListBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.noTriggersListBox.FormattingEnabled = true;
            this.noTriggersListBox.ItemHeight = 15;
            this.noTriggersListBox.Location = new System.Drawing.Point(730, 0);
            this.noTriggersListBox.Name = "noTriggersListBox";
            this.noTriggersListBox.Size = new System.Drawing.Size(358, 741);
            this.noTriggersListBox.TabIndex = 0;
            // 
            // debugDataGridView
            // 
            this.debugDataGridView.AllowUserToAddRows = false;
            this.debugDataGridView.AllowUserToDeleteRows = false;
            this.debugDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.debugDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugDataGridView.Location = new System.Drawing.Point(0, 0);
            this.debugDataGridView.Name = "debugDataGridView";
            this.debugDataGridView.ReadOnly = true;
            this.debugDataGridView.RowTemplate.Height = 25;
            this.debugDataGridView.Size = new System.Drawing.Size(730, 741);
            this.debugDataGridView.TabIndex = 1;
            // 
            // DebugMapTriggersViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.debugDataGridView);
            this.Controls.Add(this.noTriggersListBox);
            this.Name = "DebugMapTriggersViewUserControl";
            this.Size = new System.Drawing.Size(1088, 741);
            this.Load += new System.EventHandler(this.DebugMapTriggersViewUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.debugDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox noTriggersListBox;
        private System.Windows.Forms.DataGridView debugDataGridView;
    }
}
