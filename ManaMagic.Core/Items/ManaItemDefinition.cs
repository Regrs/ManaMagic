using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Items
{
    public sealed record ManaItemDefinition : NotifyRecordPropertyChanged
    {
        private byte amountHealed = 0;
        private byte paletteIndex = 0;

        public byte Unused00 { get; }
        public byte Unused01 { get; }

        public byte AmountHealed
        {
            get { return this.amountHealed; }
            set { this.SetProperty(ref this.amountHealed, value); }
        }

        public byte Unused03 { get; }
        public byte Unused04 { get; }
        public byte Unused05 { get; }
        public byte Unused06 { get; }

        public byte PaletteIndex
        {
            get { return this.paletteIndex; }
            set { this.SetProperty(ref this.paletteIndex, value); }
        }

        public ushort ProbablyItemGraphicsPointer { get; }
        public ushort ProbablyPlayerGraphicsPointer { get; }
        public ushort AnimationPointer { get; }
        public byte Unused0E { get; }
        public byte Unused0F { get; }

        public ManaItemDefinition(byte index,
                                  byte unused00,
                                  byte unused01,
                                  byte amountHealed,
                                  byte unused03,
                                  byte unused04,
                                  byte unused05,
                                  byte unused06,
                                  byte paletteIndex,
                                  ushort probablyItemGraphicsPointer,
                                  ushort probablyPlayerGraphicsPointer,
                                  ushort animationPointer,
                                  byte unused0E,
                                  byte unused0F,
                                  bool userModified = false) : base(index, userModified)
        {
            // The item definition table is weird. Each entry is 16 bytes long, but only 8 of them are used. The rest are never even loaded.
            // The 4th byte is set to 64 (100) for every entry. I wonder if items had a hit chance at some point?
            // The 5th byte is set to 04 for every entry except for Candy, which is set to 00.
            // All other bytes are set to 00.
            // The amount healed byte only affects Candy, Chocolate and Faerie Walnut.
            // Cup Of Wishes is set to restore 250 HP, but this is ignored and the code restores full HP.
            // Not a lot worthwhile to edit here, Items are mainly controlled by code.
            // Probably should condense some of these bytes into ushorts just for space reasons.
            this.Unused00 = unused00;
            this.Unused01 = unused01;
            this.amountHealed = amountHealed;
            this.Unused03 = unused03;
            this.Unused04 = unused04;
            this.Unused05 = unused05;
            this.Unused06 = unused06;
            this.paletteIndex = paletteIndex;
            this.ProbablyItemGraphicsPointer = probablyItemGraphicsPointer;
            this.ProbablyPlayerGraphicsPointer = probablyPlayerGraphicsPointer;
            this.AnimationPointer = animationPointer;
            this.Unused0E = unused0E;
            this.Unused0F = unused0F;
        }
    }
}
/*
Byte 00: Unused
Byte 01: Unused
Byte 02: Amount Healed
Byte 03: Unused
Byte 04: Unused
Byte 05: Unused
Byte 06: Unused
Byte 07: Read By C2/B49C - Stored into $E04F on the target (Seems to be palette ID)
Byte 0809: Read By C1/86C6 - Item Graphics Pointer?
Byte 0A0B: Read By C2/B49C - Player Graphics Pointer? - Stored in $E06A.
Byte 0C0C: Read By C2/B49C - Animation Pointer - Stored in $E0FC.
Byte 0E: Unused
Byte 0F: Unused

EZIndex:	00 01 02 03 04 05 06 07 0809 0A0B 0C0D 0E 0F
D0/4150:	00 00 64 64 00 00 00 82 110C A669 EF8C 00 00 [00: Candy] {Same A669, EF8C as Cure Water {Low}}
*/