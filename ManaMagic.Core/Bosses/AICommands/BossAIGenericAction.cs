using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    public abstract class BossAIGenericAction : BossAICommandAction
    {
        public BossAIGenericAction(BossAICommandActionType actionType) : base(actionType) { }

        public override void Write(RomWriter romWriter)
        {
            romWriter.Write((byte)this.ActionType);
        }
    }
}