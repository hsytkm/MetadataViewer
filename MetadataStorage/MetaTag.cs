﻿using System;
using UnitGenerator;

namespace MetadataStorage
{
    [UnitOf(typeof(int))]
    public readonly partial struct TagId { }

    public record MetaTag
    {
        public string DirectoryName { get; }
        public TagId Id { get; }
        public string Name { get; }
        public string Description { get; }
        public object? Data { get; }

        public MetaTag(MetadataExtractor.Tag tag, object? data)
        {
            DirectoryName = tag.DirectoryName;
            Id = new TagId(tag.Type);
            Name = tag.HasName ? tag.Name : "*Unknown*";
            Description = tag.Description ?? "*null*";
            Data = data;
        }
    }
}
