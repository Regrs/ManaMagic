using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManaMagic.Core.Items;

namespace ManaMagic.Controls.UserControls.Debug.Views
{
    public partial class DebugWeaponDefinitionViewUserControl : UserControl
    {
        public DebugWeaponDefinitionViewUserControl()
        {
            InitializeComponent();
        }

        private void DebugWeaponDefinitionViewUserControl_Load(object sender, EventArgs e)
        {
            this.debugDataGridView.Columns.Add("indexColumn", "Index");
            this.debugDataGridView.Columns.Add("WeaponClassColumn", "WeaponClass");
            this.debugDataGridView.Columns.Add("StatModifiersColumn", "StatModifiers");
            this.debugDataGridView.Columns.Add("unknownStatIndexStatModifiersColumn", "Unknown Stat Index");
            this.debugDataGridView.Columns.Add("strengthStatIndexStatModifiersColumn", "Strength Modifier");
            this.debugDataGridView.Columns.Add("agilityStatIndexStatModifiersColumn", "Agility Modifier");
            this.debugDataGridView.Columns.Add("ConstitutionStatIndexStatModifiersColumn", "Constitution Modifier");
            this.debugDataGridView.Columns.Add("IntelligenceStatIndexStatModifiersColumn", "Intelligence Modifier");
            this.debugDataGridView.Columns.Add("WisdomStatIndexStatModifiersColumn", "Wisdom Modifier");
            this.debugDataGridView.Columns.Add("ProjectileTypeColumn", "ProjectileType");
            this.debugDataGridView.Columns.Add("PaletteIndexColumn", "PaletteIndex");
            this.debugDataGridView.Columns.Add("AffinityColumn", "Affinity");
            this.debugDataGridView.Columns.Add("CriticalChanceColumn", "CriticalChance");
            this.debugDataGridView.Columns.Add("AccuracyColumn", "Accuracy");
            this.debugDataGridView.Columns.Add("PowerColumn", "Power");
            this.debugDataGridView.Columns.Add("StatusEffectColumn", "StatusEffect");
            this.debugDataGridView.Columns.Add("InflictionRateColumn", "InflictionRate");

            this.debugDataGridView.Columns["indexColumn"].Width = 75;

            foreach (ManaWeaponDefinition entry in ManaMagicContext.Current.Context.ItemContext.WeaponDefinitionTable)
            {
                this.debugDataGridView.Rows.Add(entry.Index.ToString("X2"),
                                                entry.WeaponClass.ToString(),
                                                entry.StatModifiers.ToString("X4"),
                                                entry.UnknownStatIndex.ToString("X2"),
                                                entry.StrengthModifier.ToString(),
                                                entry.AgilityModifier.ToString(),
                                                entry.ConstitutionModifier.ToString(),
                                                entry.IntelligenceModifier.ToString(),
                                                entry.WisdomModifier.ToString(),
                                                entry.ProjectileType.ToString(),
                                                entry.PaletteIndex.ToString("X2"),
                                                entry.Affinity.ToString(),
                                                entry.CriticalChance.ToString("X2"),
                                                entry.Accuracy.ToString("X2"),
                                                entry.Power.ToString("X2"),
                                                entry.StatusEffects.ToString(),
                                                entry.InflictionRate.ToString("X2"));
            }
        }
    }
}
