using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Items;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Items
{
    public partial class WeaponDefinitionEditorUserControl : UserControl
    {
        private readonly IReadOnlyDictionary<string, Action<ManaWeapon>> EventHandlers;
        private ManaWeapon? weapon = null;
        private bool ignoreEvents = false;

        public WeaponDefinitionEditorUserControl()
        {
            InitializeComponent();

            this.nameTextBox.MaxLength = Constants.TextMaxLength;
            this.descriptionRichTextBox.MaxLength = Constants.TextMaxLength;

            this.weaponClassComboBox.DataSource = Enum.GetValues<WeaponClassType>();
            this.weaponClassComboBox.SelectedItem = WeaponClassType.PlayerGlove;

            this.strengthModifierComboBox.DataSource = Enum.GetValues<WeaponStatModifier>();
            this.strengthModifierComboBox.SelectedItem = WeaponStatModifier.None;

            this.agilityModifierComboBox.DataSource = Enum.GetValues<WeaponStatModifier>();
            this.agilityModifierComboBox.SelectedItem = WeaponStatModifier.None;

            this.constitutionModifierComboBox.DataSource = Enum.GetValues<WeaponStatModifier>();
            this.constitutionModifierComboBox.SelectedItem = WeaponStatModifier.None;

            this.intelligenceModifierComboBox.DataSource = Enum.GetValues<WeaponStatModifier>();
            this.intelligenceModifierComboBox.SelectedItem = WeaponStatModifier.None;

            this.wisdomModifierComboBox.DataSource = Enum.GetValues<WeaponStatModifier>();
            this.wisdomModifierComboBox.SelectedItem = WeaponStatModifier.None;

            this.projectileTypeComboBox.DataSource = Enum.GetValues<WeaponProjectileType>();
            this.projectileTypeComboBox.SelectedItem = WeaponProjectileType.None;

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

        private IReadOnlyDictionary<string, Action<ManaWeapon>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<ManaWeapon>>()
            {
                { this.nameTextBox.Name, (ManaWeapon weapon) => { WeaponDefinitionEditorUserControl.ResetTimer(this.nameChangeTimer); } },
                { this.descriptionRichTextBox.Name, (ManaWeapon weapon) => { WeaponDefinitionEditorUserControl.ResetTimer(this.descriptionChangeTimer); } },

                { this.weaponClassComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.WeaponClass = (WeaponClassType)this.weaponClassComboBox.SelectedItem;} },
                { this.strengthModifierComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.StrengthModifier = (WeaponStatModifier)this.strengthModifierComboBox.SelectedItem; } },
                { this.agilityModifierComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.AgilityModifier = (WeaponStatModifier)this.agilityModifierComboBox.SelectedItem; } },
                { this.constitutionModifierComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.ConstitutionModifier = (WeaponStatModifier)this.constitutionModifierComboBox.SelectedItem; } },
                { this.intelligenceModifierComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.IntelligenceModifier = (WeaponStatModifier)this.intelligenceModifierComboBox.SelectedItem; } },
                { this.wisdomModifierComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.WisdomModifier = (WeaponStatModifier)this.wisdomModifierComboBox.SelectedItem; } },

                { this.projectileTypeComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.ProjectileType = (WeaponProjectileType)this.projectileTypeComboBox.SelectedItem; } },
                { this.paletteIndexNumericUpDown.Name, (ManaWeapon weapon) => { weapon.Definition.PaletteIndex = (byte)this.paletteIndexNumericUpDown.Value; } },
                { this.affinityComboBox.Name, (ManaWeapon weapon) => { weapon.Definition.Affinity = (MonsterType)this.affinityComboBox.SelectedItem; } },
                { this.critChanceNumericUpDown.Name, (ManaWeapon weapon) => { weapon.Definition.CriticalChance = (byte)this.critChanceNumericUpDown.Value; } },
                { this.accuracyNumericUpDown.Name, (ManaWeapon weapon) => { weapon.Definition.Accuracy = (byte)this.accuracyNumericUpDown.Value; } },
                { this.powerNumericUpDown.Name, (ManaWeapon weapon) => { weapon.Definition.Power = (byte)this.powerNumericUpDown.Value; } },
                { this.inflictionRateNumericUpDown.Name, (ManaWeapon weapon) => { weapon.Definition.InflictionRate = (byte)this.inflictionRateNumericUpDown.Value; } },
                { this.unknownIndexNumericUpDown.Name, (ManaWeapon weapon) => { weapon.Definition.UnknownStatIndex = (byte)this.unknownIndexNumericUpDown.Value; } },
            };
        }

        public void SetWeapon(ManaWeapon weapon)
        {
            this.nameChangeTimer.Stop();
            this.descriptionChangeTimer.Stop();
            this.ignoreEvents = true;

            this.weapon = weapon;
            this.nameTextBox.Text = ManaUtil.GetNameFromEvent(weapon.Name, out bool canEditName);
            this.descriptionRichTextBox.Text = ManaUtil.GetNameFromEvent(weapon.Description, out bool canEditDescription);

            this.nameTextBox.Enabled = canEditName;
            this.descriptionRichTextBox.Enabled = canEditDescription;

            this.weaponClassComboBox.SelectedItem = weapon.Definition.WeaponClass;
            this.strengthModifierComboBox.SelectedItem = weapon.Definition.StrengthModifier;
            this.agilityModifierComboBox.SelectedItem = weapon.Definition.AgilityModifier;
            this.constitutionModifierComboBox.SelectedItem = weapon.Definition.ConstitutionModifier;
            this.intelligenceModifierComboBox.SelectedItem = weapon.Definition.IntelligenceModifier;
            this.wisdomModifierComboBox.SelectedItem = weapon.Definition.WisdomModifier;

            this.projectileTypeComboBox.SelectedItem = weapon.Definition.ProjectileType;
            this.paletteIndexNumericUpDown.Value = weapon.Definition.PaletteIndex;
            this.affinityComboBox.SelectedItem = weapon.Definition.Affinity;
            this.critChanceNumericUpDown.Value = weapon.Definition.CriticalChance;
            this.accuracyNumericUpDown.Value = weapon.Definition.Accuracy;
            this.powerNumericUpDown.Value = weapon.Definition.Power;
            this.inflictionRateNumericUpDown.Value = weapon.Definition.InflictionRate;
            this.unknownIndexNumericUpDown.Value = weapon.Definition.UnknownStatIndex;

            int index = 0;
            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.statusEffectsCheckedListBox.SetItemChecked(index, weapon.Definition.StatusEffects.HasFlag(statusEffect));
                    index++;
                }
            }

            this.iconPictureBox.Image?.Dispose();
            this.iconPictureBox.Image = null;
            if (!weapon.Icon.IsEmpty)
            {
                using SuperNintendoGraphics graphics = weapon.Icon.Draw();
                this.iconPictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }

            this.ignoreEvents = false;
        }

        private void Control_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && this.weapon != null && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<ManaWeapon>? eventHandler))
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
                    this.weapon.Definition.StatusEffects |= value;
                    return;
                }
                this.weapon.Definition.StatusEffects &= ~value;
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

            if (this.weapon != null)
            {
                this.weapon.SetName(this.nameTextBox.Text);
            }
        }

        private void DescriptionChangeTimer_Tick(object sender, EventArgs e)
        {
            this.descriptionChangeTimer.Stop();
            if (this.ignoreEvents)
            {
                this.descriptionChangeTimer.Start();
                return;
            }

            if (this.weapon != null)
            {
                this.weapon.SetDescription(this.descriptionRichTextBox.Text);
            }
        }

        private static void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}