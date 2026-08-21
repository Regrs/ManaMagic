using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Bosses;
using ZwellTech.SuperNintendo;

#nullable enable

namespace SoMBossAIPatcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponent();
        }

        private BossAIUpgradeOptions CreateOptions()
        {
            BossAIUpgradeOptions options = new BossAIUpgradeOptions();

            options.AegagropilonOptions.EnableDevourAttackFix = this.enableDevourAttackFixCheckBox.Checked;
            options.AegagropilonOptions.ImprovedDevourTargeting = this.improvedDevourTargetingCheckBox.Checked;
            options.AegagropilonOptions.AlternateDevourEatAnimation = this.alternateDevourEatAnimationCheckBox.Checked;
            options.AegagropilonOptions.BallFormAlwaysAttacks = this.ballFormAlwaysAttacksCheckBox.Checked;

            options.BlueDragonOptions.IncreasedAttackRange = this.blueDragonIncreasedAttackRangeCheckBox.Checked;

            options.BramblerOptions.OutOfBoundsAndCountFix = this.outOfBoundsAndCountFixCheckBox.Checked;
            options.BramblerOptions.WeaponFix = this.bramblerWeaponFixCheckBox.Checked;

            options.DarkLichOptions.HatesHeavyMetalMusic = this.hatesHeavyMetalMusicCheckBox.Checked;
            options.DarkLichOptions.UndergroundHorizontalMovement = this.undergroundHorizontalMovementCheckBox.Checked;
            options.DarkLichOptions.UseSuperMagic = this.darkLichUseSuperMagicCheckBox.Checked;
            options.DarkLichOptions.SuperMagicRate = (byte)this.darkLichSuperMagicRateNumericUpDown.Value;

            options.DoomsWallOptions.CaveInNameFix = this.caveInSkillNameFix.Checked;

            options.DragonAllOptions.SkillTargetingFix = this.dragonSkillTargetingFixCheckBox.Checked;

            options.HexasOptions.ImprovedBarrierChange = this.improvedBarrierChangeCheckBox.Checked;
            options.HexasOptions.BarrierChangeShade = this.barrierChangeShadeCheckBox.Checked;
            options.HexasOptions.UseSuperMagic = this.hexasUseSuperMagicCheckBox.Checked;
            options.HexasOptions.SuperMagicRate = (byte)this.hexasSuperMagicRateNumericUpDown.Value;
            options.HexasOptions.InfiniteMP = this.hexasInfiniteMPCheckBox.Checked;
            options.HexasOptions.LunaMoogleBubbles = this.lunaMoogleBubblesCheckBox.Checked;
            options.HexasOptions.SpawnElementFix = this.spawnElementFixCheckBox.Checked;

            options.KettleKinOptions.RestoreDeathMachine = this.restoreDeathMachineCheckBox.Checked;
            options.KettleKinOptions.DrillSpinsDuringMovement = this.drillSpinsDuringMovementCheckBox.Checked;

            options.MechRiderAllOptions.AILocksOnDamageFix = this.aILocksOnDamageFixCheckBox.Checked;
            options.MechRiderAllOptions.TargetAlignmentDoesntLockAI = this.targetAlignmentDoesntLockAICheckBox.Checked;
            options.MechRiderAllOptions.DoubleVerticalMovement = this.doubleVerticalMovementCheckBox.Checked;

            options.MechRiderIIOptions.IncreasedStatistics = this.mechRiderIIncreasedStatisticsCheckBox.Checked;

            options.MechRiderIOptions.IncreasedStatistics = this.mechRiderIIIncreasedStatisticsCheckBox.Checked;

            options.MechRiderIIIOptions.SpellAndSkillUseFix = this.spellAndSkillUseFixCheckBox.Checked;
            options.MechRiderIIIOptions.DiffuserCannonTargetingFix = this.diffuserCannonTargetingFixCheckBox.Checked;
            options.MechRiderIIIOptions.IncreasedDiffuserCannonDamage = this.increasedDiffuserCannonDamageCheckBox.Checked;
            options.MechRiderIIIOptions.IncreasedStatistics = this.mechRiderIIIIncreasedStatisticsCheckBox.Checked;

            options.MetalMantisOptions.UsefulWeapons = this.metalMantisUsefulWeaponsCheckBox.Checked;
            options.MetalMantisOptions.IncreasedFireBeamDamage = this.metalMantisIncreaseFireBeamDamageCheckBox.Checked;
            options.MetalMantisOptions.HasImprovedAcidBreathCommand = this.hasImprovedAcidBreathCommandCheckBox.Checked;

            options.RedDragonOptions.IncreasedAttackRange = this.redDragonIncreasedAttackRangeCheckBox.Checked;
            options.RedDragonOptions.ImprovedSleepRing = this.redDragonImprovedSleepRingCheckBox.Checked;

            options.SnowDragonOptions.NewFrostWingSkill = this.newFrostWingSkillCheckBox.Checked;
            options.SnowDragonOptions.IncreasedAttackRange = this.snowDragonIncreasedAttackRangeCheckBox.Checked;

            options.TropicalloOptions.InfiniteBramblerSpawns = this.infiniteBramblerSpawnsCheckBox.Checked;
            options.TropicalloOptions.MaxBramblerSpawns = (ushort)this.maxBramblerSpawnsNumericUpDown.Value;

            return options;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            this.usRomFilePathTextBox.Text = @"D:\SOM Stuff\Secret of Mana (USA).sfc";
            this.jpRomFilePathTextBox.Text = @"D:\SOM Stuff\Seiken Densetsu 2 (Japan).sfc";
#endif
        }

        private void GenerateROMFileButton_Click(object sender, EventArgs e)
        {
            this.optionsPanel.Enabled = false;

            string usFilePath = this.usRomFilePathTextBox.Text;
            string jpFilePath = this.jpRomFilePathTextBox.Text;
            bool extendRom = this.extendRomCheckBox.Checked;
            byte dataBank = (byte)this.bossDataBankNumericUpDown.Value;

            BossAIUpgradeOptions options = this.CreateOptions();

            RomFile romFile = new RomFile(usFilePath, SecretOfManaEncoding.English);
            BossAIUpgradeWriter upgradeWriter = new BossAIUpgradeWriter(romFile, extendRom);
            upgradeWriter.BossAIAuxiliaryBank = dataBank;
            upgradeWriter.JapaneseRomFilePath = jpFilePath;
            upgradeWriter.Run(options);

#if DEBUG
            upgradeWriter.DebugSaveToFile(options);
#else
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select path to save the modified ROM file...";
            fbd.UseDescriptionForTitle = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                upgradeWriter.SaveToFile(fbd.SelectedPath, options);
            }
#endif

            this.optionsPanel.Enabled = true;
        }

        private void EnableDevourAttackFixCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.improvedDevourTargetingCheckBox.Enabled = this.enableDevourAttackFixCheckBox.Checked;
            this.alternateDevourEatAnimationCheckBox.Enabled = this.enableDevourAttackFixCheckBox.Checked;
        }

        private void DarkLichUseSuperMagicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.darkLichSuperMagicRateLabel.Enabled = this.darkLichUseSuperMagicCheckBox.Checked;
            this.darkLichSuperMagicRateNumericUpDown.Enabled = this.darkLichUseSuperMagicCheckBox.Checked;
        }

        private void HexasUseSuperMagicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.hexasSuperMagicRateLabel.Enabled = this.hexasUseSuperMagicCheckBox.Checked;
            this.hexasSuperMagicRateNumericUpDown.Enabled = this.hexasUseSuperMagicCheckBox.Checked;
        }

        private void ImprovedBarrierChangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.barrierChangeShadeCheckBox.Enabled = this.improvedBarrierChangeCheckBox.Checked;
        }

        private void RestoreDeathMachineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.drillSpinsDuringMovementCheckBox.Enabled = this.restoreDeathMachineCheckBox.Checked;
        }

        private void InfiniteBramblerSpawnsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.maxBramblerSpawnsLabel.Enabled = !this.infiniteBramblerSpawnsCheckBox.Checked;
            this.maxBramblerSpawnsNumericUpDown.Enabled = !this.infiniteBramblerSpawnsCheckBox.Checked;
        }

        private void USRomFilePathTextBox_TextChanged(object sender, EventArgs e)
        {
            bool enabled = !string.IsNullOrWhiteSpace(this.usRomFilePathTextBox.Text);
            this.generateROMFileButton.Enabled = enabled;
        }

        private void JPRomFilePathTextBox_TextChanged(object sender, EventArgs e)
        {
            bool enabled = !string.IsNullOrWhiteSpace(this.jpRomFilePathTextBox.Text);
            this.restoreDeathMachineCheckBox.Enabled = enabled;
            this.drillSpinsDuringMovementCheckBox.Enabled = enabled;
            this.restoreDeathMachineCheckBox.Checked = enabled;
            this.drillSpinsDuringMovementCheckBox.Checked = enabled;
        }

        private void USRomBrowseButton_Click(object sender, EventArgs e)
        {
            if (MainForm.TryPromptForFilePath(out string filePath))
            {
                this.usRomFilePathTextBox.Text = filePath;
            }
        }

        private void JPRomBrowseButton_Click(object sender, EventArgs e)
        {
            if (MainForm.TryPromptForFilePath(out string filePath))
            {
                this.jpRomFilePathTextBox.Text = filePath;
            }
        }

        private static bool TryPromptForFilePath(out string filePath)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select The Secret Of Mana ROM File To Edit...";
            //ofd.Filter = "Headerless ROMs (*.sfc) | *.sfc | Headered ROMs (*.smc) | *.smc";
            ofd.Filter = "ROMs (*.sfc;*.smc) | *.sfc; *.smc";

            filePath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                return true;
            }
            return false;
        }
    }
}