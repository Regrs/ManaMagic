using System.ComponentModel;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ZwellTech;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Sprites
{
    public record ManaEnemy : NotifyRecordPropertyChanged
    {
        public ManaEvent Name { get; private set; }
        public EnemyStatEntry Statistics { get; private set; }
        public EnemyLootEntry Loot { get; private set; }
        public SpritePalette Palette { get; private set; }

        public ManaEnemy(byte index, ManaEvent name, EnemyStatEntry stats, EnemyLootEntry loot, SpritePalette palette, bool userModified = false) : base(index, userModified)
        {
            this.Name = name;
            this.Statistics = stats;
            this.Loot = loot;
            this.Palette = palette;

            this.Statistics.PropertyChanged += this.Statistics_PropertyChanged;
        }

        public void SetListeners(DataTable<ManaEvent> nameTable, DataTable<EnemyStatEntry> definitionTable, DataTable<EnemyLootEntry> lootTable, DataTable<SpritePalette> paletteTable)
        {
            nameTable.RowReplaced += this.NameTable_RowReplaced;
            definitionTable.RowReplaced += this.DefinitionTable_RowReplaced;
            lootTable.RowReplaced += this.LootTable_RowReplaced;
            paletteTable.RowReplaced += this.PaletteTable_RowReplaced;
        }

        public void SetName(string name)
        {
            byte[] textData = SecretOfManaEncoding.English.GetBytes(name);
            foreach (EventOpCode opCode in this.Name)
            {
                if (opCode is TextDataEventOpCode textOpCode)
                {
                    textOpCode.TextData = textData;
                    this.OnPropertyChanged(nameof(this.Name));
                }
            }
        }

        private void NameTable_RowReplaced(object? sender, RowReplacedEventArgs<ManaEvent> e)
        {
            if (e.Index == this.Index)
            {
                this.Name = e.NewItem;
            }
        }

        private void DefinitionTable_RowReplaced(object? sender, RowReplacedEventArgs<EnemyStatEntry> e)
        {
            if (e.Index == this.Index)
            {
                this.Statistics = e.NewItem;
            }
        }

        private void LootTable_RowReplaced(object? sender, RowReplacedEventArgs<EnemyLootEntry> e)
        {
            if (e.Index == this.Index)
            {
                this.Loot = e.NewItem;
            }
        }

        private void PaletteTable_RowReplaced(object? sender, RowReplacedEventArgs<SpritePalette> e)
        {
            if (e.Index == this.Index)
            {
                this.Palette = e.NewItem;
            }
        }

        private void Statistics_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }
    }
}