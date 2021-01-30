using System;

namespace MetadataViewer.ViewModels.Records
{
    record TagRecordViewModel
    {
        public string Group { get; }
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        public TagRecordViewModel(MetadataStorage.MetaTag tag)
        {
            Group = tag.DirectoryName;
            Id = (int)tag.Id;
            Name = tag.Name;
            Description = tag.Description;
        }
    }
}
