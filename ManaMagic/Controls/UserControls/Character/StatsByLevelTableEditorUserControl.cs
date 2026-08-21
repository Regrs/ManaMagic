using System;
using System.Reflection;
using System.Windows.Forms;
using ManaMagic.Core.Character;
using ZwellTech;
using ZwellTech.SuperNintendo;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.Character
{
    public partial class StatsByLevelTableEditorUserControl : UserControl
    {
        private readonly TreeViewSection characterToDisplay = TreeViewSection.RandiStatsByLevelEditor;

        public StatsByLevelTableEditorUserControl()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.statsByLevelDataGridView,  new object[] { true });
        }

        public StatsByLevelTableEditorUserControl(TreeViewSection characterToDisplay)
        {
            InitializeComponent();
            this.characterToDisplay = characterToDisplay;
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

        private void LoadStatByLevelTable()
        {
            foreach (CharacterStatsByLevel statEntry in this.GetStatusByLevelTable())
            {
                this.statsByLevelDataGridView.Rows.Add(statEntry.Index + 1,
                                                       statEntry.HitPoints,
                                                       statEntry.ManaPoints,
                                                       statEntry.Strength,
                                                       statEntry.Agility,
                                                       statEntry.Constitution,
                                                       statEntry.Intelligence,
                                                       statEntry.Wisdom);
            }
        }

        private DataTable<CharacterStatsByLevel> GetStatusByLevelTable()
        {
            switch (this.characterToDisplay)
            {
                case TreeViewSection.RandiStatsByLevelEditor:
                    return ManaMagicContext.Current.Context.CharacterContext.RandiStatsByLevelTable;
                case TreeViewSection.PurimStatsByLevelEditor:
                    return ManaMagicContext.Current.Context.CharacterContext.PurimStatsByLevelTable;
                case TreeViewSection.PopoieStatsByLevelEditor:
                    return ManaMagicContext.Current.Context.CharacterContext.PopoieStatsByLevelTable;
                default:
                    return (DataTable<CharacterStatsByLevel>)ThrowHelper.ThrowInvalidOperationException("Unknown Character To Load");
            }
        }

        private void StatsByLevelTableEditorUserControl_Load(object sender, EventArgs e)
        {
            this.LoadStatByLevelTable();
        }

        private void StatsByLevelDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                CharacterStatsByLevel statsByLevel = this.GetStatusByLevelTable()[e.RowIndex];
                if (e.ColumnIndex == hitPointsColumn.Index) { statsByLevel.HitPoints = Convert.ToUInt16(this.statsByLevelDataGridView[this.hitPointsColumn.Index, e.RowIndex].Value); }
                if (e.ColumnIndex == manaPointsColumn.Index) { statsByLevel.ManaPoints = Convert.ToByte(this.statsByLevelDataGridView[this.manaPointsColumn.Index, e.RowIndex].Value); }

                if (e.ColumnIndex == strengthColumn.Index) { statsByLevel.Strength = Convert.ToByte(this.statsByLevelDataGridView[this.strengthColumn.Index, e.RowIndex].Value); }
                if (e.ColumnIndex == agilityColumn.Index) { statsByLevel.Agility = Convert.ToByte(this.statsByLevelDataGridView[this.agilityColumn.Index, e.RowIndex].Value); }
                if (e.ColumnIndex == constitutionColumn.Index) { statsByLevel.Constitution = Convert.ToByte(this.statsByLevelDataGridView[this.constitutionColumn.Index, e.RowIndex].Value); }
                if (e.ColumnIndex == intelligenceColumn.Index) { statsByLevel.Intelligence = Convert.ToByte(this.statsByLevelDataGridView[this.intelligenceColumn.Index, e.RowIndex].Value); }
                if (e.ColumnIndex == wisdomColumn.Index) { statsByLevel.Wisdom = Convert.ToByte(this.statsByLevelDataGridView[this.wisdomColumn.Index, e.RowIndex].Value); }
            }
        }
    }
}