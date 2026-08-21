using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Items;
using ZwellTech.Windows.Forms;
using static System.Windows.Forms.DataGridView;

#nullable enable

namespace ManaMagic.Controls.UserControls.Items.Shops
{
    public partial class ShopEditorUserControl : UserControl
    {
        private bool ignoreEvents = false;

        public ShopEditorUserControl()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.shopDataGridView, new object[] { true });

            this.ignoreEvents = true;
            this.shopTypeComboBox.DataSource = Enum.GetValues<ShopRingType>();
            this.shopTypeComboBox.SelectedItem = ShopRingType.Potos;

            this.itemColumn.DataSource = Enum.GetValues<ShopItem>();
            this.itemColumn.ValueType = typeof(ShopItem);
            this.ignoreEvents = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (int)ExtendedWindowStyles.WS_EX_COMPOSITED;
                return cp;
            }
        }

        private void SetShopTable(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                this.ignoreEvents = true;
                this.shopDataGridView.Rows.Clear();
                foreach (ShopItem item in ManaMagicContext.Current.Context.ItemContext.ShopTable[index])
                {
                    this.shopDataGridView.Rows.Add(item);
                }
                this.ignoreEvents = false;
            }
        }

        private void ShopEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetShopTable(0);
        }

        private void ShopTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.ignoreEvents)
            {
                this.SetShopTable(this.shopTypeComboBox.SelectedIndex);
            }
        }

        private void ShopDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ManaShop shop = ManaMagicContext.Current.Context.ItemContext.ShopTable[this.shopTypeComboBox.SelectedIndex];
                shop[e.RowIndex] = (ShopItem)this.shopDataGridView[this.itemColumn.Index, e.RowIndex].Value;
            }
        }

        private void ShopDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                if (dataGridView.IsCurrentCellDirty)
                {
                    dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            Point location = this.shopDataGridView.PointToClient(Control.MousePosition);
            HitTestInfo info = this.shopDataGridView.HitTest(location.X, location.Y);

            this.addItemToShopToolStripMenuItem.Enabled = this.shopDataGridView.Rows.Count <= Constants.Bank18.ShopMaxSize;
            this.removeItemFromShopToolStripMenuItem.Enabled = info.Type == DataGridViewHitTestType.Cell;
            this.removeItemFromShopToolStripMenuItem.Tag = info.RowIndex;
        }

        private void AddItemToShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManaShop shop = ManaMagicContext.Current.Context.ItemContext.ShopTable[this.shopTypeComboBox.SelectedIndex];
            shop.Add(ShopItem.Candy);
            this.shopDataGridView.Rows.Add(ShopItem.Candy);
        }

        private void RemoveItemFromShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManaShop shop = ManaMagicContext.Current.Context.ItemContext.ShopTable[this.shopTypeComboBox.SelectedIndex];
            shop.Remove((ShopItem)this.shopDataGridView[this.itemColumn.Index, (int)this.removeItemFromShopToolStripMenuItem.Tag].Value);
            this.shopDataGridView.Rows.RemoveAt((int)this.removeItemFromShopToolStripMenuItem.Tag);
        }
    }
}
