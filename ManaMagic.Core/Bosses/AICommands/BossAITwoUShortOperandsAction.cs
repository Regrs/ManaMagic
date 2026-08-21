using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    public abstract class BossAITwoUShortOperandsAction : BossAICommandAction
    {
        public ushort Operand01 { get; }
        public ushort Operand02 { get; }

        public BossAITwoUShortOperandsAction(BossAICommandActionType actionType, ushort operand01, ushort operand02) : base(actionType)
        {
            this.Operand01 = operand01;
            this.Operand02 = operand02;
        }

        public override void Write(RomWriter romWriter)
        {
            romWriter.Write((byte)this.ActionType);
            romWriter.WriteUInt16(this.Operand01);
            romWriter.WriteUInt16(this.Operand02);
        }
    }
}