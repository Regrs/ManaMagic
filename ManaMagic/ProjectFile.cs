using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using ManaMagic.Core;
using ManaMagic.Core.Bosses;
using ManaMagic.Core.Character;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ManaMagic.Core.Items;
using ManaMagic.Core.Maps;
using ManaMagic.Core.RomWriters;
using ManaMagic.Core.Sprites;
using ManaMagic.Core.WorldMap;
using ZwellTech;
using ZwellTech.SuperNintendo;
using ZwellTech.SuperNintendo.Drawing;
using ZwellTech.Xml;

#nullable enable

namespace ManaMagic
{
    public sealed class ProjectFile
    {
#pragma warning disable CA1822 // Mark members as static
        private const int CurrentFileVersion = 1;

        private readonly XmlDocument loadDocument = new XmlDocument();

        public string FilePath { get; set; } = string.Empty;
        public string RomPath { get; set; } = string.Empty;
        public int FileVersion { get; private set; } = ProjectFile.CurrentFileVersion;

        public bool PreLoad()
        {
            if (File.Exists(this.FilePath))
            {
                this.loadDocument.Load(this.FilePath);

                XmlNode? rootNode = this.loadDocument.GetElementsByTagName(XmlConstants.ManaMagicElementName)[0];
                if (rootNode != null)
                {
                    if (XmlHelper.TryReadAttribute(rootNode, XmlConstants.VersionAttributeName, out string versionString))
                    {
                        this.FileVersion = Convert.ToInt32(versionString, CultureInfo.InvariantCulture);
                    }

                    XmlNode? romPathNode = this.loadDocument.GetElementsByTagName(XmlConstants.RomPathElementName)[0];
                    if (romPathNode != null && XmlHelper.TryReadAttribute(romPathNode, XmlConstants.PathAttributeName, out string path))
                    {
                        this.RomPath = path;
                    }
                }
            }
            return !string.IsNullOrWhiteSpace(this.RomPath);
        }

        public void Load(SecretOfManaContext context)
        {
            if (this.PreLoad())
            {
                this.LoadEvents(this.loadDocument, context.EventContext);
                this.LoadStringTables(this.loadDocument, context.TextContext);
                this.LoadCharacterTables(this.loadDocument, context.CharacterContext);
                this.LoadSpriteTables(this.loadDocument, context.SpriteContext);
                this.LoadBossTables(this.loadDocument, context.BossContext);
                this.LoadItemTables(this.loadDocument, context.ItemContext);
                this.LoadMapTables(this.loadDocument, context.MapContext);
                this.LoadWorldMapTables(this.loadDocument, context.WorldMapContext);
            }
        }

        public void Save(SecretOfManaContext context)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;

            XmlWriter writer = XmlWriter.Create(this.FilePath, settings);
            using (writer.CreateDocumentNode())
            {
                // Create the main 'ManaMagic' node for the project.
                using (writer.CreateElementtNode(XmlConstants.ManaMagicElementName))
                {
                    // The version of the project save file.
                    writer.WriteAttributeString(XmlConstants.VersionAttributeName, ProjectFile.CurrentFileVersion.ToString());

                    // The path of the ROM file.
                    using (writer.CreateElementtNode(XmlConstants.RomPathElementName))
                    {
                        writer.WriteAttributeString(XmlConstants.PathAttributeName, this.RomPath);
                    }

                    this.SaveEvents(context.EventContext, writer);
                    this.SaveStringTables(context.TextContext, writer);
                    this.SaveCharacterTables(context.CharacterContext, writer);
                    this.SaveSpriteTables(context.SpriteContext, writer);
                    this.SaveBossTables(context.BossContext, writer);
                    this.SaveItemTables(context.ItemContext, writer);
                    this.SaveMapTables(context.MapContext, writer);
                    this.SaveWorldMapTables(context.WorldMapContext, writer);
                }
            }

            writer.Flush();
            writer.Close();
        }

        public void Export(SecretOfManaContext context, string filePath)
        {
            //ManaValidator validator = new ManaValidator();
            //validator.Validate(context);
            //if (validator.HasErrors) { Debugger.Break(); }

            ManaRomWriter romWriter = new ManaRomWriter(context.RomFile!);
            romWriter.WriteRomFile(context);
            romWriter.SaveToFile(filePath);
        }

        private void LoadEvents(XmlDocument document, EventContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Events.EventsElementName)[0];
            if (rootNode != null)
            {
                IReadOnlyDictionary<EventOpCodeType, Type> typeDictionary = EventUtil.GetEventOpCodeDictionary();
                foreach (XmlNode eventNode in rootNode.ChildNodes)
                {
                    ManaEvent? manaEvent = ProjectFile.ReadEvent(eventNode, out ushort index);
                    if (manaEvent != null)
                    {
                        context.Events.Replace(index, manaEvent);
                    }
                }
            }
        }

        private void LoadStringTables(XmlDocument document, TextContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Events.Text.StringTablesElementName)[0];
            if (rootNode != null)
            {
                IReadOnlyDictionary<EventOpCodeType, Type> typeDictionary = EventUtil.GetEventOpCodeDictionary();
                foreach (XmlNode typeNode in rootNode.ChildNodes)
                {
                    DataTable<ManaEvent>? stringTable = GetStringTable(typeNode, context);
                    if (stringTable != null)
                    {
                        foreach (XmlNode eventNode in typeNode.ChildNodes)
                        {
                            ManaEvent? manaEvent = ProjectFile.ReadEvent(eventNode, out ushort index);
                            if (manaEvent != null)
                            {
                                stringTable.Replace(index, manaEvent);
                            }
                        }
                    }
                }
            }

            static DataTable<ManaEvent>? GetStringTable(XmlNode typeNode, TextContext context)
            {
                switch (typeNode.Name)
                {
                    case XmlConstants.Events.Text.WeaponNamesElementName: return context.WeaponNameTable;
                    case XmlConstants.Events.Text.EquipmentNamesElementName: return context.EquipmentNameTable;
                    case XmlConstants.Events.Text.ItemNamesElementName: return context.ItemNameTable;
                    case XmlConstants.Events.Text.EnemyNamesElementName: return context.EnemyNameTable;
                    case XmlConstants.Events.Text.WeaponDescriptionsElementName: return context.WeaponDescriptionTable;
                    case XmlConstants.Events.Text.TownNamesElementName: return context.TownNameTable;
                    case XmlConstants.Events.Text.ItemErrorsElementName: return context.ItemErrorMessageTable;
                    case XmlConstants.Events.Text.ShopTextElementName: return context.ShopMessageTable;
                    case XmlConstants.Events.Text.StatusEffectMessagesElementName: return context.StatusEffectMessageTable;
                    case XmlConstants.Events.Text.ElementalFearMessagesElementName: return context.ElementalFearMessageTable;
                    case XmlConstants.Events.Text.TrapMessagesElementName: return context.TrapMessageTable;
                    case XmlConstants.Events.Text.WeaponNameMessagesElementName: return context.WeaponNameMessageTable;
                    case XmlConstants.Events.Text.BossSkillNameMessagesElementName: return context.BossSkillNameMessageTable;
                    case XmlConstants.Events.Text.LunarMagicMessagesElementName: return context.LunarMagicMessageTable;
                    case XmlConstants.Events.Text.TreasureChestMessagesElementName: return context.TreasureChestMessageTable;
                    case XmlConstants.Events.Text.CombatMessagesElementName: return context.CombatMessageTable;
                    case XmlConstants.Events.Text.AnalyzerMessagesElementName: return context.AnalyzerMessageTable;
                    case XmlConstants.Events.Text.LevelUpMessagesElementName: return context.LevelUpMessageTable;
                }
                return null;
            }
        }

        private void LoadCharacterTables(XmlDocument document, CharacterContext context)
        {
            ProjectFile.LoadStatsPerLevel(document, context);
            ProjectFile.LoadExperiencePerLevelTable(document, context);
            ProjectFile.LoadDefaultCharacterDataTable(document, context);
        }

        private void LoadItemTables(XmlDocument document, ItemContext context)
        {
            ProjectFile.LoadWeaponDefinitionTableTable(document, context);
            ProjectFile.LoadEquipmentDefinitionTableTable(document, context);
            ProjectFile.LoadItemDefinitionTableTable(document, context);
            ProjectFile.LoadShopPriceTable(document, context);
            ProjectFile.LoadShopTable(document, context);
        }

        private void LoadSpriteTables(XmlDocument document, SpriteContext context)
        {
            ProjectFile.LoadEnemyStatisticsTable(document, context);
            ProjectFile.LoadLootTable(document, context);
            ProjectFile.LoadSpritePaletteTable(document, context);
        }

        private void LoadBossTables(XmlDocument document, BossContext context)
        {
            ProjectFile.LoadBossPaletteTable(document, context);
            ProjectFile.LoadBossWeaponTable(document, context);
            ProjectFile.LoadBossHeaderTable(document, context);
        }

        private void LoadMapTables(XmlDocument document, MapContext context)
        {
            ProjectFile.LoadMapDisplaySettingsTable(document, context);
            this.LoadMapHeaderTable(context);
            ProjectFile.LoadMapPaletteTable(document, context);
            ProjectFile.LoadFlammieFlightCoordinateTable(document, context);
        }

        private void LoadWorldMapTables(XmlDocument document, WorldMapContext context)
        {
            ProjectFile.LoadWorldMapLandingLocationTable(document, context);
            ProjectFile.LoadCannonTravelCoordinatesTable(document, context);
        }

        // --------------------------------------------------------------------------------------------------

        private void SaveEvents(EventContext context, XmlWriter writer)
        {
            // Create the 'Events' container node for all the modified events.
            using (writer.CreateElementtNode(XmlConstants.Events.EventsElementName))
            {
                for (int index = 0; index < context.Events.RowCount; index++)
                {
                    ManaEvent manaEvent = context.Events[index];
                    if (manaEvent.Dirty || manaEvent.UserModified)
                    {
                        ProjectFile.WriteEvent(index, manaEvent, writer);
                    }
                }
            }
        }

        private void SaveStringTables(TextContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Events.Text.StringTablesElementName))
            {
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.WeaponNamesElementName, context.WeaponNameTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.EquipmentNamesElementName, context.EquipmentNameTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.ItemNamesElementName, context.ItemNameTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.EnemyNamesElementName, context.EnemyNameTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.WeaponDescriptionsElementName, context.WeaponDescriptionTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.TownNamesElementName, context.TownNameTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.ItemErrorsElementName, context.ItemErrorMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.ShopTextElementName, context.ShopMessageTable, writer);

                ProjectFile.SaveStringTable(XmlConstants.Events.Text.StatusEffectMessagesElementName, context.StatusEffectMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.ElementalFearMessagesElementName, context.ElementalFearMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.TrapMessagesElementName, context.TrapMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.WeaponNameMessagesElementName, context.WeaponNameMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.BossSkillNameMessagesElementName, context.BossSkillNameMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.LunarMagicMessagesElementName, context.LunarMagicMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.TreasureChestMessagesElementName, context.TreasureChestMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.CombatMessagesElementName, context.CombatMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.AnalyzerMessagesElementName, context.AnalyzerMessageTable, writer);
                ProjectFile.SaveStringTable(XmlConstants.Events.Text.LevelUpMessagesElementName, context.LevelUpMessageTable, writer);
            }
        }

        private void SaveCharacterTables(CharacterContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Characters.CharacterDataElementName))
            {
                ProjectFile.SaveStatsByLevelTable(XmlConstants.Characters.RandiStatsByLevelElementName, context.RandiStatsByLevelTable, writer);
                ProjectFile.SaveStatsByLevelTable(XmlConstants.Characters.PurimStatsByLevelElementName, context.PurimStatsByLevelTable, writer);
                ProjectFile.SaveStatsByLevelTable(XmlConstants.Characters.PopoieStatsByLevelElementName, context.PopoieStatsByLevelTable, writer);

                ProjectFile.SaveExperiencePerLevelTable(context, writer);
                ProjectFile.SaveDefaultCharacterDataTable(context, writer);
            }
        }

        private void SaveItemTables(ItemContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Items.ItemsElementName))
            {
                ProjectFile.SaveWeaponDefinitionTable(context, writer);
                ProjectFile.SaveEquipmentDefinitionTable(context, writer);
                ProjectFile.SaveItemDefinitionTable(context, writer);
                using (writer.CreateElementtNode(XmlConstants.Items.Shops.ShopsElementName))
                {
                    ProjectFile.SaveShopPriceTable(context, writer);
                    ProjectFile.SaveShopTable(context, writer);
                }
            }
        }

        private void SaveSpriteTables(SpriteContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Sprites.SpritesElementName))
            {
                ProjectFile.SaveEnemyStatisticsTable(context, writer);
                ProjectFile.SaveLootTable(context, writer);
                ProjectFile.SaveSpritePaletteTable(context, writer);
            }
        }

        private void SaveBossTables(BossContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Bosses.BossesElementName))
            {
                ProjectFile.SaveBossPaletteTable(context, writer);
                ProjectFile.SaveBossWeaponTable(context, writer);
                ProjectFile.SaveBossHeaderTable(context, writer);
            }
        }

        private void SaveMapTables(MapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Maps.MapsElementName))
            {
                ProjectFile.SaveMapDisplaySettingsTable(context, writer);
                ProjectFile.SaveMapHeaderTable(context, writer);
                ProjectFile.SaveMapPaletteTable(context, writer);
                ProjectFile.SaveFlammieFlightCoordinateTable(context, writer);
            }
        }

        private void SaveWorldMapTables(WorldMapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.WorldMap.WorldMapElementName))
            {
                ProjectFile.SaveWorldMapLandingLocationTable(context, writer);
                ProjectFile.SaveCannonTravelCoordinatesTable(context, writer);
            }
        }

        // --------------------------------------------------------------------------------------------------

        #region Private Static Methods: Load

        private static void LoadStatsPerLevel(XmlDocument document, CharacterContext context)
        {
            XmlNode? randiNode = document.GetElementsByTagName(XmlConstants.Characters.RandiStatsByLevelElementName)[0];
            XmlNode? purimNode = document.GetElementsByTagName(XmlConstants.Characters.PurimStatsByLevelElementName)[0];
            XmlNode? popoieNode = document.GetElementsByTagName(XmlConstants.Characters.PopoieStatsByLevelElementName)[0];

            LoadStats(randiNode, context.RandiStatsByLevelTable);
            LoadStats(purimNode, context.PurimStatsByLevelTable);
            LoadStats(popoieNode, context.PopoieStatsByLevelTable);

            static void LoadStats(XmlNode? rootNode, DataTable<CharacterStatsByLevel> characterStatsByLevels)
            {
                if (rootNode != null)
                {
                    foreach (XmlNode node in rootNode.ChildNodes)
                    {
                        if (node.Name == XmlConstants.Characters.StatByLevelElementName)
                        {
                            byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                            ushort hitPoints = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Characters.HitPointsAttributeName), CultureInfo.InvariantCulture);
                            byte manaPoints = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Characters.ManaPointsAttributeName), CultureInfo.InvariantCulture);
                            byte strength = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Characters.StrengthAttributeName), CultureInfo.InvariantCulture);
                            byte agility = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Characters.AgilityAttributeName), CultureInfo.InvariantCulture);
                            byte constitution = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Characters.ConstitutionAttributeName), CultureInfo.InvariantCulture);
                            byte intelligence = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Characters.IntelligenceAttributeName), CultureInfo.InvariantCulture);
                            byte wisdom = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Characters.WisdomAttributeName), CultureInfo.InvariantCulture);

                            CharacterStatsByLevel statsByLevel = new CharacterStatsByLevel(index, hitPoints, manaPoints, strength, agility, constitution, intelligence, wisdom, true);
                            characterStatsByLevels.Replace(index, statsByLevel);
                        }
                    }
                }
            }
        }

        private static void LoadExperiencePerLevelTable(XmlDocument document, CharacterContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Characters.ExperiencePerLevelsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Characters.ExperiencePerLevelElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        uint experience = Convert.ToUInt32(XmlHelper.ReadAttribute(node, XmlConstants.Characters.ExperienceAttributeName), CultureInfo.InvariantCulture);

                        ExperiencePerLevel experienceByLevel = new ExperiencePerLevel(index, experience, true);
                        context.ExperiencePerLevelTable.Replace(index, experienceByLevel);
                    }
                }
            }
        }

        private static void LoadDefaultCharacterDataTable(XmlDocument document, CharacterContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Characters.Default.DefaultPartyDataElementName)[0];
            if (rootNode != null)
            {
                byte index = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                ushort gold = Convert.ToUInt16(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.GoldAttributeName), CultureInfo.InvariantCulture);
                byte sealedManaSeeds = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.SealedManaSeedsAttributeName), CultureInfo.InvariantCulture);
                byte saveLocation = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.SaveLocationAttributeName), CultureInfo.InvariantCulture);

                byte randiAGQ = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.RandiAGQAttributeName), CultureInfo.InvariantCulture);
                byte purimAGQ = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PurimAGQAttributeName), CultureInfo.InvariantCulture);
                byte popoieAGQ = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PopoieAGQAttributeName), CultureInfo.InvariantCulture);

                byte randiAGL = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.RandiAGLAttributeName), CultureInfo.InvariantCulture);
                byte purimAGL = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PurimAGLAttributeName), CultureInfo.InvariantCulture);
                byte popoieAGL = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PopoieAGLAttributeName), CultureInfo.InvariantCulture);

                byte randiHelmet = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.RandiHelmetAttributeName), CultureInfo.InvariantCulture);
                byte randiArmor = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.RandiArmorAttributeName), CultureInfo.InvariantCulture);
                byte randiAccessory = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.RandiAccessoryAttributeName), CultureInfo.InvariantCulture);
                byte randiWeapon = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.RandiWeaponAttributeName), CultureInfo.InvariantCulture);

                byte purimHelmet = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PurimHelmetAttributeName), CultureInfo.InvariantCulture);
                byte purimArmor = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PurimArmorAttributeName), CultureInfo.InvariantCulture);
                byte purimAccessory = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PurimAccessoryAttributeName), CultureInfo.InvariantCulture);
                byte purimWeapon = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PurimWeaponAttributeName), CultureInfo.InvariantCulture);

                byte popoieHelmet = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PopoieHelmetAttributeName), CultureInfo.InvariantCulture);
                byte popoieArmor = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PopoieArmorAttributeName), CultureInfo.InvariantCulture);
                byte popoieAccessory = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PopoieAccessoryAttributeName), CultureInfo.InvariantCulture);
                byte popoieWeapon = Convert.ToByte(XmlHelper.ReadAttribute(rootNode, XmlConstants.Characters.Default.PopoieWeaponAttributeName), CultureInfo.InvariantCulture);

                ManaDefaultCharacterData defaultCharacterData = new ManaDefaultCharacterData(index,
                                                                                             gold,
                                                                                             sealedManaSeeds,
                                                                                             saveLocation,
                                                                                             randiAGQ,
                                                                                             purimAGQ,
                                                                                             popoieAGQ,
                                                                                             randiAGL,
                                                                                             purimAGL,
                                                                                             popoieAGL,
                                                                                             randiHelmet,
                                                                                             randiArmor,
                                                                                             randiAccessory,
                                                                                             randiWeapon,
                                                                                             purimHelmet,
                                                                                             purimArmor,
                                                                                             purimAccessory,
                                                                                             purimWeapon,
                                                                                             popoieHelmet,
                                                                                             popoieArmor,
                                                                                             popoieAccessory,
                                                                                             popoieWeapon,
                                                                                             true);
                context.SetDefaultCharacterData(defaultCharacterData);
            }
        }

        private static void LoadEnemyStatisticsTable(XmlDocument document, SpriteContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Sprites.Enemies.EnemyStatisticsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Sprites.Enemies.EnemyStatEntryElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte level = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.LevelAttributeName), CultureInfo.InvariantCulture);
                        ushort hitPoints = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.HitPointsAttributeName), CultureInfo.InvariantCulture);
                        byte manaPoints = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.ManaPointsAttributeName), CultureInfo.InvariantCulture);
                        byte strength = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.StrengthAttributeName), CultureInfo.InvariantCulture);
                        byte agility = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.AgilityAttributeName), CultureInfo.InvariantCulture);
                        byte intelligence = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.IntelligenceAttributeName), CultureInfo.InvariantCulture);
                        byte wisdom = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.WisdomAttributeName), CultureInfo.InvariantCulture);
                        byte evasion = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.EvasionAttributeName), CultureInfo.InvariantCulture);
                        ushort defense = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.DefenseAttributeName), CultureInfo.InvariantCulture);
                        byte magicEvasion = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.MagicEvasionAttributeName), CultureInfo.InvariantCulture);
                        ushort magicDefense = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.MagicDefenseAttributeName), CultureInfo.InvariantCulture);
                        byte monsterType = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.MonsterTypeAttributeName), CultureInfo.InvariantCulture);
                        byte element = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.ElementAttributeName), CultureInfo.InvariantCulture);
                        ushort expAward = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.ExperienceAwardAttributeName), CultureInfo.InvariantCulture);
                        byte blackMagicPower = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.BlackMagicPowerAttributeName), CultureInfo.InvariantCulture);
                        byte whiteMagicPower = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.WhiteMagicPowerAttributeName), CultureInfo.InvariantCulture);
                        ushort immunities = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.ImmunitiesAttributeName), CultureInfo.InvariantCulture);
                        byte meleeWeapon = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.MeleeWeaponAttributeName), CultureInfo.InvariantCulture);
                        byte rangedWeapon = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.RangedWeaponAttributeName), CultureInfo.InvariantCulture);
                        byte deathStyle = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.DeathStyleAttributeName), CultureInfo.InvariantCulture);
                        byte wmLevel = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.WeaponMagicLevelAttributeName), CultureInfo.InvariantCulture);
                        ushort goldAward = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Enemies.GoldAwardAttributeName), CultureInfo.InvariantCulture);

                        EnemyStatEntry entry = new EnemyStatEntry(index,
                                                                  level,
                                                                  hitPoints,
                                                                  manaPoints,
                                                                  strength,
                                                                  agility,
                                                                  intelligence,
                                                                  wisdom,
                                                                  evasion,
                                                                  defense,
                                                                  magicEvasion,
                                                                  magicDefense,
                                                                  monsterType,
                                                                  element,
                                                                  expAward,
                                                                  blackMagicPower,
                                                                  whiteMagicPower,
                                                                  immunities,
                                                                  0,
                                                                  meleeWeapon,
                                                                  rangedWeapon,
                                                                  deathStyle,
                                                                  wmLevel,
                                                                  goldAward,
                                                                  true);
                        context.EnemyStatisticsTable.Replace(index, entry);
                    }
                }
            }
        }

        private static void LoadLootTable(XmlDocument document, SpriteContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Sprites.LootTable.LootTableElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Sprites.LootTable.LootEntryElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte dropRate = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.LootTable.DropRateAttributeName), CultureInfo.InvariantCulture);
                        byte trapInfo = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.LootTable.TrapInfoAttributeName), CultureInfo.InvariantCulture);
                        byte rareDropChance = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.LootTable.RareDropChanceAttributeName), CultureInfo.InvariantCulture);
                        byte commonDrop = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.LootTable.CommonDropAttributeName), CultureInfo.InvariantCulture);
                        byte rareDrop = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.LootTable.RareDropAttributeName), CultureInfo.InvariantCulture);

                        EnemyLootEntry entry = new EnemyLootEntry(index, dropRate, trapInfo, rareDropChance, commonDrop, rareDrop, true);
                        context.LootTable.Replace(index, entry);
                    }
                }
            }
        }

        private static void LoadSpritePaletteTable(XmlDocument document, SpriteContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Sprites.Palette.PalettesElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Sprites.Palette.PaletteElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);

                        List<Rgb555Color> colorList = new List<Rgb555Color>((int)Constants.Bank08.NumberOfColorsPerSpritePalette + 1);
                        //colorList.Add(Rgb555Color.White);
                        for (int colorIndex = 0; colorIndex < Constants.Bank08.NumberOfColorsPerSpritePalette + 1; colorIndex++)
                        {
                            ushort color = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Palette.CreatePaletteAttributeName(colorIndex)), CultureInfo.InvariantCulture);
                            colorList.Add(Rgb555Color.FromRgb(color));
                        }

                        SpritePalette palette = new SpritePalette(index, colorList, true);
                        context.PaletteTable.Replace(index, palette);
                    }
                }
            }
        }

        private static void LoadBossPaletteTable(XmlDocument document, BossContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Bosses.Palette.BossPalettesElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Sprites.Palette.PaletteElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        int address = Convert.ToInt32(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Palette.AddressAttributeName), CultureInfo.InvariantCulture);

                        List<Rgb555Color> colorList = new List<Rgb555Color>((int)Constants.Bank02.BossPaletteColorCount);
                        for (int colorIndex = 0; colorIndex < Constants.Bank02.BossPaletteColorCount; colorIndex++)
                        {
                            ushort color = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Sprites.Palette.CreatePaletteAttributeName(colorIndex)), CultureInfo.InvariantCulture);
                            colorList.Add(Rgb555Color.FromRgb(color));
                        }

                        SpritePalette palette = new SpritePalette(index, colorList, address, true);
                        context.PaletteTable.Replace(index, palette);
                    }
                }
            }
        }

        private static void LoadBossWeaponTable(XmlDocument document, BossContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Bosses.Weapon.BossWeaponsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Bosses.Weapon.BossWeaponElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        MonsterType affinity = (MonsterType)Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Weapon.AffinityAttributeName), CultureInfo.InvariantCulture);
                        ElementalType element = (ElementalType)Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Weapon.ElementAttributeName), CultureInfo.InvariantCulture);
                        byte accuracy = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Weapon.AccuracyAttributeName), CultureInfo.InvariantCulture);
                        byte power = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Weapon.PowerAttributeName), CultureInfo.InvariantCulture);
                        StatusEffects statusEffects = (StatusEffects)Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Weapon.StatusEffectsAttributeName), CultureInfo.InvariantCulture);
                        byte inflictionRate = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.Weapon.InflictionRateAttributeName), CultureInfo.InvariantCulture);

                        ManaBossWeapon weapon = new ManaBossWeapon(index, affinity, element, accuracy, power, statusEffects, inflictionRate, true);
                        context.WeaponTable.Replace(index, weapon);
                    }
                }
            }
        }

        private static void LoadBossHeaderTable(XmlDocument document, BossContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Bosses.BossHeadersElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Bosses.BossHeaderElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        ushort controlFlags = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Bosses.ControlFlagsAttributeName), CultureInfo.InvariantCulture);

                        BossSetupHeader original = context.SetupHeaders[index];
                        BossSetupHeader header = new BossSetupHeader(index,
                                                                     controlFlags,
                                                                     original.SpriteFlags,
                                                                     original.InitializeRoutinePointer,
                                                                     original.MovementRoutinePointer,
                                                                     original.AttackRoutinePointer,
                                                                     original.DamagedRoutinePointer,
                                                                     original.DefaultCommandScriptPointer,
                                                                     original.DeathCommandScriptPointer,
                                                                     original.AICommandSetPointer,
                                                                     original.ShadowSize,
                                                                     original.PostDeathHandlerPointer,
                                                                     original.UnknownValue,
                                                                     original.SpecialDeathHandlerPointer,
                                                                     true);
                        context.SetupHeaders.Replace(index, header);
                    }
                }
            }
        }

        private static void LoadWeaponDefinitionTableTable(XmlDocument document, ItemContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Items.WeaponDefinition.WeaponsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Items.WeaponDefinition.WeaponElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte weaponClass = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.WeaponClassAttributeName), CultureInfo.InvariantCulture);
                        ushort statModifiers = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.StatModifiersAttributeName), CultureInfo.InvariantCulture);
                        byte projectileType = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.ProjectileTypeAttributeName), CultureInfo.InvariantCulture);
                        byte paletteIndex = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.PaletteIndexAttributeName), CultureInfo.InvariantCulture);
                        byte affinity = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.AffinityAttributeName), CultureInfo.InvariantCulture);
                        byte criticalChance = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.CriticalChanceAttributeName), CultureInfo.InvariantCulture);
                        byte accuracy = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.AccuracyAttributeName), CultureInfo.InvariantCulture);
                        byte power = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.PowerAttributeName), CultureInfo.InvariantCulture);
                        ushort statusEffects = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.StatusEffectsAttributeName), CultureInfo.InvariantCulture);
                        byte inflictionRate = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.WeaponDefinition.InflictionRateAttributeName), CultureInfo.InvariantCulture);

                        ManaWeaponDefinition manaWeaponDefinition = new ManaWeaponDefinition(index, weaponClass, statModifiers, projectileType, paletteIndex, affinity, criticalChance, accuracy, power, statusEffects, inflictionRate, true);
                        context.WeaponDefinitionTable.Replace(index, manaWeaponDefinition);
                    }
                }
            }
        }

        private static void LoadEquipmentDefinitionTableTable(XmlDocument document, ItemContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Items.EquipmentDefinition.EquipmentsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Items.EquipmentDefinition.EquipmentElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte statModifiers = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.StatModifiersAttributeName), CultureInfo.InvariantCulture);
                        byte defense = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.DefenseAttributeName), CultureInfo.InvariantCulture);
                        byte evasion = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.EvasionAttributeName), CultureInfo.InvariantCulture);
                        byte magicDefense = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.MagicDefenseAttributeName), CultureInfo.InvariantCulture);
                        byte magicEvasion = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.MagicEvasionAttributeName), CultureInfo.InvariantCulture);
                        byte equippableBy = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.EquippableByAttributeName), CultureInfo.InvariantCulture);
                        byte element = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.ElementAttributeName), CultureInfo.InvariantCulture);
                        ushort resistances = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.ResistancesAttributeName), CultureInfo.InvariantCulture);
                        byte unknown = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.EquipmentDefinition.UnknownAttributeName), CultureInfo.InvariantCulture);

                        ManaEquipmentDefinition manaEquipmentDefinition = new ManaEquipmentDefinition(index, statModifiers, defense, evasion, magicDefense, magicEvasion, equippableBy, element, resistances, unknown, true);
                        context.EquipmentDefinitionTable.Replace(index, manaEquipmentDefinition);
                    }
                }
            }
        }

        private static void LoadItemDefinitionTableTable(XmlDocument document, ItemContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Items.ItemDefinition.ConsumablesElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Items.ItemDefinition.ConsumableElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte amountHealed = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.ItemDefinition.AmountHealedAttributeName), CultureInfo.InvariantCulture);
                        byte paletteIndex = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.ItemDefinition.PaletteIndexAttributeName), CultureInfo.InvariantCulture);

                        ManaItemDefinition original = context.ItemDefinitionTable[index];
                        ManaItemDefinition manaItemDefinition = new ManaItemDefinition(index,
                                                                                       original.Unused00,
                                                                                       original.Unused01,
                                                                                       amountHealed,
                                                                                       original.Unused03,
                                                                                       original.Unused04,
                                                                                       original.Unused05,
                                                                                       original.Unused06,
                                                                                       paletteIndex,
                                                                                       original.ProbablyItemGraphicsPointer,
                                                                                       original.ProbablyPlayerGraphicsPointer,
                                                                                       original.AnimationPointer,
                                                                                       original.Unused0E,
                                                                                       original.Unused0F,
                                                                                       true);
                        context.ItemDefinitionTable.Replace(index, manaItemDefinition);
                    }
                }
            }
        }

        private static void LoadShopPriceTable(XmlDocument document, ItemContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Items.Shops.PriceTablesElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode priceTableNode in rootNode.ChildNodes)
                {
                    if (priceTableNode.Name == XmlConstants.Items.Shops.PriceTableElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(priceTableNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        DataTable<UShortValue> table = GetTable(context, index);
                        foreach (XmlNode node in priceTableNode.ChildNodes)
                        {
                            if (node.Name == XmlConstants.Items.Shops.ItemElementName)
                            {
                                byte priceIndex = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                                ushort value = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Items.Shops.PriceAttributeName), CultureInfo.InvariantCulture);
                                UShortValue price = new UShortValue(priceIndex, value, true);
                                table.Replace(priceIndex, price);
                            }
                        }
                    }
                }
            }

            static DataTable<UShortValue> GetTable(ItemContext context, int index)
            {
                switch (index)
                {
                    case 0: return context.ConsumableItemPriceTable;
                    case 1: return context.HelmetPriceTable;
                    case 2: return context.ArmorPriceTable;
                    case 3: return context.AccessoryPriceTable;
                    case 4: return context.WeaponUpgradePriceTable;
                    default: return (DataTable<UShortValue>)ThrowHelper.ThrowArgumentException("Invalid table index", nameof(index));
                }
            }
        }

        private static void LoadShopTable(XmlDocument document, ItemContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Items.Shops.ShopInventoriesElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode shopInventoryNode in rootNode.ChildNodes)
                {
                    if (shopInventoryNode.Name == XmlConstants.Items.Shops.ShopInventoryElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(shopInventoryNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        List<ShopItem> items = new List<ShopItem>(shopInventoryNode.ChildNodes.Count);
                        foreach (XmlNode node in shopInventoryNode.ChildNodes)
                        {
                            if (node.Name == XmlConstants.Items.Shops.ItemElementName)
                            {
                                byte itemIndex = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                                ShopItem item = (ShopItem)Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Items.Shops.ShopItemAttributeName), CultureInfo.InvariantCulture);
                                items.Add(item);
                            }
                        }
                        ManaShop shop = new ManaShop(index, items, true);
                        context.ShopTable.Replace(index, shop);
                    }
                }
            }
        }

        private static void LoadMapDisplaySettingsTable(XmlDocument document, MapContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Maps.DisplaySettings.DisplaySettingsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Maps.DisplaySettings.DisplaySettingElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte mosaicSettings = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Maps.DisplaySettings.MosaicSettingsAttributeName), CultureInfo.InvariantCulture);
                        byte mainScreen = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Maps.DisplaySettings.MainScreenLayersEnabledAttributeName), CultureInfo.InvariantCulture);
                        byte subScreen = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Maps.DisplaySettings.SubScreenLayersEnabledAttributeName), CultureInfo.InvariantCulture);
                        byte colorMath = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.Maps.DisplaySettings.ColorMathAttributeName), CultureInfo.InvariantCulture);
                        ushort bgColor = Convert.ToUInt16(XmlHelper.ReadAttribute(node, XmlConstants.Maps.DisplaySettings.BackgroundColorAttributeName), CultureInfo.InvariantCulture);

                        MapDisplaySettings settings = new MapDisplaySettings(index, mosaicSettings, mainScreen, subScreen, colorMath, 0, 0, 0, Rgb555Color.FromRgb(bgColor), true);
                        context.DisplaySettingsTable.Replace(index, settings);
                    }
                }
            }
        }

        private static void LoadFlammieFlightCoordinateTable(XmlDocument document, MapContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Maps.Flammie.FlammieFlightCoordinatesSetsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.Maps.Flammie.FlammieFlightCoordinateSetElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte xCoord = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.XCoordinateAttributeName), CultureInfo.InvariantCulture);
                        byte yCoord = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.YCoordinateAttributeName), CultureInfo.InvariantCulture);

                        ManaPoint8 point = new ManaPoint8(index, xCoord, yCoord, true);
                        context.FlammieFlightCoordinateTable.Replace(index, point);
                    }
                }
            }
        }

        private static void LoadMapPaletteTable(XmlDocument document, MapContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.Maps.Palette.MapPaletteSetsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode paletteSetNode in rootNode.ChildNodes)
                {
                    if (paletteSetNode.Name == XmlConstants.Maps.Palette.MapPaletteSetElementName)
                    {
                        byte setIndex = Convert.ToByte(XmlHelper.ReadAttribute(paletteSetNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        foreach (XmlNode paletteNode in paletteSetNode.ChildNodes)
                        {
                            if (paletteNode.Name == XmlConstants.Maps.Palette.PaletteElementName)
                            {
                                byte paletteIndex = Convert.ToByte(XmlHelper.ReadAttribute(paletteNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);

                                List<Rgb555Color> colorList = new List<Rgb555Color>((int)Constants.Bank0C.MapColorsPerPaletteInSet + 1);
                                //colorList.Add(Rgb555Color.White);
                                for (int colorIndex = 0; colorIndex < Constants.Bank0C.MapColorsPerPaletteInSet + 1; colorIndex++)
                                {
                                    ushort color = Convert.ToUInt16(XmlHelper.ReadAttribute(paletteNode, XmlConstants.Sprites.Palette.CreatePaletteAttributeName(colorIndex)), CultureInfo.InvariantCulture);
                                    colorList.Add(Rgb555Color.FromRgb(color));
                                }
                                SpritePalette palette = new SpritePalette(paletteIndex, colorList, true);
                                context.PaletteSetTable[setIndex].Replace(paletteIndex, palette);
                            }
                        }
                    }
                }
            }
        }

        private void LoadMapHeaderTable(MapContext context)
        {
            XmlNode? rootNode = this.loadDocument.GetElementsByTagName(XmlConstants.Maps.Header.MapHeadersElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode headerNode in rootNode.ChildNodes)
                {
                    if (headerNode.Name == XmlConstants.Maps.Header.MapHeaderElementName)
                    {
                        byte headerIndex = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte tileset8Index = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.Tileset8x8IndexAttributeName), CultureInfo.InvariantCulture);
                        byte paletteSetIndex = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.PaletteSetIndexAttributeName), CultureInfo.InvariantCulture);
                        byte tileset16Index = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.Tileset16x16IndexAttributeName), CultureInfo.InvariantCulture);
                        byte eventOptions = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.EventOptionsAttributeName), CultureInfo.InvariantCulture);
                        byte specialItemOptions = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.SpecialItemsOptionsAttributeName), CultureInfo.InvariantCulture);
                        byte displaySettings = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.DisplaySettingsIndexAttributeName), CultureInfo.InvariantCulture);
                        byte unused = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.UnusedAttributeName), CultureInfo.InvariantCulture);
                        byte npcPaletteSetIndex = Convert.ToByte(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.NpcPaletteIndexAttributeName), CultureInfo.InvariantCulture);
                        bool isValid = Convert.ToBoolean(XmlHelper.ReadAttribute(headerNode, XmlConstants.Maps.Header.IsValidAttributeName), CultureInfo.InvariantCulture);

                        List<MapSpriteObject> objects = new List<MapSpriteObject>(headerNode.ChildNodes.Count);
                        foreach (XmlNode spriteContainerNode in headerNode.ChildNodes)
                        {
                            if (spriteContainerNode.Name == XmlConstants.Maps.Header.SpriteObject.MapSpriteObjectsElementName)
                            {
                                foreach (XmlNode spriteNode in spriteContainerNode.ChildNodes)
                                {
                                    if (spriteNode.Name == XmlConstants.Maps.Header.SpriteObject.MapSpriteObjectElementName)
                                    {
                                        byte objectIndex = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                                        byte eventFlag = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.EventFlagAttributeName), CultureInfo.InvariantCulture);
                                        byte eventFlagRange = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.EventFlagRangeAttributeName), CultureInfo.InvariantCulture);
                                        byte xCoordinate = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.XCoordinateAttributeName), CultureInfo.InvariantCulture);
                                        byte yCoordinate = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.YCoordinateAttributeName), CultureInfo.InvariantCulture);
                                        byte direction = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.DirectionAttributeName), CultureInfo.InvariantCulture);
                                        byte spriteIndex = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.SpriteIndexAttributeName), CultureInfo.InvariantCulture);
                                        byte eventIDLow = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.EventIdLowAttributeName), CultureInfo.InvariantCulture);
                                        byte eventIDHigh = Convert.ToByte(XmlHelper.ReadAttribute(spriteNode, XmlConstants.Maps.Header.SpriteObject.EventIdHighAttributeName), CultureInfo.InvariantCulture);

                                        MapSpriteObject spriteObject = new MapSpriteObject(objectIndex,
                                                                                           eventFlag,
                                                                                           eventFlagRange,
                                                                                           xCoordinate,
                                                                                           yCoordinate,
                                                                                           direction,
                                                                                           spriteIndex,
                                                                                           eventIDLow,
                                                                                           eventIDHigh,
                                                                                           true);
                                        objects.Add(spriteObject);
                                    }
                                }

                            }
                        }


                        MapHeader original = context.MapHeaderTable[headerIndex];
                        MapHeader mapHeader = new MapHeader(headerIndex,
                                                            tileset8Index,
                                                            paletteSetIndex,
                                                            tileset16Index,
                                                            eventOptions,
                                                            specialItemOptions,
                                                            displaySettings,
                                                            unused,
                                                            npcPaletteSetIndex,
                                                            original.ObjectTable,
                                                            isValid,
                                                            true);

                        foreach (MapSpriteObject spriteObject in objects)
                        {
                            mapHeader.ObjectTable.Replace(spriteObject.Index, spriteObject);
                        }

                        context.MapHeaderTable.Replace(headerIndex, mapHeader);
                    }
                }
            }
        }

        private static void LoadWorldMapLandingLocationTable(XmlDocument document, WorldMapContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.WorldMap.LandingLocationsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.WorldMap.LandingLocationElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte mapIndex = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.MapIndexAttributeName), CultureInfo.InvariantCulture);
                        byte xCoord = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.XCoordinateAttributeName), CultureInfo.InvariantCulture);
                        byte yCoord = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.YCoordinateAttributeName), CultureInfo.InvariantCulture);

                        WorldMapLandingLocation landingLocation = new WorldMapLandingLocation(index, mapIndex, xCoord, yCoord, true);
                        context.WorldMapLandingLocationTable.Replace(index, landingLocation);
                    }
                }
            }
        }

        private static void LoadCannonTravelCoordinatesTable(XmlDocument document, WorldMapContext context)
        {
            XmlNode? rootNode = document.GetElementsByTagName(XmlConstants.WorldMap.CannonTravelCoordinateSetsElementName)[0];
            if (rootNode != null)
            {
                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node.Name == XmlConstants.WorldMap.CannonTravelCoordinateSetElementName)
                    {
                        byte index = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                        byte startX = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.StartXCoordinateAttributeName), CultureInfo.InvariantCulture);
                        byte startY = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.StartYCoordinateAttributeName), CultureInfo.InvariantCulture);
                        byte endX = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.EndXCoordinateAttributeName), CultureInfo.InvariantCulture);
                        byte endY = Convert.ToByte(XmlHelper.ReadAttribute(node, XmlConstants.WorldMap.EndYCoordinateAttributeName), CultureInfo.InvariantCulture);

                        CannonTravelCoordinateSet coordinateSet = new CannonTravelCoordinateSet(index, startX, startY, endX, endY, true);
                        context.CannonTravelCoordinatesTable.Replace(index, coordinateSet);
                    }
                }
            }
        }

        #endregion

        #region Private Static Methods: Save

        private static void SaveWeaponDefinitionTable(ItemContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Items.WeaponDefinition.WeaponsElementName))
            {
                foreach (ManaWeaponDefinition weaponDefinition in context.WeaponDefinitionTable)
                {
                    if (weaponDefinition.Dirty || weaponDefinition.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Items.WeaponDefinition.WeaponElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, weaponDefinition.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.WeaponClassAttributeName, Convert.ToUInt32(weaponDefinition.WeaponClass).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.StatModifiersAttributeName, weaponDefinition.StatModifiers.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.ProjectileTypeAttributeName, Convert.ToUInt32(weaponDefinition.ProjectileType).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.PaletteIndexAttributeName, weaponDefinition.PaletteIndex.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.AffinityAttributeName, Convert.ToUInt32(weaponDefinition.Affinity).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.CriticalChanceAttributeName, weaponDefinition.CriticalChance.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.AccuracyAttributeName, weaponDefinition.Accuracy.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.PowerAttributeName, weaponDefinition.Power.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.StatusEffectsAttributeName, Convert.ToUInt32(weaponDefinition.StatusEffects).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.WeaponDefinition.InflictionRateAttributeName, weaponDefinition.InflictionRate.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveEquipmentDefinitionTable(ItemContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Items.EquipmentDefinition.EquipmentsElementName))
            {
                foreach (ManaEquipmentDefinition equipmentDefinition in context.EquipmentDefinitionTable)
                {
                    if (equipmentDefinition.Dirty || equipmentDefinition.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Items.EquipmentDefinition.EquipmentElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, equipmentDefinition.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.StatModifiersAttributeName, Convert.ToUInt32(equipmentDefinition.StatModifiers).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.DefenseAttributeName, equipmentDefinition.Defense.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.EvasionAttributeName, equipmentDefinition.Evasion.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.MagicDefenseAttributeName, equipmentDefinition.MagicDefense.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.MagicEvasionAttributeName, equipmentDefinition.MagicEvasion.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.EquippableByAttributeName, Convert.ToUInt32(equipmentDefinition.EquippableBy).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.ElementAttributeName, Convert.ToUInt32(equipmentDefinition.Element).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.ResistancesAttributeName, Convert.ToUInt32(equipmentDefinition.Resistances).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.EquipmentDefinition.UnknownAttributeName, equipmentDefinition.Unknown.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveItemDefinitionTable(ItemContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Items.ItemDefinition.ConsumablesElementName))
            {
                foreach (ManaItemDefinition itemDefinition in context.ItemDefinitionTable)
                {
                    if (itemDefinition.Dirty || itemDefinition.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Items.ItemDefinition.ConsumableElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, itemDefinition.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Items.ItemDefinition.AmountHealedAttributeName, Convert.ToUInt32(itemDefinition.AmountHealed).ToString());
                            writer.WriteAttributeString(XmlConstants.Items.ItemDefinition.PaletteIndexAttributeName, itemDefinition.PaletteIndex.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveShopPriceTable(ItemContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Items.Shops.PriceTablesElementName))
            {
                SaveTable(context.ConsumableItemPriceTable, 0, writer);
                SaveTable(context.HelmetPriceTable, 1, writer);
                SaveTable(context.ArmorPriceTable, 2, writer);
                SaveTable(context.AccessoryPriceTable, 3, writer);
                SaveTable(context.WeaponUpgradePriceTable, 4, writer);
            }

            static void SaveTable(DataTable<UShortValue> dataTable, int tableIndex, XmlWriter writer)
            {
                bool dirty = dataTable.Any(p => p.Dirty || p.UserModified);
                if (dirty)
                {
                    using (writer.CreateElementtNode(XmlConstants.Items.Shops.PriceTableElementName))
                    {
                        writer.WriteAttributeString(XmlHelper.IndexAttributeName, tableIndex.ToString());
                        foreach (UShortValue value in dataTable)
                        {
                            if (value.Dirty || value.UserModified)
                            {
                                using (writer.CreateElementtNode(XmlConstants.Items.Shops.ItemElementName))
                                {
                                    writer.WriteAttributeString(XmlHelper.IndexAttributeName, value.Index.ToString());
                                    writer.WriteAttributeString(XmlConstants.Items.Shops.PriceAttributeName, value.Value.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SaveShopTable(ItemContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Items.Shops.ShopInventoriesElementName))
            {
                bool dirty = context.ShopTable.Any(p => p.Dirty || p.UserModified);
                if (dirty)
                {
                    foreach (ManaShop shop in context.ShopTable)
                    {
                        if (shop.Dirty || shop.UserModified)
                        {
                            using (writer.CreateElementtNode(XmlConstants.Items.Shops.ShopInventoryElementName))
                            {
                                writer.WriteAttributeString(XmlHelper.IndexAttributeName, shop.Index.ToString());
                                for (int i = 0; i < shop.Items.Count; i++)
                                {
                                    ShopItem item = shop[i];
                                    using (writer.CreateElementtNode(XmlConstants.Items.Shops.ItemElementName))
                                    {
                                        writer.WriteAttributeString(XmlHelper.IndexAttributeName, i.ToString());
                                        writer.WriteAttributeString(XmlConstants.Items.Shops.ShopItemAttributeName, ((uint)item).ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SaveStatsByLevelTable(string elementName, DataTable<CharacterStatsByLevel> statsByLevelTable, XmlWriter writer)
        {
            using (writer.CreateElementtNode(elementName))
            {
                foreach (CharacterStatsByLevel statsByLevel in statsByLevelTable)
                {
                    if (statsByLevel.Dirty || statsByLevel.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Characters.StatByLevelElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, statsByLevel.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.HitPointsAttributeName, statsByLevel.HitPoints.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.ManaPointsAttributeName, statsByLevel.ManaPoints.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.StrengthAttributeName, statsByLevel.Strength.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.AgilityAttributeName, statsByLevel.Agility.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.ConstitutionAttributeName, statsByLevel.Constitution.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.IntelligenceAttributeName, statsByLevel.Intelligence.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.WisdomAttributeName, statsByLevel.Wisdom.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveExperiencePerLevelTable(CharacterContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Characters.ExperiencePerLevelsElementName))
            {
                foreach (ExperiencePerLevel experienceByLevel in context.ExperiencePerLevelTable)
                {
                    if (experienceByLevel.Dirty || experienceByLevel.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Characters.ExperiencePerLevelElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, experienceByLevel.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Characters.ExperienceAttributeName, experienceByLevel.Experience.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveDefaultCharacterDataTable(CharacterContext context, XmlWriter writer)
        {
            if (context.DefaultCharacterDataTable.Dirty || context.DefaultCharacterDataTable.UserModified)
            {
                using (writer.CreateElementtNode(XmlConstants.Characters.Default.DefaultPartyDataElementName))
                {
                    writer.WriteAttributeString(XmlHelper.IndexAttributeName, context.DefaultCharacterDataTable.Index.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.GoldAttributeName, context.DefaultCharacterDataTable.Gold.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.SealedManaSeedsAttributeName, context.DefaultCharacterDataTable.SealedManaSeeds.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.SaveLocationAttributeName, context.DefaultCharacterDataTable.SaveLocation.ToString());

                    writer.WriteAttributeString(XmlConstants.Characters.Default.RandiAGQAttributeName, context.DefaultCharacterDataTable.RandiActionGridQuadrant.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PurimAGQAttributeName, context.DefaultCharacterDataTable.PurimActionGridQuadrant.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PopoieAGQAttributeName, context.DefaultCharacterDataTable.PopoieActionGridQuadrant.ToString());

                    writer.WriteAttributeString(XmlConstants.Characters.Default.RandiAGLAttributeName, context.DefaultCharacterDataTable.RandiActionGridLocation.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PurimAGLAttributeName, context.DefaultCharacterDataTable.PurimActionGridLocation.ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PopoieAGLAttributeName, context.DefaultCharacterDataTable.PopoieActionGridLocation.ToString());

                    writer.WriteAttributeString(XmlConstants.Characters.Default.RandiHelmetAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.RandiHelmet).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.RandiArmorAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.RandiArmor).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.RandiAccessoryAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.RandiAccessory).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.RandiWeaponAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.RandiWeapon).ToString());

                    writer.WriteAttributeString(XmlConstants.Characters.Default.PurimHelmetAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PurimHelmet).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PurimArmorAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PurimArmor).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PurimAccessoryAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PurimAccessory).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PurimWeaponAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PurimWeapon).ToString());

                    writer.WriteAttributeString(XmlConstants.Characters.Default.PopoieHelmetAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PopoieHelmet).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PopoieArmorAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PopoieArmor).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PopoieAccessoryAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PopoieAccessory).ToString());
                    writer.WriteAttributeString(XmlConstants.Characters.Default.PopoieWeaponAttributeName, Convert.ToByte(context.DefaultCharacterDataTable.PopoieWeapon).ToString());
                }
            }
        }

        private static void SaveStringTable(string elementName, DataTable<ManaEvent> stringTable, XmlWriter writer)
        {
            using (writer.CreateElementtNode(elementName))
            {
                for (int index = 0; index < stringTable.RowCount; index++)
                {
                    ManaEvent manaEvent = stringTable[index];
                    if (manaEvent.Dirty || manaEvent.UserModified)
                    {
                        ProjectFile.WriteEvent(index, manaEvent, writer);
                    }
                }
            }
        }

        private static void SaveEnemyStatisticsTable(SpriteContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Sprites.Enemies.EnemyStatisticsElementName))
            {
                foreach (EnemyStatEntry enemyStatEntry in context.EnemyStatisticsTable)
                {
                    if (enemyStatEntry.Dirty || enemyStatEntry.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Sprites.Enemies.EnemyStatEntryElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, enemyStatEntry.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.LevelAttributeName, enemyStatEntry.Level.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.HitPointsAttributeName, enemyStatEntry.HitPoints.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.ManaPointsAttributeName, enemyStatEntry.ManaPoints.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.StrengthAttributeName, enemyStatEntry.Strength.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.AgilityAttributeName, enemyStatEntry.Agility.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.IntelligenceAttributeName, enemyStatEntry.Intelligence.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.WisdomAttributeName, enemyStatEntry.Wisdom.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.EvasionAttributeName, enemyStatEntry.Evasion.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.DefenseAttributeName, enemyStatEntry.Defense.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.MagicEvasionAttributeName, enemyStatEntry.MagicEvasion.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.MagicDefenseAttributeName, enemyStatEntry.MagicDefense.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.MonsterTypeAttributeName, Convert.ToUInt32(enemyStatEntry.MonsterType).ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.ElementAttributeName, Convert.ToUInt32(enemyStatEntry.Element).ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.ExperienceAwardAttributeName, enemyStatEntry.ExperienceAward.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.BlackMagicPowerAttributeName, enemyStatEntry.BlackMagicPower.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.WhiteMagicPowerAttributeName, enemyStatEntry.WhiteMagicPower.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.ImmunitiesAttributeName, Convert.ToUInt32(enemyStatEntry.Immunities).ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.MeleeWeaponAttributeName, enemyStatEntry.MeleeWeapon.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.RangedWeaponAttributeName, enemyStatEntry.RangedWeapon.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.DeathStyleAttributeName, enemyStatEntry.DeathStyleAsByte.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.WeaponMagicLevelAttributeName, enemyStatEntry.WeaponMagicLevel.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.Enemies.GoldAwardAttributeName, enemyStatEntry.GoldAward.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveLootTable(SpriteContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Sprites.LootTable.LootTableElementName))
            {
                Span<byte> lootTableValues = stackalloc byte[5];
                foreach (EnemyLootEntry entry in context.LootTable)
                {
                    if (entry.Dirty || entry.UserModified)
                    {
                        entry.GetEncodedValues(lootTableValues);
                        using (writer.CreateElementtNode(XmlConstants.Sprites.LootTable.LootEntryElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, entry.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.LootTable.DropRateAttributeName, lootTableValues[0].ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.LootTable.TrapInfoAttributeName, lootTableValues[1].ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.LootTable.RareDropChanceAttributeName, lootTableValues[2].ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.LootTable.CommonDropAttributeName, lootTableValues[3].ToString());
                            writer.WriteAttributeString(XmlConstants.Sprites.LootTable.RareDropAttributeName, lootTableValues[4].ToString());
                        }
                    }
                }
            }
        }

        private static void SaveSpritePaletteTable(SpriteContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Sprites.Palette.PalettesElementName))
            {
                foreach (SpritePalette palette in context.PaletteTable)
                {
                    if (palette.Dirty || palette.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Sprites.Palette.PaletteElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, palette.Index.ToString());
                            for (int i = 0; i < palette.Colors.Count; i++)
                            {
                                Rgb555Color color = palette.Colors[i];
                                writer.WriteAttributeString(XmlConstants.Sprites.Palette.CreatePaletteAttributeName(i), color.ToRgb555().ToString());
                            }
                        }
                    }
                }
            }
        }

        private static void SaveBossPaletteTable(BossContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Bosses.Palette.BossPalettesElementName))
            {
                foreach (SpritePalette palette in context.PaletteTable)
                {
                    if (palette.Dirty || palette.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Sprites.Palette.PaletteElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, palette.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Palette.AddressAttributeName, palette.Address.ToString());
                            for (int i = 0; i < palette.Colors.Count; i++)
                            {
                                Rgb555Color color = palette.Colors[i];
                                writer.WriteAttributeString(XmlConstants.Sprites.Palette.CreatePaletteAttributeName(i), color.ToRgb555().ToString());
                            }
                        }
                    }
                }
            }
        }

        private static void SaveBossWeaponTable(BossContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Bosses.Weapon.BossWeaponsElementName))
            {
                foreach (ManaBossWeapon weapon in context.WeaponTable)
                {
                    if (weapon.Dirty || weapon.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Bosses.Weapon.BossWeaponElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, weapon.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Weapon.AffinityAttributeName, Convert.ToUInt32(weapon.MonsterAffinity).ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Weapon.ElementAttributeName, Convert.ToUInt32(weapon.Element).ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Weapon.AccuracyAttributeName, weapon.Accuracy.ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Weapon.PowerAttributeName, weapon.Power.ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Weapon.StatusEffectsAttributeName, Convert.ToUInt32(weapon.StatusEffects).ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.Weapon.InflictionRateAttributeName, weapon.InflictionRate.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveBossHeaderTable(BossContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Bosses.BossHeadersElementName))
            {
                foreach (BossSetupHeader header in context.SetupHeaders)
                {
                    if (header.Dirty || header.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Bosses.BossHeaderElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, header.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Bosses.ControlFlagsAttributeName, Convert.ToUInt32(header.ControlFlags).ToString());
                        }
                    }
                }
            }
        }

        private static void SaveMapDisplaySettingsTable(MapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Maps.DisplaySettings.DisplaySettingsElementName))
            {
                foreach (MapDisplaySettings settings in context.DisplaySettingsTable)
                {
                    if (settings.Dirty || settings.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Maps.DisplaySettings.DisplaySettingElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, settings.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.DisplaySettings.MosaicSettingsAttributeName, settings.MosaicSettings.ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.DisplaySettings.MainScreenLayersEnabledAttributeName, Convert.ToUInt32(settings.MainScreenLayersEnabled).ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.DisplaySettings.SubScreenLayersEnabledAttributeName, Convert.ToUInt32(settings.SubScreenLayersEnabled).ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.DisplaySettings.ColorMathAttributeName, Convert.ToUInt32(settings.ColorMath).ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.DisplaySettings.BackgroundColorAttributeName, settings.BackgroundColor.ToRgb555().ToString());
                        }
                    }
                }
            }
        }

        private static void SaveMapPaletteTable(MapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Maps.Palette.MapPaletteSetsElementName))
            {
                for (int setIndex = 0; setIndex < context.PaletteSetTable.RowCount; setIndex++)
                {
                    DataTable<SpritePalette> paletteTable = context.PaletteSetTable[setIndex];
                    bool dirty = paletteTable.Any(p => p.Dirty || p.UserModified);
                    if (dirty)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Maps.Palette.MapPaletteSetElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, setIndex.ToString());
                            for (int paletteIndex = 0; paletteIndex < paletteTable.RowCount; paletteIndex++)
                            {
                                SpritePalette palette = paletteTable[paletteIndex];
                                if (palette.Dirty || palette.UserModified)
                                {
                                    using (writer.CreateElementtNode(XmlConstants.Maps.Palette.PaletteElementName))
                                    {
                                        writer.WriteAttributeString(XmlHelper.IndexAttributeName, paletteIndex.ToString());
                                        for (int colorIndex = 0; colorIndex < palette.NumberOfColors; colorIndex++)
                                        {
                                            Rgb555Color color = palette[colorIndex];
                                            writer.WriteAttributeString(XmlConstants.Sprites.Palette.CreatePaletteAttributeName(colorIndex), color.ToRgb555().ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SaveMapHeaderTable(MapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Maps.Header.MapHeadersElementName))
            {
                Span<byte> headerValues = stackalloc byte[8];
                Span<byte> spriteValues = stackalloc byte[8];
                foreach (MapHeader header in context.MapHeaderTable)
                {
                    if (header.Dirty || header.UserModified)
                    {
                        header.GetEncodedValues(headerValues);
                        using (writer.CreateElementtNode(XmlConstants.Maps.Header.MapHeaderElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, header.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.Tileset8x8IndexAttributeName, headerValues[0].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.PaletteSetIndexAttributeName, headerValues[1].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.Tileset16x16IndexAttributeName, headerValues[2].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.EventOptionsAttributeName, headerValues[3].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpecialItemsOptionsAttributeName, headerValues[4].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.DisplaySettingsIndexAttributeName, headerValues[5].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.UnusedAttributeName, headerValues[6].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.NpcPaletteIndexAttributeName, headerValues[7].ToString());
                            writer.WriteAttributeString(XmlConstants.Maps.Header.IsValidAttributeName, header.IsValid.ToString());

                            using (writer.CreateElementtNode(XmlConstants.Maps.Header.SpriteObject.MapSpriteObjectsElementName))
                            {
                                foreach (MapSpriteObject spriteObject in header.ObjectTable)
                                {
                                    if (spriteObject.Dirty || spriteObject.UserModified)
                                    {
                                        spriteObject.GetEncodedValues(spriteValues);
                                        using (writer.CreateElementtNode(XmlConstants.Maps.Header.SpriteObject.MapSpriteObjectElementName))
                                        {
                                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, spriteObject.Index.ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.EventFlagAttributeName, spriteValues[0].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.EventFlagRangeAttributeName, spriteValues[1].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.XCoordinateAttributeName, spriteValues[2].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.YCoordinateAttributeName, spriteValues[3].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.DirectionAttributeName, spriteValues[4].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.SpriteIndexAttributeName, spriteValues[5].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.EventIdLowAttributeName, spriteValues[6].ToString());
                                            writer.WriteAttributeString(XmlConstants.Maps.Header.SpriteObject.EventIdHighAttributeName, spriteValues[7].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SaveFlammieFlightCoordinateTable(MapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.Maps.Flammie.FlammieFlightCoordinatesSetsElementName))
            {
                foreach (ManaPoint8 point in context.FlammieFlightCoordinateTable)
                {
                    if (point.Dirty || point.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.Maps.Flammie.FlammieFlightCoordinateSetElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, point.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.XCoordinateAttributeName, point.X.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.YCoordinateAttributeName, point.Y.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveWorldMapLandingLocationTable(WorldMapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.WorldMap.LandingLocationsElementName))
            {
                foreach (WorldMapLandingLocation landingLocation in context.WorldMapLandingLocationTable)
                {
                    if (landingLocation.Dirty || landingLocation.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.WorldMap.LandingLocationElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, landingLocation.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.MapIndexAttributeName, landingLocation.MapIndex.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.XCoordinateAttributeName, landingLocation.XCoordinate.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.YCoordinateAttributeName, landingLocation.YCoordinate.ToString());
                        }
                    }
                }
            }
        }

        private static void SaveCannonTravelCoordinatesTable(WorldMapContext context, XmlWriter writer)
        {
            using (writer.CreateElementtNode(XmlConstants.WorldMap.CannonTravelCoordinateSetsElementName))
            {
                foreach (CannonTravelCoordinateSet coordinateSet in context.CannonTravelCoordinatesTable)
                {
                    if (coordinateSet.Dirty || coordinateSet.UserModified)
                    {
                        using (writer.CreateElementtNode(XmlConstants.WorldMap.CannonTravelCoordinateSetElementName))
                        {
                            writer.WriteAttributeString(XmlHelper.IndexAttributeName, coordinateSet.Index.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.StartXCoordinateAttributeName, coordinateSet.StartXCoordinate.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.StartYCoordinateAttributeName, coordinateSet.StartYCoordinate.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.EndXCoordinateAttributeName, coordinateSet.EndXCoordinate.ToString());
                            writer.WriteAttributeString(XmlConstants.WorldMap.EndYCoordinateAttributeName, coordinateSet.EndYCoordinate.ToString());
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Static Methods: Util

        private static ManaEvent? ReadEvent(XmlNode eventNode, out ushort index)
        {
            ManaEvent? manaEvent = null;
            index = 0;
            if (eventNode.Name == XmlConstants.Events.EventElementName)
            {
                index = Convert.ToUInt16(XmlHelper.ReadAttribute(eventNode, XmlHelper.IndexAttributeName), CultureInfo.InvariantCulture);
                List<EventOpCode> opCodes = new List<EventOpCode>(eventNode.ChildNodes.Count);
                foreach (XmlNode operationCode in eventNode.ChildNodes)
                {
                    if (operationCode.Name == XmlConstants.Events.OperationCodeElementName)
                    {
                        string opCodeString = XmlHelper.ReadAttribute(operationCode, XmlHelper.ValueAttributeName);
                        EventOpCodeType opcode = (EventOpCodeType)Enum.Parse(typeof(EventOpCodeType), opCodeString);

                        // Handling for text.
                        if (opcode == EventOpCodeType.TextStart || opcode == EventOpCodeType.ASCIITextStart)
                        {
                            string parameter = XmlHelper.ReadAttribute(operationCode, XmlConstants.Events.Parameter1AttributeName);
                            string[] stringBytes = parameter.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                            List<byte> textData = new List<byte>(stringBytes.Length);
                            foreach (string str in stringBytes)
                            {
                                textData.Add(Convert.ToByte(str));
                            }
                            opCodes.Add(EventOpCodeFactory.CreateText(textData, opcode == EventOpCodeType.ASCIITextStart));
                        }
                        else // All other op-codes.
                        {
                            bool usesParameter1 = XmlHelper.TryReadAttribute(operationCode, XmlConstants.Events.Parameter1AttributeName, out string parameter1);
                            bool usesParameter2 = XmlHelper.TryReadAttribute(operationCode, XmlConstants.Events.Parameter2AttributeName, out string parameter2);
                            bool usesParameter3 = XmlHelper.TryReadAttribute(operationCode, XmlConstants.Events.Parameter3AttributeName, out string parameter3);
                            bool usesParameter4 = XmlHelper.TryReadAttribute(operationCode, XmlConstants.Events.Parameter4AttributeName, out string parameter4);

                            ushort param1 = usesParameter1 ? Convert.ToUInt16(parameter1) : (ushort)0;
                            ushort param2 = usesParameter2 ? Convert.ToUInt16(parameter2) : (ushort)0;
                            ushort param3 = usesParameter3 ? Convert.ToUInt16(parameter3) : (ushort)0;
                            ushort param4 = usesParameter4 ? Convert.ToUInt16(parameter4) : (ushort)0;

                            opCodes.Add(EventOpCodeFactory.Create(opcode, param1, param2, param3, param4));
                        }
                    }
                }
                manaEvent = new ManaEvent(opCodes, true);
            }
            return manaEvent;
        }

        private static void WriteEvent(int index, ManaEvent manaEvent, XmlWriter writer)
        {
            // Create the 'Event' node for the modified event.
            using (writer.CreateElementtNode(XmlConstants.Events.EventElementName))
            {
                writer.WriteAttributeString(XmlHelper.IndexAttributeName, index.ToString());
                foreach (EventOpCode opCode in manaEvent)
                {
                    // Create a 'OperationCode' node for each op-code in the event.
                    using (writer.CreateElementtNode(XmlConstants.Events.OperationCodeElementName))
                    {
                        writer.WriteAttributeString(XmlHelper.ValueAttributeName, opCode.OperationCode.ToString());
                        if (opCode is TextDataEventOpCode textOpCode)
                        {
                            // For text, write out all the bytes as a comma delimited list.
                            writer.WriteAttributeString(XmlConstants.Events.Parameter1AttributeName, string.Join(',', textOpCode.TextData));
                        }
                        else if (opCode.OperationCode == EventOpCodeType.EndOptionDialogSetup)
                        {
                            writer.WriteAttributeString(XmlConstants.Events.Parameter1AttributeName, opCode.Parameters.Parameter1.ToString());
                        }
                        else
                        {
                            ProjectFile.WriteParameterArray(opCode.Parameters, writer);
                        }
                    }
                }
            }
        }

        private static void WriteParameterArray(ParameterArray parameterArray, XmlWriter writer)
        {
            if (parameterArray.ParameterInfo.UsesParameter1)
            {
                writer.WriteAttributeString(XmlConstants.Events.Parameter1AttributeName, parameterArray.Parameter1.ToString());
            }
            if (parameterArray.ParameterInfo.UsesParameter2)
            {
                writer.WriteAttributeString(XmlConstants.Events.Parameter2AttributeName, parameterArray.Parameter2.ToString());
            }
            if (parameterArray.ParameterInfo.UsesParameter3)
            {
                writer.WriteAttributeString(XmlConstants.Events.Parameter3AttributeName, parameterArray.Parameter3.ToString());
            }
            if (parameterArray.ParameterInfo.UsesParameter4)
            {
                writer.WriteAttributeString(XmlConstants.Events.Parameter4AttributeName, parameterArray.Parameter4.ToString());
            }
        }

        #endregion
#pragma warning restore CA1822 // Mark members as static
    }

    internal static class XmlConstants
    {
        public const string ManaMagicElementName = "ManaMagic";
        public const string VersionAttributeName = "version";

        public const string RomPathElementName = "RomPath";
        public const string PathAttributeName = "path";

        public static class Events
        {
            public const string EventsElementName = "Events";
            public const string EventElementName = "Event";
            public const string OperationCodeElementName = "OperationCode";
            public const string Parameter1AttributeName = "parameter1";
            public const string Parameter2AttributeName = "parameter2";
            public const string Parameter3AttributeName = "parameter3";
            public const string Parameter4AttributeName = "parameter4";

            public static class Text
            {
                public const string StringTablesElementName = "StringTables";
                public const string WeaponNamesElementName = "WeaponNames";
                public const string EquipmentNamesElementName = "EquipmentNames";
                public const string ItemNamesElementName = "ItemNames";
                public const string EnemyNamesElementName = "EnemyNames";
                public const string WeaponDescriptionsElementName = "WeaponDescriptions";
                public const string TownNamesElementName = "TownNames";
                public const string ItemErrorsElementName = "ItemErrors";
                public const string ShopTextElementName = "ShopText";

                public const string StatusEffectMessagesElementName = "StatusEffectMessages";
                public const string ElementalFearMessagesElementName = "ElementalFearMessages";
                public const string TrapMessagesElementName = "TrapMessagesMessages";
                public const string WeaponNameMessagesElementName = "WeaponNameMessages";
                public const string BossSkillNameMessagesElementName = "BossSkillNameMessages";
                public const string LunarMagicMessagesElementName = "LunarMagicMessages";
                public const string TreasureChestMessagesElementName = "TreasureChestMessages";
                public const string CombatMessagesElementName = "CombatMessages";
                public const string AnalyzerMessagesElementName = "AnalyzerMessages";
                public const string LevelUpMessagesElementName = "LevelUpMessages";
            }
        }

        public static class Characters
        {
            public const string CharacterDataElementName = "CharacterData";
            public const string RandiStatsByLevelElementName = "RandiStatsByLevel";
            public const string PurimStatsByLevelElementName = "PurimStatsByLevel";
            public const string PopoieStatsByLevelElementName = "PopoieStatsByLevel";
            public const string ExperiencePerLevelsElementName = "ExperiencePerLevels";

            public const string StatByLevelElementName = "StatByLevel";
            public const string HitPointsAttributeName = "hitPoints";
            public const string ManaPointsAttributeName = "manaPoints";
            public const string StrengthAttributeName = "strength";
            public const string AgilityAttributeName = "agility";
            public const string ConstitutionAttributeName = "constitution";
            public const string IntelligenceAttributeName = "intelligence";
            public const string WisdomAttributeName = "wisdom";

            public const string ExperiencePerLevelElementName = "ExperiencePerLevel";
            public const string ExperienceAttributeName = "experience";

            public static class Default
            {
                public const string DefaultPartyDataElementName = "DefaultPartyData";

                public const string GoldAttributeName = "gold";
                public const string SealedManaSeedsAttributeName = "sealedManaSeeds";
                public const string SaveLocationAttributeName = "saveLocation";

                public const string RandiAGQAttributeName = "randiAGQ";
                public const string PurimAGQAttributeName = "purimAGQ";
                public const string PopoieAGQAttributeName = "popoieAGQ";

                public const string RandiAGLAttributeName = "randiAGL";
                public const string PurimAGLAttributeName = "purimAGL";
                public const string PopoieAGLAttributeName = "popoieAGL";

                public const string RandiHelmetAttributeName = "randiHelmet";
                public const string RandiArmorAttributeName = "randiArmor";
                public const string RandiAccessoryAttributeName = "randiAccessory";
                public const string RandiWeaponAttributeName = "randiWeapon";

                public const string PurimHelmetAttributeName = "purimHelmet";
                public const string PurimArmorAttributeName = "purimArmor";
                public const string PurimAccessoryAttributeName = "purimAccessory";
                public const string PurimWeaponAttributeName = "purimWeapon";

                public const string PopoieHelmetAttributeName = "popoieHelmet";
                public const string PopoieArmorAttributeName = "popoieArmor";
                public const string PopoieAccessoryAttributeName = "popoieAccessory";
                public const string PopoieWeaponAttributeName = "popoieWeapon";
            }
        }

        public static class Items
        {
            public const string ItemsElementName = "Items";

            public static class WeaponDefinition
            {
                public const string WeaponsElementName = "Weapons";
                public const string WeaponElementName = "Weapon";

                public const string WeaponClassAttributeName = "weaponClass";
                public const string StatModifiersAttributeName = "statModifiers";
                public const string ProjectileTypeAttributeName = "projectileType";
                public const string PaletteIndexAttributeName = "paletteIndex";
                public const string AffinityAttributeName = "affinity";
                public const string CriticalChanceAttributeName = "criticalChance";
                public const string AccuracyAttributeName = "accuracy";
                public const string PowerAttributeName = "power";
                public const string StatusEffectsAttributeName = "statusEffects";
                public const string InflictionRateAttributeName = "inflictionRate";
            }

            public static class EquipmentDefinition
            {
                public const string EquipmentsElementName = "Equipments";
                public const string EquipmentElementName = "Equipment";

                public const string StatModifiersAttributeName = "statModifiers";
                public const string DefenseAttributeName = "defense";
                public const string EvasionAttributeName = "evasion";
                public const string MagicDefenseAttributeName = "magicDefense";
                public const string MagicEvasionAttributeName = "magicEvasion";
                public const string EquippableByAttributeName = "equippableBy";
                public const string ElementAttributeName = "element";
                public const string ResistancesAttributeName = "resistances";
                public const string UnknownAttributeName = "unknown";
            }

            public static class ItemDefinition
            {
                public const string ConsumablesElementName = "Consumables";
                public const string ConsumableElementName = "Consumable";

                public const string AmountHealedAttributeName = "amountHealed";
                public const string PaletteIndexAttributeName = "paletteIndex";
            }

            public static class Shops
            {
                public const string ShopsElementName = "Shops";
                public const string PriceTablesElementName = "PriceTables";
                public const string PriceTableElementName = "PriceTable";
                public const string ShopInventoriesElementName = "ShopInventories";
                public const string ShopInventoryElementName = "ShopInventory";
                public const string ItemElementName = "Item";

                public const string PriceAttributeName = "price";
                public const string ShopItemAttributeName = "shopItem";
            }
        }

        public static class Sprites
        {
            public const string SpritesElementName = "Sprites";

            public static class Enemies
            {
                public const string EnemyStatisticsElementName = "EnemyStatistics";
                public const string EnemyStatEntryElementName = "EnemyStatEntry";

                public const string LevelAttributeName = "level";
                public const string HitPointsAttributeName = "hitPoints";
                public const string ManaPointsAttributeName = "manaPoints";
                public const string StrengthAttributeName = "strength";
                public const string AgilityAttributeName = "agility";
                public const string IntelligenceAttributeName = "intelligence";
                public const string WisdomAttributeName = "wisdom";
                public const string EvasionAttributeName = "evasion";
                public const string DefenseAttributeName = "defense";
                public const string MagicEvasionAttributeName = "magicEvasion";
                public const string MagicDefenseAttributeName = "magicDefense";
                public const string MonsterTypeAttributeName = "monsterType";
                public const string ElementAttributeName = "element";
                public const string ExperienceAwardAttributeName = "experienceAward";
                public const string BlackMagicPowerAttributeName = "blackMagicPower";
                public const string WhiteMagicPowerAttributeName = "whiteMagicPower";
                public const string ImmunitiesAttributeName = "immunities";
                public const string MeleeWeaponAttributeName = "meleeWeapon";
                public const string RangedWeaponAttributeName = "rangedWeapon";
                public const string DeathStyleAttributeName = "deathStyle";
                public const string WeaponMagicLevelAttributeName = "weaponMagicLevel";
                public const string GoldAwardAttributeName = "goldAward";
            }

            public static class LootTable
            {
                public const string LootTableElementName = "LootTable";
                public const string LootEntryElementName = "LootEntry";

                public const string DropRateAttributeName = "dropRate";
                public const string TrapInfoAttributeName = "trapInfo";
                public const string RareDropChanceAttributeName = "rareDropChance";
                public const string CommonDropAttributeName = "commonDrop";
                public const string RareDropAttributeName = "rareDrop";
            }

            public static class Palette
            {
                public const string PalettesElementName = "Palettes";
                public const string PaletteElementName = "Palette";

                public const string PaletteAttributeName = "palette";


                public static string CreatePaletteAttributeName(int index)
                {
                    return $"{Palette.PaletteAttributeName}{index}";
                }
            }
        }

        public static class Bosses
        {
            public const string BossesElementName = "Bosses";

            public const string BossHeadersElementName = "BossHeaders";
            public const string BossHeaderElementName = "BossHeader";

            public const string ControlFlagsAttributeName = "controlFlages";

            public static class Palette
            {
                public const string BossPalettesElementName = "BossPalettes";

                public const string AddressAttributeName = "address";
            }

            public static class Weapon
            {
                public const string BossWeaponsElementName = "BossWeapons";
                public const string BossWeaponElementName = "BossWeapon";

                public const string AffinityAttributeName = "affinity";
                public const string ElementAttributeName = "element";
                public const string AccuracyAttributeName = "accuracy";
                public const string PowerAttributeName = "power";
                public const string StatusEffectsAttributeName = "statusEffects";
                public const string InflictionRateAttributeName = "inflictionRate";
            }
        }

        public static class Maps
        {
            public const string MapsElementName = "Maps";

            public static class DisplaySettings
            {
                public const string DisplaySettingsElementName = "DisplaySettings";
                public const string DisplaySettingElementName = "DisplaySetting";

                public const string MosaicSettingsAttributeName = "mosaicSettings";
                public const string MainScreenLayersEnabledAttributeName = "mainScreenLayersEnabled";
                public const string SubScreenLayersEnabledAttributeName = "subScreenLayersEnabled";
                public const string ColorMathAttributeName = "colorMath";
                public const string BackgroundColorAttributeName = "backgroundColor";
            }

            public static class Palette
            {
                public const string MapPaletteSetsElementName = "MapPaletteSets";
                public const string MapPaletteSetElementName = "MapPaletteSet";

                public const string PaletteElementName = "Palette";
            }

            public static class Flammie
            {
                public const string FlammieFlightCoordinatesSetsElementName = "FlammieFlightCoordinates";
                public const string FlammieFlightCoordinateSetElementName = "FlammieFlightCoordinate";
            }

            public static class Header
            {
                public const string MapHeadersElementName = "MapHeaders";
                public const string MapHeaderElementName = "MapHeader";

                public const string Tileset8x8IndexAttributeName = "tileset8x8Index";
                public const string PaletteSetIndexAttributeName = "paletteSetIndex";
                public const string Tileset16x16IndexAttributeName = "tileset16x16Index";
                public const string EventOptionsAttributeName = "eventOptions";
                public const string SpecialItemsOptionsAttributeName = "specialItemsOptions";
                public const string DisplaySettingsIndexAttributeName = "displaySettingsIndex";
                public const string UnusedAttributeName = "unused";
                public const string NpcPaletteIndexAttributeName = "npcPaletteIndex";
                public const string IsValidAttributeName = "isValid";

                public static class SpriteObject
                {
                    public const string MapSpriteObjectsElementName = "MapSpriteObjects";
                    public const string MapSpriteObjectElementName = "MapSpriteObject";

                    public const string EventFlagAttributeName = "eventFlag";
                    public const string EventFlagRangeAttributeName = "eventFlagRange";
                    public const string XCoordinateAttributeName = "xCoordinate";
                    public const string YCoordinateAttributeName = "yCoordinate";
                    public const string DirectionAttributeName = "direction";
                    public const string SpriteIndexAttributeName = "spriteIndex";
                    public const string EventIdLowAttributeName = "eventIdLow";
                    public const string EventIdHighAttributeName = "eventIdHigh";
                }
            }
        }

        public static class WorldMap
        {
            public const string WorldMapElementName = "WorldMap";

            public const string LandingLocationsElementName = "LandingLocations";
            public const string LandingLocationElementName = "LandingLocation";

            public const string CannonTravelCoordinateSetsElementName = "CannonTravelCoordinateSets";
            public const string CannonTravelCoordinateSetElementName = "CannonTravelCoordinateSet";

            public const string MapIndexAttributeName = "mapIndex";
            public const string XCoordinateAttributeName = "xCoordinate";
            public const string YCoordinateAttributeName = "yCoordinate";

            public const string StartXCoordinateAttributeName = "startXCoordinate";
            public const string StartYCoordinateAttributeName = "startYCoordinate";
            public const string EndXCoordinateAttributeName = "endXCoordinate";
            public const string EndYCoordinateAttributeName = "endYCoordinate";
        }
    }
}