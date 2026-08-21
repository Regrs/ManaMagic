using System.Collections.Generic;
using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.TextData
{
    [EventOpCodeType(EventOpCodeType.TextStart)]
    [Description("Text displayed in the dialog window.")]
    public sealed record ManaTextEventOpCode : TextDataEventOpCode
    {
        /// <inheritdoc />
        public override byte NewLine { get { return SecretOfManaEncoding.NewLine; } }

        /// <inheritdoc />
        [TypeConverter(typeof(ManaTextTypeConverter))]
        public override IReadOnlyList<byte> TextData
        {
            get { return base.TextData; }
            set { base.TextData = value; }
        }

        internal ManaTextEventOpCode(IReadOnlyList<byte> textData, ParameterInfo parameterInfo) : base(EventOpCodeType.TextStart, parameterInfo, textData) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToString(true);
        }

        protected override string GetString(byte[] bytes)
        {
            return SecretOfManaEncoding.English.GetString(bytes);
        }

        protected override string GetString(byte[] bytes, int index, int count)
        {
            return SecretOfManaEncoding.English.GetString(bytes, index, count);
        }
    }
}