using System;
using System.Windows.Forms;
using ManaMagic.Core.Help;

#nullable enable

namespace ManaMagic.Controls.UserControls.Help
{
    public partial class HelpUserControl : UserControl
    {
        private readonly ManaHelpTopic topic;

        public HelpUserControl()
        {
            InitializeComponent();
        }

        public HelpUserControl(ManaHelpTopic topic)
        {
            InitializeComponent();
            this.topic = topic;
        }

        private void HelpUserControl_Load(object sender, EventArgs e)
        {
            this.helpRichTextBox.Rtf = ManaHelp.GetHelpFile(this.topic);
        }
    }
}