using System.Collections.Generic;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Bosses.Frames;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    public static partial class ManaMetadata
    {
        private static readonly (byte Primary, byte Secondary) EmptyAuxIdList = (ManaMetadata.NullBossTilesetAuxiliaryId, ManaMetadata.NullBossTilesetAuxiliaryId);
        private static readonly IReadOnlyDictionary<int, IReadOnlyList<(byte PaletteId, byte Slot)>> BossMode7PaletteSlotMap = new Dictionary<int, IReadOnlyList<(byte PaletteId, byte Slot)>>()
        {
            { 0x18, new List<(byte PaletteId, byte Slot)>() { (0x62, 0x04), (0x63, 0x05), (0x64, 0x06), (0x65, 0x07), (0x82, 0x0D) } },
            { 0x19, new List<(byte PaletteId, byte Slot)>() { (0x62, 0x04), (0x63, 0x05), (0x64, 0x06), (0x65, 0x07), (0x82, 0x0D) } },
            { 0x1A, new List<(byte PaletteId, byte Slot)>() { (0x5B, 0x07), (0x5B, 0x0D), (0x7E, 0x0E) } },
            { 0x1F, new List<(byte PaletteId, byte Slot)>() { (0x62, 0x04), (0x63, 0x05), (0x64, 0x06), (0x65, 0x07), (0x82, 0x0D) } },
            { 0x20, new List<(byte PaletteId, byte Slot)>() { (0x62, 0x04), (0x63, 0x05), (0x64, 0x06), (0x65, 0x07), (0x82, 0x0D) } },
            { 0x28, new List<(byte PaletteId, byte Slot)>() { (0x62, 0x04), (0x63, 0x05), (0x64, 0x06), (0x65, 0x07), (0x82, 0x0D) } },
        };

        private static readonly IReadOnlyDictionary<ushort, string> AnimationNameStrings = new Dictionary<ushort, string>()
        {
            { 0x0000, "General_Invisible" },
            { 0x0001, "General_Debug (Dummied Out)" },
            { 0x0002, "General_Dummied" },
            { 0x0003, "General_VerySmallShadow" },
            { 0x0004, "General_SmallShadow" },
            { 0x0005, "General_MediumShadow" },
            { 0x0006, "General_LargeShadow" },
            { 0x0007, "General_VerySmallShadow (Dummied Out)" },
            { 0x0008, "General_VerySmallShadow (Dummied Out)" },
            { 0x0009, "General_VerySmallShadow (Dummied Out)" },

            { 0x000A, "MantisAnt_Idle" },
            { 0x000B, "MantisAnt_HeadTurn (Dummied Out) (Broken)" },
            { 0x000C, "MantisAnt_SkillUse" },
            { 0x000D, "MantisAnt_WalkSideView" },
            { 0x000E, "MantisAnt_WalkDownView" },
            { 0x000F, "MantisAnt_DoubleKamaSwing" },
            { 0x0010, "MantisAnt_KamaDance" },
            { 0x0011, "MantisAnt_WalkBackView (Dummied Out)" },
            { 0x0012, "MantisAnt_KamaSwing" },
            { 0x0013, "MantisAnt_Jump" },
            { 0x0014, "MantisAnt_GuardPose" },
            { 0x0015, "MantisAnt_PreJump" },
            { 0x0016, "MantisAnt_PostJump" },
            { 0x0017, "MantisAnt_KamaProjectile" },

            { 0x0018, "JABBERWOCKY_ANIMATION_00" },
            { 0x0019, "JABBERWOCKY_ANIMATION_01" },
            { 0x001A, "JABBERWOCKY_ANIMATION_02" },
            { 0x001B, "JABBERWOCKY_ANIMATION_03" },
            { 0x001C, "JABBERWOCKY_ANIMATION_04" },
            { 0x001D, "JABBERWOCKY_ANIMATION_05" },
            { 0x001E, "JABBERWOCKY_ANIMATION_06" },

            { 0x001F, "Kilroy_Idle" },
            { 0x0020, "Kilroy_LeftHammerSwing" },
            { 0x0021, "Kilroy_ArchHammerSwing" },
            { 0x0022, "Kilroy_ShortCircuit" },
            { 0x0023, "Kilroy_WalkDownView" },
            { 0x0024, "Kilroy_HammerSpin" },
            { 0x0025, "Kilroy_Shake (Dummied Out)" },
            { 0x0026, "Kilroy_WheelieDownView" },
            { 0x0027, "Kilroy_IdleSideView" },
            { 0x0028, "Kilroy_RightHammerSmash" },
            { 0x0029, "Kilroy_IdleSideViewAlternate (Dummied Out)" },
            { 0x002A, "Kilroy_WalkSideView" },
            { 0x002B, "Kilroy_ShakeSideView (Dummied Out)" },
            { 0x002C, "Kilroy_WheelieSideView" },
            { 0x002D, "Kilroy_IdleBackView" },
            { 0x002E, "Kilroy_UpHammerSmash" },
            { 0x002F, "Kilroy_UpArchHammer" },
            { 0x0030, "Kilroy_ArmTwitchBackView (Dummied Out)" },
            { 0x0031, "Kilroy_WalkBackView" },
            { 0x0032, "Kilroy_ShakeBackView (Dummied Out)" },
            { 0x0033, "Kilroy_WheelieBackView" },

            { 0x0034, "KilroyAuxiliary_LegsIdle" },
            { 0x0035, "KilroyAuxiliary_LegsWalkDownView" },
            { 0x0036, "KilroyAuxiliary_LegsIdleSideView" },
            { 0x0037, "KilroyAuxiliary_LegsWalkSideView" },
            { 0x0038, "KilroyAuxiliary_LegsIdleBackView" },
            { 0x0039, "KilroyAuxiliary_LegsWalkBackView" },
            { 0x003A, "KilroyAuxiliary_WheelTurn (Dummied Out)" },
            { 0x003B, "KilroyAuxiliary_WheelIdleDownView" },
            { 0x003C, "KilroyAuxiliary_WheelIdleSideView" },
            { 0x003D, "KilroyAuxiliary_WheelIdleDiagonalView (Dummied Out)" },
            { 0x003E, "DeathMachineAuxiliary_DrillIdle (Dummied Out)" },
            { 0x003F, "DeathMachineAuxiliary_DrillSpin (Dummied Out)" },
            { 0x0040, "DeathMachineAuxiliary_DrillInGround (Dummied Out)" },

            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
            //{ 0x0000, "" },
        };

        public static IReadOnlyDictionary<byte, string> BossWeaponNameStrings { get; } = new Dictionary<byte, string>()
        {
            { 0x00, "Skill_FireBreath" },
            { 0x01, "Skill_FreezeBreath" },
            { 0x02, "Skill_BlitzBreath" },
            { 0x03, "Skill_AcidBreath" },
            { 0x04, "Skill_PoisonBubbles (Dummied Out)" },
            { 0x05, "Skill_AcidBubbles" },
            { 0x06, "Skill_MoogleBubbles (Dummied Out)" },
            { 0x07, "Skill_PetrifyGas" },
            { 0x08, "Skill_SleepGas" },
            { 0x09, "Skill_PoisonGas" },
            { 0x0A, "Skill_FlashBeam" },
            { 0x0B, "Skill_FireBeam" },
            { 0x0C, "Skill_ThunderBeam (Dummied Out)" },
            { 0x0D, "Skill_DoomBeam (Dummied Out)" },
            { 0x0E, "Skill_FreezeBeam" },
            { 0x0F, "Skill_PetrifyBeam" },
            { 0x10, "Skill_ConfuseHoops" },
            { 0x11, "Skill_SonicPulse" },
            { 0x12, "Skill_BalloonRing" },
            { 0x13, "Skill_SleepRing" },
            { 0x14, "Skill_SleepRing2" },
            { 0x15, "Skill_MoogleGlare" },
            { 0x16, "Skill_PygmusGlare" },
            { 0x17, "Skill_LeadenGlare" },
            { 0x18, "Skill_MoogleGlare (Dummied Out)" },
            { 0x19, "Skill_WaveCannon" },
            { 0x1A, "Skill_DiffuserCannon" },
            { 0x1B, "Skill_Current" },
            { 0x1C, "Tropicallo_Default" },
            { 0x1D, "BorealFace_Default" },
            { 0x1E, "Tropicallo_Brambler_Default" },
            { 0x1F, "BorealFace_Brambler_Default (Dummied Out)" },
            { 0x20, "Tiger_LostAttack (Dummied Out)" },
            { 0x21, "Tiger_Pinball" },
            { 0x22, "Tiger_SpikeBall" },
            { 0x23, "Tiger_Maw" },
            { 0x24, "Tonpole_Weapon01" },
            { 0x25, "Tonpole_Weapon02" },
            { 0x26, "Tonpole_Weapon03" },
            { 0x27, "Minotaur_Fist" },
            { 0x28, "Minotaur_Horns" },
            { 0x29, "GorgonBull_Charge" },
            { 0x2A, "GorgonBull_LostAttack (Dummied Out)" }, // No idea, probably belongs to Gorgon Bull, maybe the lost jump attack?"
            { 0x2B, "GorgonBull_VaderBomb" },
            { 0x2C, "GorgonBull_ArmClaw" },
            { 0x2D, "GorgonBull_ClawCharge" },
            { 0x2E, "DoomWall_Desperation" },
            { 0x2F, "Kilroy_Hammer_Medium" },
            { 0x30, "Kilroy_Hammer_Weak" },
            { 0x31, "Kilroy_Hammer_Strong" },
            { 0x32, "KettleKin_Hammer_Medium" },
            { 0x33, "DeathMachine_Chainsaw_Weak (Dummied Out)" },
            { 0x34, "KettleKin_Hammer_Strong" },
            { 0x35, "Skill_FireGigas_FireBreath" },
            { 0x36, "Skill_RedDragon_FireBreath" },
            { 0x37, "Skill_SnowDragon_FreezeBreath" },
            { 0x38, "Beak_Stomp" },
            { 0x39, "Beak_Jump" },
            { 0x3A, "Skill_DragonWorm_PetrifyGas" },
            { 0x3B, "Skill_DarkLich_SleepGas" },
            { 0x3C, "GreatViper_Lunge" },
            { 0x3D, "GreatViper_SwallowWhole" },
            { 0x3E, "DragonWorm_SwallowWhole" },
            { 0x3F, "MechRider_BikeImpale" },
            { 0x40, "Skill_DarkLich_PoisonGas" },
            { 0x41, "MechRider_PsychoBikeImpale" },
            { 0x42, "MechRider_BikeDrift" },
            { 0x43, "MechRider_LostAttack (Dummied Out)" },
            { 0x44, "MantisAnt_Kama" },
            { 0x45, "MantisAnt_Kama_Duplicate (Dummied Out)" },
            { 0x46, "MechRider_Missile" },
            { 0x47, "Skill_DarkLich_ConfuseHoops" },
            { 0x48, "ThunderGigas_SpellFollowup" },
            { 0x49, "Skill_DarkLich_BalloonRing" },
            { 0x4A, "Skill_DarkLich_PygmusGlare" },
            { 0x4B, "Skill_Vampire_LeadenGlare" },
            { 0x4C, "Skill_DarkLich_LeadenGlare" },
            { 0x4D, "Vampire_Weapon01" },
            { 0x4E, "Vampire_Weapon02" },
            { 0x4F, "Aegagropilon_Weapon01" },
            { 0x50, "Vampire_Weapon03_Duplicate01 (Dummied Out)" },
            { 0x51, "Vampire_Weapon03" },
            { 0x52, "Vampire_Weapon04" },
            { 0x53, "Slime_Weapon01" },
            { 0x54, "Slime_Weapon02" },
            { 0x55, "Slime_Weapon03" },
            { 0x56, "Slime_Weapon04" },
            { 0x57, "Unused_Weapon01 (Dummied Out)" }, // Pygmy
            { 0x58, "Unused_Weapon02 (Dummied Out)" }, // Knocked Out
            { 0x59, "Unused_Weapon03 (Dummied Out)" }, // Pygmy
            { 0x5A, "Unused_Weapon04 (Dummied Out)" },
            { 0x5B, "Unused_Weapon05 (Dummied Out)" },
            { 0x5C, "Aegagropilon_Weapon02" },
            { 0x5D, "Aegagropilon_Weapon03" },
            { 0x5E, "Unused_Weapon06 (Dummied Out)" },
            { 0x5F, "Aegagropilon_Weapon04" },
            { 0x60, "Jabberwocky_Weapon01" },
            { 0x61, "Jabberwocky_Weapon02" },
            { 0x62, "Jabberwocky_Weapon03" },
            { 0x63, "MantisAnt_ThrowingKama" },
            { 0x64, "Hexas_Serpent" },
            { 0x65, "Unused_Weapon07 (Dummied Out)" },
            { 0x66, "Unused_Weapon08 (Dummied Out)" },
            { 0x67, "Dragon_Tail" },
            { 0x68, "Unused_Weapon09 (Dummied Out)" },
            { 0x69, "Unused_Weapon0A (Dummied Out)" },
            { 0x6A, "Unused_Weapon0B (Dummied Out)" },
            { 0x6B, "Unused_Weapon0C (Dummied Out)" }, // Frosty
            { 0x6C, "Unused_Weapon0D (Dummied Out)" }, // Engulf
            { 0x6D, "Skill_BreathWing" },
            { 0x6E, "DarkLich_SkeletalHand" },
            { 0x6F, "ManaBeast_Weapon01_Duplicate (Dummied Out)" },
            { 0x70, "ManaBeast_Weapon01" },
            { 0x71, "ManaBeast_Weapon02" },
            { 0x72, "ManaBeast_Weapon03" },
        };

        /// <summary>
        /// Represents a null boss tileset Auxiliary Id.
        /// </summary>
        public const byte NullBossTilesetAuxiliaryId = 0xFF;

        /// <summary>
        /// The offset to subtract from a boss tile Id to account for the shadow tiles.
        /// </summary>
        public const byte BossShadowTileOffset = 0x20;

        /// <summary>
        /// The offset to subtract from a boss tile Id to account for where in memory the crystal graphics are loaded.
        /// </summary>
        public const byte BossCrystalTilesetOffset = 0x40;

        /// <summary>
        /// The offset to subtract from a boss tile Id to account for where in memory the explosion graphics are loaded.
        /// </summary>
        public const byte BossExplosionTilesetOffset = 0x80;

        /// <summary>
        /// The offset to subtract from a boss tile Id to account for where in memory the robot auxiliary graphics are loaded.
        /// </summary>
        public const byte BossRobotAuxiliaryTilesetOffset = 0x80;

        /// <summary>
        /// The offset to subtract from a boss tile Id to account for where in memory the gigas auxiliary graphics are loaded.
        /// </summary>
        public const byte BossGigasAuxiliaryTilesetOffset = 0x20;

        /// <summary>
        /// The offset to subtract from a boss tile Id to account for where in memory the skill graphics are loaded.
        /// </summary>
        public const byte BossSkillTilesetOffset = 0x80;

        /// <summary>
        /// The amount to extend a boss tile Id by when <see cref="ManaMagic.Core.Bosses.Frames.BossFrameFlags.ExtendTileId"/> is set.
        /// </summary>
        public const ushort BossExtendTileIdAmount = 0x0100;

        /// <summary>
        /// Gets the metadata for boss tilesets.
        /// </summary>
        public static IReadOnlyList<BossTilesetMetadata> BossTilesetMetadata { get; } = new List<BossTilesetMetadata>()
        {
            // While it is possible to parse this data out of the ROM directly, doing so is problematic for a few reasons.
            // The biggest of which is only the used graphics have scripts to be parsed.
            // Bosses may also load palettes and whole graphics sets via custom routines which are not listed in the loading scripts.
            // Some Examples:
            // 1) Wall/Chamber's Eye have empty scripts as the main wall script controls them.
            // 2) The Mana Beast uses the CallRoutine op code to indirectly call OpCode 1A to load one of two graphic sets.
            // 3) Dark Lich will constantly load and swap palettes.
            // 4) The Boss Explosion and it's palette are hard-coded into the routine that loads them.
            // How To Find This Information In The Rom
            // Index: D0/BC52 is the start of the boss graphics data table. Each row is 8 bytes long,
            //        It is impossible to discover the correct max index. Parsing the graphics script will stop at 0x26
            //        which will miss the graphics for BossExplosion and several unused sets.
            //        BossExplosion could be parsed directly from the death routine however.
            // DualCompressed: If the graphics are loaded with OpCode 12 or 1D then the main LZ77 Decompression is required.
            // Mode7: OpCodes 1A and 1E load mode 7 graphics, since the Mana Beast loads graphics indirectly, only slime graphics can be discovered.
            // OffsetByShadowTiles: Graphics loaded into 00/6200 count the shadow graphics are part of their tile set and their frame data references
            //                      tile Ids with their values decreased by 0x20 to account for this.
            new BossTilesetMetadata(0x00, "Tropicallo Tileset",                  (0x20, 0x21, 0x24), EmptyAuxIdList, BossFamily.Plant,                  BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x01, "Dragon Layer 2 Tileset",              (0x74, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.DragonBody,             BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x02, "Dragon Tileset",                      (0x74, 0x77, 0x77), EmptyAuxIdList, BossFamily.Dragon,                 BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x03, "Robot Tileset",                       (0x12, 0x13, 0xFF), (0x04, 0x05),   BossFamily.Robot,                  BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x04, "Robot Auxiliary Tileset",             (0x12, 0x13, 0xFF), EmptyAuxIdList, BossFamily.RobotAuxiliaryHammer,   BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x05, "Death Machine Auxiliary Tileset",     (0x16, 0x17, 0xFF), EmptyAuxIdList, BossFamily.RobotAuxiliaryChainsaw, BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x06, "Jabberwocky Tileset",                 (0x10, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Hydra,                  BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x07, "Ant Tileset",                         (0x0C, 0x0E, 0xFF), EmptyAuxIdList, BossFamily.Ant,                    BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x08, "Minotaur Tileset",                    (0x1A, 0x1B, 0xFF), EmptyAuxIdList, BossFamily.Minotaur,               BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x09, "Aegagropilon Tileset",                (0x2A, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Aegagropilon,           BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x0A, "Aegagropilon Layer 2 Tileset",        (0x2A, 0x2A, 0x2A), EmptyAuxIdList, BossFamily.AegagropilonBody,       BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x0B, "Partial Bird Tileset",                (0x67, 0x67, 0xFF), EmptyAuxIdList, BossFamily.Bird,                   BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x0C, "Tiger Tileset",                       (0x2D, 0x2D, 0xFF), EmptyAuxIdList, BossFamily.Tiger,                  BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x0D, "Lizard Tileset",                      (0x32, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Lizard,                 BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x0E, "Gigas Tileset",                       (0x35, 0xFF, 0xFF), (0x0F, 0x10),   BossFamily.Gigas,                  BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x0F, "Gigas Diamond Tileset",               (0x35, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.GigasAuxiliaryDiamond,  BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x10, "Gigas Orb Tileset",                   (0x36, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.GigasAuxiliaryOrb,      BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x11, "Super Sized Hexas Tileset",           (0x37, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Hexas,                  BossTilesetMetadataFlags.OffsetByShadowTiles | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x12, "Bird Tileset",                        (0x67, 0x67, 0xFF), EmptyAuxIdList, BossFamily.Bird,                   BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x13, "Mech Rider Tileset",                  (0x56, 0x57, 0x58), EmptyAuxIdList, BossFamily.MechRider,              BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x14, "Serpent Tileset",                     (0x69, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Serpent,                BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x15, "Vampire Tileset",                     (0x39, 0x3A, 0xFF), EmptyAuxIdList, BossFamily.Vampire,                BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x16, "Dark Lich Tileset",                   (0x3C, 0xFF, 0xFF), (0x25, 0xFF),   BossFamily.Lich,                   BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x17, "Dark Lich Layer 2 Tileset",           (0x42, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.LichBody,               BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x18, "Close Up Mana Beast Tileset",         (0x62, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.ManaBeast,              BossTilesetMetadataFlags.Mode7),
            new BossTilesetMetadata(0x19, "Ranged Mana Beast Tileset",           (0x62, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.ManaBeast,              BossTilesetMetadataFlags.Mode7),
            new BossTilesetMetadata(0x1A, "Slime Mode 7 Tileset",                (0x5B, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.SlimeBody,              BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.Mode7),
            new BossTilesetMetadata(0x1B, "Slime Tileset",                       (0x5B, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Slime,                  BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x1C, "Wall Layer 2 Tileset",                (0x6B, 0x6C, 0x6C), EmptyAuxIdList, BossFamily.Wall,                   BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x1D, "Wall Eye Tileset",                    (0x70, 0x6C, 0x6C), EmptyAuxIdList, BossFamily.WallEye,                BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x1E, "Pumpkin Bomb Tileset",                (0x22, 0x22, 0x22), EmptyAuxIdList, BossFamily.Pumpkin,                BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x1F, "Close Up Mana Beast Tileset",         (0x62, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.ManaBeast,              BossTilesetMetadataFlags.Mode7 | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x20, "Ranged Mana Beast Tileset",           (0x62, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.ManaBeast,              BossTilesetMetadataFlags.Mode7 | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x21, "Hexas Tileset",                       (0x37, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Hexas,                  BossTilesetMetadataFlags.OffsetByShadowTiles),
            new BossTilesetMetadata(0x22, "Hexas Serpent Tileset",               (0x37, 0x78, 0x38), EmptyAuxIdList, BossFamily.HexasSerpent,           BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x23, "Slime Crystals Tileset",              (0x7E, 0x7F, 0xFF), EmptyAuxIdList, BossFamily.Crystal,                BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x24, "Battle Platform Tileset",             (0x83, 0x82, 0xFF), EmptyAuxIdList, BossFamily.Platform,               BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x25, "Dark Lich Auxiliary Tileset",         (0x3C, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.LichAuxiliary,          BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x26, "Dark Lich Layer 2 Tileset Alternate", (0x42, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.LichBody,               BossTilesetMetadataFlags.DualCompressed),
            new BossTilesetMetadata(0x27, "Explosion Tileset",                   (0x48, 0x49, 0x49), EmptyAuxIdList, BossFamily.Explosion,              BossTilesetMetadataFlags.None),
            new BossTilesetMetadata(0x28, "Ranged Mana Beast Tileset",           (0x62, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.ManaBeast,              BossTilesetMetadataFlags.Mode7 | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x29, "Hexas Tileset",                       (0x37, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.Hexas,                  BossTilesetMetadataFlags.OffsetByShadowTiles | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x2A, "Hexas Serpent Tileset",               (0x37, 0x78, 0x38), EmptyAuxIdList, BossFamily.HexasSerpent,           BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x2B, "Slime Crystals Tileset",              (0x7E, 0x7F, 0xFF), EmptyAuxIdList, BossFamily.Crystal,                BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x2C, "Battle Platform Tileset",             (0x83, 0x82, 0xFF), EmptyAuxIdList, BossFamily.Platform,               BossTilesetMetadataFlags.DualCompressed | BossTilesetMetadataFlags.DummiedOut),
            new BossTilesetMetadata(0x2D, "Dark Lich Auxiliary Tileset",         (0x3C, 0xFF, 0xFF), EmptyAuxIdList, BossFamily.LichAuxiliary,          BossTilesetMetadataFlags.DummiedOut),
        };

        public static IReadOnlyList<BossFamily> BossSkillIndexToFamilyMapping { get; } = new List<BossFamily>()
        {
            BossFamily.FireBreath,         // [00: Fire Breath]
            BossFamily.FreezeBreath,       // [01: Freeze Breath]
            BossFamily.BlitzBreath,        // [02: Blitz Breath]
            BossFamily.AcidBreath,         // [03: Acid Breath]
            BossFamily.StatusBubbles,      // [04: Poison Bubbles (Dummied Out)]
            BossFamily.StatusBubbles,      // [05: Acid Bubbles]
            BossFamily.StatusBubbles,      // [06: Moogle Bubbles (Dummied Out)]
            BossFamily.GasAttack,          // [07: Petrify Gas]
            BossFamily.GasAttack,          // [08: Sleep Gas]
            BossFamily.GasAttack,          // [09: Poison Gas]
            BossFamily.BeamAttack,         // [0A: Flash Beam]
            BossFamily.BeamAttack,         // [0B: Fire Beam]
            BossFamily.BeamAttack,         // [0C: Thunder Beam (Dummied Out)]
            BossFamily.BeamAttack,         // [0D: Doom Beam (Dummied Out)]
            BossFamily.BeamAttack,         // [0E: Freeze Beam]
            BossFamily.BeamAttack,         // [0F: Petrify Beam]
            BossFamily.RingAttack,         // [10: Confuse Hoops]
            BossFamily.RingAttack,         // [11: Sonic Pulse]
            BossFamily.RingAttack,         // [12: Balloon Ring]
            BossFamily.RingAttack,         // [13: Sleep Ring]
            BossFamily.RingAttack,         // [14: Sleep Ring]
            BossFamily.GlareAttack,        // [15: Moogle Glare]
            BossFamily.GlareAttack,        // [16: Pygmus Glare]
            BossFamily.GlareAttack,        // [17: Leaden Glare]
            BossFamily.GlareAttack,        // [18: Moogle Glare]
            BossFamily.CannonAttack,       // [19: Wave Cannon]
            BossFamily.CannonAttack,       // [1A: Diffuser Cannon]
            BossFamily.Current,            // [1B: Current]
            BossFamily.BossSpecificAttack, // [1C: NoName - Doom's Wall Desperation Attack]
            BossFamily.BossSpecificAttack, // [1D: Breath Wing]
            BossFamily.BossSpecificAttack, // [1E: Cave-In]
        };

        public static uint GetBossSetupHeaderAddress(BossFamily family)
        {
            switch (family)
            {
                case BossFamily.Aegagropilon:
                    return Constants.Bank02.AegagropilonSetupHeaderAddress;
                case BossFamily.Ant:
                    return Constants.Bank02.AntFamilySetupHeaderAddress;
                case BossFamily.Lich:
                    return Constants.Bank02.DarkLichSetupHeaderAddress;
                case BossFamily.DeathMachine:
                    return Constants.Bank02.DeathMachineSetupHeaderAddress;
                case BossFamily.Dragon:
                    return Constants.Bank02.DragonFamilySetupHeaderAddress;
                case BossFamily.MechRider:
                    return Constants.Bank02.MechRiderFamilySetupHeaderAddress;
                case BossFamily.KettleKin:
                    return Constants.Bank02.KettleKinSetupHeaderAddress;
                case BossFamily.Hexas:
                    return Constants.Bank02.HexasFamilySetupHeaderAddress;
                default:
                    return (uint)ThrowHelper.ThrowArgumentException("Boss family does not have a setup header.", nameof(family));
            }
        }

        public static BossFamily GetFamilyType(byte index)
        {
            switch (index)
            {
                case 0: return BossFamily.Ant;
                case 1: return BossFamily.Minotaur;
                case 2: return BossFamily.Robot;
                case 3: return BossFamily.Gigas;
                case 4: return BossFamily.Vampire;
                case 5: return BossFamily.Lizard;
                case 6: return BossFamily.DeathMachine;
                case 7: return BossFamily.MechRider;
                case 8: return BossFamily.Lich;
                case 9: return BossFamily.Hexas;
                case 10: return BossFamily.Serpent;
                case 11: return BossFamily.Dragon;
                case 12: return BossFamily.Tiger;
                case 13: return BossFamily.Bird;
                case 14: return BossFamily.Aegagropilon;
            }

            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
            return (BossFamily)(-1);
        }

        public static class BossFrameAddressFactory
        {
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> PlantFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                // Frames for the plants sprout growing.
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xE00A, BossFrameType.Sprite, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xE022, BossFrameType.Sprite, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xE03E, BossFrameType.Sprite, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xE05A, BossFrameType.Sprite, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xE072, BossFrameType.Sprite, string.Empty) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xE08E, BossFrameType.Sprite, string.Empty) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xE09C, BossFrameType.Sprite, string.Empty) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xE0BA, BossFrameType.Sprite, string.Empty) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xE102, BossFrameType.Sprite, string.Empty) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xE120, BossFrameType.Sprite, string.Empty) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xE13E, BossFrameType.Sprite, string.Empty) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xE15C, BossFrameType.Sprite, string.Empty) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xE16A, BossFrameType.Sprite, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> DragonBodyFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF3FC, BossFrameType.Background, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF40E, BossFrameType.Background, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF420, BossFrameType.Background, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF432, BossFrameType.Background, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> DragonFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xEF2E, BossFrameType.Sprite, "Idle 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xEF4C, BossFrameType.Sprite, "Idle 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xEF6A, BossFrameType.Sprite, "Idle 03") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xEF88, BossFrameType.Sprite, "Idle 04") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xEFA6, BossFrameType.Sprite, "Idle 05") },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xEFC4, BossFrameType.Sprite, "Look Around 01") },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xEFE0, BossFrameType.Sprite, "Look Around 02") },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xEFFC, BossFrameType.Sprite, "Look Around 03") },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xF01C, BossFrameType.Sprite, "Head Back 01") },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xF038, BossFrameType.Sprite, "Head Back 02") },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xF054, BossFrameType.Sprite, "Skill Use 01") },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xF070, BossFrameType.Sprite, "Skill Use 02") },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xF08C, BossFrameType.Sprite, "Skill Use 03") },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xF0A8, BossFrameType.Sprite, "Skill Use 04") },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xF0C4, BossFrameType.Sprite, "Skill Use 05") },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xF0E0, BossFrameType.Sprite, "Skill Use 06") },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xF0FC, BossFrameType.Sprite, "Skill Use 07") },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xF118, BossFrameType.Sprite, "Skill Use 08") },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xF134, BossFrameType.Sprite, "Skill Use 09") },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xF150, BossFrameType.Sprite, "Skill Use 0A") },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xF170, BossFrameType.Sprite, "Skill Use 0B") },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xF190, BossFrameType.Sprite, "Skill Use 0C") },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xF1B0, BossFrameType.Sprite, "Dragon Tail Idle 01") },
                { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xF1D0, BossFrameType.Sprite, "Dragon Tail Idle 02") },
                { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xF1F0, BossFrameType.Sprite, "Dragon Tail Idle 03") },
                { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xF210, BossFrameType.Sprite, "Dragon Tail Idle 04") },
                { 0x1A, new BossFrameMetadata(Constants.Bank0AByte, 0xF230, BossFrameType.Sprite, "Dragon Tail Idle 05") },
                { 0x1B, new BossFrameMetadata(Constants.Bank0AByte, 0xF250, BossFrameType.Sprite, "Dragon Tail Idle 06") },
                { 0x1C, new BossFrameMetadata(Constants.Bank0AByte, 0xF270, BossFrameType.Sprite, "Dragon Tail Idle 07") },
                { 0x1D, new BossFrameMetadata(Constants.Bank0AByte, 0xF290, BossFrameType.Sprite, "Dragon Tail Idle 08") },
                { 0x1E, new BossFrameMetadata(Constants.Bank0AByte, 0xF2B0, BossFrameType.Sprite, "Dragon Tail Idle 09") },
                { 0x1F, new BossFrameMetadata(Constants.Bank0AByte, 0xF2D0, BossFrameType.Sprite, "Dragon Tail Idle 0A") },
                { 0x20, new BossFrameMetadata(Constants.Bank0AByte, 0xF2F0, BossFrameType.Sprite, "Dragon Tail Idle 0B") },
                { 0x21, new BossFrameMetadata(Constants.Bank0AByte, 0xF310, BossFrameType.Sprite, "Dragon Tail Idle 0C") },
                { 0x22, new BossFrameMetadata(Constants.Bank0AByte, 0xF330, BossFrameType.Sprite, "Tail Chain 01") },
                { 0x23, new BossFrameMetadata(Constants.Bank0AByte, 0xF352, BossFrameType.Sprite, "Tail Chain 02") },
                { 0x24, new BossFrameMetadata(Constants.Bank0AByte, 0xF374, BossFrameType.Sprite, "Tail Chain 03") },
                { 0x25, new BossFrameMetadata(Constants.Bank0AByte, 0xF396, BossFrameType.Sprite, "Tail Chain 04") },
                { 0x26, new BossFrameMetadata(Constants.Bank0AByte, 0xF3B8, BossFrameType.Sprite, "Tail Chain 05") },
                { 0x27, new BossFrameMetadata(Constants.Bank0AByte, 0xF3DA, BossFrameType.Sprite, "Tail Chain 06") },
                { 0x28, new BossFrameMetadata(Constants.Bank1BByte, 0xFEC2, BossFrameType.Sprite, "Breath Wing (Player Animation) 01", false) },
                { 0x29, new BossFrameMetadata(Constants.Bank1BByte, 0xFED8, BossFrameType.Sprite, "Breath Wing (Player Animation) 02", false) },
                { 0x2A, new BossFrameMetadata(Constants.Bank1BByte, 0xFEF2, BossFrameType.Sprite, "Breath Wing (Player Animation) 03", false) },
                { 0x2B, new BossFrameMetadata(Constants.Bank1BByte, 0xFF0C, BossFrameType.Sprite, "Breath Wing (Player Animation) 04", false) },
                { 0x2C, new BossFrameMetadata(Constants.Bank1BByte, 0xFF26, BossFrameType.Sprite, "Breath Wing (Player Animation) 05", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> RobotFrameLookup = new Dictionary<int, BossFrameMetadata>
             {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xCA34, BossFrameType.Sprite, "Idle") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xCA76, BossFrameType.Sprite, "Left Hammer Smash 01") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xCAB8, BossFrameType.Sprite, "Left Hammer Smash 02") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xCAFC, BossFrameType.Sprite, "Left Hammer Smash 03") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xCB40, BossFrameType.Sprite, "Hammer Swing 01") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xCB84, BossFrameType.Sprite, "Hammer Swing 02") },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xCBC8, BossFrameType.Sprite, "Short Circuit 01") },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xCC08, BossFrameType.Sprite, "Short Circuit 02") },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xCC3C, BossFrameType.Sprite, "Walk, Down View 01") },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xCC7E, BossFrameType.Sprite, "Walk, Down View 02") },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xCCC0, BossFrameType.Sprite, "Hammer Spin 01") },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xCD04, BossFrameType.Sprite, "Hammer Spin 02") },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xCD48, BossFrameType.Sprite, "Hammer Spin 03") },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xCD88, BossFrameType.Sprite, "Hammer Spin 04") },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xCDD0, BossFrameType.Sprite, "Hammer Spin 05") },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xCE5A, BossFrameType.Sprite, "Hammer Smash, Side View 01") },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xCE9C, BossFrameType.Sprite, "Hammer Smash, Side View 02") },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xCEE0, BossFrameType.Sprite, "Hammer Smash, Side View 03") },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xCF24, BossFrameType.Sprite, "Hammer Smash, Side View 04") },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xCF68, BossFrameType.Sprite, "Walk, Side View 01") },
                { 0x14, new BossFrameMetadata(Constants.Bank07Byte, 0xCFAA, BossFrameType.Sprite, "Walk, Side View 02") },
                { 0x15, new BossFrameMetadata(Constants.Bank07Byte, 0xCFEC, BossFrameType.Sprite, "Idle, Back View") },
                { 0x16, new BossFrameMetadata(Constants.Bank07Byte, 0xD032, BossFrameType.Sprite, "Hammer Smash, Back View 01") },
                { 0x17, new BossFrameMetadata(Constants.Bank07Byte, 0xD078, BossFrameType.Sprite, "Hammer Smash, Back View 02") },
                { 0x18, new BossFrameMetadata(Constants.Bank07Byte, 0xD0C0, BossFrameType.Sprite, "Hammer Smash, Back View 03") },
                { 0x19, new BossFrameMetadata(Constants.Bank07Byte, 0xD108, BossFrameType.Sprite, "Hammer Swing, Back View 01") },
                { 0x1A, new BossFrameMetadata(Constants.Bank07Byte, 0xD150, BossFrameType.Sprite, "Hammer Swing, Back View 02") },
                { 0x1B, new BossFrameMetadata(Constants.Bank07Byte, 0xD198, BossFrameType.Sprite, "Walk, Back View 01") },
                { 0x1C, new BossFrameMetadata(Constants.Bank07Byte, 0xD1DE, BossFrameType.Sprite, "Walk, Back View 02") },
                //{ 0x1D, new BossFrameMetadata(Constants.Bank07Byte, 0xD224, BossFrameType.Sprite, "Legs, Idle") },
                //{ 0x1E, new BossFrameMetadata(Constants.Bank07Byte, 0xD22E, BossFrameType.Sprite, "Legs, Walk, Down View") },
                //{ 0x1F, new BossFrameMetadata(Constants.Bank07Byte, 0xD238, BossFrameType.Sprite, "Legs, Idle Side View") },
                //{ 0x20, new BossFrameMetadata(Constants.Bank07Byte, 0xD242, BossFrameType.Sprite, "Legs, Walk, Side View 01") },
                //{ 0x21, new BossFrameMetadata(Constants.Bank07Byte, 0xD24C, BossFrameType.Sprite, "Legs, Walk, Side View 02") },
                //{ 0x22, new BossFrameMetadata(Constants.Bank07Byte, 0xD256, BossFrameType.Sprite, "Legs, Idle Back View") },
                //{ 0x23, new BossFrameMetadata(Constants.Bank07Byte, 0xD260, BossFrameType.Sprite, "Legs, Walk, Back View") },
                //{ 0x24, new BossFrameMetadata(Constants.Bank07Byte, 0xD26A, BossFrameType.Sprite, "Wheel, Turn 01") },
                //{ 0x25, new BossFrameMetadata(Constants.Bank07Byte, 0xD270, BossFrameType.Sprite, "Wheel, Idle") },
                //{ 0x26, new BossFrameMetadata(Constants.Bank07Byte, 0xD276, BossFrameType.Sprite, "Wheel, Turn 02") },
                //{ 0x27, new BossFrameMetadata(Constants.Bank07Byte, 0xD27C, BossFrameType.Sprite, "Wheel, Diagonal (Dummied Out)") },
                //{ 0x28, new BossFrameMetadata(Constants.Bank07Byte, 0xD282, BossFrameType.Sprite, "Wheel, Turn 03") },
                //{ 0x29, new BossFrameMetadata(Constants.Bank07Byte, 0xD288, BossFrameType.Sprite, "Wheel, Idle Side View") },
                //{ 0x2A, new BossFrameMetadata(Constants.Bank07Byte, 0xD28E, BossFrameType.Sprite, "Drill, Idle (Dummied Out)") },
                //{ 0x2B, new BossFrameMetadata(Constants.Bank07Byte, 0xD29C, BossFrameType.Sprite, "Drill, In Ground 01 (Dummied Out)") },
                //{ 0x2C, new BossFrameMetadata(Constants.Bank07Byte, 0xD2AE, BossFrameType.Sprite, "Drill, In Ground 02 (Dummied Out)") },
             };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> RobotAuxiliaryHammerFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xD224, BossFrameType.Sprite, "Legs, Idle", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xD22E, BossFrameType.Sprite, "Legs, Walk, Down View", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xD238, BossFrameType.Sprite, "Legs, Idle Side View", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xD242, BossFrameType.Sprite, "Legs, Walk, Side View 01", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xD24C, BossFrameType.Sprite, "Legs, Walk, Side View 02", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xD256, BossFrameType.Sprite, "Legs, Idle Back View", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xD260, BossFrameType.Sprite, "Legs, Walk, Back View", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xD26A, BossFrameType.Sprite, "Wheel, Turn 01", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xD270, BossFrameType.Sprite, "Wheel, Idle", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xD276, BossFrameType.Sprite, "Wheel, Turn 02", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xD27C, BossFrameType.Sprite, "Wheel, Diagonal (Dummied Out)", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xD282, BossFrameType.Sprite, "Wheel, Turn 03", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xD288, BossFrameType.Sprite, "Wheel, Idle Side View", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> RobotAuxiliaryChainsawFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xD224, BossFrameType.Sprite, "Legs, Idle", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xD22E, BossFrameType.Sprite, "Legs, Walk, Down View", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xD238, BossFrameType.Sprite, "Legs, Idle Side View", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xD242, BossFrameType.Sprite, "Legs, Walk, Side View 01", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xD24C, BossFrameType.Sprite, "Legs, Walk, Side View 02", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xD256, BossFrameType.Sprite, "Legs, Idle Back View", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xD260, BossFrameType.Sprite, "Legs, Walk, Back View", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xD28E, BossFrameType.Sprite, "Drill, Idle (Dummied Out)", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xD29C, BossFrameType.Sprite, "Drill, In Ground 01 (Dummied Out)", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xD2AE, BossFrameType.Sprite, "Drill, In Ground 02 (Dummied Out)", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> HydraFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xC8DA, BossFrameType.Sprite, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xC90C, BossFrameType.Sprite, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xC93E, BossFrameType.Sprite, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xC970, BossFrameType.Sprite, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xC9A2, BossFrameType.Sprite, string.Empty, false) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xC9A8, BossFrameType.Sprite, string.Empty, false) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xC9AE, BossFrameType.Sprite, string.Empty, false) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xC9B4, BossFrameType.Sprite, string.Empty, false) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xC9BA, BossFrameType.Sprite, string.Empty, false) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xC9C0, BossFrameType.Sprite, string.Empty, false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xC9C6, BossFrameType.Sprite, string.Empty, false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xC9CC, BossFrameType.Sprite, string.Empty, false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xC9E6, BossFrameType.Sprite, string.Empty, false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xCA00, BossFrameType.Sprite, string.Empty, false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xCA1A, BossFrameType.Sprite, string.Empty, false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> AntFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xC500, BossFrameType.Sprite, "Idle 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xC53A, BossFrameType.Sprite, "Idle 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xC574, BossFrameType.Sprite, "Head Turn (Broken)") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xC5B2, BossFrameType.Sprite, "Skill Use") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xC5E8, BossFrameType.Sprite, "Walk, Side View") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xC622, BossFrameType.Sprite, "Walk, Down View") },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xC65C, BossFrameType.Sprite, "Double Kama Swing 01") },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xC6A4, BossFrameType.Sprite, "Double Kama Swing 02") },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xC6EC, BossFrameType.Sprite, "Double Kama Swing 03") },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xC736, BossFrameType.Sprite, "Double Kama Swing 04") },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xC78C, BossFrameType.Sprite, "Kama Dance") },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xC7D0, BossFrameType.Sprite, "Jump") },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xC822, BossFrameType.Sprite, "Guard Pose") },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xC874, BossFrameType.Sprite, "Kama Projectile 01") },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xC886, BossFrameType.Sprite, "Kama Projectile 02") },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xC898, BossFrameType.Sprite, "Kama Projectile 03") },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xC8AA, BossFrameType.Sprite, "Elliot Scared 01", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xC8B4, BossFrameType.Sprite, "Elliot Scared 02", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xC8BE, BossFrameType.Sprite, "Elliot Point 01", false) },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xC8CC, BossFrameType.Sprite, "Elliot Point 02", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> MinotaurFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xD2B8, BossFrameType.Sprite, "Idle") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xD2EE, BossFrameType.Sprite, "Walk, Down View") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xD324, BossFrameType.Sprite, "Extending Horns, Down View 01") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xD35A, BossFrameType.Sprite, "Extending Horns, Down View 02") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xD390, BossFrameType.Sprite, "Extending Horns, Down View 04") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xD3D0, BossFrameType.Sprite, "Extending Horns, Down View 03") },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xD414, BossFrameType.Sprite, "Punch, Down View 01") },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xD44A, BossFrameType.Sprite, "Punch, Down View 02") },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xD486, BossFrameType.Sprite, "Arm Claw, Down View 02") },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xD4CA, BossFrameType.Sprite, "Arm Claw, Down View 01") },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xD510, BossFrameType.Sprite, "Arm Claw Rush, Down View 01") },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xD550, BossFrameType.Sprite, "Arm Claw Rush, Down View 02") },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xD594, BossFrameType.Sprite, "Skill Pose") },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xD5CA, BossFrameType.Sprite, "Guard Pose") },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xD60A, BossFrameType.Sprite, "Idle, Back View") },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xD640, BossFrameType.Sprite, "Walk, Back View") },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xD676, BossFrameType.Sprite, "Extending Horns, Back View 01") },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xD6AC, BossFrameType.Sprite, "Extending Horns, Back View 02") },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xD6E4, BossFrameType.Sprite, "Extending Horns, Back View 04") },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xD722, BossFrameType.Sprite, "Extending Horns, Back View 03") },
                { 0x14, new BossFrameMetadata(Constants.Bank07Byte, 0xD764, BossFrameType.Sprite, "Punch, Back View") },
                { 0x15, new BossFrameMetadata(Constants.Bank07Byte, 0xD7A2, BossFrameType.Sprite, "Arm Claw, Back View 02") },
                { 0x16, new BossFrameMetadata(Constants.Bank07Byte, 0xD7E8, BossFrameType.Sprite, "Arm Claw, Back View 01") },
                { 0x17, new BossFrameMetadata(Constants.Bank07Byte, 0xD82E, BossFrameType.Sprite, "Arm Claw Rush, Back View 01") },
                { 0x18, new BossFrameMetadata(Constants.Bank07Byte, 0xD86E, BossFrameType.Sprite, "Arm Claw Rush, Back View 02") },
                { 0x19, new BossFrameMetadata(Constants.Bank07Byte, 0xD8B4, BossFrameType.Sprite, "Guard Pose, Back View") },
                { 0x1A, new BossFrameMetadata(Constants.Bank07Byte, 0xD8F0, BossFrameType.Sprite, "Idle, Side View") },
                { 0x1B, new BossFrameMetadata(Constants.Bank07Byte, 0xD922, BossFrameType.Sprite, "Walk, Side View 01") },
                { 0x1C, new BossFrameMetadata(Constants.Bank07Byte, 0xD954, BossFrameType.Sprite, "Walk, Side View 02") },
                { 0x1D, new BossFrameMetadata(Constants.Bank07Byte, 0xD986, BossFrameType.Sprite, "Horn Attack, Side View 01") },
                { 0x1E, new BossFrameMetadata(Constants.Bank07Byte, 0xD9B8, BossFrameType.Sprite, "Horn Attack, Side View 02") },
                { 0x1F, new BossFrameMetadata(Constants.Bank07Byte, 0xD9EE, BossFrameType.Sprite, "Horn Attack, Side View 03") },
                { 0x20, new BossFrameMetadata(Constants.Bank07Byte, 0xDA28, BossFrameType.Sprite, "Punch, Side View 01") },
                { 0x21, new BossFrameMetadata(Constants.Bank07Byte, 0xDA60, BossFrameType.Sprite, "Punch, Side View 02") },
                { 0x22, new BossFrameMetadata(Constants.Bank07Byte, 0xDA96, BossFrameType.Sprite, "Punch, Side View 03") },
                { 0x23, new BossFrameMetadata(Constants.Bank07Byte, 0xDAD0, BossFrameType.Sprite, "Arm Claw, Side View 01") },
                { 0x24, new BossFrameMetadata(Constants.Bank07Byte, 0xDB12, BossFrameType.Sprite, "Arm Claw, Side View 02") },
                { 0x25, new BossFrameMetadata(Constants.Bank07Byte, 0xDB50, BossFrameType.Sprite, "Arm Claw Rush, Side View") },
                { 0x26, new BossFrameMetadata(Constants.Bank07Byte, 0xDB92, BossFrameType.Sprite, "Guard Pose, Side View") },
                { 0x27, new BossFrameMetadata(Constants.Bank07Byte, 0xDBCC, BossFrameType.Sprite, "Horn Charge, Down View 01") },
                { 0x28, new BossFrameMetadata(Constants.Bank07Byte, 0xDC0E, BossFrameType.Sprite, "Horn Charge, Down View 02") },
                { 0x29, new BossFrameMetadata(Constants.Bank07Byte, 0xDC50, BossFrameType.Sprite, "Horn Charge, Back View 01") },
                { 0x2A, new BossFrameMetadata(Constants.Bank07Byte, 0xDC8E, BossFrameType.Sprite, "Horn Charge, Back View 02") },
                { 0x2B, new BossFrameMetadata(Constants.Bank07Byte, 0xDCCC, BossFrameType.Sprite, "Horn Charge, Side View 01") },
                { 0x2C, new BossFrameMetadata(Constants.Bank07Byte, 0xDD06, BossFrameType.Sprite, "Horn Charge, Side View 02") },
                { 0x2D, new BossFrameMetadata(Constants.Bank07Byte, 0xDD40, BossFrameType.Sprite, "Horn Charge, Side View 03") },
                { 0x2E, new BossFrameMetadata(Constants.Bank07Byte, 0xDD7A, BossFrameType.Sprite, "Spell Cast") },
                { 0x2F, new BossFrameMetadata(Constants.Bank07Byte, 0xDDBC, BossFrameType.Sprite, "Jump Attack 02") },
                { 0x30, new BossFrameMetadata(Constants.Bank07Byte, 0xDDF4, BossFrameType.Sprite, "Jump Attack 01") },
                { 0x31, new BossFrameMetadata(Constants.Bank07Byte, 0xDE36, BossFrameType.Sprite, "Arm Blade Show Off 02") },
                { 0x32, new BossFrameMetadata(Constants.Bank07Byte, 0xDE92, BossFrameType.Sprite, "Arm Blade Show Off 01") },
                { 0x33, new BossFrameMetadata(Constants.Bank07Byte, 0xDEEE, BossFrameType.Sprite, "Arm Blade Show Off 03") },
                { 0x34, new BossFrameMetadata(Constants.Bank07Byte, 0xDF42, BossFrameType.Sprite, "Death Pose") },
                { 0x35, new BossFrameMetadata(Constants.Bank07Byte, 0xDF72, BossFrameType.Sprite, "Power Bomb 01") },
                { 0x36, new BossFrameMetadata(Constants.Bank07Byte, 0xDFA4, BossFrameType.Sprite, "Power Bomb 02") },
                { 0x37, new BossFrameMetadata(Constants.Bank07Byte, 0xDFD6, BossFrameType.Sprite, "Power Bomb 03") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> AegagropilonFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xE348, BossFrameType.Sprite, string.Empty, false) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xE362, BossFrameType.Sprite, string.Empty, false) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xE37C, BossFrameType.Sprite, string.Empty, false) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xE396, BossFrameType.Sprite, string.Empty, false) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xE3B0, BossFrameType.Sprite, string.Empty, false) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xE3CA, BossFrameType.Sprite, string.Empty, false) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xE3E4, BossFrameType.Sprite, string.Empty, false) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xE3FE, BossFrameType.Sprite, string.Empty, false) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xE418, BossFrameType.Sprite, string.Empty, false) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xE432, BossFrameType.Sprite, string.Empty, false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xE44C, BossFrameType.Sprite, string.Empty, false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xE466, BossFrameType.Sprite, string.Empty, false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xE480, BossFrameType.Sprite, string.Empty, false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xE49A, BossFrameType.Sprite, string.Empty, false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xE4B4, BossFrameType.Sprite, string.Empty, false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xE4CE, BossFrameType.Sprite, string.Empty, false) },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xE4E8, BossFrameType.Sprite, string.Empty, false) },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xE502, BossFrameType.Sprite, string.Empty, false) },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xE51C, BossFrameType.Sprite, string.Empty, false) },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xE536, BossFrameType.Sprite, string.Empty, false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> AegagropilonBodyFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xE178, BossFrameType.Background, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xE18C, BossFrameType.Background, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xE1A2, BossFrameType.Background, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xE1B6, BossFrameType.Background, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xE1CA, BossFrameType.Background, string.Empty) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xE1DE, BossFrameType.Background, string.Empty) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xE1F2, BossFrameType.Background, string.Empty) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xE206, BossFrameType.Background, string.Empty) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xE21A, BossFrameType.Background, string.Empty) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xE22E, BossFrameType.Background, string.Empty) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xE242, BossFrameType.Background, string.Empty) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xE256, BossFrameType.Background, string.Empty) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xE26C, BossFrameType.Background, string.Empty) },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xE282, BossFrameType.Background, string.Empty) },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xE298, BossFrameType.Background, string.Empty) },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xE2AE, BossFrameType.Background, string.Empty) },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xE2C4, BossFrameType.Background, string.Empty) },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xE2DA, BossFrameType.Background, string.Empty) },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xE2F0, BossFrameType.Background, string.Empty) },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xE306, BossFrameType.Background, string.Empty) },
                { 0x14, new BossFrameMetadata(Constants.Bank07Byte, 0xE31C, BossFrameType.Background, string.Empty) },
                { 0x15, new BossFrameMetadata(Constants.Bank07Byte, 0xE332, BossFrameType.Background, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> TigerFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xE550, BossFrameType.Sprite, "Idle 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xE59E, BossFrameType.Sprite, "Idle 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xE5EC, BossFrameType.Sprite, "Idle 03") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xE63A, BossFrameType.Sprite, "Damaged") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xE686, BossFrameType.Sprite, "Skill Use 01") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xE6DC, BossFrameType.Sprite, "Skill Use 02") },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xE732, BossFrameType.Sprite, "Skill Use 03") },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xE780, BossFrameType.Sprite, "Ball Curl Windup 01") },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xE7CE, BossFrameType.Sprite, "Ball Curl Windup 02") },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xE824, BossFrameType.Sprite, "Feign Death 01") },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xE85C, BossFrameType.Sprite, "Feign Death 02") },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xE894, BossFrameType.Sprite, "Feign Death 03") },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xE8CC, BossFrameType.Sprite, "Head Regrowth 01") },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xE90C, BossFrameType.Sprite, "Head Regrowth 02") },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xE954, BossFrameType.Sprite, "Spike Ball 01") },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xE978, BossFrameType.Sprite, "Spike Ball 02") },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xE99C, BossFrameType.Sprite, "Idle, Look Left 01") },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xE9EA, BossFrameType.Sprite, "Idle, Look Left 02") },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xEA38, BossFrameType.Sprite, "Idle, Look Left 03") },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xEA86, BossFrameType.Sprite, "????? 01") },
                { 0x14, new BossFrameMetadata(Constants.Bank07Byte, 0xEAD4, BossFrameType.Sprite, "Walk Left 01") },
                { 0x15, new BossFrameMetadata(Constants.Bank07Byte, 0xEB22, BossFrameType.Sprite, "Walk Left 02") },
                { 0x16, new BossFrameMetadata(Constants.Bank07Byte, 0xEB70, BossFrameType.Sprite, "Walk Left 03") },
                { 0x17, new BossFrameMetadata(Constants.Bank07Byte, 0xEBBE, BossFrameType.Sprite, "Walk Right_03") },
                { 0x18, new BossFrameMetadata(Constants.Bank07Byte, 0xEC0C, BossFrameType.Sprite, "????? 02") },
                { 0x19, new BossFrameMetadata(Constants.Bank07Byte, 0xEC5A, BossFrameType.Sprite, "Walk Right 02") },
                { 0x1A, new BossFrameMetadata(Constants.Bank07Byte, 0xECA8, BossFrameType.Sprite, "Walk Right 01") },
                { 0x1B, new BossFrameMetadata(Constants.Bank07Byte, 0xECF6, BossFrameType.Sprite, "Maw 03") },
                { 0x1C, new BossFrameMetadata(Constants.Bank07Byte, 0xED48, BossFrameType.Sprite, "Maw 04") },
                { 0x1D, new BossFrameMetadata(Constants.Bank07Byte, 0xED92, BossFrameType.Sprite, "Maw 01") },
                { 0x1E, new BossFrameMetadata(Constants.Bank07Byte, 0xEDE4, BossFrameType.Sprite, "Maw 02") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> LizardFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xF1F2, BossFrameType.Sprite, "Egg, Idle") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xF200, BossFrameType.Sprite, "Egg, Hatch 01") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xF20C, BossFrameType.Sprite, "Egg, Hatch 02") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xF218, BossFrameType.Sprite, "Egg, Hatch 03") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xF230, BossFrameType.Sprite, "Egg, Hatch 04") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xF248, BossFrameType.Sprite, "Egg, Hatch 05") },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xF258, BossFrameType.Sprite, "Tonpole, Hop 01") },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xF26A, BossFrameType.Sprite, "Tonpole, Hop 02") },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xF27A, BossFrameType.Sprite, "Tonpole, Attack 01") },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xF28E, BossFrameType.Sprite, "Tonpole, Attack 02") },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xF2A2, BossFrameType.Sprite, "Tonpole, Damaged") },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xF2B6, BossFrameType.Sprite, "Tonpole, Transform") },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xF2C6, BossFrameType.Sprite, "Tonpole, Explode 01") },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xF2E2, BossFrameType.Sprite, "Tonpole, Explode 02") },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xF306, BossFrameType.Sprite, "Tonpole, Explode 03") },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xF32A, BossFrameType.Sprite, "Tonpole, Explode 04") },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xF342, BossFrameType.Sprite, "Tonpole, Explode 05") },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xF35A, BossFrameType.Sprite, "Lizard, Idle Side View 01") },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xF374, BossFrameType.Sprite, "Lizard, Idle Side View 02") },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xF38E, BossFrameType.Sprite, "Lizard, Walk Side View") },
                { 0x14, new BossFrameMetadata(Constants.Bank07Byte, 0xF3A8, BossFrameType.Sprite, "Lizard, Devour Side View 01") },
                { 0x15, new BossFrameMetadata(Constants.Bank07Byte, 0xF3C6, BossFrameType.Sprite, "Lizard, Devour Side View 02") },
                { 0x16, new BossFrameMetadata(Constants.Bank07Byte, 0xF3E4, BossFrameType.Sprite, "Lizard, Devour Side View 03") },
                { 0x17, new BossFrameMetadata(Constants.Bank07Byte, 0xF406, BossFrameType.Sprite, "Lizard, Devour Side View 04") },
                { 0x18, new BossFrameMetadata(Constants.Bank07Byte, 0xF42C, BossFrameType.Sprite, "Lizard, Devour Side View 05") },
                { 0x19, new BossFrameMetadata(Constants.Bank07Byte, 0xF452, BossFrameType.Sprite, "Lizard, Swallow Side View 01") },
                { 0x1A, new BossFrameMetadata(Constants.Bank07Byte, 0xF46C, BossFrameType.Sprite, "Lizard, Swallow Side View 02") },
                { 0x1B, new BossFrameMetadata(Constants.Bank07Byte, 0xF486, BossFrameType.Sprite, "Lizard, Full Side View 01") },
                { 0x1C, new BossFrameMetadata(Constants.Bank07Byte, 0xF4A0, BossFrameType.Sprite, "Lizard, Full Side View 02") },
                { 0x1D, new BossFrameMetadata(Constants.Bank07Byte, 0xF4BA, BossFrameType.Sprite, "Lizard, Throw Up") },
                { 0x1E, new BossFrameMetadata(Constants.Bank07Byte, 0xF4D2, BossFrameType.Sprite, "Lizard, Walk Down View") },
                { 0x1F, new BossFrameMetadata(Constants.Bank07Byte, 0xF4E8, BossFrameType.Sprite, "Lizard, Idle Down View") },
                { 0x20, new BossFrameMetadata(Constants.Bank07Byte, 0xF4FE, BossFrameType.Sprite, "Lizard, Devour Down View 01") },
                { 0x21, new BossFrameMetadata(Constants.Bank07Byte, 0xF518, BossFrameType.Sprite, "Lizard, Devour Down View 02") },
                { 0x22, new BossFrameMetadata(Constants.Bank07Byte, 0xF536, BossFrameType.Sprite, "Lizard, Devour Down View 03") },
                { 0x23, new BossFrameMetadata(Constants.Bank07Byte, 0xF558, BossFrameType.Sprite, "Lizard, Devour Down View 04") },
                { 0x24, new BossFrameMetadata(Constants.Bank07Byte, 0xF57E, BossFrameType.Sprite, "Lizard, Devour Down View 05") },
                { 0x25, new BossFrameMetadata(Constants.Bank07Byte, 0xF5A4, BossFrameType.Sprite, "Lizard, Swallow Down View 01") },
                { 0x26, new BossFrameMetadata(Constants.Bank07Byte, 0xF5BE, BossFrameType.Sprite, "Lizard, Swallow Down View 02") },
                { 0x27, new BossFrameMetadata(Constants.Bank07Byte, 0xF5D8, BossFrameType.Sprite, "Lizard, Damaged") },
                { 0x28, new BossFrameMetadata(Constants.Bank07Byte, 0xF5EC, BossFrameType.Sprite, "Lizard, Dead") },
                { 0x29, new BossFrameMetadata(Constants.Bank07Byte, 0xF5FC, BossFrameType.Sprite, "Lizard, Walk Back View") },
                { 0x2A, new BossFrameMetadata(Constants.Bank07Byte, 0xF612, BossFrameType.Sprite, "Lizard, Idle Back View") },
                { 0x2B, new BossFrameMetadata(Constants.Bank07Byte, 0xF628, BossFrameType.Sprite, "Lizard, Devour Back View 01") },
                { 0x2C, new BossFrameMetadata(Constants.Bank07Byte, 0xF63A, BossFrameType.Sprite, "Lizard, Devour Back View 02") },
                { 0x2D, new BossFrameMetadata(Constants.Bank07Byte, 0xF650, BossFrameType.Sprite, "Lizard, Devour Back View 03") },
                { 0x2E, new BossFrameMetadata(Constants.Bank07Byte, 0xF66A, BossFrameType.Sprite, "Lizard, Devour Back View 04") },
                { 0x2F, new BossFrameMetadata(Constants.Bank07Byte, 0xF688, BossFrameType.Sprite, "Lizard, Devour Back View 05") },
                { 0x30, new BossFrameMetadata(Constants.Bank07Byte, 0xF6A6, BossFrameType.Sprite, "Lizard, Swallow Back View 01") },
                { 0x31, new BossFrameMetadata(Constants.Bank07Byte, 0xF6C0, BossFrameType.Sprite, "Lizard, Swallow Back View 02") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> GigasFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                 { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xBE8C, BossFrameType.Sprite, "Idle 01") },
                 { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xBECE, BossFrameType.Sprite, "Idle 02") },
                 { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xBF10, BossFrameType.Sprite, "Walk, Side View") },
                 { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xBF52, BossFrameType.Sprite, "Face Down") },
                 { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xBF94, BossFrameType.Sprite, "Claw Attack 01") },
                 { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xBFD6, BossFrameType.Sprite, "Disassemble 01") },
                 { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xC028, BossFrameType.Sprite, "Claw Attack 02") },
                 { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xC068, BossFrameType.Sprite, "Claw Attack 03") },
                 { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xC0A8, BossFrameType.Sprite, "Claw Attack 04") },
                 { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xC0F8, BossFrameType.Sprite, "Claw Attack 05") },
                 { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xC148, BossFrameType.Sprite, "Disassemble 02") },
                 { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xC192, BossFrameType.Sprite, "Disassemble 03") },
                 { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xC1C2, BossFrameType.Sprite, "Disassemble 04") },
                 { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xC1E2, BossFrameType.Sprite, "Reassemble 01") },
                 { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xC202, BossFrameType.Sprite, "Damaged") },
                 { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xC24A, BossFrameType.Sprite, "Skill Use, Orb Crush 01") },
                 { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xC2A4, BossFrameType.Sprite, "Skill Use, Orb Crush 02") },
                 { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xC2FE, BossFrameType.Sprite, "Skill Use, Orb Crush 03") },
                 { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xC348, BossFrameType.Sprite, "Skill Use, Wind Blow 01") },
                 { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xC392, BossFrameType.Sprite, "Skill Use, Wind Blow 02") },
                 { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xC3DC, BossFrameType.Sprite, "Skill Use, Wind Blow, Alternate 01 (Dummied Out)") },
                 { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xC42E, BossFrameType.Sprite, "Skill Use, Wind Blow, Alternate 02 (Dummied Out)") },
                 { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xC498, BossFrameType.Sprite, "Skill Use, Wind Blow 03") },
                 { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xC4DA, BossFrameType.Sprite, "Teleport Orb 01", false) },
                 { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xC4E0, BossFrameType.Sprite, "Teleport Orb 02", false) },
                 { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xC4E6, BossFrameType.Sprite, "Teleport Orb 03", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> GigasAuxiliaryDiamondFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                 { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xC1E2, BossFrameType.Sprite, "Reassemble 01") },
                 { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xC4DA, BossFrameType.Sprite, "Teleport Orb 01", false) },
                 { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xC4E0, BossFrameType.Sprite, "Teleport Orb 02", false) },
                 { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xC4E6, BossFrameType.Sprite, "Teleport Orb 03", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> GigasAuxiliaryOrbFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                 { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xC1E2, BossFrameType.Sprite, "Reassemble 01") },
                 { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xC4DA, BossFrameType.Sprite, "Teleport Orb 01", false) },
                 { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xC4E0, BossFrameType.Sprite, "Teleport Orb 02", false) },
                 { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xC4E6, BossFrameType.Sprite, "Teleport Orb 03", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> BirdFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xF7D2, BossFrameType.Sprite, "Idle") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xF7F4, BossFrameType.Sprite, "Hair Flare Up") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xF812, BossFrameType.Sprite, "Skill Use") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xF834, BossFrameType.Sprite, "Damaged") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xF852, BossFrameType.Sprite, "Spell Cast") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xF874, BossFrameType.Sprite, "Leg Ball Base (Dummied Out)", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xF87A, BossFrameType.Sprite, "Leg Ball (Dummied Out)", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xF880, BossFrameType.Sprite, "Foot Straight (Dummied Out)", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xF886, BossFrameType.Sprite, "Foot Angled Forward (Dummied Out)", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xF88C, BossFrameType.Sprite, "Foot Angled Back (Dummied Out)", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xF892, BossFrameType.Sprite, "Foot Bottom (Dummied Out)", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xF898, BossFrameType.Sprite, "Foot Vertical (Dummied Out)", false) }, // Does have partial hitbox data
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xF8A0, BossFrameType.Sprite, "Full Body Idle 03 (Dummied Out)") },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xF8F2, BossFrameType.Sprite, "Full Body Idle 02 (Dummied Out)") },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xF948, BossFrameType.Sprite, "Full Body Idle 01 (Dummied Out)") },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xF99E, BossFrameType.Sprite, "Leg Retracted", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xF9BC, BossFrameType.Sprite, "Leg Extending 01", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xF9DA, BossFrameType.Sprite, "Leg Extending 02", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xF9F8, BossFrameType.Sprite, "Leg Extended", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> MechRiderFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xDFC8, BossFrameType.Sprite, "Idle 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xE016, BossFrameType.Sprite, "Idle 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xE06C, BossFrameType.Sprite, "Horizontal Movement 01") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xE0BC, BossFrameType.Sprite, "Horizontal Movement 02") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xE10C, BossFrameType.Sprite, "Bike Drift 01") },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xE15E, BossFrameType.Sprite, "Bike Drift 02") },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xE1B0, BossFrameType.Sprite, "Bike Drift 03") },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xE202, BossFrameType.Sprite, "Bike Drift 04") },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xE254, BossFrameType.Sprite, "Bike Drift 05") },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xE2A6, BossFrameType.Sprite, "Bike Drift 06") },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xE2F8, BossFrameType.Sprite, "Bike Drift 07") },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xE34A, BossFrameType.Sprite, "Bike Drift 08") },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xE39C, BossFrameType.Sprite, "Bike Drift 09") },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xE3EE, BossFrameType.Sprite, "Idle, Down View 01") },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xE440, BossFrameType.Sprite, "Idle, Down View 02") },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xE48E, BossFrameType.Sprite, "Spell/Skill Use 01") },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xE4E0, BossFrameType.Sprite, "Spell/Skill Use 02") },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xE532, BossFrameType.Sprite, "Spell/Skill Use 03") },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xE584, BossFrameType.Sprite, "Spell/Skill Use 04") },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xE5D6, BossFrameType.Sprite, "Spell/Skill Use 05") },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xE628, BossFrameType.Sprite, "Spell/Skill Use 06") },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xE67A, BossFrameType.Sprite, "Spell/Skill Use 07") },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xE6CC, BossFrameType.Sprite, "Spell/Skill Use 08") },
                { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xE71E, BossFrameType.Sprite, "Spell/Skill Use 09") },
                { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xE770, BossFrameType.Sprite, "Spell/Skill Use 0A") },
                { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xE7C2, BossFrameType.Sprite, "Open Missile Pod 01") },
                { 0x1A, new BossFrameMetadata(Constants.Bank0AByte, 0xE810, BossFrameType.Sprite, "Open Missile Pod 02") },
                { 0x1B, new BossFrameMetadata(Constants.Bank0AByte, 0xE85E, BossFrameType.Sprite, "Open Missile Pod 03") },
                { 0x1C, new BossFrameMetadata(Constants.Bank0AByte, 0xE8AC, BossFrameType.Sprite, "Open Missile Pod 04") },
                { 0x1D, new BossFrameMetadata(Constants.Bank0AByte, 0xE8FA, BossFrameType.Sprite, "Close Missile Pod 01") },
                { 0x1E, new BossFrameMetadata(Constants.Bank0AByte, 0xE948, BossFrameType.Sprite, "Close Missile Pod 02") },
                { 0x1F, new BossFrameMetadata(Constants.Bank0AByte, 0xE996, BossFrameType.Sprite, "Close Missile Pod 03") },
                { 0x20, new BossFrameMetadata(Constants.Bank0AByte, 0xE9E4, BossFrameType.Sprite, "Close Missile Pod 04") },
                { 0x21, new BossFrameMetadata(Constants.Bank0AByte, 0xEA32, BossFrameType.Sprite, "Vertical Missile") },
                { 0x22, new BossFrameMetadata(Constants.Bank0AByte, 0xEA40, BossFrameType.Sprite, "Angled Missile") },
                { 0x23, new BossFrameMetadata(Constants.Bank0AByte, 0xEA4E, BossFrameType.Sprite, "Horizontal Missile") },
                { 0x24, new BossFrameMetadata(Constants.Bank0AByte, 0xEA5C, BossFrameType.Sprite, "Damaged") },
                { 0x25, new BossFrameMetadata(Constants.Bank0AByte, 0xEAA8, BossFrameType.Sprite, "Bike Gas Clouds 01", false) },
                { 0x26, new BossFrameMetadata(Constants.Bank0AByte, 0xEAB2, BossFrameType.Sprite, "Bike Gas Clouds 02", false) },
                { 0x27, new BossFrameMetadata(Constants.Bank0AByte, 0xEABC, BossFrameType.Sprite, "Bike Gas Clouds 03", false) },
                { 0x28, new BossFrameMetadata(Constants.Bank0AByte, 0xEACA, BossFrameType.Sprite, "Bike Gas Clouds 04", false) },
                { 0x29, new BossFrameMetadata(Constants.Bank0AByte, 0xEAD8, BossFrameType.Sprite, "Bike Gas Clouds 05", false) },
                { 0x2A, new BossFrameMetadata(Constants.Bank0AByte, 0xEAE2, BossFrameType.Sprite, "Bike Gas Clouds 06", false) },
                { 0x2B, new BossFrameMetadata(Constants.Bank0AByte, 0xEAEC, BossFrameType.Sprite, "Bike Gas Clouds 07", false) },
                { 0x2C, new BossFrameMetadata(Constants.Bank0AByte, 0xEAF2, BossFrameType.Sprite, "Bike Gas Clouds 08", false) },
                { 0x2D, new BossFrameMetadata(Constants.Bank0AByte, 0xEAF8, BossFrameType.Sprite, "Fast Bike Gas Clouds 01", false) },
                { 0x2E, new BossFrameMetadata(Constants.Bank0AByte, 0xEAFE, BossFrameType.Sprite, "Fast Bike Gas Clouds 02", false) },
                { 0x2F, new BossFrameMetadata(Constants.Bank0AByte, 0xEB04, BossFrameType.Sprite, "Fast Bike Gas Clouds 03", false) },
                { 0x30, new BossFrameMetadata(Constants.Bank0AByte, 0xEB0E, BossFrameType.Sprite, "Fast Bike Gas Clouds 04", false) },
                { 0x31, new BossFrameMetadata(Constants.Bank0AByte, 0xEB18, BossFrameType.Sprite, "Fast Bike Gas Clouds 05", false) },
                { 0x32, new BossFrameMetadata(Constants.Bank0AByte, 0xEB22, BossFrameType.Sprite, "Fast Bike Gas Clouds 06", false) },
                { 0x33, new BossFrameMetadata(Constants.Bank0AByte, 0xEB2C, BossFrameType.Sprite, "Fast Bike Gas Clouds 07", false) },
                { 0x34, new BossFrameMetadata(Constants.Bank0AByte, 0xEB32, BossFrameType.Sprite, "Fast Bike Gas Clouds 08", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> SerpentFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xF6DA, BossFrameType.Sprite, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xF6EA, BossFrameType.Sprite, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xF6FA, BossFrameType.Sprite, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xF70E, BossFrameType.Sprite, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xF722, BossFrameType.Sprite, string.Empty) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xF736, BossFrameType.Sprite, string.Empty) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xF746, BossFrameType.Sprite, string.Empty) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xF756, BossFrameType.Sprite, string.Empty) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xF76A, BossFrameType.Sprite, string.Empty) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xF77E, BossFrameType.Sprite, string.Empty) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xF792, BossFrameType.Sprite, string.Empty, false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xF798, BossFrameType.Sprite, string.Empty, false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xF79E, BossFrameType.Sprite, string.Empty, false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xF7A4, BossFrameType.Sprite, string.Empty, false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xF7AA, BossFrameType.Sprite, string.Empty, false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xF7B0, BossFrameType.Sprite, string.Empty, false) },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xF7B6, BossFrameType.Sprite, string.Empty) },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xF7C4, BossFrameType.Sprite, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> VampireFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xC92C, BossFrameType.Sprite, "Idle, Ground") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xC96E, BossFrameType.Sprite, "Right Head Turn") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xC9B0, BossFrameType.Sprite, "Left Head Turn") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xC9F2, BossFrameType.Sprite, "Jump Landing") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xCA34, BossFrameType.Sprite, "Take Flight 01") },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xCA72, BossFrameType.Sprite, "Take Flight 02") },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xCAB8, BossFrameType.Sprite, "Take Flight 03") },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xCAFC, BossFrameType.Sprite, "Flight") },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xCB46, BossFrameType.Sprite, "Damaged") },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xCB86, BossFrameType.Sprite, "Claw Attack 01") },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xCBCE, BossFrameType.Sprite, "Claw Attack 02") },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xCC12, BossFrameType.Sprite, "Claw Attack 03") },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xCC48, BossFrameType.Sprite, "Blood Drain 01") },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xCC82, BossFrameType.Sprite, "Blood Drain 02") },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xCCBA, BossFrameType.Sprite, "Blood Drain 03") },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xCCF4, BossFrameType.Sprite, "Blood Drain Claws 01") },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xCD04, BossFrameType.Sprite, "Blood Drain Claws 02") },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xCD1C, BossFrameType.Sprite, "Smile, Cape Open") },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xCD60, BossFrameType.Sprite, "Foot Hop") },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xCDA4, BossFrameType.Sprite, "Snap Spell Cast 01") },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xCDEC, BossFrameType.Sprite, "Snap Spell Cast 02") },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xCE34, BossFrameType.Sprite, "Spell Summon 01") },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xCE40, BossFrameType.Sprite, "Spell Summon 02") },
                { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xCE58, BossFrameType.Sprite, "Spell Summon 03") },
                { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xCE70, BossFrameType.Sprite, "Spell Summon 04") },
                { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xCE88, BossFrameType.Sprite, "Spell Summon 05") },
                { 0x1A, new BossFrameMetadata(Constants.Bank0AByte, 0xCEA0, BossFrameType.Sprite, "Snap Spell Cast 03") },
                { 0x1B, new BossFrameMetadata(Constants.Bank0AByte, 0xCEE2, BossFrameType.Sprite, "Snap Spell Cast 04") },
                { 0x1C, new BossFrameMetadata(Constants.Bank0AByte, 0xCF24, BossFrameType.Sprite, "Idle, Blink") },
                { 0x1D, new BossFrameMetadata(Constants.Bank0AByte, 0xCF6E, BossFrameType.Sprite, "Mini-Bat Spell Cast 01 (Dummied Out)") },
                { 0x1E, new BossFrameMetadata(Constants.Bank0AByte, 0xCF92, BossFrameType.Sprite, "Mini-Bat Spell Cast 02 (Dummied Out)") },
                { 0x1F, new BossFrameMetadata(Constants.Bank0AByte, 0xCFB6, BossFrameType.Sprite, "Mini-Bat Spell Cast 03 (Dummied Out)") },
                { 0x20, new BossFrameMetadata(Constants.Bank0AByte, 0xCFDA, BossFrameType.Sprite, "Mini-Bat 01") },
                { 0x21, new BossFrameMetadata(Constants.Bank0AByte, 0xCFEE, BossFrameType.Sprite, "Mini-Bat 02") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> LichFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xD002, BossFrameType.Sprite, "Idle 02") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xD058, BossFrameType.Sprite, "Idle 01") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xD0AE, BossFrameType.Sprite, "Idle 03") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xD104, BossFrameType.Sprite, "Look, Side View 02") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xD162, BossFrameType.Sprite, "Look, Side View 01") },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xD1C0, BossFrameType.Sprite, "Look, Side View 03") },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xD21E, BossFrameType.Sprite, "Damaged 01") },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xD272, BossFrameType.Sprite, "Damaged 02") },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xD2C6, BossFrameType.Sprite, "Spell Cast: Evil Gate 01") },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xD324, BossFrameType.Sprite, "Spell Cast: Evil Gate 02") },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xD382, BossFrameType.Sprite, "Spell Cast: Evil Gate 03") },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xD3E0, BossFrameType.Sprite, "Spell Cast: Evil Gate 04") },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xD43E, BossFrameType.Sprite, "Spell Cast: Evil Gate 05") },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xD49C, BossFrameType.Sprite, "Spell Cast: Dispel Magic 01") },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xD4F2, BossFrameType.Sprite, "Spell Cast: Dispel Magic 02") },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xD548, BossFrameType.Sprite, "Spell Cast: Dispel Magic 03") },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xD59E, BossFrameType.Sprite, "Spell Cast: HP Absorb 01") },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xD5F4, BossFrameType.Sprite, "Spell Cast: Thunderbolt 02") },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xD64E, BossFrameType.Sprite, "Spell Cast: Thunderbolt 03") },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xD6A8, BossFrameType.Sprite, "Spell Cast: Thunderbolt 04") },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xD6FE, BossFrameType.Sprite, "Spell Cast: HP Absorb 05") },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xD754, BossFrameType.Sprite, "Spell Cast: HP Absorb 04") },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xD7B2, BossFrameType.Sprite, "Spell Cast: HP Absorb 03") },
                { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xD808, BossFrameType.Sprite, "Spell Cast: HP Absorb 02") },
                { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xD85E, BossFrameType.Sprite, "Spell Cast: Dark Force 03") },
                { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xD8B8, BossFrameType.Sprite, "Spell Cast: Dark Force 04") },
                { 0x1A, new BossFrameMetadata(Constants.Bank0AByte, 0xD912, BossFrameType.Sprite, "Spell Cast: Dark Force 01") },
                { 0x1B, new BossFrameMetadata(Constants.Bank0AByte, 0xD968, BossFrameType.Sprite, "Spell Cast: Dark Force 02") },
                { 0x1C, new BossFrameMetadata(Constants.Bank0AByte, 0xD9BE, BossFrameType.Sprite, "Spell Cast: Thunderbolt 01") },
                { 0x1D, new BossFrameMetadata(Constants.Bank0AByte, 0xDA14, BossFrameType.Sprite, "Skeletal Hands Idle 01") },
                { 0x1E, new BossFrameMetadata(Constants.Bank0AByte, 0xDA4C, BossFrameType.Sprite, "Skeletal Hands Idle 02") },
                { 0x1F, new BossFrameMetadata(Constants.Bank0AByte, 0xDA84, BossFrameType.Sprite, "Skeletal Hands Head Peak 01") },
                { 0x20, new BossFrameMetadata(Constants.Bank0AByte, 0xDAC4, BossFrameType.Sprite, "Skeletal Hands Head Peak 02") },
                { 0x21, new BossFrameMetadata(Constants.Bank0AByte, 0xDB04, BossFrameType.Sprite, "Skeletal Hands Head Exposed 01") },
                { 0x22, new BossFrameMetadata(Constants.Bank0AByte, 0xDB4E, BossFrameType.Sprite, "Skeletal Hands Head Exposed 02") },
                { 0x23, new BossFrameMetadata(Constants.Bank0AByte, 0xDB98, BossFrameType.Sprite, "Skeletal Hands Damaged 01") },
                { 0x24, new BossFrameMetadata(Constants.Bank0AByte, 0xDBD0, BossFrameType.Sprite, "Skeletal Hands Damaged 02") },
                { 0x25, new BossFrameMetadata(Constants.Bank0AByte, 0xDC08, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 01") },
                { 0x26, new BossFrameMetadata(Constants.Bank0AByte, 0xDC38, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 02") },
                { 0x27, new BossFrameMetadata(Constants.Bank0AByte, 0xDC60, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 03") },
                { 0x28, new BossFrameMetadata(Constants.Bank0AByte, 0xDC90, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 04") },
                { 0x29, new BossFrameMetadata(Constants.Bank0AByte, 0xDCC0, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 05") },
                { 0x2A, new BossFrameMetadata(Constants.Bank0AByte, 0xDCF2, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 06") },
                { 0x2B, new BossFrameMetadata(Constants.Bank0AByte, 0xDD3C, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 07") },
                { 0x2C, new BossFrameMetadata(Constants.Bank0AByte, 0xDD86, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 08") },
                { 0x2D, new BossFrameMetadata(Constants.Bank0AByte, 0xDDF0, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 09") },
                { 0x2E, new BossFrameMetadata(Constants.Bank0AByte, 0xDE5A, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 0A") },
                { 0x2F, new BossFrameMetadata(Constants.Bank0AByte, 0xDEC4, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 0B") },
                { 0x30, new BossFrameMetadata(Constants.Bank0AByte, 0xDF2E, BossFrameType.Sprite, "Skeletal Hands Skeletal Squeeze 0C") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> LichBodyFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xDF56, BossFrameType.Background, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xDF62, BossFrameType.Background, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xDF6E, BossFrameType.Background, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xDF7A, BossFrameType.Background, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xDF86, BossFrameType.Background, string.Empty) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xDF92, BossFrameType.Background, string.Empty) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xDFA4, BossFrameType.Background, string.Empty) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xDFB6, BossFrameType.Background, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> LichAuxiliaryFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> ManaBeastFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                // This should be split into two lists. Mana Beast has two tilesets.
                // But since can't draw any of them correctly it doesn't matter for now.
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xEE36, BossFrameType.Mode7, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xEE4A, BossFrameType.Mode7, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xEE5E, BossFrameType.Mode7, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xEE72, BossFrameType.Mode7, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xEE8A, BossFrameType.Mode7, string.Empty) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xEEA2, BossFrameType.Mode7, string.Empty) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xEEBA, BossFrameType.Mode7, string.Empty) },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xEED2, BossFrameType.Mode7, string.Empty) },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xEEEA, BossFrameType.Mode7, string.Empty) },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xEF02, BossFrameType.Mode7, string.Empty) },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xEF1A, BossFrameType.Mode7, string.Empty) },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xEF32, BossFrameType.Mode7, string.Empty) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xEF4A, BossFrameType.Mode7, string.Empty) },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xEF62, BossFrameType.Mode7, string.Empty) },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xEF7A, BossFrameType.Mode7, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> SlimeBodyFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xEEF2, BossFrameType.Mode7, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xEEFE, BossFrameType.Mode7, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xEF0A, BossFrameType.Mode7, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xEF16, BossFrameType.Mode7, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> SlimeFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xEB38, BossFrameType.Sprite, "Idle 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xEB5A, BossFrameType.Sprite, "Idle 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xEB7C, BossFrameType.Sprite, "Idle 03") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xEB9E, BossFrameType.Sprite, "Idle 04") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xEBC0, BossFrameType.Sprite, "Idle 05") },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xEBE2, BossFrameType.Sprite, "Idle 06") },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xEC04, BossFrameType.Sprite, "Idle 07") },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xEC26, BossFrameType.Sprite, "Idle 08") },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xEC48, BossFrameType.Sprite, "Idle 09") },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xEC6A, BossFrameType.Sprite, "Idle 0A") },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xEC8C, BossFrameType.Sprite, "Idle 0B") },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xECAE, BossFrameType.Sprite, "Idle 0C") },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xECD0, BossFrameType.Sprite, "Idle 0D") },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xECF2, BossFrameType.Sprite, "Idle 0E") },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xED14, BossFrameType.Sprite, "Idle 0F") },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xED36, BossFrameType.Sprite, "Idle 10") },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xED58, BossFrameType.Sprite, "Slime Shoot 01") },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xED64, BossFrameType.Sprite, "Slime Shoot 02") },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xED74, BossFrameType.Sprite, "Slime Shoot 03") },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xED88, BossFrameType.Sprite, "Slime Shoot 04") },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xEDA0, BossFrameType.Sprite, "Slime Shoot 05") },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xEDBC, BossFrameType.Sprite, "Slime Shoot 06") },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xEDD8, BossFrameType.Sprite, "Slime Shoot 07") },
                { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xEDF0, BossFrameType.Sprite, "Slime Flight 01") },
                { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xEE06, BossFrameType.Sprite, "Slime Flight 02") },
                { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xEE1C, BossFrameType.Sprite, "Slime Flight 03") },
                { 0x1A, new BossFrameMetadata(Constants.Bank0AByte, 0xEE32, BossFrameType.Sprite, "Slime Crash 01") },
                { 0x1B, new BossFrameMetadata(Constants.Bank0AByte, 0xEE50, BossFrameType.Sprite, "Slime Crash 02") },
                { 0x1C, new BossFrameMetadata(Constants.Bank0AByte, 0xEE76, BossFrameType.Sprite, "Slime Crash 03") },
                { 0x1D, new BossFrameMetadata(Constants.Bank0AByte, 0xEEA4, BossFrameType.Sprite, "Slime Crash 04") },
                { 0x1E, new BossFrameMetadata(Constants.Bank0AByte, 0xEEC6, BossFrameType.Sprite, "Slime Crash 05") },
                { 0x1F, new BossFrameMetadata(Constants.Bank0AByte, 0xEEDC, BossFrameType.Sprite, "Slime Crash 06") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> WallFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xF18C, BossFrameType.Background, "Wall, Left Eyelid Closed") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xF19E, BossFrameType.Background, "Wall, Left Eyelid Opening") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xF1B0, BossFrameType.Background, "Wall, Left Eyelid Open") },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xF1C2, BossFrameType.Background, "Right Eyelid Closed") },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xF1CE, BossFrameType.Background, "Right Eyelid Opening") },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xF1DA, BossFrameType.Background, "Right Eyelid Open") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> WallEyeFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xEF8C, BossFrameType.Sprite, "Side Sclera, Open") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xEFA6, BossFrameType.Sprite, "Side Sclera, Half Open") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xEFB8, BossFrameType.Sprite, "Side Iris, Open", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank07Byte, 0xEFBE, BossFrameType.Sprite, "Side Iris, Closing 01", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank07Byte, 0xEFC4, BossFrameType.Sprite, "Side Iris, Closing 02", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank07Byte, 0xEFCA, BossFrameType.Sprite, "Side Iris, Closed", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank07Byte, 0xEFD0, BossFrameType.Sprite, "Center Eye, Open") },
                { 0x07, new BossFrameMetadata(Constants.Bank07Byte, 0xEFE6, BossFrameType.Sprite, "Center Eye, Opening") },
                { 0x08, new BossFrameMetadata(Constants.Bank07Byte, 0xEFF8, BossFrameType.Sprite, "Center Eye, Closed") },
                { 0x09, new BossFrameMetadata(Constants.Bank07Byte, 0xF008, BossFrameType.Sprite, "Center Eye, Iris Closing") },
                { 0x0A, new BossFrameMetadata(Constants.Bank07Byte, 0xF01C, BossFrameType.Sprite, "Center Eye, Iris Closed") },
                { 0x0B, new BossFrameMetadata(Constants.Bank07Byte, 0xF030, BossFrameType.Sprite, string.Empty, false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank07Byte, 0xF036, BossFrameType.Sprite, string.Empty, false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank07Byte, 0xF040, BossFrameType.Sprite, string.Empty, false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank07Byte, 0xF04E, BossFrameType.Sprite, string.Empty, false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank07Byte, 0xF060, BossFrameType.Sprite, string.Empty, false) },
                { 0x10, new BossFrameMetadata(Constants.Bank07Byte, 0xF076, BossFrameType.Sprite, string.Empty, false) },
                { 0x11, new BossFrameMetadata(Constants.Bank07Byte, 0xF084, BossFrameType.Sprite, string.Empty, false) },
                { 0x12, new BossFrameMetadata(Constants.Bank07Byte, 0xF092, BossFrameType.Sprite, string.Empty, false) },
                { 0x13, new BossFrameMetadata(Constants.Bank07Byte, 0xF0A0, BossFrameType.Sprite, string.Empty, false) },
                { 0x14, new BossFrameMetadata(Constants.Bank07Byte, 0xF0AE, BossFrameType.Sprite, string.Empty, false) },
                { 0x15, new BossFrameMetadata(Constants.Bank07Byte, 0xF0BC, BossFrameType.Sprite, string.Empty, false) },
                { 0x16, new BossFrameMetadata(Constants.Bank07Byte, 0xF0CE, BossFrameType.Sprite, string.Empty, false) },
                { 0x17, new BossFrameMetadata(Constants.Bank07Byte, 0xF0E8, BossFrameType.Sprite, string.Empty, false) },
                { 0x18, new BossFrameMetadata(Constants.Bank07Byte, 0xF102, BossFrameType.Sprite, string.Empty, false) },
                { 0x19, new BossFrameMetadata(Constants.Bank07Byte, 0xF146, BossFrameType.Sprite, string.Empty, false) },
                { 0x1A, new BossFrameMetadata(Constants.Bank07Byte, 0xF164, BossFrameType.Sprite, string.Empty, false) },
                { 0x1B, new BossFrameMetadata(Constants.Bank07Byte, 0xF17A, BossFrameType.Sprite, string.Empty, false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> PumpkinFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank07Byte, 0xE0D8, BossFrameType.Sprite, "Idle") },
                { 0x01, new BossFrameMetadata(Constants.Bank07Byte, 0xE0E6, BossFrameType.Sprite, "Side View") },
                { 0x02, new BossFrameMetadata(Constants.Bank07Byte, 0xE0F4, BossFrameType.Sprite, "Back View") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> HexasFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xC4EC, BossFrameType.Sprite, "Movement 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xC53A, BossFrameType.Sprite, "Movement 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xC584, BossFrameType.Sprite, "Movement 03") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xC5CA, BossFrameType.Sprite, "Movement 04") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xC614, BossFrameType.Sprite, "Idle") },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xC64E, BossFrameType.Sprite, "Damaged") },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xC692, BossFrameType.Sprite, "Lower Arm Spell Cast") },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xC6D4, BossFrameType.Sprite, "Upper Arm Spell Cast") },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xC71E, BossFrameType.Sprite, "Lower To Upper Transition 01") },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xC770, BossFrameType.Sprite, "Lower To Upper Transition 02") },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xC7C2, BossFrameType.Sprite, "Lower To Upper Transition 03") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> HexasSerpentFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xC814, BossFrameType.Sprite, "Serpent Idle 01") },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xC834, BossFrameType.Sprite, "Serpent Idle 02") },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xC854, BossFrameType.Sprite, "Serpent Idle 03") },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xC878, BossFrameType.Sprite, "Serpent Idle 04") },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xC89E, BossFrameType.Sprite, "Spell Summon 01", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xC8A4, BossFrameType.Sprite, "Spell Summon 02", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xC8AA, BossFrameType.Sprite, "Spell Summon 03", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xC8B0, BossFrameType.Sprite, "Spell Summon 04", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xC8C2, BossFrameType.Sprite, "Spell Summon 05", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xC8D4, BossFrameType.Sprite, "Spell Summon 06", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xC8E6, BossFrameType.Sprite, "Spell Summon 07", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xC8F8, BossFrameType.Sprite, "Spell Summon 08", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xC91A, BossFrameType.Sprite, "Spell Summon 09", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> ExplosionFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank1BByte, 0xFD52, BossFrameType.Sprite, string.Empty) },
                { 0x01, new BossFrameMetadata(Constants.Bank1BByte, 0xFD60, BossFrameType.Sprite, string.Empty) },
                { 0x02, new BossFrameMetadata(Constants.Bank1BByte, 0xFD6E, BossFrameType.Sprite, string.Empty) },
                { 0x03, new BossFrameMetadata(Constants.Bank1BByte, 0xFD88, BossFrameType.Sprite, string.Empty) },
                { 0x04, new BossFrameMetadata(Constants.Bank1BByte, 0xFDA2, BossFrameType.Sprite, string.Empty) },
                { 0x05, new BossFrameMetadata(Constants.Bank1BByte, 0xFDBC, BossFrameType.Sprite, string.Empty) },
                { 0x06, new BossFrameMetadata(Constants.Bank1BByte, 0xFDD6, BossFrameType.Sprite, string.Empty) },
                { 0x07, new BossFrameMetadata(Constants.Bank1BByte, 0xFDF0, BossFrameType.Sprite, string.Empty) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> PlatformFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank1BByte, 0xFE0A, BossFrameType.Sprite, "Idle", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> CrystalFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank1BByte, 0xFE78, BossFrameType.Sprite, "Idle", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> FireBreathFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF4A0, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF4A6, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF4AC, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF4B2, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF4B8, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF4BE, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF4D0, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xF4E2, BossFrameType.Sprite, "Fire Breath (Player) 01", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xF4E8, BossFrameType.Sprite, "Fire Breath (Player) 02", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xF4FA, BossFrameType.Sprite, "Fire Breath (Player) 03", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xF50C, BossFrameType.Sprite, "Fire Breath (Player) 04", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xF51E, BossFrameType.Sprite, "Fire Breath (Player) 05", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> FreezeBreathFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF4A0, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF4A6, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF4AC, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF4B2, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF4B8, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF4BE, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF4D0, BossFrameType.Sprite, "Fire & Freeze Breath (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xFB8E, BossFrameType.Sprite, "Freeze Breath (Player) 01", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xFB94, BossFrameType.Sprite, "Freeze Breath (Player) 02", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xFB9E, BossFrameType.Sprite, "Freeze Breath (Player) 03", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xFBAC, BossFrameType.Sprite, "Freeze Breath (Player) 04", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xFBBE, BossFrameType.Sprite, "Freeze Breath (Player) 05", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xFBD4, BossFrameType.Sprite, "Freeze Breath (Player) 06", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xFBF6, BossFrameType.Sprite, "Freeze Breath (Player) 07", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xFC18, BossFrameType.Sprite, "Freeze Breath (Player) 08", false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xFC3A, BossFrameType.Sprite, "Freeze Breath (Player) 09", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xFC5C, BossFrameType.Sprite, "Freeze Breath (Player) 0A", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xFC7E, BossFrameType.Sprite, "Freeze Breath (Player) 0B", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xFCA0, BossFrameType.Sprite, "Freeze Breath (Player) 0C", false) },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xFCC2, BossFrameType.Sprite, "Freeze Breath (Player) 0D", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> BlitzBreathFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                // These frames are the last bit of valid data in Bank CA.
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xFF08, BossFrameType.Sprite, "Blitz Breath (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xFF0E, BossFrameType.Sprite, "Blitz Breath (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xFF14, BossFrameType.Sprite, "Blitz Breath (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xFF1A, BossFrameType.Sprite, "Blitz Breath (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xFF20, BossFrameType.Sprite, "Blitz Breath (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xFF26, BossFrameType.Sprite, "Blitz Breath (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xFF38, BossFrameType.Sprite, "Blitz Breath (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xFF4A, BossFrameType.Sprite, "Blitz Breath (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank1BByte, 0xFCA0, BossFrameType.Sprite, "Blitz Breath (Player) 01", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank1BByte, 0xFCA6, BossFrameType.Sprite, "Blitz Breath (Player) 02", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank1BByte, 0xFCAC, BossFrameType.Sprite, "Blitz Breath (Player) 03", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank1BByte, 0xFCC2, BossFrameType.Sprite, "Blitz Breath (Player) 04", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank1BByte, 0xFCDC, BossFrameType.Sprite, "Blitz Breath (Player) 05", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank1BByte, 0xFCF2, BossFrameType.Sprite, "Blitz Breath (Player) 06", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank1BByte, 0xFD18, BossFrameType.Sprite, "Blitz Breath (Player) 07", false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank1BByte, 0xFD2E, BossFrameType.Sprite, "Blitz Breath (Player) 08", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank1BByte, 0xFD40, BossFrameType.Sprite, "Blitz Breath (Player) 09", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> AcidBreathFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF528, BossFrameType.Sprite, "Acid Breath (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF52E, BossFrameType.Sprite, "Acid Breath (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF534, BossFrameType.Sprite, "Acid Breath (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF53A, BossFrameType.Sprite, "Acid Breath (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF540, BossFrameType.Sprite, "Acid Breath (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF546, BossFrameType.Sprite, "Acid Breath (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF55C, BossFrameType.Sprite, "Acid Breath (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xF56E, BossFrameType.Sprite, "Acid Breath (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xF590, BossFrameType.Sprite, "Acid Breath (Boss) 09", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xF5A2, BossFrameType.Sprite, "Acid Breath (Player) 01", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xF5A8, BossFrameType.Sprite, "Acid Breath (Player) 02", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xF5B2, BossFrameType.Sprite, "Acid Breath (Player) 03", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xF5C0, BossFrameType.Sprite, "Acid Breath (Player) 04", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xF5D2, BossFrameType.Sprite, "Acid Breath (Player) 05", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xF5E8, BossFrameType.Sprite, "Acid Breath (Player) 06", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> BubbleAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF954, BossFrameType.Sprite, "Bubble Attack (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF95A, BossFrameType.Sprite, "Bubble Attack (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF960, BossFrameType.Sprite, "Bubble Attack (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF976, BossFrameType.Sprite, "Bubble Attack (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF98C, BossFrameType.Sprite, "Bubble Attack (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF9B2, BossFrameType.Sprite, "Bubble Attack (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF9D4, BossFrameType.Sprite, "Bubble Attack (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xF9F6, BossFrameType.Sprite, "Bubble Attack (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xFA18, BossFrameType.Sprite, "Bubble Attack (Boss) 09", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xFA3A, BossFrameType.Sprite, "Bubble Attack (Boss) 0A", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xFA4C, BossFrameType.Sprite, "Bubble Attack (Boss) 0B", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xFA5E, BossFrameType.Sprite, "Bubble Attack (Player) 01", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xFA64, BossFrameType.Sprite, "Bubble Attack (Player) 02", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xFA6E, BossFrameType.Sprite, "Bubble Attack (Player) 03", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xFA7C, BossFrameType.Sprite, "Bubble Attack (Player) 04", false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xFA8E, BossFrameType.Sprite, "Bubble Attack (Player) 05", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xFAA4, BossFrameType.Sprite, "Bubble Attack (Player) 06", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xFABA, BossFrameType.Sprite, "Bubble Attack (Player) 07", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xFAD0, BossFrameType.Sprite, "Bubble Attack (Player) 08", false) },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xFAE6, BossFrameType.Sprite, "Bubble Attack (Player) 09", false) },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xFAF8, BossFrameType.Sprite, "Bubble Attack (Player) 0A", false) },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xFB06, BossFrameType.Sprite, "Bubble Attack (Player) 0B", false) },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xFB10, BossFrameType.Sprite, "Bubble Attack (Player) 0C", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> GasAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF602, BossFrameType.Sprite, "Gas Attack (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF608, BossFrameType.Sprite, "Gas Attack (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF60E, BossFrameType.Sprite, "Gas Attack (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF614, BossFrameType.Sprite, "Gas Attack (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF62A, BossFrameType.Sprite, "Gas Attack (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF640, BossFrameType.Sprite, "Gas Attack (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF652, BossFrameType.Sprite, "Gas Attack (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xF664, BossFrameType.Sprite, "Gas Attack (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xF686, BossFrameType.Sprite, "Gas Attack (Boss) 09", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xF698, BossFrameType.Sprite, "Gas Attack (Boss) 0A", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xF6AA, BossFrameType.Sprite, "Gas Attack (Boss) 0B", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xF6BC, BossFrameType.Sprite, "Gas Attack (Boss) 0C", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xF6CE, BossFrameType.Sprite, "Gas Attack (Player) 01", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xF6D4, BossFrameType.Sprite, "Gas Attack (Player) 02", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xF6E6, BossFrameType.Sprite, "Gas Attack (Player) 03", false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xF6FC, BossFrameType.Sprite, "Gas Attack (Player) 04", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xF71E, BossFrameType.Sprite, "Gas Attack (Player) 05", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xF740, BossFrameType.Sprite, "Gas Attack (Player) 06", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xF756, BossFrameType.Sprite, "Gas Attack (Player) 07", false) },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xF778, BossFrameType.Sprite, "Gas Attack (Player) 08", false) },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xF78A, BossFrameType.Sprite, "Gas Attack (Player) 09", false) },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xF79C, BossFrameType.Sprite, "Gas Attack (Player) 0A", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> BeamAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xFB76, BossFrameType.Sprite, "Beam Attack (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xFB7C, BossFrameType.Sprite, "Beam Attack (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xFB82, BossFrameType.Sprite, "Beam Attack (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xFB88, BossFrameType.Sprite, "Beam Attack (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank1BByte, 0xFF30, BossFrameType.Sprite, "Beam Attack - Horizontal (Object)") },
                { 0x05, new BossFrameMetadata(Constants.Bank1BByte, 0xFF42, BossFrameType.Sprite, "Beam Attack - 150 Degree Angle (Object)") },
                { 0x06, new BossFrameMetadata(Constants.Bank1BByte, 0xFF54, BossFrameType.Sprite, "Beam Attack - 135 Degree Angle (Object)") },
                { 0x07, new BossFrameMetadata(Constants.Bank1BByte, 0xFF66, BossFrameType.Sprite, "Beam Attack - 115 Degree Angle (Object)") },
                { 0x08, new BossFrameMetadata(Constants.Bank1BByte, 0xFF78, BossFrameType.Sprite, "Beam Attack - Vertical (Object)") },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> RingAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF7AE, BossFrameType.Sprite, "Ring Attack (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF7B4, BossFrameType.Sprite, "Ring Attack (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF7C6, BossFrameType.Sprite, "Ring Attack (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF7DC, BossFrameType.Sprite, "Ring Attack (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF7EE, BossFrameType.Sprite, "Ring Attack (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF804, BossFrameType.Sprite, "Ring Attack (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF82A, BossFrameType.Sprite, "Ring Attack (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xF840, BossFrameType.Sprite, "Ring Attack (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xF852, BossFrameType.Sprite, "Ring Attack (Boss) 09", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xF864, BossFrameType.Sprite, "Ring Attack (Boss) 0A", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xF87A, BossFrameType.Sprite, "Ring Attack (Player) 01", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xF884, BossFrameType.Sprite, "Ring Attack (Player) 02", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xF88A, BossFrameType.Sprite, "Ring Attack (Player) 03", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xF894, BossFrameType.Sprite, "Ring Attack (Player) 04", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xF89E, BossFrameType.Sprite, "Ring Attack (Player) 05", false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xF8A4, BossFrameType.Sprite, "Ring Attack (Player) 06", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xF8AE, BossFrameType.Sprite, "Ring Attack (Player) 07", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xF8B8, BossFrameType.Sprite, "Ring Attack (Player) 08", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xF8BE, BossFrameType.Sprite, "Ring Attack (Player) 09", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> GlareAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF8E2, BossFrameType.Sprite, "Glare Attack (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xF8E8, BossFrameType.Sprite, "Glare Attack (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xF8EE, BossFrameType.Sprite, "Glare Attack (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xF8F4, BossFrameType.Sprite, "Glare Attack (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xF8FA, BossFrameType.Sprite, "Glare Attack (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xF90C, BossFrameType.Sprite, "Glare Attack (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xF91E, BossFrameType.Sprite, "Glare Attack (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xF930, BossFrameType.Sprite, "Glare Attack (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xF942, BossFrameType.Sprite, "Glare Attack (Boss) 09", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> CannonAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                // Mech Rider may be the glitchiest boss, but by god he will have the most fluid skill animations!
                // ....that he mostly can't use because of the glitches.
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xFCE4, BossFrameType.Sprite, "Cannon Attack (Boss) 01", false) },
                { 0x01, new BossFrameMetadata(Constants.Bank0AByte, 0xFCF2, BossFrameType.Sprite, "Cannon Attack (Boss) 02", false) },
                { 0x02, new BossFrameMetadata(Constants.Bank0AByte, 0xFD00, BossFrameType.Sprite, "Cannon Attack (Boss) 03", false) },
                { 0x03, new BossFrameMetadata(Constants.Bank0AByte, 0xFD16, BossFrameType.Sprite, "Cannon Attack (Boss) 04", false) },
                { 0x04, new BossFrameMetadata(Constants.Bank0AByte, 0xFD2C, BossFrameType.Sprite, "Cannon Attack (Boss) 05", false) },
                { 0x05, new BossFrameMetadata(Constants.Bank0AByte, 0xFD4A, BossFrameType.Sprite, "Cannon Attack (Boss) 06", false) },
                { 0x06, new BossFrameMetadata(Constants.Bank0AByte, 0xFD68, BossFrameType.Sprite, "Cannon Attack (Boss) 07", false) },
                { 0x07, new BossFrameMetadata(Constants.Bank0AByte, 0xFD86, BossFrameType.Sprite, "Cannon Attack (Boss) 08", false) },
                { 0x08, new BossFrameMetadata(Constants.Bank0AByte, 0xFDA4, BossFrameType.Sprite, "Cannon Attack (Boss) 09", false) },
                { 0x09, new BossFrameMetadata(Constants.Bank0AByte, 0xFDBA, BossFrameType.Sprite, "Cannon Attack (Boss) 0A", false) },
                { 0x0A, new BossFrameMetadata(Constants.Bank0AByte, 0xFDD0, BossFrameType.Sprite, "Cannon Attack (Boss) 0B", false) },
                { 0x0B, new BossFrameMetadata(Constants.Bank0AByte, 0xFDDE, BossFrameType.Sprite, "Cannon Attack (Boss) 0C", false) },
                { 0x0C, new BossFrameMetadata(Constants.Bank0AByte, 0xFDEC, BossFrameType.Sprite, "Cannon Attack (Boss) 0D", false) },
                { 0x0D, new BossFrameMetadata(Constants.Bank0AByte, 0xFDF2, BossFrameType.Sprite, "Cannon Attack (Boss) 0E", false) },
                { 0x0E, new BossFrameMetadata(Constants.Bank0AByte, 0xFE08, BossFrameType.Sprite, "Cannon Attack (Boss) 0F", false) },
                { 0x0F, new BossFrameMetadata(Constants.Bank0AByte, 0xFE1A, BossFrameType.Sprite, "Cannon Attack (Boss) 10", false) },
                { 0x10, new BossFrameMetadata(Constants.Bank0AByte, 0xFE2C, BossFrameType.Sprite, "Cannon Attack (Player) 01", false) },
                { 0x11, new BossFrameMetadata(Constants.Bank0AByte, 0xFE32, BossFrameType.Sprite, "Cannon Attack (Player) 02", false) },
                { 0x12, new BossFrameMetadata(Constants.Bank0AByte, 0xFE38, BossFrameType.Sprite, "Cannon Attack (Player) 03", false) },
                { 0x13, new BossFrameMetadata(Constants.Bank0AByte, 0xFE4A, BossFrameType.Sprite, "Cannon Attack (Player) 04", false) },
                { 0x14, new BossFrameMetadata(Constants.Bank0AByte, 0xFE60, BossFrameType.Sprite, "Cannon Attack (Player) 05", false) },
                { 0x15, new BossFrameMetadata(Constants.Bank0AByte, 0xFE6A, BossFrameType.Sprite, "Cannon Attack (Player) 06", false) },
                { 0x16, new BossFrameMetadata(Constants.Bank0AByte, 0xFE84, BossFrameType.Sprite, "Cannon Attack (Player) 07", false) },
                { 0x17, new BossFrameMetadata(Constants.Bank0AByte, 0xFEAE, BossFrameType.Sprite, "Cannon Attack (Player) 08", false) },
                { 0x18, new BossFrameMetadata(Constants.Bank0AByte, 0xFED4, BossFrameType.Sprite, "Cannon Attack (Player) 09", false) },
                { 0x19, new BossFrameMetadata(Constants.Bank0AByte, 0xFEF6, BossFrameType.Sprite, "Cannon Attack (Player) 0A", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> CurrentFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
                { 0x00, new BossFrameMetadata(Constants.Bank0AByte, 0xF8C8, BossFrameType.Sprite, "Current (Player)", false) },
            };
            private static readonly IReadOnlyDictionary<int, BossFrameMetadata> BossSpecificAttackFrameLookup = new Dictionary<int, BossFrameMetadata>
            {
            };

            private static readonly IReadOnlyDictionary<BossFamily, IReadOnlyDictionary<int, BossFrameMetadata>> LookupMapping = new Dictionary<BossFamily, IReadOnlyDictionary<int, BossFrameMetadata>>
            {
                { BossFamily.Plant, BossFrameAddressFactory.PlantFrameLookup },
                { BossFamily.DragonBody, BossFrameAddressFactory.DragonBodyFrameLookup },
                { BossFamily.Dragon, BossFrameAddressFactory.DragonFrameLookup },
                { BossFamily.Robot, BossFrameAddressFactory.RobotFrameLookup },
                { BossFamily.RobotAuxiliaryHammer, BossFrameAddressFactory.RobotAuxiliaryHammerFrameLookup },
                { BossFamily.RobotAuxiliaryChainsaw, BossFrameAddressFactory.RobotAuxiliaryChainsawFrameLookup },
                { BossFamily.Hydra, BossFrameAddressFactory.HydraFrameLookup },
                { BossFamily.Ant, BossFrameAddressFactory.AntFrameLookup },
                { BossFamily.Minotaur, BossFrameAddressFactory.MinotaurFrameLookup },
                { BossFamily.Aegagropilon, BossFrameAddressFactory.AegagropilonFrameLookup },
                { BossFamily.AegagropilonBody, BossFrameAddressFactory.AegagropilonBodyFrameLookup },
                { BossFamily.Tiger, BossFrameAddressFactory.TigerFrameLookup },
                { BossFamily.Lizard, BossFrameAddressFactory.LizardFrameLookup },
                { BossFamily.Gigas, BossFrameAddressFactory.GigasFrameLookup },
                { BossFamily.GigasAuxiliaryDiamond, BossFrameAddressFactory.GigasAuxiliaryDiamondFrameLookup },
                { BossFamily.GigasAuxiliaryOrb, BossFrameAddressFactory.GigasAuxiliaryOrbFrameLookup },
                { BossFamily.Bird, BossFrameAddressFactory.BirdFrameLookup },
                { BossFamily.MechRider, BossFrameAddressFactory.MechRiderFrameLookup },
                { BossFamily.Serpent, BossFrameAddressFactory.SerpentFrameLookup },
                { BossFamily.Vampire, BossFrameAddressFactory.VampireFrameLookup },
                { BossFamily.Lich, BossFrameAddressFactory.LichFrameLookup },
                { BossFamily.LichBody, BossFrameAddressFactory.LichBodyFrameLookup },
                { BossFamily.LichAuxiliary, BossFrameAddressFactory.LichAuxiliaryFrameLookup },
                { BossFamily.ManaBeast, BossFrameAddressFactory.ManaBeastFrameLookup },
                { BossFamily.SlimeBody, BossFrameAddressFactory.SlimeBodyFrameLookup },
                { BossFamily.Slime, BossFrameAddressFactory.SlimeFrameLookup },
                { BossFamily.Wall, BossFrameAddressFactory.WallFrameLookup },
                { BossFamily.WallEye, BossFrameAddressFactory.WallEyeFrameLookup },
                { BossFamily.Pumpkin, BossFrameAddressFactory.PumpkinFrameLookup },
                { BossFamily.Hexas, BossFrameAddressFactory.HexasFrameLookup },
                { BossFamily.HexasSerpent, BossFrameAddressFactory.HexasSerpentFrameLookup },
                { BossFamily.Explosion, BossFrameAddressFactory.ExplosionFrameLookup },
                { BossFamily.Platform, BossFrameAddressFactory.PlatformFrameLookup },
                { BossFamily.Crystal, BossFrameAddressFactory.CrystalFrameLookup },
                { BossFamily.FireBreath, BossFrameAddressFactory.FireBreathFrameLookup },
                { BossFamily.FreezeBreath, BossFrameAddressFactory.FreezeBreathFrameLookup },
                { BossFamily.BlitzBreath, BossFrameAddressFactory.BlitzBreathFrameLookup },
                { BossFamily.AcidBreath, BossFrameAddressFactory.AcidBreathFrameLookup },
                { BossFamily.StatusBubbles, BossFrameAddressFactory.BubbleAttackFrameLookup },
                { BossFamily.GasAttack, BossFrameAddressFactory.GasAttackFrameLookup },
                { BossFamily.BeamAttack, BossFrameAddressFactory.BeamAttackFrameLookup },
                { BossFamily.RingAttack, BossFrameAddressFactory.RingAttackFrameLookup },
                { BossFamily.GlareAttack, BossFrameAddressFactory.GlareAttackFrameLookup },
                { BossFamily.CannonAttack, BossFrameAddressFactory.CannonAttackFrameLookup },
                { BossFamily.Current, BossFrameAddressFactory.CurrentFrameLookup },
                { BossFamily.BossSpecificAttack, BossFrameAddressFactory.BossSpecificAttackFrameLookup },
            };

            public static bool HasFrames(BossFamily family)
            {
                return family == BossFamily.BossSpecificAttack || (BossFrameAddressFactory.LookupMapping.ContainsKey(family) && BossFrameAddressFactory.LookupMapping[family].Count > 0);
            }

            public static int GetFrameCount(BossFamily family)
            {
                return BossFrameAddressFactory.LookupMapping[family].Count;
            }

            public static int GetFrameAddress(BossFamily family, int frameIndex)
            {
                return BossFrameAddressFactory.LookupMapping[family][frameIndex].FullAddress;
            }

            public static bool FrameHasHitBoxData(BossFamily family, int frameIndex)
            {
                return BossFrameAddressFactory.LookupMapping[family][frameIndex].HasHitBoxData;
            }

            public static BossFrameType GetFrameType(BossFamily family, int frameIndex)
            {
                return BossFrameAddressFactory.LookupMapping[family][frameIndex].FrameType;
            }

            public static string GetFrameName(BossFamily family, int frameIndex)
            {
                return BossFrameAddressFactory.LookupMapping[family][frameIndex].DisplayName;
            }

            public static BossFrameMetadata GetMetadata(BossFamily family, int frameIndex)
            {
                return BossFrameAddressFactory.LookupMapping[family][frameIndex];
            }
        }
    }
}