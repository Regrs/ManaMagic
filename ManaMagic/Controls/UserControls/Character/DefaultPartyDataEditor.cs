using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ManaMagic.Core.Character;
using ManaMagic.Core.Items;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.Character
{
    public partial class DefaultPartyDataEditor : UserControl
    {
        private readonly IReadOnlyDictionary<string, Action<ManaDefaultCharacterData>> EventHandlers;
        private bool ignoreEvents = true;

        public DefaultPartyDataEditor()
        {
            InitializeComponent();

            this.ignoreEvents = true;
            this.randiHelmetComboBox.DataSource = Enum.GetValues<HelmetType>();
            this.randiHelmetComboBox.SelectedItem = HelmetType.BareHead;
            this.randiArmorComboBox.DataSource = Enum.GetValues<ArmorType>();
            this.randiArmorComboBox.SelectedItem = ArmorType.No;
            this.randiAccessoryComboBox.DataSource = Enum.GetValues<AccessoryType>();
            this.randiAccessoryComboBox.SelectedItem = AccessoryType.Nothing;
            this.randiWeaponComboBox.DataSource = Enum.GetValues<WeaponType>();
            this.randiWeaponComboBox.SelectedItem = WeaponType.SpikeKnuckle;

            this.purimHelmetComboBox.DataSource = Enum.GetValues<HelmetType>();
            this.purimHelmetComboBox.SelectedItem = HelmetType.BareHead;
            this.purimArmorComboBox.DataSource = Enum.GetValues<ArmorType>();
            this.purimArmorComboBox.SelectedItem = ArmorType.No;
            this.purimAccessoryComboBox.DataSource = Enum.GetValues<AccessoryType>();
            this.purimAccessoryComboBox.SelectedItem = AccessoryType.Nothing;
            this.purimWeaponComboBox.DataSource = Enum.GetValues<WeaponType>();
            this.purimWeaponComboBox.SelectedItem = WeaponType.SpikeKnuckle;

            this.popoieHelmetComboBox.DataSource = Enum.GetValues<HelmetType>();
            this.popoieHelmetComboBox.SelectedItem = HelmetType.BareHead;
            this.popoieArmorComboBox.DataSource = Enum.GetValues<ArmorType>();
            this.popoieArmorComboBox.SelectedItem = ArmorType.No;
            this.popoieAccessoryComboBox.DataSource = Enum.GetValues<AccessoryType>();
            this.popoieAccessoryComboBox.SelectedItem = AccessoryType.Nothing;
            this.popoieWeaponComboBox.DataSource = Enum.GetValues<WeaponType>();
            this.popoieWeaponComboBox.SelectedItem = WeaponType.SpikeKnuckle;

            this.EventHandlers = this.LoadEventHandlers();
            this.ignoreEvents = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (int)ExtendedWindowStyles.WS_EX_COMPOSITED;
                return cp;
            }
        }

        private void SetDefaultData()
        {
            this.ignoreEvents = true;
            this.goldNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.Gold;
            this.sealedManaSeedsNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.SealedManaSeeds;
            this.saveLocationNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.SaveLocation;

            this.randiAGQNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.RandiActionGridQuadrant;
            this.purimAGQNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PurimActionGridQuadrant;
            this.popoieAGQNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PopoieActionGridQuadrant;

            this.randiAGLNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.RandiActionGridLocation;
            this.purimAGLNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PurimActionGridLocation;
            this.popoieAGLNumericUpDown.Value = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PopoieActionGridLocation;

            this.randiHelmetComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.RandiHelmet;
            this.randiArmorComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.RandiArmor;
            this.randiAccessoryComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.RandiAccessory;
            this.randiWeaponComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.RandiWeapon;

            this.purimHelmetComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PurimHelmet;
            this.purimArmorComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PurimArmor;
            this.purimAccessoryComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PurimAccessory;
            this.purimWeaponComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PurimWeapon;

            this.popoieHelmetComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PopoieHelmet;
            this.popoieArmorComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PopoieArmor;
            this.popoieAccessoryComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PopoieAccessory;
            this.popoieWeaponComboBox.SelectedItem = ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable.PopoieWeapon;
            this.ignoreEvents = false;
        }

        private IReadOnlyDictionary<string, Action<ManaDefaultCharacterData>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<ManaDefaultCharacterData>>()
            {
                { this.goldNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.Gold = Convert.ToUInt16(this.goldNumericUpDown.Value); } },
                { this.sealedManaSeedsNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.SealedManaSeeds = Convert.ToByte(this.sealedManaSeedsNumericUpDown.Value); } },
                { this.saveLocationNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.SaveLocation = Convert.ToByte(this.saveLocationNumericUpDown.Value); } },

                { this.randiAGQNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.RandiActionGridQuadrant = Convert.ToByte(this.randiAGQNumericUpDown.Value); } },
                { this.purimAGQNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PurimActionGridQuadrant = Convert.ToByte(this.purimAGQNumericUpDown.Value); } },
                { this.popoieAGQNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PopoieActionGridQuadrant = Convert.ToByte(this.popoieAGQNumericUpDown.Value); } },

                { this.randiAGLNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.RandiActionGridLocation = Convert.ToByte(this.randiAGLNumericUpDown.Value); } },
                { this.purimAGLNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PurimActionGridLocation = Convert.ToByte(this.purimAGLNumericUpDown.Value); } },
                { this.popoieAGLNumericUpDown.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PopoieActionGridLocation = Convert.ToByte(this.popoieAGLNumericUpDown.Value); } },

                { this.randiHelmetComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.RandiHelmet = (HelmetType)this.randiHelmetComboBox.SelectedItem; } },
                { this.randiArmorComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.RandiArmor = (ArmorType)this.randiArmorComboBox.SelectedItem; } },
                { this.randiAccessoryComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.RandiAccessory = (AccessoryType)this.randiAccessoryComboBox.SelectedItem; } },
                { this.randiWeaponComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.RandiWeapon = (WeaponType)this.randiWeaponComboBox.SelectedItem; } },

                { this.purimHelmetComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PurimHelmet = (HelmetType)this.purimHelmetComboBox.SelectedItem; } },
                { this.purimArmorComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PurimArmor = (ArmorType)this.purimArmorComboBox.SelectedItem; } },
                { this.purimAccessoryComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PurimAccessory = (AccessoryType)this.purimAccessoryComboBox.SelectedItem; } },
                { this.purimWeaponComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PurimWeapon = (WeaponType)this.purimWeaponComboBox.SelectedItem; } },

                { this.popoieHelmetComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PopoieHelmet = (HelmetType)this.popoieHelmetComboBox.SelectedItem; } },
                { this.popoieArmorComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PopoieArmor = (ArmorType)this.popoieArmorComboBox.SelectedItem; } },
                { this.popoieAccessoryComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PopoieAccessory = (AccessoryType)this.popoieAccessoryComboBox.SelectedItem; } },
                { this.popoieWeaponComboBox.Name, (ManaDefaultCharacterData defaultData) => { defaultData.PopoieWeapon = (WeaponType)this.popoieWeaponComboBox.SelectedItem; } },
            };
        }

        private void DefaultPartyDataEditor_Load(object sender, EventArgs e)
        {
            this.SetDefaultData();
        }

        private void Control_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<ManaDefaultCharacterData>? eventHandler))
                {
                    eventHandler(ManaMagicContext.Current.Context.CharacterContext.DefaultCharacterDataTable);
                }
            }
        }
    }
}