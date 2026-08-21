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
    public partial class DebugEnemyStatViewUserControl : UserControl
    {
        public DebugEnemyStatViewUserControl()
        {
            InitializeComponent();
        }

        private void DebugEnemyStatViewUserControl_Load(object sender, EventArgs e)
        {
            this.debugDataGridView.Columns.Add("indexColumn", "Index");
            this.debugDataGridView.Columns.Add("nameColumn", "Name");

            this.debugDataGridView.Columns.Add("levelColumn", "Level");
            this.debugDataGridView.Columns.Add("maxHPColumn", "Max HP");
            this.debugDataGridView.Columns.Add("maxMPColumn", "Max MP");
            this.debugDataGridView.Columns.Add("strengthColumn", "Strength");
            this.debugDataGridView.Columns.Add("agilityColumn", "Agility");
            this.debugDataGridView.Columns.Add("intelligenceColumn", "Intelligence");
            this.debugDataGridView.Columns.Add("wisdomColumn", "Wisdom");
            this.debugDataGridView.Columns.Add("evasionColumn", "Evasion");
            this.debugDataGridView.Columns.Add("physicalDefenseColumn", "Physical Defense");
            this.debugDataGridView.Columns.Add("magicEvasionColumn", "Magic Evasion");
            this.debugDataGridView.Columns.Add("magicDefenseColumn", "Magic Defense");
            this.debugDataGridView.Columns.Add("monsterTypeColumn", "Monster Type");
            this.debugDataGridView.Columns.Add("elementColumn", "Element");
            this.debugDataGridView.Columns.Add("expAwardColumn", "Exp Award");
            this.debugDataGridView.Columns.Add("blackMagicPowerColumn", "Black Magic Power");
            this.debugDataGridView.Columns.Add("whiteMagicPowerColumn", "White Magic Power");
            this.debugDataGridView.Columns.Add("immunitiesColumn", "Immunities");
            this.debugDataGridView.Columns.Add("unknownColumn", "Unknown");
            this.debugDataGridView.Columns.Add("weapon01Column", "Weapon 01");
            this.debugDataGridView.Columns.Add("weapon02Column", "Weapon 02");
            this.debugDataGridView.Columns.Add("deathStyleColumn", "Death Style");
            this.debugDataGridView.Columns.Add("weaponLevelColumn", "Weapon Level");
            this.debugDataGridView.Columns.Add("magicLevelColumn", "Magic Level");
            this.debugDataGridView.Columns.Add("goldAwardColumn", "Gold Award");

            this.debugDataGridView.Columns["indexColumn"].Width = 75;
            this.debugDataGridView.Columns["nameColumn"].Width = 150;

            foreach (EnemyStatEntry entry in ManaMagicContext.Current.Context.SpriteContext.EnemyStatisticsTable)
            {
                this.debugDataGridView.Rows.Add(entry.Index.ToString("X2"),
                                                ManaMetadata.GetSpriteFriendlyName(entry.Index),
                                                entry.Level.ToString(),
                                                entry.HitPoints.ToString(),
                                                entry.ManaPoints.ToString(),
                                                entry.Strength.ToString(),
                                                entry.Agility.ToString(),
                                                entry.Intelligence.ToString(),
                                                entry.Wisdom.ToString(),
                                                entry.Evasion.ToString(),
                                                entry.Defense.ToString(),
                                                entry.MagicEvasion.ToString(),
                                                entry.MagicDefense.ToString(),
                                                entry.MonsterType.ToString(),
                                                entry.Element.ToString(),
                                                entry.ExperienceAward.ToString(),
                                                entry.BlackMagicPower.ToString(),
                                                entry.WhiteMagicPower.ToString(),
                                                entry.Immunities.ToString(),
                                                entry.Unused.ToString(),
                                                entry.MeleeWeapon.ToString(),
                                                entry.RangedWeapon.ToString(),
                                                entry.DeathStyle.ToString(),
                                                entry.WeaponLevel.ToString(),
                                                entry.MagicLevel.ToString(),
                                                entry.GoldAward.ToString());
            }
        }
    }
}
