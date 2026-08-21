using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents a boss frame that is drawn as a sprite.
    /// </summary>
    public sealed record BossSpriteFrame : BossFrame
    {
        private const int BossFrameSize = 192;
        public const int BossFrameOriginOffset = BossSpriteFrame.BossFrameSize / 2;

        /// <inheritdoc/>
        public BossSpriteFrame(int index, string displayName, int address, IReadOnlyList<BossFramePart> parts, BossHitBox hitBox, BossHitBox weaponBox, BossHitBox guardBox) : base(index, BossFrameType.Sprite, displayName, address, parts.Count, parts, hitBox, weaponBox, guardBox) { }
        public BossSpriteFrame(int index, BossFrameType frameType, string displayName, int address, int length, IReadOnlyList<BossFramePart> parts, BossHitBox hitBox, BossHitBox weaponBox, BossHitBox guardBox) : base(index, frameType, displayName, address, length, parts, hitBox, weaponBox, guardBox) { }

        /// <inheritdoc/>
        public override SuperNintendoGraphics DrawFrame(Tileset tileset, IReadOnlyList<SpritePalette> paletteTable, FrameDrawingOptions options)
        {
            // Boss frame parts are drawn relative to the bosses origin.
            // It account for this, create a graphic large enough for the frame to expand in all directions.
            // The frame parts will be drawn relative to the center of the graphic.
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteTable[0], BossSpriteFrame.BossFrameSize, BossSpriteFrame.BossFrameSize);
            if (options.HasFlag(FrameDrawingOptions.ShowGridlines))
            {
                // Originally this was using BossFrame.BackgroundPalette to draw the gridlines in the navy color.
                // Didn't want to inject colors outside the palette into the graphics.
                // However this causes transparency issues with Doom's Wall (and seemingly only Doom's Wall).
                // Should investigate later.
                // Nope, also Pumpkins, probably has to do with the black color.
                graphics.DrawGridlines(Rgb555Color.Navy, BossSpriteFrame.BossFrameSize / 8, BossSpriteFrame.BossFrameSize / 8);
            }

            List<GraphicTile>? tileList = null;
            for (int i = 0; i < this.Length; i++)
            {
                // TODO: Frame Collision
                if (this.Parts[i] is BossSpriteFramePart framePart)
                {
                    ushort tileId = framePart.CalculateTileId(tileset.Index, !(tileset is BossTileset));
                    SpritePalette palette = paletteTable[framePart.PaletteIndex];

                    if (tileId > tileset.Tiles.Count)
                    {
                        // Tropicallo has to be difficult. Instead of using a seperate object to draw its shadow, its opening frames draw it itself.
                        // This means it has Tile IDs references to the actual shadow tiles. Which aren't present in our tileset.
                        // TODO: Find and load shadow graphics. Shadow frames start at CA/BE60.
                        string message = $"{tileset.Name}: Frame: {this.Index}, Tile ID: {framePart.TileId:X2}, Calculated Tile ID: {tileId:X2}. Calculated Tile ID is invalid. Using base ID instead. Frame will draw incorrectly.";
                        LoggerEngine.Logger.LogError(LogComponent.RomReader, message);

                        // Maybe just let it draw error tiles instead?
                        tileId = framePart.TileId;
                    }

                    TileSize size = framePart.Draw32x32Tile ? TileSize.Size32X32 : TileSize.Size16X16;
                    int pixelCount = framePart.Draw32x32Tile ? 32 : 16;

                    using SuperNintendoGraphics tileGraphics = SuperNintendoGraphics.CreateGraphics(palette, pixelCount, pixelCount);

                    // Fetch the four/sixteen 8x8 tiles that make up the 16x16/32x32 tile we need to draw.
                    tileList = tileset.GetTile(tileId, size, TileErrorMode.FullTile);

                    // Draw the current frame part.
                    tileGraphics.DrawTile(CollectionsMarshal.AsSpan<GraphicTile>(tileList), 0, 0, framePart.FlipType, size);

                    // Merge the frame part into the main frame graphic. Draw the part relative to the center of the graphic.
                    graphics.Merge(tileGraphics, framePart.XCoordinate + BossSpriteFrame.BossFrameOriginOffset, framePart.YCoordinate + BossSpriteFrame.BossFrameOriginOffset);
                    tileList.Clear();
                }
            }

            if (HitBox.IsValid && options.HasFlag(FrameDrawingOptions.ShowHitBox))
            {
                int hitBoxX = BossSpriteFrame.BossFrameOriginOffset + (HitBox.XOffset - HitBox.Width);
                int hitBoxY = BossSpriteFrame.BossFrameOriginOffset + (HitBox.YOffset - HitBox.Height);
                graphics.DrawRectangle(Rgb555Color.Green, hitBoxX, hitBoxY, HitBox.Width * 2, HitBox.Height * 2);
            }
            if (WeaponBox.IsValid && options.HasFlag(FrameDrawingOptions.ShowWeaponBox))
            {
                int hitBoxX = BossSpriteFrame.BossFrameOriginOffset + (WeaponBox.XOffset - WeaponBox.Width);
                int hitBoxY = BossSpriteFrame.BossFrameOriginOffset + (WeaponBox.YOffset - WeaponBox.Height);
                graphics.DrawRectangle(Rgb555Color.Black, hitBoxX, hitBoxY, WeaponBox.Width * 2, WeaponBox.Height * 2);
            }
            if (GuardBox.IsValid && options.HasFlag(FrameDrawingOptions.ShowGuardBox))
            {
                int hitBoxX = BossSpriteFrame.BossFrameOriginOffset + (GuardBox.XOffset - GuardBox.Width);
                int hitBoxY = BossSpriteFrame.BossFrameOriginOffset + (GuardBox.YOffset - GuardBox.Height);
                graphics.DrawRectangle(Rgb555Color.Red, hitBoxX, hitBoxY, GuardBox.Width * 2, GuardBox.Height * 2);
            }

            return graphics;
        }
    }

    [Flags]
    public enum FrameDrawingOptions
    {
        None = 0,
        ShowGridlines = 0B_0000_0001,
        ShowHitBox = 0B_0000_0010,
        ShowWeaponBox = 0B_0000_0100,
        ShowGuardBox = 0B_0000_1000,
    }
}