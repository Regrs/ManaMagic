using ManaMagic.Core.Events;
using ManaMagic.Core.Sprites;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses
{
    public sealed record ManaBoss : ManaEnemy
    {
        public BossSetupHeader Header { get; }
        public bool HasHeader { get { return this.Header != BossSetupHeader.Empty; } }
        public SpritePalette Palette1 { get; private set; }
        public SpritePalette Palette2 { get; private set; }
        public SpritePalette Palette3 { get; private set; }
        public SpritePalette BackgroundPalette1 { get; private set; }
        public SpritePalette BackgroundPalette2 { get; private set; }
        public SpritePalette ManaBeastPalette1 { get; private set; }
        public SpritePalette ManaBeastPalette2 { get; private set; }

        public ManaBoss(byte index,
                        ManaEvent name,
                        EnemyStatEntry stats,
                        EnemyLootEntry loot,
                        BossSetupHeader header,
                        SpritePalette spritePalette,
                        SpritePalette bossPalette1,
                        SpritePalette bossPalette2,
                        SpritePalette bossPalette3,
                        SpritePalette backgroundPalette1,
                        SpritePalette backgroundPalette2,
                        SpritePalette manaBeastPalette1,
                        SpritePalette manaBeastPalette2,
                        bool userModified = false) : base(index, name, stats, loot, spritePalette, userModified)
        {
            this.Header = header;
            this.Palette1 = bossPalette1;
            this.Palette2 = bossPalette2;
            this.Palette3 = bossPalette3;
            this.BackgroundPalette1 = backgroundPalette1;
            this.BackgroundPalette2 = backgroundPalette2;
            this.ManaBeastPalette1 = manaBeastPalette1;
            this.ManaBeastPalette2 = manaBeastPalette2;
        }



        public void SetListeners(DataTable<ManaEvent> nameTable,
                                 DataTable<EnemyStatEntry> definitionTable,
                                 DataTable<EnemyLootEntry> lootTable,
                                 DataTable<SpritePalette> spritePaletteTable,
                                 DataTable<SpritePalette> bossPaletteTable)
        {
            base.SetListeners(nameTable, definitionTable, lootTable, spritePaletteTable);
            bossPaletteTable.RowReplaced += PaletteTable_RowReplaced;
        }


        private void PaletteTable_RowReplaced(object? sender, RowReplacedEventArgs<SpritePalette> e)
        {
            if (this.Palette1 != SpritePalette.Empty && e.Index == this.Palette1.Index)
            {
                this.Palette1 = e.NewItem;
            }
            else if (this.Palette2 != SpritePalette.Empty && e.Index == this.Palette2.Index)
            {
                this.Palette2 = e.NewItem;
            }
            else if (this.Palette3 != SpritePalette.Empty && e.Index == this.Palette3.Index)
            {
                this.Palette3 = e.NewItem;
            }
            else if (this.BackgroundPalette1 != SpritePalette.Empty && e.Index == this.BackgroundPalette1.Index)
            {
                this.BackgroundPalette1 = e.NewItem;
            }
            else if (this.BackgroundPalette2 != SpritePalette.Empty && e.Index == this.BackgroundPalette2.Index)
            {
                this.BackgroundPalette2 = e.NewItem;
            }
            else if (this.ManaBeastPalette1 != SpritePalette.Empty && e.Index == this.ManaBeastPalette1.Index)
            {
                this.ManaBeastPalette1 = e.NewItem;
            }
            else if (this.ManaBeastPalette2 != SpritePalette.Empty && e.Index == this.ManaBeastPalette2.Index)
            {
                this.ManaBeastPalette2 = e.NewItem;
            }
        }
    }
}