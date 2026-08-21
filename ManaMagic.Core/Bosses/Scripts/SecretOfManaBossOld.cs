using System.Collections.Generic;
using ZwellTech.SuperNintendo.Drawing;

namespace ManaMagic.Core.Bosses.Scripts
{
    public sealed class ZzZSecretOfManaBossOld
    {
        //public string Name { get; set; } = string.Empty;
        public BossGraphicsData GraphicsData { get; } = new BossGraphicsData();
        //public Dictionary<ushort, List<GraphicTile4Bpp>> TileSets { get; } = new Dictionary<ushort, List<GraphicTile4Bpp>>();

        //public Bitmap ToBitmap(List<GraphicTile4Bpp> tiles, SpritePalette palette)
        //{
        //    return TileMaker.Draw8x8Tileset(tiles, palette, tiles.Count + 1, 16, false);
        //}
        //public Bitmap ToBitmap(int tileSetIndex, SpritePalette palette)
        //{
        //    if (tileSetIndex < 0) { throw new ArgumentOutOfRangeException("tileSetIndex"); }
        //    if (tileSetIndex >= this.TileSets.Count) { throw new ArgumentOutOfRangeException("tileSetIndex"); }

        //    for (int i = 0;  i < this.TileSets.Count; i++)
        //    {
        //        if (i == tileSetIndex)
        //        {
        //            var kvp = this.TileSets[i];
        //        }
        //    }
        //    //List<GraphicTile4Bpp> tileSet = this.TileSets.Values[0];
        //    return TileMaker.Draw8x8Tileset(tiles, palette, tiles.Count + 1, 16, false);
        //}
    }
}
