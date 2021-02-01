using MetadataViewer.Models;
using MetadataViewer.ViewModels.Records;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MetadataViewer.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public IReactiveProperty<string> FilePath { get; }
        public IReactiveProperty<string> DroppedPath { get; }
        public IReadOnlyReactiveProperty<IReadOnlyCollection<MetaPageRecordViewModel>> Pages { get; }

        public MainWindowViewModel(MetaModel metaModel)
        {
            var disposables = new CompositeDisposable();    // undisposed

            FilePath = metaModel.FilePath;
            DroppedPath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged).AddTo(disposables);
            DroppedPath.Subscribe(x => FilePath.Value = x).AddTo(disposables);

            Pages = metaModel.SelectedBook
                .Where(x => x is not null)
                .Select(x => CreateMetaPages(x!).ToArray())
                .ToReadOnlyReactivePropertySlim<IReadOnlyCollection<MetaPageRecordViewModel>>()
                .AddTo(disposables);

            if (AssemblyState.IsDebugBuild) FilePath.Value = @"C:\data\official\Panasonic_DMC-GF7.jpg";
        }

        private static IEnumerable<MetaPageRecordViewModel> CreateMetaPages(MetadataStorage.MetaBook book)
        {
            var pages = book.Pages.Select(p => new MetaPageRecordViewModel(p)).ToArray();
            var all = new MetaPageRecordViewModel("All", pages.SelectMany(x => x.Tags));
            return pages.Prepend(all);
        }
    }
}
