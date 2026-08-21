#nullable enable

namespace ManaMagic
{
    public interface IApplicationNavigation
    {
        void SwitchToNode(TreeViewSection section);
        void SwitchToNode(TreeViewSection section, int index);
    }
}