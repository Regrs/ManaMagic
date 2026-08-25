using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManaMagic.Core.Metadata;
using ManaMagic.Core.Sprites;

namespace ManaMagic.Controls.UserControls.Debug.Views
{
    public partial class DebugLootTableViewUserControl : UserControl
    {
        public DebugLootTableViewUserControl()
        {
            InitializeComponent();
        }

        private void LootTableUserControl_Load(object sender, EventArgs e)
        {
            this.debugDataGridView.Columns.Add("indexColumn", "Index");
            this.debugDataGridView.Columns.Add("nameColumn", "Name");
            this.debugDataGridView.Columns.Add("fleesColumn", "Flees");
            this.debugDataGridView.Columns.Add("alwaysDropsColumn", "Always Drops");
            this.debugDataGridView.Columns.Add("agilityDodgeThresholdColumn", "Agility Dodge Threshold");
            this.debugDataGridView.Columns.Add("sylphidDisarmLevelColumn", "Sylphid Disarm Level");

            this.debugDataGridView.Columns.Add("commonLootTypeColumn", "Common Loot Type");
            this.debugDataGridView.Columns.Add("commonLootDropRateColumn", "Common Drop Rate");
            this.debugDataGridView.Columns.Add("commonLootValueColumn", "Common Value");

            this.debugDataGridView.Columns.Add("rareLootTypeColumn", "Rare Loot Type");
            this.debugDataGridView.Columns.Add("rareLootDropRateColumn", "Rare Drop Rate");
            this.debugDataGridView.Columns.Add("rareLootValueColumn", "Rare Value");

            this.debugDataGridView.Columns["nameColumn"].Width = 250;
            //this.debugDataGridView.Columns["replaceTileIndexColumn"].Width = 125;

            byte index = 0;
            foreach (EnemyLootEntry lootEntry in ManaMagicContext.Current.Context.SpriteContext.LootTable)
            {
                double cDropRate = (lootEntry.DropRate / 64d);// * 1d;
                double rDropRate = (lootEntry.RareDropChance / 64d);// * 1d;
                this.debugDataGridView.Rows.Add(index.ToString("X2"),
                                                ManaMetadata.GetSpriteFriendlyName(index),
                                                lootEntry.Flees.ToString(),
                                                lootEntry.AlwaysDrops.ToString(),
                                                lootEntry.DodgeThreshold.ToString("X2"),
                                                lootEntry.DisarmLevel.ToString("X2"),
                                                lootEntry.CommonLoot.DropType.ToString(),
                                                //lootEntry.CommonLoot.DropRate.ToString("X2"),
                                                lootEntry.DropRatePercent.ToString("P2"),
                                                lootEntry.CommonLoot.ToString(),
                                                lootEntry.RareLoot.DropType.ToString(),
                                                //lootEntry.RareLoot.DropRate.ToString("X2"),
                                                lootEntry.RareDropChancePercent.ToString("P2"),
                                                lootEntry.RareLoot.ToString());
                index++;
            }
        }
    }
}
