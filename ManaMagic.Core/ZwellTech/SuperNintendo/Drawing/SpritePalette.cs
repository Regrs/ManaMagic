using System;
using System.Collections.Generic;
using System.Drawing;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Represents a color palette used by a Super Nintendo sprite tile.
    /// </summary>
    /// <remarks>
    /// https://snes.nesdev.org/wiki/Palettes
    /// https://wiki.superfamicom.org/palettes
    /// https://gbdev.io/pandocs/SGB_Color_Palettes.html
    /// </remarks>
    public sealed record SpritePalette : NotifyRecordPropertyChanged
    {
        public static SpritePalette Empty { get; } = new SpritePalette(0, Array.Empty<Rgb555Color>(), 0);

        private readonly List<Rgb555Color> colorRgb555PaletteList;

        /// <summary>
        /// Gets the address where this palette was read from the rom file.
        /// </summary>
        public int Address { get; } = 0;
        /// <summary>
        /// Gets the number of colors contained within this sprite palette.
        /// </summary>
        public int NumberOfColors { get { return this.colorRgb555PaletteList.Count; } }
        /// <summary>
        /// Gets the list of colors contained within the palette.
        /// </summary>
        public IReadOnlyList<Rgb555Color> Colors { get { return this.colorRgb555PaletteList; } }

        /// <summary>
        /// Gets the color at the specified index in the palette.
        /// </summary>
        /// <param name="index">The zero-based index of the color to get.</param>
        /// <returns>The color at the specified index in the palette.</returns>
        public Rgb555Color this[int index] { get { return this.colorRgb555PaletteList[index]; } }

        /// <summary>
        /// Creates a new instance of the <see cref="SpritePalette"/> class with the specified array of colors.
        /// </summary>
        /// <param name="colors">The list of colors used by the sprite palette.</param>
        public SpritePalette(byte index, IReadOnlyList<Rgb555Color> colors, bool userModified = false) : base(index, userModified)
        {
            if (colors == null)
            {
                ThrowHelper.ThrowArgumentNullException(nameof(colors));
            }
            this.colorRgb555PaletteList = new List<Rgb555Color>(colors);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpritePalette"/> class with the specified array of colors that was loaded from the specified address.
        /// </summary>
        /// <param name="colors">The list of colors used by the sprite palette.</param>
        /// <param name="address">The address in the ROM file the palette was loaded from.</param>
        public SpritePalette(byte index, IReadOnlyList<Rgb555Color> colors, int address, bool userModified = false) : this(index, colors, userModified)
        {
            this.Address = address;
        }

        /// <summary>
        /// Gets a single 16-bit color from the palette array.
        /// </summary>
        /// <param name="index">The index of the color to be returned.</param>
        /// <returns>A <see cref="System.Drawing.Color"/> containing the specified color.</returns>
        public Rgb555Color GetColor(int index)
        {
            return this.colorRgb555PaletteList[index];
        }

        /// <summary>
        /// Sets the color at the specified index to the specified color.
        /// </summary>
        /// <param name="index">The index of the color to set.</param>
        /// <param name="color">The <see cref="Rgb555Color"/> to set at the specified index.</param>
        public void SetColor(int index, Rgb555Color color)
        {
            this.colorRgb555PaletteList[index] = color;
            this.OnPropertyChanged(nameof(this.Colors));
        }

        /// <summary>
        /// Sets the color at the specified index to the specified color.
        /// </summary>
        /// <param name="index">The index of the color to set.</param>
        /// <param name="color">The <see cref="Color"/> to set at the specified index.</param>
        public void SetColor(int index, Color color)
        {
            this.SetColor(index, Rgb555Color.FromArgb(color.R, color.G, color.B));
        }

        /// <summary>
        /// Returns a copy of the list of colors in this palette in a new list.
        /// </summary>
        /// <returns>A list containing a copy of the colors in this palette.</returns>
        public IList<Rgb555Color> ToList()
        {
            return new List<Rgb555Color>(this.colorRgb555PaletteList);
        }
    }
}