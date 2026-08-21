#nullable enable

namespace ManaMagic.Core.Bosses
{
    public enum BossFamily
    {
        Unknown,
        Plant,                  // Main Plant and Brambler have no shadow. Pumpkin Bombs use 1. Fucking plant draws its own.
        DragonBody,
        Dragon,                 // C2F01E: 0400            ;Boss Shadow Size.
        Robot,                  // 02D463: 0300            ;Boss Shadow Size.
        KettleKin,
        DeathMachine,
        RobotAuxiliaryHammer,
        RobotAuxiliaryChainsaw, // NA
        Hydra,                  // C25703: A90400 - LDA #$0004, C25720: A90400 - LDA #$0004
        Ant,                    // C2D092: 0400            ;Boss Shadow Size. (Elliot uses 1)
        Minotaur,               // 02D66A: 0700            ;Boss Shadow Size
        Aegagropilon,           // C2D8EA: 0400            ;Boss Shadow Size. (Also uses 5)
        AegagropilonBody,
        Tiger,                  // C2DAFC: 0400            ;Boss Shadow Size. (Also uses 5)
        Lizard,                 // C2DD47: 0200            ;Boss Shadow Size.
        Gigas,                  // C2DFB0: 0400            ;Boss Shadow Size
        GigasAuxiliaryDiamond,
        GigasAuxiliaryOrb,
        Bird,                   // 02E307: 0300            ;Boss Shadow Size.
        MechRider,              // 02EB5B: 0400            ;Boss Shadow Size.
        Serpent,                // C2ED0F: 0000            ;Boss Shadow Size.
        Vampire,                // C2E5BF: 0300            ;Boss Shadow Size. (Also uses 5)
        Lich,                   // 02E840: 0000            ;Boss Shadow Size.
        LichBody,
        LichAuxiliary,
        ManaBeast,
        // ManaBeastRanged,
        SlimeBody,
        Slime,                  // No Shadow
        Wall,
        WallEye,                // No Shadow
        Pumpkin,
        Hexas,                  // C2E132: 0400            ;Boss Shadow Size.
        HexasSerpent,
        Explosion,
        Platform,
        Crystal,

        // Skill Families
        FireBreath,
        FreezeBreath,
        BlitzBreath,
        AcidBreath,
        StatusBubbles,
        GasAttack,
        BeamAttack,
        RingAttack,
        GlareAttack,
        CannonAttack,
        Current,
        BossSpecificAttack,
    }
}