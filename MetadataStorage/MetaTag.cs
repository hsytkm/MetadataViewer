using System;

namespace MetadataStorage
{
    public record MetaTag
    {
        public string PageName { get; }
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public object? Data { get; }

        public MetaTag(MetadataExtractor.Tag tag, object? data)
        {
            PageName = tag.DirectoryName;
            Id = tag.Type;
            Name = tag.HasName ? tag.Name : "*Unknown*";
            Description = tag.Description ?? "*null*";
            Data = data;
        }
    }
}
