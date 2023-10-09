using MetadataViewer.Core;
using System.ComponentModel;
using System.Globalization;

namespace MetadataViewer.ViewModels;

/// <summary>
/// 各メタページ（Exif / MakerNote）内のメタタグです。
/// DataGrid の Row になります。
/// </summary>
internal sealed class MetaTagItemViewModel : INotifyPropertyChanged, ICompositeColoredText
{
    private readonly CompositeColoredTextHelper _coloredTextHelper;

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

        _coloredTextHelper = new(this);
    }

    /// <inheritdoc/>
    public IEnumerable<IColoredText> GetColoredTexts()
    {
        yield return Group;
        yield return Id;
        yield return Name;
        yield return Description;
        yield return Type;
        yield return Data;
    }

    /// <inheritdoc/>
    public void UpdateColoredTexts(IReadOnlyCollection<string> filterWords)
        => _coloredTextHelper.UpdateColoredTexts(filterWords);

    /// <inheritdoc/>
    public bool IsHitWords(IEnumerable<string> filterWords)
        => _coloredTextHelper.IsHitWords(filterWords);

    /// <inheritdoc/>
    public void ClearColorTexts() => _coloredTextHelper.ClearColorTexts();

#pragma warning disable CS0067 // The event 'MetaTagItemViewModel.PropertyChanged' is never used
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067 // The event 'MetaTagItemViewModel.PropertyChanged' is never used
}
