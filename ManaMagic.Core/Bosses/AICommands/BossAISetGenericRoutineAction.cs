#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAISetGenericRoutineAction : BossAIAddressAction
    {
        public BossAISetGenericRoutineAction(ushort genericRoutine) : base(BossAICommandActionType.SetGenericRoutine, genericRoutine) { }
    }
}