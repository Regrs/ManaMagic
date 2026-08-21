using System;
using System.Collections.Generic;
using ManaMagic.Core.Metadata;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses
{
    public sealed class BossTileset : Tileset
    {
        /// <summary>
        /// Gets a <see cref="BossTileset"/> that is empty.
        /// </summary>
        public static new BossTileset Empty { get; } = new BossTileset(0, Array.Empty<GraphicTile>(), BossGraphicsTableEntry.Empty, BossTilesetMetadata.Empty, TileType.Unknown);

        public override string Name { get { return this.Metadata.Name; } }

        public BossGraphicsTableEntry Row { get; }
        public BossTilesetMetadata Metadata { get; }

        public BossTileset(int index, IReadOnlyList<GraphicTile> tiles, BossGraphicsTableEntry row, BossTilesetMetadata metadata, TileType tileType) : base(index, tiles, tileType)
        {
            this.Row = row;
            this.Metadata = metadata;
        }

        /// <inheritdoc/>
        public override object Clone()
        {
            return new BossTileset(this.Index, this.Tiles, this.Row, this.Metadata, this.TileType);
        }
    }
}