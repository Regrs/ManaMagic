using System;
using System.Windows.Forms;
using ManaMagic.Core.WorldMap;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.WorldMap
{
    public partial class WorldMapLandingLocationEditorUserControl : UserControl
    {
        private bool ignoreEvents = false;

        public WorldMapLandingLocationEditorUserControl()
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
                foreach (WorldMapLandingLocation landingLocation in ManaMagicContext.Current.Context.WorldMapContext.WorldMapLandingLocationTable)
                {
                    this.dataGridView.Rows.Add(landingLocation.Index.ToString("X2"),
                                               landingLocation.MapIndex,
                                               landingLocation.XCoordinate,
                                               landingLocation.YCoordinate);
                }
                this.ignoreEvents = false;
            }
        }

        private void WorldMapLandingLocationEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetCoordinateTable();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.ignoreEvents && e.RowIndex > -1)
            {
                WorldMapLandingLocation landingLocation = ManaMagicContext.Current.Context.WorldMapContext.WorldMapLandingLocationTable[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 1:
                        landingLocation.MapIndex = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                    case 2:
                        landingLocation.XCoordinate = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                    case 3:
                        landingLocation.YCoordinate = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                }
            }
        }
    }
}