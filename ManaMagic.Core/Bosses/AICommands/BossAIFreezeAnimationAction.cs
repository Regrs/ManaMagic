namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAIFreezeAnimationAction : BossAIOneUShortOperandAction
    {
        public ushort FreezeTimer { get { return this.Operand; } }

        public BossAIFreezeAnimationAction(ushort freezeTimer) : base(BossAICommandActionType.FreezeAnimation, freezeTimer) { }
    }
}