using System;
using System.Collections.Generic;
using ManaMagic.Core.Events;
using ManaMagic.Core.Items;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public sealed class SpriteContext
    {
        public DataTable<SpriteGraphicsData> GraphicsDataTable { get; private set; } = DataTable<SpriteGraphicsData>.Empty;
        public DataTable<SpritePalette> PaletteTable { get; private set; } = DataTable<SpritePalette>.Empty;
        public DataTable<EnemyStatEntry> EnemyStatisticsTable { get; private set; } = DataTable<EnemyStatEntry>.Empty;
        public DataTable<EnemyLootEntry> LootTable { get; private set; } = DataTable<EnemyLootEntry>.Empty;

        public DataTable<ManaEnemy> Enemies { get; private set; } = DataTable<ManaEnemy>.Empty;
        public DataTable<SpriteTileset> SpriteTilesets { get; private set; } = DataTable<SpriteTileset>.Empty;


        /// <summary>
        /// Initializes the context by reading sprite data from the ROM file.
        /// </summary>
        public void Initialize(TextContext textContext)
        {
            SpriteRomReader reader = RomReaderFactory.GetRomReader<SpriteRomReader>();

            this.GraphicsDataTable = reader.ReadSpriteGraphicsDataTable();
            this.PaletteTable = reader.ReadSpritePaletteTable();
            this.EnemyStatisticsTable = reader.ReadEnemyStatisticsTable();
            this.LootTable = reader.ReadEnemyLootTable();

            List<ManaEnemy> enemyList = new List<ManaEnemy>(Constants.FirstBossIndex);
            for (int i = 0; i < Constants.FirstBossIndex; i++)
            {
                ManaEvent name = textContext.EnemyNameTable[i];
                EnemyStatEntry statistics = this.EnemyStatisticsTable[i];
                EnemyLootEntry loot = this.LootTable[i];
                SpritePalette palette = this.PaletteTable[i];

                ManaEnemy enemy = new ManaEnemy((byte)i, name, statistics, loot, palette);
                enemy.SetListeners(textContext.EnemyNameTable, this.EnemyStatisticsTable, this.LootTable, this.PaletteTable);
                enemyList.Add(enemy);
            }
            this.Enemies = new DataTable<ManaEnemy>(enemyList);

            List<SpriteTileset> spriteTilesets = new List<SpriteTileset>((int)Constants.Bank10.SpriteGraphicsTableSize);
            for (int i = 0; i < Constants.Bank10.SpriteGraphicsTableSize; i++)
            {
                SpriteGraphicsData graphicsData = this.GraphicsDataTable[i];
                SpritePalette palette = this.PaletteTable[i];

                SpriteTileset tileset = new SpriteTileset(i, TileType.FourBitsPerPixel, graphicsData, palette);
                spriteTilesets.Add(tileset);
            }
            this.SpriteTilesets = new DataTable<SpriteTileset>(spriteTilesets);
        }
    }
}