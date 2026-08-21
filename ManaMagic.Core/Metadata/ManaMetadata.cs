using System.Collections.Generic;
using System.Linq;
using ManaMagic.Core.Bosses;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Static class containing metadata for various data within the Secret of Mana ROM file.
    /// </summary>
    public static partial class ManaMetadata
    {
        public static SpritePalette GetMode7BossPalette(int graphicsIndex, DataTable<SpritePalette> paletteTable)
        {
            IReadOnlyList<(byte, byte)> paletteData = ManaMetadata.BossMode7PaletteSlotMap[graphicsIndex];
            List<Rgb555Color> colors = new List<Rgb555Color>(256);

            Rgb555Color color = paletteTable[paletteData[0].Item1][0];
            colors.Add(color);
            for (int i = 1; i < 256; i++)
            {
                colors.Add(Rgb555Color.Green);
            }

            foreach ((byte PaletteId, byte Slot) paletteInfo in paletteData)
            {

                SpritePalette palette = paletteTable[paletteInfo.PaletteId];
                int index = (paletteInfo.Slot * 16);
                for (int i = index; i < index + 16; i++)
                {
                    colors[i] = palette.GetColor(i - index);
                }
            }

            SpritePalette mode7Palette = new SpritePalette(0, colors);

            return mode7Palette;
        }

        public static BossTilesetMetadata GetBossTilesetMetadata(BossFamily family, bool ignoreDummiedOut)
        {
            if (ignoreDummiedOut)
            {
                return ManaMetadata.BossTilesetMetadata.Where(metadata => metadata.Family == family && !metadata.IsDummiedOut).First();
            }
            return ManaMetadata.BossTilesetMetadata.Where(metadata => metadata.Family == family).First();
        }

        public static string GetSpriteFriendlyName(byte spriteIndex, bool includeIndex = true)
        {
            string name =  ManaMetadata.SpriteNameStrings[spriteIndex];
            return includeIndex ? $"({spriteIndex:X2}) {name}" : name;
        }

        public static string GetDoorFriendlyName(ushort doorIndex, bool includeIndex = true)
        {
            if (ManaMetadata.DoorNameStrings.TryGetValue(doorIndex, out string? name))
            {
                return includeIndex ? $"({doorIndex:X4}) {name}" : name;
            }
            return doorIndex.ToString("X4");
        }

        public static string GetEventFriendlyName(ushort eventIndex, bool includeIndex = true)
        {
            if (ManaMetadata.EventNameStrings.TryGetValue(eventIndex, out string? name))
            {
                return includeIndex ? $"({eventIndex:X4}) {name}" : name;
            }
            return eventIndex.ToString("X4");
        }

        public static string GetMapFriendlyName(ushort mapIndex, bool includeIndex = true)
        {
            if (ManaMetadata.MapNameStrings.TryGetValue(mapIndex, out string? name))
            {
                return includeIndex ? $"({mapIndex:X4}) {name}" : name;
            }
            return mapIndex.ToString("X4");
        }

        public static string GetMapPieceFriendlyName(ushort mapPieceIndex, bool includeIndex = true)
        {
            if (ManaMetadata.MapPieceNameStrings.TryGetValue(mapPieceIndex, out string? name))
            {
                return includeIndex ? $"({mapPieceIndex:X4}) {name}" : name;
            }
            return mapPieceIndex.ToString("X4");
        }
    }
}