using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.TextData
{
    [EventOpCodeType(EventOpCodeType.ASCIITextStart)]
    [Description("Text displayed in the ASCII Text Crawl.")]
    public sealed record AsciiTextEventOpCode : TextDataEventOpCode
    {
        /// <inheritdoc />
        public override byte NewLine { get { return 0x7F; } }

        /// <inheritdoc />
        [TypeConverter(typeof(AsciiTextTypeConverter))]
        public override IReadOnlyList<byte> TextData
        {
            get { return base.TextData; }
            set { base.TextData = value; }
        }

        internal AsciiTextEventOpCode(IReadOnlyList<byte> textData, ParameterInfo parameterInfo) : base(EventOpCodeType.ASCIITextStart, parameterInfo, textData) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToString(true);
        }

        protected override string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        protected override string GetString(byte[] bytes, int index, int count)
        {
            return Encoding.ASCII.GetString(bytes, index, count);
        }
    }
}