using System.Collections.Generic;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    public static partial class ManaMetadata
    {
        private static readonly IReadOnlyDictionary<ushort, string> DoorNameStrings = new Dictionary<ushort, string>()
        {
            { 0x0000, "Potos Fields - Waterfall Log Bridge" },

            { 0x0004, "Goblin Village" },
            { 0x0005, "Potos Village - Cannon Travel Center" },

            { 0x0009, "Water Palace: Left Outer Door" },
            { 0x000A, "Water Palace: Right Outer Door" },

            { 0x0010, "Debug Room" },

            { 0x0030, "Pandora Ruins - Interior - Altar Room" },

            { 0x003A, "Mana Fortress - Boss Arena - Lime Slime" },

            { 0x0040, "Potos Fields 1 (Ending Credits Version)" },
            { 0x0041, "Potos Village - Exterior (Ending Credits Version)" },
            { 0x0042, "Water Palace - Exterior (Ending Credits Version)" },
            { 0x0043, "Pandora - Exterior (Ending Credits Version)" },
            { 0x0044, "Cannon Travel Center (Ending Credits Version)" },

            { 0x0051, "Dwarf Village - Boss Arena - Tropicallo" },

            { 0x0075, "New Game Room" },

            { 0x00CB, "Invalid Map - 01F0" },
            { 0x00CC, "Mana Fortress - Exterior (Intro Cutscene Version)" },
            { 0x00CD, "Mana Fortress - Boss Arena - Mech Rider III (Intro Cutscene Version)" },
            { 0x00CE, "Pure Land (Intro Cutscene Version)" },
            { 0x00CF, "Pure Land - Mana Tree (Intro Cutscene Version)" },

            { 0x00F8, "Dwarf Village - Interior - Inn" },

            { 0x00FA, "Dwarf Village - Interior - Shop" },

            { 0x010B, "Invalid Map - 0110" },

            { 0x0126, "Beta Kippo Village V1 (Dummied Out)" },

            { 0x013A, "Witch's Castle - Boss Arena - Spikey Tiger" },

            { 0x0146, "Undine's Cave - Undine's Room" },

            { 0x014B, "Fire Palace - Boss Arena - Fire Gigas" },

            { 0x0152, "Mana Fortress - Boss Arena - Dread Slime" },

            { 0x0159, "Upperlands Forest - Spring Exterior (Ending Credits Version)" },

            { 0x0193, "Kakkara Desert - Boss Arena - Mech Rider I" },

            { 0x01DD, "Invalid Map - 0032 (1)" },

            { 0x01ED, "Kakkara - Cannon Travel Center" },

            { 0x01FA, "Invalid Map - 0032 (2)" },

            { 0x020B, "Ice Country - Interior - Santa's House" },

            { 0x026C, "Sprite Village - Exterior (Ending Credits Version)" },

            { 0x026F, "Republic Sandship - Exterior (Ending Credits Version)" },

            { 0x0272, "Turtle Island - Shop" },

            { 0x0286, "Dark Palace - Seed Altar Room" },

            { 0x02C2, "Ice Palace - Boss Arena - Tonpole Trio" },

            { 0x02CA, "Ice Palace - Boss Arena - Frost Gigas" },

            { 0x031A, "Grand Palace - Boss Arena - Hydra" },

            { 0x0345, "Imperial Castle - Boss Arena - Metal Mantis" },

            { 0x03E6, "Matango Exterior - Castle Approach (Ending Credits Version)" },
            { 0x03E7, "Ice Country - Exterior - Santa's House (Ending Credits Version)" },
            { 0x03E8, "Northtown - Exterior (Ending Credits Version)" },
            { 0x03E9, "Tasnica - Exterior (Ending Credits Version)" },

            { 0x03EE, "Lofty Mountains - Mountain Climb (Ending Credits Version)" },
            { 0x03EF, "Witch's Castle - Exterior (Ending Credits Version)" },

            { 0x03F0, "Gaia's Navel - Exterior (Ending Credits Version)" },
            { 0x03F1, "Neko's - Exterior (Ending Credits Version)" },
            { 0x03F2, "Invalid Map - 01AD" },

            { 0x03FF, "End Game Cliffs" },
        };
        private static readonly IReadOnlyDictionary<ushort, string> MapNameStrings = new Dictionary<ushort, string>()
        {
            { 0x0000, "Beta Map - Kippo Village V1" }, // Has a Door.
            { 0x0001, "Beta Map - Kippo Village V2" },
            { 0x0002, "Beta Map - Boss Arena - Mantis Ant" }, // Has OnEnter Event.
            { 0x0003, "Invalid Map - 0003" },
            { 0x0004, "Corrupt Map - Potos Waterfall V1" }, // Has OnEnter Event.
            { 0x0005, "Corrupt Map - Potos Waterfall V2" }, // Has OnEnter Event.
            { 0x0006, "Invalid Map - 0006" },
            { 0x0007, "Beta Map - Cannon Travel Center" },
            { 0x0008, "New Game/Game Over Room" },
            { 0x0009, "Invalid Map - 0009" },
            { 0x000A, "Invalid Map - 000A" },
            { 0x000B, "Invalid Map - 000B" },
            { 0x000C, "Invalid Map - 000C" },
            { 0x000D, "Invalid Map - 000D" },
            { 0x000E, "Debug Room - Tileset Viewer" }, // Valid map, but has no doors leading to it.
            { 0x000F, "Debug Room - Developer Room" }, // Has OnEnter Event
            
            { 0x0010, "Potos Village Outskirts" },
            { 0x0011, "Joch's Cave - Trial Of Courage" },
            { 0x0012, "Joch's Cave - Entrance" },
            { 0x0013, "Water Palace - Exterior - Approach" },
            { 0x0014, "Imperial Sewers" },         
            { 0x0015, "Northtown Ruins - Exterior" },
            { 0x0016, "Turtle Island - Exterior" },
            { 0x0017, "Kippo Village Outskirts" },
            { 0x0018, "Tasnica - Exterior" },
            { 0x0019, "Gaia's Navel - Exterior" },
            { 0x001A, "Tasnica - Exterior - Rooftop" },
            { 0x001B, "Gaia's Navel - Exterior - Haunted Forest Warp" },
            { 0x001C, "Haunted Forest - Neko Area" },
            { 0x001D, "Haunted Forest - Axe Required Pathway" },
            { 0x001E, "Haunted Forest - Whip Post Pathway" },
            { 0x001F, "Haunted Forest - Passage" },

            { 0x0020, "Karon's Ferry - Dock" },
            { 0x0021, "Karon's Ferry - To The Moon Palace" },
            { 0x0022, "Karon's Ferry - To Kakkara Desert" },
            { 0x0023, "Moon Palace - Interior - Crystal Orb Room" },
            { 0x0024, "Tasnica - Interiors 1" },
            { 0x0025, "Tasnica - Interiors 2" },
            { 0x0026, "Haunted Forest - Boss Arena - Werewolves" },

            { 0x002E, "Grand Palace - Boss Arena - Kettle Kin" },
            { 0x002F, "Wind Palace - Exterior" },

            { 0x0030, "Moon Palace - Exterior" },
            { 0x0031, "Upper Lands Forest - Winter Exterior" },
            { 0x0032, "Invalid Map - 0032" }, // Has Two Doors.
            { 0x0033, "Tasnica - Interior - Throne Room" },
            { 0x0034, "Upper Lands Forest - Spring Exterior" },
            { 0x0035, "Upper Lands Forest - Summer Exterior" },
            { 0x0036, "Upper Lands Forest - Fall Exterior" },
            { 0x0037, "Upper Lands Forest - Winter Exterior (Moogle Refugee Version)" },
            { 0x0038, "Sprite Village" },
            { 0x0039, "Upper Lands Forest - Cannon Travel Center" },
            { 0x003A, "Moogle Village (Pebbler Version)" },
            { 0x003B, "Moogle Village" },
            { 0x003C, "Upper Lands Forest - Boss Arena - Spring Beak" },
            { 0x003D, "Lofty Mountains - Mountain Path" },
            { 0x003E, "Lofty Mountains - Mountain Climb" },
            { 0x003F, "Mandala - Exterior" },

            { 0x0047, "Underground City - Resistance Safe Area" },
            { 0x0048, "Joch's Cave - Boss Arena - Shadows" },
            { 0x0049, "Gaia's Navel - Interior - Entrance" },
            { 0x004A, "Invalid Map - 004A" },
            { 0x004B, "Ice Palace - Exterior" },

            { 0x004E, "Ice Country - Exterior - Neko's Cave" },
            { 0x004F, "Ice Country - Boss Arena - Boreal Face" },

            { 0x0059, "Kakkara Village - Exterior" },
            { 0x005A, "Fire Palace - Exterior" },
            { 0x005B, "Kakkara Desert - Stranded By Geshtar Location" },

            { 0x0060, "Kakkara Desert - Moon Palace Connection" },

            { 0x0064, "Imperial Castle - Boss Arena - Mech Rider II" },
            { 0x0065, "Potos Fields 1 (Ending Credits Version)" },
            { 0x0066, "Potos Village - Exterior (Ending Credits Version)" },
            { 0x0067, "Water Palace - Exterior (Ending Credits Version)" },
            { 0x0068, "Pandora - Exterior (Ending Credits Version)" },
            { 0x0069, "Cannon Travel Center (Ending Credits Version)" },
            { 0x006A, "Upper Lands Forest - Spring Exterior (Ending Credits Version)" },
            { 0x006B, "Sprite Village (Ending Credits Version)" },
            { 0x006C, "Republic Sandship - Exterior (Ending Credits Version)" },
            { 0x006D, "Water Palace - Exterior - Undine's Cave Entrance" },
            { 0x006E, "Lighthouse - Rooftop" },
            { 0x006F, "Light Palace - Boss Arena - Blue Spike" },

            { 0x0070, "Lighthouse - Exterior" },
            { 0x0071, "Gold City - Exterior" },
            { 0x0072, "Southtown - Exterior" },
            { 0x0073, "Northtown - Exterior" },
            { 0x0074, "Light Palace - Exterior" },
            { 0x0075, "Light Palace - Interior - Ascent 1" },
            { 0x0076, "Light Palace - Interior - Ascent 2" },
            { 0x0077, "Light Palace - Boss Arena - Gorgon Bull" },
            { 0x0078, "Light Palace - Interior - Seed Altar" },
            { 0x0079, "Tasnica - Interior - Throne Room (Dark Stalker Version)" },

            { 0x007C, "Gold City - Interior - Mad Mara's House" }, // Sure, dupe the whole interior map, why not.

            { 0x0080, "Potos Fields 1" },
            { 0x0081, "Potos Fields 2" }, // Has An Out Of Bounds Door Trigger To The Debug Room.
            { 0x0082, "Potos Fields 3" },
            { 0x0083, "Potos Village - Exterior" },
            { 0x0084, "Neko's Bed & Breakfast - Exterior" },
            { 0x0085, "Goblin Village" },
            { 0x0086, "Cave of the White Dragon - Flammie's Nest" },
            { 0x0087, "Water Palace - Interior - Jail" },
            { 0x0088, "Water Palace - Exterior" },

            { 0x008D, "Invalid Map - 008D" },

            { 0x008F, "Water Palace - Boss Arena - Jabberwocky" }, // One Jabberwocky! Two Jabberwocky! THREE Jabberwocky! AH! AH! AH!

            { 0x0090, "Pandora - Exterior" },
            { 0x0091, "Pandora Castle - Exterior" },
            { 0x0092, "Matango - Exterior - Castle Approach (Ending Credits Version)" },
            { 0x0093, "Ice Country - Exterior - Santa's House (Ending Credits Version)" },
            { 0x0094, "Northtown - Exterior (Ending Credits Version)" },
            { 0x0095, "Tasnica - Exterior (Ending Credits Version)" },
            { 0x0096, "Lofty Mountains - Mountain Climb (Ending Credits Version)" },
            { 0x0097, "Witch's Castle - Exterior (Ending Credits Version)" },
            { 0x0098, "Gaia's Navel - Exterior (Ending Credits Version)" },
            { 0x0099, "Neko's Bed & Breakfast - Exterior (Ending Credits Version)" },
            { 0x009A, "Invalid Map - 009A" },
            { 0x009B, "Southtown - Cannon Travel Center" },
            { 0x009C, "Ice Country - Cannon Travel Center" },
            { 0x009D, "Kakkara - Cannon Travel Center" },
            { 0x009E, "Upper Lands Forest - Matango Approach - Cannon Travel Center" },
            { 0x009F, "Potos Village - Cannon Travel Center" },

            { 0x00A6, "Pandora Ruins - Boss Arena - Wall Face" },
            { 0x00A7, "Pandora Ruins - Interior - Altar Room" },
            { 0x00A8, "Kippo Village - Exterior" },
            { 0x00A9, "Invalid Map - 00A9" },

            { 0x00AC, "Invalid Map - 00AC" },
            { 0x00AD, "Invalid Map - 00AD" },
            { 0x00AE, "Invalid Map - 00AE" },
            { 0x00AF, "Invalid Map - 00AF" },

            { 0x00B0, "Mana Fortress - Exterior (Intro Cutscene Version)" },
            { 0x00B1, "Mana Fortress - Boss Arena - Mech Rider III (Intro Cutscene Version)" },
            { 0x00B2, "Pure Land (Intro Cutscene Version)" },
            { 0x00B3, "Pure Land - Mana Tree (Intro Cutscene Version)" },
            { 0x00B4, "End Game Cliffs" },
            { 0x00B5, "Invalid Map - 00B5" },
            { 0x00B6, "Invalid Map - 00B6" },
            { 0x00B7, "Invalid Map - 00B7" },
            { 0x00B8, "Invalid Map - 00B8" },
            { 0x00B9, "Invalid Map - 00B9" },
            { 0x00BA, "Invalid Map - 00BA" },
            { 0x00BB, "Invalid Map - 00BB" },
            { 0x00BC, "Invalid Map - 00BC" },
            { 0x00BD, "Invalid Map - 00BD" },
            { 0x00BE, "Invalid Map - 00BE" },
            { 0x00BF, "Invalid Map - 00BF" },

            { 0x00C0, "Invalid Map - 00C0" },
            { 0x00C1, "Invalid Map - 00C1" },
            { 0x00C2, "Scorpion Army Village - Exterior (Summer Version)" },
            { 0x00C3, "Tree Palace - Exterior" },
            { 0x00C4, "Witch's Castle - Exterior" },
            { 0x00C5, "Scorpion Army Village - Exterior (Winter Version)" },
            //00C6: Pure Land
            //00C7: Pure Land
            //00C8: Pure Land
            //00C9: Pure Land
            //00CA: Pure Land
            //00CB: Pure Land
            { 0x00CC, "Pure Land - Boss Arena - Dragon Worm" },
            //00CD: Pure Land
            //00CE: Pure Land
            //00CF: Pure Land
            
            //00D0: Pure Land
            //00D1: Pure Land
            //00D2: Pure Land
            //00D3: Pure Land - Cave
            //00D4: Pure Land - Cave
            { 0x00D5, "Pure Land - Boss Arena - Axe Beak" },
            //00D6: Pure Land - Cave
            //00D7: Pure Land
            //00D8: Pure Land
            //00D9: Pure Land
            //00DA: Pure Land
            //00DB: Pure Land
            //00DC: Pure Land
            //00DD: Pure Land
            { 0x00DE, "Matango - Exterior - Cave of the White Dragon Approach" },
            { 0x00DF, "Matango - Exterior" },

            { 0x00E0, "Matango - Exterior - Castle Approach" },
            //00E1: Pure Land
            { 0x00E2, "Pure Land - Boss Arena - Thunder Gigas" },
            //00E3: Pure Land
            { 0x00E4, "Todo Village - Exterior" },
            //00E5: Pure Land
            { 0x00E6, "Pure Land - Boss Arena - Snow Dragon" },
            { 0x00E7, "Pure Land - Boss Arena - Red Dragon" },
            { 0x00E8, "Pure Land - Boss Arena - Blue Dragon" },
            //00E9: Pure Land - Altar
            //00EA: Pure Land - Altar
            //00EB: Pure Land - Altar
            { 0x00EC, "Pure Land - Mana Tree Outlook" },
            { 0x00ED, "Mana Fortress - Boss Arena - Mech Rider III" },
            { 0x00EE, "Tree Palace - Boss Arena - Aegagropilon" },
            { 0x00EF, "Invalid Map - 00EF" },

            { 0x00F2, "Invalid Map - 00F2" },
            { 0x00F3, "Invalid Map - 00F3" },
            { 0x00F4, "Mana Fortress - Boss Arena - Buffy" },
            { 0x00F5, "Mana Fortress - Boss Arena - Dark Lich (Cutscene Version)" }, // Also contains Dark Lich
            { 0x00F6, "Mana Fortress - Boss Arena - Dark Lich" },
            { 0x00F7, "Invalid Map - 00F7" },
            { 0x00F8, "Mana Fortress - Interiors - Buffy Section" },
            { 0x00F9, "Mana Fortress - Interiors - Dread Slime Section" },
            { 0x00FA, "Mana Fortress - Interiors - Final Section" },
            { 0x00FB, "Mana Fortress - Boss Arena - Mech Rider III (To Dark Lich Version)" },
            { 0x00FC, "Invalid Map - 00FC" },
            { 0x00FD, "Mana Fortress - Boss Arena - Mana Beast" },
            { 0x00FE, "Invalid Map - 00FE" },
            { 0x00FF, "Mana Fortress - Exterior" },

            { 0x0100, "Potos Fields - Waterfall Log Bridge" },
            { 0x0101, "Potos Village - Interiors" },
            { 0x0102, "Potos Village - Interiors (Post Mantis Ant)" },
            { 0x0103, "Potos Village - Interior - Inn" },
            { 0x0104, "Potos Village - Boss Arena - Mantis Ant" },
            //{ 0x0105, "Pandora - Interiors" },
            { 0x0106, "Pandora - Interior - Inn" },
            { 0x0107, "Pandora - Interior - Purim's House" },
            { 0x0108, "Pandora Castle - Interior - Entrance Hall" },
            { 0x0109, "Pandora Castle - Interiors" },
            { 0x010A, "Pandora Castle - Interior - Throne Room" },
            { 0x010B, "Pandora Castle - Interior - Purim's Dad Room" },
            { 0x010C, "Pandora Castle - Interior - Treasury" },
            //010D: Earth Palace

            { 0x010F, "Republic Sandship - Interior - Engine Room" },

            { 0x0110, "Invalid Map - 0110" }, // Has A Door
            { 0x0111, "Wind Palace - Interior" },
            { 0x0112, "Matango Castle - Interior - Inn" },

            { 0x0114, "Earth Palace - Interior - Seed Altar Room" },
            { 0x0115, "Undine's Cave - Boss Arena - Tonpole" },
            { 0x0116, "Undine's Cave - Undine's Room" },
            { 0x0117, "Neko's Bed & Breakfast - Interior" },
            { 0x0118, "Kippo Village - Interiors" },
            { 0x0119, "Kippo Village - Interior - Inn" },
            { 0x011A, "Matango Castle - Interior - Shop" },
            //{ 0x011B, "Pandora Castle - Interiors" },
            { 0x011C, "Gaia's Navel - Interior - Entrances" },
            { 0x011D, "Gaia's Navel - Interior - Lava Room" },
            { 0x011E, "Gaia's Navel - Interior - Undergrond River" },
            { 0x011F, "Dwarf Village - Exterior" },

            { 0x0120, "Gaia's Navel - Interior - Shortcut Staircase" },
            { 0x0121, "Gaia's Navel - Interior - Magic Rope Room" },
            { 0x0122, "Dwarf Village - Boss Arena - Tropicallo" },
            { 0x0123, "Dwarf Village - Interior - Path To Earth Palace" },
            { 0x0124, "Dwarf Village - Interior - Inn" },
            { 0x0125, "Dwarf Village - Interior - Shop" },
            { 0x0126, "Dwarf Village - Interior - Watt's Blacksmith" },
            { 0x0127, "Dwarf Village - Interior - Exhibit Show Room Hallway" },
            { 0x0128, "Dwarf Village - Interior - Village Elder Room" },
            { 0x0129, "Dwarf Village - Interior - Exhibit Show Room" },
            { 0x012A, "Todo Village - Interiors" },
            { 0x012B, "Todo Village - Interior - Inn" },

            { 0x0134, "Witch's Castle - Interior - Prison" },
            { 0x0135, "Witch's Castle - Boss Arena - Spikey Tiger (Cutscene Version)" },
            { 0x0136, "Witch's Castle - Boss Arena - Spikey Tiger" },

            { 0x013D, "Scorpion Army Ship - Boss Arena - Kilroy" },

            { 0x0144, "Republic Sandship - Interior - Hallway To Admiral Morie's Room" },
            { 0x0145, "Upper Lands Forest - Boss Arena - Great Viper" },
            
            { 0x014C, "Ice Country - Exterior - Santa's House" },
            { 0x014D, "Ice Country - Interior - Santa's House" },
            { 0x014E, "Invalid Map - 014E" },
            { 0x014F, "Scorpion Army Village - Interiors" },

            { 0x0151, "Matango Castle - Interior - Throne Room" },
            { 0x0152, "Matango Castle - Interior - Truffle's Bedroom" },
            { 0x0153, "Republic Sandship - Interior - Admiral Morie's Room" },
            { 0x0154, "Kakkara Desert - Boss Arena - Mech Rider I" },
            { 0x0155, "Mandala - Interior - Inn" },
            { 0x0156, "Mandala - Interiors" },

            { 0x015B, "Fire Palace - Boss Arena - Minotaur" },

            { 0x015D, "Fire Palace - Interior - Seed Altar Room" },

            { 0x015F, "Moon Palace - Interior - Seed Altar Room" },

            { 0x0164, "Ice Palace - Interior - Glove Orb Room" },

            { 0x0169, "Ice Palace - Boss Arena - Tonpole Trio" },

            { 0x016B, "Ice Palace - Interior - Throne Room" },
            { 0x016C, "Ice Palace - Boss Arena - Frost Gigas" },

            { 0x016E, "Invalid Map - 016E" },
            { 0x016F, "Dark Palace - Interior - Entrance" },

            { 0x0173, "Dark Palace - Interior - Glove Orb Room" },

            { 0x0176, "Dark Palace - Interior - Invisible Pathway Room" },
            { 0x0177, "Dark Palace - Interior - Seed Altar Room" },

            { 0x0179, "Imperial Castle - Boss Arena - Metal Mantis" },

            { 0x017C, "Imperial Castle - Interior - Prison" },

            { 0x017F, "Imperial Castle - Interior - Treasure Orbs Room" },

            { 0x0182, "Imperial Castle - Interior - Chapel" },
            { 0x0183, "Imperial Castle - Interior - Dining Room" },

            { 0x0185, "Imperial Castle - Interior - Barracks" },

            { 0x0187, "Imperial Castle - Interior - Throne Room" },
            { 0x0188, "Invalid Map - 0188" },

            { 0x018D, "Invalid Map - 018D" },
            { 0x018E, "Beta Map - Hut Shop Interior" },
            { 0x018F, "Invalid Map - 018F" },

            { 0x0197, "Northtown Ruins - Boss Arena - Doom's Wall" },
            { 0x0198, "Northtown Ruins - Boss Arena - Vampire" },

            { 0x019B, "Invalid Map - 019B" },

            { 0x01A2, "Grand Palace - Boss Arena - Hexas" },

            { 0x01A4, "Grand Palace - Interior - Earth Crystal Orb Room" },
            { 0x01A5, "Grand Palace - Interior - Water Crystal Orb Room" },
            { 0x01A6, "Grand Palace - Interior - Wind Crystal Orb Room" },
            { 0x01A7, "Grand Palace - Interior - Fire Crystal Orb Room" },
            { 0x01A8, "Grand Palace - Interior - Light Crystal Orb Room" },
            { 0x01A9, "Grand Palace - Interior - Shadow Crystal Orb Room" },
            { 0x01AA, "Grand Palace - Interior - Lunar Crystal Orb Room" },

            { 0x01AB, "Invalid Map - 01AB" },

            { 0x01AD, "Invalid Map - 01AD" }, // Has A Door
            { 0x01AE, "Grand Palace - Boss Arena - Snap Dragon" },
            { 0x01AF, "Tree Palace - Interior - Seed Altar" },

            { 0x01B2, "Grand Palace - Boss Arena - Hydra" },
            { 0x01B3, "Grand Palace - Boss Arena - Hydra (Post Boss Version)" },

            { 0x01B5, "Invalid Map - 01B5" },
            { 0x01B6, "Invalid Map - 01B6" },
            { 0x01B7, "Invalid Map - 01B7" },
            { 0x01B8, "Invalid Map - 01B8" },
            { 0x01B9, "Invalid Map - 01B9" },
            { 0x01BA, "Invalid Map - 01BA" },
            { 0x01BB, "Invalid Map - 01BB" },
            { 0x01BC, "Pure Land - Boss Arena - Snow Dragon (Post Boss Version)" },
            { 0x01BD, "Pure Land - Boss Arena - Red Dragon (Post Boss Version)" },
            { 0x01BE, "Pure Land - Boss Arena - Blue Dragon (Post Boss Version)" },
            { 0x01BF, "Invalid Map - 01BF" },

            { 0x01C0, "Turtle Island - Interiors" },
            { 0x01C1, "Turtle Island - Interior - Shop" },
            { 0x01C2, "Invalid Map - 01C2" },
            { 0x01C3, "Invalid Map - 01C3" },
            { 0x01C4, "Invalid Map - 01C4" },
            { 0x01C5, "Invalid Map - 01C5" },
            { 0x01C6, "Invalid Map - 01C6" },
            { 0x01C7, "Invalid Map - 01C7" },
            { 0x01C8, "Invalid Map - 01C8" },
            { 0x01C9, "Invalid Map - 01C9" },
            { 0x01CA, "Invalid Map - 01CA" },
            { 0x01CB, "Invalid Map - 01CB" },
            { 0x01CC, "Invalid Map - 01CC" },
            { 0x01CD, "Invalid Map - 01CD" },
            { 0x01CE, "Invalid Map - 01CE" },
            { 0x01CF, "Invalid Map - 01CF" },

            { 0x01D0, "Invalid Map - 01D0" },
            { 0x01D1, "Invalid Map - 01D1" },
            { 0x01D2, "Invalid Map - 01D2" },
            { 0x01D3, "Invalid Map - 01D3" },
            { 0x01D4, "Invalid Map - 01D4" },
            { 0x01D5, "Invalid Map - 01D5" },
            { 0x01D6, "Invalid Map - 01D6" },
            { 0x01D7, "Invalid Map - 01D7" },
            { 0x01D8, "Invalid Map - 01D8" },
            { 0x01D9, "Invalid Map - 01D9" },
            { 0x01DA, "Invalid Map - 01DA" },
            { 0x01DB, "Invalid Map - 01DB" },
            { 0x01DC, "Invalid Map - 01DC" },
            { 0x01DD, "Invalid Map - 01DD" },
            { 0x01DE, "Invalid Map - 01DE" },
            { 0x01DF, "Invalid Map - 01DF" },

            { 0x01E0, "Invalid Map - 01E0" },
            { 0x01E1, "Invalid Map - 01E1" },
            { 0x01E2, "Corrupt Map - Unused Ruins Interior Room" },
            { 0x01E3, "Invalid Map - 01E3" },
            { 0x01E4, "Corrupt Map - Scorpion Ship Exterior V1" },
            { 0x01E5, "Corrupt Map - Scorpion Ship Exterior V2" },
            { 0x01E6, "Corrupt Map - Upperlands Forest Lake V1" },
            { 0x01E7, "Corrupt Map - Upperlands Forest Lake V2" },
            { 0x01E8, "Mana Fortress - Boss Arena - Dread Slime" },
            { 0x01E9, "Dark Palace - Boss Arena - Lime Slime" },
            { 0x01EA, "Invalid Map - 01EA" },
            { 0x01EB, "Invalid Map - 01EB" },
            { 0x01EC, "Invalid Map - 01EC" },
            { 0x01ED, "Invalid Map - 01ED" },
            { 0x01EE, "Invalid Map - 01EE" },
            { 0x01EF, "Earth Palace - Boss Arena - Fire Gigas" },

            { 0x01F0, "Invalid Map - 01F0" }, // Has A Door
            { 0x01F1, "Invalid Map - 01F1" },
            { 0x01F2, "Invalid Map - 01F2" },
            { 0x01F3, "Invalid Map - 01F3" },
            { 0x01F4, "Invalid Map - 01F4" },
            { 0x01F5, "Invalid Map - 01F5" },
            { 0x01F6, "Invalid Map - 01F6" },
            { 0x01F7, "Invalid Map - 01F7" },
            { 0x01F8, "Invalid Map - 01F8" },
            { 0x01F9, "Invalid Map - 01F9" },
            { 0x01FA, "Invalid Map - 01FA" },
            { 0x01FB, "Invalid Map - 01FB" },
            { 0x01FC, "Invalid Map - 01FC" },
            { 0x01FD, "Invalid Map - 01FD" },
            { 0x01FE, "Invalid Map - 01FE" },
            { 0x01FF, "Invalid Map - 01FF" },
        };
        private static readonly IReadOnlyDictionary<ushort, string> MapPieceNameStrings = new Dictionary<ushort, string>()
        {
            { 0x02F2, "Map Piece - Invalid - 02F2" },
            { 0x02F3, "Map Piece - Invalid - 02F3" },
            { 0x02F4, "Map Piece - Invalid - 02F4" },
            { 0x02F5, "Map Piece - Invalid - 02F5" },
            { 0x02F6, "Map Piece - Invalid - 02F6" },
            { 0x02F7, "Map Piece - Invalid - 02F7" },
            { 0x02F8, "Map Piece - Invalid - 02F8" },
            { 0x02F9, "Map Piece - Invalid - 02F9" },
            { 0x02FA, "Map Piece - Invalid - 02FA" },
            { 0x02FB, "Map Piece - Invalid - 02FB" },
            { 0x02FC, "Map Piece - Invalid - 02FC" },
            { 0x02FD, "Map Piece - Invalid - 02FD" },
            { 0x02FE, "Map Piece - Invalid - 02FE" },
            { 0x02FF, "Map Piece - Invalid - 02FF" },

            { 0x0374, "Map Piece - Invalid - 0374" },
            { 0x0375, "Map Piece - Invalid - 0375" },
            { 0x0376, "Map Piece - Invalid - 0376" },
            { 0x0377, "Map Piece - Invalid - 0377" },
            { 0x0378, "Map Piece - Invalid - 0378" },
            { 0x0379, "Map Piece - Invalid - 0379" },
            { 0x037A, "Map Piece - Invalid - 037A" },
            { 0x037B, "Map Piece - Invalid - 037B" },
            { 0x037C, "Map Piece - Invalid - 037C" },
            { 0x037D, "Map Piece - Invalid - 037D" },
            { 0x037E, "Map Piece - Invalid - 037E" },

            { 0x0387, "Map Piece - Invalid - 0387" },
        };

        public static IReadOnlyList<ushort> InvalidMapPieceIndexes { get; } = new List<ushort>()
        {
            0x02F2, // Localization Garbage.
            0x02F3, // Localization Garbage.
            0x02F4, // Localization Garbage.
            0x02F5, // Localization Garbage.
            0x02F6, // Localization Garbage.
            0x02F7, // Localization Garbage.
            0x02F8, // Localization Garbage.
            0x02F9, // Localization Garbage.
            0x02FA, // Localization Garbage.
            0x02FB, // Localization Garbage.
            0x02FC, // Localization Garbage.
            0x02FD, // Localization Garbage.
            0x02FE, // Localization Garbage.
            0x02FF, // Localization Garbage.
            
            0x0374, // Points In The Middle Of Map Palette Data.
            0x0375, // Points In The Middle Of Map Palette Data.
            0x0376, // Points In The Middle Of Map Palette Data.
            0x0377, // Null Pointer.
            0x0378, // Null Pointer.
            0x0379, // Null Pointer.
            0x037A, // Null Pointer.
            0x037B, // Null Pointer.
            0x037C, // Null Pointer.
            0x037D, // Null Pointer.
            0x037E, // Null Pointer.
            
            0x0387, // Points To A Formatted Buffer.
        };

        /// <summary>
        /// Gets the metadata for 8x8 map tilesets.
        /// </summary>
        public static DataTable<MapTilesetMetadata> Map8x8TilesetMetadata { get; } = new DataTable<MapTilesetMetadata>(new List<MapTilesetMetadata>()
        {
            new MapTilesetMetadata(0x00, "Town Exterior",              0x18, 0x01, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x01, "Plains Exterior",            0x19, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x02, "Forest Exterior",            0x1A, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x03, "Desert Exterior",            0x1B, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x04, "House Interior",             0x1C, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x05, "Sprite Village Exterior",    0x1D, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x06, "Grassy Interior",            0x1E, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x07, "Haunted Forest",             0x1F, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x08, "Ruins Exterior",             0x20, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x09, "Ruins Interior",             0x21, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x0A, "Cave Interior",              0x22, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x0B, "Pure Land Overlook",         0x23, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x0C, "Mountain Exterior",          0x24, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x0D, "Snowy Plains Exterior",      0x25, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x0E, "Castle Exterior",            0x26, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x0F, "Castle Interior",            0x27, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x10, "Matango Exterior",           0x28, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x11, "Pure Land Arch Exterior",    0x29, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x12, "Ship Exterior",              0x2A, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x13, "Ship Interior",              0x2B, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x14, "Mana Fortress Exterior",     0x2C, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x15, "Mana Fortress Interior",     0x2D, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x16, "Seed Palace Exterior 1",     0x2E, 0x00, MapTilesetMetadataFlags.None),//0x63
            new MapTilesetMetadata(0x17, "Seed Palace Interior",       0x2F, 0x00, MapTilesetMetadataFlags.None),//0x58
            new MapTilesetMetadata(0x18, "Upscale Town Exterior",      0x30, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x19, "Snow Village Exterior",      0x31, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x1A, "Grand Palace Interior",      0x32, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x1B, "Grand Palace Sand Interior", 0x33, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x1C, "Upscale Town Interior",      0x34, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x1D, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x1E, "Pure Land Exterior",         0x36, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x1F, "Seed Palace Exterior 2",     0x37, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x20, "Desert Village Exterior",    0x38, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x21, "Desert Palace Exterior",     0x39, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x22, "Temple Exterior",            0x3A, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x23, "Temple Interior",            0x3B, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x24, "Desert Cannon Exterior",     0x3C, 0x00, MapTilesetMetadataFlags.None),
            new MapTilesetMetadata(0x25, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x26, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x27, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x28, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x29, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x2A, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x2B, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x2C, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x2D, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x2E, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x2F, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x30, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x31, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x32, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x33, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x34, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x35, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x36, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x37, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x38, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x39, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x3A, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x3B, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x3C, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x3D, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x3E, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x3F, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
            new MapTilesetMetadata(0x40, "Invalid (Dummied Out)",      0x00, 0x00, MapTilesetMetadataFlags.DummiedOut),
        });
    }
}