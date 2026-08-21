#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Provides a generic Super Nintendo ROM writer.
    /// </summary>
    public sealed class GenericRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public GenericRomWriter(WritableRomFile rom) : base(rom) { }
    }
}