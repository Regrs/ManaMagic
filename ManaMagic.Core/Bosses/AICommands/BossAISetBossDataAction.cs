using System.Diagnostics;

#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    [DebuggerDisplay("{ActionType} [Address: {Address.ToString(\"X4\"),nq}, Value: {Value.ToString(\"X4\"),nq}]")]
    public sealed class BossAISetBossDataAction : BossAITwoUShortOperandsAction
    {
        public ushort Address { get { return this.Operand01; } }
        public ushort Value { get { return this.Operand02; } }

        public BossAISetBossDataAction(ushort address, ushort value) : base(BossAICommandActionType.SetBossData, address, value) { }
    }
}