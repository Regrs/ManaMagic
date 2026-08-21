#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Provides a generic Super Nintendo ROM reader.
    /// </summary>
    public sealed class GenericRomReader : RomReader
    {
        /// <inheritdoc/>
        public GenericRomReader(RomFile rom) : base(rom) { }
    }
}