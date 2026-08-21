using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.System;
using ManaMagic.Core.Events.OpCodes.TextData;
using ManaMagic.Core.Metadata;
using ZwellTech;
using ZwellTech.Logging;

#nullable enable

namespace ManaMagic.Controls.UserControls.Events
{
    public partial class EventEditorUserControl : UserControl, IManaControl
    {
        private bool isListBoxDragDropInProgress = false;
        private ManaEvent? manaEvent = null;

        public EventEditorUserControl()
        {
            InitializeComponent();
            InitializeTreeView();
            this.eventViewerNumericUpDown.Maximum = Constants.Bank0A.EventIdMaximum;
            this.toolTip.SetToolTip(this.moveUpButton, "Moves the selected op-code up.");
            this.toolTip.SetToolTip(this.moveDownButton, "Moves the selected op-code down.");
        }

        public void SetIndex(int index)
        {
            this.eventViewerNumericUpDown.Value = index;
        }

        private void InitializeTreeView()
        {
            // These are all covered by the '1' version of the type. No need to add what would look like a bunch of duplicates to the treeview.
            EventOpCodeType[] ignoreList = new EventOpCodeType[]
            {
                EventOpCodeType.JumpToEvent2, EventOpCodeType.JumpToEvent3, EventOpCodeType.JumpToEvent4, EventOpCodeType.JumpToEvent5, EventOpCodeType.JumpToEvent6, EventOpCodeType.JumpToEvent7, EventOpCodeType.JumpToEvent8,
                EventOpCodeType.UseDoor2, EventOpCodeType.UseDoor3, EventOpCodeType.UseDoor4,
                EventOpCodeType.CallEvent2, EventOpCodeType.CallEvent3, EventOpCodeType.CallEvent4, EventOpCodeType.CallEvent5, EventOpCodeType.CallEvent6, EventOpCodeType.CallEvent7, EventOpCodeType.CallEvent8,
            };

            foreach (EventOpCodeCommandType commandType in Enum.GetValues<EventOpCodeCommandType>())
            {
                TreeNode node = new TreeNode(commandType.GetDisplayName());
                foreach (KeyValuePair<EventOpCodeType, Type> kvp in EventUtil.GetEventOpCodeDictionary())
                {
                    if (!ignoreList.Contains(kvp.Key))
                    {
                        Type type = kvp.Value;
                        EventOpCodeCommandTypeAttribute? commandAttribute = EventUtil.GetEventOpCodeCommandTypeAttribute(type);
                        if (commandAttribute != null && commandAttribute.CommandType == commandType)
                        {
                            EventOpCodeTypeAttribute? opCodeAttribute = EventUtil.GetEventOpCodeTypeAttribute(type);
                            if (opCodeAttribute != null && !ignoreList.Contains(opCodeAttribute.OperationCode))
                            {
                                TreeNode opCodeNode = new TreeNode(opCodeAttribute.OperationCode.GetDisplayName());
                                opCodeNode.Tag = opCodeAttribute.OperationCode;
                                opCodeNode.ToolTipText = EventUtil.GetEventDescription(type);
                                node.Nodes.Add(opCodeNode);
                            }
                        }
                    }
                }
                eventOpCodeTreeView.Nodes.Add(node);
            }
            this.opCodeNameLabel.Text = string.Empty;
            this.opCodeDescriptionLabel.Text = string.Empty;
        }

        private void SetEvent()
        {
            if (this.manaEvent != null)
            {
                this.manaEvent.PropertyChanged -= this.ManaEvent_PropertyChanged;
            }
            this.manaEvent = ManaMagicContext.Current.Context.EventContext.Events[(int)this.eventViewerNumericUpDown.Value];
            this.manaEvent.PropertyChanged += this.ManaEvent_PropertyChanged;
            this.eventNameLabel.Text = ManaMetadata.GetEventFriendlyName((ushort)this.eventViewerNumericUpDown.Value, false);

            this.eventViewerListBox.SuspendLayout();
            this.eventViewerListBox.SelectedItem = null;
            this.eventViewerListBox.Items.Clear();
            foreach (EventOpCode opCode in manaEvent)
            {
                this.eventViewerListBox.Items.Add(opCode);
            }
            this.eventViewerListBox.ResumeLayout();
        }

        private void ManaEvent_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.eventViewerListBox.Invalidate();
        }

        private void MoveItem(int index, EventOpCode opCode)
        {
            if (this.manaEvent != null)
            {
                this.eventViewerListBox.SuspendLayout();
                this.eventViewerListBox.Items.Remove(opCode);
                this.eventViewerListBox.Items.Insert(index, opCode);
                this.eventViewerListBox.ResumeLayout();

                this.manaEvent.Remove(opCode);
                this.manaEvent.Insert(index, opCode);

                this.moveUpButton.Enabled = this.eventViewerListBox.SelectedIndex > 0;
                this.moveDownButton.Enabled = this.eventViewerListBox.SelectedIndex < this.eventViewerListBox.Items.Count - 1;

                this.eventViewerListBox.SelectedItem = opCode;
            }
        }

        #region Event Handlers

        #region EventViewerListBox

        private void EventViewerListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                EventOpCode ev = ((EventOpCode)eventViewerListBox.Items[e.Index]);

                using SolidBrush sb = new SolidBrush(ev.Color); 
                e.Graphics.DrawString(ev.ToString(), this.eventViewerListBox.Font, sb, e.Bounds);
            }
        }

        private void EventViewerListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.Items[e.Index] is TextDataEventOpCode textDataEventOpCode)
                {
                    e.ItemHeight *= textDataEventOpCode.LineCount;
                }
            }
        }

        private void EventViewerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = null;
            if (this.eventViewerListBox.SelectedItem is EventOpCode opCode)
            {
                propertyGrid.SelectedObject = opCode;
                this.moveUpButton.Enabled = this.eventViewerListBox.SelectedIndex > 0;
                this.moveDownButton.Enabled = this.eventViewerListBox.SelectedIndex < this.eventViewerListBox.Items.Count - 1;
            }
            else
            {
                this.moveUpButton.Enabled = false;
                this.moveDownButton.Enabled = false;
            }
        }

        private void EventViewerListBox_DragEnter(object sender, DragEventArgs e)
        {
            object dragObject = e.Data.GetData(typeof(EventOpCodeType));
            if (dragObject != null)
            {
                e.Effect = e.AllowedEffect;
                return;
            }

            dragObject = e.Data.GetData(typeof(DragDropInfo));
            var thing = e.Data.GetFormats();
            if (dragObject is DragDropInfo dragDropInfo && dragDropInfo.HasValue)
            {
                e.Effect = DragDropEffects.Scroll | DragDropEffects.Move;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        private void EventViewerListBox_DragDrop(object sender, DragEventArgs e)
        {
            int index = this.eventViewerListBox.IndexFromPoint(this.eventViewerListBox.PointToClient(new Point(e.X, e.Y)));
            if (index == ListBox.NoMatches)
            {
                index = this.eventViewerListBox.Items.Count - 1;
            }

            object dragObject = e.Data.GetData(typeof(EventOpCodeType));
            if (dragObject is EventOpCodeType opCodeType && this.manaEvent != null)
            {
                EventOpCode eventOpCode = EventOpCodeFactory.Create(opCodeType);

                this.eventViewerListBox.Items.Insert(index, eventOpCode);
                this.manaEvent.Insert(index, eventOpCode);
                return;
            }

            dragObject = e.Data.GetData(typeof(DragDropInfo));
            if (dragObject is DragDropInfo dragDropInfo && dragDropInfo.HasValue && this.manaEvent != null)
            {
                this.eventViewerListBox.Items.Remove(dragDropInfo.Value);
                this.eventViewerListBox.Items.Insert(index, dragDropInfo.Value);

                this.manaEvent.Remove(dragDropInfo.Value);
                this.manaEvent.Insert(index, dragDropInfo.Value);
                return;
            }
        }

        private void EventViewerListBox_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (!this.eventViewerListBox.ClientRectangle.Contains(this.eventViewerListBox.PointToClient(Control.MousePosition)))
            {
                e.Action = DragAction.Cancel;
                this.isListBoxDragDropInProgress = false;
            }
            else if (!Control.MouseButtons.HasFlag(MouseButtons.Left))
            {
                e.Action = DragAction.Drop;
                this.isListBoxDragDropInProgress = false;
            }
        }

        private void EventViewerListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isListBoxDragDropInProgress)
            {
                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                {
                    if (this.eventViewerListBox.SelectedItem is EventOpCode opCode)
                    {
                        this.isListBoxDragDropInProgress = true;
                        this.eventViewerListBox.DoDragDrop(new DragDropInfo(opCode), DragDropEffects.Scroll | DragDropEffects.Move);
                    }
                }
            }
        }

        #endregion

        #region EventOpCodeTreeView

        private void EventOpCodeTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (this.eventOpCodeTreeView.SelectedNode?.Tag is EventOpCodeType opCodeType)
            {
                eventOpCodeTreeView.DoDragDrop(opCodeType, DragDropEffects.Scroll | DragDropEffects.Copy);
            }
        }

        private void EventOpCodeTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is EventOpCodeType opCodeType)
            {
                this.opCodeNameLabel.Text = opCodeType.GetDisplayName();
                this.opCodeDescriptionLabel.Text = e.Node.ToolTipText;
                return;
            }
            this.opCodeNameLabel.Text = string.Empty;
            this.opCodeDescriptionLabel.Text = string.Empty;
        }

        #endregion

        #region ContextMenu

        private void EventViewerContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.viewEventToolStripMenuItem.Enabled = (this.eventViewerListBox.SelectedItem is JumpToEventOpCode) || (this.eventViewerListBox.SelectedItem is CallEventOpCode);
            this.deleteToolStripMenuItem.Enabled = this.eventViewerListBox.SelectedItem != null;
        }

        private void ViewEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.eventViewerListBox.SelectedItem is JumpToEventOpCode jumpToEventOpCode)
            {
                this.eventViewerNumericUpDown.Value = jumpToEventOpCode.EventId;
            }
            else if (this.eventViewerListBox.SelectedItem is CallEventOpCode callEventOpCode)
            {
                this.eventViewerNumericUpDown.Value = callEventOpCode.EventId;
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.eventViewerListBox.SelectedItem is EventOpCode eventOpCode && this.manaEvent != null)
            {
                this.manaEvent.Remove(eventOpCode);
                this.eventViewerListBox.Items.Remove(eventOpCode);
            }
        }

        #endregion

        private void EventViewerUserControl_Load(object sender, EventArgs e)
        {
            this.SetEvent();
        }

        private void EventViewerNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.SetEvent();
        }

        private void SplitContainer2_Panel1_SizeChanged(object sender, EventArgs e)
        {
            this.panel1.Location = new Point(this.eventOpCodeTreeView.Location.X, this.eventOpCodeTreeView.Location.Y + 10);
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            if (this.eventViewerListBox.SelectedItem is EventOpCode opCode && this.manaEvent != null)
            {
                int index = this.eventViewerListBox.SelectedIndex - 1;
                this.MoveItem(index, opCode);
            }
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            if (this.eventViewerListBox.SelectedItem is EventOpCode opCode)
            {
                int index = this.eventViewerListBox.SelectedIndex + 1;
                this.MoveItem(index, opCode);
            }
        }

        #endregion

        private readonly struct DragDropInfo
        {
            public EventOpCode? Value { get; }

            [MemberNotNullWhen(true, nameof(Value))]
            public bool HasValue { get { return this.Value != null; } }

            public DragDropInfo(EventOpCode opCode)
            {
                this.Value = opCode;
            }
        }
    }
}