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
    public partial class BossSkillViewerUserControl : UserControl
    {
        private int skillIndex = 0;
        private int frameIndex = 0;
        private bool updatingUI = false;
        private readonly Dictionary<byte, BossSkillPropertyObject> skillPropertyObjectCache = new Dictionary<byte, BossSkillPropertyObject>();
        private readonly Dictionary<(BossFamily, int), BossFramePropertyObject> framePropertyObjectCache = new Dictionary<(BossFamily, int), BossFramePropertyObject>();

        private ManaBossSkill2 activeSkill = ManaBossSkill2.Empty;
        private bool hasFrames = false;

        public BossSkillViewerUserControl()
        {
            InitializeComponent();
            this.paletteSelector1UserControl.PaletteMaximum = (int)(Constants.Bank10.BossPalettePointerTableSize - 1);
            this.paletteSelector1UserControl.SetSelectionState(false);
        }

        private void UpdateUI()
        {
            this.updatingUI = true;

            this.tileSheetPictureBox.Image?.Dispose();
            this.tileSheetPictureBox.Image = null;

            this.framePictureBox.Image?.Dispose();
            this.framePictureBox.Image = null;

            this.skillIndex = Math.Clamp(this.skillIndex, 0, ManaMagicContext.Current.Context.BossContext.Skills.Count - 1);
            this.frameIndex = 0;

            this.activeSkill = ManaMagicContext.Current.Context.BossContext.Skills[this.skillIndex];
            this.hasFrames = this.activeSkill.Tileset.CanDraw && this.activeSkill.Frames.Count > 0;
            this.UpdateFrameUI(true);
            this.paletteSelector1UserControl.SetColorPalette(0, this.activeSkill.Palette.Colors);

            this.previousButton.Enabled = this.skillIndex > 0;
            this.nextButton.Enabled = this.skillIndex < ManaMagicContext.Current.Context.BossContext.Skills.Count - 1;

            this.skillPropertyGrid.SelectedObject = this.GetSkillPropertyObject();

            this.DrawTileset();
            if (this.hasFrames)
            {
                this.DrawFrame();
            }

            this.updatingUI = false;
        }

        private void UpdateFrameUI(bool reset = false)
        {
            if (reset)
            {
                this.framePropertyGrid.Visible = this.hasFrames;
                this.showGridlinesCheckBox.Enabled = this.hasFrames;
            }

            this.previousFrameButton.Enabled = this.frameIndex > 0;
            this.nextFrameButton.Enabled = this.frameIndex < this.activeSkill.Frames.Count - 1;
        }

        private void DrawTileset()
        {
            using SuperNintendoGraphics graphics = this.activeSkill.DrawTileset();

            this.tileSheetPictureBox.Image?.Dispose();
            this.tileSheetPictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        private void DrawFrame()
        {
            FrameDrawingOptions options = FrameDrawingOptions.None;
            if (showGridlinesCheckBox.Checked) { options |= FrameDrawingOptions.ShowGridlines; }
            using SuperNintendoGraphics graphics = this.activeSkill.DrawFrame(this.frameIndex, options);

            this.framePropertyGrid.SelectedObject = this.GetFramePropertyObject();
            this.framePictureBox.Image?.Dispose();
            this.framePictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        private BossSkillPropertyObject GetSkillPropertyObject()
        {
            if (!this.skillPropertyObjectCache.TryGetValue((byte)this.skillIndex, out BossSkillPropertyObject? propertyObject))
            {
                propertyObject = new BossSkillPropertyObject((byte)this.skillIndex, this.activeSkill);
                this.skillPropertyObjectCache.Add((byte)this.skillIndex, propertyObject);
            }
            return propertyObject;
        }

        private BossFramePropertyObject GetFramePropertyObject()
        {
            (BossFamily, int) key = (this.activeSkill.Frames.Family, this.frameIndex);
            if (!this.framePropertyObjectCache.TryGetValue(key, out BossFramePropertyObject? propertyObject))
            {
                BossFrameMetadata metadata = ManaMetadata.BossFrameAddressFactory.GetMetadata(this.activeSkill.Frames.Family, this.frameIndex);

                propertyObject = new BossFramePropertyObject(this.frameIndex, metadata);
                this.framePropertyObjectCache.Add(key, propertyObject);
            }
            return propertyObject;
        }

        private void BossSkillViewerUserControl_Load(object sender, EventArgs e)
        {
            this.UpdateUI();
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            this.skillIndex--;
            this.UpdateUI();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            this.skillIndex++;
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

        private void gridlinesCheckBox_CheckedChanged(object sender, EventArgs e)
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