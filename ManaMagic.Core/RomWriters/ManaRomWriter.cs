using System;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Character;
using ManaMagic.Core.Events;
using ManaMagic.Core.Items;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Sprites;
using ManaMagic.Core.WorldMap;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class ManaRomWriter : RomWriter
    {
        private readonly TextRomWriter textRomWriter;
        private readonly CharacterRomWriter characterRomWriter;
        private readonly SpriteRomWriter spriteRomWriter;
        private readonly BossRomWriter bossRomWriter;
        private readonly ItemRomWriter itemRomWriter;
        private readonly MapRomWriter mapRomWriter;
        private readonly WorldMapRomWriter worldMapRomWriter;

        public bool ApplyNamingDialogPatch { get; set; } = false;

        /// <inheritdoc/>
        public ManaRomWriter(RomFile rom) : base(rom, true)
        {
            this.textRomWriter = new TextRomWriter(this.RomFile);
            this.characterRomWriter = new CharacterRomWriter(this.RomFile);
            this.spriteRomWriter = new SpriteRomWriter(this.RomFile);
            this.bossRomWriter = new BossRomWriter(this.RomFile);
            this.itemRomWriter = new ItemRomWriter(this.RomFile);
            this.mapRomWriter = new MapRomWriter(this.RomFile);
            this.worldMapRomWriter = new WorldMapRomWriter(this.RomFile);
        }

        public void WriteRomFile(SecretOfManaContext context)
        {
            this.WriteText(context.TextContext, context.EventContext);
            this.WriteCharacterData(context.CharacterContext);
            this.WriteSpriteData(context.SpriteContext);
            this.WriteBossData(context.BossContext);
            this.WriteItemData(context.ItemContext);
            this.WriteMapData(context.MapContext);
            this.WriteWorldMapData(context.WorldMapContext);

            if (this.ApplyNamingDialogPatch)
            {
                this.WriteNamingDialog();
            }
        }

        private void WriteNamingDialog()
        {
            NamingDialogRomWriter romWriter = new NamingDialogRomWriter(this.RomFile);
            romWriter.WriteNamingDialog();
        }

        public void SaveToFile(string filePath)
        {
            this.RomFile.CorrectChecksum();
            this.RomFile.SaveToFile(filePath);
        }

        private void WriteText(TextContext textContext, EventContext eventContext)
        {
            this.textRomWriter.WriteStringTables(textContext);
            this.textRomWriter.WriteEvents(eventContext);
        }

        private void WriteCharacterData(CharacterContext context)
        {
            this.characterRomWriter.WriteStatsByLevelTables(context);
            this.characterRomWriter.WriteExperiencePerLevelTable(context);
            this.characterRomWriter.WriteDefaultCharacterDataTable(context);
        }

        private void WriteSpriteData(SpriteContext context)
        {
            this.spriteRomWriter.WriteEnemyStatisticsTable(context);
            this.spriteRomWriter.WriteLootTable(context);
            this.spriteRomWriter.WriteSpritePaletteTable(context);
        }

        private void WriteBossData(BossContext context)
        {
            this.bossRomWriter.WritePaletteTable(context);
            this.bossRomWriter.WriteWeaponTable(context);
            this.bossRomWriter.WriteSetupHeaders(context);
        }

        private void WriteItemData(ItemContext context)
        {
            this.itemRomWriter.WriteWeaponDefinitionTable(context);
            this.itemRomWriter.WriteEquipmentDefinitionTable(context);
            this.itemRomWriter.WriteItemDefinitionTable(context);
            this.itemRomWriter.WriteItemPriceTables(context);
            this.itemRomWriter.WriteShopTable(context);
        }

        private void WriteMapData(MapContext context)
        {
            this.mapRomWriter.WriteMapDisplaySettingsTable(context);

            // Have to fix the RGB1555 issue before enabling this.
            //this.mapRomWriter.WritePaletteSetTable(context);
            this.mapRomWriter.WriteFlammieFlightCoordinateTable(context);
        }

        private void WriteWorldMapData(WorldMapContext context)
        {
            this.worldMapRomWriter.WriteWorldMapLandingLocationTable(context);
            this.worldMapRomWriter.WriteCannonTravelCoordinateTable(context);
        }
    }

    public sealed class NamingDialogRomWriter : RomWriter
    {
        private const ushort NamingDialogTextOffset = 0x4290;
        private const ushort NameTransferPatchRoutineOffset = 0x4400;

        private const int UpDownControllerRoutineAddress = Constants.Bank00Offset | 0x36A2;
        private const int UpControllerPatchLocation = Constants.Bank00Offset | 0x334D;
        private const int DownControllerPatchLocation = Constants.Bank00Offset | 0x3363;
        private const int NamingDialogHelpTextPatchLocation = Constants.Bank00Offset | 0x33BE;

        private const int NamingDialogTextAddress = Constants.Bank07Offset | NamingDialogRomWriter.NamingDialogTextOffset;
        private const int NameTransferPatchRoutineAddress = Constants.Bank07Offset | 0x4400;
        private const int NameTransferPatchLocation1 = Constants.Bank07Offset | 0x50B1;
        private const int NameTransferPatchLocation2 = Constants.Bank07Offset | 0x50B5;
        private const ushort AlphabetTextPatchValue = (NamingDialogRomWriter.NamingDialogTextAddress & 0xFFFF) + 5;
        private const int LayoutPointerTablePatchLocation = Constants.Bank07Offset | 0x781C;
        private const int LayoutAddress = Constants.Bank07Offset | 0x759B;
        private const int NullLayoutOffset = 0x74EA;

        private const string UppercaseLetters = "     A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
        private const string LowercaseLetters = "     a b c d e f g h i j k l m n o p q r s t u v w x y z    ";
        private const string HelpText1 = " SELECT A LETTER USING THE CONTROL PAD. PRESS THE \u201CATTACK\u201D";
        private const string HelpText2 = " (OR B BUTTON) TO ENTER. NAMES CAN BE UP TO 6 LETTERS LONG .";
        private const string HelpText3 = " PRESS THE START BUTTON TO CONTINUE.";

        /// <inheritdoc/>
        public NamingDialogRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteNamingDialog()
        {
            // There is a giant block of free space here.
            this.Seek(NamingDialogRomWriter.NamingDialogTextAddress);

            // Combine the alphabet and help text into one large event.
            this.WriteBytes(SecretOfManaEncoding.English.GetBytes(NamingDialogRomWriter.UppercaseLetters));
            this.Write(SecretOfManaEncoding.NewLine);
            this.WriteBytes(SecretOfManaEncoding.English.GetBytes(NamingDialogRomWriter.LowercaseLetters));
            this.Write(SecretOfManaEncoding.NewLine);
            this.WriteBytes(SecretOfManaEncoding.English.GetBytes(NamingDialogRomWriter.HelpText1));
            this.Write(SecretOfManaEncoding.NewLine);
            this.WriteBytes(SecretOfManaEncoding.English.GetBytes(NamingDialogRomWriter.HelpText2));
            this.Write(SecretOfManaEncoding.NewLine);
            this.WriteBytes(SecretOfManaEncoding.English.GetBytes(NamingDialogRomWriter.HelpText3));
            this.Write((byte)EventOpCodeType.End);

            // Patch the help text pointer table.
            this.Seek(NamingDialogRomWriter.NamingDialogHelpTextPatchLocation);
            this.WriteUInt24(NamingDialogRomWriter.NamingDialogTextAddress | 0xC00000);

            // Small patch to the routine that translates the screen tile to a encoded letter.
            ReadOnlySpan<byte> nameTransferPatch = stackalloc byte[]
            {
                0xC9, 0x3C, 0x00, //CMP #$003C    ;Compare value against 0x3C.
                0x90, 0x03,       //BCC $03       ;Branch over subtraction if the pointer is less than 0x3C.
                0xE9, 0x03, 0x00, //SBC #$0003    ;Subtract 0x03 from the pointer.
                0xAA,             //TAX           ;Transfer the pointer to Register X.
                0xE2, 0x20,       //SEP #$20      ;Disable 16-Bit Accumulator.
                0x60,             //RTS           ;Return.
            };
            this.Seek(NamingDialogRomWriter.NameTransferPatchRoutineAddress);
            this.WriteBytes(nameTransferPatch);

            // Jump to the patched code in the name transfer routine.
            this.Seek(NamingDialogRomWriter.NameTransferPatchLocation1);
            this.Write(0x20); // JSR
            this.WriteUInt16(NamingDialogRomWriter.NameTransferPatchRoutineOffset);

            // Patch the transfer routine to look at the new alphabet event.
            this.Seek(0x0750B5);
            this.WriteUInt16(AlphabetTextPatchValue);

            // Patch the layout pointer to mostly do nothing.
            this.Seek(NamingDialogRomWriter.LayoutPointerTablePatchLocation);
            this.WriteUInt16(NamingDialogRomWriter.NullLayoutOffset);
            this.WriteUInt16(0x759B); //MenuLayoutData_NamingDialogTextBox
            this.WriteUInt16(NamingDialogRomWriter.NullLayoutOffset);

            ReadOnlySpan<byte> fcmLayout = stackalloc byte[]
            {
                0x01,       //;Define TextBox
                0xC0,       //;ABCD Text Box X Position
                0x02,       //;ABCD Text Box Y Position
                0x04,       //;ABCD Text Box Y Size
                0x1E,       //;ABCD Text Box X Size

                0x01,       //;Define TextBox
                0xC0,       //;Help Text Box X Position
                0x04,       //;Help Text Box Y Position
                0x06,       //;Help Text Box Y Size
                0x1E,       //;Help Text Box X Size

                0x81,       //;Define Special TextBox
                0x90,       //;Name Text Box X Position
                0x00,       //;Name Text Box Y Position
                0x02,       //;Name Text Box Y Size
                0x07,       //;Name Text Box X Size

                0x00,       //;End Layout
            };
            this.Seek(NamingDialogRomWriter.LayoutAddress);
            this.WriteBytes(fcmLayout);

            this.WriteUpDownControllerRoutine();
        }

        private void WriteUpDownControllerRoutine()
        {
            ReadOnlySpan<byte> upRoutine = stackalloc byte[]
            {
                // [Naming Dialog Up Button]
                0x20, 0x4A, 0x32,  	        //JSR $324A
                0xAD, 0x5A, 0xA1,  	        //LDA $A15A             ;Cursor location.
                0x38,      	                //SEC                   ;Set Carry Flag.
                0xE9, 0x10,    	            //SBC #$10              ;How much to move the cursor
                0xC9, 0x51,    	            //CMP #$51              (51)
                0xB0, 0x13,    	            //BCS $13
                0xA9, 0x70,    	            //LDA #$70              (70)
                0x80, 0x0F,    	            //BRA $0F
            };
            ReadOnlySpan<byte> downRoutine = stackalloc byte[]
            {
                // [Naming Dialog Down Button]
                0x20, 0x4A, 0x32,  	        //JSR $324A
                0xAD, 0x5A, 0xA1,  	        //LDA $A15A             ;Cursor location.
                0x18,      	                //CLC                   ;Clear Carry Flag.
                0x69, 0x10,    	            //ADC #$10
                0xC9, 0x71,    	            //CMP #$71              ;Compare result against 0xD0. (71)
                0x90, 0x02,    	            //BCC $02
                0xA9, 0x60,    	            //LDA #$60              ;Load 0x60 into Accumulator. (60)
                0x8D, 0x5A, 0xA1,  	        //STA $A15A
                0x22, 0x3D, 0x50, 0xC7,	    //JSR $C7503D           ;Jump and update the cursor.
                0x20, 0xAA, 0x1B,  	        //JSR $1BAA
                0x60,      	                //RTS                   ;Return.
            };

            // Some half translated garbage text is located here.
            this.Seek(NamingDialogRomWriter.UpDownControllerRoutineAddress);
            this.WriteBytes(upRoutine);
            int downPointer = this.Position;
            this.WriteBytes(downRoutine);

            // A massive pointer table contains all the routines used for buttons in FCMs.
            // The indexes for Up/Down need to be updated to the new routines. 
            // These buttons currently point to a generic "do nothing" routine.
            this.Seek(NamingDialogRomWriter.UpControllerPatchLocation);
            this.WriteUInt16((ushort)(NamingDialogRomWriter.UpDownControllerRoutineAddress & 0xFFFF));
            this.Seek(NamingDialogRomWriter.DownControllerPatchLocation);
            this.WriteUInt16((ushort)(downPointer & 0xFFFF));
        }
    }
}