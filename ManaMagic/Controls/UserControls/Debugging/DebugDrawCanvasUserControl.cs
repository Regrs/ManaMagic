using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.RomReaders;
using ManaMagic.Core.Sprites;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

namespace ManaMagic.Controls.UserControls.Debug
{
    public partial class DebugDrawCanvasUserControl : UserControl
    {
        private int frameIndex = 0;
        private int spriteIndex = 0;

        public DebugDrawCanvasUserControl()
        {
            InitializeComponent();

            this.previousCanvas1Button.Enabled = false;
            this.Load += DebugDrawCanvasUserControl_Load;
        }

        private void X3()
        {
            SpriteTileset tileset = ManaMagicContext.Current.Context.SpriteContext.SpriteTilesets[this.spriteIndex];
            using SuperNintendoGraphics graphics = tileset.DrawFrame((ushort)this.frameIndex);

            this.canvas2PictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        private void DebugDrawCanvasUserControl_Load(object sender, EventArgs e)
        {
            //this.romReader = RomReaderFactory.GetRomReader<SpriteRomReader>();
            //this.SpriteTest2(0x00, 214); // Rabite

            this.X3();
        }

        private void PreviousCanvas1Button_Click(object sender, EventArgs e)
        {
            if (this.spriteIndex >= 0)
            {
                this.spriteIndex--;
                this.frameIndex = 0;
                this.X3();
            }
        }

        private void NextCanvas1Button_Click(object sender, EventArgs e)
        {
            this.spriteIndex++;
            this.frameIndex = 0;
            this.X3();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e) { }

        private void C2NextButton_Click(object sender, EventArgs e)
        {
            this.frameIndex++;
            this.X3();
        }

        private void C2PrevButton_Click(object sender, EventArgs e)
        {
            if (this.frameIndex >= 0)
            {
                this.frameIndex--;
                this.X3();
            }
        }
    }


}
/* Code Graveyard: Sprites
        private SpriteTileset tileset2;
        private SpritePalette palette;
        private SpriteGraphicsData graphicsData;
        private SpriteRomReader romReader;

        private void SpriteTest2(int index, int tilesToLoad)
        {
            this.spriteIndex = index;
            this.graphicsData = ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[index];
            this.palette = ManaMagicContext.Current.Context.SpriteContext.PaletteTable[index];

            this.tileset2 = new SpriteTileset(0, TileType.FourBitsPerPixel, graphicsData, this.palette);

            //using SuperNintendoGraphics graphics = this.tileset.DrawTileset(this.palette, 32);
            //this.canvas1PictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        private void X2()
        {
            this.romReader.Seek(this.tileset2.GraphicsData.FrameIndexOffset + 0x130000);

            romReader.Position += (this.frameIndex * 2);
            ushort frameOffset = this.romReader.ReadUInt16();

            int framePointer = 0x130000 + frameOffset;
            this.romReader.Seek(framePointer);

            byte frameControl = this.romReader.Read();
            bool hasCollision = (frameControl & 0x40) > 0;
            byte tile16Count = (byte)(frameControl & 0x0F);

            List<ushort> tileOffsets = new List<ushort>(tile16Count * 4);
            for (int tileIndex = 0; tileIndex < tile16Count * 4; tileIndex++)
            {
                ushort encodedOffset = this.romReader.ReadUInt16();

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
                byte yCoordinate = this.romReader.Read();
                byte xCoordinate = this.romReader.Read();
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

            using SuperNintendoGraphics graphics = frame.DrawFrame(this.tileset2);
            this.canvas2PictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
        }

        public sealed class SpriteTilesetOld : Tileset
        {
            public IReadOnlyDictionary<ushort, int> ReverseLookup { get; }

            public SpriteTilesetOld(int index, IReadOnlyList<GraphicTile> tiles, TileType tileType, IReadOnlyDictionary<ushort, int> reverseLookup) : base(index, tiles, tileType)
            {
                this.ReverseLookup = reverseLookup;
            }
        }

        private SpriteTilesetOld GetSpriteTileset(SpriteRomReader romReader, int tilesToLoad)
        {
            List<GraphicTile4Bpp> tileList = new List<GraphicTile4Bpp>(tilesToLoad);
            Dictionary<ushort, int> reverseLookup = new Dictionary<ushort, int>(tilesToLoad);

            romReader.Seek((int)this.graphicsData.FullGraphicsAddress);

            Span<byte> tileBuffer = stackalloc byte[32];
            for (int i = 0; i < tilesToLoad; i++)
            {
                ushort offset = (ushort)(romReader.Position - graphicsData.FullGraphicsAddress);
                reverseLookup.Add(offset, i);

                tileBuffer.Fill(0);
                for (int j = 0; j < 32; j++)
                {
                    tileBuffer[j] = romReader.Read();
                }
                tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
            }

            SpriteTilesetOld tileset = new SpriteTilesetOld(0, tileList, TileType.FourBitsPerPixel, reverseLookup);
            return tileset;
        }

        private void X()
        {
            this.romReader.Seek(this.tileset2.GraphicsData.FrameIndexOffset + 0x130000);

            romReader.Position += (this.frameIndex * 2);
            ushort frameOffset = this.romReader.ReadUInt16();

            int framePointer = 0x130000 + frameOffset;
            this.romReader.Seek(framePointer);

            byte frameControl = this.romReader.Read();
            bool hasCollision = (frameControl & 0x40) > 0;
            byte tile16Count = (byte)(frameControl & 0x0F);

            List<ushort> tileOffsets = new List<ushort>(tile16Count * 4);
            for (int tileIndex = 0; tileIndex < tile16Count * 4; tileIndex++)
            {
                ushort encodedOffset = this.romReader.ReadUInt16();

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
                byte yCoordinate = this.romReader.Read();
                byte xCoordinate = this.romReader.Read();
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

            int size = 384;
            using SuperNintendoGraphics graphics2 = SuperNintendoGraphics.CreateGraphics(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[this.spriteIndex], size, size);
            graphics2.DrawGridlines(Rgb555Color.Navy, size / 8, size / 8);

            frame.DrawFrame(graphics2, this.tileset2);
            List<GraphicTile> tiles = new List<GraphicTile>();

            //for (int i = 0; i < tile16Count - 0; i++)
            //{
            //    tiles.Clear();
            //    tiles.Add(tileset2.GetTileByOffset(tileOffsets[0 + (i * 4)]));
            //    tiles.Add(tileset2.GetTileByOffset(tileOffsets[1 + (i * 4)]));
            //    tiles.Add(tileset2.GetTileByOffset(tileOffsets[2 + (i * 4)]));
            //    tiles.Add(tileset2.GetTileByOffset(tileOffsets[3 + (i * 4)]));

            //    graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[i].XCoordinate / 1), (size / 2) + (coordList[i].YCoordinate / 1), coordList[i].FlipFlags);
            //}

            for (int i = 0; i < tile16Count; i++)
            {
                tiles.Clear();
                tiles.Add(tileset2.GetTileByOffset(tileOffsets[0 + (i * 4)]));
                tiles.Add(tileset2.GetTileByOffset(tileOffsets[1 + (i * 4)]));
                tiles.Add(tileset2.GetTileByOffset(tileOffsets[2 + (i * 4)]));
                tiles.Add(tileset2.GetTileByOffset(tileOffsets[3 + (i * 4)]));

                FlipType flip = FlipType.None;// flipTypeList[i];
                graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (16 * i), (16 * i), flip);
            }

            this.canvas2PictureBox.Image = new Bitmap(graphics2.GetBitmap(true), graphics2.Size.Width * 2, graphics2.Size.Height * 2);
        }

        private void ReadAndDrawSpriteFrame()
        {
            // 205 wrong
            this.romReader.Seek(this.graphicsData.FrameIndexOffset + 0x130000);

            romReader.Position += (this.frameIndex * 2);
            ushort frameOffset = this.romReader.ReadUInt16();

            int framePointer = 0x130000 + frameOffset;
            this.romReader.Seek(framePointer);

            byte frameControl = this.romReader.Read();
            bool hasCollision = (frameControl & 0x40) > 0;
            byte tile16Count = (byte)(frameControl & 0x0F);
            byte tile8Count = (byte)(tile16Count * 4);
            List<ushort> tileOffsets = new List<ushort>(tile8Count);
            for (int tileIndex = 0; tileIndex < tile8Count; tileIndex++)
            {
                // $00/F676 29 F0 3F    AND #$3FF0              A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0348 VC:041 FC:31 I:01
                // $00/F679 0A          ASL A                   A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0388 VC:041 FC:31 I:01
                // $00/F67A 65 0A       ADC $0A[$00:000A]       A:1AA0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0418 VC:041 FC:31 I:01
                // $00/F67C 99 00 00    STA $0000,y[$7E:0E60]   A:7FE0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0466 VC:041 FC:31 I:01
                ushort encodedOffset = this.romReader.ReadUInt16();

                bool loadMultipleTiles = (encodedOffset & 0x8000) > 0;
                bool loadWithIncrement = (encodedOffset & 0x4000) > 0;
                byte loopCount = (byte)((encodedOffset & 0x0F) + 0);

                // D3/3D27:	14 C0 -> C0 14
                if (loadMultipleTiles)
                {
                    for (int loopIndex = 0; loopIndex <= loopCount; loopIndex++)
                    {
                        if (loadWithIncrement)
                        {
                            ushort offset = (ushort)(((encodedOffset * 2) & 0x7FE0) + (loopIndex * 32));
                            tileOffsets.Add(offset);
                        }
                        else
                        {
                            ushort offset = (ushort)((encodedOffset * 2) & 0x7FE0);
                            tileOffsets.Add(offset);
                        }
                        tileIndex++;
                    }
                    tileIndex--;
                }
                else
                {
                    ushort offset = (ushort)((encodedOffset & 0x3FF0) * 2);
                    tileOffsets.Add(offset);
                }
            }
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);
            tileOffsets.Add(tileOffsets[3]);

            //List<(byte XCoordinate, byte YCoordinate)> coordList = new List<(byte XCoordinate, byte YCoordinate)>();
            List<(sbyte XCoordinate, sbyte YCoordinate)> coordList = new List<(sbyte XCoordinate, sbyte YCoordinate)>();
            List<FlipType> flipTypeList = new List<FlipType>();
            for (int tileIndex = 0; tileIndex < tile16Count; tileIndex++)
            {
                byte yCoordinate = this.romReader.Read();
                byte xCoordinate = this.romReader.Read();
                FlipType flipType = FlipType.None;

                bool xNegFlag = (xCoordinate & 0x40) > 0;
                bool yNegFlag = (yCoordinate & 0x40) > 0;

                if ((xCoordinate & 0x80) > 0) { flipType |= FlipType.HorizontalFlip; }
                if ((xCoordinate & 0x80) > 0) { flipType |= FlipType.VerticalFlip; }
                flipTypeList.Add(flipType);

                //xCoordinate = (sbyte)((xCoordinate & 0xF0) >> 4);
                xCoordinate = (byte)((xCoordinate & 0x7F) >> 0);
                yCoordinate = (byte)((yCoordinate & 0x7F) >> 0);
                //6C: 0110_1100
                //7C: 0111_1100

                //6C: 0110_1100
                //EC: 1110_1100

                if (xNegFlag) { xCoordinate |= 0x80; }
                if (yNegFlag) { yCoordinate |= 0x80; }

                //sbyte xCoord2 = xNegFlag ? (sbyte)-xCoordinate : (sbyte)xCoordinate;
                //sbyte yCoord2 = yNegFlag ? (sbyte)-yCoordinate : (sbyte)yCoordinate;

                //xCoordinate >>= 1;
                //yCoordinate >>= 1;

                coordList.Add(((sbyte)xCoordinate, (sbyte)yCoordinate));
                //coordList.Add((xCoordinate, yCoordinate));
                //coordList.Add((xCoord2, yCoord2));

                // [73]: 0111_0011
                // [33]: 0011_0011
                // [13]: 0001_0011
            }

            for (int i = 0; i < tileOffsets.Count; i++)
            {
                //LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X2}]: {tileOffsets[i]:X4} ({tileset.ReverseLookup[tileOffsets[i]]:X2})");
            }
            for (int i = 0; i < coordList.Count; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"X-Coordinate: {coordList[i].XCoordinate}, Y-Coordinate: {coordList[i].YCoordinate}");
            }
            ////////////////////////////////////////////////F8 6C
            //$00/F745 E2 20       SEP #$20                A:F8EC
            //6C: 0110_1100
            //7C: 0111_1100
            //EC: 1110_1100

            int size = 384;
            using SuperNintendoGraphics graphics2 = SuperNintendoGraphics.CreateGraphics(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[0], size, size);
            graphics2.DrawGridlines(Rgb555Color.Navy, size / 8, size / 8);

            List<GraphicTile> tiles = new List<GraphicTile>();

            for (int i = 0; i < tile16Count - 0; i++)
            {
                tiles.Clear();
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[0 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[1 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[2 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[3 + (i * 4)]]]);

                FlipType flip = flipTypeList[i];
                graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[i].XCoordinate / 1), (size / 2) + (coordList[i].YCoordinate / 1), flip);
            }

            for (int i = 0; i < tile16Count; i++)
            {
                tiles.Clear();
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[0 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[1 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[2 + (i * 4)]]]);
                tiles.Add(tileset[tileset.ReverseLookup[tileOffsets[3 + (i * 4)]]]);

                FlipType flip = flipTypeList[i];
                graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (16 * i), (16 * i), flip);
            }

            this.canvas2PictureBox.Image = new Bitmap(graphics2.GetBitmap(true), graphics2.Size.Width * 2, graphics2.Size.Height * 2);
        }
        private void SpriteTest()
        {
            SpriteRomReader romReader = RomReaderFactory.GetRomReader<SpriteRomReader>();
            romReader.Seek((int)ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[0].FullGraphicsAddress);

            List<GraphicTile4Bpp> tileList = new List<GraphicTile4Bpp>(84);
            Dictionary<ushort, int> reverseLookup = new Dictionary<ushort, int>(84);
            Span<byte> tileBuffer = stackalloc byte[32];
            for (int i = 0; i < 214; i++)
            {
                ushort offset = (ushort)(romReader.Position - ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[0].FullGraphicsAddress);
                reverseLookup.Add(offset, i);

                tileBuffer.Fill(0);
                for (int j = 0; j < 32; j++)
                {
                    tileBuffer[j] = romReader.Read();
                }
                tileList.Add(GraphicTile4Bpp.From4BppTile(tileBuffer));
            }

            Tileset tileset = new Tileset(0, tileList, TileType.FourBitsPerPixel);
            using SuperNintendoGraphics graphics = tileset.DrawTileset(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[0], 32);
            this.canvas1PictureBox.Image = new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);

            romReader.Seek((int)ManaMagicContext.Current.Context.SpriteContext.GraphicsDataTable[0].FrameIndexOffset + 0x130000);

            //romReader.Position += 2;
            ushort frameOffset = romReader.ReadUInt16();

            int framePointer = 0x130000 + frameOffset;
            romReader.Seek(framePointer);

            byte frameControl = romReader.Read();
            bool hasCollision = (frameControl & 0x40) > 0;
            byte tile16Count = (byte)((frameControl & 0x0F) * 4);
            List<ushort> tileOffsets = new List<ushort>(tile16Count);
            for (int tileIndex = 0; tileIndex < tile16Count; tileIndex++)
            {
                // $00/F676 29 F0 3F    AND #$3FF0              A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0348 VC:041 FC:31 I:01
                // $00/F679 0A          ASL A                   A:0D50 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0388 VC:041 FC:31 I:01
                // $00/F67A 65 0A       ADC $0A[$00:000A]       A:1AA0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0418 VC:041 FC:31 I:01
                // $00/F67C 99 00 00    STA $0000,y[$7E:0E60]   A:7FE0 X:0008 Y:0E60 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0466 VC:041 FC:31 I:01
                ushort encodedOffset = romReader.ReadUInt16();

                bool loadMultipleTiles = (encodedOffset & 0x8000) > 0;
                bool loadWithIncrement = (encodedOffset & 0x4000) > 0;
                byte loopCount = (byte)((encodedOffset & 0x0F) + 1);

                // D3/3D27:	14 C0 -> C0 14
                if (loadMultipleTiles)
                {
                    for (int loopIndex = 0; loopIndex < loopCount; loopIndex++)
                    {
                        if (loadWithIncrement)
                        {
                            ushort offset = (ushort)(((encodedOffset * 2) & 0x7FE0) + (loopIndex * 32));
                            tileOffsets.Add(offset);
                        }
                        else
                        {
                            ushort offset = (ushort)((encodedOffset * 2) & 0x7FE0);
                            tileOffsets.Add(offset);
                        }
                        tileIndex++;
                    }
                }
                else
                {
                    ushort offset = (ushort)((encodedOffset & 0x3FF0) * 2);
                    tileOffsets.Add(offset);
                }
            }

            List<(sbyte XCoordinate, sbyte YCoordinate)> coordList = new List<(sbyte XCoordinate, sbyte YCoordinate)>();
            for (int tileIndex = 0; tileIndex < tile16Count / 4; tileIndex++)
            {
                sbyte yCoordinate = romReader.ReadSByte();
                sbyte xCoordinate = romReader.ReadSByte();

                //xCoordinate = (sbyte)((xCoordinate & 0xF0) >> 4);
                xCoordinate = (sbyte)(xCoordinate & 0x3F);
                //yCoordinate = (sbyte)(yCoordinate & 0xBF);
                //xCoordinate = (sbyte)(xCoordinate | 0x80);
                //xCoordinate = (sbyte)-xCoordinate;
                coordList.Add((xCoordinate, yCoordinate));
            }

            for (int i = 0; i < tileOffsets.Count; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"[{i:X2}]: {tileOffsets[i]:X4} ({reverseLookup[tileOffsets[i]]:X2})");
            }
            for (int i = 0; i < tileOffsets.Count / 4; i++)
            {
                LoggerEngine.Logger.LogDebug(LogComponent.Debug, $"X-Coordinate: {coordList[i].XCoordinate}, Y-Coordinate: {coordList[i].YCoordinate}");
            }
            ////////////////////////////////////////////////F8 6C
            //$00/F745 E2 20       SEP #$20                A:F8EC
            //6C: 0110_1100
            //7C: 0111_1100
            //EC: 1110_1100

            int size = 384;
            using SuperNintendoGraphics graphics2 = SuperNintendoGraphics.CreateGraphics(ManaMagicContext.Current.Context.SpriteContext.PaletteTable[0], size, size);
            graphics2.DrawGridlines(Rgb555Color.Navy, size / 8, size / 8);

            List<GraphicTile> tiles = new List<GraphicTile>();
            tiles.Add(tileset[reverseLookup[tileOffsets[0]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[1]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[2]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[3]]]);
            graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[0].XCoordinate / 1), (size / 2) + (coordList[0].YCoordinate / 1));

            tiles.Clear();
            tiles.Add(tileset[reverseLookup[tileOffsets[4]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[5]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[6]]]);
            tiles.Add(tileset[reverseLookup[tileOffsets[7]]]);
            graphics2.Draw16x16Tile(new Span<GraphicTile>(tiles.ToArray()), (size / 2) + (coordList[1].XCoordinate / 1), (size / 2) + (coordList[1].YCoordinate / 1));

            this.canvas2PictureBox.Image = new Bitmap(graphics2.GetBitmap(true), graphics2.Size.Width * 2, graphics2.Size.Height * 2);
        }
 */
/* Code Graveyard: Maps
        private int tileset8x8Index = 0;

        public DebugDrawCanvasUserControl()
        {
            InitializeComponent();

            this.previousCanvas1Button.Enabled = false;
            this.Load += DebugDrawCanvasUserControl_Load;
        }

        private void DebugDrawCanvasUserControl_Load(object sender, EventArgs e)
        {
            this.canvas1PictureBox.Image = TestDraw8x8MapTileset(this.tileset8x8Index);
            this.canvas2PictureBox.Image = TestDraw16x16MapTileset(this.tileset8x8Index);

            numericUpDown1.Value = 0x9F;
            this.canvas1PictureBox.Image = this.TestDrawMap().GetBitmap(true);
        }

        private void previousCanvas1Button_Click(object sender, EventArgs e)
        {
            this.tileset8x8Index--;
            this.nextCanvas1Button.Enabled = true;
            if (this.tileset8x8Index == 0)
            {
                this.previousCanvas1Button.Enabled = false;
            }
            this.canvas1PictureBox.Image = TestDraw8x8MapTileset(this.tileset8x8Index);
            this.canvas2PictureBox.Image = TestDraw16x16MapTileset(this.tileset8x8Index);
        }

        private void nextCanvas1Button_Click(object sender, EventArgs e)
        {
            this.tileset8x8Index++;
            this.previousCanvas1Button.Enabled = true;
            if ((this.tileset8x8Index + 1) >= (SecretOfManaContext.Default.MapContext.Map8x8TilesetTable.RowCount - 1))
            {
                this.nextCanvas1Button.Enabled = false;
            }
            this.canvas1PictureBox.Image = TestDraw8x8MapTileset(this.tileset8x8Index);
            this.canvas2PictureBox.Image = TestDraw16x16MapTileset(this.tileset8x8Index);
        }

        private SuperNintendoGraphics TestDrawMap()
        {
            //const int mapId = 0x58;
            //const int mapId = 0x57;
            int mapId = (int)numericUpDown1.Value;
            return SecretOfManaContext.Default.MapContext.CreateMap(mapId).DrawCompleteMap();
        }

        private SuperNintendoGraphics TestDrawMap2()
        {
            //const int mapId = 0x9F; // Cannon Travel
            //const int mapId = 0x83; // Potos
            int mapId = (int)numericUpDown1.Value;
            MapRomReader reader = RomReaderFactory.GetRomReader<MapRomReader>();

            MapHeader header = SecretOfManaContext.Default.MapContext.MapHeaderTable[mapId];
            MapObjectTable objectTable = SecretOfManaContext.Default.MapContext.MapObjectTable[mapId];

            Tileset tileset8x8 = SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[header.Tileset8x8Index];
            DataTable<Map16x16Tile> tileset16x16 = SecretOfManaContext.Default.MapContext.Map16x16TilesetTable[header.Tileset16x16Index];
            DataTable<SpritePalette> paletteSet = SecretOfManaContext.Default.MapContext.PaletteSetTable[header.PaletteSetIndex & 0x7F];

            MapPiece mapBackground = reader.ReadMapPiece(0, SecretOfManaContext.Default.MapContext.GetMapPieceAddress(objectTable.Layer1Background.Index));
            List<Map16x16Tile> mapTilesBG = new List<Map16x16Tile>(mapBackground.Width * mapBackground.Height);
            foreach (byte tileId in mapBackground.TileIdTable)
            {
                mapTilesBG.Add(tileset16x16[tileId]);
            }
            SuperNintendoGraphics bg = TestDrawMap(header.Tileset8x8Index, mapBackground.Width, mapBackground.Height, 0, 0, tileset8x8, mapTilesBG, paletteSet);
            foreach (MapObject mapObject in objectTable.Layer1)
            {
                MapPiece mapPiece = reader.ReadMapPiece(0, SecretOfManaContext.Default.MapContext.GetMapPieceAddress(mapObject.Index));
                List<Map16x16Tile> mapTilesObj = new List<Map16x16Tile>(mapPiece.Width * mapPiece.Height);
                foreach (byte tileId in mapPiece.TileIdTable)
                {
                    mapTilesObj.Add(tileset16x16[tileId]);
                }
                using SuperNintendoGraphics obj = TestDrawMap(header.Tileset8x8Index, mapPiece.Width, mapPiece.Height, 0, 0, tileset8x8, mapTilesObj, paletteSet);
                bg.Merge(obj, mapObject.Coordinates.X * 8, mapObject.Coordinates.Y * 8);
            }

            MapPiece mapForeground = reader.ReadMapPiece(0, SecretOfManaContext.Default.MapContext.GetMapPieceAddress(objectTable.Layer2Background.Index));
            List<Map16x16Tile> mapTilesFG = new List<Map16x16Tile>(mapForeground.Width * mapForeground.Height);
            foreach (byte tileId in mapForeground.TileIdTable)
            {
                mapTilesFG.Add(tileset16x16[tileId + 192]);
            }

            using SuperNintendoGraphics fg = TestDrawMap(header.Tileset8x8Index, mapForeground.Width, mapForeground.Height, 0, 0, tileset8x8, mapTilesFG, paletteSet);
            bg.Merge(fg);
            return bg;
        }

        public SuperNintendoGraphics TestDrawMap(int index, int width, int height, int xOffset, int yOffset, Tileset map8x8Tileset, List<Map16x16Tile> map16x16Tileset, DataTable<SpritePalette> paletteSet)
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, width * 16, height * 16);

            int rowIndex = 0;
            int columnIndex = 0;
            for (int i = 0; i < map16x16Tileset.Count; i++)
            {
                Map16x16Tile tile = map16x16Tileset[i];
                GraphicTile quadrant1 = map8x8Tileset[tile.Quadrants[0].TileIndex];
                GraphicTile quadrant2 = map8x8Tileset[tile.Quadrants[1].TileIndex];
                GraphicTile quadrant3 = map8x8Tileset[tile.Quadrants[2].TileIndex];
                GraphicTile quadrant4 = map8x8Tileset[tile.Quadrants[3].TileIndex];

                graphics.DrawTile(quadrant1, (rowIndex * 16) + xOffset, (columnIndex * 16) + yOffset, tile.Quadrants[0].FlipType, tile.Quadrants[0].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant2, (rowIndex * 16) + 8 + xOffset, (columnIndex * 16) + yOffset, tile.Quadrants[1].FlipType, tile.Quadrants[1].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant3, (rowIndex * 16) + xOffset, (columnIndex * 16) + 8 + yOffset, tile.Quadrants[2].FlipType, tile.Quadrants[2].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant4, (rowIndex * 16) + 8 + xOffset, (columnIndex * 16) + 8 + yOffset, tile.Quadrants[3].FlipType, tile.Quadrants[3].ModifiedPaletteIndex);

                rowIndex++;
                // Wrap around to the start of the next row if we have reached the end of the current row.
                if (rowIndex == width)
                {
                    rowIndex = 0;
                    columnIndex++;
                }
            }
            return graphics;
        }

        private (byte Width, byte Height, byte[] Map) ReadPotos(int address)
        {
            GenericRomReader reader = RomReaderFactory.GetRomReader<GenericRomReader>();
            reader.Seek(address);

            byte width = (byte)((reader.Read() / 2) + 1);
            byte height = (byte)((reader.Read() / 2) + 1);
            int tileCount = width * height;
            byte[] map = new byte[tileCount];

            int tilesIndex = 0;
            while (tilesIndex < tileCount)
            {
                byte current = reader.Read();
                if (current <= 0xBF)
                {
                    map[tilesIndex] = current;
                    tilesIndex++;
                }
                else if ((current >= 0xC0) && (current <= 0xDF))
                {
                    // Get Tile XX Spaces Back, Repeat It YY Number Of Times.
                    //int val = current;
                    byte length = (byte)((current & 0B_0000_0111) + 1);
                    byte backCount = (byte)(((current & 0B_0001_1000) >> 0x03) + 1);
                    byte tileId = map[tilesIndex - backCount];


                    for (int j = 0; j < length; j++)
                    {
                        map[tilesIndex] = tileId;
                        tilesIndex++;
                    }
                }
                else if (current == 0xE0) // 11100000
                {
                    // Go Up 1/2 Row(s), Repeat Tiles From There XX Times.

                    // Remember dumbass, replication is not emulation.
                    // The game reserves 0x80 bytes per row regardless of the actual width of the map.
                    // So to go up a row the game subtracts 0x80 from the current location. 
                    // Since we are defining the truth width of the map we have to subtract the width instead.

                    // If Bit 7 of the operand is not set then we go up a single row. If it is set then we go up two rows instead.
                    // TilesToRepeat = [(operand & 0x7F) + 1], meaning at most we can repeat an entire row with this command.
                    int val = width;
                    byte operand = reader.Read();
                    if ((operand & 0x80) != 0) { val *= 2; }
                    operand &= 0x7F;
                    operand++;

                    for (int j = 0; j < operand; j++)
                    {
                        map[tilesIndex] = map[tilesIndex - val];
                        tilesIndex++;
                    }
                }
                else if ((current >= 0xE1) && (current <= 0xE7))
                {
                    // Go up one row and repeat XX + 1 tiles.
                    byte numTiles = (byte)((current & 0x07) + 1);


                    for (int j = 0; j < numTiles; j++)
                    {
                        map[tilesIndex] = map[tilesIndex - width];
                        tilesIndex++;
                    }
                }
                else if ((current >= 0xE8) && (current <= 0xEF))
                {
                    byte operand = reader.Read();
                    // We'll be going backwards this many tiles from the current location to get a tile to draw.
                    // Fuck this still doesn't work right.
                    //ushort backCount = (ushort)((readByte & 0x07) | (operand >> 7));

                    // This seems to be the right way?
                    //ushort backCount = (ushort)((readByte & 0x07) + (operand >> 7));

                    // Third Attempt
                    ushort backCount = (ushort)(((current & 0x07) * 2) + ((operand >> 7)));

                    //backCount *= 2;

                    // And we'll draw it this many times.
                    ushort drawCount = (ushort)((operand & 0x7F) + 1);

                    for (int j = 0; j < drawCount; j++)
                    {
                        if (tilesIndex < tileCount)
                        {
                            map[tilesIndex] = map[(tilesIndex - backCount) - 1];
                            tilesIndex++;
                        }
                    }
                }
                else if (current == 0xF0)
                {
                    byte operand = reader.Read();
                    bool forward = (operand < 0x80);
                    int count = (operand & 0x7F) + 1;


                    byte t = map[tilesIndex - 1];
                    for (int j = 0; j < count; j++)
                    {
                        if (forward) { t++; }
                        else { t--; }
                        map[tilesIndex] = t;
                        tilesIndex++;
                    }
                }
                else if ((current >= 0xF1) && (current <= 0xF7))
                {
                    // Fill XX Spaces, Increment Tile Id Each Time.
                    int tilesToDraw = (current & 0x07) + 1;

                    // Get the last tile Id.
                    byte t = map[tilesIndex - 1];

                    for (int j = 0; j < tilesToDraw; j++)
                    {
                        t++;
                        map[tilesIndex] = t;
                        tilesIndex++;
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            return (width, height, map);
        }

        private Bitmap TestDraw8x8MapTileset(int index)
        {
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[index];
            if (!metadata.IsDummiedOut)
            {
                Tileset tileset = SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[metadata.Index];
                SpritePalette palette = SecretOfManaContext.Default.MapContext.PaletteSetTable[metadata.DefaultPaletteSet][metadata.DefaultPaletteIndex];

                using SuperNintendoGraphics graphics = tileset.DrawTileset(palette, 32);
                return new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }
            return null;
        }

        private Bitmap TestDraw16x16MapTileset(int index)
        {
            MapTilesetMetadata metadata = ManaMetadata.Map8x8TilesetMetadata[index];
            if (!metadata.IsDummiedOut)
            {
                Tileset tileset8x8 = SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[metadata.Index];
                DataTable<Map16x16Tile> tileset16x16 = SecretOfManaContext.Default.MapContext.Map16x16TilesetTable[metadata.Index];
                DataTable<SpritePalette> paletteSet = SecretOfManaContext.Default.MapContext.PaletteSetTable[metadata.DefaultPaletteSet];

                using SuperNintendoGraphics graphics = this.TestDrawMap16x16(0x00, tileset8x8, tileset16x16, paletteSet);
                return new Bitmap(graphics.GetBitmap(true), graphics.Size.Width * 2, graphics.Size.Height * 2);
            }
            return null;
        }

        public SuperNintendoGraphics TestDrawMap16x16(int index, Tileset map8x8Tileset, DataTable<Map16x16Tile> map16x16Tileset, DataTable<SpritePalette> paletteSet)
        {
            SuperNintendoGraphics graphics = SuperNintendoGraphics.CreateGraphics(paletteSet, 32 * 16, 32 * 12);

            int rowIndex = 0;
            int columnIndex = 0;
            for (int i = 0; i < 192; i++)//map16x16Tileset.Count
            //for (int i = 192; i < 384; i++)//map16x16Tileset.Count
            {
                Map16x16Tile tile = map16x16Tileset[i];
                GraphicTile quadrant1 = map8x8Tileset[tile.Quadrants[0].TileIndex];
                GraphicTile quadrant2 = map8x8Tileset[tile.Quadrants[1].TileIndex];
                GraphicTile quadrant3 = map8x8Tileset[tile.Quadrants[2].TileIndex];
                GraphicTile quadrant4 = map8x8Tileset[tile.Quadrants[3].TileIndex];

                graphics.DrawTile(quadrant1, (rowIndex * 16), (columnIndex * 16), tile.Quadrants[0].FlipType, tile.Quadrants[0].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant2, (rowIndex * 16) + 8, (columnIndex * 16), tile.Quadrants[1].FlipType, tile.Quadrants[1].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant3, (rowIndex * 16), (columnIndex * 16) + 8, tile.Quadrants[2].FlipType, tile.Quadrants[2].ModifiedPaletteIndex);
                graphics.DrawTile(quadrant4, (rowIndex * 16) + 8, (columnIndex * 16) + 8, tile.Quadrants[3].FlipType, tile.Quadrants[3].ModifiedPaletteIndex);

                rowIndex++;
                // Wrap around to the start of the next row if we have reached the end of the current row.
                if (rowIndex == 16)
                {
                    rowIndex = 0;
                    columnIndex++;
                }
            }
            return graphics;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.TestDrawMapEx();
           // try
            //{
            //    this.canvas1PictureBox.Image = this.TestDrawMap().GetBitmap(true);
            //}
            //catch (Exception ex)
            //{
            //    this.canvas1PictureBox.Image = null;
            //    LoggerEngine.Logger.LogError(LogComponent.General, ex.Message);
            //}
        }

        private void TestDrawMapEx()
        {
            //const int mapId = 0x58;
            //const int mapId = 0x57;
            int mapId = (int)numericUpDown1.Value;
            try
            {
                ManaMap map = SecretOfManaContext.Default.MapContext.CreateMap(mapId);
                if (map.IsValid)
                {
                    using SuperNintendoGraphics graphics = SecretOfManaContext.Default.MapContext.CreateMap(mapId).DrawCompleteMap();
                    this.canvas1PictureBox.Image = graphics.GetBitmap(true);
                    return;
                }
                this.canvas1PictureBox.Image = null;

                if (!map.Header.IsValid && !map.ObjectTable.IsValid)
                {
                    LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Header and Object Table are invalid.");
                }
                else if (!map.Header.IsValid)
                {
                    LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Header is invalid.");
                }
                else if (!map.ObjectTable.IsValid)
                {
                    LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Object Table is invalid.");
                }
                else { LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}."); }
            }
            catch (Exception ex)
            {
                this.canvas1PictureBox.Image = null;
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
            }
        }

        private void TestDrawMapEx2()
        {
            //const int mapId = 0x58;
            //const int mapId = 0x57;
            int mapId = (int)numericUpDown1.Value;
            ManaMap map = SecretOfManaContext.Default.MapContext.CreateMap(mapId);
            if (map.IsValid)
            {
                using SuperNintendoGraphics graphics = this.TestDrawMap2();
                this.canvas1PictureBox.Image = graphics.GetBitmap(true);
                return;
            }
            this.canvas1PictureBox.Image = null;
            LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}");
        }
 */

/*
$00/F706 A7 10       LDA [$10]  [$D3:3D29]   A:2A06 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1034 VC:027 FC:55 I:01
$00/F708 E6 10       INC $10    [$00:0010]   A:6CF8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1130 VC:027 FC:55 I:01
$00/F70A E6 10       INC $10    [$00:0010]   A:6CF8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1200 VC:027 FC:55 I:01
$00/F70C E2 20       SEP #$20                A:6CF8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1270 VC:027 FC:55 I:01
$00/F70E EB          XBA                     A:6CF8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1308 VC:027 FC:55 I:01
$00/F70F 0A          ASL A                   A:F86C X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1344 VC:027 FC:55 I:01
$00/F710 66 00       ROR $00    [$00:0000]   A:F8D8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0010 VC:028 FC:55 I:01
$00/F712 EB          XBA                     A:F8D8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0064 VC:028 FC:55 I:01
$00/F713 0A          ASL A                   A:D8F8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0100 VC:028 FC:55 I:01
$00/F714 66 00       ROR $00    [$00:0000]   A:D8F0 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzC HC:0130 VC:028 FC:55 I:01
$00/F716 C9 80       CMP #$80                A:D8F0 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0184 VC:028 FC:55 I:01
$00/F718 90 07       BCC $07    [$F721]      A:D8F0 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzC HC:0224 VC:028 FC:55 I:01
$00/F71A 6A          ROR A                   A:D8F0 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzC HC:0264 VC:028 FC:55 I:01
$00/F71B 24 02       BIT $02    [$00:0002]   A:D8F8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0302 VC:028 FC:55 I:01
$00/F71D 30 07       BMI $07    [$F726]      A:D8F8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0350 VC:028 FC:55 I:01
$00/F71F 10 0A       BPL $0A    [$F72B]      A:D8F8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0390 VC:028 FC:55 I:01
$00/F72B EB          XBA                     A:D8F8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0436 VC:028 FC:55 I:01
$00/F72C 10 08       BPL $08    [$F736]      A:F8D8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0480 VC:028 FC:55 I:01
$00/F72E 38          SEC                     A:F8D8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0520 VC:028 FC:55 I:01
$00/F72F 6A          ROR A                   A:F8D8 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzC HC:0598 VC:028 FC:55 I:01
$00/F730 24 02       BIT $02    [$00:0002]   A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0636 VC:028 FC:55 I:01
$00/F732 70 07       BVS $07    [$F73B]      A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0684 VC:028 FC:55 I:01
$00/F734 50 0A       BVC $0A    [$F740]      A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0724 VC:028 FC:55 I:01
$00/F740 C2 20       REP #$20                A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0770 VC:028 FC:55 I:01
$00/F742 9D 90 E0    STA $E090,x[$7E:E690]   A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0816 VC:028 FC:55 I:01
$00/F745 E2 20       SEP #$20                A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0878 VC:028 FC:55 I:01
$00/F747 A5 00       LDA $00    [$00:0000]   A:F8EC X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0916 VC:028 FC:55 I:01
$00/F749 10 0F       BPL $0F    [$F75A]      A:F882 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0956 VC:028 FC:55 I:01
$00/F74B 24 01       BIT $01    [$00:0001]   A:F882 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0988 VC:028 FC:55 I:01
$00/F74D 10 0B       BPL $0B    [$F75A]      A:F882 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1028 VC:028 FC:55 I:01
$00/F75A 29 40       AND #$40                A:F882 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1066 VC:028 FC:55 I:01
$00/F75C 45 02       EOR $02    [$00:0002]   A:F800 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1098 VC:028 FC:55 I:01
$00/F75E 05 0D       ORA $0D    [$00:000D]   A:F800 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1138 VC:028 FC:55 I:01
$00/F760 9D 93 E0    STA $E093,x[$7E:E693]   A:F83A X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1178 VC:028 FC:55 I:01
$00/F763 A5 0C       LDA $0C    [$00:000C]   A:F83A X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1232 VC:028 FC:55 I:01
$00/F765 9D 92 E0    STA $E092,x[$7E:E692]   A:F840 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1272 VC:028 FC:55 I:01
$00/F768 18          CLC                     A:F840 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1326 VC:028 FC:55 I:01
$00/F769 69 02       ADC #$02                A:F840 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1356 VC:028 FC:55 I:01
$00/F76B 89 10       BIT #$10                A:F842 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0024 VC:029 FC:55 I:01
$00/F76D F0 06       BEQ $06    [$F775]      A:F842 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0056 VC:029 FC:55 I:01
$00/F775 85 0C       STA $0C    [$00:000C]   A:F842 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0094 VC:029 FC:55 I:01
$00/F777 E8          INX                     A:F842 X:0600 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0134 VC:029 FC:55 I:01
$00/F778 E8          INX                     A:F842 X:0601 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0164 VC:029 FC:55 I:01
$00/F779 E8          INX                     A:F842 X:0602 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0194 VC:029 FC:55 I:01
$00/F77A E8          INX                     A:F842 X:0603 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0224 VC:029 FC:55 I:01
$00/F77B 88          DEY                     A:F842 X:0604 Y:0002 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0254 VC:029 FC:55 I:01
$00/F77C F0 03       BEQ $03    [$F781]      A:F842 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0284 VC:029 FC:55 I:01
$00/F77E 82 83 FF    BRL $FF83  [$F704]      A:F842 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0316 VC:029 FC:55 I:01
--------------------------------------------------------------------------------------------------
$00/F706 A7 10       LDA [$10]  [$D3:3D2B]   A:F842 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0418 VC:029 FC:55 I:01
$00/F708 E6 10       INC $10    [$00:0010]   A:7CF8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0514 VC:029 FC:55 I:01
$00/F70A E6 10       INC $10    [$00:0010]   A:7CF8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0624 VC:029 FC:55 I:01
$00/F70C E2 20       SEP #$20                A:7CF8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0694 VC:029 FC:55 I:01
$00/F70E EB          XBA                     A:7CF8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0732 VC:029 FC:55 I:01
$00/F70F 0A          ASL A                   A:F87C X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0768 VC:029 FC:55 I:01
$00/F710 66 00       ROR $00    [$00:0000]   A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0798 VC:029 FC:55 I:01
$00/F712 EB          XBA                     A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0852 VC:029 FC:55 I:01
$00/F713 0A          ASL A                   A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0888 VC:029 FC:55 I:01
$00/F714 66 00       ROR $00    [$00:0000]   A:F8F0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzC HC:0918 VC:029 FC:55 I:01
$00/F716 C9 80       CMP #$80                A:F8F0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzC HC:0972 VC:029 FC:55 I:01
$00/F718 90 07       BCC $07    [$F721]      A:F8F0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzC HC:1004 VC:029 FC:55 I:01
$00/F71A 6A          ROR A                   A:F8F0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzC HC:1036 VC:029 FC:55 I:01
$00/F71B 24 02       BIT $02    [$00:0002]   A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:1066 VC:029 FC:55 I:01
$00/F71D 30 07       BMI $07    [$F726]      A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1106 VC:029 FC:55 I:01
$00/F71F 10 0A       BPL $0A    [$F72B]      A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1138 VC:029 FC:55 I:01
$00/F72B EB          XBA                     A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1176 VC:029 FC:55 I:01
$00/F72C 10 08       BPL $08    [$F736]      A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:1212 VC:029 FC:55 I:01
$00/F72E 38          SEC                     A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:1244 VC:029 FC:55 I:01
$00/F72F 6A          ROR A                   A:F8F8 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzC HC:1274 VC:029 FC:55 I:01
$00/F730 24 02       BIT $02    [$00:0002]   A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:1304 VC:029 FC:55 I:01
$00/F732 70 07       BVS $07    [$F73B]      A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1344 VC:029 FC:55 I:01
$00/F734 50 0A       BVC $0A    [$F740]      A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0012 VC:030 FC:55 I:01
$00/F740 C2 20       REP #$20                A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0050 VC:030 FC:55 I:01
$00/F742 9D 90 E0    STA $E090,x[$7E:E694]   A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0088 VC:030 FC:55 I:01
$00/F745 E2 20       SEP #$20                A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0150 VC:030 FC:55 I:01
$00/F747 A5 00       LDA $00    [$00:0000]   A:F8FC X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0188 VC:030 FC:55 I:01
$00/F749 10 0F       BPL $0F    [$F75A]      A:F8A0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0228 VC:030 FC:55 I:01
$00/F74B 24 01       BIT $01    [$00:0001]   A:F8A0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:eNvMxdIzc HC:0260 VC:030 FC:55 I:01
$00/F74D 10 0B       BPL $0B    [$F75A]      A:F8A0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0300 VC:030 FC:55 I:01
$00/F75A 29 40       AND #$40                A:F8A0 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0338 VC:030 FC:55 I:01
$00/F75C 45 02       EOR $02    [$00:0002]   A:F800 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0370 VC:030 FC:55 I:01
$00/F75E 05 0D       ORA $0D    [$00:000D]   A:F800 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0410 VC:030 FC:55 I:01
$00/F760 9D 93 E0    STA $E093,x[$7E:E697]   A:F83A X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0450 VC:030 FC:55 I:01
$00/F763 A5 0C       LDA $0C    [$00:000C]   A:F83A X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0504 VC:030 FC:55 I:01
$00/F765 9D 92 E0    STA $E092,x[$7E:E696]   A:F842 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0544 VC:030 FC:55 I:01
$00/F768 18          CLC                     A:F842 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0638 VC:030 FC:55 I:01
$00/F769 69 02       ADC #$02                A:F842 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0668 VC:030 FC:55 I:01
$00/F76B 89 10       BIT #$10                A:F844 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0700 VC:030 FC:55 I:01
$00/F76D F0 06       BEQ $06    [$F775]      A:F844 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0732 VC:030 FC:55 I:01
$00/F775 85 0C       STA $0C    [$00:000C]   A:F844 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0770 VC:030 FC:55 I:01
$00/F777 E8          INX                     A:F844 X:0604 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0810 VC:030 FC:55 I:01
$00/F778 E8          INX                     A:F844 X:0605 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0840 VC:030 FC:55 I:01
$00/F779 E8          INX                     A:F844 X:0606 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0870 VC:030 FC:55 I:01
$00/F77A E8          INX                     A:F844 X:0607 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0900 VC:030 FC:55 I:01
$00/F77B 88          DEY                     A:F844 X:0608 Y:0001 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0930 VC:030 FC:55 I:01
$00/F77C F0 03       BEQ $03    [$F781]      A:F844 X:0608 Y:0000 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0960 VC:030 FC:55 I:01
$00/F781 E2 20       SEP #$20                A:F844 X:0608 Y:0000 D:0000 DB:7E S:01F4 P:envMxdIZc HC:0998 VC:030 FC:55 I:01
$00/F783 24 ED       BIT $ED    [$00:00ED]   A:F844 X:0608 Y:0000 D:0000 DB:7E S:01F4 P:envMxdIZc HC:1036 VC:030 FC:55 I:01
$00/F785 30 01       BMI $01    [$F788]      A:F844 X:0608 Y:0000 D:0000 DB:7E S:01F4 P:eNvMxdIZc HC:1076 VC:030 FC:55 I:01
$00/F788 A6 14       LDX $14    [$00:0014]   A:F844 X:0608 Y:0000 D:0000 DB:7E S:01F4 P:eNvMxdIZc HC:1114 VC:030 FC:55 I:01
$00/F78A A9 60       LDA #$60                A:F844 X:0600 Y:0000 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1162 VC:030 FC:55 I:01
$00/F78C 24 04       BIT $04    [$00:0004]   A:F860 X:0600 Y:0000 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1194 VC:030 FC:55 I:01
$00/F78E D0 0C       BNE $0C    [$F79C]      A:F860 X:0600 Y:0000 D:0000 DB:7E S:01F4 P:enVMxdIzc HC:1234 VC:030 FC:55 I:01
$00/F79C A0 00 00    LDY #$0000              A:F860 X:0600 Y:0000 D:0000 DB:7E S:01F4 P:enVMxdIzc HC:1272 VC:030 FC:55 I:01
$00/F79F A9 40       LDA #$40                A:F860 X:0600 Y:0000 D:0000 DB:7E S:01F4 P:enVMxdIZc HC:1312 VC:030 FC:55 I:01
$00/F7A1 24 04       BIT $04    [$00:0004]   A:F840 X:0600 Y:0000 D:0000 DB:7E S:01F4 P:enVMxdIzc HC:1344 VC:030 FC:55 I:01
*/
/*
$00/F657 A7 10       LDA [$10]  [$D3:3D56]   A:6540 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0906 VC:053 FC:25 I:01
$00/F659 85 04       STA $04    [$00:0004]   A:6543 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:0994 VC:053 FC:25 I:01
$00/F65B 29 0F       AND #$0F                A:6543 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1034 VC:053 FC:25 I:01
$00/F65D D0 02       BNE $02    [$F661]      A:6503 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1066 VC:053 FC:25 I:01
$00/F661 9D 83 E0    STA $E083,x[$7E:EA83]   A:6503 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1104 VC:053 FC:25 I:01
$00/F664 0A          ASL A                   A:6503 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1158 VC:053 FC:25 I:01
$00/F665 0A          ASL A                   A:6506 X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1188 VC:053 FC:25 I:01
$00/F666 85 00       STA $00    [$00:0000]   A:650C X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1218 VC:053 FC:25 I:01
$00/F668 C2 20       REP #$20                A:650C X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envMxdIzc HC:1258 VC:053 FC:25 I:01
$00/F66A E6 10       INC $10    [$00:0010]   A:650C X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1296 VC:053 FC:25 I:01
$00/F66C A6 00       LDX $00    [$00:0000]   A:650C X:0A00 Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1366 VC:053 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D57]   A:650C X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0074 VC:054 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:8D51 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0170 VC:054 FC:25 I:01
$00/F686 E6 10       INC $10    [$00:0010]   A:8D51 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0208 VC:054 FC:25 I:01
$00/F688 E6 10       INC $10    [$00:0010]   A:8D51 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0278 VC:054 FC:25 I:01
$00/F68A 48          PHA                     A:8D51 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0348 VC:054 FC:25 I:01
$00/F68B 29 0F 00    AND #$000F              A:8D51 X:000C Y:0F20 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0394 VC:054 FC:25 I:01
$00/F68E 85 02       STA $02    [$00:0002]   A:0001 X:000C Y:0F20 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0434 VC:054 FC:25 I:01
$00/F690 68          PLA                     A:0001 X:000C Y:0F20 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0482 VC:054 FC:25 I:01
$00/F691 0A          ASL A                   A:8D51 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0534 VC:054 FC:25 I:01
$00/F692 10 18       BPL $18    [$F6AC]      A:1AA2 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzC HC:0604 VC:054 FC:25 I:01
$00/F6AC 29 E0 7F    AND #$7FE0              A:1AA2 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzC HC:0642 VC:054 FC:25 I:01
$00/F6AF 18          CLC                     A:1AA0 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzC HC:0682 VC:054 FC:25 I:01
$00/F6B0 65 0A       ADC $0A    [$00:000A]   A:1AA0 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0712 VC:054 FC:25 I:01
$00/F6B2 99 00 00    STA $0000,y[$7E:0F20]   A:7FE0 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0760 VC:054 FC:25 I:01
$00/F6B5 C8          INY                     A:7FE0 X:000C Y:0F20 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0822 VC:054 FC:25 I:01
$00/F6B6 C8          INY                     A:7FE0 X:000C Y:0F21 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0852 VC:054 FC:25 I:01
$00/F6B7 CA          DEX                     A:7FE0 X:000C Y:0F22 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0882 VC:054 FC:25 I:01
$00/F6B8 F0 06       BEQ $06    [$F6C0]      A:7FE0 X:000B Y:0F22 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0912 VC:054 FC:25 I:01
$00/F6BA C6 02       DEC $02    [$00:0002]   A:7FE0 X:000B Y:0F22 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0944 VC:054 FC:25 I:01
$00/F6BC 10 F4       BPL $F4    [$F6B2]      A:7FE0 X:000B Y:0F22 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1014 VC:054 FC:25 I:01
$00/F6B2 99 00 00    STA $0000,y[$7E:0F22]   A:7FE0 X:000B Y:0F22 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1052 VC:054 FC:25 I:01
$00/F6B5 C8          INY                     A:7FE0 X:000B Y:0F22 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1114 VC:054 FC:25 I:01
$00/F6B6 C8          INY                     A:7FE0 X:000B Y:0F23 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1144 VC:054 FC:25 I:01
$00/F6B7 CA          DEX                     A:7FE0 X:000B Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1174 VC:054 FC:25 I:01
$00/F6B8 F0 06       BEQ $06    [$F6C0]      A:7FE0 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1204 VC:054 FC:25 I:01
$00/F6BA C6 02       DEC $02    [$00:0002]   A:7FE0 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1236 VC:054 FC:25 I:01
$00/F6BC 10 F4       BPL $F4    [$F6B2]      A:7FE0 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1306 VC:054 FC:25 I:01
$00/F6BE 30 AE       BMI $AE    [$F66E]      A:7FE0 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1338 VC:054 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D59]   A:7FE0 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0036 VC:055 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:C121 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0132 VC:055 FC:25 I:01
$00/F686 E6 10       INC $10    [$00:0010]   A:C121 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0170 VC:055 FC:25 I:01
$00/F688 E6 10       INC $10    [$00:0010]   A:C121 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0240 VC:055 FC:25 I:01
$00/F68A 48          PHA                     A:C121 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0310 VC:055 FC:25 I:01
$00/F68B 29 0F 00    AND #$000F              A:C121 X:000A Y:0F24 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0356 VC:055 FC:25 I:01
$00/F68E 85 02       STA $02    [$00:0002]   A:0001 X:000A Y:0F24 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0396 VC:055 FC:25 I:01
$00/F690 68          PLA                     A:0001 X:000A Y:0F24 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0444 VC:055 FC:25 I:01
$00/F691 0A          ASL A                   A:C121 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0496 VC:055 FC:25 I:01
$00/F692 10 18       BPL $18    [$F6AC]      A:8242 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzC HC:0526 VC:055 FC:25 I:01
$00/F694 29 E0 7F    AND #$7FE0              A:8242 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:eNvmxdIzC HC:0598 VC:055 FC:25 I:01
$00/F697 18          CLC                     A:0240 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzC HC:0638 VC:055 FC:25 I:01
$00/F698 65 0A       ADC $0A    [$00:000A]   A:0240 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0668 VC:055 FC:25 I:01
$00/F69A 99 00 00    STA $0000,y[$7E:0F24]   A:6780 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0716 VC:055 FC:25 I:01
$00/F69D C8          INY                     A:6780 X:000A Y:0F24 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0778 VC:055 FC:25 I:01
$00/F69E C8          INY                     A:6780 X:000A Y:0F25 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0808 VC:055 FC:25 I:01
$00/F69F CA          DEX                     A:6780 X:000A Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0838 VC:055 FC:25 I:01
$00/F6A0 F0 1E       BEQ $1E    [$F6C0]      A:6780 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0868 VC:055 FC:25 I:01
$00/F6A2 C6 02       DEC $02    [$00:0002]   A:6780 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0900 VC:055 FC:25 I:01
$00/F6A4 30 C8       BMI $C8    [$F66E]      A:6780 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0970 VC:055 FC:25 I:01
$00/F6A6 18          CLC                     A:6780 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1002 VC:055 FC:25 I:01
$00/F6A7 69 20 00    ADC #$0020              A:6780 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1032 VC:055 FC:25 I:01
$00/F6AA 80 EE       BRA $EE    [$F69A]      A:67A0 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1072 VC:055 FC:25 I:01
$00/F69A 99 00 00    STA $0000,y[$7E:0F26]   A:67A0 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1110 VC:055 FC:25 I:01
$00/F69D C8          INY                     A:67A0 X:0009 Y:0F26 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1172 VC:055 FC:25 I:01
$00/F69E C8          INY                     A:67A0 X:0009 Y:0F27 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1202 VC:055 FC:25 I:01
$00/F69F CA          DEX                     A:67A0 X:0009 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1232 VC:055 FC:25 I:01
$00/F6A0 F0 1E       BEQ $1E    [$F6C0]      A:67A0 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1262 VC:055 FC:25 I:01
$00/F6A2 C6 02       DEC $02    [$00:0002]   A:67A0 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1294 VC:055 FC:25 I:01
$00/F6A4 30 C8       BMI $C8    [$F66E]      A:67A0 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1364 VC:055 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D5B]   A:67A0 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0062 VC:056 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:0160 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0158 VC:056 FC:25 I:01
$00/F672 E6 10       INC $10    [$00:0010]   A:0160 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0190 VC:056 FC:25 I:01
$00/F674 E6 10       INC $10    [$00:0010]   A:0160 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0260 VC:056 FC:25 I:01
$00/F676 29 F0 3F    AND #$3FF0              A:0160 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0330 VC:056 FC:25 I:01
$00/F679 0A          ASL A                   A:0160 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0370 VC:056 FC:25 I:01
$00/F67A 65 0A       ADC $0A    [$00:000A]   A:02C0 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0400 VC:056 FC:25 I:01
$00/F67C 99 00 00    STA $0000,y[$7E:0F28]   A:6800 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0448 VC:056 FC:25 I:01
$00/F67F C8          INY                     A:6800 X:0008 Y:0F28 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0510 VC:056 FC:25 I:01
$00/F680 C8          INY                     A:6800 X:0008 Y:0F29 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0540 VC:056 FC:25 I:01
$00/F681 CA          DEX                     A:6800 X:0008 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0610 VC:056 FC:25 I:01
$00/F682 D0 EA       BNE $EA    [$F66E]      A:6800 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0640 VC:056 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D5D]   A:6800 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0702 VC:056 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:0D50 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0798 VC:056 FC:25 I:01
$00/F672 E6 10       INC $10    [$00:0010]   A:0D50 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0830 VC:056 FC:25 I:01
$00/F674 E6 10       INC $10    [$00:0010]   A:0D50 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0900 VC:056 FC:25 I:01
$00/F676 29 F0 3F    AND #$3FF0              A:0D50 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:0970 VC:056 FC:25 I:01
$00/F679 0A          ASL A                   A:0D50 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:1010 VC:056 FC:25 I:01
$00/F67A 65 0A       ADC $0A    [$00:000A]   A:1AA0 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:1040 VC:056 FC:25 I:01
$00/F67C 99 00 00    STA $0000,y[$7E:0F2A]   A:7FE0 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:1088 VC:056 FC:25 I:01
$00/F67F C8          INY                     A:7FE0 X:0007 Y:0F2A D:0000 DB:7E S:01F4 P:envmxdIzc HC:1150 VC:056 FC:25 I:01
$00/F680 C8          INY                     A:7FE0 X:0007 Y:0F2B D:0000 DB:7E S:01F4 P:envmxdIzc HC:1180 VC:056 FC:25 I:01
$00/F681 CA          DEX                     A:7FE0 X:0007 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:1210 VC:056 FC:25 I:01
$00/F682 D0 EA       BNE $EA    [$F66E]      A:7FE0 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:1240 VC:056 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D5F]   A:7FE0 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:1302 VC:056 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:0170 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0034 VC:057 FC:25 I:01
$00/F672 E6 10       INC $10    [$00:0010]   A:0170 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0066 VC:057 FC:25 I:01
$00/F674 E6 10       INC $10    [$00:0010]   A:0170 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0136 VC:057 FC:25 I:01
$00/F676 29 F0 3F    AND #$3FF0              A:0170 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0206 VC:057 FC:25 I:01
$00/F679 0A          ASL A                   A:0170 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0246 VC:057 FC:25 I:01
$00/F67A 65 0A       ADC $0A    [$00:000A]   A:02E0 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0276 VC:057 FC:25 I:01
$00/F67C 99 00 00    STA $0000,y[$7E:0F2C]   A:6820 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0324 VC:057 FC:25 I:01
$00/F67F C8          INY                     A:6820 X:0006 Y:0F2C D:0000 DB:7E S:01F4 P:envmxdIzc HC:0386 VC:057 FC:25 I:01
$00/F680 C8          INY                     A:6820 X:0006 Y:0F2D D:0000 DB:7E S:01F4 P:envmxdIzc HC:0416 VC:057 FC:25 I:01
$00/F681 CA          DEX                     A:6820 X:0006 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0446 VC:057 FC:25 I:01
$00/F682 D0 EA       BNE $EA    [$F66E]      A:6820 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0476 VC:057 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D61]   A:6820 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0538 VC:057 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:0D50 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0674 VC:057 FC:25 I:01
$00/F672 E6 10       INC $10    [$00:0010]   A:0D50 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0706 VC:057 FC:25 I:01
$00/F674 E6 10       INC $10    [$00:0010]   A:0D50 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0776 VC:057 FC:25 I:01
$00/F676 29 F0 3F    AND #$3FF0              A:0D50 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0846 VC:057 FC:25 I:01
$00/F679 0A          ASL A                   A:0D50 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0886 VC:057 FC:25 I:01
$00/F67A 65 0A       ADC $0A    [$00:000A]   A:1AA0 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0916 VC:057 FC:25 I:01
$00/F67C 99 00 00    STA $0000,y[$7E:0F2E]   A:7FE0 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:0964 VC:057 FC:25 I:01
$00/F67F C8          INY                     A:7FE0 X:0005 Y:0F2E D:0000 DB:7E S:01F4 P:envmxdIzc HC:1026 VC:057 FC:25 I:01
$00/F680 C8          INY                     A:7FE0 X:0005 Y:0F2F D:0000 DB:7E S:01F4 P:envmxdIzc HC:1056 VC:057 FC:25 I:01
$00/F681 CA          DEX                     A:7FE0 X:0005 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1086 VC:057 FC:25 I:01
$00/F682 D0 EA       BNE $EA    [$F66E]      A:7FE0 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1116 VC:057 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D63]   A:7FE0 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1178 VC:057 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:C141 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1274 VC:057 FC:25 I:01
$00/F686 E6 10       INC $10    [$00:0010]   A:C141 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1312 VC:057 FC:25 I:01
$00/F688 E6 10       INC $10    [$00:0010]   A:C141 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0018 VC:058 FC:25 I:01
$00/F68A 48          PHA                     A:C141 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0088 VC:058 FC:25 I:01
$00/F68B 29 0F 00    AND #$000F              A:C141 X:0004 Y:0F30 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0134 VC:058 FC:25 I:01
$00/F68E 85 02       STA $02    [$00:0002]   A:0001 X:0004 Y:0F30 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0174 VC:058 FC:25 I:01
$00/F690 68          PLA                     A:0001 X:0004 Y:0F30 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0222 VC:058 FC:25 I:01
$00/F691 0A          ASL A                   A:C141 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0274 VC:058 FC:25 I:01
$00/F692 10 18       BPL $18    [$F6AC]      A:8282 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:eNvmxdIzC HC:0304 VC:058 FC:25 I:01
$00/F694 29 E0 7F    AND #$7FE0              A:8282 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:eNvmxdIzC HC:0336 VC:058 FC:25 I:01
$00/F697 18          CLC                     A:0280 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzC HC:0376 VC:058 FC:25 I:01
$00/F698 65 0A       ADC $0A    [$00:000A]   A:0280 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0406 VC:058 FC:25 I:01
$00/F69A 99 00 00    STA $0000,y[$7E:0F30]   A:67C0 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0454 VC:058 FC:25 I:01
$00/F69D C8          INY                     A:67C0 X:0004 Y:0F30 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0516 VC:058 FC:25 I:01
$00/F69E C8          INY                     A:67C0 X:0004 Y:0F31 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0586 VC:058 FC:25 I:01
$00/F69F CA          DEX                     A:67C0 X:0004 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0616 VC:058 FC:25 I:01
$00/F6A0 F0 1E       BEQ $1E    [$F6C0]      A:67C0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0646 VC:058 FC:25 I:01
$00/F6A2 C6 02       DEC $02    [$00:0002]   A:67C0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0678 VC:058 FC:25 I:01
$00/F6A4 30 C8       BMI $C8    [$F66E]      A:67C0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0748 VC:058 FC:25 I:01
$00/F6A6 18          CLC                     A:67C0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0780 VC:058 FC:25 I:01
$00/F6A7 69 20 00    ADC #$0020              A:67C0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0810 VC:058 FC:25 I:01
$00/F6AA 80 EE       BRA $EE    [$F69A]      A:67E0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0850 VC:058 FC:25 I:01
$00/F69A 99 00 00    STA $0000,y[$7E:0F32]   A:67E0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0888 VC:058 FC:25 I:01
$00/F69D C8          INY                     A:67E0 X:0003 Y:0F32 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0950 VC:058 FC:25 I:01
$00/F69E C8          INY                     A:67E0 X:0003 Y:0F33 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0980 VC:058 FC:25 I:01
$00/F69F CA          DEX                     A:67E0 X:0003 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1010 VC:058 FC:25 I:01
$00/F6A0 F0 1E       BEQ $1E    [$F6C0]      A:67E0 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1040 VC:058 FC:25 I:01
$00/F6A2 C6 02       DEC $02    [$00:0002]   A:67E0 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1072 VC:058 FC:25 I:01
$00/F6A4 30 C8       BMI $C8    [$F66E]      A:67E0 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1142 VC:058 FC:25 I:01

$00/F66E A7 10       LDA [$10]  [$D3:3D65]   A:67E0 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1204 VC:058 FC:25 I:01
$00/F670 30 14       BMI $14    [$F686]      A:C181 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1300 VC:058 FC:25 I:01
$00/F686 E6 10       INC $10    [$00:0010]   A:C181 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:1338 VC:058 FC:25 I:01
$00/F688 E6 10       INC $10    [$00:0010]   A:C181 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0044 VC:059 FC:25 I:01
$00/F68A 48          PHA                     A:C181 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0114 VC:059 FC:25 I:01
$00/F68B 29 0F 00    AND #$000F              A:C181 X:0002 Y:0F34 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0160 VC:059 FC:25 I:01
$00/F68E 85 02       STA $02    [$00:0002]   A:0001 X:0002 Y:0F34 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0200 VC:059 FC:25 I:01
$00/F690 68          PLA                     A:0001 X:0002 Y:0F34 D:0000 DB:7E S:01F2 P:envmxdIzc HC:0248 VC:059 FC:25 I:01
$00/F691 0A          ASL A                   A:C181 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzc HC:0300 VC:059 FC:25 I:01
$00/F692 10 18       BPL $18    [$F6AC]      A:8302 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzC HC:0330 VC:059 FC:25 I:01
$00/F694 29 E0 7F    AND #$7FE0              A:8302 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:eNvmxdIzC HC:0362 VC:059 FC:25 I:01
$00/F697 18          CLC                     A:0300 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzC HC:0402 VC:059 FC:25 I:01
$00/F698 65 0A       ADC $0A    [$00:000A]   A:0300 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0432 VC:059 FC:25 I:01
$00/F69A 99 00 00    STA $0000,y[$7E:0F34]   A:6840 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0480 VC:059 FC:25 I:01
$00/F69D C8          INY                     A:6840 X:0002 Y:0F34 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0542 VC:059 FC:25 I:01
$00/F69E C8          INY                     A:6840 X:0002 Y:0F35 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0612 VC:059 FC:25 I:01
$00/F69F CA          DEX                     A:6840 X:0002 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0642 VC:059 FC:25 I:01
$00/F6A0 F0 1E       BEQ $1E    [$F6C0]      A:6840 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0672 VC:059 FC:25 I:01
$00/F6A2 C6 02       DEC $02    [$00:0002]   A:6840 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0704 VC:059 FC:25 I:01
$00/F6A4 30 C8       BMI $C8    [$F66E]      A:6840 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0774 VC:059 FC:25 I:01
$00/F6A6 18          CLC                     A:6840 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0806 VC:059 FC:25 I:01
$00/F6A7 69 20 00    ADC #$0020              A:6840 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIZc HC:0836 VC:059 FC:25 I:01
$00/F6AA 80 EE       BRA $EE    [$F69A]      A:6860 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0876 VC:059 FC:25 I:01
$00/F69A 99 00 00    STA $0000,y[$7E:0F36]   A:6860 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0914 VC:059 FC:25 I:01
$00/F69D C8          INY                     A:6860 X:0001 Y:0F36 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0976 VC:059 FC:25 I:01
$00/F69E C8          INY                     A:6860 X:0001 Y:0F37 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1006 VC:059 FC:25 I:01
$00/F69F CA          DEX                     A:6860 X:0001 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1036 VC:059 FC:25 I:01
$00/F6A0 F0 1E       BEQ $1E    [$F6C0]      A:6860 X:0000 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1066 VC:059 FC:25 I:01
$00/F6C0 C2 30       REP #$30                A:6860 X:0000 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1104 VC:059 FC:25 I:01
$00/F6C2 A6 14       LDX $14    [$00:0014]   A:6860 X:0000 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIZc HC:1142 VC:059 FC:25 I:01
$00/F6C4 BD 83 E0    LDA $E083,x[$7E:EA83]   A:6860 X:0A00 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1190 VC:059 FC:25 I:01
$00/F6C7 29 0F 00    AND #$000F              A:0003 X:0A00 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1252 VC:059 FC:25 I:01
$00/F6CA A8          TAY                     A:0003 X:0A00 Y:0F38 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1292 VC:059 FC:25 I:01
$00/F6CB 85 06       STA $06    [$00:0006]   A:0003 X:0A00 Y:0003 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1322 VC:059 FC:25 I:01
$00/F6CD BD 8A E0    LDA $E08A,x[$7E:EA8A]   A:0003 X:0A00 Y:0003 D:0000 DB:7E S:01F4 P:envmxdIzc HC:1370 VC:059 FC:25 I:01
$00/F6D0 85 0C       STA $0C    [$00:000C]   A:2EA0 X:0A00 Y:0003 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0068 VC:060 FC:25 I:01
$00/F6D2 E2 20       SEP #$20                A:2EA0 X:0A00 Y:0003 D:0000 DB:7E S:01F4 P:envmxdIzc HC:0116 VC:060 FC:25
 */