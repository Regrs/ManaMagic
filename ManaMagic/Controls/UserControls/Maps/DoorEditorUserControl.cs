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

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class DoorEditorUserControl : UserControl
    {
        public DoorEditorUserControl()
        {
            InitializeComponent();
        }

        private void DoorEditorUserControl_Load(object sender, EventArgs e)
        {
            foreach (ManaDoor door in ManaMagicContext.Current.Context.MapContext.DoorTable)
            {
                string rawValues = $"{door.Byte1:X2} {door.Byte2:X2} {door.Byte3:X2} {door.Byte4:X2}";
                this.dataGridView1.Rows.Add(door.Index.ToString("X4"),
                                            door.Name,
                                            ManaMetadata.GetMapFriendlyName(door.MapId),
                                            //door.MapId.ToString("X4"),
                                            door.XCoordinate.ToString("X2"),
                                            door.YCoordinate.ToString("X2"),
                                            (door.Byte4 & 0B_1110_0000).ToString("X2"),
                                            (door.Byte4 & 0B_0001_1111).ToString("X2"),
                                            rawValues);
            }
        }
    }
}
/*
    Entrance Types:
    00: 0000_0000
    80: 1000_0000
    A0: 1010_0000
    C0: 1100_0000
    E0: 1110_0000
 */