using System;
using System.Collections.Generic;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Bosses.Animations;
using ManaMagic.Core.Bosses.BossInitialization;
using ManaMagic.Core.Bosses.Frames;
using ManaMagic.Core.Metadata;
using ZwellTech;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.RomReaders
{
    /// <summary>
    /// Provides a Secret of Mana ROM reader that encapsulates methods to read boss related data.
    /// </summary>
    public sealed class BossRomReader : RomReader
    {
        private readonly BossInitializationScriptParser initializationScriptParser;
        private readonly BossAnimationParser animationParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BossRomReader"/> class with the specified <see cref="RomFile"/>.
        /// </summary>
        /// <param name="rom">A <see cref="RomFile"/> from which data will be read.</param>
        public BossRomReader(RomFile rom) : base(rom)
        {
            this.initializationScriptParser = new BossInitializationScriptParser(this);
            this.animationParser = new BossAnimationParser(this);
        }

        public DataTable<SpritePalette> ReadPaletteTable()
        {
            // There are 135 (0x87) total boss palettes.
            List<SpritePalette> palettes = new List<SpritePalette>((int)Constants.Bank10.BossPalettePointerTableSize);

            int pointerTablePosition = (int)Constants.Bank10.BossPalettePointerTableAddress;
            for (int paletteIndex = 0; paletteIndex < Constants.Bank10.BossPalettePointerTableSize; paletteIndex++)
            {
                // Bank 10 contains a pointer table for all the boss palettes. The palettes themselves are stored at the end of Bank 02.
                this.Seek(pointerTablePosition);
                int address = Constants.Bank02Offset | this.ReadUInt16();
                pointerTablePosition = this.Seek(address);

                // Boss palettes contain sixteen 15-bit colors for a total of 32 bytes per palette.
                List<Rgb555Color> colors = new List<Rgb555Color>(Constants.Bank02.BossPaletteColorCount);
                for (int colorIndex = 0; colorIndex < Constants.Bank02.BossPaletteColorCount; colorIndex++)
                {
                    colors.Add(Rgb555Color.FromRgb(this.ReadUInt16(), Rgb555Format.BGR));
                }
                palettes.Add(new SpritePalette((byte)paletteIndex, colors, address));
            }

            return new DataTable<SpritePalette>(palettes);
        }

        public DataTable<BossGraphicsTableEntry> ReadGraphicsTable()
        {
            this.Seek((int)Constants.Bank10.BossGraphicsPointerTableAddress);
            List<BossGraphicsTableEntry> entryList = new List<BossGraphicsTableEntry>(Constants.Bank10.BossGraphicsPointerTableSize);
            for (byte i = 0; i < Constants.Bank10.BossGraphicsPointerTableSize; i++)
            {
                byte gb = this.Read();
                byte rb = this.Read();
                ushort gAdr = this.ReadUInt16();
                ushort rAdr = this.ReadUInt16();
                ushort size = this.ReadUInt16();

                BossGraphicsTableEntry entry = new BossGraphicsTableEntry(i, gb, rb, gAdr, rAdr, size);
                entryList.Add(entry);
            }
            return new DataTable<BossGraphicsTableEntry>(entryList);
        }

        public DataTable<byte> ReadSkillGraphicDefinitionIndexTable()
        {
            this.Seek((int)Constants.Bank10.BossSkillGraphicDefinitionIndexTableAddress);
            List<byte> indexList = new List<byte>((int)Constants.Bank10.BossSkillGraphicDefinitionIndexTableSize);
            for (byte i = 0; i < Constants.Bank10.BossSkillGraphicDefinitionIndexTableSize; i++)
            {
                indexList.Add(this.Read());
            }
            return new DataTable<byte>(indexList);
        }

        public DataTable<DataTable<byte>> ReadSkillGraphicDefinitionTable(DataTable<byte> indexTable)
        {
            List<DataTable<byte>> indexList = new List<DataTable<byte>>(indexTable.RowCount);
            for (int i = 0; i < 0x0C; i++)
            {
                this.Seek((int)((int)Constants.Bank10.BossSkillGraphicDefinitionTableAddress + (i * Constants.Bank10.BossSkillGraphicIndexsPerSet)));

                bool read = true;
                List<byte> indexSet = new List<byte>((int)Constants.Bank10.BossSkillGraphicIndexsPerSet);
                for (int j = 0; j < Constants.Bank10.BossSkillGraphicIndexsPerSet; j++)
                {
                    byte index = read ? this.Read() : Constants.Bank10.BossSkillGraphicInvalidIndex;
                    indexSet.Add(index);
                }
                indexList.Add(new DataTable<byte>(indexSet));
            }
            return new DataTable<DataTable<byte>>(indexList);
        }

        public DataTable<ushort> ReadSkillGraphicDefinitionPointerTable()
        {
            this.Seek((int)Constants.Bank10.BossSkillGraphicsPointerTableAddress);
            List<ushort> pointerList = new List<ushort>((int)Constants.Bank10.BossSkillGraphicsPointerTableSize);
            for (byte i = 0; i < Constants.Bank10.BossSkillGraphicsPointerTableSize; i++)
            {
                pointerList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(pointerList);
        }

        public DataTable<ushort> ReadSkillAIPointerTable()
        {
            this.Seek((int)Constants.Bank1C.BossSkillAIPointerTableAddress);
            List<ushort> aiPointerList = new List<ushort>((int)Constants.Bank1C.BossSkillAIPointerTableSize);
            for (byte i = 0; i < Constants.Bank1C.BossSkillAIPointerTableSize; i++)
            {
                aiPointerList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(aiPointerList);
        }

        public DataTable<ushort> ReadSkillPaletteTable()
        {
            this.Seek((int)Constants.Bank1C.BossSkillPaletteIdTableAddress);
            List<ushort> paletteIdList = new List<ushort>((int)Constants.Bank1C.BossSkillPaletteIdTableSize);
            for (byte i = 0; i < Constants.Bank1C.BossSkillPaletteIdTableSize; i++)
            {
                paletteIdList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(paletteIdList);
        }

        public DataTable<ushort> ReadSkillBossAnimationIdTable()
        {
            this.Seek((int)Constants.Bank1C.BossSkillBossAnimationIdTableAddress);
            List<ushort> bossAnimationIdList = new List<ushort>((int)Constants.Bank1C.BossSkillBossAnimationIdTableSize);
            for (byte i = 0; i < Constants.Bank1C.BossSkillBossAnimationIdTableSize; i++)
            {
                bossAnimationIdList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(bossAnimationIdList);
        }

        public DataTable<ushort> ReadSkillPlayerAnimationIdTable()
        {
            this.Seek((int)Constants.Bank1C.BossSkillPlayerAnimationIdTableAddress);
            List<ushort> bossAnimationIdList = new List<ushort>((int)Constants.Bank1C.BossSkillPlayerAnimationIdTableSize);
            for (byte i = 0; i < Constants.Bank1C.BossSkillPlayerAnimationIdTableSize; i++)
            {
                bossAnimationIdList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(bossAnimationIdList);
        }

        public DataTable<ushort> ReadSkillBossSoundEffectIdTable()
        {
            this.Seek((int)Constants.Bank1C.BossSkillBossSoundEffectIdTableAddress);
            List<ushort> bossAnimationIdList = new List<ushort>((int)Constants.Bank1C.BossSkillBossSoundEffectIdTableSize);
            for (byte i = 0; i < Constants.Bank1C.BossSkillBossSoundEffectIdTableSize; i++)
            {
                bossAnimationIdList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(bossAnimationIdList);
        }

        public DataTable<ushort> ReadSkillPlayerSoundEffectIdTable()
        {
            this.Seek((int)Constants.Bank1C.BossSkillPlayerSoundEffectIdTableAddress);
            List<ushort> bossAnimationIdList = new List<ushort>((int)Constants.Bank1C.BossSkillPlayerSoundEffectIdTableSize);
            for (byte i = 0; i < Constants.Bank1C.BossSkillPlayerSoundEffectIdTableSize; i++)
            {
                bossAnimationIdList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(bossAnimationIdList);
        }

        public TilesetDictionary ReadSkillTilesetDictionary(DataTable<byte> indexTable, DataTable<DataTable<byte>> skillDefinitionTable, DataTable<ushort> skillGraphicsPointerTable)
        {
            Dictionary<byte, Tileset> tileSets = new Dictionary<byte, Tileset>(skillDefinitionTable.RowCount);

            // 64 is the length of a Mode 7 tile. Most boss graphics are 32 bytes per tile.
            // The initialization for a 4BPP tile will only look at the first 32 bytes of the buffer.
            Span<byte> tileBuffer = stackalloc byte[32];
            tileBuffer.Fill(0);
            for (int i = 0; i < indexTable.RowCount; i++)
            {
                DataTable<byte> definitionTable = skillDefinitionTable[indexTable[i]];
                List<GraphicTile> tileList = new List<GraphicTile>(32);
                for (int setIndex = 0; setIndex < Constants.Bank10.BossSkillGraphicIndexsPerSet; setIndex++)
                {
                    byte definitionIndex = definitionTable[setIndex];
                    if (definitionIndex == Constants.Bank10.BossSkillGraphicInvalidIndex) { break; }

                    int pointer = Constants.Bank1BOffset | skillGraphicsPointerTable[definitionIndex];
                    this.Seek(pointer);

                    List<byte> decompressedData = new List<byte>(Constants.Bank10.BossSkillGraphicsSize);
                    for (int decompressedIndex = 0; decompressedIndex < Constants.Bank10.BossSkillGraphicsSize; decompressedIndex++)
                    {
                        decompressedData.Add(this.Read());
                    }
                    decompressedData = LZ77Decompressor.Decompress(decompressedData, Constants.Bank10.BossSkillGraphicsSize);

                    // For Read Loop control.
                    int bytesPerTile = 32;
                    int numberOfTiles = decompressedData.Count / bytesPerTile;
                    int dataIndex = 0;

                    // Loop though the data stream and create all the 8x8 tiles for the tileset.
                    for (int tileIndex = 0; tileIndex < numberOfTiles; tileIndex++)
                    {
                        // Load each byte of the tile.
                        for (int byteIndex = 0; byteIndex < bytesPerTile; byteIndex++)
                        {
                            // If we've processed all the readable bytes but still need to continue, use a default value of 0x00 for the remaining bytes and tiles.
                            tileBuffer[byteIndex] = (dataIndex + byteIndex) < decompressedData.Count ? decompressedData[dataIndex + byteIndex] : (byte)0x00;
                        }

                        // Create the tile.
                        GraphicTile newTile = GraphicTile4Bpp.From4BppTile(tileBuffer);
                        tileList.Add(newTile);

                        // Advance the index to the next tile.
                        dataIndex += bytesPerTile;
                    }
                }
                tileSets.Add((byte)i, new Tileset(i, tileList, TileType.FourBitsPerPixel));
            }

            return new TilesetDictionary(tileSets);
        }

        public TilesetDictionary ReadTilesetDictionary(DataTable<BossGraphicsTableEntry> graphicsTable, LZ77Decompressor decompressor)
        {
            Dictionary<byte, Tileset> tileSets = new Dictionary<byte, Tileset>(graphicsTable.RowCount);

            // 64 is the length of a Mode 7 tile. Most boss graphics are 32 bytes per tile.
            // The initialization for a 4BPP tile will only look at the first 32 bytes of the buffer.
            Span<byte> tileBuffer = stackalloc byte[64];
            tileBuffer.Fill(0);
            for (int i = 0; i < graphicsTable.RowCount; i++)
            {
                BossGraphicsTableEntry row = graphicsTable[i];
                BossTilesetMetadata metadata = ManaMetadata.BossTilesetMetadata[row.Index];
                TileType tileType = !metadata.IsMode7 ? TileType.FourBitsPerPixel : TileType.Mode7;

                // If the graphics are double compressed then run the LZ77Decompressor and get the resulting byte stream, otherwise just read the bytes.
                IReadOnlyList<byte> decompressedData = this.ReadGraphics(row, metadata, decompressor);

                // Regardless of if the main LZ77Decompressor is ran, all boss graphics are futher compressed with an abridged version.
                decompressedData = LZ77Decompressor.Decompress(decompressedData, row.GraphicsSize);

                // For Read Loop control.
                int bytesPerTile = !metadata.IsMode7 ? 32 : 64;
                int numberOfTiles = decompressedData.Count / bytesPerTile;
                int dataIndex = 0;

                // Loop though the data stream and create all the 8x8 tiles for the tileset.
                List<GraphicTile> tileList = new List<GraphicTile>(numberOfTiles);
                for (int j = 0; j < numberOfTiles; j++)
                {
                    // Load each byte of the tile.
                    for (int byteIndex = 0; byteIndex < bytesPerTile; byteIndex++)
                    {
                        // If we've processed all the readable bytes but still need to continue, use a default value of 0x00 for the remaining bytes and tiles.
                        tileBuffer[byteIndex] = (dataIndex + byteIndex) < decompressedData.Count ? decompressedData[dataIndex + byteIndex] : (byte)0x00;
                    }

                    // Create the tile.
                    GraphicTile newTile = !metadata.IsMode7 ? GraphicTile4Bpp.From4BppTile(tileBuffer) : GraphicTileMode7.FromMode7Tile(tileBuffer);
                    tileList.Add(newTile);

                    // Advance the index to the next tile.
                    dataIndex += bytesPerTile;
                }
                tileSets.Add(row.Index, new BossTileset(i, tileList, row, metadata, tileType));
            }

            return new TilesetDictionary(tileSets);
        }

        public DataTable<ushort> ReadStateMachineSetupPointerTable()
        {
            this.Seek((int)Constants.Bank10.BossStateMachineSetupPointerTableAddress);
            List<ushort> stateMachineSetupPointerList = new List<ushort>((int)Constants.Bank10.BossStateMachineSetupPointerTableSize);
            for (byte i = 0; i < Constants.Bank10.BossStateMachineSetupPointerTableSize; i++)
            {
                stateMachineSetupPointerList.Add(this.ReadUInt16());
            }
            return new DataTable<ushort>(stateMachineSetupPointerList);
        }

        public DataTable<BossSetupHeader> ReadSetupHeaders(DataTable<ushort> pointerTable)
        {
            List<BossSetupHeader> headerList = new List<BossSetupHeader>(pointerTable.RowCount);
            int i = 0;
            foreach (ushort offset in pointerTable)
            {
                this.Seek(Constants.Bank02Offset | offset);

                ushort controlFlags = this.ReadUInt16();
                ushort spriteFlags = this.ReadUInt16();
                ushort initializeRoutinePointer = this.ReadUInt16();
                ushort movementRoutinePointer = this.ReadUInt16();
                ushort attackRoutinePointer = this.ReadUInt16();
                ushort damagedRoutinePointer = this.ReadUInt16();
                ushort defaultCommandScriptPointer = this.ReadUInt16();
                ushort deathCommandScriptPointer = this.ReadUInt16();
                ushort aiCommandSetPointer = this.ReadUInt16();
                ushort shadowSize = this.ReadUInt16();
                ushort postDeathHandlerPointer = this.ReadUInt16();
                ushort unknownValue = this.ReadUInt16();
                ushort specialDeathHandlerPointer = this.ReadUInt16();

                BossSetupHeader header = new BossSetupHeader((byte)i,
                                                             controlFlags,
                                                             spriteFlags,
                                                             initializeRoutinePointer,
                                                             movementRoutinePointer,
                                                             attackRoutinePointer,
                                                             damagedRoutinePointer,
                                                             defaultCommandScriptPointer,
                                                             deathCommandScriptPointer,
                                                             aiCommandSetPointer,
                                                             shadowSize,
                                                             postDeathHandlerPointer,
                                                             unknownValue,
                                                             specialDeathHandlerPointer);
                headerList.Add(header);
                i++;
            }
            return new DataTable<BossSetupHeader>(headerList);
        }

        public DataTable<ManaBossWeapon> ReadWeaponTable()
        {
            this.Seek((int)Constants.Bank10.BossWeaponTableAddress);
            List<ManaBossWeapon> weaponList = new List<ManaBossWeapon>((int)Constants.Bank10.BossWeaponTableSize);
            for (byte i = 0; i < Constants.Bank10.BossWeaponTableSize; i++)
            {
                MonsterType affinity = (MonsterType)this.Read();
                ElementalType element = (ElementalType)this.Read();
                byte accuracy = this.Read();
                byte power = this.Read();
                StatusEffects statusEffects = (StatusEffects)this.ReadUInt16();
                byte inflictionRate = this.Read();

                weaponList.Add(new ManaBossWeapon(i, affinity, element, accuracy, power, statusEffects, inflictionRate));
            }
            return new DataTable<ManaBossWeapon>(weaponList);
        }

        public IReadOnlyDictionary<BossFamily, BossFrameList> ReadFrameDictionary()
        {
            Dictionary<BossFamily, BossFrameList> dictionary = new Dictionary<BossFamily, BossFrameList>();
            foreach (BossFamily family in Enum.GetValues(typeof(BossFamily)))
            {
                if (ManaMetadata.BossFrameAddressFactory.HasFrames(family))//family != BossFamily.Unknown
                {
                    BossFrameList frameList = this.ReadFrameList(family);
                    dictionary.Add(family, frameList);
                }
            }
            return dictionary;
        }

        public DataTable<BossAnimationScript> ReadAnimationScriptTable()
        {
            List<BossAnimationScript> scriptList = new List<BossAnimationScript>((int)Constants.Bank1B.BossAnimationPointerTableSize);
            for (uint i = 0; i < Constants.Bank1B.BossAnimationPointerTableSize; i++)
            {
                scriptList.Add(this.animationParser.ParseAnimation(i));
            }
            return new DataTable<BossAnimationScript>(scriptList);
        }

        public DataTable<BossInitializationScript> ReadBossInitializationScriptTable()
        {
            List<BossInitializationScript> list = new List<BossInitializationScript>((int)Constants.Bank02.BossInitializationScriptPointerTableTableSize);
            for (int i = 0; i < Constants.Bank02.BossInitializationScriptPointerTableTableSize; i++)
            {
                list.Add(this.initializationScriptParser.ParseScript((uint)i));
            }
            return new DataTable<BossInitializationScript>(list);
        }

        public DataTable<BossPaletteScript> ReadBossInitializationPaletteScriptTable()
        {
            List<BossPaletteScript> list = new List<BossPaletteScript>((int)Constants.Bank0A.BossPaletteScriptPointerTableTableSize);
            for (int i = 0; i < Constants.Bank0A.BossPaletteScriptPointerTableTableSize; i++)
            {
                int pointerOffset = sizeof(ushort) * 3; // 3 invalid pointers at the start of this table. The loader subtracts 0x54 instead of 0x57.
                this.Seek((int)(Constants.Bank0A.BossPaletteScriptPointerTableAddress + (sizeof(ushort) * i) + pointerOffset));
                this.Seek(Constants.Bank01Offset | this.ReadUInt16());

                bool done = false;
                int palette1Index = -1;
                int palette2Index = -1;
                int palette3Index = -1;
                int manaBeastSlot1 = -1;
                int manaBeastSlot2 = -1;
                int backgroundPaletteSlot1 = -1;
                int backgroundPaletteSlot2 = -1;
                while (!done)
                {
                    byte command = this.Read();
                    if (command == 0xFF) { done = true; }
                    else if (command == 0x04) { manaBeastSlot1 = this.Read(); } // Mana Beast
                    else if (command == 0x05) { manaBeastSlot2 = this.Read(); } // Mana Beast
                    else if (command == 0x06) { backgroundPaletteSlot1 = this.Read(); } // Wall Face/Doom's Wall/DarkLich
                    else if (command == 0x07) { backgroundPaletteSlot2 = this.Read(); } // Lime Slime/Dread Slime/Aegagropilon/SnowDragon/RedDragon/BlueDragon/DarkLich
                    else if (command == 0x0D) { palette1Index = this.Read(); }
                    else if (command == 0x0E) { palette2Index = this.Read(); }
                    else if (command == 0x0F) { palette3Index = this.Read(); }
                    else { ThrowHelper.ThrowInvalidOperationException("Unknown palette slot"); }
                }
                list.Add(new BossPaletteScript(palette1Index, palette2Index, palette3Index, manaBeastSlot1, manaBeastSlot2, backgroundPaletteSlot1, backgroundPaletteSlot2));
            }
            return new DataTable<BossPaletteScript>(list);
        }

        private BossFrameList ReadFrameList(BossFamily family)
        {
            int frameCount = ManaMetadata.BossFrameAddressFactory.GetFrameCount(family);

            List<BossFrame> frameList = new List<BossFrame>();
            for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                BossFrame frame = this.ReadFrame(family, frameIndex);
                frameList.Add(frame);
            }
            return new BossFrameList(family, frameList);
        }

        internal BossFrame ReadFrame(BossFamily family, int frameIndex)
        {
            BossFrameType frameType = ManaMetadata.BossFrameAddressFactory.GetFrameType(family, frameIndex);
            switch (frameType)
            {
                case BossFrameType.Sprite: return this.ReadSpriteBossFrame(family, frameIndex);
                case BossFrameType.Background: return this.ReadBackgroundBossFrame(family, frameIndex);
                case BossFrameType.Mode7: return this.ReadBackgroundBossFrame(family, frameIndex);
            }
            return (BossFrame)ThrowHelper.ThrowInvalidOperationException("Invalid frame type.");
        }

        private BossSpriteFrame ReadSpriteBossFrame(BossFamily family, int frameIndex)
        {
            string name = ManaMetadata.BossFrameAddressFactory.GetFrameName(family, frameIndex);
            int baseAddress = ManaMetadata.BossFrameAddressFactory.GetFrameAddress(family, frameIndex);
            bool hasHitBox = ManaMetadata.BossFrameAddressFactory.FrameHasHitBoxData(family, frameIndex);
            ushort frameLength = this.RomFile.ReadUInt16At(baseAddress);
            this.Seek(baseAddress + 2);

            List<BossSpriteFramePart> frameParts = new List<BossSpriteFramePart>(frameLength);
            for (int i = 0; i < frameLength; i++)
            {
                sbyte x = this.ReadSByte();                           // X-Coordinate of the drawn tile, relative to the bosses origin.
                sbyte y = this.ReadSByte();                           // Y-Coordinate of the drawn tile, relative to the bosses origin.
                byte id = this.Read();                                // Id of the drawn tile.
                BossFrameFlags flags = (BossFrameFlags)this.Read();   // State flags.

                BossSpriteFramePart framePart = new BossSpriteFramePart(x, y, id, flags);
                frameParts.Add(framePart);
            }

            // Boss objects flagged as children do not run standard hit tests and in general don't define hitbox data in their frames.
            // We can't rely on this fact though as some child objects will define hitbox data and their AI will manually run hit tests.
            // This is usually the case for things like projectiles and notably, Hexas's serpent body.
            // All of this to say "Ugh, I have to hard-code another value in all the frame metadata."
            BossHitBox[] hitboxes = new BossHitBox[3];
            if (hasHitBox)
            {
                // A frame will contain data for 3 hitboxes in the order of Hit > Weapon > Guard.
                // HitBox data is 4 bytes long, unless the first two bytes (the width/height) are zero, in which case the data is only two bytes long.
                for (int hitBoxIndex = 0; hitBoxIndex < 3; hitBoxIndex++)
                {
                    byte width = this.Read();  // Half-Width of the hit box.
                    byte height = this.Read(); // Half-Height of the hit box.
                    sbyte hitX = 0;            // X-Coordinate of the center of the hitbox, relative to the bosses origin.
                    sbyte hitY = 0;            // Y-Coordinate of the center of the hitbox, relative to the bosses origin.

                    // Yeah, using OR is dumb, but that's how the game does it.
                    if (width != 0 || height != 0)
                    {
                        hitX = this.ReadSByte();
                        hitY = this.ReadSByte();
                    }

                    hitboxes[hitBoxIndex] = new BossHitBox((BossHitBoxType)hitBoxIndex, hitX, hitY, width, height);
                }
            }

            if (hitboxes[0] == null) { hitboxes[0] = new BossHitBox(BossHitBoxType.Hit, 0, 0, 0, 0); }
            if (hitboxes[1] == null) { hitboxes[1] = new BossHitBox(BossHitBoxType.Weapon, 0, 0, 0, 0); }
            if (hitboxes[2] == null) { hitboxes[2] = new BossHitBox(BossHitBoxType.Guard, 0, 0, 0, 0); }

            return new BossSpriteFrame(frameIndex, BossFrameType.Sprite, name, baseAddress, frameLength, frameParts, hitboxes[0], hitboxes[1], hitboxes[2]);
        }

        private BossBackgroundFrame ReadBackgroundBossFrame(BossFamily family, int frameIndex)
        {
            string name = ManaMetadata.BossFrameAddressFactory.GetFrameName(family, frameIndex);
            int baseAddress = ManaMetadata.BossFrameAddressFactory.GetFrameAddress(family, frameIndex);
            this.Seek(baseAddress);

            // Number of parts for the frame. Each part is 6 bytes.
            ushort frameLength = (ushort)(this.ReadUInt16() & 0x00FF);

            // Some of my notes say these are X/Y coordinates. Not sure where I got that from.
            byte unknownHeader1 = (byte)(this.ReadUInt16() & 0x00FF);
            byte unknownHeader2 = (byte)(this.ReadUInt16() & 0x00FF);

            List<BossBackgroundFramePart> frameParts = new List<BossBackgroundFramePart>(frameLength);
            for (int i = 0; i < frameLength; i++)
            {
                sbyte xOffset = this.ReadSByte();   // X-Offset of the frame, relative to the origin of this background section.
                sbyte yOffset = this.ReadSByte();   // Y-Offset of the frame, relative to the origin of this background section.
                byte columnCount = this.Read();     // The number of 8x8 columns in the background section.
                byte rowCount = this.Read();        // The number of 8x8 rows in the background section.
                ushort pointer = this.ReadUInt16(); // Pointer to a list of ID/Flag tuples for the background sprite construction in Bank C1.

                // Data for background frames is always located in Bank C1.
                int tileLocation = Constants.Bank01Offset | pointer;
                int tileCount = columnCount * rowCount;
                int position = this.Seek(tileLocation);

                // 0 == Uncompressed. Just read the list of TileID/Flags.
                // 1 == Compressed. Data is a compressed stream of TileID/Flags.
                bool compressed = (this.Read() != 0);

                // X/Y Coordinates for background inner frame parts are ignored. The inner parts are drawn left to right, top to bottom.
                DataTable<BossSpriteFramePart> innerFramePartsList = this.ReadBackgroundBossFrameParts(compressed, tileLocation, tileCount);
                BossBackgroundFramePart framePart = new BossBackgroundFramePart(compressed, xOffset, yOffset, columnCount, rowCount, pointer, innerFramePartsList);

                frameParts.Add(framePart);
                this.Seek(position);
            }

            return new BossBackgroundFrame(frameIndex, BossFrameType.Background, name, baseAddress, frameLength, unknownHeader1, unknownHeader2, frameParts);
        }

        private DataTable<BossSpriteFramePart> ReadBackgroundBossFrameParts(bool compressed, int tileLocation, int tileCount)
        {
            if (compressed)
            {
                return this.ReadCompressedBackgroundBossFrameParts(tileCount);
            }
            return this.ReadUncompressedBackgroundBossFrameParts(tileLocation, tileCount);
        }

        private DataTable<BossSpriteFramePart> ReadCompressedBackgroundBossFrameParts(int tileCount)
        {
            int index = 0;
            byte[] frameBuffer = new byte[tileCount * 2];
            byte b0295 = 0; // 0295
            byte b0296 = 0; // 0296
            while (index < frameBuffer.Length)
            {
                byte control = this.Read();
                for (int i = 0; i < 8; i++)
                {
                    bool read = (control & 0x80) > 0;
                    control <<= 1;
                    byte current = read ? this.Read() : b0296;
                    frameBuffer[index] = current;
                    b0296 = b0295;
                    b0295 = current;
                    index++;

                    if (index >= frameBuffer.Length) { break; }
                }
            }

            List<BossSpriteFramePart> frameParts = new List<BossSpriteFramePart>();
            for (int i = 0; i < frameBuffer.Length; i += 2)
            {
                byte id = frameBuffer[i];
                byte flags = frameBuffer[i + 1];

                BossSpriteFramePart innerPart = new BossSpriteFramePart(0, 0, id, (BossFrameFlags)flags);
                frameParts.Add(innerPart);
            }
            return new DataTable<BossSpriteFramePart>(frameParts);
        }

        private DataTable<BossSpriteFramePart> ReadUncompressedBackgroundBossFrameParts(int tileLocation, int tileCount)
        {
            List<BossSpriteFramePart> frameParts = new List<BossSpriteFramePart>(tileCount);
            while (this.Position < tileLocation + (tileCount * 2))
            {
                byte id = this.Read();
                byte flags = this.Read();

                BossSpriteFramePart innerPart = new BossSpriteFramePart(0, 0, id, (BossFrameFlags)flags);
                frameParts.Add(innerPart);
            }
            return new DataTable<BossSpriteFramePart>(frameParts);
        }
        
        private IReadOnlyList<byte> ReadGraphics(BossGraphicsTableEntry row, BossTilesetMetadata metadata, LZ77Decompressor decompressor)
        {
            if (metadata.IsDualCompressed)
            {
                // Boss uses the heavy-weight LZ77 Decompressor. Running the decompressor will return stream of bytes still compressed in the light-weight format.
                return decompressor.Decompress(this.RomFile, row.FullGraphicsAddress);
            }
            else
            {
                // Boss doesn't use the heavy-weight LZ77 Decompressor so the light-weight compressed bytes can be loaded directly from the rom file.
                List<byte> decompressedData = new List<byte>(row.GraphicsSize);
                this.Seek(row.FullGraphicsAddress);
                for (int i = 0; i < row.GraphicsSize; i++)
                {
                    decompressedData.Add(this.Read());
                }
                return decompressedData;
            }
        }
    }
}