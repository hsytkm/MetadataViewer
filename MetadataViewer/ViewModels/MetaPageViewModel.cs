using MetadataViewer.Core;
using System.ComponentModel;

namespace MetadataViewer.ViewModels;

/// <summary>
/// メタページ（Exif / MakerNote など）のコレクションです。
/// DataGrid の ItemsSource になります。
/// </summary>
internal sealed class MetaPageViewModel : INotifyPropertyChanged, ICompositeColoredTextsList
{
    public string Name { get; }
    public IReadOnlyList<MetaTagItemViewModel> ItemsSource { get; }

    /// <inheritdoc/>
    public IReadOnlyList<ICompositeColoredText> ColoredTexts => ItemsSource;

    public MetaPageViewModel(string name, IEnumerable<MetaTagItemViewModel> items)
    {
        Name = name;
        ItemsSource = items.ToArray();
    }

    public MetaPageViewModel(MetadataStorage.MetaPage page)
        : this(page.Name, page.Tags.Select(x => new MetaTagItemViewModel(x)))
    { }

#pragma warning disable CS0067 // The event 'MetaTagsPageViewModel.PropertyChanged' is never used
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067 // The event 'MetaTagsPageViewModel.PropertyChanged' is never used
}
