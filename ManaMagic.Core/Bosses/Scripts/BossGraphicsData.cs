using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace ManaMagic.Core.Bosses.Scripts
{
    public sealed class BossGraphicsData
    {
        public List<BossGraphicsTableRowDetails> Rows { get; } = new List<BossGraphicsTableRowDetails>();
        public int PaletteId1 { get; set; } = -1;
        public int PaletteId2 { get; set; } = -1;
        public int PaletteId3 { get; set; } = -1;
        public int PaletteId4 { get; set; } = -1;

        public override string ToString()
        {
            return $"Graphic ID(s): {string.Join<string>(",", this.Rows.Select(p => p.Index.ToString("X2")))}, PaletteId1: {this.PaletteId1:X2}, PaletteId2: {this.PaletteId2:X2}, PaletteId3: {this.PaletteId3:X2}, PaletteId4: {this.PaletteId4:X2}";
        }
    }
}