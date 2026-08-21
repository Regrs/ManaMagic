#nullable enable

namespace ManaMagic.Core.Bosses.Scripts
{
    public enum BossGraphicsScriptOpCode
    {
        LoadPaletteIntoSlot00 = 0x00,
        LoadPaletteIntoSlot01 = 0x01,
        DummiedOut02 = 0x02,
        DummiedOut03 = 0x03,
        CreateBossObject = 0x04,
        DummiedOut05 = 0x05,
        DummiedOut06 = 0x06,
        ClearEnemySlotArrayAndBossObjectArray = 0x07,
        PlayMusic = 0x08,
        LoadGraphics09 = 0x09,
        LoadGraphics0A = 0x0A,
        LoadGraphics0B = 0x0B,
        LoadGraphics0C = 0x0C,
        LoadGraphics0D = 0x0D,
        LoadGraphics0E = 0x0E,
        LoadGraphics0F = 0x0F,
        LoadGraphics10 = 0x10,
        LoadGraphics11 = 0x11,
        LoadHexasGraphics = 0x12,
        StoreValueAtAddress = 0x13,
        DummiedOutLoadGraphics14 = 0x14,
        DummiedOutLoadGraphics15 = 0x15,
        InitializeCustomBoss = 0x16,
        ClearWRAMSectionBC00 = 0x17,
        ClearWRAMSectionEC00AndBossObjectsWithDMA = 0x18,
        InitializeStructuredBoss = 0x19,
        LoadManaBeastGraphics = 0x1A,
        ClearWRAMSectionBC00AndVRAMWithDMA = 0x1B,
        CallRoutine = 0x1C,
        LoadAndDecompressBossGraphics = 0x1D,
        LoadAndDecompressSlimeGraphics = 0x1E,
        EndScript = 0xFF,
    }
}