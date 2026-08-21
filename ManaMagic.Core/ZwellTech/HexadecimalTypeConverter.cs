using System;
using System.ComponentModel;
using System.Globalization;

#nullable enable

namespace ZwellTech
{
    /// <summary>
    /// Provides a type converter to convert hexadecimal values to and from various other representations.
    /// </summary>
    public sealed class HexadecimalTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) ? true : base.CanConvertFrom(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Type valueType = value.GetType();
                switch (Type.GetTypeCode(valueType))
                {
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                        return string.Format("0x{0:X2}", value);
                    case TypeCode.UInt16:
                    case TypeCode.Int16:
                        return string.Format("0x{0:X4}", value);
                    case TypeCode.UInt32:
                    case TypeCode.Int32:
                        return string.Format("0x{0:X8}", value);
                    case TypeCode.UInt64:
                    case TypeCode.Int64:
                        return string.Format("0x{0:X16}", value);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = ((string)value).Replace("0x", "").Trim();
                if (context != null)
                {
                    switch (Type.GetTypeCode(context.PropertyDescriptor.PropertyType))
                    {
                        case TypeCode.SByte:
                            return sbyte.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.Byte:
                            return byte.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.UInt16:
                            return ushort.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.Int16:
                            return short.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.UInt32:
                            return uint.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.Int32:
                            return int.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.UInt64:
                            return ulong.Parse(input, NumberStyles.HexNumber, culture);
                        case TypeCode.Int64:
                            return long.Parse(input, NumberStyles.HexNumber, culture);
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}