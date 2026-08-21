
namespace ManaMagic.Controls.UserControls.References
{
    partial class ReferenceSearchUserControl
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
            this.searchTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.searchEventButton = new System.Windows.Forms.Button();
            this.searchEventNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.eventReferencesListView = new System.Windows.Forms.ListView();
            this.ReferenceType = new System.Windows.Forms.ColumnHeader();
            this.ReferenceIndex = new System.Windows.Forms.ColumnHeader();
            this.AdditionalInformation = new System.Windows.Forms.ColumnHeader();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.eventFlagReferencesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.eventFlagSearchButton = new System.Windows.Forms.Button();
            this.eventFlagSearchComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.eventOpCodeReferencesListView = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.searchEventOpCodesButton = new System.Windows.Forms.Button();
            this.eventOpCodeSearchComboBox = new System.Windows.Forms.ComboBox();
            this.searchTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchEventNumericUpDown)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTabControl
            // 
            this.searchTabControl.Controls.Add(this.tabPage1);
            this.searchTabControl.Controls.Add(this.tabPage2);
            this.searchTabControl.Controls.Add(this.tabPage3);
            this.searchTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchTabControl.Location = new System.Drawing.Point(0, 0);
            this.searchTabControl.Name = "searchTabControl";
            this.searchTabControl.SelectedIndex = 0;
            this.searchTabControl.Size = new System.Drawing.Size(1212, 753);
            this.searchTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.searchEventButton);
            this.tabPage1.Controls.Add(this.searchEventNumericUpDown);
            this.tabPage1.Controls.Add(this.eventReferencesListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1204, 725);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Event References";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // searchEventButton
            // 
            this.searchEventButton.Location = new System.Drawing.Point(132, 18);
            this.searchEventButton.Name = "searchEventButton";
            this.searchEventButton.Size = new System.Drawing.Size(75, 23);
            this.searchEventButton.TabIndex = 2;
            this.searchEventButton.Text = "Search";
            this.searchEventButton.UseVisualStyleBackColor = true;
            this.searchEventButton.Click += new System.EventHandler(this.SearchEventButton_Click);
            // 
            // searchEventNumericUpDown
            // 
            this.searchEventNumericUpDown.Hexadecimal = true;
            this.searchEventNumericUpDown.Location = new System.Drawing.Point(6, 18);
            this.searchEventNumericUpDown.Name = "searchEventNumericUpDown";
            this.searchEventNumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.searchEventNumericUpDown.TabIndex = 1;
            // 
            // eventReferencesListView
            // 
            this.eventReferencesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventReferencesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ReferenceType,
            this.ReferenceIndex,
            this.AdditionalInformation});
            this.eventReferencesListView.FullRowSelect = true;
            this.eventReferencesListView.HideSelection = false;
            this.eventReferencesListView.Location = new System.Drawing.Point(6, 47);
            this.eventReferencesListView.Name = "eventReferencesListView";
            this.eventReferencesListView.Size = new System.Drawing.Size(1195, 672);
            this.eventReferencesListView.TabIndex = 0;
            this.eventReferencesListView.UseCompatibleStateImageBehavior = false;
            this.eventReferencesListView.View = System.Windows.Forms.View.Details;
            this.eventReferencesListView.DoubleClick += new System.EventHandler(this.ReferencesListView_DoubleClick);
            // 
            // ReferenceType
            // 
            this.ReferenceType.Text = "Reference Type";
            this.ReferenceType.Width = 125;
            // 
            // ReferenceIndex
            // 
            this.ReferenceIndex.Text = "Reference Index";
            this.ReferenceIndex.Width = 200;
            // 
            // AdditionalInformation
            // 
            this.AdditionalInformation.Text = "Additional Information";
            this.AdditionalInformation.Width = 800;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.eventFlagReferencesListView);
            this.tabPage2.Controls.Add(this.eventFlagSearchButton);
            this.tabPage2.Controls.Add(this.eventFlagSearchComboBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1204, 725);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Event Flag References";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // eventFlagReferencesListView
            // 
            this.eventFlagReferencesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventFlagReferencesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.eventFlagReferencesListView.FullRowSelect = true;
            this.eventFlagReferencesListView.HideSelection = false;
            this.eventFlagReferencesListView.Location = new System.Drawing.Point(6, 47);
            this.eventFlagReferencesListView.Name = "eventFlagReferencesListView";
            this.eventFlagReferencesListView.Size = new System.Drawing.Size(1195, 672);
            this.eventFlagReferencesListView.TabIndex = 4;
            this.eventFlagReferencesListView.UseCompatibleStateImageBehavior = false;
            this.eventFlagReferencesListView.View = System.Windows.Forms.View.Details;
            this.eventFlagReferencesListView.DoubleClick += new System.EventHandler(this.ReferencesListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Reference Type";
            this.columnHeader1.Width = 125;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Reference Index";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Additional Information";
            this.columnHeader3.Width = 800;
            // 
            // eventFlagSearchButton
            // 
            this.eventFlagSearchButton.Location = new System.Drawing.Point(258, 18);
            this.eventFlagSearchButton.Name = "eventFlagSearchButton";
            this.eventFlagSearchButton.Size = new System.Drawing.Size(75, 23);
            this.eventFlagSearchButton.TabIndex = 3;
            this.eventFlagSearchButton.Text = "Search";
            this.eventFlagSearchButton.UseVisualStyleBackColor = true;
            this.eventFlagSearchButton.Click += new System.EventHandler(this.EventFlagSearchButton_Click);
            // 
            // eventFlagSearchComboBox
            // 
            this.eventFlagSearchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eventFlagSearchComboBox.FormattingEnabled = true;
            this.eventFlagSearchComboBox.Location = new System.Drawing.Point(6, 18);
            this.eventFlagSearchComboBox.Name = "eventFlagSearchComboBox";
            this.eventFlagSearchComboBox.Size = new System.Drawing.Size(246, 23);
            this.eventFlagSearchComboBox.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.eventOpCodeReferencesListView);
            this.tabPage3.Controls.Add(this.searchEventOpCodesButton);
            this.tabPage3.Controls.Add(this.eventOpCodeSearchComboBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1204, 725);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Event Op-Code References";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // eventOpCodeReferencesListView
            // 
            this.eventOpCodeReferencesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventOpCodeReferencesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.eventOpCodeReferencesListView.FullRowSelect = true;
            this.eventOpCodeReferencesListView.HideSelection = false;
            this.eventOpCodeReferencesListView.Location = new System.Drawing.Point(5, 41);
            this.eventOpCodeReferencesListView.Name = "eventOpCodeReferencesListView";
            this.eventOpCodeReferencesListView.Size = new System.Drawing.Size(1195, 672);
            this.eventOpCodeReferencesListView.TabIndex = 7;
            this.eventOpCodeReferencesListView.UseCompatibleStateImageBehavior = false;
            this.eventOpCodeReferencesListView.View = System.Windows.Forms.View.Details;
            this.eventOpCodeReferencesListView.DoubleClick += new System.EventHandler(this.ReferencesListView_DoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Reference Type";
            this.columnHeader4.Width = 125;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Reference Index";
            this.columnHeader5.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Additional Information";
            this.columnHeader6.Width = 800;
            // 
            // searchEventOpCodesButton
            // 
            this.searchEventOpCodesButton.Location = new System.Drawing.Point(257, 12);
            this.searchEventOpCodesButton.Name = "searchEventOpCodesButton";
            this.searchEventOpCodesButton.Size = new System.Drawing.Size(75, 23);
            this.searchEventOpCodesButton.TabIndex = 6;
            this.searchEventOpCodesButton.Text = "Search";
            this.searchEventOpCodesButton.UseVisualStyleBackColor = true;
            this.searchEventOpCodesButton.Click += new System.EventHandler(this.searchEventOpCodesButton_Click);
            // 
            // eventOpCodeSearchComboBox
            // 
            this.eventOpCodeSearchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eventOpCodeSearchComboBox.FormattingEnabled = true;
            this.eventOpCodeSearchComboBox.Location = new System.Drawing.Point(5, 12);
            this.eventOpCodeSearchComboBox.Name = "eventOpCodeSearchComboBox";
            this.eventOpCodeSearchComboBox.Size = new System.Drawing.Size(246, 23);
            this.eventOpCodeSearchComboBox.TabIndex = 5;
            // 
            // ReferenceSearchUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchTabControl);
            this.Name = "ReferenceSearchUserControl";
            this.Size = new System.Drawing.Size(1212, 753);
            this.searchTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchEventNumericUpDown)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl searchTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView eventReferencesListView;
        private System.Windows.Forms.ColumnHeader ReferenceType;
        private System.Windows.Forms.ColumnHeader ReferenceIndex;
        private System.Windows.Forms.Button searchEventButton;
        private System.Windows.Forms.NumericUpDown searchEventNumericUpDown;
        private System.Windows.Forms.ColumnHeader AdditionalInformation;
        private System.Windows.Forms.ListView eventFlagReferencesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button eventFlagSearchButton;
        private System.Windows.Forms.ComboBox eventFlagSearchComboBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView eventOpCodeReferencesListView;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button searchEventOpCodesButton;
        private System.Windows.Forms.ComboBox eventOpCodeSearchComboBox;
    }
}
