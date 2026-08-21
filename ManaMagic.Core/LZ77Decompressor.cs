using System;
using System.Collections.Generic;
using ZwellTech.SuperNintendo;

namespace ManaMagic.Core
{
    public sealed class LZ77Decompressor
    {
        private readonly IReadOnlyList<byte> decompressionKey = null;

        public LZ77Decompressor(IReadOnlyList<byte> decompressionKey)
        {
            this.decompressionKey = decompressionKey;
        }

        public IReadOnlyList<byte> Decompress(RomFile rom, int decompressionAddress)
        {
            if (rom == null) { throw new ArgumentNullException(nameof(rom)); }
            int address = decompressionAddress;

            // The first ushort of the compressed data is an index into the decompression key array.
            // This key byte will be used for block copies from the decompressed data stream.
            ushort keyIndex = rom.ReadUInt16At(address);
            byte key = this.decompressionKey[keyIndex];
            address += 2;

            // The next ushort is the size of the data stream once decompression is complete.
            // Just to make things extra complicated, the size value is stored in Big Endian.
            ushort maxSize = rom.ReadBigEndianUInt16At(address);
            List<byte> dataStream = new List<byte>(maxSize);
            address += 2;

            // Decompression Loop.
            while (dataStream.Count < maxSize)
            {
                // Read the current working byte and increment the read address.
                // The MSB determines the type of decompression operation, the rest of the bits are the bytes to read for that operation.
                byte current = rom.ReadByteAt(address);
                bool blockCopy = (current & 0B_1000_0000) != 0x00;
                byte bytesToRead = (byte)(current & 0B_0111_1111);
                address++;

                // If MSB is set, then we perform a block copy of some amount of already decompressed data to the end of the decompression array.
                // If MSB is not set, then read an amount of bytes equal to current and add them to the end of the decompression array.
                if (blockCopy)
                {
                    // Read the least significant byte of the source offset from the rom file.
                    byte sourceOffsetLSB = rom.ReadByteAt(address);

                    // The most significant byte of the source offset is the bits of bytesToRead that match the current decompression key.
                    byte sourceOffsetMSB = (byte)(bytesToRead & key);

                    // The source offset is always one higher then what the MSB and LSB indicate.
                    ushort sourceOffset = (ushort)(((sourceOffsetMSB << 8) | sourceOffsetLSB) + 1u);

                    // The number of bytes to read must be adjusted based off the decompression key being used.
                    // Divide bytesToRead by two a number of times equal to the index of the current decompression key and then add two to reach the final read value.
                    bytesToRead = (byte)((bytesToRead >> this.decompressionKey.Count - 1 - keyIndex) + 2);

                    // The source index is based off the current end index of the decompressed data stream.
                    // bytesToRead bytes will be read starting from this location and appended to the end of the decompressed data stream.
                    int sourceIndex = dataStream.Count - sourceOffset;
                    for (int i = 0; i <= bytesToRead; i++) { dataStream.Add(dataStream[sourceIndex + i]); }

                    // Move to the next byte in the compressed stream.
                    address++;
                }
                else
                {
                    // Copy bytes from the rom file to the end of the decompression array until {i == current}.
                    for (int i = 0; i <= current; i++)
                    {
                        dataStream.Add(rom.ReadByteAt(address));
                        address++;
                    }
                }
            }

            return dataStream;
        }

        public static List<byte> Decompress(IReadOnlyList<byte> slidingWindow, ushort expectedSize)
        {
            // For SoMs implementations of this method.
            // See C2/24F1 (LoadBossGraphics_BusToBus). (Normal)
            // See C2/25D7 (LoadBossGraphics_ToVRAM).   (Mode 7)

            // Take the size value from the Boss Graphics Table, Add 7 and then divide by 8 to get the true size of the graphics being decompressed.
            // Not sure why we're doing hardcoded math on hardcoded values instead of just storing the result, but who needs those CPU cycles anyway.
            ushort size = (ushort)((expectedSize + 7) >> 3);
            List<byte> dataStream = new List<byte>(size);

            int windowIndex = 0x00;
            byte byteToWrite = 0x00;
            byte lookBackByte = 0x00;
            for (int i = 0; i < size; i++)
            {
                // Great Viper and Dragon Worm report the size of their graphics incorrectly and are smaller then the table says.
                // SoM manages to handle this because the area the sliding window is loaded into was pre-filled with zeroes, so the loader will repeatedly
                // write the last two bytes of the graphic data, which also happen to be zeroes, into RAM until the size is met.
                // Expected: 768, Actual: 640
                byte controlByte = (byte)((windowIndex < slidingWindow.Count) ? slidingWindow[windowIndex] : 0x00);
                windowIndex++;

                // The control byte is a set of flags that indicates how to decompress the next block of eight bytes.
                for (int j = 0; j < 8; j++)
                {
                    // Set:     Write the byte at windowIndex into the data stream and advance windowIndex.
                    // Not Set: Write the loopback byte into the data stream.
                    bool writeNextByte = (controlByte & 0B_1000_0000) != 0;

                    // Store the last byte we wrote to the data stream on the stack.
                    byte oldByteToWrite = byteToWrite;

                    // Store the next byte to be written based off the control flag.
                    byteToWrite = writeNextByte ? slidingWindow[windowIndex] : lookBackByte;

                    // Store the last byte written as the new look back byte.
                    lookBackByte = oldByteToWrite;

                    // Add the next byte to the data stream.
                    dataStream.Add(byteToWrite);

                    // Move the control byte to the next bit in the sequence.
                    if (writeNextByte) { windowIndex++; }
                    controlByte <<= 1;
                }
            }

            dataStream.TrimExcess();
            return dataStream;
        }
    }
}