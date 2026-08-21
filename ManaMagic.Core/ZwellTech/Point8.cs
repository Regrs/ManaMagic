using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace ZwellTech
{
    /// <summary>
    /// Represents an ordered pair of byte-sized x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    /// <remarks>https://github.com/dotnet/runtime/blob/main/src/libraries/System.Drawing.Primitives/src/System/Drawing/Point.cs</remarks>
    public struct Point8 : IEquatable<Point8>
    {
        /// <summary>
        /// Represents a <see cref="Point8"/> that has <see cref="Point8.X"/> and <see cref="Point8.Y"/> values set to zero.
        /// </summary>
        public static Point8 Empty { get; } = new Point8(0, 0);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Point8"/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty { get { return this.X == 0 && this.Y == 0; } }

        /// <summary>
        /// Gets or sets the x-coordinate of this <see cref="Point8"/>.
        /// </summary>
        public byte X { readonly get; set; }
        /// <summary>
        /// Gets or sets the y-coordinate of this <see cref="Point8"/>.
        /// </summary>
        public byte Y { readonly get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point8"/> struct with the specified coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public Point8(byte x, byte y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <inheritdoc />
        public bool Equals(Point8 other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        /// <inheritdoc />
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Point8 other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }

        /// <summary>
        /// Compares two <see cref='Point8'/> objects. The result specifies whether the values of the <see cref='Point8.X'/> and <see cref='Point8.Y'/> properties of the two <see cref='Point8'/> objects are equal.
        /// </summary>
        public static bool operator ==(Point8 left, Point8 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref='Point8'/> objects. The result specifies whether the values of the <see cref='Point8.X'/> or <see cref='Point8.Y'/> properties of the two <see cref='Point8'/>  objects are unequal.
        /// </summary>
        public static bool operator !=(Point8 left, Point8 right)
        {
            return !left.Equals(right);
        }
    }

}