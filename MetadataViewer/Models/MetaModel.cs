using MetadataStorage;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MetadataViewer.Models;

internal sealed class MetaModel
{
    private readonly MetaShelf _metaShelf = new();

    public IReactiveProperty<string> FilePath { get; }
    public IReadOnlyReactiveProperty<MetaBook?> SelectedBook { get; }

    public MetaModel()
    {
        var disposables = new CompositeDisposable();    // undisposed

        FilePath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged).AddTo(disposables);

        SelectedBook = FilePath
            .Select(x => !File.Exists(x) ? null : _metaShelf.GetOrAdd(x))
            .ToReadOnlyReactivePropertySlim()
            .AddTo(disposables);
    }

    public static string GetMetadataExtractorVersion() => MetaShelf.GetMetadataExtractorVersion();
}
