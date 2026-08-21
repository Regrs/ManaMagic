using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace ZwellTech
{
    /// <summary>
    /// Represents the valid range of a value.
    /// </summary>
    /// <remarks>https://github.com/dotnet/runtime/blob/main/src/libraries/System.Drawing.Primitives/src/System/Drawing/Point.cs</remarks>
    public struct Bounds8 : IEquatable<Bounds8>
    {
        /// <summary>
        /// Represents a <see cref="Bounds8"/> that has <see cref="Bounds8.Minimum"/> and <see cref="Bounds8.Maximum"/> values set to zero.
        /// </summary>
        public static Bounds8 Empty { get; } = new Bounds8(0, 0);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Bounds8"/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty { get { return this.Minimum == 0 && this.Maximum == 0; } }

        /// <summary>
        /// Gets or sets the minimum of this <see cref="Bounds8"/>.
        /// </summary>
        public byte Minimum { readonly get; set; }
        /// <summary>
        /// Gets or sets the maximum of this <see cref="Bounds8"/>.
        /// </summary>
        public byte Maximum { readonly get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bounds8"/> struct with the specified minimum and maximum.
        /// </summary>
        /// <param name="minimum">The minimum value of the bounds.</param>
        /// <param name="maximum">The maximum value of the bounds.</param>
        public Bounds8(byte minimum, byte maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        /// <inheritdoc />
        public bool Equals(Bounds8 other)
        {
            return this.Minimum == other.Minimum && this.Maximum == other.Maximum;
        }

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Bounds8 other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Minimum, this.Maximum);
        }

        /// <summary>
        /// Compares two <see cref='Bounds8'/> objects. The result specifies whether the values of the <see cref='Bounds8.Minimum'/> and <see cref='Bounds8.Maximum'/> properties of the two <see cref='Bounds8'/> objects are equal.
        /// </summary>
        public static bool operator ==(Bounds8 left, Bounds8 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref='Bounds8'/> objects. The result specifies whether the values of the <see cref='Bounds8.Minimum'/> or <see cref='Bounds8.Maximum'/> properties of the two <see cref='Bounds8'/>  objects are unequal.
        /// </summary>
        public static bool operator !=(Bounds8 left, Bounds8 right)
        {
            return !left.Equals(right);
        }
    }
}