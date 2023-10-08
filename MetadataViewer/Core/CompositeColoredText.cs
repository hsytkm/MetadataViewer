namespace MetadataViewer.Core;

internal static class CompositeColoredText
{
    /// <summary>複数の検索文字列の区切り</summary>
    public const char Separator = ' ';

    /// <summary>
    /// 引数の検索語がTagにヒットするかを判定する Predicate を返します。
    /// CollectionView.Filter に設定します
    /// </summary>
    /// <param name="word">検索語</param>
    /// <returns>検索語のヒットを判定する Predicate</returns>
    public static Predicate<object> IsHitPredicate(string word)
    {
        // 空白で単語を分けて検索する（IgnoreCaseの仕様なので小文字化して渡す）
        var coloringLowerWords = word.ToLowerInvariant()
            .Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        if (coloringLowerWords.Length is 0)
        {
            static bool clear(object obj)
            {
                if (obj is ICompositeColoredText texts)
                    texts.ClearColorTexts();
                return true;
            }
            return clear;
        }

        return obj =>
        {
            if (obj is ICompositeColoredText texts)
                return texts.ColorLetters(coloringLowerWords);
            return false;
        };
    }
}
