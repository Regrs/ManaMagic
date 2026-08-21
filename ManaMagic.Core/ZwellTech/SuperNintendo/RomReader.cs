using System;
using System.Text;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Provides the base class for all Super Nintendo ROM readers.
    /// </summary>
    public abstract class RomReader
    {
        /// <summary>
        /// Gets or sets the position within the reader.
        /// </summary>
        public int Position { get; set; } = 0;

        /// <summary>
        /// Gets if the readers position is at or passed the end of the ROM file.
        /// </summary>
        public bool IsEndOfFile { get { return this.Position >= this.RomFile.Length; } }

        public Encoding Encoding { get { return this.RomFile.Encoding; } }

        /// <summary>
        /// Gets the underlying <see cref="RomFile"/> for the reader.
        /// </summary>
        protected RomFile RomFile { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RomReader"/> class with the specified <see cref="RomFile"/>.
        /// </summary>
        /// <param name="rom">A <see cref="RomFile"/> from which data will be read.</param>
        public RomReader(RomFile rom)
        {
            this.RomFile = rom;
        }

        /// <summary>
        /// Sets the position within the ROM file.
        /// </summary>
        /// <param name="position">The new position in the ROM file.</param>
        /// <returns>The old position in the ROM file.</returns>
        public int Seek(int position)
        {
            int oldPosition = this.Position;
            this.Position = position;

            return oldPosition;
        }

        /// <summary>
        ///  Reads an unsigned byte from the ROM file and advances the position within the stream by one.
        /// </summary>
        /// <returns>The unsigned byte that was read.</returns>
        public byte Read()
        {
            byte b = this.RomFile.ReadByteAt(this.Position);
            this.Position++;
            return b;
        }

        /// <summary>
        ///  Reads an signed byte from the ROM file and advances the position within the stream by one.
        /// </summary>
        /// <returns>The signed byte that was read.</returns>
        public sbyte ReadSByte()
        {
            return unchecked((sbyte)this.Read());
        }

        /// <summary>
        /// Reads a unsigned 16-bit integer from the ROM file and advances the position within the stream by one.
        /// </summary>
        /// <returns>The unsigned 16-bit integer that was read.</returns>
        public ushort ReadUInt16()
        {
            ushort value = this.RomFile.ReadUInt16At(this.Position);
            this.Position++;
            this.Position++;
            return value;
        }

        /// <summary>
        /// Reads a signed 16-bit integer from the ROM file and advances the position within the stream by one.
        /// </summary>
        /// <returns>The signed 16-bit integer that was read.</returns>
        public short ReadInt16()
        {
            short value = this.RomFile.ReadInt16At(this.Position);
            this.Position++;
            this.Position++;
            return value;
        }

        /// <summary>
        /// Reads a unsigned 24-bit integer from the ROM file and advances the position within the stream by one.
        /// </summary>
        /// <returns>The unsigned 24-bit integer that was read.</returns>
        public uint ReadUInt24()
        {
            uint value = this.RomFile.ReadUInt24At(this.Position);
            this.Position++;
            this.Position++;
            this.Position++;
            return value;
        }

        /// <summary>
        /// Reads a signed 24-bit integer from the ROM file and advances the position within the stream by one.
        /// </summary>
        /// <returns>The signed 24-bit integer that was read.</returns>
        public int ReadInt24()
        {
            int value = this.RomFile.ReadInt24At(this.Position);
            this.Position++;
            this.Position++;
            this.Position++;
            return value;
        }

        public int ReadBytes(Span<byte> buffer)
        {
            return this.ReadBytes(buffer, buffer.Length);
        }

        public int ReadBytes(Span<byte> buffer, int count)
        {
            int bytesRead = 0;
            for (; (bytesRead < buffer.Length && bytesRead < count); bytesRead++)
            {
                buffer[bytesRead] = this.Read();
            }
            return bytesRead;
        }

        /// <summary>
        ///  Reads an unsigned byte from the ROM file without advancing the read position.
        /// </summary>
        /// <returns>The unsigned byte that was read.</returns>
        public byte Peek()
        {
            return this.RomFile.ReadByteAt(this.Position);
        }

        /// <summary>
        ///  Reads an signed byte from the ROM file without advancing the read position.
        /// </summary>
        /// <returns>The signed byte that was read.</returns>
        public sbyte PeekSByte()
        {
            return unchecked((sbyte)this.RomFile.ReadByteAt(this.Position));
        }

        /// <summary>
        /// Reads a unsigned 16-bit integer from the ROM file without advancing the read position.
        /// </summary>
        /// <returns>The unsigned 16-bit integer that was read.</returns>
        public ushort PeekUInt16()
        {
            return this.RomFile.ReadUInt16At(this.Position);
        }

        /// <summary>
        /// Reads a signed 16-bit integer from the ROM file without advancing the read position.
        /// </summary>
        /// <returns>The signed 16-bit integer that was read.</returns>
        public ushort PeekInt16()
        {
            return this.RomFile.ReadUInt16At(this.Position);
        }

        /// <summary>
        /// Reads a unsigned 24-bit integer from the ROM file without advancing the read position.
        /// </summary>
        /// <returns>The unsigned 24-bit integer that was read.</returns>
        public uint PeekUInt24()
        {
            return this.RomFile.ReadUInt24At(this.Position);
        }

        /// <summary>
        /// Reads a signed 24-bit integer from the ROM file without advancing the read position.
        /// </summary>
        /// <returns>The signed 24-bit integer that was read.</returns>
        public int PeekInt24()
        {
            return this.RomFile.ReadInt24At(this.Position);
        }
    }
}