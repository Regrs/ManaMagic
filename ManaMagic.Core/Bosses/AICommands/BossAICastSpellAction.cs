using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAICastSpellAction : BossAICommandAction
    {
        public ManaSpell SpellId { get; }
        public byte Target { get; }

        public BossAICastSpellAction(ManaSpell spellId, byte target) : base(BossAICommandActionType.CastSpell)
        {
            this.SpellId = spellId;
            this.Target = target;
        }

        public override void Write(RomWriter romWriter)
        {
            romWriter.Write((byte)this.ActionType);
            romWriter.Write((byte)this.SpellId);
            romWriter.Write(this.Target);
        }
    }
}