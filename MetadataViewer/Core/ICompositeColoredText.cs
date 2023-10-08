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
    /// <param name="words"></param>
    /// <param name="coloringWords">色を付ける文字列</param>
    /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
    bool ColorLetters(IReadOnlyCollection<string> coloringLowerWords);

    /// <summary>
    /// 文字列の色付けを解除します
    /// </summary>
    void ClearColorTexts();
}
