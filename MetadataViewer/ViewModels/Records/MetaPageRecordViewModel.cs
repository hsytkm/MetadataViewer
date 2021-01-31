using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.ViewModels.Records
{
    record MetaPageRecordViewModel
    {
        public string Name { get; }
        public IReadOnlyCollection<MetaTagRecordViewModel> Tags { get; }

        public MetaPageRecordViewModel(string name, IEnumerable<MetaTagRecordViewModel> tags)
        {
            Name = name;
            Tags = tags.ToArray();
        }

        public MetaPageRecordViewModel(MetadataStorage.MetaPage page)
        {
            Name = page.Name;
            Tags = page.Tags.Select(x => new MetaTagRecordViewModel(x)).ToArray();
        }
    }
}
