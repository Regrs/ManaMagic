using System.Collections.Generic;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Audio;
using ZwellTech.SuperNintendo.Audio.Akao;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class AudioRomReader : RomReader
    {
        public AudioRomReader(RomFile rom) : base(rom) { }

        public DataTable<uint> ReadAkaoSongSequencePointerTable()
        {
            this.Seek(Constants.Bank03.AkaoSongSequencePointerTableAddress);

            List<uint> pointerTable = new List<uint>((int)Constants.Bank03.AkaoSongSequencePointerTableSize);
            for (int i = 0; i < Constants.Bank03.AkaoSongSequencePointerTableSize; i++)
            {
                uint pointer = this.ReadUInt24() & 0x3FFFFF;
                pointerTable.Add(pointer);
            }
            return new DataTable<uint>(pointerTable);
        }

        public DataTable<byte> ReadAkaoMessageParameterCountTable()
        {
            this.Seek(Constants.Bank03.AkaoMessageParameterCountTableAddress);

            List<byte> paramCounts = new List<byte>((int)Constants.Bank03.AkaoMessageParameterCountTableSize);
            for (int i = 0; i < Constants.Bank03.AkaoMessageParameterCountTableSize; i++)
            {
                byte count = this.Read();
                paramCounts.Add(count);
            }
            return new DataTable<byte>(paramCounts);
        }

        public DataTable<AkaoTrack> ReadAkaoTracks(DataTable<uint> sequencePointerTable, DataTable<byte> parameterCountTable, List<List<AkaoInstrument>> trackInstrumentList)
        {
            AkaoSequenceParser parser = new AkaoSequenceParser(this, parameterCountTable);

            List<AkaoTrack> tracks = new List<AkaoTrack>();
            for (int i = 0; i < sequencePointerTable.RowCount; i++)
            {
                uint pointer = sequencePointerTable[i];
                AkaoTrack track = parser.ParseAkaoTrack(pointer, trackInstrumentList[i]);
                tracks.Add(track);
            }

            return new DataTable<AkaoTrack>(tracks);
        }

        public DataTable<uint> ReadTrackInstrumentPointerTable()
        {
            this.Seek(Constants.Bank03.InstrumentPointerTableAddress);

            List<uint> pointerTable = new List<uint>((int)Constants.Bank03.InstrumentTableSize);
            for (int i = 0; i < Constants.Bank03.InstrumentTableSize; i++)
            {
                uint pointer = this.ReadUInt24() & 0x3FFFFF;
                pointerTable.Add(pointer);
            }
            return new DataTable<uint>(pointerTable);
        }

        public DataTable<ushort> ReadTrackInstrumentLoopbackTable()
        {
            this.Seek(Constants.Bank03.InstrumentLoopbackTableAddress);

            List<ushort> pointerTable = new List<ushort>((int)Constants.Bank03.InstrumentTableSize);
            for (int i = 0; i < Constants.Bank03.InstrumentTableSize; i++)
            {
                ushort pointer = this.ReadUInt16();
                pointerTable.Add(pointer);
            }
            return new DataTable<ushort>(pointerTable);
        }

        public DataTable<ushort> ReadTrackInstrumentFrequencyMultiplierTable()
        {
            this.Seek(Constants.Bank03.InstrumentFrequencyMultiplierTableAddress);

            List<ushort> pointerTable = new List<ushort>((int)Constants.Bank03.InstrumentTableSize);
            for (int i = 0; i < Constants.Bank03.InstrumentTableSize; i++)
            {
                ushort pointer = this.ReadUInt16();
                pointerTable.Add(pointer);
            }
            return new DataTable<ushort>(pointerTable);
        }

        public DataTable<ADSR> ReadTrackInstrumentAdsrTableAddressTable()
        {
            this.Seek(Constants.Bank03.InstrumentAdsrTableAddress);

            List<ADSR> adsrTable = new List<ADSR>((int)Constants.Bank03.InstrumentTableSize);
            for (int i = 0; i < Constants.Bank03.InstrumentTableSize; i++)
            {
                byte vxADSR1 = this.Read();
                byte vxADSR2 = this.Read();
                adsrTable.Add(new ADSR(vxADSR1, vxADSR2));
            }
            return new DataTable<ADSR>(adsrTable);
        }

        public DataTable<BitRateReductionSample> ReadTrackInstrumentSamples(DataTable<uint> instrumentPointerTable, DataTable<ushort> instrumentLoopbackTable)
        {
            List<BitRateReductionSample> samples = new List<BitRateReductionSample>();
            int index = 0;
            foreach (uint pointer in instrumentPointerTable)
            {
                this.Seek((int)pointer);

                // First two bytes are the size of the entire BRR sample in bytes.
                ushort sizeInBytes = this.ReadUInt16();
                int size = sizeInBytes / 9;

                List<BitRateReductionBlock> sample = new List<BitRateReductionBlock>();
                for (int i = 0; i < size; i++)
                {
                    byte control = this.Read();
                    byte sample1 = this.Read();
                    byte sample2 = this.Read();
                    byte sample3 = this.Read();
                    byte sample4 = this.Read();
                    byte sample5 = this.Read();
                    byte sample6 = this.Read();
                    byte sample7 = this.Read();
                    byte sample8 = this.Read();
                    BitRateReductionBlock block = new BitRateReductionBlock(control, sample1, sample2, sample3, sample4, sample5, sample6, sample7, sample8);
                    sample.Add(block);
                }
                samples.Add(new BitRateReductionSample(instrumentLoopbackTable[index], sample));
                index++;
            }

            return new DataTable<BitRateReductionSample>(samples);
        }

        public DataTable<DataTable<UShortValue>> ReadTrackInstrumentTable()
        {
            this.Seek(Constants.Bank03.TrackInstrumentTableAddress);
            List<DataTable<UShortValue>> trackInstrumentList = new List<DataTable<UShortValue>>((int)Constants.Bank03.TrackInstrumentTableSize);
            for (int i = 0; i < Constants.Bank03.TrackInstrumentTableSize; i++)
            {
                List<UShortValue> instruments = new List<UShortValue>((int)Constants.Bank03.InstrumentsPerTrack);
                for (int j = 0; j < Constants.Bank03.InstrumentsPerTrack; j++)
                {
                    instruments.Add(new UShortValue((byte)j, this.ReadUInt16()));
                }
                trackInstrumentList.Add(new DataTable<UShortValue>(instruments));
            }
            return new DataTable<DataTable<UShortValue>>(trackInstrumentList);
        }

        public DataTable<EchoSettings> ReadEchoSettingsTable()
        {
            this.Seek(Constants.Bank03.EchoSettingsTableAddress);

            List<EchoSettings> echoSettingsList = new List<EchoSettings>((int)Constants.Bank03.EchoSettingsTableAddressSize);
            for (int i = 0; i < Constants.Bank03.EchoSettingsTableAddressSize; i++)
            {
                byte echoDelay = this.Read();
                sbyte echoVolume = this.ReadSByte();
                sbyte echoFeedback = this.ReadSByte();
                byte filterIndex = this.Read();
                echoSettingsList.Add(new EchoSettings(echoDelay, echoVolume, echoFeedback, filterIndex));
            }
            return new DataTable<EchoSettings>(echoSettingsList);
        }

        public DataTable<ushort> ReadNotePitchTable()
        {
            this.Seek(Constants.Bank03.AkaoNotePitchesTableAddress);

            List<ushort> pitchTable = new List<ushort>((int)Constants.Bank03.AkaoNotePitchesTableSize);
            for (int i = 0; i < Constants.Bank03.AkaoNotePitchesTableSize; i++)
            {
                ushort pitch = this.ReadUInt16();
                pitchTable.Add(pitch);
            }
            return new DataTable<ushort>(pitchTable);
        }

        public DataTable<byte> ReadNoteDurationTable()
        {
            this.Seek(Constants.Bank03.AkaoNoteDurationTableAddress);

            List<byte> durationTable = new List<byte>((int)Constants.Bank03.AkaoNoteDurationTableSize);
            for (int i = 0; i < Constants.Bank03.AkaoNoteDurationTableSize; i++)
            {
                byte duration = this.Read();
                durationTable.Add(duration);
            }
            return new DataTable<byte>(durationTable);
        }

        public DataTable<FiniteImpulseResponseFilter> ReadEchoFilterCoefficientTable()
        {
            this.Seek(Constants.Bank03.EchoFilterCoefficientTableAddress);

            List<FiniteImpulseResponseFilter> coefficientTable = new List<FiniteImpulseResponseFilter>((int)Constants.Bank03.EchoFilterCoefficientTableSize);
            for (int i = 0; i < Constants.Bank03.EchoFilterCoefficientTableSize; i++)
            {
                sbyte tap0 = this.ReadSByte();
                sbyte tap1 = this.ReadSByte();
                sbyte tap2 = this.ReadSByte();
                sbyte tap3 = this.ReadSByte();
                sbyte tap4 = this.ReadSByte();
                sbyte tap5 = this.ReadSByte();
                sbyte tap6 = this.ReadSByte();
                sbyte tap7 = this.ReadSByte();
                coefficientTable.Add(new FiniteImpulseResponseFilter(tap0, tap1, tap2, tap3, tap4, tap5, tap6, tap7));
            }
            return new DataTable<FiniteImpulseResponseFilter>(coefficientTable);
        }
    }
}