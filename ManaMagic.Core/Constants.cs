#nullable enable

namespace ManaMagic.Core
{
    /// <summary>
    /// Contains addresses, offsets, and other important values related to the Secret Of Mana ROM file.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The base value for Bank 00 addressing.
        /// </summary>
        public const int Bank00Byte = 0x00;
        /// <summary>
        /// The base value for Bank 01 addressing.
        /// </summary>
        public const int Bank01Byte = 0x01;
        /// <summary>
        /// The base value for Bank 02 addressing.
        /// </summary>
        public const int Bank02Byte = 0x02;
        /// <summary>
        /// The base value for Bank 03 addressing.
        /// </summary>
        public const int Bank03Byte = 0x03;
        /// <summary>
        /// The base value for Bank 04 addressing.
        /// </summary>
        public const int Bank04Byte = 0x04;
        /// <summary>
        /// The base value for Bank 05 addressing.
        /// </summary>
        public const int Bank05Byte = 0x05;
        /// <summary>
        /// The base value for Bank 06 addressing.
        /// </summary>
        public const int Bank06Byte = 0x06;
        /// <summary>
        /// The base value for Bank 07 addressing.
        /// </summary>
        public const int Bank07Byte = 0x07;
        /// <summary>
        /// The base value for Bank 08 addressing.
        /// </summary>
        public const int Bank08Byte = 0x08;
        /// <summary>
        /// The base value for Bank 09 addressing.
        /// </summary>
        public const int Bank09Byte = 0x09;
        /// <summary>
        /// The base value for Bank 0A addressing.
        /// </summary>
        public const int Bank0AByte = 0x0A;
        /// <summary>
        /// The base value for Bank 0B addressing.
        /// </summary>
        public const int Bank0BByte = 0x0B;
        /// <summary>
        /// The base value for Bank 0C addressing.
        /// </summary>
        public const int Bank0CByte = 0x0C;
        /// <summary>
        /// The base value for Bank 0D addressing.
        /// </summary>
        public const int Bank0DByte = 0x0D;
        /// <summary>
        /// The base value for Bank 0E addressing.
        /// </summary>
        public const int Bank0EByte = 0x0E;
        /// <summary>
        /// The base value for Bank 0F addressing.
        /// </summary>
        public const int Bank0FByte = 0x0F;
        /// <summary>
        /// The base value for Bank 10 addressing.
        /// </summary>
        public const int Bank10Byte = 0x10;
        /// <summary>
        /// The base value for Bank 11 addressing.
        /// </summary>
        public const int Bank11Byte = 0x11;
        /// <summary>
        /// The base value for Bank 12 addressing.
        /// </summary>
        public const int Bank12Byte = 0x12;
        /// <summary>
        /// The base value for Bank 13 addressing.
        /// </summary>
        public const int Bank13Byte = 0x13;
        /// <summary>
        /// The base value for Bank 14 addressing.
        /// </summary>
        public const int Bank14Byte = 0x14;
        /// <summary>
        /// The base value for Bank 15 addressing.
        /// </summary>
        public const int Bank15Byte = 0x15;
        /// <summary>
        /// The base value for Bank 16 addressing.
        /// </summary>
        public const int Bank16Byte = 0x16;
        /// <summary>
        /// The base value for Bank 17 addressing.
        /// </summary>
        public const int Bank17Byte = 0x17;
        /// <summary>
        /// The base value for Bank 18 addressing.
        /// </summary>
        public const int Bank18Byte = 0x18;
        /// <summary>
        /// The base value for Bank 19 addressing.
        /// </summary>
        public const int Bank19Byte = 0x19;
        /// <summary>
        /// The base value for Bank 1A addressing.
        /// </summary>
        public const int Bank1AByte = 0x1A;
        /// <summary>
        /// The base value for Bank 1B addressing.
        /// </summary>
        public const int Bank1BByte = 0x1B;
        /// <summary>
        /// The base value for Bank 1C addressing.
        /// </summary>
        public const int Bank1CByte = 0x1C;
        /// <summary>
        /// The base value for Bank 1D addressing.
        /// </summary>
        public const int Bank1DByte = 0x1D;
        /// <summary>
        /// The base value for Bank 1E addressing.
        /// </summary>
        public const int Bank1EByte = 0x1E;
        /// <summary>
        /// The base value for Bank 1E addressing.
        /// </summary>
        public const int Bank1FByte = 0x1F;

        /// <summary>
        /// The base offset for Bank 00 addressing.
        /// </summary>
        public const int Bank00Offset = Constants.Bank00Byte << 16;
        /// <summary>
        /// The base offset for Bank 01 addressing.
        /// </summary>
        public const int Bank01Offset = Constants.Bank01Byte << 16;
        /// <summary>
        /// The base offset for Bank 02 addressing.
        /// </summary>
        public const int Bank02Offset = Constants.Bank02Byte << 16;
        /// <summary>
        /// The base offset for Bank 03 addressing.
        /// </summary>
        public const int Bank03Offset = Constants.Bank03Byte << 16;
        /// <summary>
        /// The base offset for Bank 04 addressing.
        /// </summary>
        public const int Bank04Offset = Constants.Bank04Byte << 16;
        /// <summary>
        /// The base offset for Bank 05 addressing.
        /// </summary>
        public const int Bank05Offset = Constants.Bank05Byte << 16;
        /// <summary>
        /// The base offset for Bank 06 addressing.
        /// </summary>
        public const int Bank06Offset = Constants.Bank06Byte << 16;
        /// <summary>
        /// The base offset for Bank 07 addressing.
        /// </summary>
        public const int Bank07Offset = Constants.Bank07Byte << 16;
        /// <summary>
        /// The base offset for Bank 08 addressing.
        /// </summary>
        public const int Bank08Offset = Constants.Bank08Byte << 16;
        /// <summary>
        /// The base offset for Bank 09 addressing.
        /// </summary>
        public const int Bank09Offset = Constants.Bank09Byte << 16;
        /// <summary>
        /// The base offset for Bank 0A addressing.
        /// </summary>
        public const int Bank0AOffset = Constants.Bank0AByte << 16;
        /// <summary>
        /// The base offset for Bank 0B addressing.
        /// </summary>
        public const int Bank0BOffset = Constants.Bank0BByte << 16;
        /// <summary>
        /// The base offset for Bank 0C addressing.
        /// </summary>
        public const int Bank0COffset = Constants.Bank0CByte << 16;
        /// <summary>
        /// The base offset for Bank 0D addressing.
        /// </summary>
        public const int Bank0DOffset = Constants.Bank0DByte << 16;
        /// <summary>
        /// The base offset for Bank 0E addressing.
        /// </summary>
        public const int Bank0EOffset = Constants.Bank0EByte << 16;
        /// <summary>
        /// The base offset for Bank 0F addressing.
        /// </summary>
        public const int Bank0FOffset = Constants.Bank0FByte << 16;
        /// <summary>
        /// The base offset for Bank 10 addressing.
        /// </summary>
        public const int Bank10Offset = Constants.Bank10Byte << 16;
        /// <summary>
        /// The base offset for Bank 11 addressing.
        /// </summary>
        public const int Bank11Offset = Constants.Bank11Byte << 16;
        /// <summary>
        /// The base offset for Bank 12 addressing.
        /// </summary>
        public const int Bank12Offset = Constants.Bank12Byte << 16;
        /// <summary>
        /// The base offset for Bank 13 addressing.
        /// </summary>
        public const int Bank13Offset = Constants.Bank13Byte << 16;
        /// <summary>
        /// The base offset for Bank 14 addressing.
        /// </summary>
        public const int Bank14Offset = Constants.Bank14Byte << 16;
        /// <summary>
        /// The base offset for Bank 15 addressing.
        /// </summary>
        public const int Bank15Offset = Constants.Bank15Byte << 16;
        /// <summary>
        /// The base offset for Bank 16 addressing.
        /// </summary>
        public const int Bank16Offset = Constants.Bank16Byte << 16;
        /// <summary>
        /// The base offset for Bank 17 addressing.
        /// </summary>
        public const int Bank17Offset = Constants.Bank17Byte << 16;
        /// <summary>
        /// The base offset for Bank 18 addressing.
        /// </summary>
        public const int Bank18Offset = Constants.Bank18Byte << 16;
        /// <summary>
        /// The base offset for Bank 19 addressing.
        /// </summary>
        public const int Bank19Offset = Constants.Bank19Byte << 16;
        /// <summary>
        /// The base offset for Bank 1A addressing.
        /// </summary>
        public const int Bank1AOffset = Constants.Bank1AByte << 16;
        /// <summary>
        /// The base offset for Bank 1B addressing.
        /// </summary>
        public const int Bank1BOffset = Constants.Bank1BByte << 16;
        /// <summary>
        /// The base offset for Bank 1C addressing.
        /// </summary>
        public const int Bank1COffset = Constants.Bank1CByte << 16;
        /// <summary>
        /// The base offset for Bank 1D addressing.
        /// </summary>
        public const int Bank1DOffset = Constants.Bank1DByte << 16;
        /// <summary>
        /// The base offset for Bank 1E addressing.
        /// </summary>
        public const int Bank1EOffset = Constants.Bank1EByte << 16;
        /// <summary>
        /// The base offset for Bank 1F addressing.
        /// </summary>
        public const int Bank1FOffset = Constants.Bank1FByte << 16;

        public const uint EventCount = 0x0800;
        public const int TextMaxLength = 55;

        public static class SoundEffects
        {
            public const ushort None = 0x0000;
            public const ushort BossSkillRingAttackPlayer = 0x0019;
            public const ushort DeathMachineChainsawRev = 0x0043;
            public const ushort BossSkillGeneralBoss = 0x0083;
            public const ushort BossSkillCannonBoss = 0x0087;
            public const ushort BossSkillFreezeBeamPlayer = 0x0090;
            public const ushort BossSkillSonicPulseBoss = 0x009A;
            public const ushort BossSkillPetrifyBeamPlayer = 0x009B;
            public const ushort BossSkillAcidBreathPlayer = 0x00A9;
            public const ushort BossSkillCaveInBoss = 0x00B0;
            public const ushort BossSkillWallDesperationBoss = 0x00B3;
            public const ushort BossSkillGeneralPlayer = 0x00BB;
            public const ushort BossSkillFreezeBreathPlayer = 0x00C0;
            public const ushort BossSkillBubblesAttackPlayer = 0x00C5;
            public const ushort BossSkillGasAttackPlayer = 0x00CB;
            public const ushort BossSkillCurrentPlayer = 0x00D5;
            public const ushort SnakeBossHiss = 0x00D7;
        }

        public static class BossAnimations
        {
            public const ushort DragonTailIdle = 0x0157;
            public const ushort DragonTailAttack = 0x0158;
        }

        public static class BossWeapons
        {
            public const byte SkillBalloonRing = 0x12;
            public const byte SkillSleepRing = 0x13;
            public const byte SkillSleepRing2 = 0x14;
            public const byte SkillMoogleGlare = 0x15;
            public const byte SkillMoogleGlare2 = 0x18;
            public const byte SkillWaveCannon = 0x19;
            public const byte SkillDiffuserCannon = 0x1A;
            public const byte SkillDragonWormPetrifyGas = 0x3A;
            public const byte GreatViperLunge = 0x3C;
            public const byte GreatViperSwallowWhole = 0x3D;
            public const byte DragonWormSwallowWhole = 0x3E;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 00.
        /// </summary>
        public static class Bank00
        {
            public const uint DefaultCharacterDataTableAddress = Constants.Bank00Offset | 0x57B3;

            public const uint StatusEffectMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5DBB;
            public const uint StatusEffectMessageEventsPointerTableSize = 0x10;

            public const uint ElementalFearMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5DDB;
            public const uint ElementalFearMessageEventsPointerTableSize = 0x08;

            public const uint TrapMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5DEB;
            public const uint TrapMessageEventsPointerTableSize = 0x08;

            public const uint WeaponNameMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5DFB;
            public const uint WeaponNameMessageEventsPointerTableSize = 0x08;

            public const uint BossSkillNameMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5E0B;
            public const uint BossSkillNameMessageEventsPointerTableSize = 0x1F;

            public const uint BuffDebuffMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5E49;
            public const uint BuffDebuffMessageEventsPointerTableSize = 0x08;

            public const uint LunarMagicMessageEventsPointerTableAddress = Constants.Bank00Offset | 0x5E59;
            public const uint LunarMagicMessageEventsPointerTableSize = 0x09;

            public const uint SpellLevelMessageAddress = Constants.Bank00Offset | 0x6251;
            public const uint LevelUpMessageAddress = Constants.Bank00Offset | 0x6256;
            public const uint LevelUpPeriodMessageAddress = Constants.Bank00Offset | 0x6263;
            public const uint WeaponSkillUpMessageAddress = Constants.Bank00Offset | 0x6265;
            public const uint MagicSkillUpMessageAddress = Constants.Bank00Offset | 0x6279;
            public const uint AnalyzerHPMessageAddress = Constants.Bank00Offset | 0x628C;
            public const uint AnalyzerMPMessageAddress = Constants.Bank00Offset | 0x6290;
            public const uint GPInsideMessageAddress = Constants.Bank00Offset | 0x6294;
            public const uint ChestItemExclamationMessageAddress = Constants.Bank00Offset | 0x62A0;
            public const uint DummiedOutMessageAddress = Constants.Bank00Offset | 0x62A2;
            public const uint RepelledTheMagicMessageAddress = Constants.Bank00Offset | 0x62A3;
            public const uint GetsWhackedMessageAddress = Constants.Bank00Offset | 0x62B7;
            public const uint WontFitMessageAddress = Constants.Bank00Offset | 0x62C6;
            public const uint AnalyzerExpMessageAddress = Constants.Bank00Offset | 0x62D2;
            public const uint AnalyzerGPMessageAddress = Constants.Bank00Offset | 0x62D7;
            public const uint RecoveryFailedMessageAddress = Constants.Bank00Offset | 0x62E2;
            public const uint MagicFadedMessageAddress = Constants.Bank00Offset | 0x62F3;
            public const uint StillAliveMessageAddress = Constants.Bank00Offset | 0x6303;
            public const uint CantUndoWallMessageAddress = Constants.Bank00Offset | 0x6310;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 01.
        /// </summary>
        public static class Bank01
        {
            public const uint LZ77DecompressionKeyAddress = Constants.Bank01Offset | 0x4C00;
            public const uint LZ77DecompressionKeySize = 0x06;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 02.
        /// </summary>
        public static class Bank02
        {
            public const uint BossInitializationScriptPointerTableAddress = Constants.Bank02Offset | 0x00EE;
            public const uint BossInitializationScriptPointerTableTableSize = 0x2C;

            public const uint BossGraphicScriptPointerTableAddress = Constants.Bank02Offset | 0x0146;
            public const uint BossGraphicScriptPointerTableTableSize = 0x2C;

            public const uint BossPaletteTableAddress = Constants.Bank02Offset | 0xF19C;
            public const int BossPaletteTableAddressSize = 0x88;
            public const int BossPaletteColorCount = 16;

            public const uint DragonFamilyTailAnimationIdSyncTable = Constants.Bank02Offset | 0x8EB0;
            public const uint DragonFamilyBodyAnimationIdSyncTable = Constants.Bank02Offset | 0x8EE8;

            public const uint AegagropilonSetupHeaderAddress = Constants.Bank02Offset | 0xD8D8;
            public const uint AntFamilySetupHeaderAddress = Constants.Bank02Offset | 0xD080;
            public const uint DarkLichSetupHeaderAddress = Constants.Bank02Offset | 0xE82E;
            public const uint DeathMachineSetupHeaderAddress = Constants.Bank02Offset | 0xD3F9;
            public const uint DragonFamilySetupHeaderAddress = Constants.Bank02Offset | 0xF00C;
            public const uint MechRiderFamilySetupHeaderAddress = Constants.Bank02Offset | 0xEB49;
            public const uint KettleKinSetupHeaderAddress = Constants.Bank02Offset | 0xD451;
            public const uint HexasFamilySetupHeaderAddress = Constants.Bank02Offset | 0xE120;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 03.
        /// </summary>
        public static class Bank03 { }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 06.
        /// </summary>
        public static class Bank06
        {
            public const int WorldMapLandingLocationTableAddress = Constants.Bank06Offset | 0x7780;
            public const uint WorldMapLandingLocationSize = 0x100;

            public const int FlammieFlightCoordinateTableAddress = Constants.Bank06Offset | 0x7A80;
            public const uint FlammieFlightCoordinateTableSize = 0x100;

            public const int CannonTravelCoordinateTableAddress = Constants.Bank06Offset | 0x7C80;
            public const uint CannonTravelCoordinateTableSize = 0x40;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 08.
        /// </summary>
        public static class Bank08
        {
            public const int MapDisplaySettingsTable = Constants.Bank08Offset | 0x0100;
            public const uint MapDisplaySettingsTableSize = 0x40;

            public const int MapAnimatedTilesIndexTable = Constants.Bank08Offset | 0x0400;
            public const uint MapAnimatedTilesIndexTableSize = 0x41;

            public const int MapAnimatedTilesEncodedPointerTable = Constants.Bank08Offset | 0x0441;

            public const uint SpritePaletteTableAddress = Constants.Bank08Offset | 0x1000;
            public const uint SpritePaletteTableSize = 0x100;
            public const uint NumberOfColorsPerSpritePalette = 0x0F;

            public const uint DoorTable = Constants.Bank08Offset | 0x3000;
            public const uint DoorTableSize = 0x0400;

            public const uint MapTriggerPointerTableAddress = Constants.Bank08Offset | 0x4000;

            public const int MapLayerDefinitionPointerTable = Constants.Bank08Offset | 0x5000;
            public const int MapHeaderNPCPointerTable = Constants.Bank08Offset | 0x7000;

            public const int NumberOfMaps = 0x200;     // Actually 0x1FF. Last one is an invalid stopper.

        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 09.
        /// </summary>
        public static class Bank09
        {
            public const uint EventIdMinimum = 0x0000;
            public const uint EventIdMaximum = 0x03FF;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 0A.
        /// </summary>
        public static class Bank0A
        {
            public const uint EventIdMinimum = 0x0400;
            public const uint EventIdMaximum = 0x07FF;

            public const uint SpellNamesPointerTableAddress = Constants.Bank0AOffset | 0x0800;
            public const uint SpellNamesPointerTableSize = 0x32;

            public const uint WeaponNamesPointerTableAddress = Constants.Bank0AOffset | 0x0864;
            public const uint WeaponNamesPointerTableSize = 0x48;

            public const uint EquipmentNamesPointerTableAddress = Constants.Bank0AOffset | 0x08F4;
            public const uint EquipmentNamesPointerTableSize = 0x40;

            public const uint ItemNamesPointerTableAddress = Constants.Bank0AOffset | 0x0974;
            public const uint ItemNamesPointerTableSize = 0x0C;

            public const uint RingMenuNamesPointerTableAddress = Constants.Bank0AOffset | 0x098C;
            public const uint RingMenuNamesPointerTableSize = 0x09;

            public const uint WeaponDescriptionsPointerTableAddress = Constants.Bank0AOffset | 0x0A9E;
            public const uint WeaponDescriptionsPointerTableSize = 0x48;

            public const uint SpellDescriptionsPointerTableAddress = Constants.Bank0AOffset | 0x0B2E;
            public const uint SpellDescriptionsPointerTableSize = 0x2A;

            public const uint EnemyNamesPointerTableAddress = Constants.Bank0AOffset | 0x099E;
            public const uint EnemyNamesPointerTableSize = 0x80;
            public const byte FirstBossIndex = 0x57;
            public const byte LastBossIndex = 0x7F;

            public const uint TownNameEventsPointerTableAddress = Constants.Bank0AOffset | 0x0B82;
            public const uint TownNameEventsPointerTableSize = 0x38;

            public const uint ItemErrorMessageEventsPointerTableAddress = Constants.Bank0AOffset | 0x0BF2;
            public const uint ItemErrorMessageEventsPointerTableSize = 0x08;

            public const uint BossPaletteScriptPointerTableAddress = Constants.Bank0AOffset | 0xBE00;
            public const uint BossPaletteScriptPointerTableTableSize = 0x2C;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 0B.
        /// </summary>
        public static class Bank0B
        {
            /// <summary>
            /// The address of the map collision type definition table.
            /// </summary>
            public const uint CollisionTypeDefinitionsTableAddress = Constants.Bank0BOffset | 0x0000;

            /// <summary>
            /// The size of the map collision type definition table.
            /// </summary>
            public const uint CollisionTypeDefinitionsTableSize = 0x80;

            /// <summary>
            /// The address of the map collision definition table.
            /// </summary>
            public const uint MapCollisionDefinitionTableAddress = Constants.Bank0BOffset | 0x0400;

            /// <summary>
            /// The size of the map collision definition table.
            /// </summary>
            public const uint MapCollisionDefinitionTableSize = 0x21;

            /// <summary>
            /// The size of a map collision definition entry.
            /// </summary>
            public const uint MapCollisionDefinitionSize = 0xC0;

            /// <summary>
            /// The address of the 16x16 map tileset pointer table.
            /// </summary>
            public const uint Map16x16TilesetPointerTableAddress = Constants.Bank0BOffset | 0x4000;

            /// <summary>
            /// The size of the 16x16 map tileset pointer table.
            /// </summary>
            public const uint Map16x16TilesetPointerTableSize = 0x21;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 0C.
        /// </summary>
        public static class Bank0C
        {
            /// <summary>
            /// The address of the map graphics base address table.
            /// </summary>
            public const uint MapGraphicalBaseAddressTableAddress = Constants.Bank0COffset | 0xE270;

            /// <summary>
            /// The size of the map graphics base address table.
            /// </summary>
            public const uint MapGraphicalBaseAddressTableSize = 0x04;

            /// <summary>
            /// The address of the map 8x8 tileset pointer table.
            /// </summary>
            public const uint Map8x8TilesetPointerTableAddress = Constants.Bank0COffset | 0xE27E;

            /// <summary>
            /// The size of the map 8x8 tileset pointer table.
            /// </summary>
            public const uint Map8x8TilesetPointerTableSize = 0x41;

            public const byte FirstValidMapPaletteIndex = 0x18;

            /// <summary>
            /// The address of the map palette set table.
            /// </summary>
            public const int MapPaletteSetTableAddress = Constants.Bank0COffset | 0x8000;

            /// <summary>
            /// The size of the map palette set table.
            /// </summary>
            public const int MapPaletteSetTableSize = 0x78;

            /// <summary>
            /// The number of palettes in a map palette set.
            /// </summary>
            public const int MapPalettesPerSet = 0x07;

            /// <summary>
            /// The number of colors of colors a palette has in a map palette set.
            /// </summary>
            public const int MapColorsPerPaletteInSet = 0x0F;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 0D.
        /// </summary>
        public static class Bank0D
        {
            /// <summary>
            /// The address of the map piece index maximum table.
            /// </summary>
            public const uint MapPieceIndexMaximumTableAddress = Constants.Bank0DOffset | 0x0000;

            /// <summary>
            /// The size of the map piece index maximum table.
            /// </summary>
            public const uint MapPieceIndexMaximumTableSize = 0x03;

            /// <summary>
            /// The address of the map piece pointer table.
            /// </summary>
            public const uint MapPiecePointerTableAddress = Constants.Bank0DOffset | 0x0006;

            /// <summary>
            /// The size of the map piece pointer table.
            /// </summary>
            public const uint MapPiecePointerTableSize = 0x387; // Ones-based indexing.
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 10.
        /// </summary>
        public static class Bank10
        {
            public const uint SpriteGraphicsTableAddress = Constants.Bank10Offset | 0x0000;
            public const uint SpriteGraphicsTableSize = 0xFF;

            public const uint WeaponDefinitionTableAddress = Constants.Bank10Offset | 0x1000;
            public const uint WeaponDefinitionTableSize = 0x100;

            public const uint ArmorDefinitionTableAddress = Constants.Bank10Offset | 0x3ED0;
            public const uint ArmorDefinitionTableSize = 0x40;

            public const uint ItemDefinitionTableAddress = Constants.Bank10Offset | 0x4150;
            public const uint ItemDefinitionTableSize = 0x0C;

            public const uint SpriteStatisticsTableAddress = Constants.Bank10Offset | 0x1C00;
            public const uint SpriteStatisticsTableSize = 0x80;
            public const uint SpriteStatisticsTableRowSize = 0x1D;

            public const uint BossStatTableAddress = Constants.Bank10Offset | 0x25DB;//

            public const uint EnemyLootTableAddress = Constants.Bank10Offset | 0x3A50;
            public const uint EnemyLootTableAddressSize = 0x80;

            public const uint RandiStatTableAddress = Constants.Bank10Offset | 0x4210;
            public const uint RandiStatTableAddressSize = 0x63;

            public const uint PurimStatTableAddress = Constants.Bank10Offset | 0x4528;
            public const uint PurimStatTableAddressSize = 0x63;

            public const uint PopoieStatTableAddress = Constants.Bank10Offset | 0x4840;
            public const uint PopoieStatTableAddressSize = 0x63;

            public const uint ExperiencePerLevelTableAddress = Constants.Bank10Offset | 0x4B58;
            public const uint ExperiencePerLevelTableAddressSize = 0x63;

            public const uint BossSkillGraphicDefinitionIndexTableAddress = Constants.Bank10Offset | 0xBAC0;
            public const uint BossSkillGraphicDefinitionIndexTableSize = 0x1F;

            public const uint BossSkillGraphicDefinitionTableAddress = Constants.Bank10Offset | 0xBADF;
            public const uint BossSkillGraphicIndexsPerSet = 0x04;
            public const byte BossSkillGraphicInvalidIndex = 0xFF;

            public const uint BossSkillGraphicsPointerTableAddress = Constants.Bank10Offset | 0xBB0C;
            public const uint BossSkillGraphicsPointerTableSize = 0x0C;
            public const ushort BossSkillGraphicsSize = 0x0400;

            /// <summary>
            /// The address of the boss palette pointer table.
            /// </summary>
            public const uint BossPalettePointerTableAddress = Constants.Bank10Offset | 0xBB24;

            /// <summary>
            /// The number of rows in the boss palette pointer table.
            /// </summary>
            public const uint BossPalettePointerTableSize = 0x88;

            /// <summary>
            /// The address of the boss graphics pointer table.
            /// </summary>
            public const uint BossGraphicsPointerTableAddress = Constants.Bank10Offset | 0xBC52;

            /// <summary>
            /// The number of rows in the boss graphics pointer table.
            /// </summary>
            public const byte BossGraphicsPointerTableSize = 0x2E;

            public const uint BossWeaponTableAddress = Constants.Bank10Offset | 0xBDC1;
            public const uint BossWeaponTableSize = 0x73;
            public const uint BossWeaponTableRowSize = 0x07;

            public const uint BossStateMachineSetupPointerTableAddress = Constants.Bank10Offset | 0xBC34;
            public const uint BossStateMachineSetupPointerTableSize = 0x0F;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 12.
        /// </summary>
        public static class Bank12
        {
            public const uint RingIconGraphicsTableAddress = Constants.Bank12Offset | 0x8400;
            public const uint RingIconGraphicsTableSize = 0xB0;

            public const uint RingIconPaletteTableAddress = Constants.Bank12Offset | 0xC900;
            public const uint RingIconPaletteTableSize = 0xBF;
            public const uint RingIconColorsPerPalette = 0x05;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 18.
        /// </summary>
        public static class Bank18
        {
            public const uint ConsumableItemPriceTableAddress = Constants.Bank18Offset | 0xFB9C;
            public const int ConsumableItemPriceTableSize = 0x0C;

            public const uint HelmetPriceTableAddress = Constants.Bank18Offset | 0xFBB4;
            public const int HelmetPriceTableSize = 0x15;

            public const uint ArmorPriceTableAddress = Constants.Bank18Offset | 0xFBDE;
            public const int ArmorPriceTableSize = 0x15;

            public const uint AccessoryPriceTableAddress = Constants.Bank18Offset | 0xFC08;
            public const int AccessoryPriceTableSize = 0x15;

            public const uint WeaponUpgradePriceTableAddress = Constants.Bank18Offset | 0xFCFB;
            public const int WeaponUpgradePriceTableSize = 0x09;

            public const uint ShopPointerTableAddress = Constants.Bank18Offset | 0xFC32;
            public const int ShopPointerTableSize = 0x10;

            public const byte ShopTableEndMarker = 0xFF;
            public const byte ShopMaxSize = 0x0D;
            public const byte ShopHelmetIndexOffset = 0x7B;
            public const byte ShopArmorIndexOffset = 0x90;
            public const byte ShopAccessoryIndexOffset = 0xA5;
            public const byte ShopConsumableIndexOffset = 0xBA;

            public const uint RingIconDefinitionOffsetTableAddress = Constants.Bank18Offset | 0xFD60;
            public const uint RingIconDefinitionOffsetTableSize = 0x06;

            public const int RingIconItemIconCount = 0x0C;
            public const int RingIconWeaponIconCount = 0x48;
            public const int RingIconHelmetIconCount = 0x15;
            public const int RingIconArmorIconCount = 0x15;
            public const int RingIconAccessoryIconCount = 0x15;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 19.
        /// </summary>
        public static class Bank19
        {
            public const uint ShopThankYouMessageAddress = Constants.Bank19Offset | 0xFE20;
            public const uint ShopNotEnoughMessageAddress = Constants.Bank19Offset | 0xFE2D;
            public const uint ShopCantCarryMessageAddress = Constants.Bank19Offset | 0xFE49;
            public const uint ShopNotInterestedMessageAddress = Constants.Bank19Offset | 0xFE65;
            public const uint ShopForgeItMessageAddress = Constants.Bank19Offset | 0xFE7E;
            public const uint ShopNotEnoughMoneyMessageAddress = Constants.Bank19Offset | 0xFE96;
            public const uint ShopMoreCrystalOrbMessageAddress = Constants.Bank19Offset | 0xFEB5;
            public const uint ShopForgedToBestMessageAddress = Constants.Bank19Offset | 0xFED1;
            public const uint ShopOkayMessageAddress = Constants.Bank19Offset | 0xFEEC;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 1B.
        /// </summary>
        public static class Bank1B
        {
            public const uint BossAnimationPointerTableAddress = Constants.Bank1BOffset | 0xF9A8;
            public const uint BossAnimationPointerTableSize = 0x17C;
        }

        /// <summary>
        /// Contains addresses, offsets, and other important values in Bank 1C.
        /// </summary>
        public static class Bank1C
        {
            public const uint BossSkillAIPointerTableAddress = Constants.Bank1COffset | 0xE683;
            public const uint BossSkillAIPointerTableSize = 0x1F;

            public const uint BossSkillPaletteIdTableAddress = Constants.Bank1COffset | 0xE6C1;
            public const uint BossSkillPaletteIdTableSize = 0x1F;

            public const uint BossSkillBossAnimationIdTableAddress = Constants.Bank1COffset | 0xE6FF;
            public const uint BossSkillBossAnimationIdTableSize = 0x1F;

            public const uint BossSkillPlayerAnimationIdTableAddress = Constants.Bank1COffset | 0xE73D;
            public const uint BossSkillPlayerAnimationIdTableSize = 0x1F;

            public const uint BossSkillBossSoundEffectIdTableAddress = Constants.Bank1COffset | 0xE77B;
            public const uint BossSkillBossSoundEffectIdTableSize = 0x1F;

            public const uint BossSkillPlayerSoundEffectIdTableAddress = Constants.Bank1COffset | 0xE7B9;
            public const uint BossSkillPlayerSoundEffectIdTableSize = 0x1F;

            public const uint KettleKinLegAnimationIdSyncTable = Constants.Bank1COffset | 0xE0D0;

            public const uint DarkLichUndergroundHandMovementPointerTable = Constants.Bank1COffset | 0xE479;
            public const uint DarkLichBodyAnimationIdSyncTable = Constants.Bank1COffset | 0xE4A9;
            public const byte DarkLichBodyAnimationIdSyncTableSize = 0x28;
        }
    }
}