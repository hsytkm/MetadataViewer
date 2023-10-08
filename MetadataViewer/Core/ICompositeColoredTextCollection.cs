namespace MetadataViewer.Core;

/// <summary>
/// ICompositeColoredText のコレクションを保持します。
/// 派生クラスは View における DataGrid の ItemsSource になります。
/// </summary>
/// <typeparam name="T"></typeparam>
internal interface ICompositeColoredTextCollection
{
    /// <summary>
    /// ICompositeColoredText のコレクションです。
    /// DataGrid の ItemsSource になります。
    /// </summary>
    IReadOnlyList<ICompositeColoredText> ColoredTexts { get; }
}
