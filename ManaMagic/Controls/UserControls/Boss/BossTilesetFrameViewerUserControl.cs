using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Bosses.Frames;
using ManaMagic.Core.Metadata;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable
#pragma warning disable IDE1006 // Naming Styles

namespace ManaMagic.Controls.UserControls.Boss
{
    public partial class BossTilesetFrameViewerUserControl : UserControl
    {
        private int tilesetIndex = 0;//3;//18;//7;
        private int frameIndex = 0;//0x0C;
        private bool updatingUI = false;
        private readonly Dictionary<byte, BossGraphicsTablePropertyObject> tilesetPropertyObjectCache = new Dictionary<byte, BossGraphicsTablePropertyObject>();
        private readonly Dictionary<(BossFamily, int), BossFramePropertyObject> framePropertyObjectCache = new Dictionary<(BossFamily, int), BossFramePropertyObject>();

        private BossTilesetMetadata activeMetadata = BossTilesetMetadata.Empty;
        private BossTileset activeTileset = BossTileset.Empty;
        private BossFrameList activeFrameList = BossFrameList.Empty;
        private int primaryPaletteId = 0xFF;
        private int secondaryPaletteId = 0xFF;
        private int tertiaryPaletteId = 0xFF;
        private bool hasFrames = false;

        public BossTilesetFrameViewerUserControl()
        {
            InitializeComponent();
            this.paletteSelector1UserControl.PaletteMaximum = (int)(Constants.Bank10.BossPalettePointerTableSize - 1);
            this.paletteSelector2UserControl.PaletteMaximum = (int)(Constants.Bank10.BossPalettePointerTableSize - 1);
            this.paletteSelector3UserControl.PaletteMaximum = (int)(Constants.Bank10.BossPalettePointerTableSize - 1);


            this.hitBoxXOffsetNumericUpDown.Minimum = sbyte.MinValue;
            this.hitBoxXOffsetNumericUpDown.Maximum = sbyte.MaxValue;

            this.hitBoxYOffsetNumericUpDown.Minimum = sbyte.MinValue;
            this.hitBoxYOffsetNumericUpDown.Maximum = sbyte.MaxValue;

            this.hitBoxWidthNumericUpDown.Minimum = byte.MinValue;
            this.hitBoxWidthNumericUpDown.Maximum = byte.MaxValue;

            this.hitBoxHeightNumericUpDown.Minimum = byte.MinValue;
            this.hitBoxHeightNumericUpDown.Maximum = byte.MaxValue;

            ///-------------------------------------------------------------
            this.weaponBoxXOffsetNumericUpDown.Minimum = sbyte.MinValue;
            this.weaponBoxXOffsetNumericUpDown.Maximum = sbyte.MaxValue;

            this.weaponBoxYOffsetNumericUpDown.Minimum = sbyte.MinValue;
            this.weaponBoxYOffsetNumericUpDown.Maximum = sbyte.MaxValue;

            this.weaponBoxWidthNumericUpDown.Minimum = byte.MinValue;
            this.weaponBoxWidthNumericUpDown.Maximum = byte.MaxValue;

            this.weaponBoxHeightNumericUpDown.Minimum = byte.MinValue;
            this.weaponBoxHeightNumericUpDown.Maximum = byte.MaxValue;

            ///-------------------------------------------------------------
            this.guardBoxXOffsetNumericUpDown.Minimum = sbyte.MinValue;
            this.guardBoxXOffsetNumericUpDown.Maximum = sbyte.MaxValue;

            this.guardBoxYOffsetNumericUpDown.Minimum = sbyte.MinValue;
            this.guardBoxYOffsetNumericUpDown.Maximum = sbyte.MaxValue;

            this.guardBoxWidthNumericUpDown.Minimum = byte.MinValue;
            this.guardBoxWidthNumericUpDown.Maximum = byte.MaxValue;

            this.guardBoxHeightNumericUpDown.Minimum = byte.MinValue;
            this.guardBoxHeightNumericUpDown.Maximum = byte.MaxValue;
        }

        private void UpdateUI()
        {
            this.updatingUI = true;

            this.tileSheetPictureBox.Image?.Dispose();
            this.tileSheetPictureBox.Image = null;

            this.framePictureBox.Image?.Dispose();
            this.framePictureBox.Image = null;

            this.ResetActiveVariables();
            this.UpdateFrameUI(true);
            this.ResetPaletteSelectors();

            this.previousButton.Enabled = this.tilesetIndex > 0;
            this.nextButton.Enabled = this.tilesetIndex < ManaMagicContext.Current.Context.BossContext.Tilesets.Count - 1;

            this.tilesetPropertyGrid.SelectedObject = this.GetTilesetPropertyObject();
            this.auxiliaryTilesetGroupBox.Visible = this.activeMetadata.SwitchableAuxiliaryTileset;

            this.DrawTileset();
            if (this.hasFrames)
            {
                this.DrawFrame();
            }
            this.SetHitBoxUI();

            this.updatingUI = false;
        }

        private void SetHitBoxUI()
        {
            if (this.hasFrames && ManaMetadata.BossFrameAddressFactory.FrameHasHitBoxData(activeMetadata.Family, this.frameIndex))
            {
                BossFrame currentFrame = this.activeFrameList[this.frameIndex];

                SetHitBoxNumericUpDownS(this.hitBoxXOffsetNumericUpDown, currentFrame.HitBox.XOffset);
                SetHitBoxNumericUpDownS(this.hitBoxYOffsetNumericUpDown, currentFrame.HitBox.YOffset);
                SetHitBoxNumericUpDownU(this.hitBoxWidthNumericUpDown, currentFrame.HitBox.Width);
                SetHitBoxNumericUpDownU(this.hitBoxHeightNumericUpDown, currentFrame.HitBox.Height);

                SetHitBoxNumericUpDownS(this.weaponBoxXOffsetNumericUpDown, currentFrame.WeaponBox.XOffset);
                SetHitBoxNumericUpDownS(this.weaponBoxYOffsetNumericUpDown, currentFrame.WeaponBox.YOffset);
                SetHitBoxNumericUpDownU(this.weaponBoxWidthNumericUpDown, currentFrame.WeaponBox.Width);
                SetHitBoxNumericUpDownU(this.weaponBoxHeightNumericUpDown, currentFrame.WeaponBox.Height);

                SetHitBoxNumericUpDownS(this.guardBoxXOffsetNumericUpDown, currentFrame.GuardBox.XOffset);
                SetHitBoxNumericUpDownS(this.guardBoxYOffsetNumericUpDown, currentFrame.GuardBox.YOffset);
                SetHitBoxNumericUpDownU(this.guardBoxWidthNumericUpDown, currentFrame.GuardBox.Width);
                SetHitBoxNumericUpDownU(this.guardBoxHeightNumericUpDown, currentFrame.GuardBox.Height);

                this.hitBoxGroupBox.Enabled = true;
                this.weaponBoxGroupBox.Enabled = true;
                this.guardBoxGroupBox.Enabled = true;
            }
            else
            {
                SetHitBoxNumericUpDownS(this.hitBoxXOffsetNumericUpDown, 0);
                SetHitBoxNumericUpDownS(this.hitBoxYOffsetNumericUpDown, 0);
                SetHitBoxNumericUpDownU(this.hitBoxWidthNumericUpDown, 0);
                SetHitBoxNumericUpDownU(this.hitBoxHeightNumericUpDown, 0);

                SetHitBoxNumericUpDownS(this.weaponBoxXOffsetNumericUpDown, 0);
                SetHitBoxNumericUpDownS(this.weaponBoxYOffsetNumericUpDown, 0);
                SetHitBoxNumericUpDownU(this.weaponBoxWidthNumericUpDown, 0);
                SetHitBoxNumericUpDownU(this.weaponBoxHeightNumericUpDown, 0);

                SetHitBoxNumericUpDownS(this.guardBoxXOffsetNumericUpDown, 0);
                SetHitBoxNumericUpDownS(this.guardBoxYOffsetNumericUpDown, 0);
                SetHitBoxNumericUpDownU(this.guardBoxWidthNumericUpDown, 0);
                SetHitBoxNumericUpDownU(this.guardBoxHeightNumericUpDown, 0);

                hitBoxGroupBox.Enabled = false;
                weaponBoxGroupBox.Enabled = false;
                guardBoxGroupBox.Enabled = false;
            }

            static void SetHitBoxNumericUpDownU(NumericUpDown numericUpDown, byte value)
            {
                numericUpDown.Minimum = value;
                numericUpDown.Maximum = value;
                numericUpDown.Value = value;
            }

            static void SetHitBoxNumericUpDownS(NumericUpDown numericUpDown, sbyte value)
            {
                numericUpDown.Minimum = value;
                numericUpDown.Maximum = value;
                numericUpDown.Value = value;
            }
        }

        private void ResetActiveVariables()
        {
            this.tilesetIndex = Math.Clamp(this.tilesetIndex, 0, ManaMagicContext.Current.Context.BossContext.Tilesets.Count - 1);
            this.frameIndex = 00;// x0C;

            this.activeMetadata = ManaMetadata.BossTilesetMetadata[this.tilesetIndex];
            this.activeTileset = (BossTileset)ManaMagicContext.Current.Context.BossContext.Tilesets[(byte)this.tilesetIndex];
            this.primaryPaletteId = this.activeMetadata.PaletteIds.Primary;
            this.secondaryPaletteId = this.activeMetadata.PaletteIds.Secondary;
            this.tertiaryPaletteId = this.activeMetadata.PaletteIds.Tertiary;
            this.hasFrames = ManaMetadata.BossFrameAddressFactory.HasFrames(this.activeMetadata.Family);// && !this.currentMetadata.IsDummiedOut
            this.activeFrameList = this.hasFrames ? ManaMagicContext.Current.Context.BossContext.FrameDictionary[this.activeMetadata.Family] : BossFrameList.Empty;
        }

        private void ResetPaletteSelectors()
        {
            this.paletteSelector2UserControl.Visible = false;
            this.paletteSelector3UserControl.Visible = false;

            this.paletteSelector1UserControl.SetColorPalette(this.primaryPaletteId, ManaMagicContext.Current.Context.BossContext.PaletteTable[this.primaryPaletteId].Colors);
            if (this.secondaryPaletteId != 0xFF)
            {
                this.paletteSelector2UserControl.Visible = true;
                this.paletteSelector2UserControl.SetColorPalette(this.secondaryPaletteId, ManaMagicContext.Current.Context.BossContext.PaletteTable[this.secondaryPaletteId].Colors);
            }
            if (this.tertiaryPaletteId != 0xFF)
            {
                this.paletteSelector3UserControl.Visible = true;
                this.paletteSelector3UserControl.SetColorPalette(this.tertiaryPaletteId, ManaMagicContext.Current.Context.BossContext.PaletteTable[this.tertiaryPaletteId].Colors);
            }
        }

        private void UpdateFrameUI(bool reset = false)
        {
            if (reset)
            {
                this.primaryRadioButton.Checked = true;
                this.framePropertyGrid.Visible = this.hasFrames;
                this.showGridlinesCheckBox.Enabled = this.hasFrames;
            }
            this.SetHitBoxUI();

            this.previousFrameButton.Enabled = this.frameIndex > 0;
            this.nextFrameButton.Enabled = this.frameIndex < ManaMetadata.BossFrameAddressFactory.GetFrameCount(this.activeMetadata.Family) - 1;
        }

        private void DrawTileset()
        {
            SpritePalette palette = !this.activeMetadata.IsMode7 ? ManaMagicContext.Current.Context.BossContext.PaletteTable[this.primaryPaletteId] : 
                                                                   ManaMetadata.GetMode7BossPalette(this.tilesetIndex, ManaMagicContext.Current.Context.BossContext.PaletteTable);
            if (this.activeTileset.CanDraw)
            {
                using SuperNintendoGraphics graphics = this.activeTileset.DrawTileset(palette);

                this.tileSheetPictureBox.Image?.Dispose();
                this.tileSheetPictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
                return;
            }
            this.tileSheetPictureBox.Image = new Bitmap(1, 1);
        }

        private void DrawFrame()
        {
            FrameDrawingOptions options = FrameDrawingOptions.None;
            if (showGridlinesCheckBox.Checked) { options |= FrameDrawingOptions.ShowGridlines; }
            if (showHitBoxCheckBox.Checked) { options |= FrameDrawingOptions.ShowHitBox; }
            if (showWeaponBoxCheckBox.Checked) { options |= FrameDrawingOptions.ShowWeaponBox; }
            if (showGuardBoxCheckBox.Checked) { options |= FrameDrawingOptions.ShowGuardBox; }

            bool useSecondaryAuxTileset = !this.primaryRadioButton.Checked;
            List<SpritePalette> paletteTable = GetPaletteTableForFrame();

            BossTileset tileset = ManaMagicContext.Current.Context.BossContext.GetMergedBossTileset((byte)this.tilesetIndex, useSecondaryAuxTileset);
            using SuperNintendoGraphics graphics = this.activeFrameList.DrawFrame(this.frameIndex, tileset, paletteTable, options);

            this.framePropertyGrid.SelectedObject = this.GetFramePropertyObject();
            this.framePictureBox.Image?.Dispose();
            this.framePictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);

            List<SpritePalette> GetPaletteTableForFrame()
            {
                List<SpritePalette> paletteTable = new List<SpritePalette>();
                if (this.activeMetadata.IsMode7)
                {
                    SpritePalette mode7Palette = ManaMetadata.GetMode7BossPalette(this.tilesetIndex, ManaMagicContext.Current.Context.BossContext.PaletteTable);
                    paletteTable.Add(mode7Palette);

                    return paletteTable;
                }

                paletteTable.Add(ManaMagicContext.Current.Context.BossContext.PaletteTable[this.primaryPaletteId]);
                if (this.secondaryPaletteId != 0xFF) { paletteTable.Add(ManaMagicContext.Current.Context.BossContext.PaletteTable[this.secondaryPaletteId]); }
                if (this.tertiaryPaletteId != 0xFF) { paletteTable.Add(ManaMagicContext.Current.Context.BossContext.PaletteTable[this.tertiaryPaletteId]); }
                return paletteTable;
            }
        }

        private BossGraphicsTablePropertyObject GetTilesetPropertyObject()
        {
            if (!this.tilesetPropertyObjectCache.TryGetValue((byte)this.tilesetIndex, out BossGraphicsTablePropertyObject? propertyObject))
            {
                BossGraphicsTableEntry row = ManaMagicContext.Current.Context.BossContext.GraphicsTable[this.tilesetIndex];
                BossTilesetMetadata metadata = ManaMetadata.BossTilesetMetadata[this.tilesetIndex];

                propertyObject = new BossGraphicsTablePropertyObject((byte)this.tilesetIndex, row, metadata);
                this.tilesetPropertyObjectCache.Add((byte)this.tilesetIndex, propertyObject);
            }
            return propertyObject;
        }

        private BossFramePropertyObject GetFramePropertyObject()
        {
            (BossFamily, int) key = (this.activeMetadata.Family, this.frameIndex);
            if (!this.framePropertyObjectCache.TryGetValue(key, out BossFramePropertyObject? propertyObject))
            {
                BossFrameMetadata metadata = ManaMetadata.BossFrameAddressFactory.GetMetadata(this.activeMetadata.Family, this.frameIndex);

                propertyObject = new BossFramePropertyObject(this.frameIndex, metadata);
                this.framePropertyObjectCache.Add(key, propertyObject);
            }
            return propertyObject;
        }

        private void BossViewerUserControl_Load(object sender, EventArgs e)
        {
            this.UpdateUI();
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            this.tilesetIndex--;
            this.UpdateUI();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            this.tilesetIndex++;
            this.UpdateUI();
        }

        private void previousFrameButton_Click(object sender, EventArgs e)
        {
            this.frameIndex--;
            this.DrawFrame();
            this.UpdateFrameUI();
        }

        private void nextFrameButton_Click(object sender, EventArgs e)
        {
            this.frameIndex++;
            this.DrawFrame();
            this.UpdateFrameUI();
        }

        private void paletteSelector1UserControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.DesignMode && !this.updatingUI)
            {
                if (sender is PaletteSelectorUserControl paletteSelector)
                {
                    this.primaryPaletteId = paletteSelector.SelectedIndex;
                    this.DrawTileset();
                    this.DrawFrame();
                }
            }
        }

        private void paletteSelector2UserControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.DesignMode && !this.updatingUI)
            {
                if (sender is PaletteSelectorUserControl paletteSelector)
                {
                    this.secondaryPaletteId = paletteSelector.SelectedIndex;
                    this.DrawTileset();
                    this.DrawFrame();
                }
            }
        }

        private void paletteSelector3UserControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.DesignMode && !this.updatingUI)
            {
                if (sender is PaletteSelectorUserControl paletteSelector)
                {
                    this.tertiaryPaletteId = paletteSelector.SelectedIndex;
                    this.DrawTileset();
                    this.DrawFrame();
                }
            }
        }

        private void FrameDrawingOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.DrawFrame();
            }
        }

        private void primaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.DrawFrame();
            }
        }

        private void secondaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updatingUI)
            {
                this.DrawFrame();
            }
        }
    }
}