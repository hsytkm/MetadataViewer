using MetadataViewer.Models;
using MetadataViewer.ViewModels.Records;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace MetadataViewer.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private readonly MetaModel _metaModel = new();
        public IReactiveProperty<string> DroppedPath { get; }
        public IReactiveProperty<string> FilePath { get; }
        public IReadOnlyReactiveProperty<IReadOnlyCollection<TagRecordViewModel>> Tags { get; }

        public MainWindowViewModel()
        {
            DroppedPath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged);
            FilePath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged);

            DroppedPath.Subscribe(x => FilePath.Value = x);

            Tags = FilePath
                .Where(x => File.Exists(x))
                .Select(x => _metaModel.GetTags(x).Select(x => new TagRecordViewModel(x)).ToArray())
                .ToReadOnlyReactivePropertySlim<IReadOnlyCollection<TagRecordViewModel>>();

            if (AssemblyState.IsDebugBuild) FilePath.Value = @"C:\data\official\Panasonic_DMC-GF7.jpg";
        }
    }
}
