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

            List<ManaEnemy> enemyList = new List<ManaEnemy>(0x56);
            for (int i = 0; i < 0x57; i++)
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
        }
    }
}