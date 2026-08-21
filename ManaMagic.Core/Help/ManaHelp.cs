using System.IO;
using System.Reflection;
using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Help
{
    public static class ManaHelp
    {
        private const string SpriteSlotMemoryMapHelpFile = "SpriteSlotMemoryMap.rtf";
        private const string SpecialValuesHelpFile = "SpecialValues.rtf";
        private const string CutContentHelpFile = "CutContent.rtf";

        public static string GetHelpFile(ManaHelpTopic topic)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string? assemblyName = assembly.GetName().Name;
            string fileName = ManaHelp.GetTopicFileName(topic);
            string resourcePath = $"{assemblyName}.Help.{fileName}";

            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);
            using StreamReader reader = new StreamReader(stream!);

            return reader.ReadToEnd();
        }

        private static string GetTopicFileName(ManaHelpTopic topic)
        {
            switch (topic)
            {
                case ManaHelpTopic.SpriteSlotMemoryMap: return ManaHelp.SpriteSlotMemoryMapHelpFile;
                case ManaHelpTopic.SpecialValues: return ManaHelp.SpecialValuesHelpFile;
                case ManaHelpTopic.CutContent: return ManaHelp.CutContentHelpFile;
                default: return (string)ThrowHelper.ThrowArgumentException("Unknown help topic.", nameof(topic));

            }
        }
    }

    public enum ManaHelpTopic
    {
        SpriteSlotMemoryMap,
        SpecialValues,
        CutContent,
    }
}

