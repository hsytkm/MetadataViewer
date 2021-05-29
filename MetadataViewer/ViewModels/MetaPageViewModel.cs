using MetadataViewer.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.ViewModels
{
    /// <summary>
    /// メタページ(Exif / MakerNote など)のコレクション。TabControlの各Tab
    /// </summary>
    record MetaPageViewModel : CompositeColoredTextContainerBase<MetaTagViewModel>
    {
        public string Name { get; }

        public MetaPageViewModel(string name, IEnumerable<MetaTagViewModel> tags)
            : base(tags)
        {
            Name = name;
        }

        public MetaPageViewModel(MetadataStorage.MetaPage page)
            : this(page.Name, page.Tags.Select(x => new MetaTagViewModel(x)))
        { }

    }
}
