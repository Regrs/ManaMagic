using System.Collections.Generic;
using ManaMagic.Core.Bosses.Frames;
using ManaMagic.Core.Events;
using ZwellTech;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses
{
    public sealed record ManaBossSkill : NotifyRecordPropertyChanged
    {
        public ManaEvent Name { get; private set; }
        public Tileset Tileset { get; }
        public BossFrameList Frames { get; }
        public SpritePalette Palette { get; }
        public ManaBossWeapon DefaultWeapon { get; }

        public ushort AIPointer { get; init; }
        public ushort PaletteIndex { get; init; }
        public ushort BossAnimationIndex { get; init; }
        public ushort PlayerAnimationIndex { get; init; }
        public ushort BossSoundEffectIndex { get; init; }
        public ushort PlayerSoundEffectIndex { get; init; }

        public ManaBossSkill(byte index, ManaEvent name, Tileset tileset, BossFrameList frameList, SpritePalette palette, ManaBossWeapon defaultWeapon, bool userModified = false) : base(index, userModified)
        {
            this.Name = name;
            this.Tileset = tileset;
            this.Frames = frameList;
            this.Palette = palette;
            this.DefaultWeapon = defaultWeapon;
        }
    }

    public sealed class ManaBossSkill2
    {
        public static ManaBossSkill2 Empty { get; } = new ManaBossSkill2(string.Empty, Tileset.Empty, BossFrameList.Empty, SpritePalette.Empty, ManaBossWeapon.Empty);

        private readonly IReadOnlyList<SpritePalette> paletteTable;

        public string Name { get; }
        public Tileset Tileset { get; }
        public BossFrameList Frames { get; }
        public SpritePalette Palette { get; }
        public ManaBossWeapon DefaultWeapon { get; }

        public ushort AIPointer { get; init; }
        public ushort PaletteIndex { get; init; }
        public ushort BossAnimationIndex { get; init; }
        public ushort PlayerAnimationIndex { get; init; }
        public ushort BossSoundEffectIndex { get; init; }
        public ushort PlayerSoundEffectIndex { get; init; }

        public ManaBossSkill2(string name, Tileset tileset, BossFrameList frameList, SpritePalette palette, ManaBossWeapon defaultWeapon)
        {
            this.Name = name;
            this.Tileset = tileset;
            this.Frames = frameList;
            this.Palette = palette;
            this.DefaultWeapon = defaultWeapon;

            this.paletteTable = new List<SpritePalette>() { this.Palette, this.Palette, this.Palette };
        }

        public SuperNintendoGraphics DrawTileset()
        {
            return this.Tileset.DrawTileset(this.Palette);
        }

        public SuperNintendoGraphics DrawFrame(int index, FrameDrawingOptions options)
        {
            return this.Frames[index].DrawFrame(this.Tileset, this.paletteTable, options);
        }
    }
}