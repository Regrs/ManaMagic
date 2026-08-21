using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    public abstract class BossAIOneUShortOperandAction : BossAICommandAction
    {
        public ushort Operand { get; }

        public BossAIOneUShortOperandAction(BossAICommandActionType actionType, ushort operand) : base(actionType)
        {
            this.Operand = operand;
        }

        public override void Write(RomWriter romWriter)
        {
            romWriter.Write((byte)this.ActionType);
            romWriter.WriteUInt16(this.Operand);
        }
    }
}