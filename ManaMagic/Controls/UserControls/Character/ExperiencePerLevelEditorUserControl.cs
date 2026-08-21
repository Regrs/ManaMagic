using System;
using System.Windows.Forms;
using ManaMagic.Core.Character;
using ZwellTech.Windows.Forms;

#nullable enable

namespace ManaMagic.Controls.UserControls.Character
{
    public partial class ExperiencePerLevelEditorUserControl : UserControl
    {
        public ExperiencePerLevelEditorUserControl()
        {
            InitializeComponent();
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

        private void LoadExperienceByLevelTable()
        {
            int level = 2;
            foreach (ExperiencePerLevel expPerLevel in ManaMagicContext.Current.Context.CharacterContext.ExperiencePerLevelTable)
            {
                this.experienceByLevelDataGridView.Rows.Add(level, expPerLevel.Experience);
                level++;
            }
        }

        private void ExperiencePerLevelEditorUserControl_Load(object sender, EventArgs e)
        {
            this.LoadExperienceByLevelTable();
        }

        private void ExperienceByLevelDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ExperiencePerLevel expPerLevel = ManaMagicContext.Current.Context.CharacterContext.ExperiencePerLevelTable[e.RowIndex];
                expPerLevel.Experience = Convert.ToUInt32(this.experienceByLevelDataGridView[this.expColumn.Index, e.RowIndex].Value);
            }
        }
    }
}