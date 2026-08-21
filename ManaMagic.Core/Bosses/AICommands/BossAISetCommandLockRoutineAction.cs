#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAISetCommandLockRoutineAction : BossAIAddressAction
    {
        public BossAISetCommandLockRoutineAction(ushort lockRoutine) : base(BossAICommandActionType.SetCommandLockRoutine, lockRoutine) { }
    }
}