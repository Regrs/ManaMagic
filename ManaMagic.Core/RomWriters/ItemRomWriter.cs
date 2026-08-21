using System.Collections.Generic;
using ManaMagic.Core.Items;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomWriters
{
    public sealed class ItemRomWriter : RomWriter
    {
        private const uint WeaponUpgradePriceTableAddress = Constants.Bank18Offset | 0xFD4C;
        private const uint ShopTableAddress = Constants.Bank18Offset | 0xFC52;
        private const int WeaponUpgradePricePatchLocation = Constants.Bank18Offset | 0xFB82;

        /// <inheritdoc/>
        public ItemRomWriter(WritableRomFile rom) : base(rom) { }

        public void WriteWeaponDefinitionTable(ItemContext context)
        {
            this.Seek(Constants.Bank10.WeaponDefinitionTableAddress);
            foreach (ManaWeaponDefinition weaponDefinition in context.WeaponDefinitionTable)
            {
                /*
                [Table_PlayerEnemyWeapons]
                ;     -Legend-
                ;     TP:   Weapon Type
                ;     STAT: Stat Modifiers Provided By The Weapon
                ;     PJ:   Projectile Type
                ;     PL:   Palette
                ;     MA:   Monster Affinity
                ;     CC:   Critical Chance
                ;     AC:   Accuracy
                ;     PW:   Power
                ;     S1S2: Status Effect
                ;     SR:   Status Effect Rate

                        TP STAT PJ PL MA CC AC PW S1S2 SR
                101000: 00 00C0 00 02 00 00 4B 02 0000 50 [00: Spike Knuckle]
                */
                this.Write((byte)weaponDefinition.WeaponClass);
                this.WriteUInt16(weaponDefinition.StatModifiers);
                this.Write((byte)weaponDefinition.ProjectileType);
                this.Write(weaponDefinition.PaletteIndex);
                this.Write((byte)weaponDefinition.Affinity);
                this.Write(weaponDefinition.CriticalChance);
                this.Write(weaponDefinition.Accuracy);
                this.Write(weaponDefinition.Power);
                this.WriteUInt16((ushort)weaponDefinition.StatusEffects);
                this.Write(weaponDefinition.InflictionRate);
            }
        }

        public void WriteEquipmentDefinitionTable(ItemContext context)
        {
            this.Seek((int)Constants.Bank10.ArmorDefinitionTableAddress);
            foreach (ManaEquipmentDefinition equipmentDefinition in context.EquipmentDefinitionTable)
            {
                /*
                [Table_HeadArmor]
                ;     Legend
                ;     SB: Stat Boost
                ;         0x40: Always Set
                ;         0x10: Strength
                ;         0x08: Agility
                ;         0x04: Constitution
                ;         0x02: Intelligence
                ;         0x01: Wisdom
                ;     DF: Defense
                ;     EV: Evasion
                ;     MD: Magic Defense
                ;     MV: Magic Evasion
                ;     EQ: Equippable By
                ;         0xE0: Everyone
                ;         0xA0: Randi & Popoie
                ;         0x60: Purim & Popoie
                ;         0x80: Randi
                ;         0x40: Purim
                ;         0x20: Popoie
                ;     EL: Elemental Resistance/Weakness (Dummied Out, but functional if you alter the table)
                ;     R1: Status Resist 1
                ;     R2: Status Resist 2
                ;     ??: ???
                        SB DF EV MD MV EQ EL R1R2 ??
                        00 01 02 03 04 05 06 0708 09
                103ED0: 40 00 00 00 00 E0 00 0000 00 [00: Bare Head]
                 */
                this.Write((byte)equipmentDefinition.StatModifiers);
                this.Write(equipmentDefinition.Defense);
                this.Write(equipmentDefinition.Evasion);
                this.Write(equipmentDefinition.MagicDefense);
                this.Write(equipmentDefinition.MagicEvasion);
                this.Write((byte)equipmentDefinition.EquippableBy);
                this.Write((byte)equipmentDefinition.Element);
                this.WriteUInt16((ushort)equipmentDefinition.Resistances);
                this.Write(equipmentDefinition.Unknown);
            }
        }

        public void WriteItemDefinitionTable(ItemContext context)
        {
            this.Seek((int)Constants.Bank10.ItemDefinitionTableAddress);
            foreach (ManaItemDefinition itemDefinition in context.ItemDefinitionTable)
            {
                /*
                    Byte 00: Unused
                    Byte 01: Unused
                    Byte 02: Amount Healed
                    Byte 03: Unused
                    Byte 04: Unused
                    Byte 05: Unused
                    Byte 06: Unused
                    Byte 07: Read By C2/B49C - Stored into $E04F on the target (Seems to be palette ID)
                    Byte 0809: Read By C1/86C6 - Item Graphics Pointer?
                    Byte 0A0B: Read By C2/B49C - Player Graphics Pointer? - Stored in $E06A.
                    Byte 0C0D: Read By C2/B49C - Animation Pointer - Stored in $E0FC.
                    Byte 0E: Unused
                    Byte 0F: Unused

                    EZIndex:	00 01 02 03 04 05 06 07 0809 0A0B 0C0D 0E 0F
                    D0/4150:	00 00 64 64 00 00 00 82 110C A669 EF8C 00 00 [00: Candy] {Same A669, EF8C as Cure Water {Low}}
                 */
                this.Write(itemDefinition.Unused00);
                this.Write(itemDefinition.Unused01);
                this.Write(itemDefinition.AmountHealed);
                this.Write(itemDefinition.Unused03);
                this.Write(itemDefinition.Unused04);
                this.Write(itemDefinition.Unused05);
                this.Write(itemDefinition.Unused06);
                this.Write(itemDefinition.PaletteIndex);
                this.WriteUInt16(itemDefinition.ProbablyItemGraphicsPointer);
                this.WriteUInt16(itemDefinition.ProbablyPlayerGraphicsPointer);
                this.WriteUInt16(itemDefinition.AnimationPointer);
                this.Write(itemDefinition.Unused0E);
                this.Write(itemDefinition.Unused0F);
            }
        }

        public void WriteItemPriceTables(ItemContext context)
        {
            this.Seek((int)Constants.Bank18.ConsumableItemPriceTableAddress);
            foreach (UShortValue price in context.ConsumableItemPriceTable)
            {
                this.WriteUInt16(price.Value);
            }

            this.Seek((int)Constants.Bank18.HelmetPriceTableAddress);
            foreach (UShortValue price in context.HelmetPriceTable)
            {
                this.WriteUInt16(price.Value);
            }

            this.Seek((int)Constants.Bank18.ArmorPriceTableAddress);
            foreach (UShortValue price in context.ArmorPriceTable)
            {
                this.WriteUInt16(price.Value);
            }

            this.Seek((int)Constants.Bank18.AccessoryPriceTableAddress);
            foreach (UShortValue price in context.AccessoryPriceTable)
            {
                this.WriteUInt16(price.Value);
            }


            // In between the shop data and ring menu data are a few dozen free bytes.
            // Moving this table to the end of that range will allow the Shop tables to grow to their maximum length without relocation.
            this.Seek((int)ItemRomWriter.WeaponUpgradePriceTableAddress);
            foreach (UShortValue price in context.WeaponUpgradePriceTable)
            {
                this.WriteUInt16(price.Value);
            }

            this.Seek((int)ItemRomWriter.WeaponUpgradePricePatchLocation);
            this.WriteUInt16((ushort)(ItemRomWriter.WeaponUpgradePriceTableAddress & 0xFFFF));
        }

        public void WriteShopTable(ItemContext context)
        {
            List<ushort> pointerTable = new List<ushort>(context.ShopTable.RowCount);

            this.Seek((int)ItemRomWriter.ShopTableAddress);
            foreach (ManaShop shop in context.ShopTable)
            {
                pointerTable.Add((ushort)(this.Position & 0xFFFF));
                foreach (ShopItem item in shop)
                {
                    this.Write((byte)item);
                }
                this.Write(Constants.Bank18.ShopTableEndMarker);
            }

            this.Seek((int)Constants.Bank18.ShopPointerTableAddress);
            foreach (ushort pointer in pointerTable)
            {
                this.WriteUInt16(pointer);
            }
        }
    }
}