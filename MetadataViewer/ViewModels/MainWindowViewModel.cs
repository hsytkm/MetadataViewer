using MetadataViewer.Common;
using MetadataViewer.Models;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MetadataViewer.ViewModels;

internal sealed class MainWindowViewModel : BindableBase
{
    public string MetadataExtractorVersion { get; } = MetaModel.GetMetadataExtractorVersion();
    public IReactiveProperty<string> FilePath { get; }
    public IReactiveProperty<string> DroppedPath { get; }
    public IReadOnlyReactiveProperty<CollectionSelectedItemPair<MetaTagsPageViewModel>> MetaPages { get; }

    public MainWindowViewModel(MetaModel metaModel)
    {
        var disposables = new CompositeDisposable();    // undisposed

        FilePath = metaModel.FilePath;
        DroppedPath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged).AddTo(disposables);
        DroppedPath.Subscribe(x => FilePath.Value = x).AddTo(disposables);

        MetaPages = metaModel.SelectedBook
            .Where(x => x is not null)
            .Select(x => CollectionSelectedItemPair.Create(CreateMetaPageVms(x!)))
            .ToReadOnlyReactivePropertySlim()
            .AddTo(disposables);

#if DEBUG
        FilePath.Value = @"D:\data\Image0.JPG";
#endif
    }

    private static IEnumerable<MetaTagsPageViewModel> CreateMetaPageVms(MetadataStorage.MetaBook book)
    {
        var pages = book.Pages.Select(p => new MetaTagsPageViewModel(p)).ToArray();
        var all = new MetaTagsPageViewModel("All", pages.SelectMany(x => x.ItemsSource));
        return pages.Prepend(all);
    }
}
