using System;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Represents an RGB555 (red, green, blue) color.
    /// </summary>
    public readonly struct Rgb555Color : IEquatable<Rgb555Color>
    {
        /// <summary>
        /// The mask of valid bits of a RGB555 value.
        /// </summary>
        public const ushort ColorMask = 0B_0111_1111_1111_1111;
        /// <summary>
        /// The mask of valid bits of the red component of the RGB555 value.
        /// </summary>
        public const ushort RedColorMask = 0B_0111_1100_0000_0000;
        /// <summary>
        /// The mask of valid bits of the green component of the RGB555 value.
        /// </summary>
        public const ushort GreenColorMask = 0B_0000_0011_1110_0000;
        /// <summary>
        /// The mask of valid bits of the blue component of the RGB555 value.
        /// </summary>
        public const ushort BlueColorMask = 0B_0000_0000_0001_1111;

        /// <summary>
        /// Gets a system-defined color that has an RGB555 value of #7C00.
        /// </summary>
        public static Rgb555Color Red { get; } = new Rgb555Color(KnownRgb555Color.Red);
        /// <summary>
        /// Gets a system-defined color that has an RGB555 value of #03E0.
        /// </summary>
        public static Rgb555Color Green { get; } = new Rgb555Color(KnownRgb555Color.Green);
        /// <summary>
        /// Gets a system-defined color that has an RGB555 value of #001F.
        /// </summary>
        public static Rgb555Color Blue { get; } = new Rgb555Color(KnownRgb555Color.Blue);
        /// <summary>
        /// Gets a system-defined color that has an RGB555 value of #7FFF.
        /// </summary>
        public static Rgb555Color White { get; } = new Rgb555Color(KnownRgb555Color.White);
        /// <summary>
        /// Gets a system-defined color that has an RGB555 value of #0000.
        /// </summary>
        public static Rgb555Color Black { get; } = new Rgb555Color(KnownRgb555Color.Black);
        /// <summary>
        /// Gets a system-defined color that has an RGB555 value of #0010.
        /// </summary>
        public static Rgb555Color Navy { get; } = new Rgb555Color(KnownRgb555Color.Navy);

        private readonly ushort value;

        /// <summary>
        /// Gets the red component value of this <see cref="Rgb555Color"/> structure.
        /// </summary>
        public byte R { get { return (byte)((this.value & Rgb555Color.RedColorMask) >> 10); } }
        /// <summary>
        /// Gets the green component value of this <see cref="Rgb555Color"/> structure.
        /// </summary>
        public byte G { get { return (byte)((this.value & Rgb555Color.GreenColorMask) >> 5); } }
        /// <summary>
        /// Gets the blue component value of this <see cref="Rgb555Color"/> structure.
        /// </summary>
        public byte B { get { return (byte)(this.value & Rgb555Color.BlueColorMask); } }

        private Rgb555Color(ushort value)
        {
            this.value = (ushort)(Rgb555Color.ColorMask & value);
        }

        private Rgb555Color(KnownRgb555Color color) : this(KnownRgb555Colors.GetKnownColor(color)) { }

        /// <summary>
        /// Gets the 15-bit RGB555 value of this <see cref="Rgb555Color"/> structure.
        /// </summary>
        /// <returns>The 15-bit RGB555 value of this <see cref="Rgb555Color"/>.</returns>
        public ushort ToRgb555()
        {
            return this.ToRgb555(Rgb555Format.RGB);
        }

        /// <summary>
        /// Gets the 15-bit RGB555 value of this <see cref="Rgb555Color"/> structure in the specified format.
        /// </summary>
        /// <param name="format">A <see cref="Rgb555Format"/> value that indicates the bitmask of the RGB555 value.</param>
        /// <returns>The 15-bit RGB555 value of this <see cref="Rgb555Color"/> in the specified format.</returns>
        public ushort ToRgb555(Rgb555Format format)
        {
            if (format == Rgb555Format.BGR)
            {
                // Swap the location of the R and B values.
                return (ushort)(((this.B << 10) & Rgb555Color.RedColorMask) | ((this.G << 5) & Rgb555Color.GreenColorMask) | ((this.R << 0) & Rgb555Color.BlueColorMask));
            }
            return this.value;
        }

        /// <summary>
        /// Gets the 15-bit RGB555 value of this <see cref="Rgb555Color"/> structure converted to a 32-bit ARGB value.
        /// </summary>
        /// <returns>The 32-bit ARGB value of this <see cref="Rgb555Color"/>.</returns>
        public int ToArgb()
        {
            int argb = ((byte.MaxValue << 24) | (this.R * 8) << 16) | ((this.G * 8) << 8) | (this.B * 8);
            return argb;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Rgb555Color"/> structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="Rgb555Color"/>.</returns>
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="Rgb555Color"/> structure and is equivalent to this <see cref="Rgb555Color"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>true if obj is a <see cref="Rgb555Color"/> structure equivalent to this <see cref="Rgb555Color"/> structure; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Rgb555Color other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <summary>
        /// Tests whether the specified <see cref="Rgb555Color"/> structure is equivalent to this <see cref="Rgb555Color"/> structure.
        /// </summary>
        /// <param name="other">The <see cref="Rgb555Color"/> structure to test.</param>
        /// <returns>true if the specified <see cref="Rgb555Color"/> structure equivalent to this <see cref="Rgb555Color"/> structure; otherwise, false.</returns>
        public bool Equals(Rgb555Color other)
        {
            return this == other;
        }

        /// <summary>
        /// Creates a <see cref="Rgb555Color"/> structure from a 15-bit RGB555 value.
        /// </summary>
        /// <param name="rgb"> A value specifying the 15-bit RGB555 value.</param>
        /// <returns>A <see cref="Rgb555Color"/> structure containing the specified value.</returns>
        public static Rgb555Color FromRgb(ushort rgb)
        {
            return Rgb555Color.FromRgb(rgb, Rgb555Format.RGB);
        }

        /// <summary>
        /// Creates a <see cref="Rgb555Color"/> structure from a 15-bit RGB555 value that is in the specified format.
        /// </summary>
        /// <param name="rgb"> A value specifying the 15-bit RGB555 value.</param>
        /// <param name="format">A <see cref="Rgb555Format"/> value that indicates the bitmask of the RGB555 value.</param>
        /// <returns></returns>
        public static Rgb555Color FromRgb(ushort rgb, Rgb555Format format)
        {
            rgb = (ushort)(rgb & Rgb555Color.ColorMask);
            Rgb555Color result = new Rgb555Color(rgb);
            if (format == Rgb555Format.BGR)
            {
                // Re-create the struct with the R and B values swapped around.
                result = Rgb555Color.FromRgb(result.B, result.G, result.R);
            }
            return result;
        }

        /// <summary>
        /// Creates a <see cref="Rgb555Color"/> structure from the three Argb component (red, green, and blue) values.
        /// </summary>
        /// <param name="r">The red component. Valid values are 0 through 31.</param>
        /// <param name="g">The green component. Valid values are 0 through 31.</param>
        /// <param name="b">The blue component. Valid values are 0 through 31.</param>
        /// <param name="format">A <see cref="Rgb555Format"/> value that indicates the bitmask of the RGB555 value.</param>
        /// <returns>A <see cref="Rgb555Color"/> structure containing the specified red, green, and blue values.</returns>
        public static Rgb555Color FromArgb(byte r, byte g, byte b)
        {
            ushort red = ((ushort)(((r / 8) << 10) & Rgb555Color.RedColorMask));
            ushort green = ((ushort)(((g / 8) << 5) & Rgb555Color.GreenColorMask));
            ushort blue = ((ushort)(((b / 8) << 0) & Rgb555Color.BlueColorMask));

            Rgb555Color result = new Rgb555Color((ushort)(red | green | blue));
            return result;
        }

        /// <summary>
        /// Creates a <see cref="Rgb555Color"/> structure from the three RGB555 component (red, green, and blue) values.
        /// </summary>
        /// <param name="r">The red component. Valid values are 0 through 31.</param>
        /// <param name="g">The green component. Valid values are 0 through 31.</param>
        /// <param name="b">The blue component. Valid values are 0 through 31.</param>
        /// <returns>A <see cref="Rgb555Color"/> structure containing the specified red, green, and blue values.</returns>
        public static Rgb555Color FromRgb(byte r, byte g, byte b)
        {
            return new Rgb555Color((ushort)(((r << 10) & Rgb555Color.RedColorMask) | ((g << 5) & Rgb555Color.GreenColorMask) | ((b << 0) & Rgb555Color.BlueColorMask)));
        }

        /// <summary>
        /// Tests whether two specified <see cref="Rgb555Color"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="Rgb555Color"/> that is to the left of the equality operator.</param>
        /// <param name="right">The <see cref="Rgb555Color"/> that is to the right of the equality operator.</param>
        /// <returns>true if the two <see cref="Rgb555Color"/> structures are equal; otherwise, false.</returns>
        public static bool operator ==(Rgb555Color left, Rgb555Color right)
        {
            return left.value == right.value;
        }

        /// <summary>
        /// Tests whether two specified <see cref="Rgb555Color"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="Rgb555Color"/> that is to the left of the inequality operator.</param>
        /// <param name="right">The <see cref="Rgb555Color"/> that is to the right of the inequality operator.</param>
        /// <returns>true if the two <see cref="Rgb555Color"/> structures are different; otherwise, false.</returns>
        public static bool operator !=(Rgb555Color left, Rgb555Color right)
        {
            return !(left.value == right.value);
        }
    }
}