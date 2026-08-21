using System.Collections.Generic;
using ZwellTech;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    /// <summary>
    /// Represents a boss frame that is drawn as part of the background.
    /// </summary>
    public sealed record BossBackgroundFrame : BossFrame
    {
        public byte UnknownValue1 { get; init; }
        public byte UnknownValue2 { get; init; }

        public BossBackgroundFrame(int index, BossFrameType frameType, string displayName, int address, int length, byte unk1, byte unk2, IReadOnlyList<BossFramePart> parts) : base(index, frameType, displayName, address, length, parts, BossHitBox.NullHitBox, BossHitBox.NullWeaponBox, BossHitBox.NullGuardBox)
        {
            this.UnknownValue1 = unk1;
            this.UnknownValue2 = unk2;
        }

        /// <inheritdoc/>
        public override SuperNintendoGraphics DrawFrame(Tileset tileset, IReadOnlyList<SpritePalette> paletteTable, FrameDrawingOptions options)
        {
            if (this.Parts.RowCount == 0) { ThrowHelper.ThrowInvalidOperationException("No frame parts to draw."); }

            // Background frames come in either one or two parts.
            // In the case of two frames, the first frame will determine the size of the graphics object.
            BossBackgroundFramePart currentPart = (BossBackgroundFramePart)this.Parts[0];

            // If we only have one part then we need to ignore the X/Y coordinates to draw the piece correctly.
            bool ignoreOffsets = this.Parts.RowCount == 1;

            SpritePalette palette = paletteTable[0];
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(palette, currentPart.ColumnCount * 8, currentPart.RowCount * 8);
            if (options.HasFlag(FrameDrawingOptions.ShowGridlines))
            {
                graphics.DrawGridlines(Rgb555Color.Navy, currentPart.RowCount, currentPart.ColumnCount);
            }

            using SuperNintendoGraphics tileGraphics = SuperNintendoGraphics.CreateGraphics(palette, currentPart.ColumnCount * 8, currentPart.RowCount * 8);

            DrawFramePart(tileGraphics, tileset, currentPart, ignoreOffsets);
            for (int i = 1; i < this.Parts.RowCount; i++)
            {
                currentPart = (BossBackgroundFramePart)this.Parts[i];
                DrawFramePart(tileGraphics, tileset, currentPart, ignoreOffsets);
            }

            graphics.Merge(tileGraphics);

            return graphics;

            static void DrawFramePart(SuperNintendoGraphics tileGraphics, Tileset tileset, BossBackgroundFramePart framePart, bool ignoreOffsets)
            {
                const bool AlwaysDrawLeftToRight = true;

                bool drawLeftToRight = (tileset.TileType != TileType.Mode7) || AlwaysDrawLeftToRight;
                if (drawLeftToRight)
                {
                    DrawFramePartLeftToRight(tileGraphics, tileset, framePart, ignoreOffsets);
                    return;
                }

                // Still doesn't draw Mode 7 correctly.
                DrawFramePartTopToBottom(tileGraphics, tileset, framePart, ignoreOffsets);
            }

            static void DrawFramePartLeftToRight(SuperNintendoGraphics tileGraphics, Tileset tileset, BossBackgroundFramePart framePart, bool ignoreOffsets)
            {
                int rowIndex = 0;
                int columnIndex = 0;
                foreach (BossSpriteFramePart part in framePart.Parts)
                {
                    int xOffset = !ignoreOffsets ? framePart.XCoordinate : 0;
                    int yOffset = !ignoreOffsets ? framePart.YCoordinate : 0;

                    // What the fuck is up with Dragon Body tile IDs?
                    ushort tileId = part.TileId;
                    //tileId -= 0x80;
                    //if (part.ExtendTileId) { tileId += 0x100; }

                    GraphicTile tile = tileset.GetTile(tileId, TileErrorMode.FullTile);
                    tileGraphics.DrawTile(tile, (columnIndex + xOffset) * 8, (rowIndex + yOffset) * 8, part.FlipType);

                    columnIndex++;
                    if (columnIndex == framePart.ColumnCount)
                    {
                        // Wrap around to the start of the next row if we have reached the end of the current row.
                        columnIndex = 0;
                        rowIndex++;
                    }
                }
            }

            static void DrawFramePartTopToBottom(SuperNintendoGraphics tileGraphics, Tileset tileset, BossBackgroundFramePart framePart, bool ignoreOffsets)
            {
                int rowIndex = 0;
                int columnIndex = 0;
                foreach (BossSpriteFramePart part in framePart.Parts)
                {
                    int xOffset = !ignoreOffsets ? framePart.XCoordinate : 0;
                    int yOffset = !ignoreOffsets ? framePart.YCoordinate : 0;

                    GraphicTile tile = tileset.GetTile(part.TileId, TileErrorMode.FullTile);
                    tileGraphics.DrawTile(tile, (columnIndex + xOffset) * 8, (rowIndex + yOffset) * 8, part.FlipType);

                    rowIndex++;
                    if (rowIndex == framePart.RowCount)
                    {
                        // Wrap around to the start of the next column if we have reached the end of the column row.
                        rowIndex = 0;
                        columnIndex++;
                    }
                }
            }
        }
    }
}