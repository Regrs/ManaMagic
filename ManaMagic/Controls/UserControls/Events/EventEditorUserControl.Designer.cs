
namespace ManaMagic.Controls.UserControls.Events
{
    partial class EventEditorUserControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventEditorUserControl));
            this.eventViewerListBox = new System.Windows.Forms.ListBox();
            this.eventViewerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventViewerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.eventOpCodeTreeView = new ZwellTech.Windows.Forms.ZwellTreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.eventNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.opCodeDescriptionLabel = new System.Windows.Forms.Label();
            this.opCodeNameLabel = new System.Windows.Forms.Label();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventViewerContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventViewerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventViewerListBox
            // 
            this.eventViewerListBox.AllowDrop = true;
            this.eventViewerListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventViewerListBox.BackColor = System.Drawing.Color.Black;
            this.eventViewerListBox.ContextMenuStrip = this.eventViewerContextMenuStrip;
            this.eventViewerListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.eventViewerListBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.eventViewerListBox.ForeColor = System.Drawing.Color.White;
            this.eventViewerListBox.FormattingEnabled = true;
            this.eventViewerListBox.ItemHeight = 20;
            this.eventViewerListBox.Location = new System.Drawing.Point(3, 56);
            this.eventViewerListBox.Name = "eventViewerListBox";
            this.eventViewerListBox.Size = new System.Drawing.Size(1015, 720);
            this.eventViewerListBox.TabIndex = 0;
            this.eventViewerListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.EventViewerListBox_DrawItem);
            this.eventViewerListBox.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.EventViewerListBox_MeasureItem);
            this.eventViewerListBox.SelectedIndexChanged += new System.EventHandler(this.EventViewerListBox_SelectedIndexChanged);
            this.eventViewerListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.EventViewerListBox_DragDrop);
            this.eventViewerListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.EventViewerListBox_DragEnter);
            this.eventViewerListBox.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.EventViewerListBox_QueryContinueDrag);
            this.eventViewerListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EventViewerListBox_MouseMove);
            // 
            // eventViewerContextMenuStrip
            // 
            this.eventViewerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewEventToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem});
            this.eventViewerContextMenuStrip.Name = "eventViewerContextMenuStrip";
            this.eventViewerContextMenuStrip.Size = new System.Drawing.Size(181, 76);
            this.eventViewerContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.EventViewerContextMenuStrip_Opening);
            // 
            // viewEventToolStripMenuItem
            // 
            this.viewEventToolStripMenuItem.Name = "viewEventToolStripMenuItem";
            this.viewEventToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewEventToolStripMenuItem.Text = "View Event";
            this.viewEventToolStripMenuItem.Click += new System.EventHandler(this.ViewEventToolStripMenuItem_Click);
            // 
            // eventViewerNumericUpDown
            // 
            this.eventViewerNumericUpDown.Hexadecimal = true;
            this.eventViewerNumericUpDown.Location = new System.Drawing.Point(3, 27);
            this.eventViewerNumericUpDown.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.eventViewerNumericUpDown.Name = "eventViewerNumericUpDown";
            this.eventViewerNumericUpDown.Size = new System.Drawing.Size(142, 23);
            this.eventViewerNumericUpDown.TabIndex = 1;
            this.eventViewerNumericUpDown.ValueChanged += new System.EventHandler(this.EventViewerNumericUpDown_ValueChanged);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(0, 44);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(363, 328);
            this.propertyGrid.TabIndex = 1;
            // 
            // eventOpCodeTreeView
            // 
            this.eventOpCodeTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventOpCodeTreeView.Location = new System.Drawing.Point(0, 0);
            this.eventOpCodeTreeView.Name = "eventOpCodeTreeView";
            this.eventOpCodeTreeView.ShowNodeToolTips = true;
            this.eventOpCodeTreeView.Size = new System.Drawing.Size(363, 321);
            this.eventOpCodeTreeView.TabIndex = 0;
            this.eventOpCodeTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.EventOpCodeTreeView_ItemDrag);
            this.eventOpCodeTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.EventOpCodeTreeView_NodeMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.eventNameLabel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.eventViewerListBox);
            this.splitContainer1.Panel1.Controls.Add(this.eventViewerNumericUpDown);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1396, 777);
            this.splitContainer1.SplitterDistance = 1025;
            this.splitContainer1.TabIndex = 3;
            // 
            // eventNameLabel
            // 
            this.eventNameLabel.AutoSize = true;
            this.eventNameLabel.Location = new System.Drawing.Point(46, 9);
            this.eventNameLabel.Name = "eventNameLabel";
            this.eventNameLabel.Size = new System.Drawing.Size(88, 15);
            this.eventNameLabel.TabIndex = 3;
            this.eventNameLabel.Text = "[EVENT_NAME]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.eventOpCodeTreeView);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            this.splitContainer2.Panel1.SizeChanged += new System.EventHandler(this.SplitContainer2_Panel1_SizeChanged);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.moveDownButton);
            this.splitContainer2.Panel2.Controls.Add(this.moveUpButton);
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer2.Size = new System.Drawing.Size(367, 777);
            this.splitContainer2.SplitterDistance = 397;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.opCodeDescriptionLabel);
            this.panel1.Controls.Add(this.opCodeNameLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 324);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(363, 69);
            this.panel1.TabIndex = 1;
            // 
            // opCodeDescriptionLabel
            // 
            this.opCodeDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.opCodeDescriptionLabel.Location = new System.Drawing.Point(0, 22);
            this.opCodeDescriptionLabel.Name = "opCodeDescriptionLabel";
            this.opCodeDescriptionLabel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.opCodeDescriptionLabel.Size = new System.Drawing.Size(361, 45);
            this.opCodeDescriptionLabel.TabIndex = 4;
            this.opCodeDescriptionLabel.Text = "[OP_CODE_DESCRIPTION]";
            // 
            // opCodeNameLabel
            // 
            this.opCodeNameLabel.AutoSize = true;
            this.opCodeNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.opCodeNameLabel.Location = new System.Drawing.Point(3, 4);
            this.opCodeNameLabel.Name = "opCodeNameLabel";
            this.opCodeNameLabel.Size = new System.Drawing.Size(106, 15);
            this.opCodeNameLabel.TabIndex = 3;
            this.opCodeNameLabel.Text = "[OP_CODE_NAME]";
            // 
            // moveDownButton
            // 
            this.moveDownButton.Enabled = false;
            this.moveDownButton.Image = ((System.Drawing.Image)(resources.GetObject("moveDownButton.Image")));
            this.moveDownButton.Location = new System.Drawing.Point(44, 3);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(35, 35);
            this.moveDownButton.TabIndex = 3;
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Enabled = false;
            this.moveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("moveUpButton.Image")));
            this.moveUpButton.Location = new System.Drawing.Point(3, 3);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(35, 35);
            this.moveUpButton.TabIndex = 2;
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // EventEditorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "EventEditorUserControl";
            this.Size = new System.Drawing.Size(1396, 777);
            this.Load += new System.EventHandler(this.EventViewerUserControl_Load);
            this.eventViewerContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventViewerNumericUpDown)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox eventViewerListBox;
        private System.Windows.Forms.NumericUpDown eventViewerNumericUpDown;
        private System.Windows.Forms.ContextMenuStrip eventViewerContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem viewEventToolStripMenuItem;
        private ZwellTech.Windows.Forms.ZwellTreeView eventOpCodeTreeView;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label eventNameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label opCodeNameLabel;
        private System.Windows.Forms.Label opCodeDescriptionLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
