using System.Collections.Generic;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Static class containing metadata for various data within the Secret of Mana ROM file.
    /// </summary>
    public static partial class ManaMetadata
    {
        private static readonly IReadOnlyDictionary<ushort, string> EventNameStrings = new Dictionary<ushort, string>()
        {
            { 0x0000, "Default Event Exit" },
            { 0x0001, "Open Treasure Chest" },
            { 0x0002, "Play Opening Chest Sequence" },
            { 0x0003, "Set Treasure Chest Target: Randi" },
            { 0x0004, "Set Treasure Chest Target: Purim" },
            { 0x0005, "Set Treasure Chest Target: Popoie" },
            { 0x0006, "Party Leader: Stop (Dummied Out)" }, // 7 is used instead, because fuck your charge level.
            { 0x0007, "Party Leader: Stop And Reset Charge Level" },
            { 0x0008, "Mana Sword: Revival Control" },
            { 0x0009, "Remove Barrel For Dropped Chest" },
            { 0x000A, "Call Event: 'Revived Mana Sword!'" },
            { 0x000B, "Revived Mana Sword!" },
            { 0x000C, "Dummied Out" },
            { 0x000D, "Dummied Out" },
            { 0x000E, "Dummied Out" },
            { 0x000F, "Dummied Out" },

            { 0x0010, "Jema: Pandora Castle: Witch Draining Energy Dialogue" },
            { 0x0011, "Jema: Pandora Castle: Head To Gaia's Navel Dialogue" },
            { 0x0012, "Jema: Pandora Castle: Don't Follow Me Dialogue" },
            { 0x0013, "King: Pandora Castle: Captured Soldiers Dialogue" },
            { 0x0014, "Dummied Out" },
            { 0x0015, "Dummied Out" },
            { 0x0016, "Dummied Out" },
            { 0x0017, "Dummied Out" },
            { 0x0018, "Dummied Out" },
            { 0x0019, "Dummied Out" },
            { 0x001A, "Dummied Out" },
            { 0x001B, "Dummied Out" },
            { 0x001C, "Dummied Out" },
            { 0x001D, "Dummied Out" },
            { 0x001E, "Jehk: Sage Joch Went... Dialogue" },
            { 0x001F, "Jehk: Go Away! Dialogue" },

            { 0x0020, "Jehk: Sage Joch At Tasnica Dialogue" },
            { 0x0021, "Jehk: Sage Joch At Dark Palace Dialogue" },
            { 0x0022, "Jehk: Sage Joch At Gold Isle Dialogue" },
            { 0x0023, "Jehk: Sage Joch At Moon Palace Dialogue" },
            { 0x0024, "Jehk: Fake Sage Joch Reveal Dialogue" },
            { 0x0025, "Game Over: Jehk Savior" },
            { 0x0026, "Sage Joch: Good Luck Dialogue" },
            { 0x0027, "Game Over: Sage Joch Savior" },
            { 0x0028, "Call Event 'Jema: Grand Palace Heal Dialogue'" },
            { 0x0029, "Krissie: Grand Palace Heal Dialogue" },
            { 0x002A, "Resistance Man: Grand Palace Exterior Dialogue" },
            { 0x002B, "Republic Soldier: Grand Palace Exterior Dialogue" },
            { 0x002C, "Sergo: Grand Palace Exterior Dialogue" },
            { 0x002D, "Jema: Grand Palace Forbidden Dialogue" },
            { 0x002E, "Dummied Out" },
            { 0x002F, "Phanna: Thanks! Dialogue" },

            { 0x0030, "Phanna: Apology Dialogue" },
            { 0x0031, "Mad Mara: Grand Palace Interior Dialogue" },
            { 0x0032, "Resistance Woman: Grand Palace Interior Dialogue" },
            { 0x0033, "Dummied Out" },
            { 0x0034, "Dummied Out" },
            { 0x0035, "Dummied Out" },
            { 0x0036, "Dummied Out" },
            { 0x0037, "Dummied Out" },
            { 0x0038, "Earth Palace Entrance Crystal Orb Correct Spell" },
            { 0x0039, "Dummied Out" },
            { 0x003A, "Dummied Out" },
            { 0x003B, "Dummied Out" },
            { 0x003C, "Dummied Out" },
            { 0x003D, "Dummied Out" },
            { 0x003E, "Dummied Out" },

            { 0x0040, "Purim Joins The Party!" },
            { 0x0041, "Popoie Joins The Party!" },
            { 0x0042, "Popoie Joins The Party! (Purim Leads Version)" },
            { 0x0043, "Dummied Out" },
            { 0x0044, "Dummied Out" },
            { 0x0045, "Dummied Out" },
            { 0x0046, "Dummied Out" },
            { 0x0047, "Display End Game Slide" },
            { 0x0048, "Dummied Out" },
            { 0x0049, "Dummied Out" },
            { 0x004A, "Dummied Out" },
            { 0x004B, "Dummied Out" },
            { 0x004C, "Dummied Out" },
            { 0x004D, "Dummied Out" },
            { 0x004E, "Dummied Out" },
            { 0x004F, "Save Dialogue From Options" },

            { 0x0050, "Save Dialogue" },

            { 0x0071, "Dummied Out" },
            { 0x0072, "Dummied Out" },
            { 0x0073, "Unused CollisionType5B Event (Dummied Out)" },
            { 0x0074, "Dummied Out" },
            { 0x0075, "Dummied Out" },
            { 0x0076, "Dummied Out" },
            { 0x0077, "Dummied Out" },
            { 0x0078, "Dummied Out" },
            { 0x0079, "Dummied Out" },
            { 0x007A, "Dummied Out" },
            { 0x007B, "Dummied Out" },
            { 0x007C, "Dummied Out" },
            { 0x007D, "Dummied Out" },
            { 0x007E, "Dummied Out" },
            { 0x007F, "Dummied Out" },

            { 0x0080, "Dummied Out" },
            { 0x0081, "Water Palace: Captured By Geshtar" },
            { 0x0082, "Water Palace: Gave Seed To Geshtar" },
            { 0x0083, "Water Palace: Refused Geshtar" },
            { 0x0084, "Water Palace: Geshtar Summons Jabberwocky" },
            { 0x0085, "Dummied Out" },
            { 0x0086, "Dummied Out" },
            { 0x0087, "Dummied Out" },
            { 0x0088, "Dummied Out" },
            { 0x0089, "Dummied Out" },
            { 0x008A, "Dummied Out" },
            { 0x008B, "Dummied Out" },
            { 0x008C, "Dummied Out" },
            { 0x008D, "Dummied Out" },
            { 0x008E, "Debug Event: Purim" },
            { 0x008F, "Debug Event: Popoie" },

            { 0x0090, "Dummied Out" },
            { 0x0091, "Dummied Out" },
            { 0x0092, "Dummied Out" },
            { 0x0093, "Dummied Out" },
            { 0x0094, "Dummied Out" },
            { 0x0095, "Dummied Out" },
            { 0x0096, "Dummied Out" },
            { 0x0097, "Dummied Out" },
            { 0x0098, "Dummied Out" },
            { 0x0099, "Neko: Home Dialogue" },
            { 0x009A, "Dummied Out" },
            { 0x009B, "Dummied Out" },
            { 0x009C, "Dummied Out" },
            { 0x009D, "Dummied Out" },
            { 0x009E, "Dummied Out" },
            { 0x009F, "Dummied Out" },

            { 0x00A0, "Dummied Out" },
            { 0x00A1, "Dummied Out" },
            { 0x00A2, "Dummied Out" },
            { 0x00A3, "Dummied Out" },
            { 0x00A4, "Dummied Out" },
            { 0x00A5, "Dummied Out" },
            { 0x00A6, "Randi: Waterfall Log Bridge Dialogue (Dummied Out)" },   // Weird. You can't talk to characters on the bridge.
            { 0x00A7, "Dummied Out" },
            { 0x00A8, "Elliot: Waterfall Log Bridge Dialogue (Dummied Out)" },
            { 0x00A9, "Timothy: Waterfall Log Bridge Dialogue (Dummied Out)" }, // This one says something. '...'
            { 0x00AA, "Pecard Dialogue" },
            { 0x00AB, "OnEnter: Lighthouse - Exterior" },
            { 0x00AC, "Dummied Out" },
            { 0x00AD, "Dummied Out" },
            { 0x00AE, "Undine's Cave Entrance: No Entry" },
            { 0x00AF, "Dummied Out" },

            { 0x0100, "Intro Sequence: Debug Shortcut" },
            { 0x0101, "Potos Waterfall: Crash Landing" },

            { 0x014E, "Generic '...' Dialogue" },

            { 0x01AF, "Gaia's Navel: Cave State Controller" },

            { 0x01DC, "Witch's Castle: Brainwashed Soldier Dialogue" },

            { 0x01EC, "Haunted Forest: Axe Requirement Prompt" },

            { 0x02E0, "Tasnica Exterior: Greeting Soldier Dialogue" },

            { 0x0312, "Shop Ring Controller" },
            { 0x0313, "Open Sell Item Ring" },
            { 0x0314, "Second Chance Shopping Dialogue" },

            { 0x0334, "On Boss Death: Check If Randi Is Alive" },
            { 0x0335, "On Boss Death: Check If Purim Is Alive" },
            { 0x0336, "On Boss Death: Check If Popoie Is Alive" },
            { 0x0337, "Shop: Insufficient Funds Dialogue" },

            { 0x0396, "Mad Mara: Good Luck Dialogue" },
            { 0x0397, "Mad Mara: Got Gold Tower Key Dialogue" },
            { 0x0398, "Mad Mara: Run Into House When Noticed" },
            { 0x0399, "Mad Mara: Dialogue Control" },

            { 0x03F0, "Veedio: Boot-Up Dialogue" },
            { 0x03F1, "OnEnter: Beta Map - Boss Arena - Mantis Ant (Dummied Out)" },
            { 0x03F2, "Dummied Out" },
            { 0x03F3, "Dummied Out" },
            { 0x03F4, "Dummied Out" },
            { 0x03F5, "Dummied Out" },
            { 0x03F6, "Dummied Out" },
            { 0x03F7, "Dummied Out" },
            { 0x03F8, "Dummied Out" },
            { 0x03F9, "Dummied Out" },
            { 0x03FA, "Dummied Out" },
            { 0x03FB, "Dummied Out" },
            { 0x03FC, "Dummied Out" },
            { 0x03FD, "Dummied Out" },
            { 0x03FE, "Dummied Out" },
            { 0x03FF, "Call Event: 'End Of Game Sequence'" }, // This was the location of the original JP ending. It still exists unpointed after this event.

            { 0x0400, "Intro Sequence (Debug Event 01)" }, // Artist Formly Known as "Debug Event 01"
            { 0x0401, "Debug Event 02" },
            { 0x0402, "Debug Event 03" },
            { 0x0403, "Debug Event 04" },
            { 0x0404, "Debug Event 05" },
            { 0x0405, "Debug Event 06" },
            { 0x0406, "Dummied Out" },
            { 0x0407, "Dummied Out" },
            { 0x0408, "Cannon Travel: Ghost Check" },
            { 0x0409, "Cannon Travel: Ghost Check Failure Dialogue" },
            { 0x040A, "Dummied Out" },
            { 0x040B, "Dummied Out" },
            { 0x040C, "Dummied Out" },
            { 0x040D, "Dummied Out" },
            { 0x040E, "Dummied Out" },
            { 0x040F, "Dummied Out" },

            { 0x0410, "Cure Party's Status Effects" },
            { 0x0411, "Cure Randi's Status Effects (No Refresh) (Dummied Out)" },
            { 0x0412, "Cure Purim's Status Effects (No Refresh) (Dummied Out)" },
            { 0x0413, "Cure Popoie's Status Effects (No Refresh) (Dummied Out)" },
            { 0x0414, "Cure Party's Low-Byte Status Effects (Dummied Out)" },
            { 0x0415, "Cure Randi's Low-Byte Status Effects" },
            { 0x0416, "Cure Purim's Low-Byte Status Effects (Dummied Out)" },
            { 0x0417, "Cure Popoie's Low-Byte Status Effects (Dummied Out)" },
            { 0x0418, "Cure Randi's Status Effects" },
            { 0x0419, "Cure Purim's Status Effects" },
            { 0x041A, "Cure Popoie's Status Effects" },
            { 0x041B, "Cure Party HP And Status Effects With Cheer" },
            { 0x041C, "Cure Party's HP & MP On Boss Defeat (Dummied Out)" },
            { 0x041D, "Dummied Out" },
            { 0x041E, "Cure Party HP, MP And Status Effects" },
            { 0x041F, "Dummied Out" },

            { 0x04E0, "Jema: Grand Palace Heal Dialogue" },

            { 0x04EB, "Dummied Out" },
            { 0x04EC, "Dummied Out" },
            { 0x04ED, "Dummied Out" },
            { 0x04EE, "Dummied Out" },
            { 0x04EF, "Dummied Out" },

            { 0x04F0, "Tasnica Exterior: No Entry Dialogue" },
            { 0x04F1, "Dummied Out" },
            { 0x04F2, "Dummied Out" },
            { 0x04F3, "Dummied Out" },
            { 0x04F4, "Dummied Out" },
            { 0x04F5, "Dummied Out" },
            { 0x04F6, "Dummied Out" },
            { 0x04F7, "Dummied Out" },
            { 0x04F8, "Dummied Out" },
            { 0x04F9, "Dummied Out" },
            { 0x04FA, "Dummied Out" },
            { 0x04FB, "Dummied Out" },
            { 0x04FC, "Dummied Out" },
            { 0x04FD, "End Of Game Sequence" },
            { 0x04FE, "Dummied Out" },
            { 0x04FF, "Dummied Out" },

            { 0x0500, "Award Glove Orb" },
            { 0x0501, "Award Sword Orb" },
            { 0x0502, "Award Axe Orb" },
            { 0x0503, "Award Spear Orb" },
            { 0x0504, "Award Whip Orb" },
            { 0x0505, "Award Bow Orb" },
            { 0x0506, "Award Boomerang Orb" },
            { 0x0507, "Award Javelin Orb" },
            { 0x0508, "Weapon Power - MAX (Bugged)" }, // This is the event that is called when the game can't give you a weapon orb, which never normally happens.
                                                       // In the US version the event just opens a text window. However in the JP version it will print "Weapon Power - MAX!"
                                                       // Even in the JP version this event will soft lock due due to it returning incorrectly.
            { 0x0509, "Got Weapon Orb: Common Exit" },
            { 0x050A, "Tasnica King Dialogue: Award Sword Orb" },
            { 0x050B, "Dummied Out" },
            { 0x050C, "Dummied Out" },
            { 0x050D, "Dummied Out" },
            { 0x050E, "Dummied Out" },
            { 0x050F, "Dummied Out" },

            { 0x052D, "Imperial Castle: Wall Switch" },

            { 0x0565, "Grand Palace: Red Switch" },
            { 0x0566, "Grand Palace: Blue Switch" },
            { 0x0567, "Grand Palace: Yellow Switch" },
            { 0x0568, "Grand Palace: Green Switch" },

            { 0x056D, "Dummied Out" },
            { 0x056E, "Dummied Out" },
            { 0x056F, "Dummied Out" },

            { 0x0570, "Grand Palace: Earth Crystal Orb" },
            { 0x0571, "Grand Palace: Water Crystal Orb" },
            { 0x0572, "Grand Palace: Wind Crystal Orb" },
            { 0x0573, "Grand Palace: Fire Crystal Orb" },
            { 0x0574, "Grand Palace: Light Crystal Orb" },
            { 0x0575, "Grand Palace: Shadow Crystal Orb" },
            { 0x0576, "Grand Palace: Lunar Crystal Orb" },

            { 0x0599, "Grand Palace Underground: Left Wall Switch" },
            { 0x059A, "Grand Palace Underground: Right Wall Switch" },

            { 0x05FF, "Generic Text Event End" },

            { 0x05C0, "Dummied Out" },
            { 0x05C1, "Dummied Out" },
            { 0x05C2, "Dummied Out" },
            { 0x05C3, "Dummied Out" },
            { 0x05C4, "Dummied Out" },
            { 0x05C5, "Dummied Out" },
            { 0x05C6, "Dummied Out" },
            { 0x05C7, "Dummied Out" },
            { 0x05C8, "Dummied Out" },
            { 0x05C9, "Dummied Out" },
            { 0x05CA, "Dummied Out" },
            { 0x05CB, "Dummied Out" },
            { 0x05CC, "Dummied Out" },
            { 0x05CD, "Dummied Out" },
            { 0x05CE, "Dummied Out" },
            { 0x05CF, "Dummied Out" },

            { 0x0600, "Signpost: Water Palace/Potos/Pandora" },
            { 0x0601, "Signpost: Potos/Pandora" },
            { 0x0602, "Signpost: Gaia's Navel/Kippo Village/Pandora" },
            { 0x0603, "Signpost: Topaz Falls (Dummied Out)" },
            { 0x0604, "Signpost: Water Palace" },
            { 0x0605, "Signpost: Forest Off Limits" },
            { 0x0606, "Signpost: Cannon Travel Center" },
            { 0x0607, "Signpost: Neko's" },
            { 0x0608, "Signpost: Beware Of Goblins" },
            { 0x0609, "Signpost: Haunted Forest/Gaia's Navel" },
            { 0x060A, "Signpost Text: Water Palace" },
            { 0x060B, "Signpost Text: Kingdom of Pandora" },
            { 0x060C, "Signpost Text: Potos Village" },
            { 0x060D, "Dummied Out" },
            { 0x060E, "Dummied Out" },
            { 0x060F, "Call Event: 'Fast Fade Out Music Track' (Dummied Out)" },

            { 0x0640, "Dummied Out" },

            { 0x0643, "OnEnter: Fire Palace - Exterior" },

            { 0x0645, "Dummied Out" },
            { 0x0646, "OnEnter: Kakkara Desert Maze" },

            { 0x0649, "Dummied Out" },
            { 0x064A, "Dummied Out" },
            { 0x064B, "Dummied Out" },
            { 0x064C, "Dummied Out" },
            { 0x064D, "Dummied Out" },
            { 0x064E, "OnEnter: Corrupt Map - Potos Waterfall (Dummied Out)" },
            { 0x064F, "OnEnter: Debug Room - Developer Room (Dummied Out)" },

            { 0x066F, "Game Over: Santa Savior" },

            { 0x0670, "Treasure Chest: Glove Orb" },
            { 0x0671, "Treasure Chest: Sword Orb" },
            { 0x0672, "Treasure Chest: Axe Orb" },
            { 0x0673, "Treasure Chest: Spear Orb" },
            { 0x0674, "Treasure Chest: Whip Orb" },
            { 0x0675, "Treasure Chest: Bow Orb" },
            { 0x0676, "Treasure Chest: Boomerang Orb" },
            { 0x0677, "Treasure Chest: Javelin Orb" },
            { 0x0678, "Dummied Out" },
            { 0x0679, "Dummied Out" },
            { 0x067A, "Dummied Out" },
            { 0x067B, "Dummied Out" },
            { 0x067C, "Dummied Out" },
            { 0x067D, "Dummied Out" },
            { 0x067E, "Award Chest Contents: 1000 GP" },
            { 0x067F, "Award Chest Contents: 50 GP" },

            { 0x0680, "Potos Village: 50 GP Chest" },
            { 0x0681, "Unused: 50 GP Chest 01 (Dummied Out)" },
            { 0x0682, "Unused: 50 GP Chest 02 (Dummied Out)" },
            { 0x0683, "Pandora Treasury: 50 GP Chest 01" },
            { 0x0684, "Pandora Treasury: 50 GP Chest 02" },
            { 0x0685, "Pandora Treasury: 50 GP Chest 03" },
            { 0x0686, "Pandora Treasury: 50 GP Chest 04" },
            { 0x0687, "Gaia's Navel: Magic Rope Chest" },
            { 0x0688, "Witch's Castle: 50 GP Chest" },
            { 0x0689, "Witch's Castle: Whip Chest" },
            { 0x068A, "Fire Palace: 1000 GP Chest 01" },
            { 0x068B, "Fire Palace: 1000 GP Chest 02" },
            { 0x068C, "Unused: 1000 GP Chest (Dummied Out)" },
            { 0x068D, "Imperial Castle: 1000 GP Chest" },
            { 0x068E, "Dark Palace: 1000 GP Chest" },
            { 0x068F, "Unused: 50 GP Chest 03 (Dummied Out)" },

            { 0x0690, "Unused: 50 GP Chest 04 (Dummied Out)" },
            { 0x0691, "Unused: 50 GP Chest 05 (Dummied Out)" },
            { 0x0692, "Unused: 50 GP Chest 06 (Dummied Out)" },
            { 0x0693, "Unused: 50 GP Chest 07 (Dummied Out)" },
            { 0x0694, "Unused: 50 GP Chest 08 (Dummied Out)" },
            { 0x0695, "Unused: 50 GP Chest 09 (Dummied Out)" },
            { 0x0696, "Unused: 50 GP Chest 0A (Dummied Out)" },
            { 0x0697, "Unused: 50 GP Chest 0B (Dummied Out)" },
            { 0x0698, "Unused: 50 GP Chest [Grand Palace Earth] (Dummied Out)" }, // Chests 698-69F are tied to event flags used for
            { 0x0699, "Unused: 50 GP Chest [Grand Palace Water] (Dummied Out)" }, // the Grand Palace crystal orbs.
            { 0x069A, "Unused: 50 GP Chest [Grand Palace Wind] (Dummied Out)" },  // This indicates the chests came first and was
            { 0x069B, "Unused: 50 GP Chest [Grand Palace Fire] (Dummied Out)" },  // later overwritten by the crystal orbs.
            { 0x069C, "Unused: 50 GP Chest [Grand Palace Light] (Dummied Out)" },
            { 0x069D, "Unused: 50 GP Chest [Grand Palace Shadow] (Dummied Out)" },
            { 0x069E, "Unused: 50 GP Chest [Grand Palace Moon] (Dummied Out)" },
            { 0x069F, "Unused: 50 GP Chest [Grand Palace Tree] (Dummied Out)" },

            { 0x06A0, "Dummied Out" },
            { 0x06A1, "Dummied Out" },
            { 0x06A2, "Dummied Out" },
            { 0x06A3, "Dummied Out" },
            { 0x06A4, "Dummied Out" },
            { 0x06A5, "Dummied Out" },
            { 0x06A6, "Dummied Out" },
            { 0x06A7, "Dummied Out" },
            { 0x06A8, "Dummied Out" },
            { 0x06A9, "Dummied Out" },
            { 0x06AA, "Dummied Out" },
            { 0x06AB, "Dummied Out" },
            { 0x06AC, "Dummied Out" },
            { 0x06AD, "Dummied Out" },
            { 0x06AE, "Dummied Out" },
            { 0x06AF, "Dummied Out" },

            { 0x06D7, "Whip Post: Perform Jump" },
            { 0x06D8, "Whip Post: Weapon Check" },

            { 0x06EB, "Close And Lock All Generic Palace Doors" },
            { 0x06EC, "Close Generic Palace Door 01" },
            { 0x06ED, "Close Generic Palace Door 02" },
            { 0x06EE, "Close Generic Palace Door 03" },
            { 0x06EF, "Close Generic Palace Door 04" },

            { 0x06F0, "Toggle Generic Palace Door 01" },
            { 0x06F1, "Toggle Generic Palace Door 02" },
            { 0x06F2, "Toggle Generic Palace Door 03" },
            { 0x06F3, "Toggle Generic Palace Door 04" },
            { 0x06F4, "Call Event: 'Close All Generic Palace Doors'" }, // This probably did something else at some point.
            { 0x06F5, "Close All Generic Palace Doors And Refresh Map" },
            { 0x06F6, "Close All Generic Palace Doors" },

            { 0x06F8, "Whip Post Collision: Start Jump" },

            { 0x0700, "Play Track: 'Secret of the Arid Sands' (E)" }, // Not Returnable
            { 0x0701, "Play Track: 'Flight into the Unknown' (E) (Dummied Out)" }, // Not Returnable
            { 0x0702, "Play Track: 'The Dark Star' (E)" }, // Not Returnable
            { 0x0703, "Play Track: 'Prophecy' (E) (Dummied Out)" }, // Not Returnable
            { 0x0704, "Play Track: 'Danger' (E)" }, // Not Returnable
            { 0x0705, "Play Track: 'Distant Thunder' (E)" }, // Not Returnable
            { 0x0706, "Play Track: 'The Wind Never Ceases' (E)" }, // Not Returnable
            { 0x0707, "Play Track: 'I Closed My Eyes' (E) (Dummied Out)" }, // Not Returnable
            { 0x0708, "Play Track: 'Spirit of the Night' (E)" }, // Not Returnable
            { 0x0709, "Play Track: 'The Little Sprite' (E)" }, // Not Returnable
            { 0x070A, "Play Track: 'What the Forest Taught Me' (E) (Dummied Out)" }, // Not Returnable
            { 0x070B, "Play Track: 'Eternal Recurrence' (E)" }, // Not Returnable
            { 0x070C, "Play Track: 'The Oracle' (E)" }, // Not Returnable
            { 0x070D, "Play Track: 'A Curious Tale' (E)" }, // Not Returnable
            { 0x070E, "Play Track: 'Into the Thick of It' (E)" }, // Not Returnable
            { 0x070F, "Play Track: 'Phantom and A Rose' (E)" }, // Not Returnable
            
            { 0x0710, "Play Track: 'Did You See the Ocean?' (E)" }, // Not Returnable
            { 0x0711, "Play Track: 'The Color Of The Summer Sky' (E)" }, // Not Returnable
            { 0x0712, "Play Track: 'The Door into the World' (R)" }, // Returnable
            { 0x0713, "Play Track: 'The Legend' (E)" }, // Not Returnable
            { 0x0714, "Play Track: 'The Calm Before the Storm' (E)" }, // Not Returnable
            { 0x0715, "Play Track: 'A Bell Is Tolling' (E)" }, // Not Returnable
            { 0x0716, "Play Track: 'Dancing Animals' (E)" }, // Not Returnable
            { 0x0717, "Play Track: 'Victory!!' (E) (Dummied Out)" }, // Not Returnable
            { 0x0718, "Play Track: 'BossDefeated' (E) (Dummied Out)" }, // Not Returnable
            { 0x0719, "Play Track: 'CannonTravelLaunch' (E) (Dummied Out)" }, // Not Returnable
            { 0x071A, "Play Track: 'CannonTravelFlight' (E) (Dummied Out)" }, // Not Returnable
            { 0x071B, "Play Track: 'Ceremony' (E)" }, // Not Returnable
            { 0x071C, "Play Track: 'Together Always' (E)" }, // Not Returnable
            { 0x071D, "Play Track: 'Whisper And Mantra' (E)" }, // Not Returnable
            { 0x071E, "Play Track: 'BurningCastle' (D)" }, // Returnable
            { 0x071F, "Play Track: 'It Happened Late One Evening' (E)" }, // Not Returnable
            
            { 0x0720, "Play Track: 'A Curious Happening' (E)" }, // Not Returnable
            { 0x0721, "Play Track: 'Ultimate!' (R)" }, // Returnable
            { 0x0722, "Play Track: 'Complete!' (E) (Dummied Out)" }, // Not Returnable
            { 0x0723, "Play Track: 'Victory!' (E) (Dummied Out)" }, // Not Returnable
            { 0x0724, "Play Track: 'A Wish...' (E)" }, // Not Returnable
            { 0x0725, "Play Track: 'Monarch On the Shore' (E)" }, // Not Returnable
            { 0x0726, "Play Track: 'Steel and Snare' (E)" }, // Not Returnable
            { 0x0727, "Play Track: 'Still of the Night' (E)" }, // Not Returnable
            { 0x0728, "Play Track: 'FlammieDescends' (R)" }, // Returnable
            { 0x0729, "Play Track: 'Fond Memories' (E) (Dummied Out)" }, // Not Returnable
            { 0x072A, "Play Track: 'Mystic Invasion' (E)" }, // Not Returnable
            { 0x072B, "Play Track: 'In the Dead of the Night' (E) (Dummied Out)" }, // Not Returnable
            { 0x072C, "Play Track: 'Fear The Heavens' (R)" }, // Returnable
            { 0x072D, "Play Track: 'Intro' (R)" }, // Returnable
            { 0x072E, "Play Track: 'UnusedFanfare01' (E) (Dummied Out)" }, // Not Returnable
            { 0x072F, "Play Track: 'Eureka!' (R)" }, // Returnable
            
            { 0x0730, "Play Track: 'UnusedFanfare02' (R) (Dummied Out)" }, // Returnable
            { 0x0731, "Play Track: 'Leave Time for Love' (R)" }, // Returnable
            { 0x0732, "Play Track: 'The Second Truth from the Left' (R)" }, // Returnable
            { 0x0733, "Play Track: 'The Curse' (R)" }, // Returnable
            { 0x0734, "Play Track: 'I Won't Forget' (R)" }, // Returnable
            { 0x0735, "Play Track: 'Buddy!' (E) (Dummied Out)" }, // Not Returnable
            { 0x0736, "Play Track: 'Morning Is Here' (R)" }, // Returnable
            { 0x0737, "Play Track: 'One of Them Is Hope' (R)" }, // Returnable
            { 0x0738, "Play Track: 'A Conclusion' (R)" }, // Returnable
            { 0x0739, "Play Track: 'Meridian Dance' (R)" }, // Returnable
            { 0x073A, "Play Track: 'Now Flightless Wings' (R)" }, // Returnable
            { 0x073B, "Dummied Out" },
            { 0x073C, "Dummied Out" },
            { 0x073D, "Stop Music Track" },
            { 0x073E, "Fast Fade Out Music Track" },
            { 0x073F, "Fast Fade In Music Track" },

            { 0x0740, "Dummied Out" },
            { 0x0741, "Dummied Out" },
            { 0x0742, "Dummied Out" },
            { 0x0743, "Dummied Out" },
            { 0x0744, "Dummied Out" },
            { 0x0745, "Dummied Out" },
            { 0x0746, "Dummied Out" },
            { 0x0747, "Dummied Out" },
            { 0x0748, "Fade Out Music Track" },
            { 0x0749, "Fade In Music Track" },
            // [74A] Sound Effect 24
            { 0x074B, "Dummied Out" },
            { 0x074C, "Dummied Out" },
            { 0x074D, "Dummied Out" },
            { 0x074E, "Dummied Out" },
            { 0x074F, "Dummied Out" },

            { 0x0750, "Dummied Out" },
            { 0x0751, "Dummied Out" },
            { 0x0752, "Dummied Out" },
            { 0x0753, "Dummied Out" },
            { 0x0754, "Dummied Out" },
            { 0x0755, "Dummied Out" },
            { 0x0756, "Dummied Out" },
            { 0x0757, "Dummied Out" },
            { 0x0758, "Dummied Out" },
            { 0x0759, "Dummied Out" },
            { 0x075A, "Dummied Out" },
            { 0x075B, "Dummied Out" },
            { 0x075C, "Dummied Out" },
            { 0x075D, "Dummied Out" },
            { 0x075E, "Dummied Out" },
            { 0x075F, "Dummied Out" },

            { 0x0760, "Dummied Out" },
            { 0x0761, "Dummied Out" },
            { 0x0762, "Dummied Out" },
            { 0x0763, "Dummied Out" },
            { 0x0764, "Dummied Out" },
            { 0x0765, "Dummied Out" },
            { 0x0766, "Dummied Out" },
            { 0x0767, "Dummied Out" },
            { 0x0768, "Dummied Out" },
            { 0x0769, "Dummied Out" },
            { 0x076A, "Dummied Out" },
            { 0x076B, "Dummied Out" },
            { 0x076C, "Dummied Out" },
            { 0x076D, "Dummied Out" },
            { 0x076E, "Dummied Out" },
            { 0x076F, "Dummied Out" },

            { 0x0770, "Dummied Out" },
            { 0x0771, "Dummied Out" },
            { 0x0772, "Dummied Out" },
            { 0x0773, "Dummied Out" },
            { 0x0774, "Dummied Out" },
            { 0x0775, "Dummied Out" },
            { 0x0776, "Dummied Out" },
            { 0x0777, "Dummied Out" },
            { 0x0778, "Dummied Out" },
            { 0x0779, "Dummied Out" },
            { 0x077A, "Dummied Out" },
            { 0x077B, "Dummied Out" },
            { 0x077C, "Dummied Out" },
            { 0x077D, "Dummied Out" },
            { 0x077E, "Dummied Out" },
            { 0x077F, "Dummied Out" },

            { 0x0780, "Tunnel Transport: North" },
            { 0x0781, "Tunnel Transport: South" },
            { 0x0782, "Play Sound Effect: Mana Seed Twinkle (R)" }, // Returnable
            { 0x0783, "Play Sound Effect: Heavy Object Moved (R)" }, // Returnable
            { 0x0784, "Play Sound Effect: Explosion (R)" }, // Returnable
            { 0x0785, "Play Sound Effect: Earthquake (R)" }, // Returnable, Continuous
            { 0x0786, "Play Sound Effect: Heal Twinkle (R) (Dummied Out)" }, // Returnable
            { 0x0787, "Play Sound Effect: Bitch Slap (R)" }, // Returnable, Also does a white palette flash.
            { 0x0788, "Stop Sound Effect" }, // Needed to stop the earthquake, water rushing, and heart beat sound effects.
            { 0x0789, "Play Sound Effect: Stove Clank (R)" }, // Returnable
            { 0x078A, "Play Sound Effect: Attack Failure (R) (Dummied Out)" }, // Returnable
            { 0x078B, "Play Sound Effect: Mana Seed Twinkle (R) (No Fade) (Dummied Out)" }, // Returnable
            { 0x078C, "Play Sound Effect: Palace Door Opening (R)" }, // Returnable
            { 0x078D, "Play Sound Effect: Water Rushing (R)" }, // Returnable, Continuous
            { 0x078E, "Play Sound Effect: Weapon Slash (R)" }, // Returnable
            { 0x078F, "Play Sound Effect: Heavy Door Slam (R)" }, // Returnable
            
            { 0x0790, "Play Sound Effect: Heart Beat (R)" }, // Returnable, Continuous
            { 0x0791, "Play Sound Effect: Switch Activated (R)" }, // Returnable
            { 0x0792, "Play Sound Effect: Heavy Switch Activated (R) (Dummied Out)" }, // Returnable
            { 0x0793, "Play Sound Effect: Teleport (R)" }, // Returnable
            { 0x0794, "Play Sound Effect: Long Teleport (R)" }, // Returnable
            { 0x0795, "Play Sound Effect: Whistle (R)" }, // Returnable
            { 0x0796, "Play Sound Effect: Spell Cast (R)" }, // Returnable, Used when Grandpa summons Sylphid.
            { 0x0797, "Play Sound Effect: Purchase Item (R)" }, // Returnable
            { 0x0798, "Play Sound Effect: Cave-In (R)" }, // Returnable
            { 0x0799, "Play Sound Effect: Orb Cast Failure (R)" }, // Returnable
            { 0x079A, "Play Sound Effect: Spring Beak Chirp (R)" }, // Returnable
            { 0x079B, "Play Sound Effect: Flammie Chirp (R)" }, // Returnable
            { 0x079C, "Play Sound Effect: Heavy Object Collapse (R)" }, // Returnable
            { 0x079D, "Play Sound Effect: Brazier Lighting (R)" }, // Returnable
            { 0x079E, "Play Sound Effect: Multiple Braziers Lighting (R)" }, // Returnable
            { 0x079F, "Play Sound Effect: Veedio Shutdown Noise (R)" }, // Returnable
            
            { 0x07A1, "Dummied Out" },
            { 0x07A2, "Dummied Out" },
            { 0x07A3, "Dummied Out" },
            { 0x07A4, "Dummied Out" },
            { 0x07A5, "Dummied Out" },
            { 0x07A6, "Dummied Out" },
            { 0x07A7, "Dummied Out" },
            { 0x07A8, "Dummied Out" },
            { 0x07A9, "Dummied Out" },
            { 0x07AA, "Dummied Out" },
            { 0x07AB, "Dummied Out" },
            { 0x07AC, "Dummied Out" },
            { 0x07AD, "Dummied Out" },
            { 0x07AE, "Dummied Out" },
            { 0x07AF, "Dummied Out" },

            { 0x07B0, "Dummied Out" },
            { 0x07B1, "Dummied Out" },
            { 0x07B2, "Dummied Out" },
            { 0x07B3, "Dummied Out" },
            { 0x07B4, "Dummied Out" },
            { 0x07B5, "Dummied Out" },
            { 0x07B6, "Dummied Out" },
            { 0x07B7, "Dummied Out" },
            { 0x07B8, "Dummied Out" },
            { 0x07B9, "Dummied Out" },
            { 0x07BA, "Dummied Out" },
            { 0x07BB, "Dummied Out" },
            { 0x07BC, "Dummied Out" },
            { 0x07BD, "Dummied Out" },
            { 0x07BE, "Dummied Out" },
            { 0x07BF, "Dummied Out" },

            { 0x07C0, "Play Track: 'Victory!!' (R)" }, // Returnable
            { 0x07C1, "Play Track: 'Victory!!' (Shorter Fade) (R) (Dummied Out)" }, // Returnable, Fade Is 3 Ticks Shorter
            { 0x07C2, "Play Track: 'Secret of the Arid Sands' (R)" }, // Returnable
            { 0x07C3, "Play Track: 'The Calm Before the Storm' (R)" }, // Returnable
            { 0x07C4, "Play Track: 'Danger' (R)" }, // Returnable
            { 0x07C5, "Play Track: 'Victory!' (R) (Dummied Out)" }, // Returnable
            { 0x07C6, "Play Track: 'Victory!' (Shorter Fade) (R)" }, // Returnable, Fade Is 7 Ticks Shorter
            { 0x07C7, "Play Track: 'I Closed My Eyes' (R)" }, // Returnable
            { 0x07C8, "Play Track: 'I Closed My Eyes' (Longer Fade) (R) (Dummied Out)" }, // Returnable, Fade Is 4 Ticks Longer
            { 0x07C9, "Play Track: 'Eternal Recurrence' (R)" }, // Returnable
            { 0x07CA, "Play Track: 'Monarch On the Shore' (R)" }, // Returnable
            { 0x07CB, "Play Track: 'Phantom and A Rose' (R)" }, // Returnable
            { 0x07CC, "Play Track: 'Phantom and A Rose' (Longer Fade) (R)" }, // Returnable, Fade Is 5 Ticks Longer
            { 0x07CD, "Play Track: 'Into the Thick of It' (R)" }, // Returnable
            { 0x07CE, "Play Track: 'Into the Thick of It' (Shorter Fade) (R)" }, // Returnable, Fade Is 1 Tick Shorter
            { 0x07CF, "Play Track: 'It Happened Late One Evening' (R)" }, // Returnable
            
            { 0x07D0, "Play Track: 'Did You See the Ocean?' (R)" }, // Returnable
            { 0x07D1, "Play Track: 'Did You See the Ocean?' (Longer Fade) (R) (Dummied Out)" }, // Returnable, Fade Is 1 Tick Longer
            { 0x07D2, "Play Track: 'Whisper And Mantra' (R)" }, // Returnable
            { 0x07D3, "Play Track: 'Whisper And Mantra' (Longer Fade) (R)" }, // Returnable, Fade Is 1 Tick Longer
            { 0x07D4, "Play Track: 'Complete!' (R)" }, // Returnable
            { 0x07D5, "Play Track: 'Complete!' (Shorter Fade) (R) (Dummied Out)" }, // Returnable, Fade Is 7 Ticks Shorter
            { 0x07D6, "Play Track: 'Dancing Animals' (R)" }, // Returnable
            { 0x07D7, "Play Track: 'Ceremony' (R)" }, // Returnable
            { 0x07D8, "Play Track: 'Spirit of the Night' (R)" }, // Returnable
            { 0x07D9, "Play Track: 'Mystic Invasion' (R)" }, // Returnable
            { 0x07DA, "Play Track: 'Fond Memories' (R)" }, // Returnable
            { 0x07DB, "Play Track: 'What the Forest Taught Me' (R)" }, // Returnable
            { 0x07DC, "Play Track: 'Steel and Snare' (R)" }, // Returnable
            { 0x07DD, "Play Track: 'A Curious Happening' (R)" }, // Returnable
            { 0x07DE, "Play Track: 'The Little Sprite' (R)" }, // Returnable
            { 0x07DF, "Dummied Out" },

            { 0x07E0, "Dummied Out" },
            { 0x07E1, "Dummied Out" },
            { 0x07E2, "Dummied Out" },
            { 0x07E3, "Dummied Out" },
            { 0x07E4, "Dummied Out" },
            { 0x07E5, "Dummied Out" },
            { 0x07E6, "Dummied Out" },
            { 0x07E7, "Dummied Out" },
            { 0x07E8, "Dummied Out" },
            { 0x07E9, "Dummied Out" },
            { 0x07EA, "Dummied Out" },
            { 0x07EB, "Dummied Out" },
            { 0x07EC, "Dummied Out" },
            { 0x07ED, "Dummied Out" },
            { 0x07EE, "Dummied Out" },
            { 0x07EF, "Dummied Out" },

            { 0x07F0, "Dummied Out" },
            { 0x07F1, "Dummied Out" },
            { 0x07F2, "Dummied Out" },
            { 0x07F3, "Game Over: Print Randi's Name" },
            { 0x07F4, "Game Over: To The Game Over Room (Dummied Out)" },
            { 0x07F5, "On Boss Death: Randi Cheers" },
            { 0x07F6, "On Boss Death: Purim Cheers" },
            { 0x07F7, "On Boss Death: Popoie Cheers" },
            { 0x07F8, "Generic Boss Victory" },
            { 0x07F9, "Dark Lich Post-Boss Cleanup" },
            { 0x07FA, "Game Over: Test For Purim" },
            { 0x07FB, "Game Over: Test For Popoie" },
            { 0x07FC, "Dummied Out" },
            { 0x07FD, "Game Over: Jema Savior" },
            { 0x07FE, "Game Over: Dad Savior" },
            { 0x07FF, "Game Over Handler" },
        };
    }
}