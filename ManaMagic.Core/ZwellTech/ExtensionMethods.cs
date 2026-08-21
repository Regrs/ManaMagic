using System;
using System.ComponentModel;
using System.Reflection;

#nullable enable

namespace ZwellTech
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this Enum value)
        {
            FieldInfo? field = ExtensionMethods.GetFieldInfo(value);
            if (field != null && Attribute.GetCustomAttribute(field, typeof(FieldDisplayNameAttribute)) is FieldDisplayNameAttribute fieldDisplayNameAttribute)
            {
                return fieldDisplayNameAttribute.DisplayName;
            }
            return value.ToString();
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo? field = ExtensionMethods.GetFieldInfo(value);
            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }
            return string.Empty;
        }

        private static FieldInfo? GetFieldInfo(Enum value)
        {
            Type type = value.GetType();
            string? name = Enum.GetName(type, value);
            return name != null ? type.GetField(name) : null;
        }
    }

    /// <summary>
    /// Specifies the display name for a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class FieldDisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the display name for the field.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="EnumDisplayNameAttribute"/> with the specified display name.
        /// </summary>
        /// <param name="displayName"></param>
        public FieldDisplayNameAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }
    }
}