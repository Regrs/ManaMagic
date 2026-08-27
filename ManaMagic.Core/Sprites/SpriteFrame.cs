using System;
using System.Collections.Generic;
using System.Linq;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed class SpriteFrame
    {
        public static SpriteFrame Empty { get; } = new SpriteFrame(new List<SpriteFramePart>());

        public IReadOnlyList<SpriteFramePart> Parts { get; }

        public SpriteFrame(IReadOnlyList<SpriteFramePart> parts)
        {
            this.Parts = parts;
        }

        public void DrawFrame(SuperNintendoGraphics graphics, SpriteTileset tileset)
        {
            GraphicTile[] tiles = new GraphicTile[4];
            foreach (SpriteFramePart part in this.Parts)
            {
                tiles[0] = tileset.GetTileByOffset(part.TopLeftOffset);
                tiles[1] = tileset.GetTileByOffset(part.TopRightOffset);
                tiles[2] = tileset.GetTileByOffset(part.BottomLeftOffset);
                tiles[3] = tileset.GetTileByOffset(part.BottomRightOffset);

                graphics.Draw16x16Tile(tiles, (graphics.Size.Width / 2) + part.XCoordinate, (graphics.Size.Height / 2) + part.YCoordinate, part.FlipFlags);
            }
        }

        public SuperNintendoGraphics DrawFrame(SpriteTileset tileset)
        {
            return this.DrawFrame(tileset, tileset.Palette);
        }

        public SuperNintendoGraphics DrawFrame(SpriteTileset tileset, SpritePalette palette)
        {
            if (this.Parts.Count == 0)
            {
                return SuperNintendoGraphics.CreateGraphics(palette, 1, 1);
            }

            int x = this.Parts.Max(p => p.XCoordinate) - this.Parts.Min(p => p.XCoordinate) + 16;
            int y = this.Parts.Max(p => p.YCoordinate) - this.Parts.Min(p => p.YCoordinate) + 16;
            int xCoord = (int)(Math.Round(this.Parts.Sum(p => Math.Abs(p.XCoordinate)) + 1 / 16d) * 16);
            int yCoord = (int)(Math.Round(this.Parts.Sum(p => Math.Abs(p.YCoordinate)) + 1 / 16d) * 16);

            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(palette, x, y);
            graphics.Fill();

            GraphicTile[] tiles = new GraphicTile[4];
            foreach (SpriteFramePart part in this.Parts)
            {
                tiles[0] = tileset.GetTileByOffset(part.TopLeftOffset);
                tiles[1] = tileset.GetTileByOffset(part.TopRightOffset);
                tiles[2] = tileset.GetTileByOffset(part.BottomLeftOffset);
                tiles[3] = tileset.GetTileByOffset(part.BottomRightOffset);

                graphics.Draw16x16Tile(tiles, (graphics.Size.Width / 2) + part.XCoordinate, (graphics.Size.Height / 2) + part.YCoordinate, part.FlipFlags);
            }

            return graphics;
        }
    }
}