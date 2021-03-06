﻿using MetadataViewer.Models;
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
        public string MetadataExtractorVersion { get; } = MetaModel.GetMetadataExtractorVersion();
        public IReactiveProperty<string> FilePath { get; }
        public IReactiveProperty<string> DroppedPath { get; }
        public IReadOnlyReactiveProperty<IReadOnlyCollection<MetaPageViewModel>> MetaPages { get; }

        public MainWindowViewModel(MetaModel metaModel)
        {
            var disposables = new CompositeDisposable();    // undisposed

            FilePath = metaModel.FilePath;
            DroppedPath = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged).AddTo(disposables);
            DroppedPath.Subscribe(x => FilePath.Value = x).AddTo(disposables);

            MetaPages = metaModel.SelectedBook
                .Where(x => x is not null)
                .Select(x => CreateMetaPages(x!).ToArray())
                .ToReadOnlyReactivePropertySlim<IReadOnlyCollection<MetaPageViewModel>>()
                .AddTo(disposables);

#if DEBUG
            FilePath.Value = @"C:\data\official\Panasonic_DMC-GF7.jpg";
#endif
        }

        private static IEnumerable<MetaPageViewModel> CreateMetaPages(MetadataStorage.MetaBook book)
        {
            var pages = book.Pages.Select(p => new MetaPageViewModel(p)).ToArray();
            var all = new MetaPageViewModel("All", pages.SelectMany(x => x.Tags));
            return pages.Prepend(all);
        }
    }
}
