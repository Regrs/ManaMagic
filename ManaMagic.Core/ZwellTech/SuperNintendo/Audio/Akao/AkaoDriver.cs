using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ManaMagic.Core.Debugger;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    /// <summary>
    /// Represents an emulator for the Akao SNES sound driver for Secret of Mana.
    /// </summary>
    /// <remarks>
    /// References:
    /// https://snes.nesdev.org/wiki/S-SMP
    /// https://github.com/everything8215/ff5/blob/997c87d1d8fdff2c142d91050fe400b6cb293905/src/sound/ff5-spc.asm
    /// https://github.com/mogue/FF6-SoundEngine/blob/master/spc_program.asm
    /// </remarks>
    public sealed partial class AkaoDriver
    {
        private const int ClockFrequency = 1024000;
        private const byte ClockTimer0CycleCount = 128;
        private const byte ClockTimer2CycleCount = 16;

        private const byte TrackInstrumentOffset = 0x20; // Secret Of Mana: 00-1F are SFX instruments.
        private const byte Timer0Target = 36;            // Secret Of Mana, Final Fantasy 5
        private const byte Timer1Target = 128;           // Secret Of Mana, Final Fantasy 5
        private const byte RecordedNoteOctave = 0x04;    // Secret Of Mana, Final Fantasy 5, Final Fantasy 6

        private const sbyte MainVolume = 0x7F; // Secret Of Mana, Final Fantasy 5
        private const byte TrackVolume = 0xFF; // Secret Of Mana
        private const byte VoiceGain = 0xA0;   // Secret Of Mana, Final Fantasy 5, Final Fantasy 6
        private const byte EDL_SOM = 0x02;
        private const byte EDL_FF = 0x05;      // Final Fantasy 5, Final Fantasy 6
        private const byte DIR_SOM = 0x19;
        private const byte DIR_FF = 0x1B;      // Final Fantasy 5, Final Fantasy 6

        internal const byte DefaultTempo = 0x01;
        internal const byte TempoTarget = 0xFF;

        private readonly DataTable<byte> noteDurations;
        private readonly DataTable<ushort> notePitches;
        private readonly DataTable<EchoSettings> echoSettings;
        private readonly DataTable<FiniteImpulseResponseFilter> echoFilterCoefficients;

        private readonly List<AkaoInstrument> instruments = new List<AkaoInstrument>(16);
        private readonly SDSP sdsp = new SDSP();
        private readonly SMPTimer timer0 = new SMPTimer();
        private readonly SMPTimer timer1 = new SMPTimer();
        private readonly SMPTimer timer2 = new SMPTimer();
        private readonly AkaoVoice voice0;
        private readonly AkaoVoice voice1;
        private readonly AkaoVoice voice2;
        private readonly AkaoVoice voice3;
        private readonly AkaoVoice voice4;
        private readonly AkaoVoice voice5;
        private readonly AkaoVoice voice6;
        private readonly AkaoVoice voice7;

        private ulong clockCounter = 0;
        private readonly SMPControlSettings control = SMPControlSettings.EnableBootRom;

        private VoiceEnable enabledVoices = VoiceEnable.Off;      // $AD
        private VoiceEnable enabledSFXVoices = VoiceEnable.Off;   // $AE

        private VoiceEnable keyOn = VoiceEnable.Off;              // $AF
        private VoiceEnable keyOff = VoiceEnable.All;             // $B0
        private VoiceEnable echoOn = VoiceEnable.Off;             // $B8
        private VoiceEnable pitchModOn = VoiceEnable.Off;         // $BA

        private VoiceEnable voicesWithTieNotes = VoiceEnable.Off; // $C2

        private VoiceEnable enableVolumeUpdate = VoiceEnable.Off; // $C9
        private VoiceEnable enablePitchUpdate = VoiceEnable.Off;  // $CA

        private readonly SMPCountdownTimer trackEnvelopeTimer = new SMPCountdownTimer(1, 7);
        private readonly AkaoTempoTimer tempoTimer = new AkaoTempoTimer();

        private byte absoluteVolume = 0;
        private byte trackVolume = AkaoDriver.TrackVolume;

        private byte instrumentIgnoreTrackVolume = 0xFF; // Secret Of Mana. FF5 does not have this ability. FF6 has it per voice.

        private readonly List<short> waveformL = new List<short>();
        private readonly List<short> waveformR = new List<short>();

        public AkaoDriver(DataTable<ushort> notePitches, DataTable<byte> noteDurations, DataTable<EchoSettings> echoSettings, DataTable<FiniteImpulseResponseFilter> echoFilterCoefficients)
        {
            this.notePitches = notePitches;
            this.noteDurations = noteDurations;
            this.echoSettings = echoSettings;
            this.echoFilterCoefficients = echoFilterCoefficients;

            this.voice0 = new AkaoVoice(VoiceEnable.Voice0, this.sdsp.Registers.Voice0);
            this.voice1 = new AkaoVoice(VoiceEnable.Voice1, this.sdsp.Registers.Voice1);
            this.voice2 = new AkaoVoice(VoiceEnable.Voice2, this.sdsp.Registers.Voice2);
            this.voice3 = new AkaoVoice(VoiceEnable.Voice3, this.sdsp.Registers.Voice3);
            this.voice4 = new AkaoVoice(VoiceEnable.Voice4, this.sdsp.Registers.Voice4);
            this.voice5 = new AkaoVoice(VoiceEnable.Voice5, this.sdsp.Registers.Voice5);
            this.voice6 = new AkaoVoice(VoiceEnable.Voice6, this.sdsp.Registers.Voice6);
            this.voice7 = new AkaoVoice(VoiceEnable.Voice7, this.sdsp.Registers.Voice7);
            this.ResetSDSP();

            this.timer0.Target = AkaoDriver.Timer0Target;
            this.timer1.Target = AkaoDriver.Timer1Target;

            // Timer 0 is the time keeper for the driver, ticking every 4.5ms (Akao v3) or 4.875ms (Akao v4).
            // Timer 1 is only used to initialize the echo buffer and fill it with silence.
            // Timer 2 is never even initialized in Akao v3, it is set to a target of 5 in Akao v4 however.
            this.control = SMPControlSettings.EnableTimer0;
        }

        public void LoadTrack(AkaoTrack track, int echoSettingsIndex, byte volume, byte volumeEnvelope)
        {
            this.ResetSDSP();

            this.LoadInstruments(track.Instruments);
            this.trackVolume = AkaoDriver.TrackVolume;
            this.SetAbsoluteVolume(volume, volumeEnvelope);

            EchoSettings echo = this.echoSettings[echoSettingsIndex];
            this.InitializeSDSP(echo);
            this.InitializeVoices(track);

            //00/0D5F: 8F017D          MOV $7D, #$01       ;Tempo Rate
            //00/0D62: 8FFF7E          MOV $7E, #$FF       ;Tempo Counter
            //00/0D65: 8FFFAC          MOV $AC, #$FF       ;Track Volume
            this.tempoTimer.Reset();
        }

        public void Step()
        {
            // Advance the cycle clock and tick the timers if they are enabled and have hit their interval.
            if (this.StepClock())
            {
                // The DSP is stepped every 32 clock cycles.
                this.sdsp.Step();
                this.waveformL.Add(this.sdsp.MainStereoOutL);
                this.waveformR.Add(this.sdsp.MainStereoOutR);
            }

            // Main Loop will process interrupts every step and processes audio at the rate of timer0 (Every 4,608 steps).
            this.StepMainLoop();
        }

        public void StepToEnd()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            do
            {
                this.Step();
            }
            while (!this.IsTrackComplete());

            watch.Stop();
            ManaDebugger.DebugPrint(watch.Elapsed.ToString());

            if (this.voice0.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice0 has one or more active loops."); }
            if (this.voice1.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice1 has one or more active loops."); }
            if (this.voice2.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice2 has one or more active loops."); }
            if (this.voice3.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice3 has one or more active loops."); }
            if (this.voice4.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice4 has one or more active loops."); }
            if (this.voice5.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice5 has one or more active loops."); }
            if (this.voice6.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice6 has one or more active loops."); }
            if (this.voice7.HasActiveLoops) { ThrowHelper.ThrowInvalidOperationException("Sequence ended while voice7 has one or more active loops."); }

            ManaDebugger.DebugPrint($"Voice0: {this.voice0.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice1: {this.voice1.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice2: {this.voice2.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice3: {this.voice3.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice4: {this.voice4.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice5: {this.voice5.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice6: {this.voice6.WaveIndexAtGoto}");
            ManaDebugger.DebugPrint($"Voice7: {this.voice7.WaveIndexAtGoto}");
        }

        public bool IsTrackComplete()
        {
            bool readComplete = !this.voice0.CanRead && !this.voice1.CanRead && !this.voice2.CanRead && !this.voice3.CanRead &&
                                !this.voice4.CanRead && !this.voice5.CanRead && !this.voice6.CanRead && !this.voice7.CanRead;

            bool playComplete = !this.voice0.Playing && !this.voice1.Playing && !this.voice2.Playing && !this.voice3.Playing &&
                                !this.voice4.Playing && !this.voice5.Playing && !this.voice6.Playing && !this.voice7.Playing;

            return readComplete && playComplete;
        }

        public void SaveAsWavFile(string filePath)
        {
            const int sampleRate = 32000;   // 32 kHz
            const short channels = 2;       // Stereo
            const short bitsPerSample = 16; // 16-bit Audio

            int dataLength = (this.waveformL.Count * channels * sizeof(ushort));
            int fileLength = 44 + dataLength;

            using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using BinaryWriter writer = new BinaryWriter(stream);

            // --- RIFF Header ---
            writer.Write(Encoding.ASCII.GetBytes("RIFF")); // ChunkID
            writer.Write(fileLength - 8);                  // ChunkSize (Total file size minus 8 bytes)
            writer.Write(Encoding.ASCII.GetBytes("WAVE")); // Format

            // --- Format Subchunk ("fmt ") ---
            writer.Write(Encoding.ASCII.GetBytes("fmt ")); // Subchunk1ID
            writer.Write(16);                              // Subchunk1Size (16 for PCM)
            writer.Write((short)1);                        // AudioFormat (1 for PCM)
            writer.Write(channels);                        // NumChannels
            writer.Write(sampleRate);                      // SampleRate
            writer.Write(sampleRate * 2);                  // ByteRate
            writer.Write((short)2);                        // BlockAlign
            writer.Write(bitsPerSample);                   // BitsPerSample

            // --- Data Subchunk ("data") ---
            writer.Write(Encoding.ASCII.GetBytes("data")); // Subchunk2ID
            writer.Write(dataLength);                      // Subchunk2Size
            for (int i = 0; i < this.waveformL.Count; i++)
            {
                writer.Write(this.waveformL[i]);
                writer.Write(this.waveformR[i]);
            }
        }

        private void StepMainLoop()
        {
            // Step 1: Process Interrupts From CPU.

            // Step 2: Wait for Timer 0 to tick. (4.5ms for Akao v3, 4.875ms for Akao v4).
            if (this.timer0.Out != 0)
            {
                // Step 3: Update the DSP Registers from the local values in this order: FLG > EON > NON > PMON > KOFF > KON.
                this.sdsp.Registers.FLG = 0;               // TODO
                this.sdsp.Registers.EON = this.echoOn;
                this.sdsp.Registers.NON = VoiceEnable.Off; // TODO
                this.sdsp.Registers.PMON = this.pitchModOn;
                this.sdsp.Registers.KOFF = this.keyOff;
                this.sdsp.Registers.KON = this.keyOn;

                // Step 4: Clear the local KON & KOFF values.
                this.keyOn = VoiceEnable.Off;
                this.keyOff = VoiceEnable.Off;

                // #5) CPU Output Update
                // #6) Track Envelope Update
                if (this.trackEnvelopeTimer.Tick())
                {
                    // TODO
                    this.trackEnvelopeTimer.Reset();
                }

                // Step 7 & 8, step the track voices.
                if (this.tempoTimer.Tick())
                {
                    this.StepVoice(this.voice0);
                    this.StepVoice(this.voice1);
                    this.StepVoice(this.voice2);
                    this.StepVoice(this.voice3);
                    this.StepVoice(this.voice4);
                    this.StepVoice(this.voice5);
                    this.StepVoice(this.voice6);
                    this.StepVoice(this.voice7);

                    this.UpdateDSPVoiceVolume(this.voice0);
                    this.UpdateDSPVoiceVolume(this.voice1);
                    this.UpdateDSPVoiceVolume(this.voice2);
                    this.UpdateDSPVoiceVolume(this.voice3);
                    this.UpdateDSPVoiceVolume(this.voice4);
                    this.UpdateDSPVoiceVolume(this.voice5);
                    this.UpdateDSPVoiceVolume(this.voice6);
                    this.UpdateDSPVoiceVolume(this.voice7);

                    this.UpdateDSPVoicePitch(this.voice0);
                    this.UpdateDSPVoicePitch(this.voice1);
                    this.UpdateDSPVoicePitch(this.voice2);
                    this.UpdateDSPVoicePitch(this.voice3);
                    this.UpdateDSPVoicePitch(this.voice4);
                    this.UpdateDSPVoicePitch(this.voice5);
                    this.UpdateDSPVoicePitch(this.voice6);
                    this.UpdateDSPVoicePitch(this.voice7);
                }

                // 9) Step All Sound Effect Voice Sequences
                // 10) Step Active Sound Effect Volume & Pitches
            }
        }

        private void StepVoice(AkaoVoice voice)
        {
            // Step 1: Test If Voice Is Enabled.
            // Step 2: Decrement Voice Duration.
            // Step 3: If Duration == 0 Then Process Sequence Until A New Note Is Played. 
            // Step 4: If Duration == 2 && NextNote != Tie Then Key The Note Off.
            // Step 5: Tick Vibrato Timer
            // Step 6: Tick Tremolo Timer (TODO)
            // Step 7: Update Volume Envelopes For Voice. (Volume > Pan > Pitch > Pan LFO)
            // Not Handled: Sound Effect Channel Processing.
            if (this.enabledVoices.HasFlag(voice.VoiceEnable))
            {
                voice.DurationTimer.Tick();
                while (voice.DurationTimer.Expired && voice.CanRead)
                {
                    this.ParseNextMessage(voice);
                }

                if (!this.voicesWithTieNotes.HasFlag(voice.VoiceEnable) && voice.DurationTimer.Counter == 2)
                {
                    this.keyOff |= voice.VoiceEnable;
                }

                voice.Vibrato.DelayTimer.Tick();
                this.StepVoiceVolumeEnvelopes(voice);
            }
        }

        private void StepVoiceVolumeEnvelopes(AkaoVoice voice)
        {
            // TODO: Volume Envelope
            // TODO: Pan Envelope
            // TODO: Pitch Envelope
            if (voice.PanLFO.Step())
            {
                // Pan changed, trigger a DSP volume update at the end of the step.
                this.enableVolumeUpdate |= voice.VoiceEnable;
            }
        }

        private bool StepClock()
        {
            this.clockCounter++;
            if (this.control.HasFlag(SMPControlSettings.EnableTimer0))
            {
                // Timer 0 ticks every 128 clock cycles.
                if ((this.clockCounter % AkaoDriver.ClockTimer0CycleCount) == 0)
                {
                    this.timer0.Tick();
                }
            }
            if (this.control.HasFlag(SMPControlSettings.EnableTimer1))
            {
                // Timer 1 also ticks every 128 clock cycles.
                if ((this.clockCounter % AkaoDriver.ClockTimer0CycleCount) == 0)
                {
                    this.timer1.Tick();
                }
            }
            if (this.control.HasFlag(SMPControlSettings.EnableTimer2))
            {
                // Timer 2 ticks every 16 clock cycles.
                if ((this.clockCounter % AkaoDriver.ClockTimer2CycleCount) == 0)
                {
                    this.timer2.Tick();
                }
            }

            // The S-DSP outputs a sample every 32 S-SMP clock cycles.
            return (this.clockCounter % 32) == 0;
        }

        private void SetAbsoluteVolume(byte inputVolume, byte volumeDuration)
        {
            // Interrupt Command 81
            this.absoluteVolume = (byte)(((inputVolume & 0xF0) >> 4) * 0x11);
            if (volumeDuration != 0) { ThrowHelper.ThrowNotImplementedException("Absolute Volume Duration is not implemented."); }
        }

        private void ParseNextMessage(AkaoVoice voice)
        {
            AkaoMessage message = voice.Channel!.Sequence[voice.SequenceIndex];
            switch (message.MessageType)
            {
                case AkaoMessageType.Note:
                case AkaoMessageType.Tie:
                case AkaoMessageType.Rest:
                    this.PlayNote(voice, (BaseNoteAkaoMessage)message);
                    break;
                case AkaoMessageType.InstrumentChange:
                    voice.InstrumentIndex = (byte)(((InstrumentChangeAkaoMessage)message).Index - AkaoDriver.TrackInstrumentOffset);
                    break;
                case AkaoMessageType.Tempo:
                    this.tempoTimer.Interval = ((TempoAkaoMessage)message).Tempo;
                    break;
                case AkaoMessageType.Octave:
                    voice.Octave = ((OctaveAkaoMessage)message).Octave;
                    break;
                case AkaoMessageType.OctaveUp:
                    voice.Octave++;
                    break;
                case AkaoMessageType.OctaveDown:
                    voice.Octave--;
                    break;
                case AkaoMessageType.Transpose:
                    voice.Transpose = ((TransposeAkaoMessage)message).Step;
                    break;
                case AkaoMessageType.Detune:
                    voice.Detune = ((DetuneAkaoMessage)message).Detune;
                    break;
                case AkaoMessageType.IgnoreTrackVolume:
                    this.instrumentIgnoreTrackVolume = (byte)(((IgnoreTrackVolumeAkaoMessage)message).InstrumentIndex - AkaoDriver.TrackInstrumentOffset);
                    break;
                case AkaoMessageType.Pan:
                    voice.Pan = ((PanAkaoMessage)message).Position;
                    break;
                case AkaoMessageType.PanLFOOn:
                    voice.PanLFO.Enable((PanLFOOnAkaoMessage)message);
                    break;
                case AkaoMessageType.PanLFOOff:
                    voice.PanLFO.Disable();
                    break;
                case AkaoMessageType.VibratoOn:
                    voice.Vibrato.Enable((VibratoOnAkaoMessage)message);
                    break;
                case AkaoMessageType.VibratoOff:
                    voice.Vibrato.Disable();
                    break;
                case AkaoMessageType.PitchModOn:
                    this.pitchModOn |= voice.VoiceEnable;
                    break;
                case AkaoMessageType.PitchModOff:
                    this.pitchModOn &= ~voice.VoiceEnable;
                    break;
                case AkaoMessageType.ChannelVolume:
                    voice.Volume = (sbyte)((ChannelVolumeAkaoMessage)message).Volume;
                    break;
                case AkaoMessageType.TrackVolume:
                    this.trackVolume = ((TrackVolumeAkaoMessage)message).Volume;
                    break;
                case AkaoMessageType.EchoOn:
                    this.echoOn |= voice.VoiceEnable;
                    break;
                case AkaoMessageType.EchoOff:
                    this.echoOn &= ~voice.VoiceEnable;
                    break;
                case AkaoMessageType.LoopStart:
                    voice.StartLoop(((LoopStartAkaoMessage)message).LoopCount);
                    break;
                case AkaoMessageType.LoopEnd:
                    voice.EndLoop();
                    break;
                case AkaoMessageType.Goto:
                    voice.WaveIndexAtGoto = this.waveformL.Count;
                    break;
                default:
                    ManaDebugger.DebugPrint($"{voice.VoiceEnable}: {message}");
                    break;
            }

            voice.SequenceIndex++;
        }

        private void PlayNote(AkaoVoice voice, BaseNoteAkaoMessage message)
        {
            voice.DurationTimer.Target = this.noteDurations[message.DurationIndex];
            voice.DurationTimer.Reset();

            // Look ahead to the next message to see if its a tie message, if so this note will not be keyed off at the end of its duration.
            CheckForTie(voice);

            // If the note is a rest then do nothing, the voice will remain keyed off and output silence for the duration.
            // If the note is a tie then just continue playing the previous pitch, adjusted by pitch slide, the voice is still keyed on from the previous note.
            if (message.IsNote)
            {
                byte calcNoteIndex = (byte)(message.NoteIndex + (voice.Octave * 12) + voice.Transpose - 10);

                byte octave = (byte)(calcNoteIndex / 12);
                byte noteIndex = (byte)(calcNoteIndex % 12);

                ushort notePitch = this.notePitches[noteIndex];
                ushort instrumentPitch = this.instruments[voice.InstrumentIndex].FrequencyMultiplier;

                byte instrumentPitchL = (byte)(instrumentPitch & 0xFF);
                byte instrumentPitchH = (byte)((instrumentPitch >> 8) & 0xFF);

                byte notePitchL = (byte)(notePitch & 0xFF);
                byte notePitchH = (byte)((notePitch >> 8) & 0xFF);

                // If I knew anything about audio mixing this equation might make sense instead of here be dragons.
                // Secret of Mana, Final Fantasy 5 and Final Fantasy 6 all use this equation. This somehow produces a 2.12 fixed point value.
                // Note pitches are probably also 2.12 fixed point values, but the instrument pitch might actually be two bytes instead of a ushort.
                instrumentPitchL += voice.Detune;
                ushort pitchPart1 = (ushort)((ushort)(notePitchH * instrumentPitchL) + (ushort)(((notePitchL * instrumentPitchL) >> 8) & 0x00FF));
                ushort pitchPart2 = (instrumentPitchH > 0) ? (ushort)(((instrumentPitchH * (byte)((pitchPart1 >> 8) & 0xFF)) >> 8) & 0x00FF) : (ushort)0;
                ushort pitch = (ushort)(pitchPart1 + pitchPart2);
                if ((sbyte)(instrumentPitchL) >= 0)
                {
                    pitch += notePitch;
                }

                if (octave > AkaoDriver.RecordedNoteOctave) { pitch <<= (octave - AkaoDriver.RecordedNoteOctave); }
                else if (octave < AkaoDriver.RecordedNoteOctave) { pitch >>= (AkaoDriver.RecordedNoteOctave - octave); }

                voice.Pitch = pitch;
                voice.Vibrato.Reset();

                // Queue this voice to have both its pitch and volume updated at the end of this step and to key on at the start of the next step.
                this.enableVolumeUpdate |= voice.VoiceEnable;
                this.enablePitchUpdate |= voice.VoiceEnable;
                this.keyOn |= voice.VoiceEnable;

                // Update the DSPs source number and ADSR for this voice.
                this.UpdateDSPVoiceSample(voice);
            }

            bool CheckForTie(AkaoVoice voice)
            {
                int index = voice.SequenceIndex + 1;
                if (!voice.EndOfSequence && index < voice.Channel.Sequence.Count)
                {
                    if (voice.Channel.Sequence[index].MessageType == AkaoMessageType.Tie)
                    {
                        this.voicesWithTieNotes |= voice.VoiceEnable;
                        return true;
                    }
                }

                // Next message is not a Tie.
                this.voicesWithTieNotes &= ~voice.VoiceEnable;
                return false;
            }
        }

        private void InitializeVoices(AkaoTrack track)
        {
            this.voice0.Initialize(track.Channel01);
            this.voice1.Initialize(track.Channel02);
            this.voice2.Initialize(track.Channel03);
            this.voice3.Initialize(track.Channel04);
            this.voice4.Initialize(track.Channel05);
            this.voice5.Initialize(track.Channel06);
            this.voice6.Initialize(track.Channel07);
            this.voice7.Initialize(track.Channel08);

            this.enabledVoices = VoiceEnable.Off;
            this.enabledSFXVoices = VoiceEnable.Off;
            if (this.voice0.CanRead) { this.enabledVoices |= this.voice0.VoiceEnable; }
            if (this.voice1.CanRead) { this.enabledVoices |= this.voice1.VoiceEnable; }
            if (this.voice2.CanRead) { this.enabledVoices |= this.voice2.VoiceEnable; }
            if (this.voice3.CanRead) { this.enabledVoices |= this.voice3.VoiceEnable; }
            if (this.voice4.CanRead) { this.enabledVoices |= this.voice4.VoiceEnable; }
            if (this.voice5.CanRead) { this.enabledVoices |= this.voice5.VoiceEnable; }
            if (this.voice6.CanRead) { this.enabledVoices |= this.voice6.VoiceEnable; }
            if (this.voice7.CanRead) { this.enabledVoices |= this.voice7.VoiceEnable; }
        }

        private void InitializeSDSP(EchoSettings echo)
        {
            List<BitRateReductionSample> samples = new List<BitRateReductionSample>(this.instruments.Count);
            foreach (AkaoInstrument instrument in this.instruments) { samples.Add(instrument.Sample); }
            this.sdsp.Registers.DIR = samples;

            ADSR adsr = this.instruments[0].ADSR;
            this.sdsp.Registers.Voice0.VxADSR = adsr;
            this.sdsp.Registers.Voice1.VxADSR = adsr;
            this.sdsp.Registers.Voice2.VxADSR = adsr;
            this.sdsp.Registers.Voice3.VxADSR = adsr;
            this.sdsp.Registers.Voice4.VxADSR = adsr;
            this.sdsp.Registers.Voice5.VxADSR = adsr;
            this.sdsp.Registers.Voice6.VxADSR = adsr;
            this.sdsp.Registers.Voice7.VxADSR = adsr;

            this.sdsp.Registers.EDL = echo.EchoDelay;
            this.sdsp.Registers.EVOLL = echo.EchoVolume;
            this.sdsp.Registers.EVOLR = echo.EchoVolume;
            this.sdsp.Registers.EFB = echo.EchoFeedback;

            FiniteImpulseResponseFilter filter = this.echoFilterCoefficients[echo.FilterCoefficientIndex];
            this.sdsp.Registers.FIR0 = filter.Tap0;
            this.sdsp.Registers.FIR1 = filter.Tap1;
            this.sdsp.Registers.FIR2 = filter.Tap2;
            this.sdsp.Registers.FIR3 = filter.Tap3;
            this.sdsp.Registers.FIR4 = filter.Tap4;
            this.sdsp.Registers.FIR5 = filter.Tap5;
            this.sdsp.Registers.FIR6 = filter.Tap6;
            this.sdsp.Registers.FIR7 = filter.Tap7;
        }

        private void ResetSDSP()
        {
            this.sdsp.Registers.DIR = DSPRegisters.NullDirectory;
            this.sdsp.Registers.EDL = AkaoDriver.EDL_SOM;

            // Gain for every voice is always set to A0 (Exp Decrease).
            // Track data never uses this, opting for ADSR instead.
            this.sdsp.Registers.Voice0.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice1.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice2.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice3.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice4.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice5.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice6.VxGAIN = AkaoDriver.VoiceGain;
            this.sdsp.Registers.Voice7.VxGAIN = AkaoDriver.VoiceGain;

            // The main volume is always set to maximum.
            this.sdsp.Registers.MVOLL = AkaoDriver.MainVolume;
            this.sdsp.Registers.MVOLR = AkaoDriver.MainVolume;

            this.sdsp.Registers.EVOLL = 0;
            this.sdsp.Registers.EVOLR = 0;
            this.sdsp.Registers.FLG = 0;
            this.sdsp.Registers.PMON = VoiceEnable.Off;
            this.sdsp.Registers.NON = VoiceEnable.Off;
            this.sdsp.Registers.EON = VoiceEnable.Off;

            // When the driver initializes it will load the first FIR filter into the DSP, which should be the identity filter.
            FiniteImpulseResponseFilter identity = this.echoFilterCoefficients[0];
            this.sdsp.Registers.FIR0 = identity.Tap0;
            this.sdsp.Registers.FIR1 = identity.Tap1;
            this.sdsp.Registers.FIR2 = identity.Tap2;
            this.sdsp.Registers.FIR3 = identity.Tap3;
            this.sdsp.Registers.FIR4 = identity.Tap4;
            this.sdsp.Registers.FIR5 = identity.Tap5;
            this.sdsp.Registers.FIR6 = identity.Tap6;
            this.sdsp.Registers.FIR7 = identity.Tap7;
        }

        private void UpdateDSPVoiceVolume(AkaoVoice voice)
        {
            if (voice.Vibrato.Step(voice.Pitch))
            {
                this.enablePitchUpdate |= voice.VoiceEnable;
            }

            if (this.enableVolumeUpdate.HasFlag(voice.VoiceEnable))
            {
                byte pan = (byte)(voice.Pan + voice.PanLFO.Offset);
                byte panL = (byte)(pan ^ 0xFF);
                byte panR = pan;
                byte volume = (byte)((this.absoluteVolume * voice.Volume) >> 8);
                if (this.instrumentIgnoreTrackVolume != voice.InstrumentIndex)
                {
                    volume = (byte)((volume * this.trackVolume) >> 8);
                }
                voice.Registers.VxVOLL = (sbyte)(((volume * panL) >> 8) >> 1);
                voice.Registers.VxVOLR = (sbyte)(((volume * panR) >> 8) >> 1);

                this.enableVolumeUpdate &= ~voice.VoiceEnable;
            }
        }

        private void UpdateDSPVoicePitch(AkaoVoice voice)
        {
            if (this.enablePitchUpdate.HasFlag(voice.VoiceEnable))
            {
                voice.Registers.VxPITCH = (ushort)(voice.Pitch + voice.Vibrato.Amount);
                this.enablePitchUpdate &= ~voice.VoiceEnable;
            }
        }

        private void UpdateDSPVoiceSample(AkaoVoice voice)
        {
            voice.Registers.VxSRCN = voice.InstrumentIndex;
            voice.Registers.VxADSR = this.instruments[voice.InstrumentIndex].ADSR;
        }

        private void LoadInstruments(IReadOnlyList<AkaoInstrument> instruments)
        {
            this.instruments.Clear();
            foreach (AkaoInstrument instrument in instruments)
            {
                this.instruments.Add(instrument);
            }
        }
    }

    [Flags]
    public enum AkaoDriverSettings : byte
    {
        None = 0x00,
        Mono = 0B_0000_0001,
        FastForward = 0B_0000_0010,
        WaveformChannelControl = 0B_0000_0100,
        SwapSoundEffectStereo = 0B_0000_1000,

        EnableVolumeEnvelopeOutput = 0B_0010_0000, // SoM only, writes the current volume counter to CPU01. If counter == 0 then the flag is unset.

        EnableWaveformOutput = 0B_1000_0000,

        // $B1 = Secret Of Mana
        // $BE = Final Fantasy 5
        // $86 - Final Fantasy 6
    }

    /* --- Akao RAM Map (Secret of Mana) ---
     * $003C - Vx_NOTE_DURATION
     * $003D - Vx_OCTAVE
     * $005D - Vx_INSTRUMENT
     * $007D - TEMPO_RATE
     * $007E - TEMPO_TIMER
     * $00AC - TRACK_VOLUME
     * $00AD - ENABLED_VOICE_CHANNELS
     * $00AE - ENABLED_SFX_CHANNELS
     * $00AF - AKAO_KEY_ON
     * $00B0 - AKAO_KEY_OFF
     * $00B1 - DRIVER_SYSTEM_FLAGS
     * $00B2 = ACTIVE_VOICE_MASK
     * $00B8 - AKAO_ECHO_ON
     * $00BA - AKAO_PMON
     * $00C2 - VOICES_QUEUED_TIE
     * $00C9 - VOICES_ENABLE_DSP_VOL_UPDATE
     * $00CA - VOICES_ENABLE_DSP_PITCH_UPDATE
     * 
     * $0100 - Vx_VIBRATO_DELAY_TIMER
     * $0121 - Vx_VIBRATO_AMPLITUDE
     * $0141 - Vx_PAN_LFO_ENABLED
     * $0160 - Vx_VIBRATO_DELAY
     * 
     * $152C - Note Duration Table
     * 
     * $FCA0 - Vx_VIBRATO_DURATION
     * $FCC0 - Vx_PAN_LFO_DURATION
     * $FCE0 - Vx_VIBRATO_DURATION_TIMER
     * $FD00 - Vx_PAN_LFO_DURATION_TIMER
     * $FD20 - Vx_INSTRUMENT_MULTIPLER
     * $FD41 - Vx_CALCULATED_NOTE
     * $FDA0 - Vx_PAN_LFO_AMPLITUDE
     * $FDC0 - Vx_VIBRATO_MULTIPLER
     * $FDE0 - Vx_PAN_LFO_FREQUENCY
     * $FE00 - Vx_VIBRATO_ENVELOPE
     * $FE40 - Vx_VIBRATO_AMOUNT
     * $FE60 - Vx_PITCH
     * $FE80 - Vx_ADSR
     */
    /* --- Flow Of Main Program Loop (V3) ---
     1) Process Interrupts From CPU
     2) Wait For Timer0 Tick
     3) Update DSP Registers: FLG > EON > NON > PMON > KOFF > KON
     4) Clear Local KON/KOFF
     5) If EnableWaveformOutput Then
           Output Current Wave Info To CPUIO0-CPUIO3.
        Else
           CPUIO2 = ActiveTrackChannels
           CPUIO3 = ActiveSoundEffectChannels
           If MinorVersion == SoM Then
              If EnableVolumeEnvelopeOutput Then
                 CPUIO1 = VolumeEnvelope
                 If VolumeEnvelope == 0 Then
                    DriverSettings &= ~EnableVolumeEnvelopeOutput
                 End If
               End If
            Else
               CPUIO1 = ActiveConditionalGotoChannels
            End If
         End If
     6) If PausedVoices == 0 Then
           TrackEnvelopeCounter--
           If TrackEnvelopeCounter == 0 Then
              TrackEnvelopeCounter = 7
              Step Track Envelopes
           End If
        End If
     7) Step All Track Voice Sequences
     8) Step Active Track Volume & Pitches (If Required)
     9) Step All Sound Effect Voice Sequences
    10) Step Active Sound Effect Volume & Pitches (If Required)
    11) Jump To #1
    */
}
