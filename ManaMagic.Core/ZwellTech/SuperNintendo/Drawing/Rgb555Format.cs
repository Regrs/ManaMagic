#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    /// <summary>
    /// Specifies the way a RGB555 value is formatted,
    /// </summary>
    public enum Rgb555Format
    {
        /// <summary>
        /// The RGB555 value has a bitmask of 0RRRRRGGGGGBBBBB.
        /// </summary>
        RGB,
        /// <summary>
        /// The RGB555 value has a bitmask of 0BBBBBGGGGGRRRRR.
        /// </summary>
        BGR,
    }
}