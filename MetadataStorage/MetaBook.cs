using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetadataStorage
{
    /// <summary>Book=File</summary>
    public class MetaBook
    {
        public enum Extensions
        {
            NotSupported, Jpeg, Bmp, Png, Tiff, Gif
        };

        public string FilePath { get; }
        public IReadOnlyCollection<MetaPage> Pages { get; }

        public MetaBook(string filePath)
        {
            FilePath = filePath;
            Pages = ReadMetaPages(filePath);
        }

        private static Extensions GetExtension(string filePath)
            => Path.GetExtension(filePath).ToLower() switch
            {
                ".jpg" or ".jpeg" => Extensions.Jpeg,
                ".bmp" => Extensions.Bmp,
                ".png" => Extensions.Png,
                ".tif" or ".tiff" => Extensions.Tiff,
                ".gif" => Extensions.Gif,
                _ => Extensions.NotSupported,
            };

        private static IReadOnlyCollection<MetaPage> ReadMetaPages(string filePath)
        {
            var directories = GetExtension(filePath) is Extensions.NotSupported
                ? null
                : ImageMetadataReader.ReadMetadata(filePath);

            return directories is not null
                ? directories.Select(d => new MetaPage(d)).ToArray()
                : Array.Empty<MetaPage>();
        }

        //public IEnumerable<MetaTag> GetAllTags() => Pages.SelectMany(x => x.Tags);
        //public IEnumerable<string> GetPagesName() => Pages.Select(x => x.Name);

    }
}
