#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAISetBossCoordinateRoutineAction : BossAIAddressAction
    {
        public BossAISetBossCoordinateRoutineAction(ushort coordinateRoutine) : base(BossAICommandActionType.SetBossCoordinateRoutine, coordinateRoutine) { }
    }
}