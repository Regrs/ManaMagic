#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public readonly struct EchoSettings
    {
        public byte EchoDelay { get; }
        public sbyte EchoVolume { get; }
        public sbyte EchoFeedback { get; }
        public byte FilterCoefficientIndex { get; }

        public EchoSettings(byte echoDelay, sbyte echoVolume, sbyte echoFeedback, byte filterIndex)
        {
            this.EchoDelay = echoDelay;
            this.EchoVolume = echoVolume;
            this.EchoFeedback = echoFeedback;
            this.FilterCoefficientIndex = filterIndex;
        }
    }
}