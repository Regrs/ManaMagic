using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ManaMagic.Core.Items;
using ZwellTech.SuperNintendo;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.Items.Shops
{
    public partial class ShopPriceEditorUserControl : UserControl
    {
        private bool ignoreEvents = false;
        private const int ValueColumnIndex = 1;
        private const int CanSellColumnIndex = 2;

        public ShopPriceEditorUserControl()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.consumableDataGridView, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.weaponUpgradesDataGridView, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.helmetsDataGridView, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.armorDataGridView, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.accessoriesDataGridView, new object[] { true });
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

        private void LoadPriceTables()
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                this.ignoreEvents = true;
                LoadSellItemTable(this.consumableDataGridView, ManaMagicContext.Current.Context.ItemContext.ConsumableItemPriceTable, Enum.GetValues<ConsumableItemType>());
                LoadSellItemTable(this.helmetsDataGridView, ManaMagicContext.Current.Context.ItemContext.HelmetPriceTable, Enum.GetValues<HelmetType>());
                LoadSellItemTable(this.armorDataGridView, ManaMagicContext.Current.Context.ItemContext.ArmorPriceTable, Enum.GetValues<ArmorType>());
                LoadSellItemTable(this.accessoriesDataGridView, ManaMagicContext.Current.Context.ItemContext.AccessoryPriceTable, Enum.GetValues<AccessoryType>());

                // Don't edit the blocking marker.
                for (int i = 0; i < ManaMagicContext.Current.Context.ItemContext.WeaponUpgradePriceTable.RowCount - 1; i++)
                {
                    UShortValue valueContainer = ManaMagicContext.Current.Context.ItemContext.WeaponUpgradePriceTable[i];
                    this.weaponUpgradesDataGridView.Rows.Add($"Level {i + 1}", valueContainer.Value);
                }
                this.ignoreEvents = false;
            }

            static void LoadSellItemTable<T>(DataGridView dataGridView, DataTable<UShortValue> priceTable, T[] indexNames)
            {
                for (int i = 0; i < priceTable.RowCount; i++)
                {
                    UShortValue valueContainer = priceTable[i];
                    bool canSell = valueContainer.Value != ItemContext.CannotSellItem;
                    ushort value = canSell ? valueContainer.Value : (ushort)0;

                    int rowIndex = dataGridView.Rows.Add(indexNames[i], value, canSell);
                    dataGridView[ShopPriceEditorUserControl.ValueColumnIndex, rowIndex].ReadOnly = !canSell;
                }
            }
        }

        private void ShopPriceEditorUserControl_Load(object sender, EventArgs e)
        {
            this.LoadPriceTables();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && !this.ignoreEvents)
            {
                this.ignoreEvents = true;
                DataGridView? dataGridView = sender as DataGridView;
                DataTable<UShortValue>? dataTable = null;
                bool sellable = true;

                if (sender == this.consumableDataGridView) { dataTable = ManaMagicContext.Current.Context.ItemContext.ConsumableItemPriceTable; }
                else if (sender == this.helmetsDataGridView) { dataTable = ManaMagicContext.Current.Context.ItemContext.HelmetPriceTable; }
                else if (sender == this.armorDataGridView) { dataTable = ManaMagicContext.Current.Context.ItemContext.ArmorPriceTable; }
                else if (sender == this.accessoriesDataGridView) { dataTable = ManaMagicContext.Current.Context.ItemContext.AccessoryPriceTable; }
                else if (sender == this.weaponUpgradesDataGridView) { dataTable = ManaMagicContext.Current.Context.ItemContext.WeaponUpgradePriceTable; sellable = false; }

                if (dataGridView != null && dataTable != null)
                {
                    ushort value = Convert.ToUInt16(dataGridView[ShopPriceEditorUserControl.ValueColumnIndex, e.RowIndex].Value);
                    if (!sellable)
                    {
                        dataTable[e.RowIndex].Value = value;
                        this.ignoreEvents = false;
                        return;
                    }

                    bool canSell = Convert.ToBoolean(dataGridView[ShopPriceEditorUserControl.CanSellColumnIndex, e.RowIndex].Value);
                    if (!canSell)
                    {
                        dataGridView[ShopPriceEditorUserControl.ValueColumnIndex, e.RowIndex].Value = 0;
                        dataGridView[ShopPriceEditorUserControl.ValueColumnIndex, e.RowIndex].ReadOnly = true;
                        dataTable[e.RowIndex].Value = ItemContext.CannotSellItem;
                        this.ignoreEvents = false;
                        return;
                    }
                    dataGridView[ShopPriceEditorUserControl.ValueColumnIndex, e.RowIndex].ReadOnly = false;
                    dataTable[e.RowIndex].Value = value;
                }
                else { Debugger.Break(); }
                this.ignoreEvents = false;
            }
        }

        private void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                if (dataGridView.IsCurrentCellDirty && dataGridView.CurrentCell is DataGridViewCheckBoxCell)
                {
                    dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }
    }
}
