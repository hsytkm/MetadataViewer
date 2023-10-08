using MetadataViewer.Core;
using System.Collections.Immutable;

namespace MetadataViewer.ViewModels;

/// <summary>
/// メタページ（Exif / MakerNote など）のコレクションです。
/// DataGrid の ItemsSource になります。
/// </summary>
internal sealed record MetaTagsPageViewModel : ICompositeColoredTextCollection<MetaTagItemViewModel>
{
    public string Name { get; }
    public IImmutableList<MetaTagItemViewModel> Collection { get; }

    public MetaTagsPageViewModel(string name, IEnumerable<MetaTagItemViewModel> items)
    {
        Name = name;
        Collection = ImmutableArray.CreateRange(items);
    }

    public MetaTagsPageViewModel(MetadataStorage.MetaPage page)
        : this(page.Name, page.Tags.Select(x => new MetaTagItemViewModel(x)))
    { }

}
