using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.TextData
{
    [EventOpCodeCommandType(EventOpCodeCommandType.TextData)]
    public abstract record TextDataEventOpCode : EventOpCode
    {
        private IReadOnlyList<byte> textData;

        /// <inheritdoc />
        public override Color Color { get { return Color.WhiteSmoke; } }

        /// <summary>
        /// Gets the byte that represents the new line character for the encoding.
        /// </summary>
        [Browsable(false)]
        public abstract byte NewLine { get; }

        /// <summary>
        /// Gets or sets the text to display.
        /// </summary>
        [DisplayName("Text")]
        [Description("The text to display.")]
        public virtual IReadOnlyList<byte> TextData
        {
            get { return this.textData; }
            set
            {
                this.textData = value;
                this.Parameters.OnPropertyChanged(nameof(this.Parameters.Parameter1));
            }
        }

        /// <inheritdoc />
        public override int Size { get { return this.textData.Count; } }

        /// <summary>
        /// Gets the number of lines used to display the text.
        /// </summary>
        [Category("Miscellaneous")]
        [Description("The number of lines used to display the text.")]
        public int LineCount { get { return this.TextData.Count(p => p == this.NewLine) + 1; } }

        internal TextDataEventOpCode(EventOpCodeType opCodeType, ParameterInfo parameterInfo, IReadOnlyList<byte> textData) : base(opCodeType, EventOpCodeCommandType.TextData, parameterInfo)
        {
            this.textData = new List<byte>(textData);
        }

        protected abstract string GetString(byte[] bytes);
        protected abstract string GetString(byte[] bytes, int index, int count);

        public string ToString(bool displayFormatting)
        {
            if (displayFormatting)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("--> ");
                byte[] textBuffer = new byte[128];
                int length = 0;
                for (int i = 0; i < this.TextData.Count; i++)
                {
                    length = 0;
                    bool newLine = false;
                    for (; length + i < this.TextData.Count; length++)
                    {
                        byte b = this.TextData[length + i];
                        textBuffer[length] = b;

                        if (b == this.NewLine)
                        {
                            newLine = true;
                            break;
                        }
                    }

                    i += length;
                    if (newLine)
                    {
                        sb.AppendLine(this.GetString(textBuffer, 0, length));
                        sb.Append("--> ");
                    }
                    else
                    {
                        sb.Append(this.GetString(textBuffer, 0, length));
                    }
                }
                return sb.ToString();
            }

            return this.GetString(this.TextData.ToArray());
        }
    }
}