using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Maps;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class DisplaySettingsUserControl : UserControl, IManaControl
    {
        private readonly IReadOnlyDictionary<string, Action<MapDisplaySettings>> EventHandlers;
        private MapDisplaySettings? settings = null;
        private bool ignoreEvents = false;

        public DisplaySettingsUserControl()
        {
            InitializeComponent();

            this.indexNumericUpDown.Maximum = Constants.Bank08.MapDisplaySettingsTableSize - 1;
            this.mosaicSizeComboBox.DataSource = Enum.GetValues<MosaicRegisterSize>();
            this.mosaicSizeComboBox.SelectedItem = MosaicRegisterSize.Size1x1;

            this.EventHandlers = this.LoadEventHandlers();
        }

        public void SetIndex(int index)
        {
            this.indexNumericUpDown.Value = index;
        }

        private void SetDebugControls()
        {
            if (this.settings != null)
            {
                this.mosaicSettingsNumericUpDown.Value = this.settings.MosaicSettings;
                this.mainScreenLayersEnabledNumericUpDown.Value = (byte)this.settings.MainScreenLayersEnabled;
                this.subScreenLayersEnabledNumericUpDown.Value = (byte)this.settings.SubScreenLayersEnabled;
                this.colorMathNumericUpDown.Value = (byte)this.settings.ColorMath;
                this.unusedValue01NumericUpDown.Value = this.settings.UnusedValue01;
                this.unusedValue02NumericUpDown.Value = this.settings.UnusedValue02;
                this.unusedValue03NumericUpDown.Value = this.settings.UnusedValue03;
                this.backgroundColorNumericUpDown.Value = this.settings.BackgroundColor.ToRgb555(Rgb555Format.BGR);
            }
        }

        private void SetDisplaySettings(int index)
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                MapDisplaySettings settings = ManaMagicContext.Current.Context.MapContext.DisplaySettingsTable[index];
                this.SetDisplaySettings(settings);
            }
        }

        private void SetDisplaySettings(MapDisplaySettings settings)
        {
            this.ignoreEvents = true;
            this.settings = settings;

            this.mosaicSizeComboBox.SelectedItem = this.settings.MosaicSize;
            this.enableMosaicOnBG01CheckBox.Checked = this.settings.EnableOnBackground01;
            this.enableMosaicOnBG02CheckBox.Checked = this.settings.EnableOnBackground02;
            this.enableMosaicOnBG03CheckBox.Checked = this.settings.EnableOnBackground03;
            this.enableMosaicOnBG04CheckBox.Checked = this.settings.EnableOnBackground04;

            this.mainScreenEnableBG01CheckBox.Checked = this.settings.MainScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground01);
            this.mainScreenEnableBG02CheckBox.Checked = this.settings.MainScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground02);
            this.mainScreenEnableBG03CheckBox.Checked = this.settings.MainScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground03);
            this.mainScreenEnableBG04CheckBox.Checked = this.settings.MainScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground04);
            this.mainScreenEnableObjectsCheckBox.Checked = this.settings.MainScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableObjects);

            this.subScreenEnableBG01CheckBox.Checked = this.settings.SubScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground01);
            this.subScreenEnableBG02CheckBox.Checked = this.settings.SubScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground02);
            this.subScreenEnableBG03CheckBox.Checked = this.settings.SubScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground03);
            this.subScreenEnableBG04CheckBox.Checked = this.settings.SubScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableBackground04);
            this.subScreenEnableObjectsCheckBox.Checked = this.settings.SubScreenLayersEnabled.HasFlag(ScreenLayerEnableRegister.EnableObjects);

            this.colorMathEnableBG01CheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.EnableBackground01);
            this.colorMathEnableBG02CheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.EnableBackground02);
            this.colorMathEnableBG03CheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.EnableBackground03);
            this.colorMathEnableBG04CheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.EnableBackground04);
            this.colorMathEnableObjectsCheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.EnableObjects);
            this.colorMathEnableBackdropCheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.EnableBackdrop);
            this.colorMathHalfColorMathCheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.HalfColorMath);
            this.colorMathSubtractiveCheckBox.Checked = this.settings.ColorMath.HasFlag(CGADSUB.Subtractive);

            this.backgroundColorLabel.BackColor = Color.FromArgb(this.settings.BackgroundColor.ToArgb());

            this.SetDebugControls();
            this.ignoreEvents = false;
        }

        private IReadOnlyDictionary<string, Action<MapDisplaySettings>> LoadEventHandlers()
        {
            return new Dictionary<string, Action<MapDisplaySettings>>()
            {
                { this.enableMosaicOnBG01CheckBox.Name, (MapDisplaySettings settings) => { settings.EnableOnBackground01 = this.enableMosaicOnBG01CheckBox.Checked; } },
                { this.enableMosaicOnBG02CheckBox.Name, (MapDisplaySettings settings) => { settings.EnableOnBackground02 = this.enableMosaicOnBG02CheckBox.Checked; } },
                { this.enableMosaicOnBG03CheckBox.Name, (MapDisplaySettings settings) => { settings.EnableOnBackground03 = this.enableMosaicOnBG03CheckBox.Checked; } },
                { this.enableMosaicOnBG04CheckBox.Name, (MapDisplaySettings settings) => { settings.EnableOnBackground04 = this.enableMosaicOnBG04CheckBox.Checked; } },
                { this.mosaicSizeComboBox.Name, (MapDisplaySettings settings) => { settings.MosaicSize = (MosaicRegisterSize)this.mosaicSizeComboBox.SelectedItem; } },

                { this.mainScreenEnableBG01CheckBox.Name, (MapDisplaySettings settings) => { settings.MainScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.MainScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground01, this.mainScreenEnableBG01CheckBox.Checked); } },
                { this.mainScreenEnableBG02CheckBox.Name, (MapDisplaySettings settings) => { settings.MainScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.MainScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground02, this.mainScreenEnableBG02CheckBox.Checked); } },
                { this.mainScreenEnableBG03CheckBox.Name, (MapDisplaySettings settings) => { settings.MainScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.MainScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground03, this.mainScreenEnableBG03CheckBox.Checked); } },
                { this.mainScreenEnableBG04CheckBox.Name, (MapDisplaySettings settings) => { settings.MainScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.MainScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground04, this.mainScreenEnableBG04CheckBox.Checked); } },
                { this.mainScreenEnableObjectsCheckBox.Name, (MapDisplaySettings settings) => { settings.MainScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.MainScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableObjects, this.mainScreenEnableObjectsCheckBox.Checked); } },

                { this.subScreenEnableBG01CheckBox.Name, (MapDisplaySettings settings) => { settings.SubScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.SubScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground01, this.subScreenEnableBG01CheckBox.Checked); } },
                { this.subScreenEnableBG02CheckBox.Name, (MapDisplaySettings settings) => { settings.SubScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.SubScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground02, this.subScreenEnableBG02CheckBox.Checked); } },
                { this.subScreenEnableBG03CheckBox.Name, (MapDisplaySettings settings) => { settings.SubScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.SubScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground03, this.subScreenEnableBG03CheckBox.Checked); } },
                { this.subScreenEnableBG04CheckBox.Name, (MapDisplaySettings settings) => { settings.SubScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.SubScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableBackground04, this.subScreenEnableBG04CheckBox.Checked); } },
                { this.subScreenEnableObjectsCheckBox.Name, (MapDisplaySettings settings) => { settings.SubScreenLayersEnabled = (ScreenLayerEnableRegister)DisplaySettingsUserControl.SetFlagState((byte)settings.SubScreenLayersEnabled, (byte)ScreenLayerEnableRegister.EnableObjects, this.subScreenEnableObjectsCheckBox.Checked); } },

                { this.colorMathEnableBG01CheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.EnableBackground01, this.colorMathEnableBG01CheckBox.Checked); } },
                { this.colorMathEnableBG02CheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.EnableBackground02, this.colorMathEnableBG02CheckBox.Checked); } },
                { this.colorMathEnableBG03CheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.EnableBackground03, this.colorMathEnableBG03CheckBox.Checked); } },
                { this.colorMathEnableBG04CheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.EnableBackground04, this.colorMathEnableBG04CheckBox.Checked); } },
                { this.colorMathEnableObjectsCheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.EnableObjects, this.colorMathEnableObjectsCheckBox.Checked); } },
                { this.colorMathEnableBackdropCheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.EnableBackdrop, this.colorMathEnableBackdropCheckBox.Checked); } },
                { this.colorMathHalfColorMathCheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.HalfColorMath, this.colorMathHalfColorMathCheckBox.Checked); } },
                { this.colorMathSubtractiveCheckBox.Name, (MapDisplaySettings settings) => { settings.ColorMath = (CGADSUB)DisplaySettingsUserControl.SetFlagState((byte)settings.ColorMath, (byte)CGADSUB.Subtractive, this.colorMathSubtractiveCheckBox.Checked); } },
            };
        }

        private void DisplaySettingsUserControl_Load(object sender, EventArgs e)
        {
            this.rawValuesGroupBox.Visible = ManaMagicContext.DebugMode;
            this.SetDisplaySettings(0);
        }

        private void IndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetDisplaySettings((int)this.indexNumericUpDown.Value);
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            if (!this.ignoreEvents && this.settings != null && sender is Control control)
            {
                if (this.EventHandlers.TryGetValue(control.Name, out Action<MapDisplaySettings>? eventHandler))
                {
                    eventHandler(this.settings);
                }
            }
        }

        private void backgroundColorLabel_Click(object sender, EventArgs e)
        {
            if (!this.ignoreEvents && this.settings != null)
            {
                using ColorDialog colorDialog = new ColorDialog();
                colorDialog.FullOpen = true;
                colorDialog.SolidColorOnly = true;
                colorDialog.Color = Color.FromArgb(this.settings.BackgroundColor.ToArgb());
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Rgb555Color rgb555Color = Rgb555Color.FromArgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                    this.settings.BackgroundColor = rgb555Color;
                    this.backgroundColorLabel.BackColor = Color.FromArgb(this.settings.BackgroundColor.ToArgb());
                }
            }
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
}