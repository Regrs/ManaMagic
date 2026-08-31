using System;
using System.Collections.Generic;
using ManaMagic.Core.Maps;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class MapRomWriter : RomWriter
    {
        private const int MapHeaderBankByte = 0x28;
        private const int MapHeaderOffset = MapRomWriter.MapHeaderBankByte << 16;

        private const uint MapHeaderPointerTableAddress = MapRomWriter.MapHeaderOffset | 0x0000;

        private const uint NpcLoaderPointerTablePatchLocation = 0x00C465;
        private const uint NpcLoaderDataBankPatchLocation = 0x00C46D;

        private const uint MapHeaderLoaderPointerTablePatchLocation1 = 0x00C488;
        private const uint MapHeaderLoaderPointerTablePatchLocation2 = 0x00C48E;
        private const uint MapHeaderLoaderDataBankPatchLocation = 0x00C495;

        /// <inheritdoc/>
        public MapRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteDisplaySettingsTable(MapContext context)
        {
            this.Seek(Constants.Bank08.MapDisplaySettingsTable);
            foreach (MapDisplaySettings displaySettings in context.DisplaySettingsTable)
            {
                this.Write(displaySettings.MosaicSettings);
                this.Write((byte)displaySettings.MainScreenLayersEnabled);
                this.Write((byte)displaySettings.SubScreenLayersEnabled);
                this.Write((byte)displaySettings.ColorMath);
                this.Write(displaySettings.UnusedValue01);
                this.Write(displaySettings.UnusedValue02);
                this.Write(displaySettings.UnusedValue03);
                this.WriteUInt16(displaySettings.BackgroundColor.ToRgb555(Rgb555Format.BGR));
            }
        }

        public void WriteHeaderTable(MapContext context)
        {
            // 512 maps.
            // 8 bytes per header.
            // 8 bytes per sprite object in the header.

            // 1024 bytes for the pointer table.
            // 4096 bytes for the header.
            // 5120 bytes required.
            // 60,415 remaining.
            // This gives room for 7,551 sprite objects.
            // This is 14.7 sprites per map, which would be fine if some maps didn't use up to 46.
            this.Seek(MapRomWriter.MapHeaderPointerTableAddress + (Constants.Bank08.NumberOfMaps * sizeof(ushort)));

            Span<byte> headerValues = stackalloc byte[8];
            Span<byte> spriteValues = stackalloc byte[8];
            List<ushort> pointerTable = new List<ushort>(Constants.Bank08.NumberOfMaps);
            foreach (MapHeader header in context.MapHeaderTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));

                header.GetEncodedValues(headerValues);
                this.Write(headerValues[0]); // 8x8 Tileset Index
                this.Write(headerValues[1]); // Palette Set Index
                this.Write(headerValues[2]); // 16x16 Tileset Index
                this.Write(headerValues[3]); // Event Options
                this.Write(headerValues[4]); // Special Items Options
                this.Write(headerValues[5]); // Display Settings Index
                this.Write(headerValues[6]); // Unused
                this.Write(headerValues[7]); // NPC Palette Index

                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                {
                    spriteObject.GetEncodedValues(spriteValues);
                    this.Write(spriteValues[0]); // Event Flag
                    this.Write(spriteValues[1]); // Event Flag Range
                    this.Write(spriteValues[2]); // X-Coordinate
                    this.Write(spriteValues[3]); // Y-Coordinate
                    this.Write(spriteValues[4]); // Direction
                    this.Write(spriteValues[5]); // SpriteI ndex
                    this.Write(spriteValues[6]); // Event ID Low
                    this.Write(spriteValues[7]); // Event ID High
                }
            }

            // Write out the pointer table.
            this.Seek(MapRomWriter.MapHeaderPointerTableAddress);
            foreach (ushort pointer in pointerTable)
            {
                this.WriteUInt16(pointer);
            }

            // This is the location where the NPC loader loads from the map header pointer table.
            this.Seek(MapRomWriter.NpcLoaderPointerTablePatchLocation);
            this.WriteUInt24(MapRomWriter.MapHeaderPointerTableAddress | 0xC00000);

            // The bank byte used with the pointer loaded above.
            this.Seek(MapRomWriter.NpcLoaderDataBankPatchLocation);
            this.Write(MapRomWriter.MapHeaderBankByte | 0xC0);

            // This is the location where the map header loader loads from the map header pointer table.
            this.Seek(MapRomWriter.MapHeaderLoaderPointerTablePatchLocation1);
            this.WriteUInt24(MapRomWriter.MapHeaderPointerTableAddress | 0xC00000);

            // This is the location where the map header loader loads the next header pointer to do its size math.
            this.Seek(MapRomWriter.MapHeaderLoaderPointerTablePatchLocation2);
            this.WriteUInt24((MapRomWriter.MapHeaderPointerTableAddress + sizeof(ushort)) | 0xC00000);

            // The bank byte used with the pointers loaded above.
            this.Seek(MapRomWriter.MapHeaderLoaderDataBankPatchLocation);
            this.Write(MapRomWriter.MapHeaderBankByte | 0xC0);

            // C0/C464:	BF0070C8	LDA $C87000,X		[Load NPC Location Data Pointer]
            // C0/C46C:	A9C8    	LDA #$C8

            // C0/C487:	BF0070C8	LDA $C87000,X		[Load Room Data Pointer]
            // C0/C48D:	BF0270C8	LDA $C87002,X		[Load Next Room Address]
            // C0/C494:	A9C8		LDA #$C8			[Load #$C8 into Accumulator]
        }

        public void WritePaletteSetTable(MapContext context)
        {
            //return;
            // The first 23 map palette sets aren't palettes, they are map pieces.
            // Why not just repoint the table? Who knows, either way don't write those ones.
            int offset = ((Constants.Bank0C.FirstValidMapPaletteIndex - 0) * (Constants.Bank0C.MapPalettesPerSet * Constants.Bank0C.MapColorsPerPaletteInSet)) * sizeof(ushort);
            this.Seek(Constants.Bank0C.MapPaletteSetTableAddress + offset);
            for (int i = Constants.Bank0C.FirstValidMapPaletteIndex; i < context.PaletteSetTable.RowCount; i++)
            {
                DataTable<SpritePalette> paletteTable = context.PaletteSetTable[i];
                foreach (SpritePalette palette in paletteTable)
                {
                    // Skip the first palette, this the transparency color, which the game will automatically add.
                    for (int colorIndex = 1; colorIndex < palette.NumberOfColors; colorIndex++)
                    {
                        this.WriteUInt16(palette[colorIndex].ToRgb555(Rgb555Format.BGR));
                    }
                }
            }
        }

        public void WriteFlammieFlightCoordinateTable(MapContext context)
        {
            this.Seek(Constants.Bank06.FlammieFlightCoordinateTableAddress);
            foreach (ManaPoint8 point in context.FlammieFlightCoordinateTable)
            {
                this.Write(point.X);
                this.Write(point.Y);
            }
        }
    }
}