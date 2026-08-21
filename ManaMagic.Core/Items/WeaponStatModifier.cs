#nullable enable

namespace ManaMagic.Core.Items
{
    public enum WeaponStatModifier : byte
    {
        None = 0x00,
        PlusOne = 0x01,
        PlusFive = 0x02,
        SubtractFive = 0x03,
    }
}
/*
[Table_WeaponStatModifiers]
004B79: 00                                  ;+0
004B7A: 01                                  ;+1
004B7B: 05                                  ;+5
004B7C: FB                                  ;-5

https://stackoverflow.com/questions/906899/binding-an-enum-to-a-winforms-combo-box-and-then-setting-it
 */