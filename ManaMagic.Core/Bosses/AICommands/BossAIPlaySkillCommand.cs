using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAIPlaySkillCommand : BossAICommandAction
    {
        public byte SkillIndex { get; }
        public byte XCoordinate { get; }
        public byte YCoordinate { get; }

        public BossAIPlaySkillCommand(byte skillID, byte xCoordinate, byte yCoordinate) : base(BossAICommandActionType.PlaySkillAnimation)
        {
            this.SkillIndex = skillID;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }

        public override void Write(RomWriter romWriter)
        {
            romWriter.Write((byte)this.ActionType);
            romWriter.Write(this.SkillIndex);
            romWriter.Write(this.XCoordinate);
            romWriter.Write(this.YCoordinate);
        }
    }
}