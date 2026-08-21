#nullable enable

namespace ManaMagic.Core.Bosses
{
    /// <summary>
    /// Represents a row in Secret of Mana's boss graphics table.
    /// </summary>
    public sealed record BossGraphicsTableEntry
    {
        /// <summary>
        /// Gets a <see cref="BossGraphicsTableEntry"/> that is empty.
        /// </summary>
        public static BossGraphicsTableEntry Empty { get; } = new BossGraphicsTableEntry(0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Gets the index of the table row.
        /// </summary>
        public byte Index { get; }
        /// <summary>
        /// Gets the ROM bank the graphics are stored in.
        /// </summary>
        public byte GraphicsBank { get; }
        /// <summary>
        /// Gets the RAM bank the graphics will be loaded into.
        /// </summary>
        public byte RamBank { get; }
        /// <summary>
        /// Gets the 16-bit address the graphics are stored at.
        /// </summary>
        public ushort GraphicsAddress { get; }
        /// <summary>
        /// Gets the 16-bit address the graphics will be loaded into.
        /// </summary>
        public ushort RamAddress { get; }
        /// <summary>
        /// Gets the size of the graphics data in the ROM.
        /// </summary>
        public ushort GraphicsSize { get; }

        /// <summary>
        /// Gets the full address the graphics are stored at.
        /// </summary>
        public int FullGraphicsAddress { get { return (this.GraphicsBank & 0x1F) << 16 | this.GraphicsAddress; } }

        /// <summary>
        /// Gets the full address the graphics will be loaded into.
        /// </summary>
        public int FullRamAddress { get { return (this.RamBank & 0x1F) << 16 | this.RamAddress; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BossGraphicsTableEntry"/> class.
        /// </summary>
        /// <param name="index">The index of the table row.</param>
        /// <param name="graphicsBank">The ROM bank the graphics are stored in.</param>
        /// <param name="ramBank">The RAM bank the graphics will be loaded into.</param>
        /// <param name="graphicsAddress">The 16-bit address the graphics are stored at.</param>
        /// <param name="ramAddress">The 16-bit address the graphics will be loaded into.</param>
        /// <param name="graphicsSize">The size of the graphics data.</param>
        public BossGraphicsTableEntry(byte index, byte graphicsBank, byte ramBank, ushort graphicsAddress, ushort ramAddress, ushort graphicsSize)
        {
            this.Index = index;
            this.GraphicsBank = graphicsBank;
            this.RamBank = ramBank;
            this.GraphicsAddress = graphicsAddress;
            this.RamAddress = ramAddress;
            this.GraphicsSize = graphicsSize;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"GB: {this.GraphicsBank:X2}, RB: {this.RamBank:X2}, GAdr: {this.GraphicsAddress:X4}, RAdr: {this.RamAddress:X4}, Size: {this.GraphicsSize:X4}";
        }
    }
}