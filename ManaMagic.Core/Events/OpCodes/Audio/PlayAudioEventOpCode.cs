using System;
using System.ComponentModel;
using ManaMagic.Core.Metadata;

#nullable enable

namespace ManaMagic.Core.Events.OpCodes.Audio
{
    [EventOpCodeType(EventOpCodeType.PlayAudio)]
    [Description("Plays a music track or sound effect.")]
    public sealed record PlayAudioEventOpCode : AudioEventOpCode
    {
        public EventSoundType SoundType
        {
            get { return (EventSoundType)this.Parameters.Parameter1; }
            set { this.Parameters.Parameter1 = Convert.ToUInt16(value); }
        }
        public byte SoundIndex
        {
            get { return Convert.ToByte(this.Parameters.Parameter2); }
            set { this.Parameters.Parameter2 = value; }
        }
        public byte Fade
        {
            get { return Convert.ToByte(this.Parameters.Parameter3); }
            set { this.Parameters.Parameter3 = value; }
        }
        public byte Volume
        {
            get { return Convert.ToByte(this.Parameters.Parameter4); }
            set { this.Parameters.Parameter4 = value; }
        }

        internal PlayAudioEventOpCode(EventOpCodeType opCodeType, EventOpCodeCommandType commandType, ParameterInfo parameterInfo,
                                      ushort parameter1, ushort parameter2, ushort parameter3, ushort parameter4)
                                      : base(opCodeType, commandType, parameterInfo, parameter1, parameter2, parameter3, parameter4) { }

        /// <inheritdoc />
        public override string ToString()
        {
            string soundIndex = this.SoundType == EventSoundType.Music ? ManaMetadata.MusicMetadata[this.SoundIndex].TrackName : this.SoundIndex.ToString("X2");
            string fadeType = string.Empty;
            if (this.SoundType == EventSoundType.Fade)
            {
                fadeType = (this.SoundIndex & 0x0F) > 0 ? " In" : " Out";
            }
            return $"{this.CommandType} Command: Play {this.SoundType}: Index: {soundIndex}, Fade: {this.Fade:X2}, Volume: {this.Volume:X2}.";
        }
    }

    /// <summary>
    /// Represents the audio type played by an event.
    /// </summary>
    public enum EventSoundType : byte
    {
        /// <summary>
        /// The Unknown audio type.
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// The audio is a music track.
        /// </summary>
        Music = 0x01,
        /// <summary>
        /// The audio is a sound effect.
        /// </summary>
        SoundEffect = 0x02,
        /// <summary>
        /// The audio will fade in or out.
        /// </summary>
        Fade = 0x80,
        /// <summary>
        /// The currently playing music track will be stopped.
        /// </summary>
        StopMusic = 0xF1,
        /// <summary>
        /// The currently playing sound effect will be stopped.
        /// </summary>
        StopSoundEffect = 0xF2,
    }
}
/*
[Event 748: Music Fade Out]
0A946B: 40 80 80 00 00                  ;Play Music: (Id: 8080, Fade: 00, Volume: 00).
[Event 73E: Fast Music Fade Out]
0A9452: 40 80 20 00 00                  ;Play Music: (Id: 8020, Fade: 00, Volume: 00).

[Event 749: Music Fade In]
0A9472: 40 80 8F 00 00                  ;Play Music: (Id: 808F, Fade: 00, Volume: 00).
[Event 73F: Fast Music Fade In]
0A945C: 40 80 2F 00 00                  ;Play Music: (Id: 802F, Fade: 00, Volume: 00).

[Event 73D: Halt Music]
0A944C: 40 F1 00 00 00                  ;Play Music: (Id: F100, Fade: 00, Volume: 00).
[Event 788: Halt Sound Effect]
0A9541: 40 F2 00 00 00                  ;Play Music: (Id: F200, Fade: 00, Volume: 00).

[Event 704: Play Song: 'Danger']
0A92EF: 40 01 04 04 FF                  ;Play Music: (Id: 0104, Fade: 04, Volume: FF).

[Event 789: Play Sound Effect: 'Stove Clank']
0A9548: 40 02 D9 0F 88                  ;Play Music: (Id: 02D9, Fade: 0F, Volume: 88).
 */