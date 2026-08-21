using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Provides the base class for all Super Nintendo ROM writers.
    /// </summary>
    public abstract class RomWriter
    {
        /// <summary>
        /// Gets or sets the position within the writer.
        /// </summary>
        public int Position { get; set; } = 0;

        /// <summary>
        /// Gets if the writer position is at or passed the end of the ROM file.
        /// </summary>
        public bool IsEndOfFile { get { return this.Position >= this.RomFile.Length; } }

        /// <summary>
        /// Gets the underlying <see cref="RomFile"/> for the writer.
        /// </summary>
        protected WritableRomFile RomFile { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RomReader"/> class with the specified <see cref="RomFile"/>.
        /// </summary>
        /// <param name="rom">A <see cref="RomFile"/> from which data will be written.</param>
        public RomWriter(RomFile rom, bool extendRom) : this(new WritableRomFile(rom, extendRom)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RomReader"/> class with the specified <see cref="WritableRomFile"/>.
        /// </summary>
        /// <param name="rom">A <see cref="RomFile"/> from which data will be written.</param>
        public RomWriter(WritableRomFile rom)
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
        /// Sets the position within the ROM file.
        /// </summary>
        /// <param name="position">The new position in the ROM file.</param>
        /// <returns>The old position in the ROM file.</returns>
        public int Seek(uint position)
        {
            return this.Seek((int)position);
        }

        public void Write(byte value)
        {
            this.RomFile.WriteByteAt(this.Position, value);
            this.Position++;
        }

        public void Write(sbyte value)
        {
            this.RomFile.WriteSByteAt(this.Position, value);
            this.Position++;
        }

        public void WriteUInt16(ushort value)
        {
            this.RomFile.WriteUInt16At(this.Position, value);
            this.Position++;
            this.Position++;
        }

        public void WriteUInt24(uint value)
        {
            this.RomFile.WriteUInt24At(this.Position, value);
            this.Position++;
            this.Position++;
            this.Position++;
        }

        public void WriteBytes(ReadOnlySpan<byte> bytes)
        {
            foreach (byte current in bytes)
            {
                this.Write(current);
            }
        }
    }
}