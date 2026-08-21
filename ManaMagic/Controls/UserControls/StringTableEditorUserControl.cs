using System;
using System.Windows.Forms;
using ManaMagic.Core;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Controls.UserControls
{
    public partial class StringTableEditorUserControl : UserControl
    {
        private readonly StringTableEditType stringTableEditType = StringTableEditType.None;
        private DataTable<ManaEvent>? stringTable = null;
        private bool isShopTable = false;

        public StringTableEditorUserControl()
        {
            InitializeComponent();
        }

        public StringTableEditorUserControl(StringTableEditType stringTableEditType)
        {
            InitializeComponent();
            this.stringTableEditType = stringTableEditType;
            this.nameColumn.MaxInputLength = Constants.TextMaxLength;
        }

        private void SetStringTable(DataTable<ManaEvent> eventTable, bool isShopTable = false)
        {
            this.dataGridView.Rows.Clear();
            this.stringTable = eventTable;
            this.isShopTable = isShopTable;

            int index = 0;
            foreach (ManaEvent manaEvent in this.stringTable)
            {
                bool added = false;
                bool skipped = false;
                foreach (EventOpCode opCode in manaEvent.OpCodes)
                {
                    if (opCode is ManaTextEventOpCode textOpCode)
                    {
                        if (this.isShopTable && !skipped)
                        {
                            // Shop messages start with a new line op code we need to skip over.
                            skipped = true;
                            continue;
                        }

                        this.dataGridView.Rows.Add(index.ToString("X2"), textOpCode.ToString(false));
                        index++;
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    this.dataGridView.Rows.Add(index.ToString("X2"), string.Empty);
                    index++;
                }
            }
        }

        private void StringTableEditorUserControl_Load(object sender, EventArgs e)
        {
            switch (this.stringTableEditType)
            {
                case StringTableEditType.TownNames:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.TownNameTable);
                    break;
                case StringTableEditType.EnemyNames:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.EnemyNameTable);
                    break;
                case StringTableEditType.ItemErrors:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.ItemErrorMessageTable);
                    break;
                case StringTableEditType.StatusEffectMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.StatusEffectMessageTable);
                    break;
                case StringTableEditType.ElementalFearMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.ElementalFearMessageTable);
                    break;
                case StringTableEditType.TrapMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.TrapMessageTable);
                    break;
                case StringTableEditType.WeaponNameMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.WeaponNameMessageTable);
                    break;
                case StringTableEditType.BossSkillMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.BossSkillNameMessageTable);
                    break;
                case StringTableEditType.BuffDebuffMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.BuffDebuffMessageTable);
                    break;
                case StringTableEditType.LunarMagicMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.LunarMagicMessageTable);
                    break;
                case StringTableEditType.MiscellaneousMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.MiscellaneousMessageTable);
                    break;
                case StringTableEditType.TreasureChestMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.TreasureChestMessageTable);
                    break;
                case StringTableEditType.CombatMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.CombatMessageTable);
                    break;
                case StringTableEditType.AnalyzerMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.AnalyzerMessageTable);
                    break;
                case StringTableEditType.LevelUpMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.LevelUpMessageTable);
                    break;
                case StringTableEditType.ShopMessages:
                    this.SetStringTable(ManaMagicContext.Current.Context.TextContext.ShopMessageTable, true);
                    break;
            }
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.stringTable != null && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (this.dataGridView[nameColumn.Index, e.RowIndex].Value is string value)
                {
                    ManaEvent manaEvent = this.stringTable[e.RowIndex];
                    if (manaEvent.OpCodes.Count == 1 && manaEvent.OpCodes[0].OperationCode == EventOpCodeType.End)
                    {
                        ManaTextEventOpCode textOpCode = (ManaTextEventOpCode)EventOpCodeFactory.CreateText(SecretOfManaEncoding.English.GetBytes(value), false);
                        manaEvent.Insert(0, textOpCode);
                    }
                    else
                    {
                        bool skipped = false;
                        foreach (EventOpCode opCode in manaEvent.OpCodes)
                        {
                            if (opCode is ManaTextEventOpCode textOpCode)
                            {
                                if (this.isShopTable && !skipped)
                                {
                                    // Shop messages start with a new line op code we need to skip over.
                                    skipped = true;
                                    continue;
                                }
                                textOpCode.TextData = SecretOfManaEncoding.English.GetBytes(value);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public enum StringTableEditType
    {
        None,
        TownNames,
        EnemyNames,
        ItemErrors,
        StatusEffectMessages,
        ElementalFearMessages,
        TrapMessages,
        WeaponNameMessages,
        BossSkillMessages,
        BuffDebuffMessages,
        LunarMagicMessages,
        MiscellaneousMessages,
        TreasureChestMessages,
        CombatMessages,
        AnalyzerMessages,
        ShopMessages,
        LevelUpMessages,
    }
}