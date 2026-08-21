using System;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Represents a Super Nintendo ROM file.
    /// </summary>
    public class RomFile
    {
        private const int SmcHeaderSize = 0x200;
        private const int LoRomHeaderOffset = 0x7FC0;
        private const int HiRomHeaderOffset = 0xFFC0;
        protected readonly byte[] buffer;

        /// <summary>
        /// Gets the character <see cref="Encoding"/> used by text in the ROM file.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Gets the file path of the ROM file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets the length (in bytes) of the ROM file.
        /// </summary>
        public int Length { get { return this.buffer.Length; } }

        /// <summary>
        /// Gets a value indicating if the ROM file has a 512 byte SMC header.
        /// </summary>
        public bool HasSmcHeader { get; }

        protected int SmcOffset { get { return this.HasSmcHeader ? RomFile.SmcHeaderSize : 0x00; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="RomFile"/> class with the ROM file at the specified file path with the specified character encoding.
        /// </summary>
        /// <param name="filePath">The path of the ROM file.</param>
        /// <param name="encoding">The character encoding used by the ROMs text.</param>
        public RomFile(string filePath, Encoding encoding)
        {
            if (File.Exists(filePath))
            {
                this.Encoding = encoding;
                this.buffer = File.ReadAllBytes(filePath);
                this.FilePath = filePath;
                //this.HasSmcHeader = this.buffer.Length % 32768 > 0;
                this.HasSmcHeader = this.buffer.Length % 1024 > 0;
                return;
            }
            ThrowHelper.ThrowFileNotFoundException($"File: {filePath} does not exist.");
        }

        protected RomFile(RomFile romFile, bool extendRom)
        {
            this.Encoding = romFile.Encoding;
            this.FilePath = romFile.FilePath;
            this.buffer = extendRom ? new byte[0x400000] : new byte[romFile.Length];
            Array.Copy(romFile.buffer, this.buffer, romFile.buffer.Length);
            if (extendRom)
            {
                for (int i = romFile.Length; i <= 0x3FFFFF; i++) { this.buffer[i] = 0xFF; }
                // Update the Rom header with the new Rom size.
                this.buffer[0x00FFD7] = 0x0C;
            }
        }

        /// <summary>
        /// Reads a byte from the specified index.
        /// </summary>
        /// <param name="index">The index of the byte to be read.</param>
        /// <returns>The byte at the specified index.</returns>
        public byte ReadByteAt(int index)
        {
            return this.buffer[index + this.SmcOffset];
        }

        /// <summary>
        /// Reads a signed byte from the specified index.
        /// </summary>
        /// <param name="index">The index of the signed byte to be read.</param>
        /// <returns>The signed byte at the specified index.</returns>
        public sbyte ReadSByteAt(int index)
        {
            return unchecked((sbyte)this.buffer[index + this.SmcOffset]);
        }

        /// <summary>
        /// Reads a unsigned 16-bit integer from the specified index.
        /// </summary>
        /// <param name="index">The index of the unsigned 16-bit integer to be read.</param>
        /// <returns>The unsigned 16-bit integer at the specified index.</returns>
        public ushort ReadUInt16At(int index)
        {
            Span<byte> buffer = stackalloc byte[2];
            buffer[0] = this.ReadByteAt(index + this.SmcOffset);
            buffer[1] = this.ReadByteAt(index + this.SmcOffset + 1);

            return BitConverter.ToUInt16(buffer);
        }

        /// <summary>
        /// Reads a signed 16-bit integer from the specified index.
        /// </summary>
        /// <param name="index">The index of the signed 16-bit integer to be read.</param>
        /// <returns>The signed 16-bit integer at the specified index.</returns>
        public short ReadInt16At(int index)
        {
            Span<byte> buffer = stackalloc byte[2];
            buffer[0] = this.ReadByteAt(index + this.SmcOffset);
            buffer[1] = this.ReadByteAt(index + this.SmcOffset + 1);

            return BitConverter.ToInt16(buffer);
        }

        /// <summary>
        /// Reads a unsigned 16-bit integer in big endian format from the specified index.
        /// </summary>
        /// <param name="index">The index of the unsigned 16-bit integer to be read.</param>
        /// <returns>The unsigned big endian 16-bit integer at the specified index.</returns>
        public ushort ReadBigEndianUInt16At(int index)
        {
            Span<byte> buffer = stackalloc byte[2];
            buffer[0] = this.ReadByteAt(index + this.SmcOffset + 1);
            buffer[1] = this.ReadByteAt(index + this.SmcOffset);

            return BitConverter.ToUInt16(buffer);
        }

        /// <summary>
        /// Reads a signed 16-bit integer in big endian format from the specified index.
        /// </summary>
        /// <param name="index">The index of the signed 16-bit integer to be read.</param>
        /// <returns>The signed big endian 16-bit integer at the specified index.</returns>
        public short ReadBigEndianInt16At(int index)
        {
            Span<byte> buffer = stackalloc byte[2];
            buffer[0] = this.ReadByteAt(index + this.SmcOffset + 1);
            buffer[1] = this.ReadByteAt(index + this.SmcOffset);

            return BitConverter.ToInt16(buffer);
        }

        /// <summary>
        /// Reads a unsigned 24-bit integer from the specified index.
        /// </summary>
        /// <param name="index">The index of the unsigned 24-bit integer to be read.</param>
        /// <returns>The unsigned 24-bit integer at the specified index.</returns>
        public uint ReadUInt24At(int index)
        {
            Span<byte> buffer = stackalloc byte[4];
            buffer[0] = this.ReadByteAt(index + this.SmcOffset);
            buffer[1] = this.ReadByteAt(index + this.SmcOffset + 1);
            buffer[2] = this.ReadByteAt(index + this.SmcOffset + 2);
            buffer[3] = 0;

            return BitConverter.ToUInt32(buffer);
        }

        /// <summary>
        /// Reads a signed 24-bit integer from the specified index.
        /// </summary>
        /// <param name="index">The index of the signed 24-bit integer to be read.</param>
        /// <returns>The signed 24-bit integer at the specified index.</returns>
        public int ReadInt24At(int index)
        {
            Span<byte> buffer = stackalloc byte[4];
            buffer[0] = this.ReadByteAt(index + this.SmcOffset);
            buffer[1] = this.ReadByteAt(index + this.SmcOffset + 1);
            buffer[2] = this.ReadByteAt(index + this.SmcOffset + 2);
            buffer[3] = 0;

            return BitConverter.ToInt32(buffer);
        }
        
        private void ReadSnesHeader()
        {
            int lowRomScore = 0;
            for (int i = 0; i < 21; i++)
            {
                byte b = this.ReadByteAt(RomFile.LoRomHeaderOffset + i);
                if (b < 128) { lowRomScore++; }
            }
            byte romLayout = this.ReadByteAt(RomFile.LoRomHeaderOffset + 21);
            byte cartType = this.ReadByteAt(RomFile.LoRomHeaderOffset + 22);
            byte romSize = this.ReadByteAt(RomFile.LoRomHeaderOffset + 23);
            byte sRamSize = this.ReadByteAt(RomFile.LoRomHeaderOffset + 24);
            byte countryCode = this.ReadByteAt(RomFile.LoRomHeaderOffset + 25);
            byte licenseCode = this.ReadByteAt(RomFile.LoRomHeaderOffset + 26);
            byte verison = this.ReadByteAt(RomFile.LoRomHeaderOffset + 27);
            ushort checksumComp = this.ReadUInt16At(RomFile.LoRomHeaderOffset + 28);
            ushort checksum = this.ReadUInt16At(RomFile.LoRomHeaderOffset + 30);
        }

    }

    public sealed class WritableRomFile : RomFile
    {
        public WritableRomFile(string filePath, Encoding encoding) : base(filePath, encoding) { }

        public WritableRomFile(RomFile romFile, bool extendRom) : base(romFile, extendRom) { }

        public void WriteByteAt(int index, byte value)
        {
            this.buffer[index + this.SmcOffset] = value;
        }

        public void WriteSByteAt(int index, sbyte value)
        {
            this.buffer[index + this.SmcOffset] = unchecked((byte)value);
        }

        public void WriteUInt16At(int index, ushort value)
        {
            byte valueH = (byte)(value >> 8);
            byte valueL = (byte)(value & 0x00FF);

            this.WriteByteAt(index, valueL);
            this.WriteByteAt(index + 1, valueH);
        }

        public void WriteUInt24At(int index, uint value)
        {
            byte valueH = (byte)(value >> 16);
            byte valueM = (byte)(value >> 8);
            byte valueL = (byte)(value & 0x00FF);

            this.WriteByteAt(index, valueL);
            this.WriteByteAt(index + 1, valueM);
            this.WriteByteAt(index + 2, valueH);
        }

        [Conditional("DEBUG")]
        public void DebugSaveToFile()
        {
            string filePath = @"D:\SOM Stuff\SecretOfManaTestROM.sfc";
            this.CorrectChecksum();
            this.SaveToFile(filePath);
        }

        public void SaveToFile(string filePath)
        {
            File.WriteAllBytes(filePath, this.buffer);
        }

        public void CorrectChecksum()
        {
            int checksum = 0x0000;
            int checksumComplement = 0x0000;

            // Reset the Checksum Complement
            this.buffer[0x00FFDC] = 0xFF;
            this.buffer[0x00FFDD] = 0xFF;

            // Reset the Checksum
            this.buffer[0x00FFDE] = 0x00;
            this.buffer[0x00FFDF] = 0x00;

            // Calculate the new checksum.
            for (int i = 0; i < this.buffer.Length; i++) { checksum += this.buffer[i]; }

            // Keep the checksum 16-Bit.
            checksum &= 0xFFFF;

            // Get the complement.
            checksumComplement = checksum ^ 0xFFFF;

            byte[] checksumBytes = BitConverter.GetBytes(checksum);
            byte[] checksumCompBytes = BitConverter.GetBytes(checksumComplement);

            // Set the new complement.
            this.buffer[0x00FFDC] = checksumCompBytes[0];
            this.buffer[0x00FFDD] = checksumCompBytes[1];

            // Set the new checksum.
            this.buffer[0x00FFDE] = checksumBytes[0];
            this.buffer[0x00FFDF] = checksumBytes[1];
        }
    }
}