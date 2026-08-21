#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    public sealed class BossAIPlaySoundEffectAction : BossAIOneUShortOperandAction
    {
        public ushort SoundEffectIndex { get { return this.Operand; } }

        public BossAIPlaySoundEffectAction(ushort soundEffectIndex) : base(BossAICommandActionType.PlaySoundEffect, soundEffectIndex) { }
    }
}