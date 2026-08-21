using System.Collections.Generic;
using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Menu
{
    public sealed class MenuContext
    {
        public DataTable<DataTable<GraphicTile4Bpp>> RingIconGraphicsTable { get; private set; } = DataTable<DataTable<GraphicTile4Bpp>>.Empty;
        public DataTable<SpritePalette> RingIconPaletteTable { get; private set; } = DataTable<SpritePalette>.Empty;
        public DataTable<ushort> RingIconDefinitionOffsetTable { get; private set; } = DataTable<ushort>.Empty;

        public DataTable<RingIconDefinition> RingIconItemDefinitionTable { get; private set; } = DataTable<RingIconDefinition>.Empty;
        public DataTable<RingIconDefinition> RingIconWeaponDefinitionTable { get; private set; } = DataTable<RingIconDefinition>.Empty;
        public DataTable<RingIconDefinition> RingIconHelmetDefinitionTable { get; private set; } = DataTable<RingIconDefinition>.Empty;
        public DataTable<RingIconDefinition> RingIconArmorDefinitionTable { get; private set; } = DataTable<RingIconDefinition>.Empty;
        public DataTable<RingIconDefinition> RingIconAccessoriesDefinitionTable { get; private set; } = DataTable<RingIconDefinition>.Empty;

        public DataTable<RingIcon> ItemRingIcons { get; private set; } = DataTable<RingIcon>.Empty;
        public DataTable<RingIcon> WeaponRingIcons { get; private set; } = DataTable<RingIcon>.Empty;
        public DataTable<RingIcon> HelmetRingIcons { get; private set; } = DataTable<RingIcon>.Empty;
        public DataTable<RingIcon> ArmorRingIcons { get; private set; } = DataTable<RingIcon>.Empty;
        public DataTable<RingIcon> AccessoryRingIcons { get; private set; } = DataTable<RingIcon>.Empty;

        /// <summary>
        /// Initializes the context by reading menu data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            MenuRomReader reader = RomReaderFactory.GetRomReader<MenuRomReader>();

            this.RingIconGraphicsTable = reader.ReadRingIconGraphicsTable();
            this.RingIconPaletteTable = reader.ReadRingIconPaletteTable();
            this.RingIconDefinitionOffsetTable = reader.ReadRingIconDefinitionOffsetTable();

            this.RingIconItemDefinitionTable = reader.ReadRingIconDefinitionTable(this.RingIconDefinitionOffsetTable[(int)RingIconOffset.Item], Constants.Bank18.RingIconItemIconCount);
            this.RingIconWeaponDefinitionTable = reader.ReadRingIconDefinitionTable(this.RingIconDefinitionOffsetTable[(int)RingIconOffset.Weapon], Constants.Bank18.RingIconWeaponIconCount);
            this.RingIconHelmetDefinitionTable = reader.ReadRingIconDefinitionTable(this.RingIconDefinitionOffsetTable[(int)RingIconOffset.Helmet], Constants.Bank18.RingIconHelmetIconCount);
            this.RingIconArmorDefinitionTable = reader.ReadRingIconDefinitionTable(this.RingIconDefinitionOffsetTable[(int)RingIconOffset.Armor], Constants.Bank18.RingIconArmorIconCount);
            this.RingIconAccessoriesDefinitionTable = reader.ReadRingIconDefinitionTable(this.RingIconDefinitionOffsetTable[(int)RingIconOffset.Accessory], Constants.Bank18.RingIconAccessoryIconCount);

            this.ItemRingIcons = this.CreateFullRingIconTable(this.RingIconItemDefinitionTable);
            this.WeaponRingIcons = this.CreateFullRingIconTable(this.RingIconWeaponDefinitionTable);
            this.HelmetRingIcons = this.CreateFullRingIconTable(this.RingIconHelmetDefinitionTable);
            this.ArmorRingIcons = this.CreateFullRingIconTable(this.RingIconArmorDefinitionTable);
            this.AccessoryRingIcons = this.CreateFullRingIconTable(this.RingIconAccessoriesDefinitionTable);
        }

        private DataTable<RingIcon> CreateFullRingIconTable(DataTable<RingIconDefinition> definitionTable)
        {
            byte index = 0;
            List<RingIcon> icons = new List<RingIcon>();
            foreach (RingIconDefinition definition in definitionTable)
            {
                DataTable<GraphicTile4Bpp> icon = this.RingIconGraphicsTable[definition.IconIndex];
                SpritePalette palette = this.RingIconPaletteTable[definition.PaletteIndex];
                icons.Add(new RingIcon(index, icon, palette, definition));

                index++;
            }
            return new DataTable<RingIcon>(icons);
        }

        private enum RingIconOffset
        {
            Item = 0,
            Elemental = 1,
            Options = 2,
            Weapon = 3,
            Helmet = 4,
            Armor = 5,
            Accessory = 6,
        }
    }
}
/*
[Table_RingIconOffsets]
; Offset from D8FD60
D8/FD60: 9001                             ;Base Offset for Item Ring Icons.
D8/FD62: 7200                             ;Base Offset for Elemental Ring Icons.
D8/FD64: A801                             ;Base Offset for Options Ring Icons.
D8/FD66: 8200                             ;Base Offset for Weapon Ring Icons.
D8/FD68: 1201                             ;Base Offset for Helmet Ring Icons.
D8/FD6A: 3C01                             ;Base Offset for Body Armor Ring Icons.
D8/FD6C: 6601                             ;Base Offset for Accessory Ring Icons.
D8/FD6E: FFFF                             ;Dummied Out.
 */