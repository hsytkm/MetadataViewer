using MetadataViewer.Models;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public IReadOnlyReactiveProperty<IReadOnlyCollection<MetaTag>> MetaTags { get; }

        public MainWindowViewModel()
        {
            string path = @"C:\data\official\Panasonic_DMC-GF7.JPG";

            var book = new MetaBook();
            book.ReadTags(path);
            MetaTags = book.Tags;
        }
    }
}
