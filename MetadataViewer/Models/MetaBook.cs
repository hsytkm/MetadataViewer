using MetadataExtractor.Formats.Jpeg;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace MetadataViewer.Models
{
    class MetaBook : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public IReactiveProperty<IReadOnlyCollection<MetaTag>> Tags { get; private set; }

        public MetaBook()
        {
            Tags = new ReactivePropertySlim<IReadOnlyCollection<MetaTag>>().AddTo(_disposables);
        }

        public void ReadTags(string filePath)
        {
            //var directories = ImageMetadataReader.ReadMetadata(filePath);
            var directories = JpegMetadataReader.ReadMetadata(filePath);
            Tags.Value = directories.SelectMany(x => x.Tags.Select(x => new MetaTag(x))).ToArray();
        }

        public void Dispose() => _disposables.Dispose();
    }
}
