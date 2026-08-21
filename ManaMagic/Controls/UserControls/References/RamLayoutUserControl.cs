using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ManaMagic.Core;
using ZwellTech;
using ZwellTech.Xml;

#nullable enable

namespace ManaMagic.Controls.UserControls.References
{
    public partial class RamLayoutUserControl : UserControl
    {
        private readonly RamMapType ramMapType = RamMapType.General;

        public RamLayoutUserControl()
        {
            InitializeComponent();
        }

        public RamLayoutUserControl(RamMapType ramMapType)
        {
            InitializeComponent();
            this.ramMapType = ramMapType;
        }

        private void RamLayoutUserControl_Load(object sender, EventArgs e)
        {
            this.LoadXml();
            this.typeColumn.Visible = false;
        }

        private void LoadXml()
        {
            using Stream? stream = RamLayoutUserControl.GetResourceStream(this.ramMapType);
            if (stream != null)
            {
                XmlDocument document = new XmlDocument();
                document.Load(stream);

                foreach (XmlNode node in document.GetElementsByTagName("Address"))
                {
                    string name = XmlHelper.ReadAttribute(node, "name");
                    string offset = $"${XmlHelper.ReadAttribute(node, "offset")}";
                    string size = XmlHelper.ReadAttribute(node, "size");
                    string type = XmlHelper.ReadAttribute(node, "type");
                    string description = string.Empty;

                    if (XmlHelper.TryGetElementByTagName(node, "Description", out XmlNode? descriptionNode))
                    {
                        description = XmlHelper.ReadAttribute(descriptionNode, XmlHelper.ValueAttributeName).Replace(",", Environment.NewLine);
                    }

                    this.dataGridView.Rows.Add(offset, name, size, type, description);
                }
            }
        }

        private static Stream? GetResourceStream(RamMapType ramMapType)
        {
            switch (ramMapType)
            {
                case RamMapType.General: return typeof(Constants).Assembly.GetManifestResourceStream("ManaMagic.Core.Documentation.RAMLayout.xml");
                case RamMapType.SpriteSlot: return typeof(Constants).Assembly.GetManifestResourceStream("ManaMagic.Core.Documentation.SpriteSlot.xml");
                case RamMapType.BossSpriteSlot: return typeof(Constants).Assembly.GetManifestResourceStream("ManaMagic.Core.Documentation.BossSpriteSlot.xml");
                default: return (Stream?)ThrowHelper.ThrowArgumentException("Unknown Ram Map", nameof(ramMapType));
            }
        }
    }

    public enum RamMapType
    {
        General,
        SpriteSlot,
        BossSpriteSlot,
    }
}
