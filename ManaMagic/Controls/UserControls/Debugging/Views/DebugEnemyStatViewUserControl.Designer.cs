
namespace ManaMagic.Controls.UserControls.Debug.Views
{
    partial class DebugEnemyStatViewUserControl
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
            this.debugDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.debugDataGridView)).BeginInit();
            this.SuspendLayout();
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
            this.debugDataGridView.Size = new System.Drawing.Size(591, 452);
            this.debugDataGridView.TabIndex = 2;
            // 
            // DebugEnemyStatViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.debugDataGridView);
            this.Name = "DebugEnemyStatViewUserControl";
            this.Size = new System.Drawing.Size(591, 452);
            this.Load += new System.EventHandler(this.DebugEnemyStatViewUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.debugDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView debugDataGridView;
    }
}
