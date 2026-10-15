#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public sealed class AkaoInstrument
    {
        public BitRateReductionSample Sample { get; }
        public ushort Loopback { get; }
        public ushort FrequencyMultiplier { get; }
        public ADSR ADSR { get; }

        public AkaoInstrument(BitRateReductionSample sample, ushort loopback, ushort freqMultiplier, ADSR adsr)
        {
            this.Sample = sample;
            this.Loopback = loopback;
            this.FrequencyMultiplier = freqMultiplier;
            this.ADSR = adsr;
        }
    }
}