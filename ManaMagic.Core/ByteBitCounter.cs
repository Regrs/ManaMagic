using System.Collections.Generic;
using System.Text;

#nullable enable

namespace ManaMagic.Core
{
    internal sealed class ByteBitCounter
    {
        private readonly string name = string.Empty;
        private readonly Dictionary<byte, byte> bitCounts = new Dictionary<byte, byte>(8)
        {
            { 0B_0000_0000, 0 },
            { 0B_0000_0001, 0 },
            { 0B_0000_0010, 0 },
            { 0B_0000_0100, 0 },
            { 0B_0000_1000, 0 },
            { 0B_0001_0000, 0 },
            { 0B_0010_0000, 0 },
            { 0B_0100_0000, 0 },
            { 0B_1000_0000, 0 },
        };

        public ByteBitCounter(string byteName)
        {
            this.name = byteName;
        }

        public void RecordBits(byte value)
        {
            if (value == 0) { bitCounts[0]++; }
            if ((value & 0B_0000_0001) > 0) { bitCounts[0B_0000_0001]++; }
            if ((value & 0B_0000_0010) > 0) { bitCounts[0B_0000_0010]++; }
            if ((value & 0B_0000_0100) > 0) { bitCounts[0B_0000_0100]++; }
            if ((value & 0B_0000_1000) > 0) { bitCounts[0B_0000_1000]++; }
            if ((value & 0B_0001_0000) > 0) { bitCounts[0B_0001_0000]++; }
            if ((value & 0B_0010_0000) > 0) { bitCounts[0B_0010_0000]++; }
            if ((value & 0B_0100_0000) > 0) { bitCounts[0B_0100_0000]++; }
            if ((value & 0B_1000_0000) > 0) { bitCounts[0B_1000_0000]++; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[").Append(this.name).AppendLine("]");
            foreach (KeyValuePair<byte, byte> kvp in this.bitCounts)
            {
                sb.AppendLine($"{kvp.Key:X2}: {kvp.Value}");
            }
            return sb.ToString();
        }
    }
}