using System;

#nullable enable

namespace ManaMagic.Core.Maps
{
    /// <summary>
    /// Represents flags that indicate how items are handled on the map.
    /// </summary>
    /// <remarks>
    /// 00: 86 - None
    /// 20: 213 - Dungeon
    /// 40: 137 - MagicRopeAllowed
    /// 80: 103 - FlammieDrumAllowed
    /// </remarks>
    [Flags]
    public enum MapSpecialItemsOptions : byte
    {
        /// <summary>
        /// No item options.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// The map is considered to be a dungeon.
        /// </summary>
        /// <remarks>
        /// This flag ultimately controls where the Magic Rope will send you.
        /// Transitioning from a map without the Dungeon bit set to one that does will set the ROPE_DOOR_INDEX to the value contained in LAST_DOOR_INDEX.
        /// Transitioning between two maps that have the Dungeon bit set will maintain the ROPE_DOOR_INDEX to the value from the original transition.
        /// Transitioning from a map with the Dungeon bit to one without it will set ROPE_DOOR_INDEX to zero. (The Magic Rope will not execute Door 0).
        /// 
        /// (0:Off) <-> (1:On) <-> (2:On) <-> (3:On) <-> (4:Off) <-> (5:On)
        /// Going from 0 to 1 would set the magic rope to return you to 1. It would return you to 1 on 2 and 3 as well.
        /// If you entered map 4 then the item wouldn't work.
        /// If you exit 4 to 5 then the rope would take you to 5.
        /// If you exit 4 to 3 then the rope would take you to 3.
        /// </remarks>
        Dungeon = 0x20,
        /// <summary>
        /// The Magic Rope can be used on this map.
        /// </summary>
        MagicRopeAllowed = 0x40,
        /// <summary>
        /// The Flammie Drum can be used on this map.
        /// </summary>
        FlammieDrumAllowed = 0x80,
        /// <summary>
        /// The map allows the Magic Rope, the Flammie Drum and is a dungeon.
        /// </summary>
        All = Dungeon | MagicRopeAllowed | FlammieDrumAllowed,
    }
    /*
     If $E8 == 0x83 Then Exit
     Else If PrevMap.ItemFlag != 0x20 && NextMap.ItemFlag != 0x20 Then Exit
     Else If PrevMap.ItemFlag == 0x20 && NextMap.ItemFlag == 0x20 Then Exit
     Else If PrevMap.ItemFlag != 0x20 Then $RopeDoor == 0
     Else $RopeDoor = $LastDoor
     */
}