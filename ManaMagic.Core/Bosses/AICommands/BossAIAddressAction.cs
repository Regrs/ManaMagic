#nullable enable

using System.Diagnostics;

namespace ManaMagic.Core.Bosses.AICommands
{
    [DebuggerDisplay("{ActionType} [Address: {Address.ToString(\"X4\"),nq}]")]
    public abstract class BossAIAddressAction : BossAIOneUShortOperandAction
    {
        public ushort Address { get { return this.Operand; } }

        public BossAIAddressAction(BossAICommandActionType actionType, ushort address) : base(actionType, address) { }
    }
}