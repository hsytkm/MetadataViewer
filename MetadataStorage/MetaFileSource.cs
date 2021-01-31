using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetadataStorage
{
    internal record MetaFileSource
    {
        public enum Extensions
        {
            NotSupported, Jpeg, Bmp, Png, Tiff, Gif
        };

        public string FilePath { get; }
        public Extensions Extension { get; }

        public MetaFileSource(string path)
        {
            if (!File.Exists(path)) throw new NotImplementedException();

            FilePath = path;
            Extension = GetExtension(path);
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

        private static IReadOnlyCollection<MetaTag> ReadMetaTags(string path, Extensions extension)
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
                ? directories.SelectMany(x => x.Tags.Select(x => new MetaTag(x))).ToArray()
                : Array.Empty<MetaTag>();
        }

        public IReadOnlyCollection<MetaTag> ReadMetaTags() => ReadMetaTags(FilePath, Extension);

    }
}
