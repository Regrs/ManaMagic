using ZwellTech.SuperNintendo;

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAISetBossCoordinateSpeedAction : BossAICommandAction
    {
        public ushort XSubPixelSpeed { get; }
        public ushort XSpeed { get; }
        public ushort YSubPixelSpeed { get; }
        public ushort YSpeed { get; }
        public ushort ZSubPixelSpeed { get; }
        public ushort ZSpeed { get; }

        public BossAISetBossCoordinateSpeedAction(ushort xSubPixelSpeed, ushort xSpeed,
                                                  ushort ySubPixelSpeed, ushort ySpeed,
                                                  ushort zSubPixelSpeed, ushort zSpeed) : base(BossAICommandActionType.SetBossCoordinateSpeed)
        {
            this.XSubPixelSpeed = xSubPixelSpeed;
            this.XSpeed = xSpeed;
            this.YSubPixelSpeed = ySubPixelSpeed;
            this.YSpeed = ySpeed;
            this.ZSubPixelSpeed = zSubPixelSpeed;
            this.ZSpeed = zSpeed;
        }

        public override void Write(RomWriter romWriter)
        {
            romWriter.Write((byte)this.ActionType);
            romWriter.WriteUInt16(this.XSubPixelSpeed);
            romWriter.WriteUInt16(this.XSpeed);
            romWriter.WriteUInt16(this.YSubPixelSpeed);
            romWriter.WriteUInt16(this.YSpeed);
            romWriter.WriteUInt16(this.ZSubPixelSpeed);
            romWriter.WriteUInt16(this.ZSpeed);
        }
    }
}