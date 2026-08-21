#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAICallRoutineAction : BossAIAddressAction
    {
        public BossAICallRoutineAction(ushort address) : base(BossAICommandActionType.CallRoutine, address) { }
    }
}