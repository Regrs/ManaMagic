using System.Diagnostics.CodeAnalysis;

namespace ManaMagic
{
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types")]
    public readonly struct Void
    {
        public static readonly Void Value = default(Void);
    }
}//typeof(void)
