using System;
using System.Windows.Forms;

#nullable enable

namespace ManaMagic
{
    public partial class NewProjectForm : Form
    {
        public string RomPath { get; private set; } = string.Empty;
        public string ProjectPath { get; private set; } = string.Empty;
        public string FullProjectPath { get { return $"{this.ProjectPath}//{this.projectNameTextBox.Text}.mmagproj"; } }

        public NewProjectForm()
        {
            InitializeComponent();
        }

        private void SetCreateButtonEnabledState()
        {
            this.createButton.Enabled = !string.IsNullOrWhiteSpace(this.RomPath) && !string.IsNullOrWhiteSpace(this.ProjectPath) && !string.IsNullOrWhiteSpace(this.projectNameTextBox.Text);
        }

        private void RomFileBrowseButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select The Secret Of Mana ROM File To Edit...";
            ofd.Filter = "Headerless ROMs (*.sfc)|*.sfc|Headered ROMs (*.smc)|*.smc|All ROMs (*.sfc;*.smc)|*.sfc;*.smc";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.RomPath = ofd.FileName;
                this.romFileTextBox.Text = this.RomPath;
                this.SetCreateButtonEnabledState();
            }
        }

        private void ProjectPathButton_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.ProjectPath = fbd.SelectedPath;
                this.projectPathTextBox.Text = this.ProjectPath;
                this.SetCreateButtonEnabledState();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ProjectNameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.SetCreateButtonEnabledState();
        }
    }
}