using ManaMagic.Core.Bosses;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class BossRomWriter : RomWriter
    {
        /// <inheritdoc/>
        public BossRomWriter(WritableRomFile rom) : base(rom) { }

        public void WritePaletteTable(BossContext context)
        {
            for (int i = 0; i < context.PaletteTable.RowCount; i++)
            {
                // Put the palette back where we found it.
                SpritePalette palette = context.PaletteTable[i];
                this.Seek(palette.Address);
                for (int colorIndex = 0; colorIndex < palette.NumberOfColors; colorIndex++)
                {
                    this.WriteUInt16(palette[colorIndex].ToRgb555(Rgb555Format.BGR));
                }
            }
        }

        public void WriteWeaponTable(BossContext context)
        {
            this.Seek(Constants.Bank10.BossWeaponTableAddress);
            foreach (ManaBossWeapon weapon in context.WeaponTable)
            {
                this.Write((byte)weapon.MonsterAffinity);
                this.Write((byte)weapon.Element);
                this.Write(weapon.Accuracy);
                this.Write(weapon.Power);
                this.WriteUInt16((ushort)weapon.StatusEffects);
                this.Write(weapon.InflictionRate);
            }
        }

        public void WriteSetupHeaders(BossContext context)
        {
            for (int i = 0; i < context.StateMachineSetupPointerTable.RowCount; i++)
            {
                this.Seek(Constants.Bank02Offset | context.StateMachineSetupPointerTable[i]);
                BossSetupHeader header = context.SetupHeaders[i];

                this.WriteUInt16((ushort)header.ControlFlags);
                this.WriteUInt16(header.SpriteFlags);
                this.WriteUInt16(header.InitializeRoutinePointer);
                this.WriteUInt16(header.MovementRoutinePointer);
                this.WriteUInt16(header.AttackRoutinePointer);
                this.WriteUInt16(header.DamagedRoutinePointer);
                this.WriteUInt16(header.DefaultCommandScriptPointer);
                this.WriteUInt16(header.DeathCommandScriptPointer);
                this.WriteUInt16(header.AICommandSetPointer);
                this.WriteUInt16(header.ShadowSize);
                this.WriteUInt16(header.PostDeathHandlerPointer);
                this.WriteUInt16(header.UnknownValue);
                this.WriteUInt16(header.SpecialDeathHandlerPointer);
            }
        }
    }
}