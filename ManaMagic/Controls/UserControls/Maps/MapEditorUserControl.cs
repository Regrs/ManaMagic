using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Debugger;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;
using ZwellTech;
using ZwellTech.Logging;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class MapEditorUserControl : UserControl, IManaControl
    {
        private readonly IReadOnlyDictionary<string, Action<MapSpriteObject>> MapSpriteObjectEventHandlers;

        private ManaMap activeMap = ManaMap.Empty;
        private int mapSpiteObjectIndex = -1;
        private bool ignoreEvents = false;

        public MapEditorUserControl()
        {
            InitializeComponent();

            this.DebugEnableOptions();
            this.Initialize();
            this.onEnterEventNumericUpDown.Maximum = Constants.Bank0A.EventIdMaximum;
            this.SetEnabledState(false);

            this.MapSpriteObjectEventHandlers = this.LoadMapSpriteObjectEventHandlers();
        }

        public void SetIndex(int index)
        {
            this.mapIdNumericUpDown.Value = index;
        }

        private void Initialize()
        {
            this.SuspendLayout();
            this.ignoreEvents = true;
            foreach (MapEventOptions option in Enum.GetValues<MapEventOptions>())
            {
                if (option != MapEventOptions.None)
                {
                    this.eventOptionsCheckedListBox.Items.Add(option);
                }
            }

            foreach (MapSpecialItemsOptions option in Enum.GetValues<MapSpecialItemsOptions>())
            {
                if (option != MapSpecialItemsOptions.None)
                {
                    this.specialItemOptionsCheckedListBox.Items.Add(option);
                }
            }

            foreach (LayerLoadMode option in Enum.GetValues<LayerLoadMode>())
            {
                this.layerLoadModeCheckedListBox.Items.Add(option);
            }

            ManaUtil.BindComboBox(this.spriteIndexComboBox, ManaMetadata.SpriteNameStrings);

            this.spriteEventFlagComboBox.DataSource = Enum.GetValues<EventFlag>();
            this.spriteEventFlagComboBox.SelectedItem = EventFlag.HeadsUpDisplayEnabled;

            this.spriteDirectionComboBox.DataSource = Enum.GetValues<SpriteDirection>();
            this.spriteDirectionComboBox.SelectedItem = SpriteDirection.North;

            this.ignoreEvents = false;
            this.ResumeLayout();
        }

        private void SetMap(bool changeForceState = true)
        {
            this.SuspendLayout();
            this.mapIdNumericUpDown.Enabled = false;
            this.mapSpiteObjectIndex = -1;
            ushort mapId = (ushort)this.mapIdNumericUpDown.Value;
            try
            {
                this.activeMap = ManaMagicContext.Current.Context.MapContext.CreateMap(mapId);

                if (changeForceState) { this.UpdateForceState(); }
                this.mapNameLabel.Text = ManaMetadata.GetMapFriendlyName(mapId, false);
                this.SetHeader();
                this.SetMapObjectTable();
                this.SetTriggerTable();

                this.DrawMap(mapId, this.activeMap, this.CreateDrawingOptions());
            }
            catch (Exception ex)
            {
                this.mapPictureBox.Image = null;
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Exception: {ex.GetType()}, Message: {ex.Message}");
            }
            finally
            {
                this.mapIdNumericUpDown.Enabled = true;
                this.mapIdNumericUpDown.Focus();
                this.ResumeLayout();
            }
        }

        private void SetEnabledState(bool enabled)
        {
            this.SuspendLayout();
            this.tileset8x8NumericUpDown.Enabled = enabled;
            this.tileset16x16NumericUpDown.Enabled = enabled;
            this.paletteSetNumericUpDown.Enabled = enabled;
            this.displaySettingsNumericUpDown.Enabled = enabled;
            this.layerScrollSettingsNumericUpDown.Enabled = enabled;
            this.unknownNumericUpDown.Enabled = enabled;
            this.npcPaletteSetNumericUpDown.Enabled = enabled;
            this.combatMapCheckBox.Enabled = enabled;
            this.eventOptionsCheckedListBox.Enabled = enabled;
            this.specialItemOptionsCheckedListBox.Enabled = enabled;
            this.layerLoadModeCheckedListBox.Enabled = enabled;
            this.onEnterGroupBox.Enabled = enabled;
            this.ResumeLayout();
        }

        [Conditional("DEBUG")]
        private void DebugEnableOptions()
        {
            this.debugDrawAllButton.Tag = MapDebuggerOption.DrawAllMaps;
            this.debugScanForTileIDButton.Tag = MapDebuggerOption.ScanForTileIndex;
            this.debugPrintSpiteActionsButton.Tag = MapDebuggerOption.PrintSpiteActions;
            this.debugLayerSettingsCountButton.Tag = MapDebuggerOption.CountLayerSettings;
            this.debugSpriteUnknownBitsButton.Tag = MapDebuggerOption.SpriteUnknownBits;
            this.debugMapUnknownByteButton.Tag = MapDebuggerOption.MapUnknownByte;
            this.debugScanForF8FFCommandsButton.Tag = MapDebuggerOption.ScanForF8FFCommands;
        }

        private void UpdateForceState()
        {
            this.forceDrawCheckBox.CheckedChanged -= this.ForceDrawCheckBox_CheckedChanged;
            this.forceDrawCheckBox.Checked = false;
            this.forceDrawCheckBox.Enabled = !this.activeMap.IsValid && this.activeMap.ObjectTable.IsValid;
            if (this.forceDrawCheckBox.Enabled)
            {
                this.forceDrawCheckBox.CheckedChanged += this.ForceDrawCheckBox_CheckedChanged;
            }
        }

        private void SetHeader()
        {
            this.tileset8x8NumericUpDown.Value = this.activeMap.Header.Tileset8x8Index;
            this.tileset16x16NumericUpDown.Value = this.activeMap.Header.Tileset16x16Index;
            this.paletteSetNumericUpDown.Value = this.activeMap.Header.PaletteSetIndex;
            this.displaySettingsNumericUpDown.Value = this.activeMap.Header.DisplaySettingsIndex;
            this.layerScrollSettingsNumericUpDown.Value = this.activeMap.Header.LayerScrollSettingsIndex;
            this.unknownNumericUpDown.Value = this.activeMap.Header.Unused;
            this.npcPaletteSetNumericUpDown.Value = this.activeMap.Header.NpcPaletteIndex;
            this.combatMapCheckBox.Checked = this.activeMap.Header.IsCombatMap;

            int index = 0;
            foreach (MapEventOptions option in Enum.GetValues<MapEventOptions>())
            {
                if (option != MapEventOptions.None)
                {
                    this.eventOptionsCheckedListBox.SetItemChecked(index, this.activeMap.Header.EventOptions.HasFlag(option));
                    index++;
                }
            }

            index = 0;
            foreach (MapSpecialItemsOptions option in Enum.GetValues<MapSpecialItemsOptions>())
            {
                if (option != MapSpecialItemsOptions.None)
                {
                    this.specialItemOptionsCheckedListBox.SetItemChecked(index, this.activeMap.Header.SpecialItemsOptions.HasFlag(option));
                    index++;
                }
            }

            index = 0;
            foreach (LayerLoadMode option in Enum.GetValues<LayerLoadMode>())
            {
                this.layerLoadModeCheckedListBox.SetItemChecked(index, this.activeMap.Header.LayerLoadMode.HasFlag(option));
                index++;
            }

            string onEnterText = "0000";
            ushort onEnterValue = 0;
            if (this.activeMap.Header.EventOptions.HasFlag(MapEventOptions.OnEnter))
            {
                onEnterText = ManaMetadata.GetEventFriendlyName(this.activeMap.Triggers[0].Value);
                onEnterValue = this.activeMap.Triggers[0].Value;
            }
            this.onEnterEventLabel.Text = onEnterText;
            this.onEnterEventNumericUpDown.Value = onEnterValue;

            this.spriteObjectTreeView.Nodes.Clear();
            this.spriteEditorPanel.Visible = false;
            foreach (MapSpriteObject spriteObject in this.activeMap.Header.ObjectTable)
            {
                TreeNode rootNode = new TreeNode($"Sprite: {ManaMetadata.GetSpriteFriendlyName(spriteObject.SpriteIndex)}");
                rootNode.Checked = true;
                rootNode.Tag = spriteObject;

                this.spriteObjectTreeView.Nodes.Add(rootNode);
            }
        }

        private void SetMapObjectTable()
        {
            this.layer1ObjectsTreeView.Nodes.Clear();
            this.layer2ObjectsTreeView.Nodes.Clear();
            if (this.activeMap.ObjectTable.IsValid)
            {
                TreeNode layer1Node = new TreeNode($"Background Object: {this.activeMap.ObjectTable.Layer1Background.Index:X4}");
                layer1Node.Nodes.Add(new TreeNode($"Event Flag: {this.activeMap.ObjectTable.Layer1Background.EventFlag.GetDisplayName()}"));
                layer1Node.Nodes.Add(new TreeNode($"Event Flag Minimum: {this.activeMap.ObjectTable.Layer1Background.EventFlagRange.Minimum:X2}"));
                layer1Node.Nodes.Add(new TreeNode($"Event Flag Maximum: {this.activeMap.ObjectTable.Layer1Background.EventFlagRange.Maximum:X2}"));
                layer1Node.Nodes.Add(new TreeNode($"X-Coordinate: {this.activeMap.ObjectTable.Layer1Background.Location.X:X2}"));
                layer1Node.Nodes.Add(new TreeNode($"Y-Coordinate: {this.activeMap.ObjectTable.Layer1Background.Location.Y:X2}"));
                layer1Node.Checked = true;
                this.layer1ObjectsTreeView.Nodes.Add(layer1Node);

                foreach (MapObject mapObject in this.activeMap.ObjectTable.Layer1)
                {
                    TreeNode rootNode = new TreeNode($"Map Object: {mapObject.Index:X4}");
                    rootNode.Nodes.Add(new TreeNode($"Event Flag: {mapObject.EventFlag.GetDisplayName()}"));
                    rootNode.Nodes.Add(new TreeNode($"Event Flag Minimum: {mapObject.EventFlagRange.Minimum:X2}"));
                    rootNode.Nodes.Add(new TreeNode($"Event Flag Maximum: {mapObject.EventFlagRange.Maximum:X2}"));
                    rootNode.Nodes.Add(new TreeNode($"X-Coordinate: {mapObject.Location.X:X2}"));
                    rootNode.Nodes.Add(new TreeNode($"Y-Coordinate: {mapObject.Location.Y:X2}"));
                    rootNode.Checked = true;

                    this.layer1ObjectsTreeView.Nodes.Add(rootNode);
                }

                TreeNode layer2Node = new TreeNode($"Background Object: {this.activeMap.ObjectTable.Layer2Background.Index:X4}");
                layer2Node.Nodes.Add(new TreeNode($"Event Flag: {this.activeMap.ObjectTable.Layer2Background.EventFlag.GetDisplayName()}"));
                layer2Node.Nodes.Add(new TreeNode($"Event Flag Minimum: {this.activeMap.ObjectTable.Layer2Background.EventFlagRange.Minimum:X2}"));
                layer2Node.Nodes.Add(new TreeNode($"Event Flag Maximum: {this.activeMap.ObjectTable.Layer2Background.EventFlagRange.Maximum:X2}"));
                layer2Node.Nodes.Add(new TreeNode($"X-Coordinate: {this.activeMap.ObjectTable.Layer2Background.Location.X:X2}"));
                layer2Node.Nodes.Add(new TreeNode($"Y-Coordinate: {this.activeMap.ObjectTable.Layer2Background.Location.Y:X2}"));
                layer2Node.Checked = true;
                this.layer2ObjectsTreeView.Nodes.Add(layer2Node);

                foreach (MapObject mapObject in this.activeMap.ObjectTable.Layer2)
                {
                    TreeNode rootNode = new TreeNode($"Map Object: {mapObject.Index:X4}");
                    rootNode.Nodes.Add(new TreeNode($"Event Flag: {mapObject.EventFlag.GetDisplayName()}"));
                    rootNode.Nodes.Add(new TreeNode($"Event Flag Minimum: {mapObject.EventFlagRange.Minimum:X2}"));
                    rootNode.Nodes.Add(new TreeNode($"Event Flag Maximum: {mapObject.EventFlagRange.Maximum:X2}"));
                    rootNode.Nodes.Add(new TreeNode($"X-Coordinate: {mapObject.Location.X:X2}"));
                    rootNode.Nodes.Add(new TreeNode($"Y-Coordinate: {mapObject.Location.Y:X2}"));
                    rootNode.Checked = true;

                    this.layer2ObjectsTreeView.Nodes.Add(rootNode);
                }
            }
        }

        private void SetTriggerTable()
        {
            this.triggersListView.Items.Clear();
            foreach (MapTrigger trigger in this.activeMap.Triggers)
            {
                ListViewItem item = new ListViewItem(trigger.TriggerType.ToString());
                if (trigger.IsEvent) { item.SubItems.Add(new ListViewItem.ListViewSubItem(item, ManaMetadata.GetEventFriendlyName(trigger.Value))); }
                else if (trigger.IsDoor) { item.SubItems.Add(new ListViewItem.ListViewSubItem(item, ManaMetadata.GetDoorFriendlyName(trigger.Value))); }
                else { item.SubItems.Add(new ListViewItem.ListViewSubItem(item, trigger.Value.ToString("X4"))); }

                this.triggersListView.Items.Add(item);
            }
        }

        private void RefreshMap()
        {
            ushort mapId = (ushort)this.mapIdNumericUpDown.Value;
            this.DrawMap(mapId, this.activeMap, this.CreateDrawingOptions());
        }

        private void DrawMap(int mapId, ManaMap map, MapDrawingOptions options)
        {
            if (map.IsValid)
            {
                using SuperNintendoGraphics graphics = map.DrawMap(options, ManaMagicContext.Current.Context.SpriteContext, this.mapSpiteObjectIndex);
                this.mapPictureBox.Image = graphics.GetBitmap(true);
                return;
            }
            else if (this.forceDrawCheckBox.Checked)
            {
                //ManaMap map2 = new ManaMap(map.Header, SecretOfManaContext.Default.MapContext.DisplaySettingsTable[0], map.ObjectTable, map.Layer1Background, map.Layer2Background, map.Layer1, map.Layer2, SecretOfManaContext.Default.MapContext.Map8x8TilesetTable[0], SecretOfManaContext.Default.MapContext.Map16x16TilesetTable[0], SecretOfManaContext.Default.MapContext.PaletteSetTable[0]);
                using SuperNintendoGraphics graphics = ManaMagicContext.Current.Context.MapContext.CreateMap(mapId, true).DrawMap(options, ManaMagicContext.Current.Context.SpriteContext, this.mapSpiteObjectIndex);
                this.mapPictureBox.Image = graphics.GetBitmap(true);
                return;
            }

            this.mapPictureBox.Image = null;
            if (!map.Header.IsValid && !map.ObjectTable.IsValid)
            {
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Header and Object Table are invalid.");
            }
            else if (!map.Header.IsValid)
            {
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Header is invalid.");
            }
            else if (!map.ObjectTable.IsValid)
            {
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}. Object Table is invalid.");
            }
            else
            {
                LoggerEngine.Logger.LogError(LogComponent.General, $"Cannot draw map index {mapId:X4}.");
            }
        }

        private MapDrawingOptions CreateDrawingOptions()
        {
            return new MapDrawingOptions(this.backgroundToolStripMenuItem.Checked,
                                         this.layer01Background01ToolStripMenuItem.Checked,
                                         this.layer01Background02ToolStripMenuItem.Checked,
                                         this.layer02Background01ToolStripMenuItem.Checked,
                                         this.layer02Background02ToolStripMenuItem.Checked);
        }

        private void MapEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetIndex(0x0010); // Color Math map.
            //this.SetIndex(0x001C); // F8-FD command using map.
            //this.SetMap();
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            this.SetMap();
        }

        private void TreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = true;
        }

        private void ForceDrawCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            this.SetMap(false);
        }

        private void ViewDisplaySettingsButton_Click(object sender, EventArgs e)
        {
            ManaMagicContext.Current.SwitchToSection(TreeViewSection.MapDisplaySettingsEditor, (int)this.displaySettingsNumericUpDown.Value);
        }

        private void View8x8TilesetButton_Click(object sender, EventArgs e)
        {
            ManaMagicContext.Current.SwitchToSection(TreeViewSection.Map8x8TilesetViewer, (int)this.tileset8x8NumericUpDown.Value);
        }

        private void View16x16TilesetButton_Click(object sender, EventArgs e)
        {
            ManaMagicContext.Current.SwitchToSection(TreeViewSection.Map16x16TilesetViewer, (int)this.tileset16x16NumericUpDown.Value);
        }

        private void RunMapDebuggerButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is MapDebuggerOption debuggerOption)
            {
                ManaDebugger.RunDebugger(debuggerOption);
            }
        }

        private void SpriteObjectTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is MapSpriteObject spriteObject)
            {
                this.ignoreEvents = true;
                this.mapSpiteObjectIndex = spriteObject.Index;

                this.spriteIndexComboBox.SelectedIndex = spriteObject.SpriteIndex;
                this.spriteEventFlagComboBox.SelectedItem = spriteObject.EventFlag;
                this.spriteFlagMinNumericUpDown.Value = spriteObject.EventFlagMinimum;
                this.spriteFlagMaxNumericUpDown.Value = spriteObject.EventFlagMaximum;
                this.spriteXCoordinateNumericUpDown.Value = spriteObject.Location.X;
                this.spriteYCoordinateNumericUpDown.Value = spriteObject.Location.Y;
                this.spriteDirectionComboBox.SelectedItem = spriteObject.Direction;
                this.spritePaletteIndexNumericUpDown.Value = spriteObject.PaletteIndex;
                this.spriteEventNumericUpDown.Value = spriteObject.EventIndex;
                this.spriteEventInRadiusCheckBox.Checked = spriteObject.Interactions.HasFlag(SpriteInteractTypes.EventInRadius);
                this.spriteDoNotFaceOnInteractCheckBox.Checked = spriteObject.Interactions.HasFlag(SpriteInteractTypes.DoNotFaceOnInteract);
                this.spriteEventOnInteractCheckBox.Checked = spriteObject.Interactions.HasFlag(SpriteInteractTypes.EventOnInteract);
                this.spritePushableCheckBox.Checked = spriteObject.Interactions.HasFlag(SpriteInteractTypes.Pushable);

                this.spriteAlwaysLoadedCheckBox.Checked = spriteObject.AlwaysLoaded;
                this.spriteStationaryCheckBox.Checked = spriteObject.Stationary;

                this.spriteEventNameLabel.Text = ManaMetadata.GetEventFriendlyName(spriteObject.EventIndex);
                this.spriteEditorPanel.Visible = true;
                this.ignoreEvents = false;

                this.RefreshMap();
                return;
            }
            this.mapSpiteObjectIndex = -1;
        }

        private IReadOnlyDictionary<string, Action<MapSpriteObject>> LoadMapSpriteObjectEventHandlers()
        {
            return new Dictionary<string, Action<MapSpriteObject>>()
            {
                { this.spriteIndexComboBox.Name, (MapSpriteObject spriteObject) =>
                    {
                        spriteObject.SpriteIndex = (byte)this.spriteIndexComboBox.SelectedIndex;
                        this.spriteObjectTreeView.SelectedNode.Text = $"Sprite: {ManaMetadata.GetSpriteFriendlyName(spriteObject.SpriteIndex)}";
                        this.RefreshMap();
                    }
                },

                { this.spriteEventFlagComboBox.Name, (MapSpriteObject spriteObject) => { spriteObject.EventFlag = (EventFlag)this.spriteEventFlagComboBox.SelectedItem; } },
                { this.spriteFlagMinNumericUpDown.Name, (MapSpriteObject spriteObject) => { spriteObject.EventFlagMinimum = (byte)this.spriteFlagMinNumericUpDown.Value; } },
                { this.spriteFlagMaxNumericUpDown.Name, (MapSpriteObject spriteObject) => { spriteObject.EventFlagMaximum = (byte)this.spriteFlagMaxNumericUpDown.Value; } },
                { this.spriteXCoordinateNumericUpDown.Name, (MapSpriteObject spriteObject) => { spriteObject.Location.X = (byte)this.spriteXCoordinateNumericUpDown.Value; } },
                { this.spriteYCoordinateNumericUpDown.Name, (MapSpriteObject spriteObject) => { spriteObject.Location.Y = (byte)this.spriteYCoordinateNumericUpDown.Value; } },
                { this.spriteDirectionComboBox.Name, (MapSpriteObject spriteObject) => { spriteObject.Direction = (SpriteDirection)this.spriteDirectionComboBox.SelectedItem; } },
                { this.spritePaletteIndexNumericUpDown.Name, (MapSpriteObject spriteObject) => { spriteObject.PaletteIndex = (byte)this.spritePaletteIndexNumericUpDown.Value; } },
                { this.spriteEventNumericUpDown.Name, (MapSpriteObject spriteObject) => { spriteObject.EventIndex = (byte)this.spriteEventNumericUpDown.Value; } },

                { this.spriteEventInRadiusCheckBox.Name, (MapSpriteObject spriteObject) => { spriteObject.Interactions = (SpriteInteractTypes)ManaUtil.SetFlagState((byte)spriteObject.Interactions, (byte)SpriteInteractTypes.EventInRadius, this.spriteEventInRadiusCheckBox.Checked); } },
                { this.spriteDoNotFaceOnInteractCheckBox.Name, (MapSpriteObject spriteObject) => { spriteObject.Interactions = (SpriteInteractTypes)ManaUtil.SetFlagState((byte)spriteObject.Interactions, (byte)SpriteInteractTypes.DoNotFaceOnInteract, this.spriteDoNotFaceOnInteractCheckBox.Checked); } },
                { this.spriteEventOnInteractCheckBox.Name, (MapSpriteObject spriteObject) => { spriteObject.Interactions = (SpriteInteractTypes)ManaUtil.SetFlagState((byte)spriteObject.Interactions, (byte)SpriteInteractTypes.EventOnInteract, this.spriteEventOnInteractCheckBox.Checked); } },
                { this.spritePushableCheckBox.Name, (MapSpriteObject spriteObject) => { spriteObject.Interactions = (SpriteInteractTypes)ManaUtil.SetFlagState((byte)spriteObject.Interactions, (byte)SpriteInteractTypes.Pushable, this.spritePushableCheckBox.Checked); } },

                { this.spriteAlwaysLoadedCheckBox.Name, (MapSpriteObject spriteObject) => { spriteObject.AlwaysLoaded = this.spriteAlwaysLoadedCheckBox.Checked; } },
                { this.spriteStationaryCheckBox.Name, (MapSpriteObject spriteObject) => { spriteObject.Stationary = this.spriteStationaryCheckBox.Checked; } },
            };
        }

        private void MapSpriteObjectControl_ValueChanged(object sender, EventArgs args)
        {
            if (!this.ignoreEvents && this.activeMap != null && this.mapSpiteObjectIndex != -1 && sender is Control control)
            {
                if (this.MapSpriteObjectEventHandlers.TryGetValue(control.Name, out Action<MapSpriteObject>? eventHandler))
                {
                    eventHandler(this.activeMap.Header.ObjectTable[this.mapSpiteObjectIndex]);
                }
            }
        }
    }
}