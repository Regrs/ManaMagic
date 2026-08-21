#nullable enable

namespace ManaMagic.Core.Bosses.Scripts
{
    public sealed record BossGraphicsTableRowDetails
    {
        public ushort Index { get; }
        public bool TwoStepDecompression { get; }
        public bool IsMode7 { get; }
        public BossGraphicsTableEntry Row { get; }

        public BossGraphicsTableRowDetails(ushort index, BossGraphicsTableEntry row, bool twoStepDecompression, bool isMode7)
        {
            this.Index = index;
            this.Row = row;
            this.TwoStepDecompression = twoStepDecompression;
            this.IsMode7 = isMode7;
        }
    }
}