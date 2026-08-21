using System.Windows.Forms;

#nullable enable

namespace ManaMagic
{
    public sealed class ManaTreeNode : TreeNode
    {
        public TreeViewSection Section { get; }

        public ManaTreeNode(string text, TreeViewSection section) : base(text)
        {
            this.Section = section;
        }
    }
}