using System;
using System.ComponentModel;
using System.Globalization;

#nullable enable

namespace ZwellTech
{
    /// <summary>
    /// Provides a type converter to convert percentage values to and from various other representations.
    /// </summary>
    public sealed class PercentageTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ||
                   sourceType == typeof(byte) ||
                   sourceType == typeof(sbyte) ||
                   sourceType == typeof(short) ||
                   sourceType == typeof(ushort) ||
                   sourceType == typeof(int) ||
                   sourceType == typeof(uint) ||
                   sourceType == typeof(long) ||
                   sourceType == typeof(ulong);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) ||
                   destinationType == typeof(byte) ||
                   destinationType == typeof(sbyte) ||
                   destinationType == typeof(short) ||
                   destinationType == typeof(ushort) ||
                   destinationType == typeof(int) ||
                   destinationType == typeof(uint) ||
                   destinationType == typeof(long) ||
                   destinationType == typeof(ulong);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Type type = value.GetType();
                TypeCode code = Type.GetTypeCode(type);
                double convertedValue = Convert.ToDouble(value) / 100.0d;
                if (value.GetType() == typeof(byte)) { return $"{convertedValue:P2}"; }
                else if (value.GetType() == typeof(sbyte)) { return $"{convertedValue:P2}"; }
                else if (value.GetType() == typeof(short)) { return $"{convertedValue:P4}"; }
                else if (value.GetType() == typeof(ushort)) { return $"{convertedValue:P4}"; }
                else if (value.GetType() == typeof(int)) { return $"{convertedValue:P8}"; }
                else if (value.GetType() == typeof(uint)) { return $"{convertedValue:P8}"; }
                else if (value.GetType() == typeof(long)) { return $"{convertedValue:P16}"; }
                else if (value.GetType() == typeof(ulong)) { return $"{convertedValue:P16}"; }
                return string.Format("0x{0:X8}", value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                ReadOnlySpan<char> stringValue = (string)value;
                if (stringValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) { stringValue = stringValue.Slice(2); }

                if (value.GetType() == typeof(byte)) { return byte.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(sbyte)) { return sbyte.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(short)) { return short.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(ushort)) { return ushort.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(int)) { return int.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(uint)) { return uint.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(long)) { return long.Parse(stringValue, NumberStyles.Number, culture) * 100; }
                else if (value.GetType() == typeof(ulong)) { return ulong.Parse(stringValue, NumberStyles.Number, culture) * 100; }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}