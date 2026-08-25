using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Controls.UserControls.Debug
{
    public partial class DebugMapTriggersViewUserControl : UserControl
    {
        public DebugMapTriggersViewUserControl()
        {
            InitializeComponent();
        }

        private void DebugMapTriggersViewUserControl_Load(object sender, EventArgs e)
        {
            this.debugDataGridView.SuspendLayout();
            this.noTriggersListBox.SuspendLayout();

            this.debugDataGridView.Columns.Add("nameColumn", "Name");
            this.debugDataGridView.Columns.Add("TypeColumn", "Type");
            this.debugDataGridView.Columns.Add("valueColumn", "Value");

            this.debugDataGridView.Columns["nameColumn"].Width = 450;
            this.debugDataGridView.Columns["valueColumn"].Width = 450;

            ushort index = 0;
            foreach (DataTable<MapTrigger> triggerList in ManaMagicContext.Current.Context.MapContext.MapTriggersTable)
            {
                if (triggerList.RowCount == 0)
                {
                    this.noTriggersListBox.Items.Add(ManaMetadata.GetMapFriendlyName(index));
                }
                else
                {
                    foreach (MapTrigger trigger in triggerList)
                    {
                        string valueString = string.Empty;
                        if (trigger.IsEvent) { valueString = ManaMetadata.GetEventFriendlyName(trigger.Value); }
                        else if (trigger.IsDoor) { valueString = ManaMetadata.GetDoorFriendlyName(trigger.Value); }
                        else { valueString = trigger.Value.ToString("X4"); }

                        this.debugDataGridView.Rows.Add(ManaMetadata.GetMapFriendlyName(index),
                                                        trigger.TriggerType,
                                                        valueString);
                    }
                }
                index++;
            }

            this.debugDataGridView.ResumeLayout();
            this.noTriggersListBox.ResumeLayout();
        }
    }
}