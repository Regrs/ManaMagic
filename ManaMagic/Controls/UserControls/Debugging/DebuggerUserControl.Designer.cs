
namespace ManaMagic.Controls.UserControls.Debug
{
    partial class DebuggerUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.debugScanForF8FFCommandsButton = new System.Windows.Forms.Button();
            this.debugMapUnknownByteButton = new System.Windows.Forms.Button();
            this.debugSpriteUnknownBitsButton = new System.Windows.Forms.Button();
            this.debugLayerSettingsCountButton = new System.Windows.Forms.Button();
            this.debugPrintSpiteActionsButton = new System.Windows.Forms.Button();
            this.debugScanForTileIDButton = new System.Windows.Forms.Button();
            this.debugDrawAllButton = new System.Windows.Forms.Button();
            this.debugPrintDefinitionOffsetsButton = new System.Windows.Forms.Button();
            this.debugPrintWeaponDefinitionButton = new System.Windows.Forms.Button();
            this.debugPrintSpecialItem40Button = new System.Windows.Forms.Button();
            this.debugPrintSpecialItem20Button = new System.Windows.Forms.Button();
            this.debugPrintAnimationScriptCommandCountButton = new System.Windows.Forms.Button();
            this.debugScanForDisplaySettingsByteBitsButton = new System.Windows.Forms.Button();
            this.debugPrintMaxSpriteCountButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // debugScanForF8FFCommandsButton
            // 
            this.debugScanForF8FFCommandsButton.Location = new System.Drawing.Point(20, 193);
            this.debugScanForF8FFCommandsButton.Name = "debugScanForF8FFCommandsButton";
            this.debugScanForF8FFCommandsButton.Size = new System.Drawing.Size(187, 23);
            this.debugScanForF8FFCommandsButton.TabIndex = 51;
            this.debugScanForF8FFCommandsButton.Text = "Scan For F8-FF";
            this.debugScanForF8FFCommandsButton.UseVisualStyleBackColor = true;
            this.debugScanForF8FFCommandsButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugMapUnknownByteButton
            // 
            this.debugMapUnknownByteButton.Location = new System.Drawing.Point(20, 164);
            this.debugMapUnknownByteButton.Name = "debugMapUnknownByteButton";
            this.debugMapUnknownByteButton.Size = new System.Drawing.Size(187, 23);
            this.debugMapUnknownByteButton.TabIndex = 50;
            this.debugMapUnknownByteButton.Text = "Map Unknown Byte";
            this.debugMapUnknownByteButton.UseVisualStyleBackColor = true;
            this.debugMapUnknownByteButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugSpriteUnknownBitsButton
            // 
            this.debugSpriteUnknownBitsButton.Location = new System.Drawing.Point(20, 135);
            this.debugSpriteUnknownBitsButton.Name = "debugSpriteUnknownBitsButton";
            this.debugSpriteUnknownBitsButton.Size = new System.Drawing.Size(187, 23);
            this.debugSpriteUnknownBitsButton.TabIndex = 49;
            this.debugSpriteUnknownBitsButton.Text = "Sprite Unknown Bits";
            this.debugSpriteUnknownBitsButton.UseVisualStyleBackColor = true;
            this.debugSpriteUnknownBitsButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugLayerSettingsCountButton
            // 
            this.debugLayerSettingsCountButton.Location = new System.Drawing.Point(20, 106);
            this.debugLayerSettingsCountButton.Name = "debugLayerSettingsCountButton";
            this.debugLayerSettingsCountButton.Size = new System.Drawing.Size(187, 23);
            this.debugLayerSettingsCountButton.TabIndex = 48;
            this.debugLayerSettingsCountButton.Text = "Layer Settings Count";
            this.debugLayerSettingsCountButton.UseVisualStyleBackColor = true;
            this.debugLayerSettingsCountButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintSpiteActionsButton
            // 
            this.debugPrintSpiteActionsButton.Location = new System.Drawing.Point(20, 77);
            this.debugPrintSpiteActionsButton.Name = "debugPrintSpiteActionsButton";
            this.debugPrintSpiteActionsButton.Size = new System.Drawing.Size(187, 23);
            this.debugPrintSpiteActionsButton.TabIndex = 47;
            this.debugPrintSpiteActionsButton.Text = "Print Sprite Actions";
            this.debugPrintSpiteActionsButton.UseVisualStyleBackColor = true;
            this.debugPrintSpiteActionsButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugScanForTileIDButton
            // 
            this.debugScanForTileIDButton.Location = new System.Drawing.Point(20, 48);
            this.debugScanForTileIDButton.Name = "debugScanForTileIDButton";
            this.debugScanForTileIDButton.Size = new System.Drawing.Size(187, 23);
            this.debugScanForTileIDButton.TabIndex = 46;
            this.debugScanForTileIDButton.Text = "Scan For Tile ID";
            this.debugScanForTileIDButton.UseVisualStyleBackColor = true;
            this.debugScanForTileIDButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugDrawAllButton
            // 
            this.debugDrawAllButton.Location = new System.Drawing.Point(20, 19);
            this.debugDrawAllButton.Name = "debugDrawAllButton";
            this.debugDrawAllButton.Size = new System.Drawing.Size(187, 23);
            this.debugDrawAllButton.TabIndex = 45;
            this.debugDrawAllButton.Text = "Debug Draw All";
            this.debugDrawAllButton.UseVisualStyleBackColor = true;
            this.debugDrawAllButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintDefinitionOffsetsButton
            // 
            this.debugPrintDefinitionOffsetsButton.Location = new System.Drawing.Point(225, 277);
            this.debugPrintDefinitionOffsetsButton.Name = "debugPrintDefinitionOffsetsButton";
            this.debugPrintDefinitionOffsetsButton.Size = new System.Drawing.Size(154, 23);
            this.debugPrintDefinitionOffsetsButton.TabIndex = 52;
            this.debugPrintDefinitionOffsetsButton.Text = "Print Definition Offsets";
            this.debugPrintDefinitionOffsetsButton.UseVisualStyleBackColor = true;
            this.debugPrintDefinitionOffsetsButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintWeaponDefinitionButton
            // 
            this.debugPrintWeaponDefinitionButton.Location = new System.Drawing.Point(225, 306);
            this.debugPrintWeaponDefinitionButton.Name = "debugPrintWeaponDefinitionButton";
            this.debugPrintWeaponDefinitionButton.Size = new System.Drawing.Size(154, 23);
            this.debugPrintWeaponDefinitionButton.TabIndex = 53;
            this.debugPrintWeaponDefinitionButton.Text = "Print Weapon Definitions";
            this.debugPrintWeaponDefinitionButton.UseVisualStyleBackColor = true;
            this.debugPrintWeaponDefinitionButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintSpecialItem40Button
            // 
            this.debugPrintSpecialItem40Button.Location = new System.Drawing.Point(20, 250);
            this.debugPrintSpecialItem40Button.Name = "debugPrintSpecialItem40Button";
            this.debugPrintSpecialItem40Button.Size = new System.Drawing.Size(187, 23);
            this.debugPrintSpecialItem40Button.TabIndex = 54;
            this.debugPrintSpecialItem40Button.Text = "PrintSpecialItem40";
            this.debugPrintSpecialItem40Button.UseVisualStyleBackColor = true;
            this.debugPrintSpecialItem40Button.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintSpecialItem20Button
            // 
            this.debugPrintSpecialItem20Button.Location = new System.Drawing.Point(20, 221);
            this.debugPrintSpecialItem20Button.Name = "debugPrintSpecialItem20Button";
            this.debugPrintSpecialItem20Button.Size = new System.Drawing.Size(187, 23);
            this.debugPrintSpecialItem20Button.TabIndex = 55;
            this.debugPrintSpecialItem20Button.Text = "PrintSpecialItem20";
            this.debugPrintSpecialItem20Button.UseVisualStyleBackColor = true;
            this.debugPrintSpecialItem20Button.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintAnimationScriptCommandCountButton
            // 
            this.debugPrintAnimationScriptCommandCountButton.Location = new System.Drawing.Point(315, 77);
            this.debugPrintAnimationScriptCommandCountButton.Name = "debugPrintAnimationScriptCommandCountButton";
            this.debugPrintAnimationScriptCommandCountButton.Size = new System.Drawing.Size(206, 23);
            this.debugPrintAnimationScriptCommandCountButton.TabIndex = 56;
            this.debugPrintAnimationScriptCommandCountButton.Text = "Print Animation Script Command Count";
            this.debugPrintAnimationScriptCommandCountButton.UseVisualStyleBackColor = true;
            this.debugPrintAnimationScriptCommandCountButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugScanForDisplaySettingsByteBitsButton
            // 
            this.debugScanForDisplaySettingsByteBitsButton.Location = new System.Drawing.Point(20, 279);
            this.debugScanForDisplaySettingsByteBitsButton.Name = "debugScanForDisplaySettingsByteBitsButton";
            this.debugScanForDisplaySettingsByteBitsButton.Size = new System.Drawing.Size(187, 23);
            this.debugScanForDisplaySettingsByteBitsButton.TabIndex = 57;
            this.debugScanForDisplaySettingsByteBitsButton.Text = "ScanForDisplaySettingsByteBits";
            this.debugScanForDisplaySettingsByteBitsButton.UseVisualStyleBackColor = true;
            this.debugScanForDisplaySettingsByteBitsButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // debugPrintMaxSpriteCountButton
            // 
            this.debugPrintMaxSpriteCountButton.Location = new System.Drawing.Point(20, 308);
            this.debugPrintMaxSpriteCountButton.Name = "debugPrintMaxSpriteCountButton";
            this.debugPrintMaxSpriteCountButton.Size = new System.Drawing.Size(187, 23);
            this.debugPrintMaxSpriteCountButton.TabIndex = 58;
            this.debugPrintMaxSpriteCountButton.Text = "PrintMaxSpriteCount";
            this.debugPrintMaxSpriteCountButton.UseVisualStyleBackColor = true;
            this.debugPrintMaxSpriteCountButton.Click += new System.EventHandler(this.RunMapDebuggerButton_Click);
            // 
            // DebuggerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.debugPrintMaxSpriteCountButton);
            this.Controls.Add(this.debugScanForDisplaySettingsByteBitsButton);
            this.Controls.Add(this.debugPrintAnimationScriptCommandCountButton);
            this.Controls.Add(this.debugPrintSpecialItem20Button);
            this.Controls.Add(this.debugPrintSpecialItem40Button);
            this.Controls.Add(this.debugPrintWeaponDefinitionButton);
            this.Controls.Add(this.debugPrintDefinitionOffsetsButton);
            this.Controls.Add(this.debugScanForF8FFCommandsButton);
            this.Controls.Add(this.debugMapUnknownByteButton);
            this.Controls.Add(this.debugSpriteUnknownBitsButton);
            this.Controls.Add(this.debugLayerSettingsCountButton);
            this.Controls.Add(this.debugPrintSpiteActionsButton);
            this.Controls.Add(this.debugScanForTileIDButton);
            this.Controls.Add(this.debugDrawAllButton);
            this.Name = "DebuggerUserControl";
            this.Size = new System.Drawing.Size(575, 576);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button debugScanForF8FFCommandsButton;
        private System.Windows.Forms.Button debugMapUnknownByteButton;
        private System.Windows.Forms.Button debugSpriteUnknownBitsButton;
        private System.Windows.Forms.Button debugLayerSettingsCountButton;
        private System.Windows.Forms.Button debugPrintSpiteActionsButton;
        private System.Windows.Forms.Button debugScanForTileIDButton;
        private System.Windows.Forms.Button debugDrawAllButton;
        private System.Windows.Forms.Button debugPrintDefinitionOffsetsButton;
        private System.Windows.Forms.Button debugPrintWeaponDefinitionButton;
        private System.Windows.Forms.Button debugPrintSpecialItem40Button;
        private System.Windows.Forms.Button debugPrintSpecialItem20Button;
        private System.Windows.Forms.Button debugPrintAnimationScriptCommandCountButton;
        private System.Windows.Forms.Button debugScanForDisplaySettingsByteBitsButton;
        private System.Windows.Forms.Button debugPrintMaxSpriteCountButton;
    }
}
