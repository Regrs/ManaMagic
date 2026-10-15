using System;

#nullable enable

namespace ManaMagic.Core.Maps
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 00: 201 - None
    /// 01: 33  - SpecialTile
    /// 02: 3
    /// 04: 172 - OnEnter
    /// 08: 4
    /// 10: 10
    /// 20: 10
    /// 40: 10
    /// 80: 10
    /// ------------------------------------------------------------------
    /// From Tomm:
    /// Special Triggers (Map Header)
    /// This adds trigger locations upstream of the map's triggers.
    /// 
    /// They are defined in the 4th byte of the header.
    /// 
    /// 0x00 Nothing
    /// 0x01 Special event location tile BF = assigns a trigger to tile number 0xBF in the tileset; all BF tiles will necessarily be linked to the same event or door.
    /// 0x02 Special event location tile BF and BE Same as above, with one trigger for BF and one trigger for BE.
    /// 0x03 Special event location tile BF, BE, and BD Same as above, with one trigger for BF, one trigger for BE, and one trigger for BD.
    /// 0x04 Walk-on trigger = event that fires when you enter a map through a door(door trigger; if an event makes you use a door, the walk-on event does not fire).
    /// 0x08 Not linked to triggers used for special maps.
    /// 0xF0 Not linked to triggers; means the map is not composed of only one layer
    /// 
    /// You can add these values ​​to combine the effects
    /// 
    /// 0xF5 Corresponds to 0x01 + 0x04 + 0xF0
    /// 
    /// In order, the triggers will fill:
    /// Walk-on trigger(if present)
    /// Tile BF(if present)
    /// Tile BE(if present)
    /// Tile BD(if present)
    /// Map triggers
    /// 
    /// In a special case, the header indicates 0x01, 0x02, or 0x03, and no event is defined for the map.
    /// -> Special tiles will act as "return" triggers.
    /// </remarks>
    [Flags]
    public enum MapEventOptions : byte
    {
        None = 0x00,
        SpecialTile = 0x01,
        Unknown02 = 0x02,
        OnEnter = 0x04,
        Unknown08 = 0x08,
        Unknown10 = 0x10,
        Unknown20 = 0x20,
        Unknown40 = 0x40,
        Unknown80 = 0x80,
        //All = SpecialTile | Unknown02 | OnEnter | Unknown08 | Unknown10 | Unknown20 | Unknown40 | Unknown80,
    }
}