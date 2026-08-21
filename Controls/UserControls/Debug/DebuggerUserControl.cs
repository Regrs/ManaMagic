using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManaMagic.Core.Debugger;

namespace ManaMagic.Controls.UserControls.Debug
{
    public partial class DebuggerUserControl : UserControl
    {
        public DebuggerUserControl()
        {
            InitializeComponent();
            this.DebugEnableOptions();
        }

        [Conditional("DEBUG")]
        private void DebugEnableOptions()
        {
            this.debugDrawAllButton.Tag = MapDebuggerOption.DrawAllMaps;
            this.debugScanForTileIDButton.Tag = MapDebuggerOption.ScanForTileIndex;
            this.debugPrintSpiteActionsButton.Tag = MapDebuggerOption.PrintSpiteActions;
            this.debugLayerSettingsCountButton.Tag = MapDebuggerOption.CountLayerSettings;
            this.debugSpriteUnknownBitsButton.Tag = MapDebuggerOption.SpriteUnknownBits;
            this.debugMapUnknownByteButton.Tag = MapDebuggerOption.MapUnknownByte;
            this.debugScanForF8FFCommandsButton.Tag = MapDebuggerOption.ScanForF8FFCommands;
            this.debugPrintSpecialItem20Button.Tag = MapDebuggerOption.PrintSpecialItem20;
            this.debugPrintSpecialItem40Button.Tag = MapDebuggerOption.PrintSpecialItem40;

            this.debugPrintDefinitionOffsetsButton.Tag = RingMenuDebuggerOption.PrintDefinitionOffsets;
            this.debugPrintWeaponDefinitionButton.Tag = RingMenuDebuggerOption.PrintWeaponDefinitions;

            this.debugPrintAnimationScriptCommandCountButton.Tag = BossDebuggerOption.PrintAnimationScriptCommandCount;
        }

        private void RunMapDebuggerButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is MapDebuggerOption mapDebuggerOption)
                {
                    ManaDebugger.RunDebugger(mapDebuggerOption);
                }
                else if (button.Tag is RingMenuDebuggerOption ringMenuDebuggerOption)
                {
                    ManaDebugger.RunDebugger(ringMenuDebuggerOption);
                }
                else if (button.Tag is BossDebuggerOption bossDebuggerOption)
                {
                    ManaDebugger.RunDebugger(bossDebuggerOption);
                }
            }
        }
    }
}
