using System.Collections.Generic;
using System.Text;

#nullable enable

namespace ManaMagic.Core
{
    /// <summary>
    /// Represents a character encoding for Secret Of Mana.
    /// </summary>
    /// <inheritdoc />
    public sealed class SecretOfManaEncoding : Encoding
    {
        /// <summary>
        /// Represents the smallest possible value for text encoding. This field is constant.
        /// </summary>
        public const int MinimumTextEncodingValue = 0x7F;
        /// <summary>
        /// Represents the largest possible value for text encoding. This field is constant.
        /// </summary>
        public const int MaximumTextEncodingValue = 0xD2;

        public static byte NewLine { get { return SecretOfManaEncoding.TextEncodingReverseMap['\n']; } }

        /// <summary>
        /// Gets the English language encoding for the Secret Of Mana character set.
        /// </summary>
        public static SecretOfManaEncoding English { get; } = new SecretOfManaEncoding();

        private const int CP_UTF8 = 65001;
        private static readonly IReadOnlyDictionary<byte, char> TextEncodingMap = new Dictionary<byte, char>()
        {
            // This exists because Cave-In contains an invalid character between the words "Cave" and "In".
            // In game this results in only the word "Cave" being displayed.
            // I assume its either a space or hyphen in Japanese that was not localized.
            { 0x2A, '\u24D8' }, // Unicode CIRCLED LATIN SMALL LETTER I
            { 0x80, ' ' },
            { 0x81, 'a' },
            { 0x82, 'b' },
            { 0x83, 'c' },
            { 0x84, 'd' },
            { 0x85, 'e' },
            { 0x86, 'f' },
            { 0x87, 'g' },
            { 0x88, 'h' },
            { 0x89, 'i' },
            { 0x8A, 'j' },
            { 0x8B, 'k' },
            { 0x8C, 'l' },
            { 0x8D, 'm' },
            { 0x8E, 'n' },
            { 0x8F, 'o' },
            { 0x90, 'p' },
            { 0x91, 'q' },
            { 0x92, 'r' },
            { 0x93, 's' },
            { 0x94, 't' },
            { 0x95, 'u' },
            { 0x96, 'v' },
            { 0x97, 'w' },
            { 0x98, 'x' },
            { 0x99, 'y' },
            { 0x9A, 'z' },
            { 0x9B, 'A' },
            { 0x9C, 'B' },
            { 0x9D, 'C' },
            { 0x9E, 'D' },
            { 0x9F, 'E' },
            { 0xA0, 'F' },
            { 0xA1, 'G' },
            { 0xA2, 'H' },
            { 0xA3, 'I' },
            { 0xA4, 'J' },
            { 0xA5, 'K' },
            { 0xA6, 'L' },
            { 0xA7, 'M' },
            { 0xA8, 'N' },
            { 0xA9, 'O' },
            { 0xAA, 'P' },
            { 0xAB, 'Q' },
            { 0xAC, 'R' },
            { 0xAD, 'S' },
            { 0xAE, 'T' },
            { 0xAF, 'U' },
            { 0xB0, 'V' },
            { 0xB1, 'W' },
            { 0xB2, 'X' },
            { 0xB3, 'Y' },
            { 0xB4, 'Z' },
            { 0xB5, '0' },
            { 0xB6, '1' },
            { 0xB7, '2' },
            { 0xB8, '3' },
            { 0xB9, '4' },
            { 0xBA, '5' },
            { 0xBB, '6' },
            { 0xBC, '7' },
            { 0xBD, '8' },
            { 0xBE, '9' },
            { 0xBF, '.' },
            { 0xC0, ',' },
            { 0xC1, '/' },
            { 0xC2, '\'' },
            { 0xC3, '\u201C' }, // Unicode LEFT DOUBLE QUOTATION MARK.
            { 0xC4, '\u201D' }, // Unicode RIGHT DOUBLE QUOTATION MARK.
            { 0xC5, ':' },
            { 0xC6, '-' },
            { 0xC7, '%' },
            { 0xC8, '!' },
            { 0xC9, '&' },
            { 0xCA, '?' },
            { 0xCB, '(' },
            { 0xCC, ')' },
            { 0xCD, '#' },
            { 0xCE, '\u25BC' }, // Unicode BLACK DOWN-POINTING TRIANGLE
            { 0xCF, '\u2190' }, // Unicode LEFTWARDS ARROW
            { 0xD0, '\u2192' }, // Unicode RIGHTWARDS ARROW
            { 0xD1, '\u2191' }, // Unicode UPWARDS ARROW
            { 0xD2, '\u2193' }, // Unicode DOWNWARDS ARROW
            { 0x7F, '\n' },
        };
        private static readonly IReadOnlyDictionary<char, byte> TextEncodingReverseMap = new Dictionary<char, byte>()
        {
            // See the entry in TextEncodingMap.
            {  '\u24D8', 0x2A }, // Unicode CIRCLED LATIN SMALL LETTER I
            { ' ', 0x80 },
            { 'a', 0x81 },
            { 'b', 0x82 },
            { 'c', 0x83 },
            { 'd', 0x84 },
            { 'e', 0x85 },
            { 'f', 0x86 },
            { 'g', 0x87 },
            { 'h', 0x88 },
            { 'i', 0x89 },
            { 'j', 0x8A },
            { 'k', 0x8B },
            { 'l', 0x8C },
            { 'm', 0x8D },
            { 'n', 0x8E },
            { 'o', 0x8F },
            { 'p', 0x90 },
            { 'q', 0x91 },
            { 'r', 0x92 },
            { 's', 0x93 },
            { 't', 0x94 },
            { 'u', 0x95 },
            { 'v', 0x96 },
            { 'w', 0x97 },
            { 'x', 0x98 },
            { 'y', 0x99 },
            { 'z', 0x9A },
            { 'A', 0x9B },
            { 'B', 0x9C },
            { 'C', 0x9D },
            { 'D', 0x9E },
            { 'E', 0x9F },
            { 'F', 0xA0 },
            { 'G', 0xA1 },
            { 'H', 0xA2 },
            { 'I', 0xA3 },
            { 'J', 0xA4 },
            { 'K', 0xA5 },
            { 'L', 0xA6 },
            { 'M', 0xA7 },
            { 'N', 0xA8 },
            { 'O', 0xA9 },
            { 'P', 0xAA },
            { 'Q', 0xAB },
            { 'R', 0xAC },
            { 'S', 0xAD },
            { 'T', 0xAE },
            { 'U', 0xAF },
            { 'V', 0xB0 },
            { 'W', 0xB1 },
            { 'X', 0xB2 },
            { 'Y', 0xB3 },
            { 'Z', 0xB4 },
            { '0', 0xB5 },
            { '1', 0xB6 },
            { '2', 0xB7 },
            { '3', 0xB8 },
            { '4', 0xB9 },
            { '5', 0xBA },
            { '6', 0xBB },
            { '7', 0xBC },
            { '8', 0xBD },
            { '9', 0xBE },
            { '.', 0xBF },
            { ',', 0xC0 },
            { '/', 0xC1 },
            { '\'',0xC2 },
            { '\u201C', 0xC3 }, // Unicode LEFT DOUBLE QUOTATION MARK.
            { '\u201D', 0xC4 }, // Unicode RIGHT DOUBLE QUOTATION MARK.
            { ':', 0xC5 },
            { '-', 0xC6 },
            { '%', 0xC7 },
            { '!', 0xC8 },
            { '&', 0xC9 },
            { '?', 0xCA },
            { '(', 0xCB },
            { ')', 0xCC },
            { '#',  0xCD },
            { '\u25BC', 0xCE }, // Unicode BLACK DOWN-POINTING TRIANGLE
            { '\u2190', 0xCF }, // Unicode LEFTWARDS ARROW
            { '\u2192', 0xD0 }, // Unicode RIGHTWARDS ARROW
            { '\u2191', 0xD1 }, // Unicode UPWARDS ARROW
            { '\u2193', 0xD2 }, // Unicode DOWNWARDS ARROW
            { '\n', 0x7F },
        };

        /// <inheritdoc/>
        public override string EncodingName { get { return "Secret Of Mana Encoding"; } }
        /// <inheritdoc/>
        public override bool IsMailNewsDisplay { get { return false; } }
        /// <inheritdoc/>
        public override bool IsBrowserDisplay { get { return false; } }
        /// <inheritdoc/>
        public override bool IsBrowserSave { get { return false; } }
        /// <inheritdoc/>
        public override bool IsMailNewsSave { get { return false; } }
        /// <inheritdoc/>
        public override bool IsSingleByte { get { return true; } }
        /// <inheritdoc/>
        public override string HeaderName { get { return string.Empty; } }
        /// <inheritdoc/>
        public override string WebName { get { return string.Empty; } }
        /// <inheritdoc/>
        public override int WindowsCodePage { get { return SecretOfManaEncoding.CP_UTF8; } }

        /// <inheritdoc/>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count - index;
        }

        /// <inheritdoc/>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int byteWritten = 0;
            for (int i = charIndex; i < charCount; i++)
            {
                bytes[byteIndex + byteWritten] = SecretOfManaEncoding.TextEncodingReverseMap[chars[i]];
                byteWritten++;
            }
            return byteWritten;
        }

        /// <inheritdoc/>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return count - index;
        }

        /// <inheritdoc/>
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int charsWritten = 0;
            for (int i = byteIndex; i < byteCount; i++)
            {
                chars[charIndex + charsWritten] = SecretOfManaEncoding.TextEncodingMap[bytes[i]];
                charsWritten++;
            }
            return charsWritten;
        }

        /// <inheritdoc/>
        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        /// <inheritdoc/>
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
    }
}