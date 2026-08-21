using System;
using System.Collections.Generic;
using ManaMagic.Core.Bosses.Animations;
using ManaMagic.Core.Bosses.BossInitialization;
using ManaMagic.Core.Bosses.Frames;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ManaMagic.Core.Metadata;
using ManaMagic.Core.RomReaders;
using ManaMagic.Core.Sprites;
using ZwellTech;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Core.Bosses
{
    public sealed class BossContext
    {
        private DataTable<byte> SkillGraphicDefinitionIndexTable = DataTable<byte>.Empty;
        private DataTable<ushort> SkillGraphicDefinitionPointerTable = DataTable<ushort>.Empty;
        private DataTable<DataTable<byte>> SkillGraphicDefinitionTable = DataTable<DataTable<byte>>.Empty;

        public DataTable<BossGraphicsTableEntry> GraphicsTable { get; private set; } = DataTable<BossGraphicsTableEntry>.Empty;
        public DataTable<SpritePalette> PaletteTable { get; private set; } = DataTable<SpritePalette>.Empty;
        public DataTable<ManaBossWeapon> WeaponTable { get; private set; } = DataTable<ManaBossWeapon>.Empty;
        public TilesetDictionary Tilesets { get; private set; } = TilesetDictionary.Empty;
        public DataTable<BossInitializationScript> BossInitializationScriptTable { get; private set; } = DataTable<BossInitializationScript>.Empty;
        public DataTable<BossPaletteScript> BossInitializationPaletteScriptTable { get; private set; } = DataTable<BossPaletteScript>.Empty;
        public DataTable<BossAnimationScript> AnimationScriptTable { get; private set; } = DataTable<BossAnimationScript>.Empty;
        public DataTable<ushort> StateMachineSetupPointerTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<BossSetupHeader> SetupHeaders { get; private set; } = DataTable<BossSetupHeader>.Empty;

        public TilesetDictionary SkillTilesets { get; private set; } = TilesetDictionary.Empty;
        public DataTable<ushort> SkillAIPointerTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<ushort> SkillPaletteTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<ushort> SkillBossAnimationIdTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<ushort> SkillPlayerAnimationIdTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<ushort> SkillBossSoundEffectIdTable { get; private set; } = DataTable<ushort>.Empty;
        public DataTable<ushort> SkillPlayerSoundEffectIdTable { get; private set; } = DataTable<ushort>.Empty;

        public IReadOnlyDictionary<BossFamily, BossFrameList> FrameDictionary { get; private set; } = new Dictionary<BossFamily, BossFrameList>();
        public IReadOnlyList<ManaBossSkill2> Skills { get; private set; } = Array.Empty<ManaBossSkill2>();
        public DataTable<ManaBoss> Bosses { get; private set; } = DataTable<ManaBoss>.Empty;

        public void Initialize(TextContext textContext, SpriteContext spriteContext, LZ77Decompressor lz77Decompressor)
        {
            BossRomReader reader = RomReaderFactory.GetRomReader<BossRomReader>();

            this.GraphicsTable = reader.ReadGraphicsTable();
            this.PaletteTable = reader.ReadPaletteTable();
            this.WeaponTable = reader.ReadWeaponTable();
            this.Tilesets = reader.ReadTilesetDictionary(this.GraphicsTable, lz77Decompressor);

            this.BossInitializationScriptTable = reader.ReadBossInitializationScriptTable();
            this.BossInitializationPaletteScriptTable = reader.ReadBossInitializationPaletteScriptTable();
            this.AnimationScriptTable = reader.ReadAnimationScriptTable();
            this.StateMachineSetupPointerTable = reader.ReadStateMachineSetupPointerTable();
            this.SetupHeaders = reader.ReadSetupHeaders(this.StateMachineSetupPointerTable);

            this.SkillGraphicDefinitionIndexTable = reader.ReadSkillGraphicDefinitionIndexTable();
            this.SkillGraphicDefinitionPointerTable = reader.ReadSkillGraphicDefinitionPointerTable();
            this.SkillGraphicDefinitionTable = reader.ReadSkillGraphicDefinitionTable(this.SkillGraphicDefinitionIndexTable);
            this.SkillTilesets = reader.ReadSkillTilesetDictionary(this.SkillGraphicDefinitionIndexTable, this.SkillGraphicDefinitionTable, this.SkillGraphicDefinitionPointerTable);

            this.SkillAIPointerTable = reader.ReadSkillAIPointerTable();
            this.SkillPaletteTable = reader.ReadSkillPaletteTable();
            this.SkillBossAnimationIdTable = reader.ReadSkillBossAnimationIdTable();
            this.SkillPlayerAnimationIdTable = reader.ReadSkillPlayerAnimationIdTable();
            this.SkillBossSoundEffectIdTable = reader.ReadSkillBossSoundEffectIdTable();
            this.SkillPlayerSoundEffectIdTable = reader.ReadSkillPlayerSoundEffectIdTable();

            this.FrameDictionary = reader.ReadFrameDictionary();

            this.CreateBossSkillTable(textContext);
            this.Bosses = this.CreateBossEnemyTable(textContext, spriteContext);
        }

        public BossTileset GetMergedBossTileset(byte tilesetIndex, bool useSecondaryAuxiliaryId = false)
        {
            BossTilesetMetadata metadata = ManaMetadata.BossTilesetMetadata[tilesetIndex];
            BossTileset tileset = (BossTileset)this.Tilesets[tilesetIndex];
            if (metadata.AuxiliaryIds.Primary != ManaMetadata.NullBossTilesetAuxiliaryId)
            {
                byte auxiliaryIndex = !useSecondaryAuxiliaryId ? metadata.AuxiliaryIds.Primary : metadata.AuxiliaryIds.Secondary;

                BossTileset tilesetCopy = (BossTileset)tileset.Clone();
                tilesetCopy.Merge(this.Tilesets[auxiliaryIndex]);
                tileset = tilesetCopy;
            }
            else if (tilesetIndex == 0x01)
            {
                // TODO: Not this
                tileset = (BossTileset)this.Tilesets[0x02];
                BossTileset tilesetCopy = (BossTileset)tileset.Clone();
                tilesetCopy.Merge(this.Tilesets[tilesetIndex]);
                tileset = new BossTileset(0x01, tilesetCopy.Tiles, ((BossTileset)this.Tilesets[tilesetIndex]).Row, metadata, ((BossTileset)this.Tilesets[tilesetIndex]).TileType);
            }
            return tileset;
        }

        private DataTable<ManaBoss> CreateBossEnemyTable(TextContext textContext, SpriteContext spriteContext)
        {
            List<ManaBoss> bossList = new List<ManaBoss>(0x100);
            for (int i = 0x57; i < 0x80; i++)
            {
                ManaEvent name = textContext.EnemyNameTable[i];
                EnemyStatEntry statistics = spriteContext.EnemyStatisticsTable[i];

                // All bosses have an entry in the loot table, however the entries are invalid as they don't drop loot.
                EnemyLootEntry loot = spriteContext.LootTable[i];

                // They also have a palette entry, however this is not used by the boss.
                // Palette indexes in the boss range are repurposed for other uses, like spell palettes and status effects.
                SpritePalette palette = spriteContext.PaletteTable[i];

                int baseIndex = i - 0x57;
                BossInitializationScript script = this.BossInitializationScriptTable[baseIndex];
                BossPaletteScript paletteScript = this.BossInitializationPaletteScriptTable[baseIndex];
                SpritePalette palette1 = paletteScript.Palette1Index != -1 ? this.PaletteTable[paletteScript.Palette1Index] : SpritePalette.Empty;
                SpritePalette palette2 = paletteScript.Palette2Index != -1 ? this.PaletteTable[paletteScript.Palette2Index] : SpritePalette.Empty;
                SpritePalette palette3 = paletteScript.Palette3Index != -1 ? this.PaletteTable[paletteScript.Palette3Index] : SpritePalette.Empty;

                SpritePalette bgPalette1 = paletteScript.BackgroundPaletteSlot1 != -1 ? this.PaletteTable[paletteScript.BackgroundPaletteSlot1] : SpritePalette.Empty;
                SpritePalette bgPalette2 = paletteScript.BackgroundPaletteSlot2 != -1 ? this.PaletteTable[paletteScript.BackgroundPaletteSlot2] : SpritePalette.Empty;
                SpritePalette mbPalette1 = paletteScript.ManaBeastSlot1 != -1 ? this.PaletteTable[paletteScript.ManaBeastSlot1] : SpritePalette.Empty;
                SpritePalette mbPalette2 = paletteScript.ManaBeastSlot2 != -1 ? this.PaletteTable[paletteScript.ManaBeastSlot2] : SpritePalette.Empty;

                BossSetupHeader header = BossSetupHeader.Empty;
                foreach (BossInitializationCommand opCode in script)
                {
                    if (opCode is LoadPaletteIntoSlot01BossInitializationCommand paletteCommand)
                    {
                        if (paletteCommand.Slot == BossPaletteSlot.Slot1) { palette1 = this.PaletteTable[paletteCommand.PaletteIndex]; }
                        else if (paletteCommand.Slot == BossPaletteSlot.Slot2) { palette2 = this.PaletteTable[paletteCommand.PaletteIndex]; }
                        else if (paletteCommand.Slot == BossPaletteSlot.Slot3) { palette3 = this.PaletteTable[paletteCommand.PaletteIndex]; }
                        else { ThrowHelper.ThrowInvalidOperationException("Unknown Slot ID"); }
                    }
                    else if (opCode is InitializeStandardStateMachineBossInitializationCommand standardInitCommand)
                    {
                        header = this.SetupHeaders[standardInitCommand.AIIndex];
                    }
                }

                ManaBoss boss = new ManaBoss((byte)i, name, statistics, loot, header, palette, palette1, palette2, palette3, bgPalette1, bgPalette2, mbPalette1, mbPalette2);
                boss.SetListeners(textContext.EnemyNameTable, spriteContext.EnemyStatisticsTable, spriteContext.LootTable, spriteContext.PaletteTable, this.PaletteTable);
                bossList.Add(boss);
            }
            return new DataTable<ManaBoss>(bossList);
        }

        private void CreateBossSkillTable(TextContext textContext)
        {
            List<ManaBossSkill2> skills = new List<ManaBossSkill2>();
            for (int i = 0; i < textContext.BossSkillNameMessageTable.RowCount; i++)
            {
                BossFamily family = ManaMetadata.BossSkillIndexToFamilyMapping[i];

                ushort aiPointer = this.SkillAIPointerTable[i];
                ushort paletteIndex = this.SkillPaletteTable[i];
                ushort bossAnimationIndex = this.SkillBossAnimationIdTable[i];
                ushort playerAnimationIndex = this.SkillPlayerAnimationIdTable[i];
                ushort bossSoundEffectIndex = this.SkillBossSoundEffectIdTable[i];
                ushort playerSoundEffectIndex = this.SkillPlayerSoundEffectIdTable[i];

                string name = GetNameFromEvent(textContext.BossSkillNameMessageTable[i]);
                Tileset tileset = this.SkillTilesets[(byte)i];
                BossFrameList frameList = this.FrameDictionary[family];
                SpritePalette palette = this.PaletteTable[paletteIndex];
                ManaBossWeapon defaultWeapon = this.WeaponTable[i];

                ManaBossSkill2 skill = new ManaBossSkill2(name, tileset, frameList, palette, defaultWeapon)
                {
                    AIPointer = aiPointer,
                    PaletteIndex = paletteIndex,
                    BossAnimationIndex = bossAnimationIndex,
                    PlayerAnimationIndex = playerAnimationIndex,
                    BossSoundEffectIndex = bossSoundEffectIndex,
                    PlayerSoundEffectIndex = playerSoundEffectIndex,
                };
                skills.Add(skill);
            }
            this.Skills = skills;
        }

        public static string GetNameFromEvent(ManaEvent manaEvent)
        {
            foreach (EventOpCode opCode in manaEvent.OpCodes)
            {
                if (opCode is ManaTextEventOpCode textOpCode)
                {
                    return textOpCode.ToString(false);
                }
            }
            return "N/A";
        }
    }
}