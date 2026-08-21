using System;
using System.Reflection;
using System.Windows.Forms;
using ManaMagic.Core.WorldMap;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.WorldMap
{
    public partial class CannonTravelEditorUserControl : UserControl
    {
        private bool ignoreEvents = false;

        public CannonTravelEditorUserControl()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.dataGridView, new object[] { true });
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
                foreach (CannonTravelCoordinateSet coordinateSet in ManaMagicContext.Current.Context.WorldMapContext.CannonTravelCoordinatesTable)
                {
                    this.dataGridView.Rows.Add(coordinateSet.Index.ToString("X2"),
                                               coordinateSet.StartXCoordinate,
                                               coordinateSet.StartYCoordinate,
                                               coordinateSet.EndXCoordinate,
                                               coordinateSet.EndYCoordinate);
                }
                this.ignoreEvents = false;
            }
        }

        private void CannonTravelEditorUserControl_Load(object sender, EventArgs e)
        {
            this.SetCoordinateTable();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.ignoreEvents && e.RowIndex > -1)
            {
                CannonTravelCoordinateSet coordinateSet = ManaMagicContext.Current.Context.WorldMapContext.CannonTravelCoordinatesTable[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 1:
                        coordinateSet.StartXCoordinate = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                    case 2:
                        coordinateSet.StartYCoordinate = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                    case 3:
                        coordinateSet.EndXCoordinate = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                    case 4:
                        coordinateSet.EndYCoordinate = Convert.ToByte(this.dataGridView[e.ColumnIndex, e.RowIndex].Value);
                        break;
                }
            }
        }
    }
}