using System;

#nullable enable

namespace ManaMagic.Core.Items
{
    [Flags]
    public enum EquipmentEquippableSettings : byte
    {
        None = 0,
        Randi = 0B_1000_0000,
        Purim = 0B_0100_0000,
        Popoie = 0B_0010_0000,
        All = Randi | Purim | Popoie,
    }
}