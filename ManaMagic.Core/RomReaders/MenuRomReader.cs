using System;
using System.Collections.Generic;
using ManaMagic.Core.Menu;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class MenuRomReader : RomReader
    {
        public MenuRomReader(RomFile rom) : base(rom) { }

        public DataTable<ushort> ReadRingIconDefinitionOffsetTable()
        {
            this.Seek((int)Constants.Bank18.RingIconDefinitionOffsetTableAddress);

            List<ushort> offsetList = new List<ushort>((int)Constants.Bank18.RingIconDefinitionOffsetTableSize);
            for (int i = 0; i <= Constants.Bank18.RingIconDefinitionOffsetTableSize; i++)
            {
                offsetList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(offsetList);
        }

        public DataTable<RingIconDefinition> ReadRingIconDefinitionTable(ushort offset, int iconCount)
        {
            // The offsets provided by the offset table are offset from the start of the offset table, so that's fun.
            this.Seek((int)Constants.Bank18.RingIconDefinitionOffsetTableAddress + offset);

            List<RingIconDefinition> definitionList = new List<RingIconDefinition>(iconCount);
            for (int i = 0; i < iconCount; i++)
            {
                byte tileID = this.Read();
                byte palettteID = this.Read();
                definitionList.Add(new RingIconDefinition(tileID, palettteID));
            }
            return new DataTable<RingIconDefinition>(definitionList);
        }

        public DataTable<SpritePalette> ReadRingIconPaletteTable()
        {
            this.Seek((int)Constants.Bank12.RingIconPaletteTableAddress);

            List<SpritePalette> palettes = new List<SpritePalette>((int)Constants.Bank12.RingIconPaletteTableSize);
            for (int paletteIndex = 0; paletteIndex <= Constants.Bank12.RingIconPaletteTableSize; paletteIndex++)
            {
                List<Rgb555Color> colors = new List<Rgb555Color>((int)Constants.Bank12.RingIconColorsPerPalette);
                colors.Add(Rgb555Color.White);
                for (int colorIndex = 0; colorIndex < Constants.Bank12.RingIconColorsPerPalette; colorIndex++)
                {
                    // 5 colors per palette.
                    colors.Add(Rgb555Color.FromRgb(this.ReadUInt16(), Rgb555Format.BGR));
                }
                palettes.Add(new SpritePalette((byte)paletteIndex, colors));
            }

            return new DataTable<SpritePalette>(palettes);
        }

        public DataTable<DataTable<GraphicTile4Bpp>> ReadRingIconGraphicsTable()
        {
            this.Seek((int)Constants.Bank12.RingIconGraphicsTableAddress);

            Span<byte> buffer = stackalloc byte[24];
            List<DataTable<GraphicTile4Bpp>> iconList = new List<DataTable<GraphicTile4Bpp>>();
            for (int tableIndex = 0; tableIndex < Constants.Bank12.RingIconGraphicsTableSize; tableIndex++)
            {
                // Icons are stored in 3BPP format and there are 4 8x8 tiles per icon.
                List<GraphicTile4Bpp> icon = new List<GraphicTile4Bpp>(4);
                for (int tileIndex = 0; tileIndex < 4; tileIndex++)
                {
                    buffer.Fill(0);
                    this.ReadBytes(buffer);

                    icon.Add(GraphicTile4Bpp.From3BppTile(buffer));
                }
                iconList.Add(new DataTable<GraphicTile4Bpp>(icon));
            }

            return new DataTable<DataTable<GraphicTile4Bpp>>(iconList);
        }
    }
}