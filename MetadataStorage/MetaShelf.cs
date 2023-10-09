using System.Collections.Concurrent;
using System.Reflection;

namespace MetadataStorage;

public sealed class MetaShelf
{
    private readonly ConcurrentDictionary<string, MetaBook> _booksDict = new();

    public MetaShelf() { }

    public MetaBook GetOrAdd(string filePath)
    {
        if (!_booksDict.TryGetValue(filePath, out var book))
        {
            var sw = Stopwatch.StartNew();
            book = new MetaBook(filePath);
            sw.Stop();

            _ = _booksDict.TryAdd(filePath, book);
            Debug.WriteLine($"ReadMeta: {sw.ElapsedMilliseconds} msec");
        }
        return book;
    }

    public static string GetMetadataExtractorVersion()
        => Assembly.GetAssembly(typeof(MetadataExtractor.Tag))?.GetName().Version?.ToString() ?? "";
}
