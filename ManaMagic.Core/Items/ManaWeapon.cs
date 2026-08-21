using System.ComponentModel;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ManaMagic.Core.Menu;
using ZwellTech;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Items
{
    public sealed record ManaWeapon : NotifyRecordPropertyChanged
    {
        public ManaEvent Name { get; private set; }
        public ManaEvent Description { get; private set; }
        public RingIcon Icon { get; }
        public ManaWeaponDefinition Definition { get; private set; }

        public ManaWeapon(byte index, ManaEvent name, ManaEvent description, RingIcon icon, ManaWeaponDefinition definition, bool userModified = false) : base(index, userModified)
        {
            this.Name = name;
            this.Description = description;
            this.Icon = icon;
            this.Definition = definition;

            this.Definition.PropertyChanged += this.Definition_PropertyChanged;
        }

        public void SetListeners(DataTable<ManaEvent> nameTable, DataTable<ManaEvent> descriptionTable, DataTable<ManaWeaponDefinition> definitionTable)
        {
            nameTable.RowReplaced += this.NameTable_RowReplaced;
            descriptionTable.RowReplaced += this.DescriptionTable_RowReplaced;
            definitionTable.RowReplaced += this.DefinitionTable_RowReplaced;
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

        public void SetDescription(string name)
        {
            byte[] textData = SecretOfManaEncoding.English.GetBytes(name);
            foreach (EventOpCode opCode in this.Description)
            {
                if (opCode is TextDataEventOpCode textOpCode)
                {
                    textOpCode.TextData = textData;
                    this.OnPropertyChanged(nameof(this.Description));
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

        private void DescriptionTable_RowReplaced(object? sender, RowReplacedEventArgs<ManaEvent> e)
        {
            if (e.Index == this.Index)
            {
                this.Description = e.NewItem;
            }
        }

        private void DefinitionTable_RowReplaced(object? sender, RowReplacedEventArgs<ManaWeaponDefinition> e)
        {
            if (e.Index == this.Index)
            {
                this.Definition = e.NewItem;
            }
        }

        private void Definition_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }
    }
}