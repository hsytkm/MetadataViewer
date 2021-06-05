using MetadataViewer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MetadataViewer.ViewModels
{
    /// <summary>
    /// 各メタページ（Exif / MakerNote）内のメタタグです。
    /// DataGrid の Row になります。
    /// </summary>
    record MetaTagItemViewModel : CompositeColoredTextBase
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

        public MetaTagItemViewModel(MetadataStorage.MetaTag tag)
        {
            Group = new ColoredText(tag.PageName);
            Id = new ColoredText("0x" + tag.Id.ToString("x4", CultureInfo.InvariantCulture));
            Name = new ColoredText(tag.Name);
            Description = new ColoredText(tag.Description);
            Type = new ColoredText(tag.Data?.GetType().ToString() ?? "null");
            Data = new ColoredText(tag.Data?.ToString() ?? "null");
        }
    }
}
