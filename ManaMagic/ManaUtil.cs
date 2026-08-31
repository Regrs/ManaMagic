using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ManaMagic.Core.Events;
using ManaMagic.Core.Events.OpCodes;
using ManaMagic.Core.Events.OpCodes.TextData;
using ManaMagic.Core.Metadata;

#nullable enable

namespace ManaMagic
{
    public static class ManaUtil
    {
        public static string GetNameFromEvent(ManaEvent manaEvent, out bool canEdit)
        {
            foreach (EventOpCode opCode in manaEvent.OpCodes)
            {
                if (opCode is ManaTextEventOpCode textOpCode)
                {
                    canEdit = true;
                    return textOpCode.ToString(false);
                }
            }
            canEdit = false;
            return "N/A";
        }

        public static byte SetFlagState(byte value, byte flag, bool set)
        {
            if (set)
            {
                value |= flag;
            }
            else
            {
                value &= (byte)~flag;
            }
            return value;
        }

        public static void BindComboBox<TKey, TValue>(ComboBox comboBox, IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            comboBox.DataSource = dictionary.ToList();
            comboBox.ValueMember = "Key";
            comboBox.DisplayMember = "Value";
            comboBox.SelectedIndex = 0;
        }

        public static void SetDoubleBuffered(this DataGridView dataGridView, bool doubleBuffered)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView, new object[] { doubleBuffered });
        }
    }
}