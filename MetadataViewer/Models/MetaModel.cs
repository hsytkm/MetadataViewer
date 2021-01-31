using MetadataStorage;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MetadataViewer.Models
{
    class MetaModel : IDisposable
    {
        private readonly CompositeDisposable disposables = new();
        private readonly MetaShelf _metaShelf = new();

        public IReactiveProperty<string> FilePath { get; }
        public IReadOnlyReactiveProperty<MetaBook?> SelectedBook { get; }
        public IReactiveProperty<IReadOnlyCollection<MetaTag>> Tags { get; }

        public MetaModel()
        {
            FilePath = new ReactivePropertySlim<string>().AddTo(disposables);
            Tags = new ReactivePropertySlim<IReadOnlyCollection<MetaTag>>().AddTo(disposables);

            SelectedBook = FilePath
                .Select(x => !File.Exists(x) ? null : _metaShelf.GetOrAdd(x))
                .ToReadOnlyReactivePropertySlim()
                .AddTo(disposables);
        }

        //public MetaBook GetBook(string filePath)
        //{
        //    if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
        //    return _metaShelf.GetOrAdd(filePath);
        //}

        //public IEnumerable<MetaTag> GetAllTags(string filePath) => GetBook(filePath).GetAllTags();

        //public IEnumerable<string> GetPagesName(string filePath) => GetBook(filePath).GetPagesName();

        public void Dispose() => disposables.Dispose();
    }
}
