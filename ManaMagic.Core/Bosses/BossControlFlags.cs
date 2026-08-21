using System;

namespace ManaMagic.Core.Bosses
{
    [Flags]
    public enum BossControlFlags : ushort
    {
        None = 0B_0000_0000_0000_0000,
        UnknownBit16 = 0B_1000_0000_0000_0000, //8000
        DebugSkipFrameRuleCheck = 0B_0100_0000_0000_0000, //4000
        IsChild = 0B_0010_0000_0000_0000, //2000
        PlayerCollisionEnabled = 0B_0001_0000_0000_0000, //1000
        BossEnabled = 0B_0000_1000_0000_0000, //0800
        MapCollisionEnabled = 0B_0000_0100_0000_0000, //0400
        FrameRuleH = 0B_0000_0010_0000_0000, //0200
        FrameRuleL = 0B_0000_0001_0000_0000, //0100
        VerticalFlip = 0B_0000_0000_1000_0000, //0080
        HorizontalFlip = 0B_0000_0000_0100_0000, //0040
        UnknownBit06 = 0B_0000_0000_0010_0000, //0020
        UnknownBit05 = 0B_0000_0000_0001_0000, //0010
        IsBackgroundSprite = 0B_0000_0000_0000_1000, //0008
        UnknownBit03 = 0B_0000_0000_0000_0100, //0004
        CollidesWithLayer2 = 0B_0000_0000_0000_0010, //0002
        CollidesWithLayer1 = 0B_0000_0000_0000_0001, //0001
    }
}