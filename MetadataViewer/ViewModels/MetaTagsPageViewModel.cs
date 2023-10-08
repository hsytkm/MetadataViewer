using MetadataViewer.Core;
using System.Collections.Immutable;

namespace MetadataViewer.ViewModels;

/// <summary>
/// メタページ（Exif / MakerNote など）のコレクションです。
/// DataGrid の ItemsSource になります。
/// </summary>
internal sealed record MetaTagsPageViewModel : ICompositeColoredTextCollection
{
    public string Name { get; }
    public IReadOnlyList<MetaTagItemViewModel> ItemsSource { get; }
    public IReadOnlyList<ICompositeColoredText> ColoredTexts => ItemsSource;

    public MetaTagsPageViewModel(string name, IEnumerable<MetaTagItemViewModel> items)
    {
        Name = name;
        ItemsSource = items.ToArray();
    }

    public MetaTagsPageViewModel(MetadataStorage.MetaPage page)
        : this(page.Name, page.Tags.Select(x => new MetaTagItemViewModel(x)))
    { }
}
