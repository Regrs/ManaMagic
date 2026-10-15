using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public sealed class AkaoSequenceParser
    {
        private const byte NoteMax = 0xB3;
        private const byte TieMax = 0xC2;
        private const byte RestMax = 0xD1;
        public const byte FirstMessage = 0xD2;

        private readonly RomReader romReader;
        private readonly DataTable<byte> parameterCountTable;

        public AkaoSequenceParser(RomReader romReader, DataTable<byte> parameterCountTable)
        {
            this.romReader = romReader;
            this.parameterCountTable = parameterCountTable;
        }

        public AkaoTrack ParseAkaoTrack(uint address, IReadOnlyList<AkaoInstrument> instruments)
        {
            this.romReader.Seek((int)address);

            // Every track starts with a header.
            ushort size = this.romReader.ReadUInt16();            // The size of the entire track sequence in bytes, not counting the size value.
            ushort startOffset = this.romReader.ReadUInt16();     // Offset where the entire track sequence starts.
            ushort channel01Offset = this.romReader.ReadUInt16(); // Offset where the Channel 01 sequence starts.
            ushort channel02Offset = this.romReader.ReadUInt16(); // Offset where the Channel 02 sequence starts.
            ushort channel03Offset = this.romReader.ReadUInt16(); // Offset where the Channel 03 sequence starts.
            ushort channel04Offset = this.romReader.ReadUInt16(); // Offset where the Channel 04 sequence starts.
            ushort channel05Offset = this.romReader.ReadUInt16(); // Offset where the Channel 05 sequence starts.
            ushort channel06Offset = this.romReader.ReadUInt16(); // Offset where the Channel 06 sequence starts.
            ushort channel07Offset = this.romReader.ReadUInt16(); // Offset where the Channel 07 sequence starts.
            ushort channel08Offset = this.romReader.ReadUInt16(); // Offset where the Channel 08 sequence starts.
            ushort endOffset = this.romReader.ReadUInt16();       // Offset where the entire track sequence ends. (This address - 1)

            // Channels are enabled in order, unused channels will point to endOffset.
            // However due to the way its checked you can only disable channels in reverse order.
            bool channel01Enabled = channel01Offset != channel02Offset;
            bool channel02Enabled = channel02Offset != channel03Offset;
            bool channel03Enabled = channel03Offset != channel04Offset;
            bool channel04Enabled = channel04Offset != channel05Offset;
            bool channel05Enabled = channel05Offset != channel06Offset;
            bool channel06Enabled = channel06Offset != channel07Offset;
            bool channel07Enabled = channel07Offset != channel08Offset;
            bool channel08Enabled = channel08Offset != endOffset;

            // Sequence data can rollover the end of a bank, so that has to be accounted for.
            int channel01Size = channel02Offset > channel01Offset ? (channel02Offset - channel01Offset) : ((0xFFFF - channel01Offset) + channel02Offset);
            int channel02Size = channel03Offset > channel02Offset ? (channel03Offset - channel02Offset) : ((0xFFFF - channel02Offset) + channel03Offset);
            int channel03Size = channel04Offset > channel03Offset ? (channel04Offset - channel03Offset) : ((0xFFFF - channel03Offset) + channel04Offset);
            int channel04Size = channel05Offset > channel04Offset ? (channel05Offset - channel04Offset) : ((0xFFFF - channel04Offset) + channel05Offset);
            int channel05Size = channel06Offset > channel05Offset ? (channel06Offset - channel05Offset) : ((0xFFFF - channel05Offset) + channel06Offset);
            int channel06Size = channel07Offset > channel06Offset ? (channel07Offset - channel06Offset) : ((0xFFFF - channel06Offset) + channel07Offset);
            int channel07Size = channel08Offset > channel07Offset ? (channel08Offset - channel07Offset) : ((0xFFFF - channel07Offset) + channel08Offset);
            int channel08Size = endOffset > channel08Offset ? (endOffset - channel08Offset) : ((0xFFFF - channel08Offset) + endOffset);

            // Channel 01 will contain global information, such as the Tempo and Master Volume.
            // Disabled channels don't have a defined sequence.
            AkaoSequence channel01Sequence = channel01Enabled ? this.ParseSequence(channel01Size) : AkaoSequence.Empty;
            AkaoSequence channel02Sequence = channel02Enabled ? this.ParseSequence(channel02Size) : AkaoSequence.Empty;
            AkaoSequence channel03Sequence = channel03Enabled ? this.ParseSequence(channel03Size) : AkaoSequence.Empty;
            AkaoSequence channel04Sequence = channel04Enabled ? this.ParseSequence(channel04Size) : AkaoSequence.Empty;
            AkaoSequence channel05Sequence = channel05Enabled ? this.ParseSequence(channel05Size) : AkaoSequence.Empty;
            AkaoSequence channel06Sequence = channel06Enabled ? this.ParseSequence(channel06Size) : AkaoSequence.Empty;
            AkaoSequence channel07Sequence = channel07Enabled ? this.ParseSequence(channel07Size) : AkaoSequence.Empty;
            AkaoSequence channel08Sequence = channel08Enabled ? this.ParseSequence(channel08Size) : AkaoSequence.Empty;

            AkaoChannel channel01 = new AkaoChannel(channel01Enabled, channel01Offset, (ushort)(channel01Enabled ? channel01Size : 0), channel01Sequence);
            AkaoChannel channel02 = new AkaoChannel(channel02Enabled, channel02Offset, (ushort)(channel02Enabled ? channel02Size : 0), channel02Sequence);
            AkaoChannel channel03 = new AkaoChannel(channel03Enabled, channel03Offset, (ushort)(channel03Enabled ? channel03Size : 0), channel03Sequence);
            AkaoChannel channel04 = new AkaoChannel(channel04Enabled, channel04Offset, (ushort)(channel04Enabled ? channel04Size : 0), channel04Sequence);
            AkaoChannel channel05 = new AkaoChannel(channel05Enabled, channel05Offset, (ushort)(channel05Enabled ? channel05Size : 0), channel05Sequence);
            AkaoChannel channel06 = new AkaoChannel(channel06Enabled, channel06Offset, (ushort)(channel06Enabled ? channel06Size : 0), channel06Sequence);
            AkaoChannel channel07 = new AkaoChannel(channel07Enabled, channel07Offset, (ushort)(channel07Enabled ? channel07Size : 0), channel07Sequence);
            AkaoChannel channel08 = new AkaoChannel(channel08Enabled, channel08Offset, (ushort)(channel08Enabled ? channel08Size : 0), channel08Sequence);

            AkaoTrack track = new AkaoTrack(address, size, startOffset, endOffset, channel01, channel02, channel03, channel04, channel05, channel06, channel07, channel08, instruments);
            return track;
        }

        private AkaoSequence ParseSequence(int size)
        {
            List<AkaoMessage> messages = new List<AkaoMessage>();
            for (int i = 0; i < size; i++)
            {
                byte sequence = romReader.Read();
                if (sequence <= AkaoSequenceParser.NoteMax) { messages.Add(new NoteAkaoMessage(sequence)); }
                else if (sequence <= AkaoSequenceParser.TieMax) { messages.Add(new TieAkaoMessage(sequence)); }
                else if (sequence <= AkaoSequenceParser.RestMax) { messages.Add(new RestAkaoMessage(sequence)); }
                else
                {
                    byte parameter1 = 0;
                    byte parameter2 = 0;
                    byte parameter3 = 0;
                    sbyte sParameter2 = 0;
                    switch ((AkaoMessageType)sequence)
                    {
                        case AkaoMessageType.ChannelVolume:
                            parameter1 = romReader.Read();
                            messages.Add(new ChannelVolumeAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.ChannelVolumeFade:
                            parameter1 = romReader.Read();
                            parameter2 = romReader.Read();
                            messages.Add(new ChannelVolumeFadeAkaoMessage(parameter1, parameter2));
                            break;
                        case AkaoMessageType.Pan:
                            parameter1 = romReader.Read();
                            messages.Add(new PanAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.PanFade:
                            parameter1 = romReader.Read();
                            parameter2 = romReader.Read();
                            messages.Add(new PanFadeAkaoMessage(parameter1, parameter2));
                            break;
                        case AkaoMessageType.PitchSlide:
                            parameter1 = romReader.Read();
                            sParameter2 = romReader.ReadSByte();
                            messages.Add(new PitchSlideAkaoMessage(parameter1, sParameter2));
                            break;
                        case AkaoMessageType.VibratoOn:
                            parameter1 = romReader.Read();
                            parameter2 = romReader.Read();
                            parameter3 = romReader.Read();
                            messages.Add(new VibratoOnAkaoMessage(parameter1, parameter2, parameter3));
                            break;
                        case AkaoMessageType.VibratoOff:
                            messages.Add(new VibratoOffAkaoMessage());
                            break;
                        case AkaoMessageType.TremoloOn:
                            parameter1 = romReader.Read();
                            parameter2 = romReader.Read();
                            parameter3 = romReader.Read();
                            messages.Add(new TremoloOnAkaoMessage(parameter1, parameter2, parameter3));
                            break;
                        case AkaoMessageType.TremoloOff:
                            messages.Add(new TremoloOffAkaoMessage());
                            break;
                        case AkaoMessageType.PanLFOOn:
                            parameter1 = romReader.Read();
                            sParameter2 = romReader.ReadSByte();
                            messages.Add(new PanLFOOnAkaoMessage(parameter1, sParameter2));
                            break;
                        case AkaoMessageType.PanLFOOff:
                            messages.Add(new PanLFOOffAkaoMessage());
                            break;
                        case AkaoMessageType.NoiseFrequency:
                            parameter1 = romReader.Read();
                            messages.Add(new NoiseFrequencyAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.NoiseOn:
                            messages.Add(new NoiseOnAkaoMessage());
                            break;
                        case AkaoMessageType.NoiseOff:
                            messages.Add(new NoiseOffAkaoMessage());
                            break;
                        case AkaoMessageType.PitchModOn:
                            messages.Add(new PitchModOnAkaoMessage());
                            break;
                        case AkaoMessageType.PitchModOff:
                            messages.Add(new PitchModOffAkaoMessage());
                            break;
                        case AkaoMessageType.EchoOn:
                            messages.Add(new EchoOnAkaoMessage());
                            break;
                        case AkaoMessageType.EchoOff:
                            messages.Add(new EchoOffAkaoMessage());
                            break;
                        case AkaoMessageType.Octave:
                            parameter1 = romReader.Read();
                            messages.Add(new OctaveAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.OctaveUp:
                            messages.Add(new OctaveUpAkaoMessage());
                            break;
                        case AkaoMessageType.OctaveDown:
                            messages.Add(new OctaveDownAkaoMessage());
                            break;
                        case AkaoMessageType.Transpose:
                            parameter1 = romReader.Read();
                            messages.Add(new TransposeAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.TransposeAdjust:
                            parameter1 = romReader.Read();
                            messages.Add(new TransposeAdjustAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.Detune:
                            parameter1 = romReader.Read();
                            messages.Add(new DetuneAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.InstrumentChange:
                            parameter1 = romReader.Read();
                            messages.Add(new InstrumentChangeAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.EnvelopeAttack:
                            parameter1 = romReader.Read();
                            messages.Add(new EnvelopeAttackAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.EnvelopeDecay:
                            parameter1 = romReader.Read();
                            messages.Add(new EnvelopeDecayAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.EnvelopeSustain:
                            parameter1 = romReader.Read();
                            messages.Add(new EnvelopeSustainAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.EnvelopeRelease:
                            parameter1 = romReader.Read();
                            messages.Add(new EnvelopeReleaseAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.EnvelopeDefault:
                            messages.Add(new EnvelopeDefaultAkaoMessage());
                            break;
                        case AkaoMessageType.LoopStart:
                            parameter1 = this.romReader.Read();
                            if (parameter1 > 0) { parameter1++; }
                            messages.Add(new LoopStartAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.LoopEnd:
                            messages.Add(new LoopEndAkaoMessage());
                            break;
                        case AkaoMessageType.EndSequence:
                            messages.Add(new EndSequenceAkaoMessage());
                            break;
                        case AkaoMessageType.Tempo:
                            parameter1 = romReader.Read();
                            messages.Add(new TempoAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.TempoFade:
                            parameter1 = romReader.Read();
                            parameter2 = romReader.Read();
                            messages.Add(new TempoFadeAkaoMessage(parameter1, parameter2));
                            break;
                        case AkaoMessageType.TrackVolume:
                            parameter1 = romReader.Read();
                            messages.Add(new TrackVolumeAkaoMessage(parameter1));
                            break;
                        case AkaoMessageType.VoltaRepeat:
                            parameter1 = romReader.Read();
                            ushort voltaOffset = romReader.ReadUInt16();
                            messages.Add(new VoltaRepeatAkaoMessage(parameter1, voltaOffset));
                            break;
                        case AkaoMessageType.Goto:
                            ushort gotoOffset = romReader.ReadUInt16();
                            messages.Add(new GotoAkaoMessage(gotoOffset));
                            break;
                        case AkaoMessageType.LoopRestart:
                            messages.Add(new LoopRestartAkaoMessage());
                            break;
                        case AkaoMessageType.IgnoreTrackVolume:
                            parameter1 = romReader.Read();
                            messages.Add(new IgnoreTrackVolumeAkaoMessage(parameter1));
                            break;
                        default:
                            ThrowHelper.ThrowInvalidOperationException($"Unknown Akao Message: {sequence:X2}");
                            break;
                    }

                    i += this.parameterCountTable[sequence - AkaoSequenceParser.FirstMessage];
                }
            }

            return new AkaoSequence(messages);
        }
    }
}