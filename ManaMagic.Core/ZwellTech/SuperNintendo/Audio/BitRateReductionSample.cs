using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio
{
    public sealed class BitRateReductionSample : IEnumerable<BitRateReductionBlock>
    {
        public static BitRateReductionSample Empty { get; } = new BitRateReductionSample(9, new List<BitRateReductionBlock>() { BitRateReductionBlock.Empty });

        public ushort LoopbackOffset { get; }
        public int LoopbackIndex { get { return this.LoopbackOffset / 9; } }

        public IReadOnlyList<BitRateReductionBlock> Blocks { get; }

        public int Count { get { return this.Blocks.Count; } }

        public BitRateReductionBlock this[int index] { get { return this.Blocks[index]; } }

        public BitRateReductionSample(ushort loopbackOffset, IReadOnlyList<BitRateReductionBlock> blocks)
        {
            this.LoopbackOffset = loopbackOffset;
            this.Blocks = blocks;
        }

        /// <inheritdoc />
        public IEnumerator<BitRateReductionBlock> GetEnumerator()
        {
            return this.Blocks.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public short[] DecodeSample(int loopIndex, int loopCount)
        {
            int additional = (loopCount * (this.Blocks.Count - loopIndex)) * 16;
            short[] sampleArray = new short[16 * this.Blocks.Count + additional];
            int index = 0;
            foreach (BitRateReductionBlock block in this)
            {
                DecodeSample(block.Sample01, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample02, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample03, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample04, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample05, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample06, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample07, block.Range, block.Filter, ref index, sampleArray);
                DecodeSample(block.Sample08, block.Range, block.Filter, ref index, sampleArray);
            }

            for (int lIndex = 0; lIndex < loopCount; lIndex++)
            {
                for (int j = loopIndex; j < this.Blocks.Count; j++)
                {
                    BitRateReductionBlock block = this[j];
                    DecodeSample(block.Sample01, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample02, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample03, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample04, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample05, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample06, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample07, block.Range, block.Filter, ref index, sampleArray);
                    DecodeSample(block.Sample08, block.Range, block.Filter, ref index, sampleArray);
                }
            }

            return sampleArray;

            static void DecodeSample(byte sample, byte range, byte filter, ref int index, short[] sampleArray)
            {
                short value = (short)((sample >> 4) & 0x0F);
                if (value >= 8) { value -= 16; }
                value <<= range;
                sampleArray[index] = value;

                if (filter == 1) { sampleArray[index] += (short)((double)sampleArray[index - 1] * (15 / 16)); }
                else if (filter == 2) { sampleArray[index] += (short)(((double)sampleArray[index - 1] * (61 / 32)) - ((double)sampleArray[index - 2] * (15 / 16))); }
                else if (filter == 3) { sampleArray[index] += (short)(((double)sampleArray[index - 1] * (115 / 64)) - ((double)sampleArray[index - 2] * (13 / 16))); }

                index++;

                value = (short)(sample & 0x0F);
                if (value >= 8) { value -= 16; }
                value <<= range;
                sampleArray[index] = value;

                if (filter == 1) { sampleArray[index] += (short)((double)sampleArray[index - 1] * (15 / 16)); }
                else if (filter == 2) { sampleArray[index] += (short)(((double)sampleArray[index - 1] * (61 / 32)) - ((double)sampleArray[index - 2] * (15 / 16))); }
                else if (filter == 3) { sampleArray[index] += (short)(((double)sampleArray[index - 1] * (115 / 64)) - ((double)sampleArray[index - 2] * (13 / 16))); }

                index++;
            }
        }
    }
}