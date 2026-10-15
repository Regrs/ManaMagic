using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Debugger;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.Audio;
using ManaMagic.Core.Metadata;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Audio;
using ZwellTech.SuperNintendo.Audio.Akao;

namespace ManaMagic.Controls.UserControls.Debugging
{
    public partial class AudioDebuggerUserControl : UserControl
    {
        public const double Frequency = 36d;
        public const byte QuarterNoteDuration = 0x30;

        public AudioDebuggerUserControl()
        {
            InitializeComponent();
            ManaUtil.BindMusicTrackComboBox(this.trackSequenceComboBox, false);
            this.instrumentSampleNumericUpDown.Maximum = Constants.Bank03.InstrumentTableSize - 1;
        }

        private void LoadTrackDataButton_Click(object sender, EventArgs e)
        {
            //this.PrintDangerSequence();
            this.PrintTrackSequence(this.useSPCOffsetsCheckBox.Checked);
            this.PrintTrackInstrumentList();
            this.PrintTrackInstrumentUsage();
            //this.PrintSample(0x034782);
            //short[] sample = this.DecodeSample(0x034782); // [00: ]
            //short[] sample = this.DecodeSample(0x034E68); // [01: ]
            //short[] sample = this.DecodeSample(0x041606, 200, 0xE4); // [13: ]

            //this.PrintSample(0x041606);

            //this.CreateSineWaveWav(@"Data/SoundTest.wav", sample);
            //SoundPlayer player = new SoundPlayer(@"Data/SoundTest.wav");
            //player.Load();
            //player.Play();
        }

        private void PrintTrackSequence(bool asSpcOffset = false)
        {
            AkaoTrack track = ManaMagicContext.Current.Context.AudioContext.AkaoTracks[(int)this.trackSequenceComboBox.SelectedValue];

            this.trackStartAddressLabel.Text = (track.Address | 0xC00000).ToString("X6");
            this.trackSizeLabel.Text = track.Size.ToString("X4");
            this.seqStartOffsetLabel.Text = track.StartOffset.ToString("X4");
            this.seqEndOffsetLabel.Text = track.EndOffset.ToString("X4");
            this.channel01OffsetLabel.Text = track.Channel01.Offset.ToString("X4");
            this.channel02OffsetLabel.Text = track.Channel02.Offset.ToString("X4");
            this.channel03OffsetLabel.Text = track.Channel03.Offset.ToString("X4");
            this.channel04OffsetLabel.Text = track.Channel04.Offset.ToString("X4");
            this.channel05OffsetLabel.Text = track.Channel05.Offset.ToString("X4");
            this.channel06OffsetLabel.Text = track.Channel06.Offset.ToString("X4");
            this.channel07OffsetLabel.Text = track.Channel07.Offset.ToString("X4");
            this.channel08OffsetLabel.Text = track.Channel08.Offset.ToString("X4");

            int spcOffset = (int)(asSpcOffset ? 0x001A14 : (track.Address & 0xFF0000) + track.Channel01.Offset);
            if (!asSpcOffset) { spcOffset |= 0xC00000; }

            spcOffset = PrintChannelSequence(spcOffset, track.Channel01, this.trackSequenceC01RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel02, this.trackSequenceC02RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel03, this.trackSequenceC03RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel04, this.trackSequenceC04RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel05, this.trackSequenceC05RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel06, this.trackSequenceC06RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel07, this.trackSequenceC07RichTextBox);
            spcOffset = PrintChannelSequence(spcOffset, track.Channel08, this.trackSequenceC08RichTextBox);

            int PrintChannelSequence(int offset, AkaoChannel channel, RichTextBox richTextBox)
            {
                richTextBox.Text = string.Empty;
                int changeIndex = 0;
                ushort instrumentIndex = ManaMagicContext.Current.Context.AudioContext.TrackInstrumentUsageTable[(int)this.trackSequenceComboBox.SelectedValue][changeIndex].Value;
                if (channel.Enabled)
                {
                    StringBuilder sb = new StringBuilder(4096);
                    foreach (AkaoMessage message in channel.Sequence)
                    {
                        int bank = (offset >> 16) & 0xFF;
                        int printOffset = offset & 0xFFFF;
                        byte messageID = (byte)message.MessageType;
                        if (message is InstrumentChangeAkaoMessage instrumentMessage)
                        {
                            changeIndex = instrumentMessage.Index - 0x20;
                            sb.AppendLine($"{bank:X2}/{printOffset:X4}: [{message}]");
                        }
                        else if (message is NoteAkaoMessage noteMessage)
                        {
                            byte pitchIndex1 = (byte)(noteMessage.Note / 15);
                            byte pitchIndex2 = (byte)(noteMessage.Note % 12);
                            byte durationIndex = (byte)(noteMessage.Note % 15);
                            short notePitch = (short)(pitchIndex1 + (6 * 12) + 0 - 10);

                            sb.AppendLine($"{bank:X2}/{printOffset:X4}: [{message}, {ManaMetadata.NoteNames[pitchIndex1]}, {ManaMetadata.NoteNames[notePitch % 12]}, {ManaMetadata.DurationNames[durationIndex]}]");
                        }
                        else if (message is RestAkaoMessage restMessage)
                        {
                            byte durationIndex = (byte)(restMessage.Note % 15);
                            sb.AppendLine($"{bank:X2}/{printOffset:X4}: [{message}, {ManaMetadata.DurationNames[durationIndex]}]");
                        }
                        else if (message is TieAkaoMessage tieMessage)
                        {
                            byte durationIndex = (byte)(tieMessage.Note % 15);
                            sb.AppendLine($"{bank:X2}/{printOffset:X4}: [{message}, {ManaMetadata.DurationNames[durationIndex]}]");
                        }
                        else { sb.AppendLine($"{bank:X2}/{printOffset:X4}: [{message}]"); }

                        if (messageID < AkaoSequenceParser.FirstMessage) { offset += 1; }
                        else { offset += 1 + ManaMagicContext.Current.Context.AudioContext.AkaoMessageParameterCountTable[messageID - AkaoSequenceParser.FirstMessage]; }
                    }
                    richTextBox.Text = sb.ToString();
                }
                return offset;
            }
        }

        private void PrintTrackInstrumentList()
        {
            StringBuilder sb = new StringBuilder(512);
            foreach (UShortValue value in ManaMagicContext.Current.Context.AudioContext.TrackInstrumentUsageTable[(int)this.trackSequenceComboBox.SelectedValue])
            {
                sb.AppendLine($"{value.Value:X4}");
            }

            this.richTextBox2.Text = sb.ToString();
        }

        private void PrintTrackInstrumentUsage()
        {
            Dictionary<ushort, int> usage = new Dictionary<ushort, int>();
            for (int i = 0; i < 16; i++) { usage.Add((ushort)i, 0); }

            AkaoTrack track = ManaMagicContext.Current.Context.AudioContext.AkaoTracks[(int)this.trackSequenceComboBox.SelectedValue];
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel01.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel02.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel03.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel04.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel05.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel06.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel07.Sequence, usage);
            AudioDebuggerUserControl.CountInstrumentUsage(track.Channel08.Sequence, usage);

            this.trackInstrumentUsageDataGridView.Rows.Clear();
            foreach (KeyValuePair<ushort, int> kvp in usage)
            {
                UShortValue instrument = ManaMagicContext.Current.Context.AudioContext.TrackInstrumentUsageTable[(int)this.trackSequenceComboBox.SelectedValue][kvp.Key];
                this.trackInstrumentUsageDataGridView.Rows.Add(instrument.Value.ToString("X4"), kvp.Value);
            }
        }

        private static void CountInstrumentUsage(AkaoSequence sequence, Dictionary<ushort, int> usage)
        {
            foreach (AkaoMessage message in sequence)
            {
                if (message is InstrumentChangeAkaoMessage icMessage)
                {
                    usage[(ushort)(icMessage.Index - 0x20)]++;
                }
            }
        }

        public void PrintSample(int address)
        {
            GenericRomReader romReader = RomReaderFactory.GetRomReader<GenericRomReader>();
            romReader.Seek(address);
            ushort sizeInBytes = romReader.ReadUInt16();
            int size = sizeInBytes / 9;

            List<BitRateReductionBlock> sample = new List<BitRateReductionBlock>();
            List<int> offsets = new List<int>();
            for (int i = 0; i < size; i++)
            {
                offsets.Add(romReader.Position);
                byte control = romReader.Read();
                byte sample1 = romReader.Read();
                byte sample2 = romReader.Read();
                byte sample3 = romReader.Read();
                byte sample4 = romReader.Read();
                byte sample5 = romReader.Read();
                byte sample6 = romReader.Read();
                byte sample7 = romReader.Read();
                byte sample8 = romReader.Read();
                BitRateReductionBlock block = new BitRateReductionBlock(control, sample1, sample2, sample3, sample4, sample5, sample6, sample7, sample8);
                sample.Add(block);
            }

            this.richTextBox2.Text = string.Empty;
            int index = 0;
            foreach (BitRateReductionBlock block in sample)
            {
                int blockAddress = offsets[index] & 0xFFFF;
                int blockBank = ((offsets[index] & 0xFF0000) >> 16) | 0xC0;
                string offsetString = $"{blockBank:X2}\\{blockAddress:X4}: ";

                this.richTextBox2.Text += $"{offsetString}{block.Control:X2} {block.Sample01:X2} {block.Sample02:X2} {block.Sample03:X2} {block.Sample04:X2} {block.Sample05:X2} {block.Sample06:X2} {block.Sample07:X2} {block.Sample08:X2} [{index:X2}: Range: {block.Range}, Filter: {block.Filter}, Loop: {block.Loop}, End: {block.End}]";
                this.richTextBox2.Text += Environment.NewLine;
                index++;
            }
        }

        private void AudioDebuggerUserControl_Load(object sender, EventArgs e) { }

        private void LoadInstrumentSampleButton_Click(object sender, EventArgs e)
        {
            this.PrintTrackInstrumentBRR();
            this.PrintTrackInstrumentWaveform();
        }

        private void PrintTrackInstrumentBRR()
        {
            StringBuilder sb = new StringBuilder(512);
            BitRateReductionSample sample = ManaMagicContext.Current.Context.AudioContext.TrackInstrumentSamples[(int)this.instrumentSampleNumericUpDown.Value];
            uint pointer = ManaMagicContext.Current.Context.AudioContext.TrackInstrumentSamplePointerTable[(int)this.instrumentSampleNumericUpDown.Value] + 2;
            int index = 0;
            foreach (BitRateReductionBlock block in sample)
            {
                int blockAddress = (int)(pointer & 0xFFFF);
                int blockBank = (int)((pointer & 0xFF0000) >> 16) | 0xC0;
                string offsetString = $"{blockBank:X2}\\{blockAddress:X4}: ";

                sb.AppendLine($"{offsetString}{block.Control:X2} {block.Sample01:X2} {block.Sample02:X2} {block.Sample03:X2} {block.Sample04:X2} {block.Sample05:X2} {block.Sample06:X2} {block.Sample07:X2} {block.Sample08:X2} [{index:X2}: Range: {block.Range}, Filter: {block.Filter}, Loop: {block.Loop}, End: {block.End}]");
                pointer +=9;
                index++;
            }
            this.trackInstrumentBBRRichTextBox.Text = sb.ToString();
        }

        private void PrintTrackInstrumentWaveform()
        {
            BitRateReductionSample sample = ManaMagicContext.Current.Context.AudioContext.TrackInstrumentSamples[(int)this.instrumentSampleNumericUpDown.Value];
            StringBuilder sb = new StringBuilder();
            short[] waveform = sample.DecodeSample(0, 0);
            foreach (short wave in waveform)
            {
                double note = 12 * Math.Log2(wave / 440) + 69;
                sb.AppendLine($"{note}, {wave}, {wave:X4}");
            }
            this.trackInstrumentWaveformRichTextBox.Text = sb.ToString();
            //this.trackInstrumentWaveformRichTextBox.Text = string.Join(',', sample.DecodeSample(0, 0));
        }

        private void CountInstrumentsButton_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();
            for (int i = 0; i <= 33; i++)
            {
                counts.Add(i, 0);
            }
            foreach (DataTable<UShortValue> table in ManaMagicContext.Current.Context.AudioContext.TrackInstrumentUsageTable)
            {
                foreach (UShortValue value in table)
                {
                    counts[value.Value]++;
                }
            }

            this.trackInstrumentCountDataGridView.Rows.Clear();
            foreach (KeyValuePair<int, int> kvp in counts)
            {
                this.trackInstrumentCountDataGridView.Rows.Add($"{kvp.Key:X4}", kvp.Value);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AkaoDriver driver = new AkaoDriver(ManaMagicContext.Current.Context.AudioContext.NotePitchTable,
                                               ManaMagicContext.Current.Context.AudioContext.NoteDurationTable,
                                               ManaMagicContext.Current.Context.AudioContext.EchoSettingsTable,
                                               ManaMagicContext.Current.Context.AudioContext.EchoFilterCoefficientTable);

            driver.LoadTrack(ManaMagicContext.Current.Context.AudioContext.AkaoTracks[4], 4, 255, 0);
            driver.StepToEnd();
            driver.SaveAsWavFile(@"Data/SoundTest.wav");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Dictionary<byte, int> counts = new Dictionary<byte, int>();
            int relVolIncreaseCount = 0;
            for (byte i = 0; i < 0x10; i++) { counts.Add(i, 0); }
            int index = 0;
            foreach (ManaEvent manaEvent in ManaMagicContext.Current.Context.EventContext.Events)
            {
                foreach (EventOpCode opCode in manaEvent)
                {
                    if (opCode is PlayAudioEventOpCode audioOpCode)
                    {
                        if ((audioOpCode.Fade & 0xF0) > 0)
                        {
                            relVolIncreaseCount++;
                        }
                        else
                        {
                            counts[audioOpCode.Fade]++;
                            if (audioOpCode.Fade == 0x06) { ManaDebugger.DebugPrint($"Event {index:X2} uses Echo Settings 06."); }
                            if (audioOpCode.Fade == 0x09) { ManaDebugger.DebugPrint($"Event {index:X2} uses Echo Settings 09."); }
                            if (audioOpCode.Fade == 0x0D) { ManaDebugger.DebugPrint($"Event {index:X2} uses Echo Settings 0D."); }
                        }
                    }
                }
                index++;
            }

            ManaDebugger.DebugPrint($"Relative Volume Increase: {relVolIncreaseCount}");
            foreach (KeyValuePair<byte, int> kvp in counts)
            {
                ManaDebugger.DebugPrint($"{kvp.Key:X2}: {kvp.Value}");
            }
        }
    }

}