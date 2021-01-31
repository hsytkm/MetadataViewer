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
        public enum Extensions
        {
            NotSupported, Jpeg, Bmp, Png, Tiff, Gif
        };
        private Extensions Extension { get; }

        public string FilePath { get; }
        public IReadOnlyCollection<MetaPage> Pages { get; }

        public MetaBook(string filePath)
        {
            FilePath = filePath;
            Extension = GetExtension(filePath);
            Pages = ReadMetaPages(filePath, Extension);
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

        private static IReadOnlyCollection<MetaPage> ReadMetaPages(string path, Extensions extension)
        {
            var directories = extension switch
            {
                Extensions.Jpeg => JpegMetadataReader.ReadMetadata(path),
                Extensions.Bmp => ImageMetadataReader.ReadMetadata(path),
                Extensions.Png => ImageMetadataReader.ReadMetadata(path),
                Extensions.Tiff => ImageMetadataReader.ReadMetadata(path),
                Extensions.Gif => ImageMetadataReader.ReadMetadata(path),
                Extensions.NotSupported => null,
                _ => throw new NotImplementedException(),
            };

            return directories is not null
                ? directories.Select(d => new MetaPage(d)).ToArray()
                : Array.Empty<MetaPage>();
        }

        public IEnumerable<MetaTag> GetAllTags() => Pages.SelectMany(x => x.Tags);
        public IEnumerable<string> GetPagesName() => Pages.Select(x => x.Name);

    }
}
