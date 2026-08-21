using System;

#nullable enable

namespace ManaMagic.Core
{
    [Flags]
    public enum MonsterType : byte
    {
        None = 0B_0000_0000,
        Humanoid = 0B_0000_0001,
        PlantFish = 0B_0000_0010,
        Insect = 0B_0000_0100,
        AnimalBird = 0B_0000_1000,
        SlimeLizard = 0B_0001_0000,
        EvilDead = 0B_0010_0000,
        Ghost = 0B_0100_0000,
        Dragon = 0B_1000_0000,
    }
}