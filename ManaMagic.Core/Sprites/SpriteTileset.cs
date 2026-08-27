using System;
using System.Collections.Generic;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed class SpriteTileset : Tileset
    {
        private readonly Dictionary<ushort, int> offsetLookup = new Dictionary<ushort, int>();

        public SpriteGraphicsData GraphicsData { get; }
        public SpritePalette Palette { get; }

        public SpriteTileset(int index, TileType tileType, SpriteGraphicsData graphicsData, SpritePalette palette) : base(index, new List<GraphicTile>(), tileType)
        {
            this.GraphicsData = graphicsData;
            this.Palette = palette;
        }

        public SuperNintendoGraphics DrawFrame(ushort frameIndex)
        {
            return this.GetFrame(frameIndex).DrawFrame(this);
        }

        public SpriteFrame GetFrame(ushort frameIndex)
        {
            // This should ideally be somewhere else, but good enough for now.
            if (this.Index < Constants.FirstBossIndex || this.Index > Constants.LastBossIndex)
            {
                int bankAddress = this.Index < Constants.FirstBossIndex ? 0x130000 : 0x120000;
                SpriteRomReader romReader = RomReaderFactory.GetRomReader<SpriteRomReader>();
                romReader.Seek(this.GraphicsData.FrameIndexOffset + bankAddress + (frameIndex * 2));

                ushort frameOffset = romReader.ReadUInt16();

                int framePointer = bankAddress + frameOffset;
                romReader.Seek(framePointer);

                byte frameControl = romReader.Read();
                bool hasCollision = (frameControl & 0x40) > 0;
                byte tile16Count = (byte)(frameControl & 0x0F);

                List<ushort> tileOffsets = new List<ushort>(tile16Count * 4);
                for (int tileIndex = 0; tileIndex < tile16Count * 4; tileIndex++)
                {
                    ushort encodedOffset = romReader.ReadUInt16();

                    // If the high bit is set then the encoded offset points to a number of tiles.
                    bool loadLoop = (encodedOffset & 0x8000) > 0;
                    if (loadLoop)
                    {
                        // If this bit is set then we load a number of tiles in a row, otherwise load the same tile repeatedly.
                        bool loadLoopIncrement = (encodedOffset & 0x4000) > 0;

                        // The number of tiles to load.
                        byte loadLoopCount = (byte)((encodedOffset & 0x000F) + 1);

                        ushort offset = (ushort)((encodedOffset * 2) & 0x7FE0);
                        for (int loopIndex = 0; loopIndex < loadLoopCount; loopIndex++)
                        {
                            tileOffsets.Add(offset);
                            tileIndex++;

                            if (loadLoopIncrement) { offset += 32; }
                        }

                        // tileIndex is currently at the correct index, however the outer loop will increment it so this needs to be un-done.
                        tileIndex--;
                    }
                    else
                    {
                        // If its not a loop then its just an encoded offset to the tile.
                        ushort offset = (ushort)((encodedOffset & 0x3FF0) * 2);
                        tileOffsets.Add(offset);
                    }
                }

                List<(sbyte XCoordinate, sbyte YCoordinate, FlipType FlipFlags)> coordList = new List<(sbyte XCoordinate, sbyte YCoordinate, FlipType FlipFlags)>();
                for (int tileIndex = 0; tileIndex < tile16Count; tileIndex++)
                {
                    byte yCoordinate = romReader.Read();
                    byte xCoordinate = romReader.Read();
                    FlipType flipType = (xCoordinate & 0x80) > 0 ? FlipType.HorizontalFlip : FlipType.None;
                    //if ((yCoordinate & 0x80) > 0) { flipType |= FlipType.VerticalFlip; }

                    bool xNegFlag = (xCoordinate & 0x40) > 0;
                    bool yNegFlag = (yCoordinate & 0x40) > 0;

                    xCoordinate = (byte)(xCoordinate & 0x7F);
                    yCoordinate = (byte)(yCoordinate & 0x7F);

                    if (xNegFlag) { xCoordinate |= 0x80; }
                    if (yNegFlag) { yCoordinate |= 0x80; }

                    coordList.Add(((sbyte)xCoordinate, (sbyte)yCoordinate, flipType));
                }

                List<SpriteFramePart> frameParts = new List<SpriteFramePart>(tile16Count + 1);
                for (int i = 0; i < tile16Count - 0; i++)
                {
                    frameParts.Add(new SpriteFramePart(tileOffsets[0 + (i * 4)],
                                                       tileOffsets[1 + (i * 4)],
                                                       tileOffsets[2 + (i * 4)],
                                                       tileOffsets[3 + (i * 4)],
                                                       coordList[i].XCoordinate,
                                                       coordList[i].YCoordinate,
                                                       coordList[i].FlipFlags));
                }
                SpriteFrame frame = new SpriteFrame(frameParts);

                return frame;
            }
            return SpriteFrame.Empty;
        }

        public GraphicTile GetTileByOffset(ushort offset)
        {
            if (!this.offsetLookup.TryGetValue(offset, out int index))
            {
                index = this.offsetLookup.Count;
                this.Add(this.ReadTile(offset));
                this.offsetLookup.Add(offset, this.offsetLookup.Count);
            }

            return this.Tiles[index];
        }

        private GraphicTile ReadTile(int offset)
        {
            SpriteRomReader romReader = RomReaderFactory.GetRomReader<SpriteRomReader>();
            romReader.Seek((int)this.GraphicsData.FullGraphicsAddress + offset);

            Span<byte> tileBuffer = stackalloc byte[32];
            for (int j = 0; j < 32; j++)
            {
                tileBuffer[j] = romReader.Read();
            }
            return GraphicTile4Bpp.From4BppTile(tileBuffer);
        }
    }
}