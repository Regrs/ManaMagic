using System;
using System.Windows.Forms;
using ManaMagic.Core.Items;

#nullable enable

namespace ManaMagic.Controls.UserControls.Items
{
    public partial class WeaponEditorUserControl : UserControl
    {
        private const byte GlovesMinimum = 0x00;
        private const byte GlovesMaximum = 0x08;

        private const byte SwordsMinimum = 0x09;
        private const byte SwordsMaximum = 0x11;

        private const byte AxesMinimum = 0x12;
        private const byte AxesMaximum = 0x1A;

        private const byte SpearsMinimum = 0x1B;
        private const byte SpearsMaximum = 0x23;

        private const byte WhipsMinimum = 0x24;
        private const byte WhipsMaximum = 0x2C;

        private const byte BowsMinimum = 0x2D;
        private const byte BowsMaximum = 0x35;

        private const byte BoomerangsMinimum = 0x36;
        private const byte BoomerangsMaximum = 0x3E;

        private const byte JavelinsMinimum = 0x3F;
        private const byte JavelinsMaximum = 0x47;

        private const byte EnemyWeaponsMinimum = 0x48;
        private const byte EnemyWeaponsMaximum = 0xBD;

        private const byte PlaceholdersMinimum = 0xBE;
        private const byte PlaceholdersMaximum = 0xF0;

        private const byte StatusEffectsMinimum = 0xF1;
        private const byte StatusEffectsMaximum = 0xFF;

        public WeaponEditorUserControl()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            this.glovesNumericUpDown.Minimum = WeaponEditorUserControl.GlovesMinimum;
            this.glovesNumericUpDown.Maximum = WeaponEditorUserControl.GlovesMaximum;
            this.glovesNumericUpDown.Tag = this.glovesDefinitionEditorUserControl;

            this.swordsNumericUpDown.Minimum = WeaponEditorUserControl.SwordsMinimum;
            this.swordsNumericUpDown.Maximum = WeaponEditorUserControl.SwordsMaximum;
            this.swordsNumericUpDown.Tag = this.swordsDefinitionEditorUserControl;

            this.axesNumericUpDown.Minimum = WeaponEditorUserControl.AxesMinimum;
            this.axesNumericUpDown.Maximum = WeaponEditorUserControl.AxesMaximum;
            this.axesNumericUpDown.Tag = this.axesDefinitionEditorUserControl;

            this.spearsNumericUpDown.Minimum = WeaponEditorUserControl.SpearsMinimum;
            this.spearsNumericUpDown.Maximum = WeaponEditorUserControl.SpearsMaximum;
            this.spearsNumericUpDown.Tag = this.spearsDefinitionEditorUserControl;

            this.whipsNumericUpDown.Minimum = WeaponEditorUserControl.WhipsMinimum;
            this.whipsNumericUpDown.Maximum = WeaponEditorUserControl.WhipsMaximum;
            this.whipsNumericUpDown.Tag = this.whipsDefinitionEditorUserControl;

            this.bowsNumericUpDown.Minimum = WeaponEditorUserControl.BowsMinimum;
            this.bowsNumericUpDown.Maximum = WeaponEditorUserControl.BowsMaximum;
            this.bowsNumericUpDown.Tag = this.bowsDefinitionEditorUserControl;

            this.boomerangsNumericUpDown.Minimum = WeaponEditorUserControl.BoomerangsMinimum;
            this.boomerangsNumericUpDown.Maximum = WeaponEditorUserControl.BoomerangsMaximum;
            this.boomerangsNumericUpDown.Tag = this.boomerangsDefinitionEditorUserControl;

            this.javelinsNumericUpDown.Minimum = WeaponEditorUserControl.JavelinsMinimum;
            this.javelinsNumericUpDown.Maximum = WeaponEditorUserControl.JavelinsMaximum;
            this.javelinsNumericUpDown.Tag = this.javelinsDefinitionEditorUserControl;

            this.enemyWeaponsNumericUpDown.Minimum = WeaponEditorUserControl.EnemyWeaponsMinimum;
            this.enemyWeaponsNumericUpDown.Maximum = WeaponEditorUserControl.EnemyWeaponsMaximum;
            this.enemyWeaponsNumericUpDown.Tag = this.enemyWeaponsDefinitionEditorUserControl;

            this.placeholdersNumericUpDown.Minimum = WeaponEditorUserControl.PlaceholdersMinimum;
            this.placeholdersNumericUpDown.Maximum = WeaponEditorUserControl.PlaceholdersMaximum;
            this.placeholdersNumericUpDown.Tag = this.placeholdersDefinitionEditorUserControl;

            this.statusEffectsNumericUpDown.Minimum = WeaponEditorUserControl.StatusEffectsMinimum;
            this.statusEffectsNumericUpDown.Maximum = WeaponEditorUserControl.StatusEffectsMaximum;
            this.statusEffectsNumericUpDown.Tag = this.statusEffectsDefinitionEditorUserControl;

            WeaponEditorUserControl.SetWeapon((int)this.glovesNumericUpDown.Value, this.glovesDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.swordsNumericUpDown.Value, this.swordsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.axesNumericUpDown.Value, this.axesDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.spearsNumericUpDown.Value, this.spearsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.whipsNumericUpDown.Value, this.whipsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.bowsNumericUpDown.Value, this.bowsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.boomerangsNumericUpDown.Value, this.boomerangsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.javelinsNumericUpDown.Value, this.javelinsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.enemyWeaponsNumericUpDown.Value, this.enemyWeaponsDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.placeholdersNumericUpDown.Value, this.placeholdersDefinitionEditorUserControl);
            WeaponEditorUserControl.SetWeapon((int)this.statusEffectsNumericUpDown.Value, this.statusEffectsDefinitionEditorUserControl);

            this.glovesNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.swordsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.axesNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.spearsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.whipsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.bowsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.boomerangsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.javelinsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.enemyWeaponsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.placeholdersNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
            this.statusEffectsNumericUpDown.ValueChanged += WeaponEditorUserControl.WeaponIndexNumericUpDown_ValueChanged;
        }

        private void WeaponsUserControl_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        private static void WeaponIndexNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is NumericUpDown numericUpDown)
            {
                int index = (int)numericUpDown.Value;
                if (numericUpDown.Tag is WeaponDefinitionEditorUserControl weaponDefinitionEditor)
                {
                    WeaponEditorUserControl.SetWeapon(index, weaponDefinitionEditor);
                }
            }
        }

        private static void SetWeapon(int index, WeaponDefinitionEditorUserControl weaponDefinitionEditor)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                ManaWeapon weapon = ManaMagicContext.Current.Context.ItemContext.Weapons[index];
                weaponDefinitionEditor.SetWeapon(weapon);
            }
        }
    }
}