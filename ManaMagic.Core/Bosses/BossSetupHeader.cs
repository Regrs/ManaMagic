using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Bosses
{
    public sealed record BossSetupHeader : NotifyRecordPropertyChanged
    {
        public static BossSetupHeader Empty { get; } = new BossSetupHeader(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        private BossControlFlags controlFlags = BossControlFlags.None;

        public BossControlFlags ControlFlags
        {
            get { return this.controlFlags; }
            set { this.SetProperty(ref this.controlFlags, value); }
        }

        public byte AIFrame
        {
            get { return (byte)((((ushort)this.controlFlags) >> 8) & 0x03); }
            set
            {
                ushort frameFlags = (ushort)((value & 0x03) << 8);
                ushort flags = (ushort)((ushort)this.controlFlags & 0xFCFF);
                this.SetProperty(ref this.controlFlags, (BossControlFlags)(flags | frameFlags));
            }
        }

        public ushort SpriteFlags { get; }
        public ushort InitializeRoutinePointer { get; }
        public ushort MovementRoutinePointer { get; }
        public ushort AttackRoutinePointer { get; }
        public ushort DamagedRoutinePointer { get; }
        public ushort DefaultCommandScriptPointer { get; }
        public ushort DeathCommandScriptPointer { get; }
        public ushort AICommandSetPointer { get; }
        public ushort ShadowSize { get; }
        public ushort PostDeathHandlerPointer { get; }
        public ushort UnknownValue { get; set; }
        public ushort SpecialDeathHandlerPointer { get; }

        public BossSetupHeader(byte index,
                               ushort controlFlags,
                               ushort spriteFlags,
                               ushort initPointer,
                               ushort movePointer,
                               ushort atkPointer,
                               ushort dmgPointer,
                               ushort defaultCommandPointer,
                               ushort deathCommandPointer,
                               ushort commandSetPointer,
                               ushort shadowSize,
                               ushort postDeathHandler,
                               ushort unknown,
                               ushort specialDeathPointer,
                               bool userModified = false) : base(index, userModified)
        {
            this.controlFlags = (BossControlFlags)controlFlags;
            this.SpriteFlags = spriteFlags;
            this.InitializeRoutinePointer = initPointer;
            this.MovementRoutinePointer = movePointer;
            this.AttackRoutinePointer = atkPointer;
            this.DamagedRoutinePointer = dmgPointer;
            this.DefaultCommandScriptPointer = defaultCommandPointer;
            this.DeathCommandScriptPointer = deathCommandPointer;
            this.AICommandSetPointer = commandSetPointer;
            this.ShadowSize = shadowSize;
            this.PostDeathHandlerPointer = postDeathHandler;
            this.UnknownValue = unknown;
            this.SpecialDeathHandlerPointer = specialDeathPointer;
        }
    }
}