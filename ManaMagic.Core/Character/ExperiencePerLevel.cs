using ZwellTech;

#nullable enable

namespace ManaMagic.Core.Character
{
    public sealed record ExperiencePerLevel : NotifyRecordPropertyChanged
    {
        private uint experience = 0;

        public uint Experience
        {
            get { return this.experience; }
            set { this.SetProperty(ref this.experience, value); }
        }

        public ExperiencePerLevel(byte index, uint experience, bool userModified = false) : base(index, userModified)
        {
            this.experience = experience;
        }
    }
}