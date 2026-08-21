using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;
using ZwellTech;

#nullable enable

namespace ManaMagic.Controls.UserControls.Debug
{
    public partial class DebugCollisionTypeViewUserControl : UserControl
    {
        public DebugCollisionTypeViewUserControl()
        {
            InitializeComponent();
        }

        private void MapCollisionDefinitionUserControl_Load(object sender, EventArgs e)
        {
            this.debugDataGridView.Columns.Add("indexColumn", "Name");
            this.debugDataGridView.Columns.Add("b1Column", "Byte 01");
            this.debugDataGridView.Columns.Add("b2Column", "Byte 02");
            this.debugDataGridView.Columns.Add("b3Column", "Byte 03");
            this.debugDataGridView.Columns.Add("b4Column", "Byte 04");
            this.debugDataGridView.Columns.Add("tileCollisionOptionsColumn", "Tile Collision Options");
            this.debugDataGridView.Columns.Add("eventIndexColumn", "Event Index");
            this.debugDataGridView.Columns.Add("replaceTileIndexColumn", "Replace Tile Index");
            this.debugDataGridView.Columns.Add("usageColumn", "Usage");

            this.debugDataGridView.Columns["indexColumn"].Width = 450;
            this.debugDataGridView.Columns["replaceTileIndexColumn"].Width = 125;

            foreach (MapCollision mapCollision in ManaMagicContext.Current.Context.MapContext.MapCollisionDefinitionTable)
            {
                this.debugDataGridView.Rows.Add(mapCollision.CollisionType.GetDisplayName(), 
                                            mapCollision.Byte1.ToString("X2"), 
                                            mapCollision.Byte2.ToString("X2"), 
                                            mapCollision.Byte3.ToString("X2"),
                                            mapCollision.Byte4.ToString("X2"),
                                            mapCollision.TileOptions.ToString(),
                                            mapCollision.IsEvent ? ManaMetadata.GetEventFriendlyName(mapCollision.EventIndex) : string.Empty,
                                            mapCollision.IsTileReplacer ? mapCollision.ReplaceTileIndex.ToString("X2") : string.Empty,
                                            ManaMagicContext.Current.Context.ReferenceCounter.MapCollisionTypeUsageCount[mapCollision.CollisionType]);
            }
        }
    }
}
