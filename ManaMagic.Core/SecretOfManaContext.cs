using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ManaMagic.Core.Analyzer;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Character;
using ManaMagic.Core.Events;
using ManaMagic.Core.Items;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Menu;
using ManaMagic.Core.Sprites;
using ManaMagic.Core.WorldMap;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core
{
    public sealed class SecretOfManaContext
    {
        private IReadOnlyList<byte>? LZ77DecompressionKey = null;
        private LZ77Decompressor? LZ77Decompressor = null;

        public RomFile? RomFile { get; private set; } = null;
        public bool Loaded { get; private set; } = false;

        public ManaAnalyzer Analyzer { get; }
        public ManaReferenceCounter ReferenceCounter { get; } = new ManaReferenceCounter();

        public BossContext BossContext { get; private set; } = new BossContext();
        public CharacterContext CharacterContext { get; } = new CharacterContext();
        public EventContext EventContext { get; } = new EventContext();
        public ItemContext ItemContext { get; } = new ItemContext();
        public MapContext MapContext { get; } = new MapContext();
        public WorldMapContext WorldMapContext { get; } = new WorldMapContext();
        public MenuContext MenuContext { get; } = new MenuContext();
        public SpriteContext SpriteContext { get; } = new SpriteContext();
        public TextContext TextContext { get; } = new TextContext();

        public SecretOfManaContext() { this.Analyzer = new ManaAnalyzer(this); }

        [MemberNotNull(nameof(RomFile))]
        public void Initialize(RomFile romFile)
        {
            this.RomFile = romFile;
        }

        [MemberNotNull(nameof(LZ77DecompressionKey), nameof(LZ77Decompressor))]
        public void Load()
        {
            this.CreateLZ77Decompressor();

            this.TextContext.Initialize();
            this.MenuContext.Initialize();
            this.CharacterContext.Initialize();
            this.EventContext.Initialize();
            this.MapContext.Initialize();
            this.WorldMapContext.Initialize();
            this.SpriteContext.Initialize(this.TextContext);
            this.ItemContext.Initialize(this.TextContext, this.MenuContext);
            this.BossContext.Initialize(this.TextContext, this.SpriteContext, this.LZ77Decompressor);

            this.ReferenceCounter.RunUsageCounters(this);

            this.Loaded = true;
        }

        [MemberNotNull(nameof(LZ77DecompressionKey), nameof(LZ77Decompressor))]
        private void CreateLZ77Decompressor()
        {
            GenericRomReader romReader = RomReaderFactory.GetRomReader<GenericRomReader>();
            romReader.Seek((int)Constants.Bank01.LZ77DecompressionKeyAddress);

            // Read the decompression key for the LZ77 algorithm.
            List<byte> key = new List<byte>((int)Constants.Bank01.LZ77DecompressionKeySize);
            for (int i = 0; i < Constants.Bank01.LZ77DecompressionKeySize; i++)
            {
                key.Add(romReader.Read());
            }

            this.LZ77DecompressionKey = key;
            this.LZ77Decompressor = new LZ77Decompressor(this.LZ77DecompressionKey);
        }
    }
}