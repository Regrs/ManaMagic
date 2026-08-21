using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Items;
using ZwellTech;

#nullable enable

namespace ManaMagic.Controls.UserControls.Items
{
    public partial class EquipmentEditorUserControl : UserControl
    {
        public EquipmentEditorUserControl()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            this.helmetsNumericUpDown.Minimum = 0;
            this.helmetsNumericUpDown.Maximum = Constants.Bank18.RingIconHelmetIconCount - 1;
            this.helmetsNumericUpDown.Tag = this.helmetDefinitionEditorUserControl;

            this.armorNumericUpDown.Minimum = 0;
            this.armorNumericUpDown.Maximum = Constants.Bank18.RingIconArmorIconCount - 1;
            this.armorNumericUpDown.Tag = this.armorDefinitionEditorUserControl;

            this.accessoryNumericUpDown.Minimum = 0;
            this.accessoryNumericUpDown.Maximum = Constants.Bank18.RingIconAccessoryIconCount;
            this.accessoryNumericUpDown.Tag = this.accessoryDefinitionEditorUserControl;

            this.helmetDefinitionEditorUserControl.DisplayType = EquipmentDisplayType.Helmets;
            this.armorDefinitionEditorUserControl.DisplayType = EquipmentDisplayType.Armor;
            this.accessoryDefinitionEditorUserControl.DisplayType = EquipmentDisplayType.Accessories;

            EquipmentEditorUserControl.SetEquipment((int)this.helmetsNumericUpDown.Value, this.helmetDefinitionEditorUserControl);
            EquipmentEditorUserControl.SetEquipment((int)this.armorNumericUpDown.Value, this.armorDefinitionEditorUserControl);
            EquipmentEditorUserControl.SetEquipment((int)this.accessoryNumericUpDown.Value, this.accessoryDefinitionEditorUserControl);

            this.helmetsNumericUpDown.ValueChanged += EquipmentEditorUserControl.EquipmentIndexNumericUpDown_ValueChanged;
            this.armorNumericUpDown.ValueChanged += EquipmentEditorUserControl.EquipmentIndexNumericUpDown_ValueChanged;
            this.accessoryNumericUpDown.ValueChanged += EquipmentEditorUserControl.EquipmentIndexNumericUpDown_ValueChanged;
        }

        private void EquipmentEditorUserControl_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        private static void EquipmentIndexNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is NumericUpDown numericUpDown)
            {
                int index = (int)numericUpDown.Value;
                if (numericUpDown.Tag is EquipmentDefinitionEditorUserControl equipmentDefinitionEditor)
                {
                    EquipmentEditorUserControl.SetEquipment(index, equipmentDefinitionEditor);
                }
            }
        }

        private static void SetEquipment(int index, EquipmentDefinitionEditorUserControl equipmentDefinitionEditor)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaEquipment equipment;
                switch (equipmentDefinitionEditor.DisplayType)
                {
                    case EquipmentDisplayType.Helmets:
                        equipment = ManaMagicContext.Current.Context.ItemContext.Helmets[index];
                        break;
                    case EquipmentDisplayType.Armor:
                        equipment = ManaMagicContext.Current.Context.ItemContext.Armor[index];
                        break;
                    case EquipmentDisplayType.Accessories:
                        equipment = ManaMagicContext.Current.Context.ItemContext.Accessories[index];
                        break;
                    default:
                        ThrowHelper.ThrowInvalidOperationException("Invalid Display Type");
                        return;
                }

                equipmentDefinitionEditor.SetEquipment(equipment);
            }
        }
    }
}
