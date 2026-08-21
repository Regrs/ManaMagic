using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Drawing
{
    internal static class KnownRgb555Colors
    {
        private static readonly IReadOnlyDictionary<KnownRgb555Color, KnownRgb555ColorContext> KnownColors = new Dictionary<KnownRgb555Color, KnownRgb555ColorContext>()
        {
            { KnownRgb555Color.Red, new KnownRgb555ColorContext(0x7C00, "Red") },
            { KnownRgb555Color.Green, new KnownRgb555ColorContext(0x03E0, "Green") },
            { KnownRgb555Color.Blue, new KnownRgb555ColorContext(0x001F, "Blue") },
            { KnownRgb555Color.White, new KnownRgb555ColorContext(0x7FFF, "White") },
            { KnownRgb555Color.Black, new KnownRgb555ColorContext(0x0000, "Black") },
            { KnownRgb555Color.Navy, new KnownRgb555ColorContext(0x0010, "Navy") },
        };

        public static ushort GetKnownColor(KnownRgb555Color color)
        {
            return KnownRgb555Colors.KnownColors[color].RGB;
        }

        private readonly struct KnownRgb555ColorContext
        {
            public ushort RGB { get; }
            public string Name { get; }

            public KnownRgb555ColorContext(ushort rgb, string name)
            {
                this.RGB = rgb;
                this.Name = name;
            }
        }
    }
}