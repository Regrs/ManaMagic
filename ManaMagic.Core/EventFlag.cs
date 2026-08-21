using System.ComponentModel;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core
{
    public enum EventFlag : byte
    {
        [FieldDisplayName("FLAG_HUD_ENABLED")]
        HeadsUpDisplayEnabled = 0x00,
        [FieldDisplayName("FLAG_COLLISON_TOGGLE")]
        CollisionToggle = 0x01,
        [FieldDisplayName("FLAG_GRAND_PALACE_STATE")]
        GrandPalaceState = 0x02,
        [FieldDisplayName("FLAG_ON_ROPE_TILE")]
        OnRopeTile = 0x03,
        [FieldDisplayName("FLAG_PURIM_DYLUCK_SEARCH")]
        PurimDyLuckSearchState = 0x04,
        [FieldDisplayName("FLAG_DWARF_LIAR")]
        DwarfLiar = 0x05,
        [FieldDisplayName("FLAG_UNUSED_06")]
        Unused06 = 0x06,
        [FieldDisplayName("FLAG_UNUSED_07")]
        Unused07 = 0x07,
        [FieldDisplayName("FLAG_UNUSED_08")]
        Unused08 = 0x08,
        [FieldDisplayName("FLAG_POTOS_ELDER")]
        PotosElder = 0x09,
        [FieldDisplayName("EVENT_FLAG_0A")]
        Flag0A = 0x0A,
        [FieldDisplayName("FLAG_DEATH_STATE")]
        DeathState = 0x0B,
        [FieldDisplayName("FLAG_RANDI_IN_PARTY")]
        RandiInParty = 0x0C,
        [FieldDisplayName("FLAG_PURIM_IN_PARTY")]
        PurimInParty = 0x0D,
        [FieldDisplayName("FLAG_POPOIE_IN_PARTY")]
        PopoieInParty = 0x0E,
        [FieldDisplayName("FLAG_ADD_ITEM_RESULT")]
        AddItemResult = 0x0F,
        [FieldDisplayName("FLAG_POTOS_EVENT_STATE")]
        PotosEventState = 0x10,
        [FieldDisplayName("EVENT_FLAG_11")]
        Flag11 = 0x11, // Dylucks Trooops
        [FieldDisplayName("EVENT_FLAG_12")]
        Flag12 = 0x12, // Pandora
        [FieldDisplayName("EVENT_FLAG_13")]
        Flag13 = 0x13, // Purim
        [FieldDisplayName("FLAG_DWARF_VILLAGE_EVENT_STATE")]
        DwarfVillageEventState = 0x14,
        [FieldDisplayName("FLAG_DWARF_VILLAGE_SIDESHOW")]
        DwarfVillageSlideShow = 0x15,
        [FieldDisplayName("FLAG_PURIM_RECRUITMENT")]
        PurimRecruitment = 0x16,
        [FieldDisplayName("FLAG_WITCH_CASTLE_EVENT_STATE")]
        WitchCastleEventState = 0x17,
        [FieldDisplayName("FLAG_EARTH_PALACE_EVENT_STATE")]
        EarthPalaceEventState = 0x18,
        [FieldDisplayName("FLAG_UNUSED_19")]
        Unused19 = 0x19,
        [FieldDisplayName("FLAG_PANDORA_RUINS_EVENT_STATE")]
        PandoraRuinsEventState = 0x1A,
        [FieldDisplayName("FLAG_WATER_SEED_STOLEN")]
        WaterSeedStolen = 0x1B,
        [FieldDisplayName("FLAG_EARTH_PALACE_LEFT_SWITCH")]
        EarthPalaceLeftSwitch = 0x1C,
        [FieldDisplayName("FLAG_EARTH_PALACE_RIGHT_SWITCH")]
        EarthPalaceRightSwitch = 0x1D,
        [FieldDisplayName("FLAG_WATER_PALACE_TAKEOVER")]
        WaterPalaceTakeover = 0x1E,
        [FieldDisplayName("FLAG_UNUSED_1F")]
        Unused1F = 0x1F,
        [FieldDisplayName("FLAG_UPPERLANDS_FOREST_MOOGLE_STATE")]
        UpperlandsForestMoogleState = 0x20,
        [FieldDisplayName("FLAG_UPPERLANDS_FOREST_POPOIE")]
        UpperlandsForestPopoie = 0x21,
        [FieldDisplayName("FLAG_SPRITE_VILLAGE_STATE")]
        SpriteVillageState = 0x22,
        [FieldDisplayName("FLAG_UNUSED_23")]
        Unused23 = 0x23,
        [FieldDisplayName("FLAG_MATANGO_EVENT_STATE")]
        MatangoEventState = 0x24,
        [FieldDisplayName("FLAG_ICE_PALACE_TONPOLE_STATE")]
        IcePalaceTonpoleState = 0x25,
        [FieldDisplayName("FLAG_ICE_PALACE_EVENT_STATE")]
        IcePalaceEventState = 0x26,
        [FieldDisplayName("FLAG_SCORPION_VILLAGE_EVENT_STATE")]
        ScorpionVillageEventState = 0x27,
        [FieldDisplayName("EVENT_FLAG_28")]
        Flag28 = 0x28,
        [FieldDisplayName("FLAG_SEA_HARE_TAIL_EVENT_STATE")]
        SeaHareTailEventState = 0x29,
        [FieldDisplayName("FLAG_UNUSED_2A")]
        Unused2A = 0x2A,
        [FieldDisplayName("FLAG_FIRE_PALACE_ORB_PUZZLE")]
        FirePalaceOrbPuzzle = 0x2B,
        [FieldDisplayName("FLAG_FIRE_PALACE_EVENT_STATE")]
        FirePalaceEventState = 0x2C,
        [FieldDisplayName("EVENT_FLAG_2D")]
        Flag2D = 0x2D,
        [FieldDisplayName("EVENT_FLAG_2E")]
        Flag2E = 0x2E,
        [FieldDisplayName("EVENT_FLAG_2F")]
        Flag2F = 0x2F,
        [FieldDisplayName("FLAG_MAD_MARA_STATE")]
        MadMaraState = 0x30,
        [FieldDisplayName("FLAG_RESISTANCE_PASSWORD_WOMAN")]
        ResistancePasswordWoman = 0x31,
        [FieldDisplayName("FLAG_NORTHTOWN_RESISTANCE_STATE")]
        NorthTownResistanceState = 0x32,
        [FieldDisplayName("FLAG_NORTH_SOUTH_TOWN_NPC_STATE")]
        NorthAndSouthTownNpcState = 0x33,
        [FieldDisplayName("FLAG_VANDOLE_TRAP_STATE")]
        VandoleTrapState = 0x34,
        [FieldDisplayName("EVENT_FLAG_35")]
        Flag35 = 0x35,
        [FieldDisplayName("FLAG_DESERT_CRASH_LANDING")]
        DesertCrashLanding = 0x36,
        [FieldDisplayName("FLAG_LIGHT_PALACE_EVENT_STATE")]
        LightPalaceEventState = 0x37,
        [FieldDisplayName("FLAG_DARK_PALACE_MAP_STATE")]
        DarkPalaceMapState = 0x38,
        [FieldDisplayName("FLAG_DARK_PALACE_MAIN_GATE_STATE")]
        DarkPalaceMainGateState = 0x39,
        [FieldDisplayName("FLAG_PHANNA_ILLNESS_STATE")]
        PhannaIllnessState = 0x3A,
        [FieldDisplayName("FLAG_LUNAR_PALACE_EVENT_STATE")]
        LunarPalaceEventState = 0x3B,
        [FieldDisplayName("FLAG_TASNICA_EVENT_STATE")]
        TasnicaEventState = 0x3C,
        [FieldDisplayName("FLAG_UNUSED_3D")]
        Unused3D = 0x3D,
        [FieldDisplayName("FLAG_MANDALA_NPC_STATE")]
        MandalaNpcState = 0x3E,
        [FieldDisplayName("FLAG_TRIAL_OF_COURAGE_STATE")]
        TrialOfCourageState = 0x3F,
        [FieldDisplayName("FLAG_HYDRA_ENCOUNTER_STATE")]
        HydraEncounterState = 0x40,
        [FieldDisplayName("EVENT_FLAG_41")]
        Flag41 = 0x41,
        [FieldDisplayName("EVENT_FLAG_42")]
        Flag42 = 0x42,
        [FieldDisplayName("EVENT_FLAG_43")]
        Flag43 = 0x43,
        [FieldDisplayName("EVENT_FLAG_44")]
        Flag44 = 0x44,
        [FieldDisplayName("EVENT_FLAG_45")]
        Flag45 = 0x45,
        [FieldDisplayName("EVENT_FLAG_46")]
        Flag46 = 0x46,
        [FieldDisplayName("EVENT_FLAG_47")]
        Flag47 = 0x47,
        [FieldDisplayName("EVENT_FLAG_48")]
        Flag48 = 0x48,
        [FieldDisplayName("EVENT_FLAG_49")]
        Flag49 = 0x49,
        [FieldDisplayName("FLAG_GRAND_PALACE_LEFT_BRIDGE_STATE")]
        GrandPalaceLeftBridgeState = 0x4A,
        [FieldDisplayName("FLAG_GRAND_PALACE_CENTER_BRIDGE_STATE")]
        GrandPalaceCenterBridgeState = 0x4B,
        [FieldDisplayName("FLAG_GRAND_PALACE_RIGHT_BRIDGE_STATE")]
        GrandPalaceRightBridgeState = 0x4C,
        [FieldDisplayName("FLAG_PHANNA_APOLOGY")]
        PhannaApology = 0x4D,
        [FieldDisplayName("FLAG_MANA_FORTRESS_EVENT_STATE")]
        ManaFortressEventState = 0x4E,
        [FieldDisplayName("FLAG_MANA_FORTRESS_BRIDGE_STATE")]
        ManaFortressBridgeState = 0x4F,
        [FieldDisplayName("EVENT_FLAG_50")]
        Flag50 = 0x50,
        [FieldDisplayName("FLAG_UNUSED_TOGGLE_DOOR_01")]
        UnusedToggleDoor01 = 0x51,
        [FieldDisplayName("FLAG_UNUSED_TOGGLE_DOOR_02")]
        UnusedToggleDoor02 = 0x52,
        [FieldDisplayName("FLAG_WITCH_CASTLE_SLIDING_WALL_STATE")]
        WitchCastleSlidingWallState = 0x53,
        [FieldDisplayName("EVENT_FLAG_54")]
        Flag54 = 0x54,
        [FieldDisplayName("EVENT_FLAG_55")]
        Flag55 = 0x55,
        [FieldDisplayName("EVENT_FLAG_56")]
        Flag56 = 0x56,
        [FieldDisplayName("FLAG_TRAVEL_CANNON_FIRED")]
        TravelCannonFired = 0x57,
        [FieldDisplayName("FLAG_POPOIE_GP_DONATION")]
        PopoieGoldDonation = 0x58,
        [FieldDisplayName("FLAG_POST_JABBERWOCKY_JEMA_DIALOG")]
        PostJabberwockyJemaDialog = 0x59,
        [FieldDisplayName("FLAG_RESISTANCE_PASSWORD")]
        ResistancePassword = 0x5A,
        [FieldDisplayName("FLAG_LUKA_JAIL_DIALOG")]
        LukaJailDialog = 0x5B,
        [FieldDisplayName("FLAG_MIDGE_MALLET")]
        MidgeMallet = 0x5C,
        [FieldDisplayName("FLAG_PURE_LAND_ENTRANCE_DIALOG")]
        PureLandEntranceDialog = 0x5D,
        [FieldDisplayName("FLAG_HAUNTED_FOREST_SKULL_PILLAR_STATE")]
        HauntedForestSkullPillarState = 0x5E,
        [FieldDisplayName("FLAG_HAUNTED_FOREST_WALL_STATE")]
        HauntedForestWallState = 0x5F,
        [FieldDisplayName("FLAG_GAIAS_NAVEL_MAP_STATE")]
        GaiasNavelMapState = 0x60,
        [FieldDisplayName("FLAG_GAIAS_NAVEL_EVENT_STATE")]
        GaiasNavelEventState = 0x61,
        [FieldDisplayName("FLAG_UNUSED_62")]
        Unused62 = 0x62,
        [FieldDisplayName("FLAG_UNUSED_63")]
        Unused63 = 0x63,
        [FieldDisplayName("FLAG_UNUSED_64")]
        Unused64 = 0x64,
        [FieldDisplayName("FLAG_UNUSED_65")]
        Unused65 = 0x65,
        [FieldDisplayName("FLAG_UNUSED_66")]
        Unused66 = 0x66,
        [FieldDisplayName("FLAG_UNUSED_67")]
        Unused67 = 0x67,
        [FieldDisplayName("FLAG_PURIM_ENDING_STATE")]
        PurimEndingState = 0x68,
        [FieldDisplayName("FLAG_UNUSED_69")]
        Unused69 = 0x69,
        [FieldDisplayName("FLAG_UNUSED_6A")]
        Unused6A = 0x6A,
        [FieldDisplayName("FLAG_UNUSED_6B")]
        Unused6B = 0x6B,
        [FieldDisplayName("FLAG_UNUSED_6C")]
        Unused6C = 0x6C,
        [FieldDisplayName("FLAG_UNUSED_6D")]
        Unused6D = 0x6D,
        [FieldDisplayName("FLAG_UNUSED_6E")]
        Unused6E = 0x6E,
        [FieldDisplayName("FLAG_TURTLE_ISLAND_NPC_STATE")]
        TurtleIslandNpcState = 0x6F,
        [FieldDisplayName("FLAG_RANDI_DEAD_ON_BOSS_DEFEAT")]
        RandiDeadOnBossDefeat = 0x70,
        [FieldDisplayName("FLAG_PURIM_DEAD_ON_BOSS_DEFEAT")]
        PurimDeadOnBossDefeat = 0x71,
        [FieldDisplayName("FLAG_POPOIE_DEAD_ON_BOSS_DEFEAT")]
        PopoieDeadOnBossDefeat = 0x72,
        [FieldDisplayName("FLAG_UNUSED_73")]
        Unused73 = 0x73,
        [FieldDisplayName("FLAG_UNUSED_74")]
        Unused74 = 0x74,
        [FieldDisplayName("FLAG_UNUSED_75")]
        Unused75 = 0x75,
        [FieldDisplayName("FLAG_UNUSED_76")]
        Unused76 = 0x76,
        [FieldDisplayName("FLAG_UNUSED_77")]
        Unused77 = 0x77,
        [FieldDisplayName("FLAG_UNUSED_78")]
        Unused78 = 0x78,
        [FieldDisplayName("FLAG_UNUSED_79")]
        Unused79 = 0x79,
        [FieldDisplayName("FLAG_UNUSED_7A")]
        Unused7A = 0x7A,
        [FieldDisplayName("FLAG_UNUSED_7B")]
        Unused7B = 0x7B,
        [FieldDisplayName("FLAG_UNUSED_7C")]
        Unused7C = 0x7C,
        [FieldDisplayName("FLAG_UNUSED_7D")]
        Unused7D = 0x7D,
        [FieldDisplayName("FLAG_UNUSED_7E")]
        Unused7E = 0x7E,
        [FieldDisplayName("FLAG_MANA_SWORD_REVIVED")]
        ManaSwordRevived = 0x7F,
        [FieldDisplayName("FLAG_UNUSED_80")]
        Unused80 = 0x80,
        [FieldDisplayName("FLAG_UNUSED_81")]
        Unused81 = 0x81,
        [FieldDisplayName("FLAG_UNUSED_82")]
        Unused82 = 0x82,
        [FieldDisplayName("FLAG_UNUSED_83")]
        Unused83 = 0x83,
        [FieldDisplayName("FLAG_UNUSED_84")]
        Unused84 = 0x84,
        [FieldDisplayName("FLAG_UNUSED_85")]
        Unused85 = 0x85,
        [FieldDisplayName("FLAG_UNUSED_86")]
        Unused86 = 0x86,
        [FieldDisplayName("FLAG_UNUSED_87")]
        Unused87 = 0x87,
        [FieldDisplayName("FLAG_UNUSED_88")]
        Unused88 = 0x88,
        [FieldDisplayName("FLAG_UNUSED_89")]
        Unused89 = 0x89,
        [FieldDisplayName("FLAG_UNUSED_8A")]
        Unused8A = 0x8A,
        [FieldDisplayName("FLAG_UNUSED_8B")]
        Unused8B = 0x8B,
        [FieldDisplayName("FLAG_UNUSED_8C")]
        Unused8C = 0x8C,
        [FieldDisplayName("FLAG_UNUSED_8D")]
        Unused8D = 0x8D,
        [FieldDisplayName("FLAG_UNUSED_8E")]
        Unused8E = 0x8E,
        [FieldDisplayName("FLAG_UNUSED_8F")]
        Unused8F = 0x8F,
        [FieldDisplayName("FLAG_WATER_SEED_STATE")]
        WaterSeedState = 0x90,
        [FieldDisplayName("FLAG_EARTH_SEED_STATE")]
        EarthSeedState = 0x91,
        [FieldDisplayName("FLAG_WIND_SEED_STATE")]
        WindSeedState = 0x92,
        [FieldDisplayName("FLAG_FIRE_SEED_STATE")]
        FireSeedState = 0x93,
        [FieldDisplayName("FLAG_LIGHT_SEED_STATE")]
        LightSeedState = 0x94,
        [FieldDisplayName("FLAG_SHADOW_SEED_STATE")]
        ShadowSeedState = 0x95,
        [FieldDisplayName("FLAG_LUNAR_SEED_STATE")]
        LunarSeedState = 0x96,
        [FieldDisplayName("FLAG_TREE_SEED_STATE")]
        TreeSeedState = 0x97,
        [FieldDisplayName("FLAG_EARTH_CRYSTAL_ORB_SPELL_ID")]
        EarthCrystalOrbSpellId = 0x98,
        [FieldDisplayName("FLAG_WATER_CRYSTAL_ORB_SPELL_ID")]
        WaterCrystalOrbSpellId = 0x99,
        [FieldDisplayName("FLAG_FIRE_CRYSTAL_ORB_SPELL_ID")]
        FireCrystalOrbSpellId = 0x9A,
        [FieldDisplayName("FLAG_WIND_CRYSTAL_ORB_SPELL_ID")]
        WindCrystalOrbSpellId = 0x9B,
        [FieldDisplayName("FLAG_LIGHT_CRYSTAL_ORB_SPELL_ID")]
        LightCrystalOrbSpellId = 0x9C,
        [FieldDisplayName("FLAG_SHADOW_CRYSTAL_ORB_SPELL_ID")]
        ShadowCrystalOrbSpellId = 0x9D,
        [FieldDisplayName("FLAG_LUNAR_CRYSTAL_ORB_SPELL_ID")]
        LunarCrystalOrbSpellId = 0x9E,
        [FieldDisplayName("FLAG_TREE_CRYSTAL_ORB_SPELL_ID")]
        TreeCrystalOrbSpellId = 0x9F,
        [FieldDisplayName("FLAG_UNUSED_A0")]
        UnusedA0 = 0xA0,
        [FieldDisplayName("FLAG_UNUSED_A1")]
        UnusedA1 = 0xA1,
        [FieldDisplayName("FLAG_UNUSED_A2")]
        UnusedA2 = 0xA2,
        [FieldDisplayName("FLAG_UNUSED_A3")]
        UnusedA3 = 0xA3,
        [FieldDisplayName("FLAG_GENERIC_PALACE_DOOR_01")]
        GenericPalaceDoor01 = 0xA4,
        [FieldDisplayName("FLAG_GENERIC_PALACE_DOOR_02")]
        GenericPalaceDoor02 = 0xA5,
        [FieldDisplayName("FLAG_GENERIC_PALACE_DOOR_03")]
        GenericPalaceDoor03 = 0xA6,
        [FieldDisplayName("FLAG_GENERIC_PALACE_DOOR_04")]
        GenericPalaceDoor04 = 0xA7,
        [FieldDisplayName("FLAG_GAINED_GNOME_POWERS")]
        GainedGnomePowers = 0xA8,
        [FieldDisplayName("FLAG_GAINED_UNDINE_POWERS")]
        GainedUndinePowers = 0xA9,
        [FieldDisplayName("FLAG_GAINED_SALAMANDO_POWERS")]
        GainedSalamandoPowers = 0xAA,
        [FieldDisplayName("FLAG_GAINED_SYLPHID_POWERS")]
        GainedSylphidPowers = 0xAB,
        [FieldDisplayName("FLAG_GAINED_LUMINA_POWERS")]
        GainedLuminaPowers = 0xAC,
        [FieldDisplayName("FLAG_GAINED_SHADE_POWERS")]
        GainedShadePowers = 0xAD,
        [FieldDisplayName("FLAG_GAINED_LUNA_POWERS")]
        GainedLunaPowers = 0xAE,
        [FieldDisplayName("FLAG_GAINED_DRYAD_POWERS")]
        GainedDryadPowers = 0xAF,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_GLOVE")]
        TotalUpgradesGlove = 0xB0,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_SWORD")]
        TotalUpgradesSword = 0xB1,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_AXE")]
        TotalUpgradesAxe = 0xB2,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_SPEAR")]
        TotalUpgradesSpear = 0xB3,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_WHIP")]
        TotalUpgradesWhip = 0xB4,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_BOW")]
        TotalUpgradesBow = 0xB5,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_BOOMERANG")]
        TotalUpgradesBoomerang = 0xB6,
        [FieldDisplayName("FLAG_TOTAL_UPGRADES_JAVELIN")]
        TotalUpgradesJavelin = 0xB7,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_GLOVE")]
        OrbsObtainedGlove = 0xB8,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_SWORD")]
        OrbsObtainedSword = 0xB9,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_AXE")]
        OrbsObtainedAxe = 0xBA,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_SPEAR")]
        OrbsObtainedSpear = 0xBB,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_WHIP")]
        OrbsObtainedWhip = 0xBC,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_BOW")]
        OrbsObtainedBow = 0xBD,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_BOOMERANG")]
        OrbsObtainedBoomerang = 0xBE,
        [FieldDisplayName("FLAG_ORBS_OBTAINED_JAVELIN")]
        OrbsObtainedJavelin = 0xBF,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_GLOVE")]
        MaxAllowedOrbsGlove = 0xC0,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_SWORD")]
        MaxAllowedOrbsSword = 0xC1,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_AXE")]
        MaxAllowedOrbsAxe = 0xC2,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_SPEAR")]
        MaxAllowedOrbsSpear = 0xC3,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_WHIP")]
        MaxAllowedOrbsWhip = 0xC4,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_BOW")]
        MaxAllowedOrbsBow = 0xC5,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_BOOMERANG")]
        MaxAllowedOrbsBoomerang = 0xC6,
        [FieldDisplayName("FLAG_MAX_ALLOWED_ORBS_JAVELIN")]
        MaxAllowedOrbsJavelin = 0xC7,
        [FieldDisplayName("FLAG_UNUSED_C8")]
        UnusedC8 = 0xC8,
        [FieldDisplayName("FLAG_UNUSED_C9")]
        UnusedC9 = 0xC9,
        [FieldDisplayName("FLAG_UNUSED_CA")]
        UnusedCA = 0xCA,
        [FieldDisplayName("FLAG_UNUSED_CB")]
        UnusedCB = 0xCB,
        [FieldDisplayName("FLAG_UNUSED_CC")]
        UnusedCC = 0xCC,
        [FieldDisplayName("FLAG_UNUSED_CD")]
        UnusedCD = 0xCD,
        [FieldDisplayName("FLAG_UNUSED_CE")]
        UnusedCE = 0xCE,
        [FieldDisplayName("FLAG_UNUSED_CF")]
        UnusedCF = 0xCF,
        [FieldDisplayName("FLAG_POTOS_50GP_CHEST")]
        Potos50GoldChest = 0xD0,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_01")]
        Unused50GoldChest01 = 0xD1,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_02")]
        Unused50GoldChest02 = 0xD2,
        [FieldDisplayName("FLAG_PANDORA_TREASURY_50GP_CHEST_01")]
        PandoraTreasury50GoldChest01 = 0xD3,
        [FieldDisplayName("FLAG_PANDORA_TREASURY_50GP_CHEST_02")]
        PandoraTreasury50GoldChest02 = 0xD4,
        [FieldDisplayName("FLAG_PANDORA_TREASURY_50GP_CHEST_03")]
        PandoraTreasury50GoldChest03 = 0xD5,
        [FieldDisplayName("FLAG_PANDORA_TREASURY_50GP_CHEST_04")]
        PandoraTreasury50GoldChest04 = 0xD6,
        [FieldDisplayName("FLAG_MAGIC_ROPE_CHEST")]
        MagicRopeChest = 0xD7,
        [FieldDisplayName("FLAG_WITCH_CASTLE_50GP_CHEST")]
        WitchCastle50GoldChest = 0xD8,
        [FieldDisplayName("FLAG_WHIP_CHEST")]
        WhipChest = 0xD9,
        [FieldDisplayName("FLAG_FIRE_PALACE_1000GP_CHEST_01")]
        FirePalace1000GoldChest01 = 0xDA,
        [FieldDisplayName("FLAG_FIRE_PALACE_1000GP_CHEST_02")]
        FirePalace1000GoldChest02 = 0xDB,
        [FieldDisplayName("FLAG_UNUSED_1000GP_CHEST")]
        Unused1000GoldChest = 0xDC,
        [FieldDisplayName("FLAG_EMPIRE_CASTLE_EVENT_STATE")]
        EmpireCastleEventState = 0xDD,
        [FieldDisplayName("FLAG_DARK_PALACE_1000GP_CHEST")]
        DarkPalace1000GoldChest = 0xDE,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_03")]
        Unused50GoldChest03 = 0xDF,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_04")]
        Unused50GoldChest04 = 0xE0,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_05")]
        Unused50GoldChest05 = 0xE1,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_06")]
        Unused50GoldChest06 = 0xE2,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_07")]
        Unused50GoldChest07 = 0xE3,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_08")]
        Unused50GoldChest08 = 0xE4,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_09")]
        Unused50GoldChest09 = 0xE5,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_0A")]
        Unused50GoldChest0A = 0xE6,
        [FieldDisplayName("FLAG_UNUSED_50GP_CHEST_0B")]
        Unused50GoldChest0B = 0xE7,
        [FieldDisplayName("FLAG_GRAND_PALACE_EARTH_CRYSTAL_ORB")]
        GrandPalaceEarthCrystalOrb = 0xE8,
        [FieldDisplayName("FLAG_GRAND_PALACE_WATER_CRYSTAL_ORB")]
        GrandPalaceWaterCrystalOrb = 0xE9,
        [FieldDisplayName("FLAG_GRAND_PALACE_WIND_CRYSTAL_ORB")]
        GrandPalaceWindCrystalOrb = 0xEA,
        [FieldDisplayName("FLAG_GRAND_PALACE_FIRE_CRYSTAL_ORB")]
        GrandPalaceFireCrystalOrb = 0xEB,
        [FieldDisplayName("FLAG_GRAND_PALACE_LIGHT_CRYSTAL_ORB")]
        GrandPalaceLightCrystalOrb = 0xEC,
        [FieldDisplayName("FLAG_GRAND_PALACE_SHADOW_CRYSTAL_ORB")]
        GrandPalaceShadowCrystalOrb = 0xED,
        [FieldDisplayName("FLAG_GRAND_PALACE_LUNAR_CRYSTAL_ORB")]
        GrandPalaceLunarCrystalOrb = 0xEE,
        [FieldDisplayName("FLAG_GRAND_PALACE_TREE_CRYSTAL_ORB")]
        GrandPalaceTreeCrystalOrb = 0xEF,
        [FieldDisplayName("FLAG_UNUSED_F0")]
        UnusedF0 = 0xF0,
        [FieldDisplayName("FLAG_UNUSED_F1")]
        UnusedF1 = 0xF1,
        [FieldDisplayName("FLAG_UNUSED_F2")]
        UnusedF2 = 0xF2,
        [FieldDisplayName("FLAG_UNUSED_F3")]
        UnusedF3 = 0xF3,
        [FieldDisplayName("FLAG_UNUSED_F4")]
        UnusedF4 = 0xF4,
        [FieldDisplayName("FLAG_DEBUG_COORDINATE_DISPLAY")]
        DebugCoordinateDisplay = 0xF5,
        [FieldDisplayName("FLAG_DEBUG_CRASH_GAME")]
        DebugCrashGame = 0xF6,
        [FieldDisplayName("FLAG_SNOWFALL_ENABLED")]
        SnowfallEnabled = 0xF7,
        [FieldDisplayName("FLAG_DIALOG_WINDOW_VISIBILITY")]
        DialogWindowVisibility = 0xF8,
        [FieldDisplayName("FLAG_INN_PRICE_INDEX")]
        InnPriceIndex = 0xF9,
        [FieldDisplayName("FLAG_ACTIVE_SHOP_RING_INDEX")]
        ActiveShopRingIndex = 0xFA,
        [FieldDisplayName("FLAG_ENEMY_SEALED_MANA_SEEDS")]
        EnemySealedManaSeeds = 0xFB,
        [FieldDisplayName("FLAG_SEALED_MANA_SEEDS")]
        SealedManaSeeds = 0xFC,
        [FieldDisplayName("FLAG_SAVE_SCREEN_LOCATION_INDEX_01")]
        SaveScreenLocationIndex01 = 0xFD,
        [FieldDisplayName("FLAG_SAVE_SCREEN_LOCATION_INDEX_02")]
        SaveScreenLocationIndex02 = 0xFE,
        [FieldDisplayName("FLAG_BOSS_AL_DISABLED")]
        BossAIDisabled = 0xFF,
    }
}