using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Items;
using ManaMagic.Core.Sprites;

#nullable enable

namespace ManaMagic.Controls.UserControls.Sprites
{
    public partial class EnemyEditorUserControl : UserControl
    {
        private readonly IReadOnlyDictionary<string, Action<ManaEnemy>> EventHandlers;
        private ManaEnemy? enemy = null;
        private bool ignoreEvents = false;

        public EnemyEditorUserControl()
        {
            InitializeComponent();

            this.nameTextBox.MaxLength = Constants.TextMaxLength;
            this.monsterTypeComboBox.DataSource = Enum.GetValues<MonsterType>();
            this.monsterTypeComboBox.SelectedItem = MonsterType.None;

            this.elementComboBox.DataSource = Enum.GetValues<ElementalType>();
            this.elementComboBox.SelectedItem = ElementalType.None;

            this.deathStyleComboBox.DataSource = Enum.GetValues<DeathStyle>();
            this.deathStyleComboBox.SelectedItem = DeathStyle.Splat;

            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.immunitiesCheckedListBox.Items.Add(statusEffect);
                }
            }

            this.commonDropTypeComboBox.DataSource = Enum.GetValues<LootDropType>();
            this.commonDropTypeComboBox.SelectedItem = LootDropType.Gold;

            this.rareDropTypeComboBox.DataSource = Enum.GetValues<LootDropType>();
            this.rareDropTypeComboBox.SelectedItem = LootDropType.Gold;

            this.EventHandlers = this.LoadEventHandlers();
        }

        private void SetEnemy(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaEnemy enemy = ManaMagicContext.Current.Context.SpriteContext.Enemies[index];
                this.SetEnemy(enemy);
            }
        }

        private void SetEnemy(ManaEnemy enemy)
        {
            this.nameChangeTimer.Stop();
            this.ignoreEvents = true;

            this.enemy = enemy;
            this.nameTextBox.Text = ManaUtil.GetNameFromEvent(enemy.Name, out bool canEditName);
            this.nameTextBox.Enabled = canEditName;

            this.levelNumericUpDown.Value = enemy.Statistics.Level;
            this.monsterTypeComboBox.SelectedItem = enemy.Statistics.MonsterType;
            this.elementComboBox.SelectedItem = enemy.Statistics.Element;
            this.expAwardNumericUpDown.Value = enemy.Statistics.ExperienceAward;
            this.goldAwardNumericUpDown.Value = enemy.Statistics.GoldAward;
            this.deathStyleComboBox.SelectedItem = enemy.Statistics.DeathStyle;

            this.unknownDeathBit01CheckBox.Checked = enemy.Statistics.UnknownDeathBit01;
            this.unknownDeathBit20CheckBox.Checked = enemy.Statistics.UnknownDeathBit20;
            this.unknownDeathBit40CheckBox.Checked = enemy.Statistics.UnknownDeathBit40;
            this.unknownDeathBit80CheckBox.Checked = enemy.Statistics.UnknownDeathBit80;

            this.hpNumericUpDown.Value = enemy.Statistics.HitPoints;
            this.mpNumericUpDown.Value = enemy.Statistics.ManaPoints;
            this.strengthNumericUpDown.Value = enemy.Statistics.Strength;
            this.agilityNumericUpDown.Value = enemy.Statistics.Agility;
            this.intelligenceNumericUpDown.Value = enemy.Statistics.Intelligence;
            this.wisdomNumericUpDown.Value = enemy.Statistics.Wisdom;
            this.defenseNumericUpDown.Value = enemy.Statistics.Defense;
            this.evasionNumericUpDown.Value = enemy.Statistics.Evasion;
            this.magicDefenseNumericUpDown.Value = enemy.Statistics.MagicDefense;
            this.magicEvasionNumericUpDown.Value = enemy.Statistics.MagicEvasion;

            this.blackMagicPowerNumericUpDown.Value = enemy.Statistics.BlackMagicPower;
            this.whiteMagicPowerNumericUpDown.Value = enemy.Statistics.WhiteMagicPower;
            this.weaponLevelNumericUpDown.Value = enemy.Statistics.WeaponLevel;
            this.magicLevelNumericUpDown.Value = enemy.Statistics.MagicLevel;
            this.meleeWeaponNumericUpDown.Value = enemy.Statistics.MeleeWeapon;
            this.rangedWeaponNumericUpDown.Value = enemy.Statistics.RangedWeapon;

            int index = 0;
            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.immunitiesCheckedListBox.SetItemChecked(index, enemy.Statistics.Immunities.HasFlag(statusEffect));
                    index++;
                }
            }

            this.dropRateNumericUpDown.Value = enemy.Loot.DropRate;
            this.rareChanceNumericUpDown.Value = enemy.Loot.RareDropChance;
            this.agilityMultiplierNumericUpDown.Value = enemy.Loot.AgilityMultiplier;
            this.disarmLevelNumericUpDown.Value = enemy.Loot.DisarmLevel;
            this.fleesCheckBox.Checked = enemy.Loot.Flees;
            this.alwaysDropsCheckBox.Checked = enemy.Loot.AlwaysDrops;

            this.commonDropTypeComboBox.SelectedItem = enemy.Loot.CommonLoot.DropType;
            this.rareDropTypeComboBox.SelectedItem = enemy.Loot.RareLoot.DropType;

            this.UpdateDropRateLabel();
            this.UpdateRareDropChanceLabel();
            this.UpdateAgilityThresholdLabel();
            this.UpdateDropTypeMultiRef(enemy.Loot.CommonLoot.DropType, this.commonDropNumericUpDown, this.commonDropComboBox);
            this.UpdateDropTypeMultiRef(enemy.Loot.RareLoot.DropType, this.rareDropNumericUpDown, this.rareDropComboBox);
            this.UpdateDropVaue(enemy.Loot.CommonLoot, this.commonDropNumericUpDown, this.commonDropComboBox);
            this.UpdateDropVaue(enemy.Loot.RareLoot, this.rareDropNumericUpDown, this.rareDropComboBox);

            this.paletteControl.SetColorPalette(enemy.Palette);

            this.ignoreEvents = false;
        }

        private void UpdateDropRateLabel()
        {
            if (this.enemy != null)
            {
                this.dropRateLabel.Text = enemy.Loot.DropRatePercent.ToString("P2");
            }
        }

        private void UpdateRareDropChanceLabel()
        {
            if (this.enemy != null)
            {
                this.rareChanceLabel.Text = enemy.Loot.RareDropChancePercent.ToString("P2");
            }
        }

        private void UpdateAgilityThresholdLabel()
        {
            if (this.enemy != null)
            {
                this.agilityThresholdLabel.Text = enemy.Loot.DodgeThreshold.ToString();
            }
        }

        private void UpdateDropVaue(DropEntry entry, NumericUpDown dropNumericUpDown, ComboBox dropComboBox)
        {
            if (entry.IsGold)
            {
                dropNumericUpDown.Value = entry.Value;
                return;
            }
            dropComboBox.SelectedIndex = entry.GetValueAsIndex();
        }

        private void UpdateDropTypeMultiRef(LootDropType dropType, NumericUpDown dropNumericUpDown, ComboBox dropComboBox)
        {
            switch (dropType)
            {
                case LootDropType.Gold:
                    dropNumericUpDown.Visible = true;
                    dropComboBox.Visible = false;
                    dropNumericUpDown.Value = 0;
                    break;
                case LootDropType.Consumable:
                    dropNumericUpDown.Visible = false;
                    dropComboBox.Visible = true;
                    dropComboBox.DataSource = null;
                    dropComboBox.DataSource = Enum.GetValues<ConsumableItemType>();
                    dropComboBox.SelectedItem = ConsumableItemType.Candy;
                    break;
                case LootDropType.Helmet:
                    dropNumericUpDown.Visible = false;
                    dropComboBox.Visible = true;
                    dropComboBox.DataSource = null;
                    dropComboBox.DataSource = Enum.GetValues<HelmetType>();
                    dropComboBox.SelectedItem = HelmetType.BareHead;
                    break;
                case LootDropType.Armor:
                    dropNumericUpDown.Visible = false;
                    dropComboBox.Visible = true;
                    dropComboBox.DataSource = null;
                    dropComboBox.DataSource = Enum.GetValues<ArmorType>();
                    dropComboBox.SelectedItem = ArmorType.No;
                    break;
                case LootDropType.Accessory:
                    dropNumericUpDown.Visible = false;
                    dropComboBox.Visible = true;
                    dropComboBox.DataSource = null;
                    dropComboBox.DataSource = Enum.GetValues<AccessoryType>();
                    dropComboBox.SelectedItem = AccessoryType.Nothing;
                    break;
                case LootDropType.WeaponOrb:
                    dropNumericUpDown.Visible = false;
                    dropComboBox.Visible = true;
                    dropComboBox.DataSource = null;
                    dropComboBox.DataSource = Enum.GetValues<WeaponOrbType>();
                    dropComboBox.SelectedItem = WeaponOrbType.Glove;
                    break;
            }
        }

        private IReadOnlyDictionary<string, Action<ManaEnemy>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<ManaEnemy>>()
            {
                { this.nameTextBox.Name, (ManaEnemy enemy) => { EnemyEditorUserControl.ResetTimer(this.nameChangeTimer); } },
                { this.levelNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Level = (byte)this.levelNumericUpDown.Value; } },
                { this.monsterTypeComboBox.Name, (ManaEnemy enemy) => { enemy.Statistics.MonsterType = (MonsterType)this.monsterTypeComboBox.SelectedItem;} },
                { this.elementComboBox.Name, (ManaEnemy enemy) => { enemy.Statistics.Element = (ElementalType)this.elementComboBox.SelectedItem;} },
                { this.expAwardNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.ExperienceAward = (ushort)this.expAwardNumericUpDown.Value; } },
                { this.goldAwardNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.GoldAward = (ushort)this.goldAwardNumericUpDown.Value; } },
                { this.deathStyleComboBox.Name, (ManaEnemy enemy) => { enemy.Statistics.DeathStyle = (DeathStyle)this.deathStyleComboBox.SelectedItem; } },

                { this.hpNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.HitPoints = (ushort)this.hpNumericUpDown.Value; } },
                { this.mpNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.ManaPoints = (byte)this.mpNumericUpDown.Value; } },
                { this.strengthNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Strength = (byte)this.strengthNumericUpDown.Value; } },
                { this.agilityNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Agility = (byte)this.agilityNumericUpDown.Value; } },
                { this.intelligenceNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Intelligence = (byte)this.intelligenceNumericUpDown.Value; } },
                { this.wisdomNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Wisdom = (byte)this.wisdomNumericUpDown.Value; } },
                { this.defenseNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Defense = (ushort)this.defenseNumericUpDown.Value; } },
                { this.evasionNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.Evasion = (byte)this.evasionNumericUpDown.Value; } },
                { this.magicDefenseNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.MagicDefense = (ushort)this.magicDefenseNumericUpDown.Value; } },
                { this.magicEvasionNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.MagicEvasion = (byte)this.magicEvasionNumericUpDown.Value; } },

                { this.blackMagicPowerNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.BlackMagicPower = (byte)this.blackMagicPowerNumericUpDown.Value; } },
                { this.whiteMagicPowerNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.WhiteMagicPower = (byte)this.whiteMagicPowerNumericUpDown.Value; } },
                { this.weaponLevelNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.WeaponLevel = (byte)this.weaponLevelNumericUpDown.Value; } },
                { this.magicLevelNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.MagicLevel = (byte)this.magicLevelNumericUpDown.Value; } },
                { this.meleeWeaponNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.MeleeWeapon = (byte)this.meleeWeaponNumericUpDown.Value; } },
                { this.rangedWeaponNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Statistics.RangedWeapon = (byte)this.rangedWeaponNumericUpDown.Value; } },

                { this.dropRateNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Loot.DropRate = (byte)this.dropRateNumericUpDown.Value; this.UpdateDropRateLabel(); } },
                { this.rareChanceNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Loot.RareDropChance = (byte)this.rareChanceNumericUpDown.Value; this.UpdateRareDropChanceLabel(); } },
                { this.agilityMultiplierNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Loot.AgilityMultiplier = (byte)this.agilityMultiplierNumericUpDown.Value; this.UpdateAgilityThresholdLabel(); } },
                { this.disarmLevelNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Loot.DisarmLevel = (byte)this.disarmLevelNumericUpDown.Value; } },
                { this.fleesCheckBox.Name, (ManaEnemy enemy) => { enemy.Loot.Flees = this.fleesCheckBox.Checked; } },
                { this.alwaysDropsCheckBox.Name, (ManaEnemy enemy) => { enemy.Loot.AlwaysDrops = this.alwaysDropsCheckBox.Checked; } },

                { this.commonDropNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Loot.CommonLoot.Value = (byte)this.commonDropNumericUpDown.Value; } },
                { this.commonDropComboBox.Name, (ManaEnemy enemy) => { enemy.Loot.CommonLoot.Value = (byte)this.commonDropComboBox.SelectedItem; } },
                { this.rareDropNumericUpDown.Name, (ManaEnemy enemy) => { enemy.Loot.RareLoot.Value = (byte)this.rareDropNumericUpDown.Value; } },
                { this.rareDropComboBox.Name, (ManaEnemy enemy) => { enemy.Loot.RareLoot.Value = (byte)this.rareDropComboBox.SelectedItem; } },
            };
        }

        private void EnemyEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetEnemy(0);
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetEnemy((int)this.indexNumericUpDown.Value);
        }

        private void Control_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && this.enemy != null && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<ManaEnemy>? eventHandler))
                {
                    eventHandler(this.enemy);
                }
            }
        }

        private void ImmunitiesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this.ignoreEvents && this.enemy != null)
            {
                StatusEffects value = (StatusEffects)this.immunitiesCheckedListBox.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    this.enemy.Statistics.Immunities |= value;
                    return;
                }
                this.enemy.Statistics.Immunities &= ~value;
            }
        }

        private void NameChangeTimer_Tick(object sender, EventArgs e)
        {
            this.nameChangeTimer.Stop();
            if (this.ignoreEvents)
            {
                this.nameChangeTimer.Start();
                return;
            }

            if (this.enemy != null)
            {
                this.enemy.SetName(this.nameTextBox.Text);
            }
        }

        private void CommonDropTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.ignoreEvents && this.enemy != null && this.commonDropTypeComboBox.SelectedItem is LootDropType dropType)
            {
                this.ignoreEvents = true;
                this.enemy.Loot.CommonLoot.DropType = dropType;
                this.UpdateDropTypeMultiRef(dropType, this.commonDropNumericUpDown, this.commonDropComboBox);
                this.ignoreEvents = false;
            }
        }

        private void RareDropTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.ignoreEvents && this.enemy != null && this.rareDropTypeComboBox.SelectedItem is LootDropType dropType)
            {
                this.ignoreEvents = true;
                this.enemy.Loot.RareLoot.DropType = dropType;
                this.UpdateDropTypeMultiRef(dropType, this.rareDropNumericUpDown, this.rareDropComboBox);
                this.ignoreEvents = false;
            }
        }

        private void PaletteControl_ColorChanged(object sender, ColorChanged e)
        {
            if (!this.ignoreEvents && this.enemy != null)
            {
                this.enemy.Palette.SetColor(e.Index, this.paletteControl.Colors[e.Index]);
            }
        }

        private static void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}
