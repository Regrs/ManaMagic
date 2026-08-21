using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Analyzer;
using ManaMagic.Core.Events;
using ManaMagic.Core.Maps;
using ManaMagic.Core.Metadata;

#nullable enable

namespace ManaMagic.Controls.UserControls.References
{
    public partial class ReferenceSearchUserControl : UserControl
    {
        public ReferenceSearchUserControl()
        {
            InitializeComponent();
            this.searchEventNumericUpDown.Maximum = Constants.Bank0A.EventIdMaximum;
            this.eventFlagSearchComboBox.DataSource = Enum.GetValues<EventFlag>();
            this.eventFlagSearchComboBox.SelectedIndex = 0;

            this.eventOpCodeSearchComboBox.DataSource = Enum.GetValues<EventOpCodeType>();
            this.eventOpCodeSearchComboBox.SelectedIndex = 0;
        }

        private void SearchEventButton_Click(object sender, EventArgs e)
        {
            ushort eventIndex = (ushort)this.searchEventNumericUpDown.Value;
            IReadOnlyList<AnalyzerResult> results = ManaMagicContext.Current.Context.Analyzer.FindEventReferences(eventIndex);

            ReferenceSearchUserControl.SetListViewResults(this.eventReferencesListView, results);
        }

        private void EventFlagSearchButton_Click(object sender, EventArgs e)
        {
            EventFlag eventFlag = (EventFlag)this.eventFlagSearchComboBox.SelectedItem;
            IReadOnlyList<AnalyzerResult> results = ManaMagicContext.Current.Context.Analyzer.FindEventFlagReferences(eventFlag);

            ReferenceSearchUserControl.SetListViewResults(this.eventFlagReferencesListView, results);
        }

        private void searchEventOpCodesButton_Click(object sender, EventArgs e)
        {
            EventOpCodeType opCodeType = (EventOpCodeType)this.eventOpCodeSearchComboBox.SelectedItem;
            IReadOnlyList<AnalyzerResult> results = ManaMagicContext.Current.Context.Analyzer.FindEventOpCodeReferences(opCodeType);

            ReferenceSearchUserControl.SetListViewResults(this.eventOpCodeReferencesListView, results);
        }

        private void ReferencesListView_DoubleClick(object sender, EventArgs e)
        {
            if (sender is ListView listView)
            {
                if (listView.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView.SelectedItems[0];
                    if (item.Tag is AnalyzerResult analyzerResult)
                    {
                        ReferenceSearchUserControl.SwitchToSection(analyzerResult);
                    }
                }
            }
        }

        private static void SetListViewResults(ListView listView, IReadOnlyList<AnalyzerResult> results)
        {
            listView.SuspendLayout();
            foreach (ListViewItem item in listView.Items) { item.Tag = null; }
            listView.Items.Clear();

            foreach (AnalyzerResult analyzerResult in results)
            {
                ListViewItem item = new ListViewItem(analyzerResult.ReferenceType.ToString());
                item.Tag = analyzerResult;

                if (analyzerResult.ReferenceType == AnalyzerReferenceType.Event)
                {
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, ManaMetadata.GetEventFriendlyName((ushort)analyzerResult.ReferenceIndex)));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, analyzerResult.AdditionalInformation));
                }
                else if (analyzerResult.ReferenceType == AnalyzerReferenceType.HardcodedEvent)
                {
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, analyzerResult.ReferenceIndex.ToString("X6")));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, analyzerResult.AdditionalInformation));
                }
                else if (analyzerResult.ReferenceType == AnalyzerReferenceType.SpriteObject)
                {
                    MapSpriteObject spriteObject = (MapSpriteObject)analyzerResult.ReferenceObject!;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, analyzerResult.ReferenceIndex.ToString("X4")));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, ManaMetadata.GetSpriteFriendlyName(spriteObject.SpriteIndex)));
                }
                else
                {
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, analyzerResult.ReferenceIndex.ToString("X4")));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, analyzerResult.AdditionalInformation));
                }

                listView.Items.Add(item);
            }

            listView.ResumeLayout();
        }

        private static void SwitchToSection(AnalyzerResult analyzerResult)
        {
            if (analyzerResult.ReferenceType == AnalyzerReferenceType.Event)
            {
                ManaMagicContext.Current.SwitchToSection(TreeViewSection.EventEditor, (int)analyzerResult.ReferenceIndex);
            }
            else if (analyzerResult.ReferenceType == AnalyzerReferenceType.SpriteObject)
            {
                ManaMagicContext.Current.SwitchToSection(TreeViewSection.MapEditor, (int)analyzerResult.ReferenceIndex);
            }
            else if (analyzerResult.ReferenceType == AnalyzerReferenceType.MapTrigger)
            {
                ManaMagicContext.Current.SwitchToSection(TreeViewSection.MapEditor, (int)analyzerResult.ReferenceIndex);
            }
        }
    }
}