using System.Text;

namespace MetadataViewer.Core;

/// <inheritdoc cref="ICompositeColoredText"/>
internal abstract record CompositeColoredTextBase : ICompositeColoredText
{
    /// <summary>
    /// 検索の高速化用に 各ColoredText を連結した文字列を保持しておきます。
    /// IgnoreCase の仕様なので Lower です。
    /// </summary>
    private string ConcatLowerText
    {
        get
        {
            if (_concatLowerText is null)
            {
                var sb = new StringBuilder();
                foreach (var coloredText in this)
                {
                    sb.Append(coloredText.SourceText.ToLowerInvariant());

                    // ワードが密着すると意図通りに色付けされません
                    sb.Append(CompositeColoredText.Separator);
                }
                _concatLowerText = sb.ToString();
            }
            return _concatLowerText;
        }
    }
    private string? _concatLowerText;

    /// <summary>
    /// 引数の文字列にヒットする文字列に色を付けます
    /// </summary>
    /// <param name="words">色を付ける文字列</param>
    /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
    public bool ColorLetters(IReadOnlyCollection<string> coloringLowerWords)
    {
        // words は Lower で通知される取り決め（高速化のため）
        var isHitAll = IsHitAllWords(ConcatLowerText, coloringLowerWords);

        if (!isHitAll)
        {
            ClearColorTexts();
        }
        else
        {
            // 文字列のヒット位置を更新
            foreach (var prop in this)
                prop.FilterWords(coloringLowerWords);
        }
        return isHitAll;

        // 全ての検索文字列にヒットしかどうかを返します
        static bool IsHitAllWords(string source, IReadOnlyCollection<string> words)
        {
            foreach (var w in words)
            {
                if (!source.Contains(w))
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 文字列の色付けを解除します
    /// </summary>
    public void ClearColorTexts()
    {
        foreach (var prop in this)
            prop.ClearColor();
    }

    public abstract IEnumerator<IColoredText> GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
