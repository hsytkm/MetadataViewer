using MetadataViewer.Core;
using System.Globalization;

namespace MetadataViewer.ViewModels;

/// <summary>
/// 各メタページ（Exif / MakerNote）内のメタタグです。
/// DataGrid の Row になります。
/// </summary>
internal sealed record MetaTagItemViewModel : CompositeColoredTextBase
{
    public IColoredText Group { get; }
    public IColoredText Id { get; }
    public IColoredText Name { get; }
    public IColoredText Description { get; }
    public IColoredText Type { get; }
    public IColoredText Data { get; }

    public MetaTagItemViewModel(MetadataStorage.MetaTag tag)
    {
        Group = new ColoredText(tag.PageName);
        Id = new ColoredText("0x" + tag.Id.ToString("x4", CultureInfo.InvariantCulture));
        Name = new ColoredText(tag.Name);
        Description = new ColoredText(tag.Description);
        Type = new ColoredText(tag.Data?.GetType().ToString() ?? "null");
        Data = new ColoredText(tag.Data?.ToString() ?? "null");
    }

    public override IEnumerator<IColoredText> GetEnumerator()
    {
        yield return Group;
        yield return Id;
        yield return Name;
        yield return Description;
        yield return Type;
        yield return Data;
    }
}
