using MetadataViewer.Models;
using MetadataViewer.ViewModels.Records;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Data;

namespace MetadataViewer.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private readonly MetaModel _metaModel = new();
        public IReactiveProperty<string> DroppedPath { get; }
        public IReactiveProperty<string> FilePath { get; }
        public IReadOnlyReactiveProperty<IReadOnlyCollection<TagRecordViewModel>> Tags { get; }

        public IReactiveProperty<string> FilterWord { get; }
        public ReactiveCommand<string> FilterTagCommand { get; }

        public MainWindowViewModel()
        {
            DroppedPath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged);
            FilePath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged);
            FilterWord = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged);

            DroppedPath.Subscribe(x => FilePath.Value = x);

            Tags = FilePath
                .Where(x => File.Exists(x))
                .Select(x => _metaModel.GetTags(x).Select(x => new TagRecordViewModel(x)).ToArray())
                .Do(tags => FilterTagRecord(tags, FilterWord.Value))    // filter when tags changed
                .ToReadOnlyReactivePropertySlim<IReadOnlyCollection<TagRecordViewModel>>();

            FilterTagCommand = Tags.Select(x => x?.Any() ?? false)
                .ToReactiveCommand<string>()
                .WithSubscribe(word => FilterTagRecord(Tags?.Value, word));

            if (AssemblyState.IsDebugBuild) FilePath.Value = @"C:\data\official\Panasonic_DMC-GF7.jpg";
        }

        private static void FilterTagRecord(IReadOnlyCollection<TagRecordViewModel>? source, string word)
        {
            if (source is null) return;

            var collectionView = CollectionViewSource.GetDefaultView(source);
            collectionView.Filter = string.IsNullOrEmpty(word)
                ? _ => true   // clear
                : x => (x as TagRecordViewModel).IsContains(word);
        }
    }
}
