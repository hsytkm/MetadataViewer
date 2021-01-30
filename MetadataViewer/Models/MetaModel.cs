using MetadataStorage;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;

namespace MetadataViewer.Models
{
    class MetaModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly MetaShelf _metaShelf = new();

        public IReactiveProperty<IReadOnlyCollection<MetaTag>> Tags { get; }

        public MetaModel()
        {
            Tags = new ReactivePropertySlim<IReadOnlyCollection<MetaTag>>().AddTo(_disposables);
        }

        public IReadOnlyCollection<MetaTag> GetTags(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            return _metaShelf.GetOrAdd(filePath).Tags;
        }

        public void Dispose() => _disposables.Dispose();
    }
}
