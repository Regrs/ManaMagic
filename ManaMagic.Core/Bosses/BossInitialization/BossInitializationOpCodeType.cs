#nullable enable

namespace ManaMagic.Core.Bosses.BossInitialization
{
    public enum BossInitializationOpCodeType : byte
    {
        //LoadPaletteIntoSlot00 = 0x00,
        LoadPaletteIntoSlot01 = 0x01,
        //DummiedOut02 = 0x02,         // Does nothing.
        //DummiedOut03 = 0x03,         // Does nothing.
        CreateBossObject = 0x04,
        //DummiedOut05 = 0x05,         // Does nothing.
        //DummiedOut06 = 0x06,         // Does nothing.
        //ClearEnemySlotArrayAndBossObjectArray = 0x07,
        //PlayMusic 0x08,
        //LoadGraphics09 = 0x09,
        //LoadGraphics0A = 0x0A,
        //LoadGraphics0B = 0x0B,
        //LoadGraphics0C = 0x0C,
        //LoadGraphics0D = 0x0D,
        //LoadGraphics0E = 0x0E,
        //LoadGraphics0F = 0x0F,
        //LoadGraphics10 = 0x10,
        LoadGraphics11 = 0x11,
        LoadHexasGraphics = 0x12,
        StoreValueAtAddress = 0x13,
        //LoadGraphics14 = 0x14,       // Takes a ushort parameter, then does nothing.
        //LoadGraphics15 = 0x15,       // Takes a ushort parameter, then does nothing.
        InitializeCustomStateMachineBoss = 0x16,
        ClearWRAMSectionBC00 = 0x17,
        //ClearWRAMSectionEC00AndBossObjectsWithDMA = 0x18,
        InitializeStandardStateMachineBoss = 0x19,
        //LoadManaBeastGraphics = 0x1A,
        ClearWRAMSectionBC00AndVRAMWithDMA = 0x1B,
        CallRoutine = 0x1C,
        LoadAndDecompressBossGraphics = 0x1D,
        LoadAndDecompressSlimeGraphics = 0x1E,
        End = 0xFF,
    }
}