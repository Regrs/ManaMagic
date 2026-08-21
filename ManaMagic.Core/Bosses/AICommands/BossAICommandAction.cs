using System.Diagnostics;
using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    [DebuggerDisplay("{ActionType}")]
    public abstract class BossAICommandAction
    {
        public BossAICommandActionType ActionType { get; }

        public BossAICommandAction(BossAICommandActionType actionType)
        {
            this.ActionType = actionType;
        }

        public abstract void Write(RomWriter romWriter);
    }
}