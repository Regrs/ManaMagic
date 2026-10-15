using System.Collections.Generic;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Audio;
using ZwellTech.SuperNintendo.Audio.Akao;

#nullable enable

namespace ManaMagic.Core.Audio
{
    public sealed class AudioContext
    {
        private DataTable<uint> AkaoTrackSequencePointerTable = DataTable<uint>.Empty;
        private DataTable<ADSR> TrackInstrumentAdsrTable = DataTable<ADSR>.Empty;
        private DataTable<ushort> TrackInstrumentFrequencyMultiplierTable = DataTable<ushort>.Empty;

        public DataTable<ushort> NotePitchTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<byte> NoteDurationTable { get; private set; } = DataTable<byte>.Empty;

        public DataTable<EchoSettings> EchoSettingsTable { get; private set; } = DataTable<EchoSettings>.Empty;
        public DataTable<FiniteImpulseResponseFilter> EchoFilterCoefficientTable { get; private set; } = DataTable<FiniteImpulseResponseFilter>.Empty;

        public DataTable<uint> TrackInstrumentSamplePointerTable { get; private set; } = DataTable<uint>.Empty;
        public DataTable<BitRateReductionSample> TrackInstrumentSamples { get; private set; } = DataTable<BitRateReductionSample>.Empty;
        public DataTable<ushort> TrackInstrumentLoopbackTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<byte> AkaoMessageParameterCountTable { get; private set; } = DataTable<byte>.Empty;
        public DataTable<DataTable<UShortValue>> TrackInstrumentUsageTable { get; private set; } = DataTable<DataTable<UShortValue>>.Empty;

        public DataTable<AkaoInstrument> AkaoInstruments { get; private set; } = DataTable<AkaoInstrument>.Empty;
        public DataTable<AkaoTrack> AkaoTracks { get; private set; } = DataTable<AkaoTrack>.Empty;

        /// <summary>
        /// Initializes the context by reading audio data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            AudioRomReader reader = RomReaderFactory.GetRomReader<AudioRomReader>();

            // Note Data
            this.NotePitchTable = reader.ReadNotePitchTable();
            this.NoteDurationTable = reader.ReadNoteDurationTable();

            // Echo Buffer Data
            this.EchoSettingsTable = reader.ReadEchoSettingsTable();
            this.EchoFilterCoefficientTable = reader.ReadEchoFilterCoefficientTable();

            // Track Instruments
            this.TrackInstrumentSamplePointerTable = reader.ReadTrackInstrumentPointerTable();
            this.TrackInstrumentLoopbackTable = reader.ReadTrackInstrumentLoopbackTable();
            this.TrackInstrumentFrequencyMultiplierTable = reader.ReadTrackInstrumentFrequencyMultiplierTable();
            this.TrackInstrumentAdsrTable = reader.ReadTrackInstrumentAdsrTableAddressTable();
            this.TrackInstrumentSamples = reader.ReadTrackInstrumentSamples(this.TrackInstrumentSamplePointerTable, this.TrackInstrumentLoopbackTable);
            this.AkaoInstruments = this.CreateAkaoInstrumentTable();

            // Tracks
            this.AkaoTrackSequencePointerTable = reader.ReadAkaoSongSequencePointerTable();
            this.AkaoMessageParameterCountTable = reader.ReadAkaoMessageParameterCountTable();
            this.TrackInstrumentUsageTable = reader.ReadTrackInstrumentTable();

            List<List<AkaoInstrument>> trackInstrumentList = this.CreateTrackInstrumentTable();
            this.AkaoTracks = reader.ReadAkaoTracks(this.AkaoTrackSequencePointerTable, this.AkaoMessageParameterCountTable, trackInstrumentList);
        }

        private DataTable<AkaoInstrument> CreateAkaoInstrumentTable()
        {
            List<AkaoInstrument> instruments = new List<AkaoInstrument>();
            for (int i = 0; i < Constants.Bank03.InstrumentTableSize; i++)
            {
                BitRateReductionSample sample = this.TrackInstrumentSamples[i];
                ushort loopback = this.TrackInstrumentLoopbackTable[i];
                ushort freqMultiplier = this.TrackInstrumentFrequencyMultiplierTable[i];
                ADSR adsr = this.TrackInstrumentAdsrTable[i];

                AkaoInstrument instrument = new AkaoInstrument(sample, loopback, freqMultiplier, adsr);
                instruments.Add(instrument);
            }
            return new DataTable<AkaoInstrument>(instruments);
        }

        private List<List<AkaoInstrument>> CreateTrackInstrumentTable()
        {
            List<List<AkaoInstrument>> trackInstrumentList = new List<List<AkaoInstrument>>();
            for (int i = 0; i < this.TrackInstrumentUsageTable.RowCount; i++)
            {
                DataTable<UShortValue> usageTable = this.TrackInstrumentUsageTable[i];
                List<AkaoInstrument> instruments = new List<AkaoInstrument>();
                foreach (UShortValue value in usageTable)
                {
                    // Indexes in the track instrument table are ones-based.
                    // A value of zero means no instrument should be loaded for that index.
                    if (value.Value > 0)
                    {
                        instruments.Add(this.AkaoInstruments[value.Value - 1]);
                    }
                }
                trackInstrumentList.Add(instruments);
            }
            return trackInstrumentList;
        }
    }
}

/*
        private void StepVoiceVolumeEnvelopes(AkaoVoice voice)
        {
            // TODO: Volume Envelope
            // TODO: Pan Envelope
            // TODO: Pitch Envelope
            if (voice.PanLFO.Enabled)
            {
                short newFrequency = (short)(voice.PanLfoAmplitude + voice.PanLfoFrequency);
                if ((voice.PanLfoAmplitude >= 0) && (voice.PanLfoFrequency >= 0) && (newFrequency < 0))
                {
                    newFrequency = (short)((newFrequency & 0x00FF) | 0x7F00);
                }
                else if ((voice.PanLfoAmplitude < 0) && (voice.PanLfoFrequency < 0) && (newFrequency >= 0))
                {
                    newFrequency = (short)((newFrequency & 0x00FF) | 0x8000);
                }

                if ((newFrequency & 0xFF00) != (voice.PanLfoFrequency & 0xFF00))
                {
                    voice.PanLfoOffset = (sbyte)((newFrequency >> 8) & 0xFF);
                    voice.PanLfoFrequency = newFrequency;

                    // Pan changed, trigger a DSP volume update at the end of the step.
                    this.enableVolumeUpdate |= voice.VoiceEnable;
                }

                voice.PanLfoAmplitudeDuration--;
                if (voice.PanLfoAmplitudeDuration == 0)
                {
                    voice.PanLfoAmplitude = (short)(voice.PanLfoAmplitude ^ 0xFFFF);
                    voice.PanLfoAmplitude++;
                    voice.PanLfoAmplitudeDuration = voice.PanLfoBaseDuration;
                }
            }
        }

        private void EnablePanLfo(AkaoVoice voice, PanLFOOnAkaoMessage message)
        {
            voice.PanLfoBaseDuration = (byte)(message.Duration + 1);
            voice.PanLfoAmplitudeDuration = voice.PanLfoBaseDuration > 0 ? (byte)(voice.PanLfoBaseDuration >> 1) : (byte)128;

            byte amplitudeL = message.Amplitude > 0 ? (byte)message.Amplitude : (byte)((message.Amplitude & 0x7F) ^ 0x7F);
            byte amplitudeH = 0;
            voice.PanLfoAmplitude = (short)((amplitudeH << 8) | amplitudeL);
            if (voice.PanLfoAmplitudeDuration > 0)
            {
                amplitudeH = (byte)((amplitudeL / voice.PanLfoAmplitudeDuration) & 0xFF);
                amplitudeL = (byte)((((amplitudeL % voice.PanLfoAmplitudeDuration) << 8) / voice.PanLfoAmplitudeDuration) & 0xFF);
                if (amplitudeL == 0)
                {
                    amplitudeL++;
                }

                voice.PanLfoAmplitude = (short)((amplitudeH << 8) | amplitudeL);
                if (message.Amplitude < 0)
                {
                    voice.PanLfoAmplitude = (short)(voice.PanLfoAmplitude ^ 0xFFFF);
                    voice.PanLfoAmplitude++;
                }
            }

            // The driver will simply write the amplitude value from the message to the enabled byte.
            // Its not used for any other purpose other then making the value not zero.
            voice.PanLfoEnabled = true;
            voice.PanLfoFrequency = 0;
            voice.PanLfoOffset = 0;
        }

        private void DisablePanLfo(AkaoVoice voice)
        {
            // The driver writes zero (which happens to come from the number of parameters this message has) to disable the LFO.
            voice.PanLfoEnabled = false;
            voice.PanLfoFrequency = 0;
            voice.PanLfoOffset = 0;
        }
        //public bool PanLfoEnabled = false; // $0141
        //public byte PanLfoBaseDuration = 0; // $FCC0
        //public byte PanLfoAmplitudeDuration = 0; // $FD00
        //public short PanLfoAmplitude = 0; // $FDA0
        //public short PanLfoFrequency = 0; // $FDE0
        //public sbyte PanLfoOffset = 0; // $FDE1
        private void PlayNote(AkaoVoice voice, AkaoMessage message)
        {
            byte durationIndex = 0;
            if (message is RestAkaoMessage restMessage)
            {
                durationIndex = (byte)(restMessage.Note % 15);
                //voice.Pitch = 0;
                //voice.IsTie = false;
            }
            else if (message is TieAkaoMessage tieMessage)
            {
                durationIndex = (byte)(tieMessage.Note % 15);
                this.enablePitchUpdate |= voice.VoiceEnable;
                this.keyOn |= voice.VoiceEnable;
                //voice.IsTie = true;
            }
            else if (message is NoteAkaoMessage noteMessage)
            {
                durationIndex = (byte)(noteMessage.Note % 15);
                byte baseNoteIndex = (byte)(noteMessage.Note / 15);
                byte calcNoteIndex = (byte)(baseNoteIndex + (voice.Octave * 12) + voice.Transpose - 10);

                byte octave = (byte)(calcNoteIndex / 12);
                byte noteIndex = (byte)(calcNoteIndex % 12);

                ushort notePitch = this.notePitches[noteIndex];
                ushort instrumentPitch = this.instruments[voice.InstrumentIndex].FrequencyMultiplier;

                byte instrumentPitchL = (byte)(instrumentPitch & 0xFF);
                byte instrumentPitchH = (byte)((instrumentPitch >> 8) & 0xFF);

                byte notePitchL = (byte)(notePitch & 0xFF);
                byte notePitchH = (byte)((notePitch >> 8) & 0xFF);

                // If I knew anything about audio mixing this equation might make sense instead of being just insane math.
                // Secret of Mana, Final Fantasy 5 and Final Fantasy 6 all use this equation.
                // This somehow produces a 2.12 fixed point value.
                // Note pitches are probably also 2.12 fixed point values, but the instrument pitch might actually be two bytes instead of a ushort.
                instrumentPitchL += voice.Detune;
                ushort pitch1 = (ushort)((ushort)(notePitchH * instrumentPitchL) + (ushort)(((notePitchL * instrumentPitchL) >> 8) & 0x00FF));
                ushort pitch2 = 0;
                if (instrumentPitchH > 0)
                {
                    pitch2 = (ushort)(((instrumentPitchH * (byte)((pitch1 >> 8) & 0xFF)) >> 8) & 0x00FF);
                }

                ushort pitch = (ushort)(pitch1 + pitch2);
                if ((sbyte)(instrumentPitchL) >= 0)
                {
                    pitch += notePitch;
                }

                if (octave > AkaoDriver.RecordedNoteOctave) { pitch <<= (octave - AkaoDriver.RecordedNoteOctave); }
                else if (octave < AkaoDriver.RecordedNoteOctave) { pitch >>= (AkaoDriver.RecordedNoteOctave - octave); }

                voice.Pitch = pitch;
                this.enablePitchUpdate |= voice.VoiceEnable;
                this.keyOn |= voice.VoiceEnable;
                //voice.IsTie = false;
            }
            else { ThrowHelper.ThrowInvalidOperationException("Not a note message."); }

            // Notes, but not Rests or Ties, will key off two ticks before the note actually ends, Ties will not key off at all.
            //voice.Duration = (int)(32000 * (this.noteDurations[durationIndex] * this.tempoMS));
            //voice.KeyOffDuration = (int)(32000 * ((this.noteDurations[durationIndex] - 2) * this.tempoMS));

            voice.Duration = this.noteDurations[durationIndex];

            //voice.Registers.VxPITCH = voice.Pitch;
            //voice.Playing = true;
        }
        private void DebugPitchCalculation()
        {
            ushort notePitch = this.notePitches[0];
            ushort instrumentPitch = this.instruments[0].FrequencyMultiplier;
            byte octave = AkaoDriver.RecordedNoteOctave;

            byte instrumentPitchL = (byte)(instrumentPitch & 0xFF);
            byte instrumentPitchH = (byte)((instrumentPitch >> 8) & 0xFF);

            byte notePitchL = (byte)(notePitch & 0xFF);
            byte notePitchH = (byte)((notePitch >> 8) & 0xFF);

            ushort pitchPart1 = (ushort)(notePitchH * instrumentPitchL);
            ushort pitchPart2 = (ushort)(((notePitchL * instrumentPitchL) >> 8) & 0x00FF);
            ushort pitch1 = (ushort)(pitchPart1 + pitchPart2);
            ushort pitch2 = 0;
            if (instrumentPitchH > 0)
            {
                byte pitchH = (byte)((pitch1 >> 8) & 0xFF);
                pitch2 = (ushort)(((instrumentPitchH * pitchH) >> 8) & 0x00FF);
            }
            ushort pitch = (ushort)(pitch1 + pitch2);
            if ((sbyte)(instrumentPitchL) >= 0)
            {
                pitch += notePitch;
            }

            if (octave > 4)
            {
                int currentOctave = 4;
                while (currentOctave != octave)
                {
                    pitch <<= 1;
                    currentOctave++;
                }
            }
            else if (octave < 4)
            {
                int currentOctave = 4;
                while (currentOctave != octave)
                {
                    pitch >>= 1;
                    currentOctave--;
                }
            }
        }

        #region Step MS
        private readonly double timer0TickMS = 0d;
        private double tempoMS = 0d;

        private void RunMS()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                this.StepVoiceMS(this.voice0);
                this.StepVoiceMS(this.voice1);
                this.StepVoiceMS(this.voice2);
                this.StepVoiceMS(this.voice3);
                this.StepVoiceMS(this.voice4);
                this.StepVoiceMS(this.voice5);
                this.StepVoiceMS(this.voice6);
                this.StepVoiceMS(this.voice7);

                this.waveformL.Add(this.sdsp.MainStereoOutL);
                this.waveformR.Add(this.sdsp.MainStereoOutR);

                if (this.IsTrackComplete()) { break; }
                this.sdsp.Step();
            }

            watch.Stop();
            ManaDebugger.DebugPrint(watch.Elapsed.ToString());

            if (this.voice0.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice0 has one or more active loops."); }
        }

        private void StepVoiceMS(AkaoVoice voice)
        {
            if (voice.CanRead)
            {
                if (voice.Playing)
                {
                    _ = voice.Tick();
                    if (voice.ShouldKeyOff)
                    {
                        voice.KeyedOff = true;
                        this.sdsp.Registers.KON &= ~voice.VoiceEnable;
                        this.sdsp.Registers.KOFF |= voice.VoiceEnable;
                    }
                }
                while (!voice.Playing && voice.CanRead)
                {
                    this.ParseNextMessage(voice);
                    if (voice.Playing)
                    {
                        voice.KeyedOff = false;
                        this.sdsp.Registers.KON |= voice.VoiceEnable;
                        this.sdsp.Registers.KOFF &= ~voice.VoiceEnable;
                    }
                }
            }
            else if (voice.Playing)
            {
                _ = voice.Tick();
                if (voice.ShouldKeyOff)
                {
                    voice.KeyedOff = true;
                    this.sdsp.Registers.KON &= ~voice.VoiceEnable;
                    this.sdsp.Registers.KOFF |= voice.VoiceEnable;
                }
            }
        }

        #endregion
 */