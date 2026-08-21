using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.BeginASCIITextCrawl)]
    [Description("Begins the ASCII crawl sequence and places the event parser in ASCII mode.")]
    public sealed record BeginASCIITextCrawlEventOpCode : TextEventOpCode
    {
        internal BeginASCIITextCrawlEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                                ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                                : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: Begin ASCII Text Crawl.";
        }
    }
}