using System.ComponentModel;
using ManaMagic.Core;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Bosses.Frames;
using ManaMagic.Core.Metadata;
using ZwellTech;

#nullable enable

namespace ManaMagic.Controls.UserControls.Boss
{
    [ReadOnly(true)]
    internal sealed class BossGraphicsTablePropertyObject
    {
        private readonly BossGraphicsTableEntry row;
        private readonly BossTilesetMetadata metadata;

        [Category("General")]
        [Description("The index of the tileset in the graphics table.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte Index { get; }

        [Category("General")]
        [Description("The name of the tileset.")]
        public string Name { get { return metadata.Name; } }

        [Category("Graphics Table")]
        [Description("The ROM bank the graphics are stored in.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte GraphicsBank { get { return row.GraphicsBank; } }

        [Category("Graphics Table")]
        [Description("The RAM bank the graphics will be loaded into.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte RamBank { get { return row.RamBank; } }

        [Category("Graphics Table")]
        [Description("The ROM address the graphics are stored in.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort GraphicsAddress { get { return row.GraphicsAddress; } }

        [Category("Graphics Table")]
        [Description("The RAM address the graphics will be loaded into.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort RamAddress { get { return row.RamAddress; } }

        [Category("Graphics Table")]
        [Description("The size of the graphics stored in the ROM in bytes.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort GraphicsSize { get { return row.GraphicsSize; } }

        [Category("Graphics Table")]
        [Description("The full ROM address of the graphics.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public int FullGraphicsAddress { get { return (this.GraphicsBank & 0x1F) << 16 | this.GraphicsAddress; } }

        [Category("Graphics Table")]
        [Description("The full RAM address of the graphics.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public int FullRamAddress { get { return (this.RamBank & 0x1F) << 16 | this.RamAddress; } }

        [Category("State")]
        [Description("Whether or not the tileset is used in-game.")]
        public bool DummiedOut { get { return metadata.IsDummiedOut; } }

        [Category("State")]
        [Description("If true, then the graphics are to be used during Mode 7.")]
        public bool Mode7 { get { return metadata.IsMode7; } }

        [Category("State")]
        [Description("If true, then the graphics are decompressed using the primary LZ77 Decompressor.")]
        public bool DoubleCompressed { get { return metadata.IsDualCompressed; } }

        public BossGraphicsTablePropertyObject(byte index, BossGraphicsTableEntry row, BossTilesetMetadata metadata)
        {
            this.Index = index;
            this.row = row;
            this.metadata = metadata;
        }
    }

    [ReadOnly(true)]
    internal sealed class BossFramePropertyObject
    {
        private readonly int index = 0;
        private readonly BossFrameMetadata metadata;

        [Category("General")]
        [Description("The internal index of the frame.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public int Index { get { return this.index; } }

        [Category("General")]
        [Description("The name of the frame.")]
        public string Name { get { return metadata.DisplayName; } }

        [Category("General")]
        [Description("The type of the frame.")]
        public BossFrameType FrameType { get { return metadata.FrameType; } }

        [Category("Addressing")]
        [Description("The ROM bank the frame is stored in.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte Bank { get { return metadata.Bank; } }

        [Category("Addressing")]
        [Description("The ROM address the frame is stored in.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort Address { get { return metadata.Address; } }

        [Category("Addressing")]
        [Description("The ROM full address the frame is stored in.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public int FullAddress { get { return metadata.FullAddress; } }

        public BossFramePropertyObject(int index, BossFrameMetadata metadata)
        {
            this.index = index;
            this.metadata = metadata;
        }
    }

    [ReadOnly(true)]
    internal sealed class BossSkillPropertyObject
    {
        private readonly ManaBossSkill2 skill;

        [Category("General")]
        [Description("The index of the skill.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte Index { get; }

        [Category("General")]
        [Description("The name of the skill.")]
        public string Name { get { return skill.Name; } }

        [Category("General")]
        [Description("The pointer to the skills AI in Bank C2.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort AIPointer { get { return skill.AIPointer; } }

        [Category("Indexes")]
        [Description("The index of the palette the skill uses.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort PaletteIndex { get { return skill.PaletteIndex; } }

        [Category("Indexes")]
        [Description("The index of the animation played on the boss when the skill is used.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort BossAnimationIndex { get { return skill.BossAnimationIndex; } }

        [Category("Indexes")]
        [Description("The index of the animation played on the player when the skill is used.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort PlayerAnimationIndex { get { return skill.PlayerAnimationIndex; } }

        [Category("Indexes")]
        [Description("The index of the sound effect played during the boss animation when the skill is used.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort BossSoundEffectIndex { get { return skill.BossSoundEffectIndex; } }

        [Category("Indexes")]
        [Description("The index of the sound effect played during the player animation when the skill is used.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort PlayerSoundEffectIndex { get { return skill.PlayerSoundEffectIndex; } }

        [Category("Weapon Data (Default)")]
        [Description("The monster type this weapon will deal additional damage to. This field is unused.")]
        public MonsterType MonsterAffinity { get { return skill.DefaultWeapon.MonsterAffinity; } }

        [Category("Weapon Data (Default)")]
        [Description("The element of this weapon. This field is unused.")]
        public ElementalType Element { get { return skill.DefaultWeapon.Element; } }

        [Category("Weapon Data (Default)")]
        [Description("The accuracy of this weapon.")]
        [TypeConverter(typeof(PercentageTypeConverter))]
        public byte Accuracy { get { return skill.DefaultWeapon.Accuracy; } }

        [Category("Weapon Data (Default)")]
        [Description("The power of this weapon.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte Power { get { return skill.DefaultWeapon.Power; } }

        [Category("Weapon Data (Default)")]
        [Description("The status effects inflicted by this weapon.")]
        public StatusEffects StatusEffects { get { return skill.DefaultWeapon.StatusEffects; } }

        [Category("Weapon Data (Default)")]
        [Description("The rate at which this weapon will inflict its status effects.")]
        [TypeConverter(typeof(PercentageTypeConverter))]
        public byte InflictionRate { get { return skill.DefaultWeapon.InflictionRate; } }

        public BossSkillPropertyObject(byte index, ManaBossSkill2 skill)
        {
            this.Index = index;
            this.skill = skill;
        }
    }
}