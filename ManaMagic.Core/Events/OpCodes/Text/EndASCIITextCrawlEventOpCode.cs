using System.ComponentModel;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Text
{
    [EventOpCodeType(EventOpCodeType.EndASCIITextCrawl)]
    [Description("Ends the ASCII crawl sequence and places the event parser in normal mode.")]
    public sealed record EndASCIITextCrawlEventOpCode : TextEventOpCode
    {
        internal EndASCIITextCrawlEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                              ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                              : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.CommandType} Command: End ASCII Text Crawl.";
        }
    }
}