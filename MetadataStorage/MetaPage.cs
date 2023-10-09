namespace MetadataStorage;

public sealed record MetaPage
{
    public string Name { get; }
    public IReadOnlyList<MetaTag> Tags { get; }

    public MetaPage(MetadataExtractor.Directory directory)
    {
        Name = directory.Name;
        Tags = directory.Tags.Select(x => new MetaTag(x, directory.GetObject(x.Type))).ToArray();
    }
}
