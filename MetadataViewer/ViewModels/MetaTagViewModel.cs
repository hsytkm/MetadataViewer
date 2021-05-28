using MetadataViewer.Common;
using System;
using System.Collections.Generic;

namespace MetadataViewer.ViewModels
{
    record MetaTagViewModel : ColoredTextContainerBase
    {
        public ColoredText Group { get; }
        public ColoredText Id { get; }
        public ColoredText Name { get; }
        public ColoredText Description { get; }
        public ColoredText Type { get; }
        public ColoredText Data { get; }

        protected override IEnumerable<ColoredText> GetColoredTextProperties()
        {
            yield return Group;
            yield return Id;
            yield return Name;
            yield return Description;
            yield return Type;
            yield return Data;
        }

        public MetaTagViewModel(MetadataStorage.MetaTag tag)
        {
            Group = new ColoredText(tag.PageName);
            Id = new ColoredText("0x" + tag.Id.ToString("x4"));
            Name = new ColoredText(tag.Name);
            Description = new ColoredText(tag.Description);
            Type = new ColoredText(tag.Data?.GetType().ToString() ?? "null");
            Data = new ColoredText(tag.Data?.ToString() ?? "null");
        }
    }
}
