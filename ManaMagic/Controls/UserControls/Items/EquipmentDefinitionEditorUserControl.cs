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
    public partial class EquipmentDefinitionEditorUserControl : UserControl
    {
        private readonly IReadOnlyDictionary<string, Action<ManaEquipment>> EventHandlers;
        private ManaEquipment? equipment = null;
        private bool ignoreEvents = false;

        public EquipmentDisplayType DisplayType { get; set; }

        public EquipmentDefinitionEditorUserControl()
        {
            InitializeComponent();
            this.elementComboBox.DataSource = Enum.GetValues<ElementalType>();
            this.elementComboBox.SelectedItem = ElementalType.None;

            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.resistancesCheckedListBox.Items.Add(statusEffect);
                }
            }
            this.EventHandlers = this.LoadEventHandlers();
        }

        public void SetEquipment(ManaEquipment equipment)
        {
            this.nameChangeTimer.Stop();
            this.ignoreEvents = true;

            this.equipment = equipment;
            this.nameTextBox.Text = ManaUtil.GetNameFromEvent(equipment.Name, out bool canEditName);
            this.nameTextBox.Enabled = canEditName;

            this.elementComboBox.SelectedItem = equipment.Definition.Element;
            this.randiCheckBox.Checked = equipment.Definition.EquippableBy.HasFlag(EquipmentEquippableSettings.Randi);
            this.purimCheckBox.Checked = equipment.Definition.EquippableBy.HasFlag(EquipmentEquippableSettings.Purim);
            this.popoieCheckBox.Checked = equipment.Definition.EquippableBy.HasFlag(EquipmentEquippableSettings.Popoie);
            this.defenseNumericUpDown.Value = equipment.Definition.Defense;
            this.evasionNumericUpDown.Value = equipment.Definition.Evasion;
            this.magicDefenseNumericUpDown.Value = equipment.Definition.MagicDefense;
            this.magicEvasionNumericUpDown.Value = equipment.Definition.MagicEvasion;
            this.strengthCheckBox.Checked = equipment.Definition.StatModifiers.HasFlag(EquipmentStatModifiers.Strength);
            this.agilityCheckBox.Checked = equipment.Definition.StatModifiers.HasFlag(EquipmentStatModifiers.Agility);
            this.constitutionCheckBox.Checked = equipment.Definition.StatModifiers.HasFlag(EquipmentStatModifiers.Constitution);
            this.intelligenceCheckBox.Checked = equipment.Definition.StatModifiers.HasFlag(EquipmentStatModifiers.Intelligence);
            this.wisdomCheckBox.Checked = equipment.Definition.StatModifiers.HasFlag(EquipmentStatModifiers.Wisdom);
            this.statAdjustmentTypeFlagCheckBox.Checked = equipment.Definition.StatModifiers.HasFlag(EquipmentStatModifiers.StatAdjustmentTypeFlag);

            int index = 0;
            foreach (StatusEffects statusEffect in Enum.GetValues<StatusEffects>())
            {
                if (statusEffect != StatusEffects.None)
                {
                    this.resistancesCheckedListBox.SetItemChecked(index, equipment.Definition.Resistances.HasFlag(statusEffect));
                    index++;
                }
            }

            this.iconPictureBox.Image?.Dispose();
            this.iconPictureBox.Image = null;
            if (!equipment.Icon.IsEmpty)
            {
                using SuperNintendoGraphics graphics = equipment.Icon.Draw();
                this.iconPictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }

            this.ignoreEvents = false;
        }

        private IReadOnlyDictionary<string, Action<ManaEquipment>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<ManaEquipment>>()
            {
                { this.nameTextBox.Name, (ManaEquipment equipment) => { EquipmentDefinitionEditorUserControl.ResetTimer(this.nameChangeTimer); } },

                { this.strengthCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.StatModifiers = (EquipmentStatModifiers)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.StatModifiers, (byte)EquipmentStatModifiers.Strength, this.strengthCheckBox.Checked); } },
                { this.agilityCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.StatModifiers = (EquipmentStatModifiers)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.StatModifiers, (byte)EquipmentStatModifiers.Agility, this.agilityCheckBox.Checked); } },
                { this.constitutionCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.StatModifiers = (EquipmentStatModifiers)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.StatModifiers, (byte)EquipmentStatModifiers.Constitution, this.constitutionCheckBox.Checked); } },
                { this.intelligenceCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.StatModifiers = (EquipmentStatModifiers)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.StatModifiers, (byte)EquipmentStatModifiers.Intelligence, this.intelligenceCheckBox.Checked); } },
                { this.wisdomCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.StatModifiers = (EquipmentStatModifiers)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.StatModifiers, (byte)EquipmentStatModifiers.Wisdom, this.wisdomCheckBox.Checked); } },

                { this.elementComboBox.Name, (ManaEquipment equipment) => { equipment.Definition.Element = (ElementalType)this.elementComboBox.SelectedItem;} },
                { this.defenseNumericUpDown.Name, (ManaEquipment equipment) => { equipment.Definition.Defense = (byte)this.defenseNumericUpDown.Value; } },
                { this.evasionNumericUpDown.Name, (ManaEquipment equipment) => { equipment.Definition.Evasion = (byte)this.evasionNumericUpDown.Value; } },
                { this.magicDefenseNumericUpDown.Name, (ManaEquipment equipment) => { equipment.Definition.MagicDefense = (byte)this.magicDefenseNumericUpDown.Value; } },
                { this.magicEvasionNumericUpDown.Name, (ManaEquipment equipment) => { equipment.Definition.MagicEvasion = (byte)this.magicEvasionNumericUpDown.Value; } },

                { this.randiCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.EquippableBy = (EquipmentEquippableSettings)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.EquippableBy, (byte)EquipmentEquippableSettings.Randi, this.randiCheckBox.Checked); } },
                { this.purimCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.EquippableBy = (EquipmentEquippableSettings)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.EquippableBy, (byte)EquipmentEquippableSettings.Purim, this.purimCheckBox.Checked); } },
                { this.popoieCheckBox.Name, (ManaEquipment equipment) => { equipment.Definition.EquippableBy = (EquipmentEquippableSettings)EquipmentDefinitionEditorUserControl.SetFlagState((byte)equipment.Definition.EquippableBy, (byte)EquipmentEquippableSettings.Popoie, this.popoieCheckBox.Checked); } },
            };
        }

        private void Control_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && this.equipment != null && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<ManaEquipment>? eventHandler))
                {
                    eventHandler(this.equipment);
                }
            }
        }

        private void ResistancesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this.ignoreEvents && this.equipment != null)
            {
                StatusEffects value = (StatusEffects)this.resistancesCheckedListBox.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    this.equipment.Definition.Resistances |= value;
                    return;
                }
                this.equipment.Definition.Resistances &= ~value;
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

            if (this.equipment != null)
            {
                this.equipment.SetName(this.nameTextBox.Text);
            }
        }

        private static void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        private static byte SetFlagState(byte value, byte flag, bool set)
        {
            if (set)
            {
                value |= flag;
            }
            else
            {
                value &= (byte)~flag;
            }
            return value;
        }
    }

    public enum EquipmentDisplayType
    {
        Helmets,
        Armor,
        Accessories,
    }
}