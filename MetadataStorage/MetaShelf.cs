using System.Collections.Concurrent;
using System.Reflection;

namespace MetadataStorage;

public sealed class MetaShelf
{
    private readonly IDictionary<string, MetaBook> _booksDict = new ConcurrentDictionary<string, MetaBook>();

    public MetaShelf() { }

    public MetaBook GetOrAdd(string filePath)
    {
        if (!_booksDict.TryGetValue(filePath, out var value))
        {
            var sw = Stopwatch.StartNew();

            value = new MetaBook(filePath);
            _booksDict.Add(filePath, value);

            sw.Stop();
            Debug.WriteLine($"ReadMeta: {sw.ElapsedMilliseconds} msec");
        }
        return value;
    }

    public static string GetMetadataExtractorVersion()
        => Assembly.GetAssembly(typeof(MetadataExtractor.Tag))?.GetName().Version?.ToString() ?? "";
}
