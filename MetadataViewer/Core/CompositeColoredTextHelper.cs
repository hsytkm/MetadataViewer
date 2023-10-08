using System.Text;

namespace MetadataViewer.Core;

internal sealed class CompositeColoredTextHelper
{
    private readonly IReadOnlyList<IColoredText> _coloredTexts;

    // 検索の高速化用に 各ColoredText を連結した文字列を保持しておきます。
    private readonly string _concatLowerText;

    public CompositeColoredTextHelper(ICompositeColoredText compositeColoredText)
    {
        var coloredTexts = compositeColoredText.GetColoredTexts().ToArray();

        _coloredTexts = coloredTexts;
        _concatLowerText = GetConcatLowerText(coloredTexts);
    }

    /// <summary>
    /// 検索の高速化用に 各ColoredText を連結した文字列を保持しておきます。
    /// IgnoreCase の仕様なので Lower です。
    /// </summary>
    private static string GetConcatLowerText(IEnumerable<IColoredText> coloredTexts)
    {
        var sb = new StringBuilder();

        foreach (var text in coloredTexts)
        {
            sb.Append(text.SourceText.ToLowerInvariant());

            // ワードが密着すると意図通りに色付けされません
            sb.Append(CompositeColoredText.Separator);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 引数の文字列にヒットする文字列に色を付けます
    /// </summary>
    /// <param name="words">色を付ける文字列</param>
    /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
    public bool ColorLetters(IReadOnlyCollection<string> coloringLowerWords)
    {
        var coloredTexts = _coloredTexts;

        // words は Lower で通知される取り決め（高速化のため）
        var isHitAll = IsHitAllWords(_concatLowerText, coloringLowerWords);

        if (!isHitAll)
        {
            ClearColorTexts();
        }
        else
        {
            // 文字列のヒット位置を更新
            foreach (var prop in coloredTexts)
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
        foreach (var prop in _coloredTexts)
            prop.ClearColor();
    }
}
