using MetadataExtractor;

namespace MetadataStorage;

/// <summary>Book=File</summary>
public sealed record MetaBook
{
    public enum Extensions
    {
        NotSupported, Jpeg, Bmp, Png, Tiff, Gif
    };

    public string FilePath { get; }
    public IReadOnlyList<MetaPage> Pages { get; }

    public MetaBook(string filePath)
    {
        FilePath = filePath;
        Pages = ReadMetaPages(filePath);
    }

    private static Extensions GetExtension(string filePath)
        => Path.GetExtension(filePath).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => Extensions.Jpeg,
            ".bmp" => Extensions.Bmp,
            ".png" => Extensions.Png,
            ".tif" or ".tiff" => Extensions.Tiff,
            ".gif" => Extensions.Gif,
            _ => Extensions.NotSupported,
        };

    private static bool IsSupportedExtension(string filePath)
        => GetExtension(filePath) is not Extensions.NotSupported;

    private static IReadOnlyList<MetaPage> ReadMetaPages(string filePath)
    {
       var directories = IsSupportedExtension(filePath)
            ? ImageMetadataReader.ReadMetadata(filePath)
            : null;

        return directories is not null
            ? directories.Select(d => new MetaPage(d)).ToArray()
            : [];
    }
}
