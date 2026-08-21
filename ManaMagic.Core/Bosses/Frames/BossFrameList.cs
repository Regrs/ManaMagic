using System;
using System.Collections;
using System.Collections.Generic;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses.Frames
{
    public sealed class BossFrameList : IReadOnlyList<BossFrame>
    {
        public static BossFrameList Empty { get; } = new BossFrameList(BossFamily.Unknown, Array.Empty<BossFrame>());

        private readonly IReadOnlyList<BossFrame> frameList;

        /// <inheritdoc/>
        public BossFrame this[int index] { get { return this.frameList[index]; } }

        /// <inheritdoc/>
        public int Count { get { return this.frameList.Count; } }

        public BossFamily Family { get; } = BossFamily.Unknown;

        public BossFrameList(BossFamily family, IReadOnlyList<BossFrame> frames)
        {
            this.Family = family;
            this.frameList = frames;
        }

        public SuperNintendoGraphics DrawFrame(int frameIndex, Tileset tileset, List<SpritePalette> paletteTable, FrameDrawingOptions options)
        {
            return this.frameList[frameIndex].DrawFrame(tileset, paletteTable, options);
        }

        /// <inheritdoc/>
        public IEnumerator<BossFrame> GetEnumerator()
        {
            return this.frameList.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}