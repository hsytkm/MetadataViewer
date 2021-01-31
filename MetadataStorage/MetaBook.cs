using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetadataStorage
{
    public class MetaBook
    {
        internal MetaFileSource FileSource { get; }
        public IReadOnlyCollection<MetaTag> Tags { get; }

        public MetaBook(string filePath)
        {
            var file = new MetaFileSource(filePath);

            FileSource = file;
            Tags = file.ReadMetaTags();
        }

    }
}
