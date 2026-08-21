using ManaMagic.Core.Maps;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class MapRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public MapRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteMapDisplaySettingsTable(MapContext context)
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