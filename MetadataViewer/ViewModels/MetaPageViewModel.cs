using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.ViewModels
{
    record MetaPageViewModel
    {
        public string Name { get; }
        public IReadOnlyCollection<MetaTagViewModel> Tags { get; }

        public MetaPageViewModel(string name, IReadOnlyCollection<MetaTagViewModel> tags)
        {
            Name = name;
            Tags = tags.ToArray();
        }

        public MetaPageViewModel(string name, IEnumerable<MetaTagViewModel> tags)
            : this(name, tags.ToArray()) { }

        public MetaPageViewModel(MetadataStorage.MetaPage page)
            : this(page.Name, page.Tags.Select(x => new MetaTagViewModel(x)).ToArray()) { }

    }
}
