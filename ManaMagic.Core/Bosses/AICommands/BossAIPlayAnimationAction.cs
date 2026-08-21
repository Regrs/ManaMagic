namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAIPlayAnimationAction : BossAIOneUShortOperandAction
    {
        public ushort AnimationIndex { get { return this.Operand; } }

        public BossAIPlayAnimationAction(ushort animationIndex) : base(BossAICommandActionType.PlayAnimation, animationIndex) { }
    }
}