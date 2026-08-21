using System.Collections.Generic;

#nullable enable

namespace ManaMagic.Core.Metadata
{
    /// <summary>
    /// Static class containing metadata for various data within the Secret of Mana ROM file.
    /// </summary>
    public static partial class ManaMetadata
    {
        /// <summary>
        /// Gets the metadata for music tracks.
        /// </summary>
        public static IReadOnlyList<MusicMetadata> MusicMetadata { get; } = new List<MusicMetadata>()
        {
                new MusicMetadata(0x00, "'Secret of the Arid Sands'", MusicMetadataFlags.None),
                new MusicMetadata(0x01, "'Flight into the Unknown'", MusicMetadataFlags.None),
                new MusicMetadata(0x02, "'The Dark Star'", MusicMetadataFlags.None),
                new MusicMetadata(0x03, "'Prophecy'", MusicMetadataFlags.None),
                new MusicMetadata(0x04, "'Danger'", MusicMetadataFlags.None),
                new MusicMetadata(0x05, "'Distant Thunder'", MusicMetadataFlags.None),
                new MusicMetadata(0x06, "'The Wind Never Ceases'", MusicMetadataFlags.None),
                new MusicMetadata(0x07, "'I Closed My Eyes'", MusicMetadataFlags.None),
                new MusicMetadata(0x08, "'Spirit of the Night'", MusicMetadataFlags.None),
                new MusicMetadata(0x09, "'The Little Sprite'", MusicMetadataFlags.None),
                new MusicMetadata(0x0A, "'What the Forest Taught Me'", MusicMetadataFlags.None),
                new MusicMetadata(0x0B, "'Eternal Recurrence'", MusicMetadataFlags.None),
                new MusicMetadata(0x0C, "'The Oracle'", MusicMetadataFlags.None),
                new MusicMetadata(0x0D, "'A Curious Tale'", MusicMetadataFlags.None),
                new MusicMetadata(0x0E, "'Into the Thick of It'", MusicMetadataFlags.None),
                new MusicMetadata(0x0F, "'Phantom and A Rose'", MusicMetadataFlags.None),
                new MusicMetadata(0x10, "'Did You See the Ocean?'", MusicMetadataFlags.None),
                new MusicMetadata(0x11, "'The Color Of The Summer Sky'", MusicMetadataFlags.None),

                // The Artist Formerly Known As 'Menu'.
                new MusicMetadata(0x12, "'The Door into the World'", MusicMetadataFlags.None),
                new MusicMetadata(0x13, "'The Legend'", MusicMetadataFlags.None),
                new MusicMetadata(0x14, "'The Calm Before the Storm'", MusicMetadataFlags.None),
                new MusicMetadata(0x15, "'A Bell Is Tolling'", MusicMetadataFlags.None),
                new MusicMetadata(0x16, "'Dancing Animals'", MusicMetadataFlags.None),

                // 'Way To Go!'.
                new MusicMetadata(0x17, "'Victory!!'", MusicMetadataFlags.None),

                // This is the track that plays while a boss is exploding.
                new MusicMetadata(0x18, "BossDefeated", MusicMetadataFlags.UnnamedTrack),

                // This track is the sound of the Cannon Travel cannon firing.
                new MusicMetadata(0x19, "CannonTravelLaunch", MusicMetadataFlags.UnnamedTrack),

                // This track is the song that plays during the Mode 7 overworld transition when using Cannon Travel.
                new MusicMetadata(0x1A, "CannonTravelFlight", MusicMetadataFlags.UnnamedTrack),
                new MusicMetadata(0x1B, "'Ceremony'", MusicMetadataFlags.None),
                new MusicMetadata(0x1C, "'Together Always'", MusicMetadataFlags.None),
                new MusicMetadata(0x1D, "'Whisper And Mantra'", MusicMetadataFlags.None),

                // This track plays during the castle rooftop events after defeating Mech Rider 2.
                new MusicMetadata(0x1E, "BurningCastle", MusicMetadataFlags.UnnamedTrack),
                new MusicMetadata(0x1F, "'It Happened Late One Evening'", MusicMetadataFlags.None),
                new MusicMetadata(0x20, "'A Curious Happening'", MusicMetadataFlags.None),

                // An unused Fanfare track. Loud and leads with trumpets.
                new MusicMetadata(0x21, "UnusedFanfare01", MusicMetadataFlags.UnnamedTrack | MusicMetadataFlags.DummiedOut),

                // The 'Item Get' Fanfare.
                new MusicMetadata(0x22, "'Eureka!'", MusicMetadataFlags.None),
                
                // An unused Fanfare track. Leads with snare rolls.
                new MusicMetadata(0x23, "UnusedFanfare02", MusicMetadataFlags.UnnamedTrack | MusicMetadataFlags.DummiedOut),
                new MusicMetadata(0x24, "'A Wish...'", MusicMetadataFlags.None),
                new MusicMetadata(0x25, "'Monarch On the Shore'", MusicMetadataFlags.None),
                new MusicMetadata(0x26, "'Steel and Snare'", MusicMetadataFlags.None),
                new MusicMetadata(0x27, "'Still of the Night'", MusicMetadataFlags.None),

                // This track is played while Flammie is picking up the PC after using the Flammie Drum.
                new MusicMetadata(0x28, "FlammieDescends", MusicMetadataFlags.UnnamedTrack),
                new MusicMetadata(0x29, "'Fond Memories'", MusicMetadataFlags.None),
                new MusicMetadata(0x2A, "'Mystic Invasion'", MusicMetadataFlags.None),
                new MusicMetadata(0x2B, "'In the Dead of the Night'", MusicMetadataFlags.None),
                new MusicMetadata(0x2C, "'Fear The Heavens'", MusicMetadataFlags.None),

                // This is the track that plays when the game first turns on, during the Squaresoft logo splash screen.
                new MusicMetadata(0x2D, "Intro", MusicMetadataFlags.UnnamedTrack),

                // 'Mara's Key Get' Fanfare.
                new MusicMetadata(0x2E, "'Ultimate!'", MusicMetadataFlags.None),

                // 'Weapon Get' Fanfare.
                new MusicMetadata(0x2F, "'Complete!'", MusicMetadataFlags.None),

                // 'Elemental Get' Fanfare.
                new MusicMetadata(0x30, "'Victory!'", MusicMetadataFlags.None),
                new MusicMetadata(0x31, "'Leave Time for Love'", MusicMetadataFlags.None),
                new MusicMetadata(0x32, "'The Second Truth from the Left'", MusicMetadataFlags.None),
                new MusicMetadata(0x33, "'The Curse'", MusicMetadataFlags.None),
                new MusicMetadata(0x34, "'I Won't Forget'", MusicMetadataFlags.None),

                // 'Ally Get' Fanfare
                new MusicMetadata(0x35, "'Buddy!'", MusicMetadataFlags.None),
                new MusicMetadata(0x36, "'Morning Is Here'", MusicMetadataFlags.None),
                new MusicMetadata(0x37, "'One of Them Is Hope'", MusicMetadataFlags.None),
                new MusicMetadata(0x38, "'A Conclusion'", MusicMetadataFlags.None),
                new MusicMetadata(0x39, "'Meridian Dance'", MusicMetadataFlags.None),
                new MusicMetadata(0x3A, "'Now Flightless Wings'", MusicMetadataFlags.None),

                // These are all duplicates of Track 38, pointing to the same data.
                new MusicMetadata(0x3B, "'A Conclusion'", MusicMetadataFlags.DummiedOut),
                new MusicMetadata(0x3C, "'A Conclusion'", MusicMetadataFlags.DummiedOut),
                new MusicMetadata(0x3D, "'A Conclusion'", MusicMetadataFlags.DummiedOut),
                new MusicMetadata(0x3E, "'A Conclusion'", MusicMetadataFlags.DummiedOut),
                new MusicMetadata(0x3F, "'A Conclusion'", MusicMetadataFlags.DummiedOut),
        };
    }
}
/*
 * 01 - Attack Failure
 * 07 - Heal Twinkle
 * 11 - Slap
 * 12 - Earthquake
 * 15 - Heavy Object Moved
 * 16 - Explosion (Kilroy Battery, Veedio)
 * 17 = Mana Seed Twinkle
 * 1D = Palace Door Opening
 * 21 = Heavy Switch Activated
 * 2B - Orb Cast Failure
 * 2C - Purchase Item
 * 2D - Spell Cast
 * 1E - Heart Beat (Intro Sequence)
 * 22 - WaterRushing
 * 33 - Weapon Slash
 * 41 - Whistle
 * 53 - Spring Beak Chirp
 * 5A - Heavy Object Collapse
 * 6A - Heavy Door Slam
 * 70 - Teleport
 * 71 - Long Teleport
 * 72 = Switch Activated
 * B0 = Cave-In
 * B2 - Multiple Braziers Lighting
 * B3 - Veedio Shutdown Noise
 * BB - Brazier Lighting
 * D6 - Flammie Chirp
 * D9 - Stove Clank
10CEFC: A90C            LDA #$0C        ;Load 0x0C into Accumulator. (Select Target Sound Effect)
10CF04: A90D            LDA #$0D        ;Load 0x0D into Accumulator. (Target Error Sound Effect)
10CF0C: A90E            LDA #$0E        ;Load 0x0E into Accumulator. (Select Equipment Sound Effect)
10CF3C: A928    	    LDA #$28        ;Load 0x28 into Accumulator. (Error Sound Effect)
10CF14: A929            LDA #$29        ;Load 0x29 into Accumulator. (Menu Spin Sound Effect)
10CF1C: A92A    	    LDA #$2A        ;Load 0x2A into Accumulator. (Move Menu Up Sound Effect)
10CF24: A92B    	    LDA #$2B        ;Load 0x2B into Accumulator. (Move Menu Down Sound Effect)
10CF2C:	A92C    	    LDA #$2C        ;Load 0x2C into Accumulator. (Buy/Sell Item Sound Effect)
10CF34: A94B    	    LDA #$4B        ;Load 0x4B into Accumulator. (Leap Sound Effect)
10CF44: A9A3    	    LDA #$A3        ;Load 0xA3 into Accumulator. (HP/MP Absorb Sound Effect)
 * 07 == Full Heal
        public static class SoundEffects
        {
            public const ushort None = 0x0000;
            public const ushort BossSkillRingAttackPlayer = 0x0019;
            public const ushort DeathMachineChainsawRev = 0x0043;
            public const ushort BossSkillGeneralBoss = 0x0083;
            public const ushort BossSkillCannonBoss = 0x0087;
            public const ushort BossSkillFreezeBeamPlayer = 0x0090;
            public const ushort BossSkillSonicPulseBoss = 0x009A;
            public const ushort BossSkillPetrifyBeamPlayer = 0x009B;
            public const ushort BossSkillAcidBreathPlayer = 0x00A9;
            public const ushort BossSkillWallDesperationBoss = 0x00B3;
            public const ushort BossSkillGeneralPlayer = 0x00BB;
            public const ushort BossSkillFreezeBreathPlayer = 0x00C0;
            public const ushort BossSkillBubblesAttackPlayer = 0x00C5;
            public const ushort BossSkillGasAttackPlayer = 0x00CB;
            public const ushort BossSkillCurrentPlayer = 0x00D5;
            public const ushort SnakeBossHiss = 0x00D7;
        }

Animation
AA: Cheer
AB: Static Cheer
 */