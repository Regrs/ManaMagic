using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Metadata;

#nullable enable

namespace ManaMagic.Controls.UserControls.Boss
{
    public partial class BossWeaponEditorUserControl : UserControl
    {
        private readonly IReadOnlyDictionary<string, Action<ManaBossWeapon>> EventHandlers;
        private ManaBossWeapon? weapon = null;
        private bool ignoreEvents = false;

        public BossWeaponEditorUserControl()
        {
            InitializeComponent();
            this.indexNumericUpDown.Maximum = Constants.Bank10.BossWeaponTableSize - 1;

            this.elementComboBox.DataSource = Enum.GetValues<ElementalType>();
            this.elementComboBox.SelectedItem = ElementalType.None;

            this.affinityComboBox.DataSource = Enum.GetValues<MonsterType>();
            this.affinityComboBox.SelectedItem = MonsterType.None;

            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.statusEffectsCheckedListBox.Items.Add(statusEffect);
                }
            }

            this.EventHandlers = this.LoadEventHandlers();
        }

        private void SetWeapon(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaBossWeapon weapon = ManaMagicContext.Current.Context.BossContext.WeaponTable[index];
                this.SetWeapon(weapon);
            }
        }

        private void SetWeapon(ManaBossWeapon weapon)
        {
            this.ignoreEvents = true;

            this.weapon = weapon;
            this.nameLabel.Text = ManaMetadata.BossWeaponNameStrings[weapon.Index];

            this.affinityComboBox.SelectedItem = weapon.MonsterAffinity;
            this.elementComboBox.SelectedItem = weapon.Element;

            this.accuracyNumericUpDown.Value = weapon.Accuracy;
            this.powerNumericUpDown.Value = weapon.Power;
            this.inflictionRateNumericUpDown.Value = weapon.InflictionRate;

            int index = 0;
            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.statusEffectsCheckedListBox.SetItemChecked(index, weapon.StatusEffects.HasFlag(statusEffect));
                    index++;
                }
            }

            this.ignoreEvents = false;
        }

        private IReadOnlyDictionary<string, Action<ManaBossWeapon>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<ManaBossWeapon>>()
            {
                { this.affinityComboBox.Name, (ManaBossWeapon weapon) => { weapon.MonsterAffinity = (MonsterType)this.affinityComboBox.SelectedItem;} },
                { this.elementComboBox.Name, (ManaBossWeapon weapon) => { weapon.Element = (ElementalType)this.elementComboBox.SelectedItem;} },
                { this.accuracyNumericUpDown.Name, (ManaBossWeapon weapon) => { weapon.Accuracy = (byte)this.accuracyNumericUpDown.Value; } },
                { this.powerNumericUpDown.Name, (ManaBossWeapon weapon) => { weapon.Power = (byte)this.powerNumericUpDown.Value; } },
                { this.inflictionRateNumericUpDown.Name, (ManaBossWeapon weapon) => { weapon.InflictionRate = (byte)this.inflictionRateNumericUpDown.Value; } },
            };
        }

        private void BossWeaponEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetWeapon(0);
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetWeapon((int)this.indexNumericUpDown.Value);
        }

        private void Control_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && this.weapon != null && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<ManaBossWeapon>? eventHandler))
                {
                    eventHandler(this.weapon);
                }
            }
        }

        private void StatusEffectsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this.ignoreEvents && this.weapon != null)
            {
                StatusEffects value = (StatusEffects)this.statusEffectsCheckedListBox.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    this.weapon.StatusEffects |= value;
                    return;
                }
                this.weapon.StatusEffects &= ~value;
            }
        }
    }
}