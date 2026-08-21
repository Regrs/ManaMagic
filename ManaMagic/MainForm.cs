using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ManaMagic.Controls.UserControls;
using ManaMagic.Controls.UserControls.Boss;
using ManaMagic.Controls.UserControls.Character;
using ManaMagic.Controls.UserControls.Debug;
using ManaMagic.Controls.UserControls.Debug.Views;
using ManaMagic.Controls.UserControls.Events;
using ManaMagic.Controls.UserControls.Help;
using ManaMagic.Controls.UserControls.Items;
using ManaMagic.Controls.UserControls.Items.Shops;
using ManaMagic.Controls.UserControls.Maps;
using ManaMagic.Controls.UserControls.References;
using ManaMagic.Controls.UserControls.Sprites;
using ManaMagic.Controls.UserControls.WorldMap;
using ManaMagic.Core.Help;
using ZwellTech;

#nullable enable

namespace ManaMagic
{
    // https://sneslab.net/wiki/Graphics_Format
    // https://snes.nesdev.org/wiki/PPU_registers
    // https://wiki.superfamicom.org/registers
    // https://web.archive.org/web/20221112231515if_/http://archive.6502.org/datasheets/wdc_65816_programming_manual.pdf
    public partial class MainForm : Form, IApplicationNavigation
    {
        private readonly IReadOnlyDictionary<TreeViewSection, UserControl> treeViewSections = new Dictionary<TreeViewSection, UserControl>()
        {
            // Events
            { TreeViewSection.EventEditor, new EventEditorUserControl() { Dock = DockStyle.Fill } },

            // Events -> Text
            { TreeViewSection.TownNameEditor, new StringTableEditorUserControl(StringTableEditType.TownNames) { Dock = DockStyle.Fill } },
            //{ TreeViewSection.EnemyNameEditor, new StringTableEditorUserControl(StringTableEditType.EnemyNames) { Dock = DockStyle.Fill } },
            { TreeViewSection.ItemErrorEditor, new StringTableEditorUserControl(StringTableEditType.ItemErrors) { Dock = DockStyle.Fill } },

            // Events -> Field Messages
            { TreeViewSection.StatusEffectMessageEditor, new StringTableEditorUserControl(StringTableEditType.StatusEffectMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.ElementalFearMessageEditor, new StringTableEditorUserControl(StringTableEditType.ElementalFearMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.TrapMessageEditor, new StringTableEditorUserControl(StringTableEditType.TrapMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.WeaponNameMessageEditor, new StringTableEditorUserControl(StringTableEditType.WeaponNameMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.BossSkillNameMessageEditor, new StringTableEditorUserControl(StringTableEditType.BossSkillMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.BuffDebuffMessageEditor, new StringTableEditorUserControl(StringTableEditType.BuffDebuffMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.LunarMagicMessageEditor, new StringTableEditorUserControl(StringTableEditType.LunarMagicMessages) { Dock = DockStyle.Fill } },
            //{ TreeViewSection.MiscellaneousMessageEditor, new StringTableEditorUserControl(StringTableEditType.MiscellaneousMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.TreasureChestMessageEditor, new StringTableEditorUserControl(StringTableEditType.TreasureChestMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.CombatMessageEditor, new StringTableEditorUserControl(StringTableEditType.CombatMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.AnalyzerMessageEditor, new StringTableEditorUserControl(StringTableEditType.AnalyzerMessages) { Dock = DockStyle.Fill } },
            { TreeViewSection.LevelUpMessageEditor, new StringTableEditorUserControl(StringTableEditType.LevelUpMessages) { Dock = DockStyle.Fill } },

            // Character
            { TreeViewSection.DefaultPartyDataEditor, new DefaultPartyDataEditor() { Dock = DockStyle.Fill } },
            { TreeViewSection.RandiStatsByLevelEditor, new StatsByLevelTableEditorUserControl(TreeViewSection.RandiStatsByLevelEditor) { Dock = DockStyle.Fill } },
            { TreeViewSection.PurimStatsByLevelEditor, new StatsByLevelTableEditorUserControl(TreeViewSection.PurimStatsByLevelEditor) { Dock = DockStyle.Fill } },
            { TreeViewSection.PopoieStatsByLevelEditor, new StatsByLevelTableEditorUserControl(TreeViewSection.PopoieStatsByLevelEditor) { Dock = DockStyle.Fill } },
            { TreeViewSection.ExperiencePerLevelEditor, new ExperiencePerLevelEditorUserControl() { Dock = DockStyle.Fill } },

            // Boss
            { TreeViewSection.BossEditor, new BossEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.BossPaletteEditor, new BossPaletteEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.BossWeaponEditor, new BossWeaponEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.BossTilesetAndFrameViewer, new BossTilesetFrameViewerUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.BossSkillViewer, new BossSkillViewerUserControl() { Dock = DockStyle.Fill } },

            // Sprites
            { TreeViewSection.EnemyEditor, new EnemyEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.SpritePaletteEditor, new SpritePaletteEditorUserControl() { Dock = DockStyle.Fill } },

            // Maps
            { TreeViewSection.MapEditor, new MapEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.MapDisplaySettingsEditor, new DisplaySettingsUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.MapPaletteEditor, new MapPaletteEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.FlammieFlightCoordinateEditor, new FlammieFlightCoordinateEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.Map8x8TilesetViewer, new Map8x8TilesetViewerUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.Map16x16TilesetViewer, new Map16x16TilesetViewerUserControl() { Dock = DockStyle.Fill } },

            // World Map
            { TreeViewSection.CannonTravelEditor, new CannonTravelEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.WorldMapLandingLocationEditor, new WorldMapLandingLocationEditorUserControl() { Dock = DockStyle.Fill } },

            // Items
            { TreeViewSection.WeaponEditor, new WeaponEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.EquipmentEditor, new EquipmentEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.ConsumablesEditor, new ConsumablesEditorUserControl() { Dock = DockStyle.Fill } },

            // Items -> Shops
            { TreeViewSection.ShopEditor, new ShopEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.ShopPriceEditor, new ShopPriceEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.ShopMessageEditor, new StringTableEditorUserControl(StringTableEditType.ShopMessages) { Dock = DockStyle.Fill } },

            // References
            { TreeViewSection.ReferenceSearch, new ReferenceSearchUserControl() { Dock = DockStyle.Fill } },
            
            // References -> RAM Maps
            { TreeViewSection.RamLayout, new RamLayoutUserControl(RamMapType.General) { Dock = DockStyle.Fill } },
            { TreeViewSection.RamSpriteSlot, new RamLayoutUserControl(RamMapType.SpriteSlot) { Dock = DockStyle.Fill } },
            { TreeViewSection.RamBossSpriteSlot, new RamLayoutUserControl(RamMapType.BossSpriteSlot) { Dock = DockStyle.Fill } },
            //bossRAMLayoutNode

            // References -> Usage
            { TreeViewSection.MapPieceUsageCounter, new UsageUserControl(UsageCounter.MapPiece) { Dock = DockStyle.Fill } },
            { TreeViewSection.MapCollisionUsageCounter, new UsageUserControl(UsageCounter.MapCollision) { Dock = DockStyle.Fill } },
            { TreeViewSection.MapPaletteUsageCounter, new UsageUserControl(UsageCounter.MapPalette) { Dock = DockStyle.Fill } },
            { TreeViewSection.SpriteUsageCounter, new UsageUserControl(UsageCounter.Sprite) { Dock = DockStyle.Fill } },
            { TreeViewSection.EventUsageCounter, new UsageUserControl(UsageCounter.Event) { Dock = DockStyle.Fill } },

            // Help
            //{ TreeViewSection.HelpSpriteSlotMemoryMap, new HelpUserControl(ManaHelpTopic.SpriteSlotMemoryMap) { Dock = DockStyle.Fill } },
            { TreeViewSection.HelpSpecialValues, new HelpUserControl(ManaHelpTopic.SpecialValues) { Dock = DockStyle.Fill } },
            { TreeViewSection.HelpCutContent, new HelpUserControl(ManaHelpTopic.CutContent) { Dock = DockStyle.Fill } },

            // Debug
            { TreeViewSection.DebugDisassembly, new DisassemblyUtilUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugDebugger, new DebuggerUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugDrawCanvas, new DebugDrawCanvasUserControl() { Dock = DockStyle.Fill } },

            // Debug -> Views
            { TreeViewSection.DebugDoorView, new DoorEditorUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugCollisionTypeView, new DebugCollisionTypeViewUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugMapTriggerView, new DebugMapTriggersViewUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugEnemyStatisticsView, new DebugEnemyStatViewUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugLootTableView, new DebugLootTableViewUserControl() { Dock = DockStyle.Fill } },
            { TreeViewSection.DebugWeaponDefinitionView, new DebugWeaponDefinitionViewUserControl() { Dock = DockStyle.Fill } },
        };

        public RichTextBox LoggingControl { get { return this.loggerOutputRichTextBox; } }

        public MainForm()
        {
            InitializeComponent();

            this.mainTreeView.Enabled = false;
            this.InitializeTreeView();
        }

        public void SwitchToNode(TreeViewSection section)
        {
            TreeNode? node = FindNode(this.mainTreeView.Nodes, section);

            this.mainTreeView.SelectedNode = node;
            this.DisplaySection(section);

            static TreeNode? FindNode(TreeNodeCollection nodes, TreeViewSection section)
            {
                foreach (TreeNode node in nodes)
                {
                    TreeNode? searchNode = FindNodeInner(node, section);
                    if (searchNode != null) { return searchNode; }
                }
                return null;
            }

            static TreeNode? FindNodeInner(TreeNode rootNode, TreeViewSection section)
            {
                if (IsMatchingNode(rootNode, section)) { return rootNode; }
                foreach (TreeNode node in rootNode.Nodes)
                {
                    if (IsMatchingNode(node, section)) { return node; }

                    TreeNode? innerNode = FindNodeInner(node, section);
                    if (innerNode != null) { return innerNode; }
                }
                return null;
            }

            static bool IsMatchingNode(TreeNode node, TreeViewSection section)
            {
                if (node is ManaTreeNode manaTreeNode) { return manaTreeNode.Section == section; }
                return (node != null && node.Tag != null) && (TreeViewSection)node.Tag == section;
            }
        }

        public void SwitchToNode(TreeViewSection section, int index)
        {
            this.SwitchToNode(section);
            if (this.treeViewSections[section] is IManaControl manaControl)
            {
                manaControl.SetIndex(index);
            }
            else
            {
                ThrowHelper.ThrowInvalidOperationException("Tried to switch to a section that wasn't of type IManaControl.");
            }
        }

        private void DisplaySection(TreeViewSection section)
        {
            this.secondarySplitContainer.Panel2.Controls.Clear();
            this.secondarySplitContainer.Panel2.Controls.Add(this.treeViewSections[section]);
        }

        private void InitializeTreeView()
        {
            this.mainTreeView.Nodes.Clear();

            // Events
            TreeNode eventsNode = new TreeNode("Events");
            eventsNode.Nodes.Add(new ManaTreeNode("Event Editor", TreeViewSection.EventEditor));

            // Events -> Text
            TreeNode textNode = new TreeNode("Text Editor");
            textNode.Nodes.Add(new ManaTreeNode("Save Locations", TreeViewSection.TownNameEditor));
            textNode.Nodes.Add(new ManaTreeNode("Item Errors", TreeViewSection.ItemErrorEditor));
            eventsNode.Nodes.Add(textNode);

            // Events -> Field Messages
            TreeNode fieldMessagesNode = new TreeNode("Field Message Editor");
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Status Effects", TreeViewSection.StatusEffectMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Elemental Fear", TreeViewSection.ElementalFearMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Traps", TreeViewSection.TrapMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Weapon Names", TreeViewSection.WeaponNameMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Boss Skill Names", TreeViewSection.BossSkillNameMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Buffs & Debuffs", TreeViewSection.BuffDebuffMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Lunar Magic", TreeViewSection.LunarMagicMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Treasure Chests", TreeViewSection.TreasureChestMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Combat", TreeViewSection.CombatMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Analyzer", TreeViewSection.AnalyzerMessageEditor));
            fieldMessagesNode.Nodes.Add(new ManaTreeNode("Level Up", TreeViewSection.LevelUpMessageEditor));
            eventsNode.Nodes.Add(fieldMessagesNode);

            // Character
            TreeNode characterNode = new TreeNode("Character");

            // Character -> Default Party Data Editor
            characterNode.Nodes.Add(new ManaTreeNode("Default Party Data Editor", TreeViewSection.DefaultPartyDataEditor));

            // Character -> Stats By Level
            TreeNode statsByLevelNode = new TreeNode("Stats By Level Editor");
            statsByLevelNode.Nodes.Add(new ManaTreeNode("Randi", TreeViewSection.RandiStatsByLevelEditor));
            statsByLevelNode.Nodes.Add(new ManaTreeNode("Purim", TreeViewSection.PurimStatsByLevelEditor));
            statsByLevelNode.Nodes.Add(new ManaTreeNode("Popoie", TreeViewSection.PopoieStatsByLevelEditor));
            characterNode.Nodes.Add(statsByLevelNode);

            // Character -> Experience Per Level
            characterNode.Nodes.Add(new ManaTreeNode("Experience Per Level", TreeViewSection.ExperiencePerLevelEditor));

            // Sprites
            TreeNode spritesNode = new TreeNode("Sprites");
            spritesNode.Nodes.Add(new ManaTreeNode("Enemy Editor", TreeViewSection.EnemyEditor));
            spritesNode.Nodes.Add(new ManaTreeNode("Palette Editor", TreeViewSection.SpritePaletteEditor));

            // Bosses
            TreeNode bossesNode = new TreeNode("Bosses");
            bossesNode.Nodes.Add(new ManaTreeNode("Boss Editor", TreeViewSection.BossEditor));
            bossesNode.Nodes.Add(new ManaTreeNode("Palette Editor", TreeViewSection.BossPaletteEditor));
            bossesNode.Nodes.Add(new ManaTreeNode("Weapon Editor", TreeViewSection.BossWeaponEditor));
            bossesNode.Nodes.Add(new ManaTreeNode("Tileset & Frame Viewer", TreeViewSection.BossTilesetAndFrameViewer));
            bossesNode.Nodes.Add(new ManaTreeNode("Skill Viewer", TreeViewSection.BossSkillViewer));

            // Maps
            TreeNode mapsNode = new TreeNode("Maps");
            mapsNode.Nodes.Add(new ManaTreeNode("Map Editor?", TreeViewSection.MapEditor));
            mapsNode.Nodes.Add(new ManaTreeNode("Display Settings Editor", TreeViewSection.MapDisplaySettingsEditor));
            mapsNode.Nodes.Add(new ManaTreeNode("Palette Viewer", TreeViewSection.MapPaletteEditor));
            mapsNode.Nodes.Add(new ManaTreeNode("Flammie Flight Editor", TreeViewSection.FlammieFlightCoordinateEditor));
            mapsNode.Nodes.Add(new ManaTreeNode("8x8 Tileset Viewer", TreeViewSection.Map8x8TilesetViewer));
            mapsNode.Nodes.Add(new ManaTreeNode("16x16 Tileset Viewer", TreeViewSection.Map16x16TilesetViewer));

            // World Map
            TreeNode worldMapNode = new TreeNode("World Map");
            worldMapNode.Nodes.Add(new ManaTreeNode("Cannon Travel Editor", TreeViewSection.CannonTravelEditor));
            worldMapNode.Nodes.Add(new ManaTreeNode("Landing Location Editor", TreeViewSection.WorldMapLandingLocationEditor));

            // Items
            TreeNode itemsNode = new TreeNode("Items");
            itemsNode.Nodes.Add(new ManaTreeNode("Weapons", TreeViewSection.WeaponEditor));
            itemsNode.Nodes.Add(new ManaTreeNode("Equipment", TreeViewSection.EquipmentEditor));
            itemsNode.Nodes.Add(new ManaTreeNode("Consumables", TreeViewSection.ConsumablesEditor));

            // Items -> Shops
            TreeNode shopsNode = new TreeNode("Shops");
            shopsNode.Nodes.Add(new ManaTreeNode("Shop Editor", TreeViewSection.ShopEditor));
            shopsNode.Nodes.Add(new ManaTreeNode("Price Editor", TreeViewSection.ShopPriceEditor));
            shopsNode.Nodes.Add(new ManaTreeNode("Shop Text Editor", TreeViewSection.ShopMessageEditor));
            itemsNode.Nodes.Add(shopsNode);

            // References
            TreeNode referencesNode = new TreeNode("References");
            referencesNode.Nodes.Add(new ManaTreeNode("Search", TreeViewSection.ReferenceSearch));

            // References -> RAM Maps
            TreeNode ramMapsNode = new TreeNode("RAM Maps");
            ramMapsNode.Nodes.Add(new ManaTreeNode("RAM Layout", TreeViewSection.RamLayout));
            ramMapsNode.Nodes.Add(new ManaTreeNode("Sprite Slot", TreeViewSection.RamSpriteSlot));
            ramMapsNode.Nodes.Add(new ManaTreeNode("Boss Sprite Slot", TreeViewSection.RamBossSpriteSlot));
            referencesNode.Nodes.Add(ramMapsNode);

            // References -> Usage
            TreeNode usageNode = new TreeNode("Usage");
            usageNode.Nodes.Add(new ManaTreeNode("Map Pieces", TreeViewSection.MapPieceUsageCounter));
            usageNode.Nodes.Add(new ManaTreeNode("Map Collision", TreeViewSection.MapCollisionUsageCounter));
            usageNode.Nodes.Add(new ManaTreeNode("Map Palettes", TreeViewSection.MapPaletteUsageCounter));
            usageNode.Nodes.Add(new ManaTreeNode("Sprites", TreeViewSection.SpriteUsageCounter));
            usageNode.Nodes.Add(new ManaTreeNode("Events", TreeViewSection.EventUsageCounter));
            referencesNode.Nodes.Add(usageNode);

            // Help
            TreeNode helpNode = new TreeNode("Help");
            helpNode.Nodes.Add(new ManaTreeNode("Special Values", TreeViewSection.HelpSpecialValues));
            helpNode.Nodes.Add(new ManaTreeNode("Cut Content", TreeViewSection.HelpCutContent));

            this.mainTreeView.Nodes.Add(eventsNode);
            this.mainTreeView.Nodes.Add(characterNode);
            this.mainTreeView.Nodes.Add(spritesNode);
            this.mainTreeView.Nodes.Add(bossesNode);
            this.mainTreeView.Nodes.Add(mapsNode);
            this.mainTreeView.Nodes.Add(worldMapNode);
            this.mainTreeView.Nodes.Add(itemsNode);
            this.mainTreeView.Nodes.Add(referencesNode);
            this.mainTreeView.Nodes.Add(helpNode);

            if (ManaMagicContext.DebugMode)
            {
                // Debug
                TreeNode debugNode = new TreeNode("Debug");
                debugNode.Nodes.Add(new ManaTreeNode("Disassembly", TreeViewSection.DebugDisassembly));
                debugNode.Nodes.Add(new ManaTreeNode("Debugger", TreeViewSection.DebugDebugger));
                debugNode.Nodes.Add(new ManaTreeNode("Draw Canvas", TreeViewSection.DebugDrawCanvas));

                // Debug -> Views
                TreeNode debugViewsNode = new TreeNode("Views");
                debugViewsNode.Nodes.Add(new ManaTreeNode("Doors", TreeViewSection.DebugDoorView));
                debugViewsNode.Nodes.Add(new ManaTreeNode("Collision", TreeViewSection.DebugCollisionTypeView));
                debugViewsNode.Nodes.Add(new ManaTreeNode("Map Triggers", TreeViewSection.DebugMapTriggerView));
                debugViewsNode.Nodes.Add(new ManaTreeNode("Enemy Statistics", TreeViewSection.DebugEnemyStatisticsView));
                debugViewsNode.Nodes.Add(new ManaTreeNode("Loot Table", TreeViewSection.DebugLootTableView));
                debugViewsNode.Nodes.Add(new ManaTreeNode("Weapon Definitions", TreeViewSection.DebugWeaponDefinitionView));
                debugNode.Nodes.Add(debugViewsNode);

                this.mainTreeView.Nodes.Add(debugNode);
            }
        }

        private void EnableMenuItems()
        {
            this.newProjectToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = true;
            this.exportToolStripMenuItem.Enabled = true;
            this.mainTreeView.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (ManaMagicContext.DebugMode)
            {
                if (ManaMagicContext.Current.DebugInitializeEditor())
                {
                    this.newProjectToolStripMenuItem.Enabled = false;
                    this.openToolStripMenuItem.Enabled = false;
                    this.saveToolStripMenuItem.Enabled = true;
                    this.exportToolStripMenuItem.Enabled = true;
                    this.mainTreeView.Enabled = true;
                }
            }
        }

        private void MainTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is ManaTreeNode node)
            {
                this.DisplaySection(node.Section);
                return;
            }
            if (e.Node.Tag is TreeViewSection section)
            {
                this.DisplaySection(section);
            }
        }

        private void TreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = true;
        }

        private void NewProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using NewProjectForm npf = new NewProjectForm();
            if (npf.ShowDialog() == DialogResult.OK)
            {
                ManaMagicContext.Current.InitializeEditor(npf.RomPath, npf.FullProjectPath);
                this.EnableMenuItems();
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select The Mana Magic Project File Path...";
            ofd.Filter = "Mana Magic Projects (*.mmagproj)|*.mmagproj";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ManaMagicContext.Current.InitializeEditor(ofd.FileName);
                this.EnableMenuItems();
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManaMagicContext.Current.Save();
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ManaMagicContext.DebugMode)
            {
                ManaMagicContext.Current.Export(@"Data\Secret of Mana (Debug).sfc");
                return;
            }

            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ManaMagicContext.Current.Export($"{fbd.SelectedPath}\\Secret of Mana (ManaMagic).sfc");
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}