using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.TextData
{
    public sealed class AsciiTextTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string strValue)
            {
                strValue = strValue.Replace("\\n", "\n").ToUpper();
                byte[] strEncoding = Encoding.ASCII.GetBytes(strValue);
                for (int i = 0; i < strEncoding.Length; i++)
                {
                    byte b = strEncoding[i];
                    if (b == 0x0A) { strEncoding[i] = 0x7F; }
                }
                return (IReadOnlyList<byte>)new List<byte>(strEncoding);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is IReadOnlyList<byte> textData)
            {
                byte[] convertedBytes = new byte[textData.Count];
                for (int i = 0; i < textData.Count; i++)
                {
                    byte b = textData[i];
                    convertedBytes[i] = (byte)(b != 0x7F ? b : 0x0A);
                }
                return Encoding.ASCII.GetString(convertedBytes).Replace("\n", "\\n");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}