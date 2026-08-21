using System.Collections.Generic;
using System.Text;
using ManaMagic.Core.Metadata;
using ManaMagic.Disassembler;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    public sealed class MapDisassemblyRomReader : RomReader
    {
        public readonly static IReadOnlyDictionary<byte, string> SpriteNameStrings = new Dictionary<byte, string>()
        {
            { 0x00, "Rabite" },
            { 0x01, "Buzz Bee" },
            { 0x02, "Mushboom" },
            { 0x03, "Chobin Hood" },
            { 0x04, "Lullabud" },
            { 0x05, "Iffish" },
            { 0x06, "Kid Goblin" },
            { 0x07, "Eye Spy" },
            { 0x08, "Green Drop" },
            { 0x09, "Specter" },
            { 0x0A, "Blat" },
            { 0x0B, "Goblin" },
            { 0x0C, "Water Thug" },
            { 0x0D, "Polter Chair" },
            { 0x0E, "Ma Goblin" },
            { 0x0F, "Dark Funk" },
            { 0x10, "Crawler" },
            { 0x11, "Ice Thug" },
            { 0x12, "Zombie" },
            { 0x13, "Kimono Bird" },
            { 0x14, "Silktail" },
            { 0x15, "Nemesis Owl" },
            { 0x16, "Pebbler" },
            { 0x17, "Pumpkin Bomb" },
            { 0x18, "Steamed Crab" },
            { 0x19, "Chess Knight" },
            { 0x1A, "Wizard Eye" },
            { 0x1B, "Howler" },
            { 0x1C, "Robin Foot" },
            { 0x1D, "LA Funk" },
            { 0x1E, "Grave Bat" },
            { 0x1F, "Werewolf" },
            { 0x20, "Shadow X3" },
            { 0x21, "Evil Sword" },
            { 0x22, "Tomato Man" },
            { 0x23, "Mystic Book" },
            { 0x24, "Sand Stinger" },
            { 0x25, "Mad Mallard" },
            { 0x26, "Emberman" },
            { 0x27, "Red Drop" },
            { 0x28, "Eggatrice" },
            { 0x29, "Bomb Bee" },
            { 0x2A, "Mushgloom" },
            { 0x2B, "Trap Flower" },
            { 0x2C, "Dinofish" },
            { 0x2D, "Mimic Box" },
            { 0x2E, "Shadow X1" },
            { 0x2F, "Kimono Wizard" },
            { 0x30, "Ghost" },
            { 0x31, "Metal Crawler" },
            { 0x32, "Spider Legs" },
            { 0x33, "Weepy Eye" },
            { 0x34, "Shellblast" },
            { 0x35, "Beast Zombie" },
            { 0x36, "Ghoul" },
            { 0x37, "Imp" },
            { 0x38, "Blue Drop" },
            { 0x39, "Marmablue" },
            { 0x3A, "Fierce Head" },
            { 0x3B, "Griffin Hand" },
            { 0x3C, "Needlion" },
            { 0x3D, "Metal Crab" },
            { 0x3E, "Armored Man" },
            { 0x3F, "Shadow X2" },
            { 0x40, "Eggplant Man" },
            { 0x41, "Captain Duck" },
            { 0x42, "Nitro Pumpkin" },
            { 0x43, "Turtlance" },
            { 0x44, "Tsunami" },
            { 0x45, "Basilisk" },
            { 0x46, "Gremlin" },
            { 0x47, "Steelpion" },
            { 0x48, "Dark Ninja" },
            { 0x49, "Whimper" },
            { 0x4A, "Heck Hound" },
            { 0x4B, "Fiend Head" },
            { 0x4C, "National Scar" },
            { 0x4D, "Dark Stalker" },
            { 0x4E, "Dark Knight" },
            { 0x4F, "Shape Shifter" },
            { 0x50, "Wolf Lord" },
            { 0x51, "Doom Sword" },
            { 0x52, "Terminator" },
            { 0x53, "Master Ninja" },
            { 0x54, "?" },
            { 0x55, "Crystal Orb" },
            { 0x56, "Treasure Chest Control" },
            { 0x57, "Mantis Ant" },
            { 0x58, "Wall Face" },
            { 0x59, "Tropicallo" },
            { 0x5A, "Minotaur" },
            { 0x5B, "Spikey Tiger" },
            { 0x5C, "Jabberwocky" },
            { 0x5D, "Spring Beak" },
            { 0x5E, "Frost Gigas" },
            { 0x5F, "Snap Dragon" },
            { 0x60, "Mech Rider I" },
            { 0x61, "Doom's Wall" },
            { 0x62, "Vampire" },
            { 0x63, "Metal Mantis" },
            { 0x64, "Mech Rider II" },
            { 0x65, "Kilroy" },
            { 0x66, "Gorgon Bull" },
            { 0x67, "Brambler" },
            { 0x68, "Boreal Face" },
            { 0x69, "Great Viper" },
            { 0x6A, "Lime Slime" },
            { 0x6B, "Blue Spike" },
            { 0x6C, "Chamber's Eye" },
            { 0x6D, "Hydra" },
            { 0x6E, "Aegagropilon" },
            { 0x6F, "Hexas" },
            { 0x70, "Kettle Kin" },
            { 0x71, "Tonpole" },
            { 0x72, "Mech Rider III" },
            { 0x73, "Snow Dragon" },
            { 0x74, "Fire Gigas" },
            { 0x75, "Red Dragon" },
            { 0x76, "Axe Beak" },
            { 0x77, "Blue Dragon" },
            { 0x78, "Buffy" },
            { 0x79, "Dark Lich" },
            { 0x7A, "Biting Lizard" },
            { 0x7B, "Dragon Worm" },
            { 0x7C, "Dread Slime" },
            { 0x7D, "Thunder Gigas" },
            { 0x7E, "Doom's Eye" },
            { 0x7F, "Mana Beast" },
            { 0x80, "Randi" },
            { 0x81, "Purim" },
            { 0x82, "Popoie" },
            { 0x83, "Monster Snowman" },
            { 0x84, "Red Spell Palette Placeholder" },
            { 0x85, "Blue Spell Palette Placeholder" },
            { 0x86, "Green Spell Palette Placeholder" },
            { 0x87, "Yellow Spell Palette Placeholder" },
            { 0x88, "Midget Moogle/Purple Spell Palette Placeholder" },
            { 0x89, "Salamando's Stove" },
            { 0x8A, "Randi-Cutscene Version" },
            { 0x8B, "Purim-Cutscene Version" },
            { 0x8C, "Popoie-Cutscene Version" },
            { 0x8D, "Weapon Recharge Palette Placeholder" },
            { 0x8E, "White Spell Palette Placeholder" },
            { 0x8F, "Black Spell Palette Placeholder" },
            { 0x90, "Gnome" },
            { 0x91, "Undine" },
            { 0x92, "Salamando" },
            { 0x93, "Sylphid" },
            { 0x94, "Lumina" },
            { 0x95, "Shade" },
            { 0x96, "Luna" },
            { 0x97, "Dryad" },
            { 0x98, "Moogle" },
            { 0x99, "Neko" },
            { 0x9A, "Dwarf" },
            { 0x9B, "Dwarven Chief" },
            { 0x9C, "Watts" },
            { 0x9D, "Sprite Grandpa" },
            { 0x9E, "King Truffle" },
            { 0x9F, "Walrus Villager" },
            { 0xA0, "Jema" },
            { 0xA1, "Luka" },
            { 0xA2, "Fake Sage Joch" },
            { 0xA3, "Sage Joch" },
            { 0xA4, "Scorpion Boss" },
            { 0xA5, "Scorpion Henchman" },
            { 0xA6, "Phanna" },
            { 0xA7, "Krissie" },
            { 0xA8, "Sergo" },
            { 0xA9, "Cultist" },
            { 0xAA, "Temple Monk" },
            { 0xAB, "Pecard" },
            { 0xAC, "Dyluck" },
            { 0xAD, "Rudolph" },
            { 0xAE, "Santa" },
            { 0xAF, "Mushroom Villager" },
            { 0xB0, "Emperor Vandole" },
            { 0xB1, "Geshtar" },
            { 0xB2, "Fanha" },
            { 0xB3, "Lost Lacky" },
            { 0xB4, "Sheex" },
            { 0xB5, "Flammie Head" },
            { 0xB6, "Imperial Soldier" },
            { 0xB7, "Republic Soldier" },
            { 0xB8, "General Meria" },
            { 0xB9, "Admiral Morie" },
            { 0xBA, "Elinee" },
            { 0xBB, "Old Man Villager" },
            { 0xBC, "Female Villager (Green Hair)" },
            { 0xBD, "King" },
            { 0xBE, "Mana Fortress Cannon" },
            { 0xBF, "Thanatos" },
            { 0xC0, "Elliot" },
            { 0xC1, "Timothy" },
            { 0xC2, "Resistance Man" },
            { 0xC3, "Resistance Woman" },
            { 0xC4, "Potos Guard" },
            { 0xC5, "Pink Dress Villager" },
            { 0xC6, "Long Hair & Dress Villager" },
            { 0xC7, "Child Hat Villager" },
            { 0xC8, "Female Child Villager" },
            { 0xC9, "Village Elder (Green Robe)" },
            { 0xCA, "Old Woman" },
            { 0xCB, "Mara" },
            { 0xCC, "Mini-Mana Beast" },
            { 0xCD, "Flammie Body" },
            { 0xCE, "Cannon Travel Man" },
            { 0xCF, "Shop Owner" },
            { 0xD0, "Glitched Salamando Stove (Dummied Out)" },
            { 0xD1, "Long Hair & Dress Villager (Strange Palette)" },
            { 0xD2, "Veedio" },
            { 0xD3, "High Stepper" },
            { 0xD4, "Cannon Fuse" },
            { 0xD5, "Cannon Explosion 01" },
            { 0xD6, "Cannon Explosion 02" },
            { 0xD7, "Serin" },
            { 0xD8, "Mana Sword In Stone" },
            { 0xD9, "Cannon Barrel Tip" },
            { 0xDA, "Cannon Barrel" },
            { 0xDB, "Waterfall//Geyser Effect" },
            { 0xDC, "Cannon Lower Left Section" },
            { 0xDD, "Cannon Barrel Section" },
            { 0xDE, "Cannon Lower Right Section" },
            { 0xDF, "Mana Seed" },
            { 0xE0, "Pygmy Randi (Broken Palette)" },
            { 0xE1, "Pygmy Purim (Broken Palette)" },
            { 0xE2, "Pygmy Popoie (Broken Palette)" },
            { 0xE3, "Female Villager (Palette Only?)" },
            { 0xE4, "Ghost Randi" },
            { 0xE5, "Ghost Purim" },
            { 0xE6, "Ghost Popoie" },
            { 0xE7, "Female Villager (Palette Only?)" },
            { 0xE8, "Female Villager (Palette Only?)" },
            { 0xE9, "Female Villager (Palette Only?)" },
            { 0xEA, "Treasure Chest (Dropped)" },
            { 0xEB, "Treasure Chest (Static)" },
            { 0xEC, "Statue" },
            { 0xED, "Pygmy Statue" },
            { 0xEE, "Snowman" },
            { 0xEF, "Pygmy Snowman" },
            { 0xF0, "Randi Moogle" },
            { 0xF1, "Purim Moogle" },
            { 0xF2, "Popoie Moogle" },
            { 0xF3, "Randi Pygmy Moogle" },
            { 0xF4, "Purim Pygmy Moogle" },
            { 0xF5, "Popoie Pygmy Moogle" },
            { 0xF6, "Pygmy Randi" },
            { 0xF7, "Pygmy Purim" },
            { 0xF8, "Pygmy Popoie" },
            { 0xF9, "Death Sprite: Splat" },
            { 0xFA, "Death Sprite: Feathers" },
            { 0xFB, "Death Sprite: Ghost" },
            { 0xFC, "Death Sprite: Goo" },
            { 0xFD, "Death Sprite: Insect Bones" },
            { 0xFE, "Death Sprite: Explosion" },
            { 0xFF, "Death Sprite: Humanoid Bones" },
        };

        public MapDisassemblyRomReader(RomFile rom) : base(rom) { }

        public void ReadDoors()
        {
            this.Seek(0x083000);
            for (int i = 0; i < 0x401; i++)
            {
                ushort mapID = (ushort)(this.ReadUInt16() & 0x01FF);
                _ = this.ReadUInt16();
                LoggerEngine.Logger.LogDebug(LogComponent.RomReader, $"[{i:X2}]: {mapID:X4}");
            }
        }

        public void FormatFrame()
        {
            this.Seek(0x07EDE4);
            int index = -1;
            StringBuilder sb = new StringBuilder();
            ushort len = 0;
            while (this.Position < 0x07EE36)
            {
                if (index == -1)
                {
                    sb.AppendLine("[]");
                    sb.Append("C7/").Append($"{(this.Position & 0xFFFF):X4}: ");

                    byte b1 = this.Read();
                    byte b2 = this.Read();
                    len = (ushort)((b2 << 8) | b1);
                    sb.Append(b1.ToString("X2")).Append(b2.ToString("X2"));
                    sb.AppendLine($"        [Length: {len:X2}]");
                    index++;
                }
                else
                {
                    sb.Append("C7/").Append($"{(this.Position & 0xFFFF):X4}: ");
                    sb.Append(this.Read().ToString("X2")).Append(" ").Append(this.Read().ToString("X2")).Append(" ");
                    sb.Append(this.Read().ToString("X2")).Append(" ").Append(this.Read().ToString("X2")).Append(" ");
                    if (index < len) { sb.AppendLine($"[{index:X2}: ]"); }
                    else { sb.AppendLine(); }
                    index++;
                }
            }
            LoggerEngine.Logger.LogDebug(LogComponent.RomReader, sb.ToString());
        }

        public string ReadMapLayerDefinitionPointerTable()
        {
            StringBuilder sb = new StringBuilder();

            this.Seek(Constants.Bank08.MapLayerDefinitionPointerTable);
            for (int i = 0; i <= Constants.Bank08.NumberOfMaps; i++)
            {
                sb.Append(this.Position.ToString("X6")).Append(": ");
                byte offsetL = this.Read();
                byte offsetH = this.Read();
                sb.Append(offsetL.ToString("X2")).Append(offsetH.ToString("X2")).AppendLine($" [{i:X2}: ]");
            }

            return sb.ToString();
        }

        public string ReadMapLayerData()
        {
            StringBuilder sb = new StringBuilder();
            this.Seek(Constants.Bank08.MapLayerDefinitionPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                ushort offset = this.ReadUInt16();
                ushort nextOffset = this.PeekUInt16();
                int pointerTablePosition = this.Position;

                sb.AppendLine($"[{i:X2}: ]");

                // 0xF3 and 0x1FF both point to junk that cannot be read.
                if (offset != nextOffset && i != 0xF3 && i != 0x1FF)
                {
                    this.Seek(Constants.Bank08Offset | offset);
                    int pieceIndex = 0;
                    while (this.Position < (Constants.Bank08Offset | nextOffset))
                    {
                        sb.Append(this.Position.ToString("X6")).Append(": ");
                        byte layer = this.Read();
                        if (layer == 0xFE || this.IsKilroyArenaLayer1(i, offset))
                        {
                            sb.AppendFormat("{0:X2}             [Layer 1 Objects]", layer).AppendLine();
                            sb.AppendLine("        FG RG ID XC YC");
                            pieceIndex = 0;
                        }
                        else if (layer == 0xFF)
                        {
                            sb.AppendFormat("{0:X2}             [Layer 2 Objects]", layer).AppendLine();
                            sb.AppendLine("        FG RG ID XC YC");
                            pieceIndex = 0;
                        }
                        else
                        {
                            byte flag = layer;
                            byte range = this.Read();
                            byte idL = this.Read();
                            byte xCoord = this.Read();
                            byte yCoord = this.Read();

                            byte idH = (byte)((yCoord & 0x01) << 1 | (xCoord & 0x01));
                            ushort id = (ushort)(idH << 8 | idL);

                            sb.AppendFormat("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} [{5:X2}: ({6:X2} - )]", flag, range, idL, xCoord, yCoord, pieceIndex, id).AppendLine();
                            pieceIndex++;
                        }
                    }
                    sb.AppendLine();
                }

                this.Seek(pointerTablePosition);
            }

            return sb.ToString();
        }

        public string ReadMapHeaderNPCPointerTable()
        {
            StringBuilder sb = new StringBuilder();

            this.Seek(Constants.Bank08.MapHeaderNPCPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                sb.Append(this.Position.ToString("X6")).Append(": ");
                byte offsetL = this.Read();
                byte offsetH = this.Read();
                sb.Append(offsetL.ToString("X2")).Append(offsetH.ToString("X2")).AppendLine($" [{i:X2}: ]");
            }

            return sb.ToString();
        }

        public string ReadMapHeaderAndNpcData()
        {
            StringBuilder sb = new StringBuilder();
            int highestId = 0;

            this.Seek(Constants.Bank08.MapHeaderNPCPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                ushort offset = this.ReadUInt16();
                ushort nextOffset = this.PeekUInt16();
                int pointerTablePosition = this.Position;

                sb.AppendLine($"[{i:X2}: ]");
                if (offset != nextOffset && i != 0x1FF)
                {
                    this.Seek(Constants.Bank08Offset | offset);
                    sb.AppendLine("        T8 PL T6 SI SE DS ?? P2");

                    byte tileset8Id = this.Read();
                    byte paletteId = this.Read();
                    byte tileset16Id = this.Read();
                    byte settings = this.Read();
                    byte itemsAllowed = this.Read();
                    byte displaySettingsIndex = this.Read();
                    byte unknownValue = this.Read();
                    byte npcPaletteId = this.Read();

                    byte t8 = (byte)(tileset8Id & 0x3F);
                    if (t8 > highestId) { highestId = t8; }
                    if (t8 == 0x24)
                    {
                        //System.Diagnostics.Debugger.Break();
                    }

                    sb.Append(this.Position.ToString("X6")).Append(": ");
                    sb.AppendFormat("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2}", tileset8Id, paletteId, tileset16Id, settings, itemsAllowed, displaySettingsIndex, unknownValue, npcPaletteId).AppendLine();
                    sb.AppendLine();

                    int npcIndex = 0;
                    while (this.Position < (Constants.Bank08Offset | nextOffset))
                    {
                        byte flag = this.Read();
                        byte range = this.Read();
                        byte xCoord = this.Read();
                        byte yCoord = this.Read();
                        byte direction = this.Read();
                        byte spriteId = this.Read();
                        byte eventIdA = this.Read();
                        byte eventIdB = this.Read();

                        if (npcIndex == 0)
                        {
                            sb.AppendLine("        Fl Rg XC YC DR Tp Evnt");
                        }
                        sb.Append(this.Position.ToString("X6")).Append(": ");
                        string name = MapDisassemblyRomReader.SpriteNameStrings[spriteId];

                        sb.AppendFormat("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2}{7:X2} [{8:X2}: {9}]", flag, range, xCoord, yCoord, direction, spriteId, eventIdA, eventIdB, npcIndex, name).AppendLine();
                        npcIndex++;
                    }
                    sb.AppendLine();
                }

                this.Seek(pointerTablePosition);
            }

            sb.AppendLine(highestId.ToString("X2"));
            return sb.ToString();
        }

        public Dictionary<byte, int> ReadNpcReferenceCount()
        {
            Dictionary<byte, int> npcReferenceCounts = new Dictionary<byte, int>();
            for (int i = 0; i < 256; i++)
            {
                npcReferenceCounts.Add((byte)i, 0);
            }

            this.Seek(Constants.Bank08.MapHeaderNPCPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                ushort offset = this.ReadUInt16();
                ushort nextOffset = this.PeekUInt16();
                int pointerTablePosition = this.Position;

                if (offset != nextOffset && i != 0x1FF)
                {
                    this.Seek(Constants.Bank08Offset | offset);
                    this.Position += 8;
                    while (this.Position < (Constants.Bank08Offset | nextOffset))
                    {
                        this.Position += 5;
                        byte spriteId = this.Read();
                        this.Position += 2;

                        npcReferenceCounts[spriteId]++;
                    }
                }

                this.Seek(pointerTablePosition);
            }

            return npcReferenceCounts;
        }

        public Dictionary<ushort, int> ReadMapPieceReferenceCount()
        {
            Dictionary<ushort, int> mapPieceReferenceCounts = new Dictionary<ushort, int>();
            for (ushort i = 1; i <= Constants.Bank0D.MapPiecePointerTableSize; i++)
            {
                mapPieceReferenceCounts.Add(i, 0);
            }

            this.Seek(Constants.Bank08.MapLayerDefinitionPointerTable);
            for (int i = 0; i < Constants.Bank08.NumberOfMaps; i++)
            {
                ushort offset = this.ReadUInt16();
                ushort nextOffset = this.PeekUInt16();
                int pointerTablePosition = this.Position;

                // 0xF3 and 0x1FF both point to junk that cannot be read.
                if (offset != nextOffset && i != 0xF3 && i != 0x1FF)
                {
                    this.Seek(Constants.Bank08Offset | offset);
                    while (this.Position < (Constants.Bank08Offset | nextOffset))
                    {
                        byte layer = this.Read();
                        if (layer != 0xFE && layer != 0xFF && !this.IsKilroyArenaLayer1(i, offset))
                        {
                            // Don't need the flag and range bytes.
                            this.Position++;
                            byte idL = this.Read();
                            byte xCoord = this.Read();
                            byte yCoord = this.Read();

                            byte idH = (byte)((yCoord & 0x01) << 1 | (xCoord & 0x01));
                            ushort id = (ushort)(idH << 8 | idL);

                            mapPieceReferenceCounts[id]++;
                        }
                    }
                }

                this.Seek(pointerTablePosition);
            }

            return mapPieceReferenceCounts;
        }

        public Dictionary<byte, int> ReadCollisionReferenceCount()
        {
            Dictionary<byte, int> collisionReferenceCounts = new Dictionary<byte, int>();
            for (int i = 0; i <= 0x7F; i++)
            {
                collisionReferenceCounts.Add((byte)i, 0);
            }

            this.Seek((int)Constants.Bank0B.MapCollisionDefinitionTableAddress);
            uint lastOffset = (Constants.Bank0B.MapCollisionDefinitionSize * 2) * (Constants.Bank0B.MapCollisionDefinitionTableSize - 1);
            uint lastIndex = Constants.Bank0B.MapCollisionDefinitionTableAddress | lastOffset;
            while (this.Position < lastIndex)
            {
                collisionReferenceCounts[this.Read()]++;
            }

            return collisionReferenceCounts;
        }

        public string ReadAnimationTileTable()
        {
            StringBuilder sb = new StringBuilder();
            this.Seek(Constants.Bank08.MapAnimatedTilesIndexTable);

            List<byte> animationTileIndexes = new List<byte>((int)Constants.Bank0C.Map8x8TilesetPointerTableSize);
            for (int i = 0; i < Constants.Bank08.MapAnimatedTilesIndexTableSize; i++)
            {
                sb.Append(this.Position.ToString("X6")).Append(": ");
                byte b = this.Read();

                sb.AppendLine($"{b:X2} [{i:X2}: {ManaMetadata.Map8x8TilesetMetadata[i].Name}]");
                animationTileIndexes.Add(b);
            }
            sb.AppendLine();


            Dictionary<byte, List<byte>> encodedOffsets = new Dictionary<byte, List<byte>>();
            for (int i = 0; i < Constants.Bank0C.Map8x8TilesetPointerTableSize; i++)
            {
                this.Seek(Constants.Bank08.MapAnimatedTilesEncodedPointerTable + (animationTileIndexes[i] * 2));
                sb.AppendLine($"[{i:X2}: {ManaMetadata.Map8x8TilesetMetadata[i].Name}]");

                if ((i + 1) < Constants.Bank08.MapAnimatedTilesIndexTableSize)
                {
                    int length = (animationTileIndexes[i + 1] - animationTileIndexes[i]) * 2;
                    for (int j = 0; j < length; j++)
                    {
                        sb.Append(this.Position.ToString("X6")).Append(": ");
                        byte b = this.Read();
                        ushort decodedValue = (ushort)((((b & 0x3F) << 1) | 0x80) << 8);
                        sb.AppendLine($"{b:X2} [{j:X2}: {(Constants.Bank1CByte | 0xC0):X2}/{decodedValue:X4}]");
                    }
                    if (length == 0)
                    {
                        sb.AppendLine("***NO ANIMATED TILES***");
                    }
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private bool IsKilroyArenaLayer1(int index, ushort offset)
        {
            // The map data for Kilroy's arena is missing the FE marker for its layer 1 data.
            return index == 0x13D && this.Position == (Constants.Bank08Offset | offset);
        }
    }

    internal sealed class DisassemblyUtil
    {
        private SecretOfManaContext context = null!;

        private void LoadBank03_Stuff()
        {
            int spcCode = 0x033D39;
            //ushort length = this.context.RomFile.ReadUInt16At(spcCode);
            //spcCode += 2;

            int offset = 0x033D39;
            int endIndex = offset + (256 * 3);

            StringBuilder sb = new StringBuilder(0);
            int index = 0;
            while (offset < endIndex)
            {
                //byte header = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("03/").Append((offset & 0xFFFF).ToString("X4")).Append(": ");
                    }
                    sb.Append(this.context!.RomFile!.ReadByteAt(spcCode).ToString("X2"));
                    spcCode++;
                    offset++;
                }
                sb.AppendLine($" [{index:X2}: ]");
                index++;
            }

            //richTextBox1.Text = sb.ToString();
        }

        private void LoadSPC700_01_R()
        {
            // C3/1AE1
            int spcCode = 0x03074A;
            //ushort length = this.context.RomFile.ReadUInt16At(spcCode);
            //spcCode += 2;

            int offset = 0x00074A;
            int endIndex = 0x031AE1;

            StringBuilder sb = new StringBuilder(0);
            while (spcCode <= endIndex)
            {
                byte header = 0;
                for (int i = 0; i < 16; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("03").Append((offset & 0xFFFF).ToString("X4")).Append(": ");
                        header = this.context!.RomFile!.ReadByteAt(spcCode);
                    }
                    sb.Append(this.context!.RomFile!.ReadByteAt(spcCode).ToString("X2"));

                    //if (i == 1) { sb.Append(" "); }

                    offset++;
                    spcCode++;
                }
                sb.AppendLine();
            }

            //richTextBox1.Text = sb.ToString();
        }

        private void LoadSPC700_01_Sample()
        {
            int spcCode = 0x032137;
            ushort length = this.context!.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            int offset = 0x002C00;
            int endIndex = offset + length;

            StringBuilder sb = new StringBuilder(0);
            while (offset < endIndex)
            {
                byte header = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("00/").Append(offset.ToString("X4")).Append(": ");
                        header = this.context.RomFile.ReadByteAt(spcCode);
                    }
                    sb.Append(this.context.RomFile.ReadByteAt(spcCode).ToString("X2"));

                    if (i == 1) { sb.Append(" "); }

                    offset++;
                    spcCode++;
                }
                sb.AppendLine();
            }

            //richTextBox1.Text = sb.ToString();
        }

        private StringBuilder ParseBRRSampleBlockForDisassembly(StringBuilder sb, int bank, int offset)
        {
            int location = (bank << 16) | offset;
            ushort length = this.context.RomFile!.ReadUInt16At(location);
            int endIndex = location + length + 2;

            sb.Append(location.ToString("X6")).Append(": ");
            sb.Append(this.context.RomFile!.ReadByteAt(location).ToString("X2"));
            location++;
            sb.Append(this.context.RomFile!.ReadByteAt(location).ToString("X2"));
            sb.AppendLine(" [Size Of Data Block]");
            location++;

            sb.AppendLine("        Hd B1 B2 B3 B4 B5 B6 B7 B8");
            while (location < endIndex)
            {
                byte header = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(location.ToString("X6")).Append(": ");
                        header = this.context.RomFile.ReadByteAt(location);
                    }
                    sb.Append(this.context.RomFile.ReadByteAt(location).ToString("X2")).Append(" ");

                    location++;
                }
                sb.Append("[");
                byte range = (byte)((header & 0xF0) >> 4);
                byte filer = (byte)((header & 0x0C) >> 2);
                string loop = (header & 0x02) > 0 ? "Yes" : "No";
                string end = (header & 0x01) > 0 ? "Yes" : "No";

                sb.Append("Range: ").Append(range.ToString("X2"));
                sb.Append(", Filer: ").Append(filer.ToString("X2"));
                sb.Append(", Loop: ").Append(loop);
                sb.Append(", End: ").Append(end);
                sb.Append("]");
                sb.AppendLine();
            }

            return sb;
        }

        private StringBuilder ParseBRRSamplesPerSongForDisassembly(StringBuilder sb, int bank, int offset)
        {
            int location = (bank << 16) | offset;
            int endIndex = 0x034781;

            int index = 0;
            while (location < endIndex)
            {
                string indexName = index < ManaMetadata.MusicMetadata.Count ? ManaMetadata.MusicMetadata[index].TrackName : string.Empty;
                sb.Append("[").Append(index.ToString("X2")).AppendFormat(": {0}]", indexName).AppendLine();
                int innerIndex = 0;
                for (int i = 0; i < 16; i++)
                {
                    sb.Append(location.ToString("X6")).Append(": ");
                    sb.Append(this.context.RomFile!.ReadByteAt(location).ToString("X2"));
                    location++;
                    sb.Append(this.context.RomFile.ReadByteAt(location).ToString("X2"));
                    location++;

                    if (this.context.RomFile.ReadUInt16At(location - 2) == 0)
                    {
                        sb.Append(" [").Append(innerIndex.ToString("X2")).AppendLine(": No Instrument]");
                    }
                    else
                    {
                        sb.Append(" [").Append(innerIndex.ToString("X2")).AppendLine(": ]");
                    }
                    innerIndex++;
                }
                sb.AppendLine();
                index++;
            }
            return sb;
        }

        private StringBuilder ParseAkaoEngine01ForDisassembly(StringBuilder sb)
        {
            int bank = 0x03;
            int offset = 0x002137;
            int location = (bank << 16) | offset;
            ushort length = this.context.RomFile!.ReadUInt16At(location);
            int endIndex = location + length + 2;
            int endIndex2 = 0x2C00 + length;
            location += 2;

            // 00/2C00 - 11264
            // C3/2139 - 8505
            // ---------------
            //   /0AE6 - 2790

            ushort addressFixup = (ushort)(0x2C00 - (offset + 2));//0AC9 - C90A
            int fixUp2 = ((addressFixup & 0x00FF)) << 8 | ((addressFixup & 0xFF00) >> 8);

            int bank2 = 0x00;
            int location2 = 0x002C00;
            List<ushort> sequencePointers = new List<ushort>(256);
            for (int i = 0; i < 256; i++)
            {

                ushort p1BE = this.context.RomFile.ReadBigEndianUInt16At(location);
                ushort p1LE = this.context.RomFile.ReadUInt16At(location);
                location += 2;
                location2 += 2;
                ushort p2BE = this.context.RomFile.ReadBigEndianUInt16At(location);
                ushort p2LE = this.context.RomFile.ReadUInt16At(location);
                location += 2;
                location2 += 2;

                if (p1BE > 0) { sequencePointers.Add((ushort)(p1LE - addressFixup)); }
                if (p2BE > 0) { sequencePointers.Add((ushort)(p2LE - addressFixup)); }

                p1LE = (ushort)(p1LE > 0 ? p1LE - addressFixup : 0);
                p2LE = (ushort)(p2LE > 0 ? p2LE - addressFixup : 0);
                sb.AppendFormat("{0:X6}: {1:X4} {2:X4} [{3:X2}: ]", location2 - 4, p1BE, p2BE, i).AppendLine();
            }

            sb.AppendLine();
            for (int i = 0; i < sequencePointers.Count; i++)
            {
                ushort current = sequencePointers[i];
                ushort next = (ushort)(i + 1 < sequencePointers.Count ? sequencePointers[i + 1] : endIndex);

                sb.Append(bank2.ToString("X2")).Append("/").Append((current + addressFixup).ToString("X4")).Append(": ");
                while (current != next)
                {
                    byte scriptByte = this.context.RomFile.ReadByteAt((bank << 16) | current);
                    sb.Append(scriptByte.ToString("X2"));
                    current++;
                    if (current != next) { sb.Append(" "); }
                }
                sb.AppendLine();
            }

            return sb;
        }

        private void LoadSPC700_00()
        {
            int spcCode = 0x030748;
            ushort length = this.context.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            SPC700Disassembler disassembler = new SPC700Disassembler();
            DisassembledCodeBlock codeBlock = disassembler.Disassemble(this.context.RomFile, spcCode, length, 0x000200);
            //richTextBox1.AppendText(codeBlock.ToString());
        }

        private void LoadSPC700_01()
        {
            int spcCode = 0x032137;
            ushort length = this.context.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            SPC700Disassembler disassembler = new SPC700Disassembler();
            DisassembledCodeBlock codeBlock = disassembler.Disassemble(this.context.RomFile, spcCode, length, 0x0002C00);
            //richTextBox1.AppendText(codeBlock.ToString());
        }

        private void LoadSPC700_02()
        {
            int spcCode = 0x031AE2;
            ushort length = this.context.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            SPC700Disassembler disassembler = new SPC700Disassembler();
            DisassembledCodeBlock codeBlock = disassembler.Disassemble(this.context.RomFile, spcCode, length, 0x0004800);
            //richTextBox1.AppendText(codeBlock.ToString());
        }

        private void LoadSPC700_03()
        {
            int spcCode = 0x0320D5;
            ushort length = this.context.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            SPC700Disassembler disassembler = new SPC700Disassembler();
            DisassembledCodeBlock codeBlock = disassembler.Disassemble(this.context.RomFile, spcCode, length, 0x0001900);
            //richTextBox1.AppendText(codeBlock.ToString());
        }

        private void LoadSPC700_04()
        {
            int spcCode = 0x032103;
            ushort length = this.context.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            SPC700Disassembler disassembler = new SPC700Disassembler();
            DisassembledCodeBlock codeBlock = disassembler.Disassemble(this.context.RomFile, spcCode, length, 0x0001880);
            //richTextBox1.AppendText(codeBlock.ToString());
        }

        private void LoadSPC700_05()
        {
            int spcCode = 0x03211D;
            ushort length = this.context.RomFile!.ReadUInt16At(spcCode);
            spcCode += 2;

            SPC700Disassembler disassembler = new SPC700Disassembler();
            DisassembledCodeBlock codeBlock = disassembler.Disassemble(this.context.RomFile, spcCode, length, 0x0001800);
            //richTextBox1.AppendText(codeBlock.ToString());
        }
    }
}