#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    public readonly struct FiniteImpulseResponseFilter
    {
        public static FiniteImpulseResponseFilter Identity { get; } = new FiniteImpulseResponseFilter(0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);

        public sbyte Tap0 { get; init; }
        public sbyte Tap1 { get; init; }
        public sbyte Tap2 { get; init; }
        public sbyte Tap3 { get; init; }
        public sbyte Tap4 { get; init; }
        public sbyte Tap5 { get; init; }
        public sbyte Tap6 { get; init; }
        public sbyte Tap7 { get; init; }

        public FiniteImpulseResponseFilter(sbyte tap0, sbyte tap1, sbyte tap2, sbyte tap3, sbyte tap4, sbyte tap5, sbyte tap6, sbyte tap7)
        {
            this.Tap0 = tap0;
            this.Tap1 = tap1;
            this.Tap2 = tap2;
            this.Tap3 = tap3;
            this.Tap4 = tap4;
            this.Tap5 = tap5;
            this.Tap6 = tap6;
            this.Tap7 = tap7;
        }
    }
}