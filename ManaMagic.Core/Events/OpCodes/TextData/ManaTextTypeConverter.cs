using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.TextData
{
    public sealed class ManaTextTypeConverter : TypeConverter
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
                strValue = strValue.Replace("\\n", "\n");
                byte[] strEncoding = SecretOfManaEncoding.English.GetBytes(strValue);
                return (IReadOnlyList<byte>)new List<byte>(strEncoding);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is IReadOnlyList<byte> textData)
            {
                return SecretOfManaEncoding.English.GetString(textData.ToArray()).Replace("\n", "\\n");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}