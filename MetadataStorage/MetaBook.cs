using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataStorage
{
    public class MetaBook
    {
        //private static readonly MetaTag[] _emptyMetaTags = Array.Empty<MetaTag>();

        public IReadOnlyCollection<MetaTag> Tags { get; }

        public MetaBook(string filePath)
        {
            //var directories = ImageMetadataReader.ReadMetadata(filePath);
            var directories = JpegMetadataReader.ReadMetadata(filePath);
            Tags = directories.SelectMany(x => x.Tags.Select(x => new MetaTag(x))).ToArray();
        }
    }
}
