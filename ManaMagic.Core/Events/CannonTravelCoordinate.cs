using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Events
{
    public enum CannonTravelCoordinate : byte
    {
        MatangoMountainsToPureLandOcean1 = 0x00,
        MatangoMountainsToPureLandOcean2 = 0x01,
        MatangoMountainsToPureLandOcean3 = 0x02,
        MatangoMountainsToPureLandOcean4 = 0x03,
        MatangoMountainsToPureLandOcean5 = 0x04,
        KakkaraBorderToPureLandOcean = 0x05,
        MatangoMountainsToPureLandOcean6 = 0x06,
        MatangoMountainsToPureLandOcean7 = 0x07,
        KakkaraDesertToPureLandOcean1 = 0x08,
        KakkaraOceanToOcean1 = 0x09,
        KakkaraDesertToPureLandOcean2 = 0x0A,
        KakkaraOceanToOcean2 = 0x0B,
        KakkaraOceanToOcean3 = 0x0C,
        KakkaraOceanToIceCountryOcean1 = 0x0D,
        KakkaraOceanToOcean4 = 0x0E,
        KakkaraOceanToIceCountryOcean2 = 0x0F,
        GoldIsleOceanToPureLandOcean1 = 0x10,
        GoldIsleOceanToPureLandOcean2 = 0x11,
        GoldIsleOceanToOcean = 0x12,
        GoldIsleOceanToPureLandOcean3 = 0x13,
        KakkaraOceanToPureLandMountains1 = 0x14,
        KakkaraOceanToPureLandMountains2 = 0x15,
        OceanToPureLandOcean = 0x16,
        KakkaraDesertToPureLandOcean3 = 0x17,
        MoonSeaToPureLandOcean = 0x18,
        KakkaraCannonTravelToOcean = 0x19,
        KakkaraOceanToPureLandOcean = 0x1A,
        KakkaraOceanToOcean5 = 0x1B,
        KakkaraOceanToIceCountryOcean3 = 0x1C,
        KakkaraOceanToIceCountryOcean4 = 0x1D,
        KakkaraOceanToOcean6 = 0x1E,
        KakkaraOceanToIceCountryOcean5 = 0x1F,

        // Made it this far down the list before finding one that is actually used.
        [FieldDisplayName("Potos Cannon Travel To Water Palace")]
        PotosCannonTravelToWaterPalace = 0x20,
        [FieldDisplayName("Potos Cannon Travel To Gaia's Navel")]
        PotosCannonTravelToGaiasNavel = 0x21,
        [FieldDisplayName("Gaia's Navel Cannon Travel To Pandora")]
        GaiasNavelCannonTravelToPandora = 0x22,
        [FieldDisplayName("Potos Cannon Travel To Great Forest")]
        PotosCannonTravelToGreatForest = 0x23,
        [FieldDisplayName("Matango Cannon Travel To Kakkara Desert")]
        MatangoCannonTravelToKakkaraDesert = 0x24,
        [FieldDisplayName("Matango Cannon Travel To Todo Village")]
        MatangoCannonTravelToTodoVillage = 0x25,
        [FieldDisplayName("Great Forest Cannon Travel To Potos Cannon Travel")]
        GreatForestCannonTravelToPotosCannonTravel = 0x26,
        [FieldDisplayName("Matango Cannon Travel To Potos Cannon Travel")]
        MatangoCannonTravelToPotosCannonTravel = 0x27,
        MatangoCannonTravelToMatangoCannonTravel = 0x28, // God I love this one. lol "Sorry, not enough gunpowder."
        [FieldDisplayName("Gaia's Navel Cannon Travel To Water Palace")]
        GaiasNavelCannonTravelToWaterPalace = 0x29,
        [FieldDisplayName("Great Forest Cannon Travel To Gaia's Navel")]
        GreatForestCannonTravelToGaiasNavel = 0x2A,
        [FieldDisplayName("Great Forest Cannon Travel To Pandora")]
        GreatForestCannonTravelToPandora = 0x2B,
        [FieldDisplayName("Ice Country Cannon Travel To Matango Castle")]
        IceCountryCannonTravelToMatangoCastle = 0x2C,
        [FieldDisplayName("Ice Country Cannon Travel To Kakkara Village")]
        IceCountryCannonTravelToKakkaraVillage = 0x2D,
        [FieldDisplayName("Kakkara Village To Ice Country Cannon Travel")]
        KakkaraVillageToIceCountryCannonTravel = 0x2E,
        [FieldDisplayName("Kakkara Village To Iodo Village")]
        KakkaraVillageToTodoVillage = 0x2F,
        [FieldDisplayName("Kakkara Village To Southtown Cannon Travel")]
        KakkaraVillageToSouthtownCannonTravel = 0x30,
        [FieldDisplayName("Southtown Cannon Travel To Kakkara Village")]
        SouthtownCannonTravelToKakkaraVillage = 0x31,
        [FieldDisplayName("Matango Cannon Travel To Kakkara Village")]
        MatangoCannonTravelToKakkaraVillage = 0x32,

        // Back to unused ones.
        EmpireNorthtownPenisulaToKakkaraDesert = 0x33, // Interesting that this one is valid.
        OceantoKakkaraDesert1 = 0x34,
        PureLandOceanToKakkaraDesertStarField = 0x35,
        OceantoKakkaraDesert2 = 0x36,
        PureLandOceanToKakkaraDesert = 0x37,
        OceanToKakkaraDesertStarField = 0x38,
        PureLandMountainsToKakkaraOcean = 0x39,
        OceantoKakkaraDesert3 = 0x3A,
        PureLandMountainsToKakkaraODesert = 0x3B,
        PureLandMountainsToOcean = 0x3C,
        OceanToGoldIsleOcean = 0x3D,

        // Special
        [FieldDisplayName("Mana Beast Escape")]
        ManaBeastEscape = 0x3E,
        Intro = 0x3F,
    }
}