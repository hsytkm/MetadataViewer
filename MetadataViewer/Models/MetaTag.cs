using System;

namespace MetadataViewer.Models
{
    record MetaTag
    {
        public string DirectoryName { get; }
        public int Type { get; }
        public string Name { get; }
        public string Description { get; }

        public MetaTag(MetadataExtractor.Tag tag)
        {
            DirectoryName = tag.DirectoryName;
            Type = tag.Type;
            Name = tag.HasName ? tag.Name : "no name";
            Description = tag.Description ?? "empty";
        }
    }
}
