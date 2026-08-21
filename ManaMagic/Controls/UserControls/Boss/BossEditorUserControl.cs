using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Sprites;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Boss
{
    public partial class BossEditorUserControl : UserControl
    {
        private ManaBoss? boss = null;
        private bool ignoreEvents = false;
        private readonly IReadOnlyDictionary<string, Action<ManaBoss>> EventHandlers;

        public BossEditorUserControl()
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

            foreach (BossControlFlags controlFlag in Enum.GetValues<BossControlFlags>())
            {
                if (controlFlag != BossControlFlags.None &&
                    controlFlag != BossControlFlags.FrameRuleL &&
                    controlFlag != BossControlFlags.FrameRuleH)
                {
                    this.controlFlagsCheckedListBox.Items.Add(controlFlag);
                }
            }

            this.EventHandlers = this.LoadEventHandlers();
        }

        private void SetBoss(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaBoss boss = ManaMagicContext.Current.Context.BossContext.Bosses[index - 0x57];
                this.SetBoss(boss);
            }
        }

        private void SetBoss(ManaBoss boss)
        {
            this.nameChangeTimer.Stop();
            this.ignoreEvents = true;

            this.boss = boss;
            this.nameTextBox.Text = ManaUtil.GetNameFromEvent(boss.Name, out bool canEditName);
            this.nameTextBox.Enabled = canEditName;

            this.levelNumericUpDown.Value = boss.Statistics.Level;
            this.monsterTypeComboBox.SelectedItem = boss.Statistics.MonsterType;
            this.elementComboBox.SelectedItem = boss.Statistics.Element;
            this.expAwardNumericUpDown.Value = boss.Statistics.ExperienceAward;
            this.goldAwardNumericUpDown.Value = boss.Statistics.GoldAward;
            this.deathStyleComboBox.SelectedItem = boss.Statistics.DeathStyle;

            this.unknownDeathBit01CheckBox.Checked = boss.Statistics.UnknownDeathBit01;
            this.unknownDeathBit20CheckBox.Checked = boss.Statistics.UnknownDeathBit20;
            this.unknownDeathBit40CheckBox.Checked = boss.Statistics.UnknownDeathBit40;
            this.unknownDeathBit80CheckBox.Checked = boss.Statistics.UnknownDeathBit80;

            this.hpNumericUpDown.Value = boss.Statistics.HitPoints;
            this.mpNumericUpDown.Value = boss.Statistics.ManaPoints;
            this.strengthNumericUpDown.Value = boss.Statistics.Strength;
            this.agilityNumericUpDown.Value = boss.Statistics.Agility;
            this.intelligenceNumericUpDown.Value = boss.Statistics.Intelligence;
            this.wisdomNumericUpDown.Value = boss.Statistics.Wisdom;
            this.defenseNumericUpDown.Value = boss.Statistics.Defense;
            this.evasionNumericUpDown.Value = boss.Statistics.Evasion;
            this.magicDefenseNumericUpDown.Value = boss.Statistics.MagicDefense;
            this.magicEvasionNumericUpDown.Value = boss.Statistics.MagicEvasion;

            this.blackMagicPowerNumericUpDown.Value = boss.Statistics.BlackMagicPower;
            this.whiteMagicPowerNumericUpDown.Value = boss.Statistics.WhiteMagicPower;
            this.weaponLevelNumericUpDown.Value = boss.Statistics.WeaponLevel;
            this.magicLevelNumericUpDown.Value = boss.Statistics.MagicLevel;
            this.meleeWeaponNumericUpDown.Value = boss.Statistics.MeleeWeapon;
            this.rangedWeaponNumericUpDown.Value = boss.Statistics.RangedWeapon;

            int index = 0;
            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.immunitiesCheckedListBox.SetItemChecked(index, boss.Statistics.Immunities.HasFlag(statusEffect));
                    index++;
                }
            }

            index = 0;
            bool hasHeader = boss.HasHeader;
            this.aiFrameNumericUpDown.Enabled = hasHeader;
            this.controlFlagsCheckedListBox.Enabled = hasHeader;

            this.aiFrameNumericUpDown.Value = hasHeader ? this.boss.Header.AIFrame : 0;
            foreach (BossControlFlags controlFlag in Enum.GetValues<BossControlFlags>())
            {
                if (controlFlag != BossControlFlags.None &&
                    controlFlag != BossControlFlags.FrameRuleL &&
                    controlFlag != BossControlFlags.FrameRuleH)
                {
                    bool value = hasHeader ? boss.Header.ControlFlags.HasFlag(controlFlag) : false;
                    this.controlFlagsCheckedListBox.SetItemChecked(index, value);
                    index++;
                }
            }

            SetPaletteControl(boss.Palette1, this.palette1Control);
            SetPaletteControl(boss.Palette2, this.palette2Control);
            SetPaletteControl(boss.Palette3, this.palette3Control);
            SetPaletteControl(boss.BackgroundPalette1, this.paletteBackground1Control);
            SetPaletteControl(boss.BackgroundPalette2, this.paletteBackground2Control);
            SetPaletteControl(boss.ManaBeastPalette1, this.paletteManaBeast1Control);
            SetPaletteControl(boss.ManaBeastPalette2, this.paletteManaBeast2Control);

            this.ignoreEvents = false;

            static void SetPaletteControl(SpritePalette palette, PaletteControl paletteControl)
            {
                paletteControl.SetColorPalette(palette);
                paletteControl.Visible = palette != SpritePalette.Empty;
            }
        }

        private IReadOnlyDictionary<string, Action<ManaBoss>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<ManaBoss>>()
            {
                { this.nameTextBox.Name, (ManaBoss enemy) => { BossEditorUserControl.ResetTimer(this.nameChangeTimer); } },
                { this.levelNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Level = (byte)this.levelNumericUpDown.Value; } },
                { this.monsterTypeComboBox.Name, (ManaBoss enemy) => { enemy.Statistics.MonsterType = (MonsterType)this.monsterTypeComboBox.SelectedItem;} },
                { this.elementComboBox.Name, (ManaBoss enemy) => { enemy.Statistics.Element = (ElementalType)this.elementComboBox.SelectedItem;} },
                { this.expAwardNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.ExperienceAward = (ushort)this.expAwardNumericUpDown.Value; } },
                { this.goldAwardNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.GoldAward = (ushort)this.goldAwardNumericUpDown.Value; } },
                { this.deathStyleComboBox.Name, (ManaBoss enemy) => { enemy.Statistics.DeathStyle = (DeathStyle)this.deathStyleComboBox.SelectedItem; } },

                { this.hpNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.HitPoints = (ushort)this.hpNumericUpDown.Value; } },
                { this.mpNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.ManaPoints = (byte)this.mpNumericUpDown.Value; } },
                { this.strengthNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Strength = (byte)this.strengthNumericUpDown.Value; } },
                { this.agilityNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Agility = (byte)this.agilityNumericUpDown.Value; } },
                { this.intelligenceNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Intelligence = (byte)this.intelligenceNumericUpDown.Value; } },
                { this.wisdomNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Wisdom = (byte)this.wisdomNumericUpDown.Value; } },
                { this.defenseNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Defense = (ushort)this.defenseNumericUpDown.Value; } },
                { this.evasionNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.Evasion = (byte)this.evasionNumericUpDown.Value; } },
                { this.magicDefenseNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.MagicDefense = (ushort)this.magicDefenseNumericUpDown.Value; } },
                { this.magicEvasionNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.MagicEvasion = (byte)this.magicEvasionNumericUpDown.Value; } },

                { this.blackMagicPowerNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.BlackMagicPower = (byte)this.blackMagicPowerNumericUpDown.Value; } },
                { this.whiteMagicPowerNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.WhiteMagicPower = (byte)this.whiteMagicPowerNumericUpDown.Value; } },
                { this.weaponLevelNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.WeaponLevel = (byte)this.weaponLevelNumericUpDown.Value; } },
                { this.magicLevelNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.MagicLevel = (byte)this.magicLevelNumericUpDown.Value; } },
                { this.meleeWeaponNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.MeleeWeapon = (byte)this.meleeWeaponNumericUpDown.Value; } },
                { this.rangedWeaponNumericUpDown.Name, (ManaBoss enemy) => { enemy.Statistics.RangedWeapon = (byte)this.rangedWeaponNumericUpDown.Value; } },
            };
        }

        private void BossEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetBoss(0x57);
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetBoss((int)this.indexNumericUpDown.Value);
        }

        private void Control_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && this.boss != null && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<ManaBoss>? eventHandler))
                {
                    eventHandler(this.boss);
                }
            }
        }

        private void PaletteControl_ColorChanged(object sender, ColorChanged e)
        {
            if (!this.ignoreEvents && this.boss != null)
            {
                if (sender == this.palette1Control) { this.boss.Palette1.SetColor(e.Index, this.palette1Control[e.Index]); }
                else if (sender == this.palette2Control) { this.boss.Palette2.SetColor(e.Index, this.palette2Control[e.Index]); }
                else if (sender == this.palette3Control) { this.boss.Palette3.SetColor(e.Index, this.palette3Control[e.Index]); }
                else if (sender == this.paletteBackground1Control) { this.boss.BackgroundPalette1.SetColor(e.Index, this.paletteBackground1Control[e.Index]); }
                else if (sender == this.paletteBackground2Control) { this.boss.BackgroundPalette2.SetColor(e.Index, this.paletteBackground2Control[e.Index]); }
                else if (sender == this.paletteManaBeast1Control) { this.boss.ManaBeastPalette1.SetColor(e.Index, this.paletteManaBeast1Control[e.Index]); }
                else if (sender == this.paletteManaBeast2Control) { this.boss.ManaBeastPalette2.SetColor(e.Index, this.paletteManaBeast2Control[e.Index]); }
            }
        }

        private void ImmunitiesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this.ignoreEvents && this.boss != null)
            {
                StatusEffects value = (StatusEffects)this.immunitiesCheckedListBox.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    this.boss.Statistics.Immunities |= value;
                    return;
                }
                this.boss.Statistics.Immunities &= ~value;
            }
        }

        private void ControlFlagsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this.ignoreEvents && this.boss != null && boss.HasHeader)
            {
                BossControlFlags value = (BossControlFlags)this.controlFlagsCheckedListBox.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    this.boss.Header.ControlFlags |= value;
                    return;
                }
                this.boss.Header.ControlFlags &= ~value;
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

            if (this.boss != null)
            {
                this.boss.SetName(this.nameTextBox.Text);
            }
        }

        private static void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}
