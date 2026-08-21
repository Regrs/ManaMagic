using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;

#nullable enable

namespace ManaMagic.Controls.UserControls.References
{
    public partial class UsageUserControl : UserControl
    {
        private readonly UsageCounter usageCounter = UsageCounter.None;

        public UsageUserControl()
        {
            InitializeComponent();
        }

        public UsageUserControl(UsageCounter usageCounter)
        {
            InitializeComponent();
            this.usageCounter = usageCounter;
        }

        private void LoadUsageCounters()
        {
            if (this.usageCounter == UsageCounter.MapCollision)
            {
                foreach (KeyValuePair<MapCollisionType, int> kvp in ManaMagicContext.Current.Context.ReferenceCounter.MapCollisionTypeUsageCount)
                {
                    ListViewItem item = new ListViewItem(kvp.Key.ToString());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, kvp.Value.ToString()));
                    this.usageListView.Items.Add(item);
                }
            }
            else if (this.usageCounter == UsageCounter.MapPalette)
            {
                foreach (KeyValuePair<byte, int> kvp in ManaMagicContext.Current.Context.ReferenceCounter.MapPaletteUsageCount)
                {
                    ListViewItem item = new ListViewItem(kvp.Key.ToString("X2"));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, kvp.Value.ToString()));
                    this.usageListView.Items.Add(item);
                }
            }
            else if (this.usageCounter == UsageCounter.Event)
            {
                foreach (KeyValuePair<ushort, int> kvp in ManaMagicContext.Current.Context.ReferenceCounter.EventUsageCount)
                {
                    ListViewItem item = new ListViewItem(ManaMetadata.GetEventFriendlyName(kvp.Key));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, kvp.Value.ToString()));
                    this.usageListView.Items.Add(item);
                }
            }
            else if (this.usageCounter == UsageCounter.MapPiece)
            {
                foreach (KeyValuePair<ushort, int> kvp in ManaMagicContext.Current.Context.ReferenceCounter.MapPieceUsageCount)
                {
                    ListViewItem item = new ListViewItem(ManaMetadata.GetMapPieceFriendlyName(kvp.Key));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, kvp.Value.ToString()));
                    this.usageListView.Items.Add(item);
                }
            }
            else if (this.usageCounter == UsageCounter.Sprite)
            {
                foreach (KeyValuePair<byte, int> kvp in ManaMagicContext.Current.Context.ReferenceCounter.SpriteUsageCount)
                {
                    ListViewItem item = new ListViewItem(ManaMetadata.GetSpriteFriendlyName(kvp.Key));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, kvp.Value.ToString()));
                    this.usageListView.Items.Add(item);
                }
            }
        }

        private void UsageUserControl_Load(object sender, EventArgs e)
        {
            this.LoadUsageCounters();
        }
    }

    public enum UsageCounter
    {
        None,
        MapPiece,
        MapCollision,
        MapPalette,
        Sprite,
        Event,
    }
}
