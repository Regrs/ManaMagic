namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAIScriptJumpAction : BossAIAddressAction
    {
        public BossAIScriptJumpAction(ushort address) : base(BossAICommandActionType.JumpToScriptAddress, address) { }
    }
}