using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Metadata;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.Maps
{
    public partial class FlammieFlightCoordinateEditorUserControl : UserControl
    {
        private bool ignoreEvents = false;

        public FlammieFlightCoordinateEditorUserControl()
        {
            InitializeComponent();
            this.dataGridView.SetDoubleBuffered(true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= (int)ExtendedWindowStyles.WS_EX_COMPOSITED;
                return cp;
            }
        }

        private void SetCoordinateTable()
        {
            if (ManaMagicContext.Current.Context.Loaded)
            {
                this.ignoreEvents = true;
                this.dataGridView.Rows.Clear();
                foreach (ManaPoint8 point in ManaMagicContext.Current.Context.MapContext.FlammieFlightCoordinateTable)
                {
                    this.dataGridView.Rows.Add(ManaMetadata.GetMapFriendlyName(point.Index), point.X, point.Y);
                }
                this.ignoreEvents = false;
            }
        }

        private void FlammieFlightCoordinateEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetCoordinateTable();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.ignoreEvents && e.RowIndex > -1)
            {
                ManaPoint8 point = ManaMagicContext.Current.Context.MapContext.FlammieFlightCoordinateTable[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 1:
                        point.X = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                    case 2:
                        point.Y = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                }
            }
        }
    }
}