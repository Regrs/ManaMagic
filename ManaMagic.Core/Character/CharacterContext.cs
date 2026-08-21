using ManaMagic.Core.RomReaders;
using ZwellTech.SuperNintendo;

#nullable enable

namespace ManaMagic.Core.Character
{
    public sealed class CharacterContext
    {
        public DataTable<CharacterStatsByLevel> RandiStatsByLevelTable { get; private set; } = DataTable<CharacterStatsByLevel>.Empty;
        public DataTable<CharacterStatsByLevel> PurimStatsByLevelTable { get; private set; } = DataTable<CharacterStatsByLevel>.Empty;
        public DataTable<CharacterStatsByLevel> PopoieStatsByLevelTable { get; private set; } = DataTable<CharacterStatsByLevel>.Empty;
        public DataTable<ExperiencePerLevel> ExperiencePerLevelTable { get; private set; } = DataTable<ExperiencePerLevel>.Empty;
        public ManaDefaultCharacterData DefaultCharacterDataTable { get; private set; } = ManaDefaultCharacterData.Empty;

        /// <summary>
        /// Initializes the context by reading character data from the ROM file.
        /// </summary>
        public void Initialize()
        {
            CharacterRomReader reader = RomReaderFactory.GetRomReader<CharacterRomReader>();

            this.RandiStatsByLevelTable = reader.ReadRandiStatsByLevelTable();
            this.PurimStatsByLevelTable = reader.ReadPurimStatsByLevelTable();
            this.PopoieStatsByLevelTable = reader.ReadPopoieStatsByLevelTable();
            this.ExperiencePerLevelTable = reader.ReadExperiencePerLevelTable();
            this.DefaultCharacterDataTable = reader.ReadDefaultCharacterDataTable();
        }

        public void SetDefaultCharacterData(ManaDefaultCharacterData defaultData)
        {
            this.DefaultCharacterDataTable = defaultData;
        }
    }
}