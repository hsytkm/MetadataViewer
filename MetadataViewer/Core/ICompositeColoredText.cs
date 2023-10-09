namespace MetadataViewer.Core;

/// <summary>
/// 複数の ColoredText をまとめます。
/// 派生クラスは View における DataGrid の Row になります。
/// </summary>
internal interface ICompositeColoredText
{
    /// <summary>
    /// IColoredTextを取得します
    /// </summary>
    IEnumerable<IColoredText> GetColoredTexts();

    /// <summary>
    /// 引数の文字列にヒットする文字列に色を付けます
    /// </summary>
    /// <param name="filterWords">色付けする単語</param>
    void UpdateColoredTexts(IReadOnlyCollection<string> filterWords);

    /// <summary>
    /// 引数の単語にヒットするかを判定します
    /// </summary>
    /// <param name="filterWords">色付けする単語</param>
    bool IsHitWords(IEnumerable<string> filterWords);

    /// <summary>
    /// 文字列の色付けを解除します
    /// </summary>
    void ClearColorTexts();
}
