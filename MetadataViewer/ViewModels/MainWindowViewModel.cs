using MetadataViewer.Models;
using MetadataViewer.ViewModels.Records;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Data;

namespace MetadataViewer.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public IReactiveProperty<string> DroppedPath { get; }
        public IReactiveProperty<string> FilePath { get; }
        public IReadOnlyReactiveProperty<IReadOnlyCollection<MetaTagRecordViewModel>> Tags { get; }
        public IReadOnlyReactiveProperty<IReadOnlyCollection<MetaPageRecordViewModel>> Pages { get; }

        public IReactiveProperty<string> FilterWord { get; }
        public ReactiveCommand<string> FilterTagCommand { get; }

        public MainWindowViewModel(MetaModel metaModel)
        {
            var disposables = new CompositeDisposable();

            DroppedPath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged).AddTo(disposables);
            FilterWord = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged).AddTo(disposables);
            FilePath = metaModel.FilePath;

            DroppedPath.Subscribe(x => FilePath.Value = x).AddTo(disposables);

            Pages = metaModel.SelectedBook
                .Where(x => x is not null)
                .Select(x => CreateMetaPages(x!).ToArray())
                .ToReadOnlyReactivePropertySlim<IReadOnlyCollection<MetaPageRecordViewModel>>()
                .AddTo(disposables);

            Tags = metaModel.SelectedBook
                .Where(x => x is not null)
                .Select(x => x!.GetAllTags().Select(x => new MetaTagRecordViewModel(x)).ToArray())
                .Do(tags => FilterTagRecord(tags, FilterWord.Value))    // filter when tags changed
                .ToReadOnlyReactivePropertySlim<IReadOnlyCollection<MetaTagRecordViewModel>>()
                .AddTo(disposables);

            FilterTagCommand = Tags.Select(x => x?.Any() ?? false)
                .ToReactiveCommand<string>()
                .WithSubscribe(word => FilterTagRecord(Tags?.Value, word), disposables.Add);

            if (AssemblyState.IsDebugBuild) FilePath.Value = @"C:\data\official\Panasonic_DMC-GF7.jpg";
        }

        private static IEnumerable<MetaPageRecordViewModel> CreateMetaPages(MetadataStorage.MetaBook book)
        {
            var pages = book.Pages.Select(p => new MetaPageRecordViewModel(p)).ToList();    // ◆TempList使いたい
            var all = new MetaPageRecordViewModel("All", pages.SelectMany(x => x.Tags));

            return pages.Prepend(all);
        }

        private static void FilterTagRecord(IReadOnlyCollection<MetaTagRecordViewModel>? source, string word)
        {
            if (source is null) return;

            var collectionView = CollectionViewSource.GetDefaultView(source);
            collectionView.Filter = string.IsNullOrEmpty(word)
                ? _ => true   // clear
                : x => (x as MetaTagRecordViewModel).IsContains(word);
        }
    }
}
