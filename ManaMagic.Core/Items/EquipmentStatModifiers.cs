using System;

#nullable enable

namespace ManaMagic.Core.Items
{
    [Flags]
    public enum EquipmentStatModifiers : byte
    {
        StatAdjustmentTypeFlag = 0B_0100_0000,
        Strength = 0B_0001_0000,
        Agility = 0B_0000_1000,
        Constitution = 0B_0000_0100,
        Intelligence = 0B_0000_0010,
        Wisdom = 0B_0000_0001,
    }
}