namespace ManaMagic.Core.Bosses.AICommands
{
    public enum BossAICommandActionType : byte
    {
        PlayAnimation = 0x00,
        SetBossCoordinateSpeed = 0x01,
        SetBossCoordinateRoutine = 0x02,
        SetGenericRoutine = 0x03,
        SetCommandLockRoutine = 0x04,
        FreezeAnimation = 0x05,
        SetHorizontalFlip = 0x06,
        //SetVerticalFlip = 0x07,
        //ToggleHorizontalFlip = 0x08,
        //ToggleVerticalFlip = 0x09,
        ClearHorizontalFlip = 0x0A,
        //ClearVerticalFlip = 0x0B,
        //SetBothBossWeaponsBroken = 0x0C, // Dummied Out
        //SetBossShadow = 0x0D, // Dummied Out
        CastSpell = 0x0E,
        PlaySkillAnimation = 0x0F,
        CallRoutine = 0x10,
        SetBossData = 0x11,
        // Unknown12 = 0x12, // Calls an animation based routine in Bank 01.
        EquipBossWeapon = 0x13,
        PlaySoundEffect = 0x14,
        JumpToScriptAddress = 0x15,
        EndCommand = 0xFF,
    }
}