namespace MetadataViewer.Core;

internal static class ColoredTextHelper
{
    /// <summary>複数の検索文字列の区切り</summary>
    internal const char Separator = ' ';

    /// <summary>
    /// フィルタ文字列をセパレータで単語に分解します
    /// </summary>
    /// <param name="filterSource"></param>
    /// <returns></returns>
    internal static IReadOnlyList<string> SplitFilterWords(string filterSource)
        => filterSource.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    /// 引数の検索語がTagにヒットするかを判定する Predicate を返します。
    /// CollectionView.Filter に設定します
    /// </summary>
    /// <param name="filterWords">検索する単語</param>
    /// <returns>検索語のヒットを判定する Predicate</returns>
    internal static Predicate<object> GetIsHitPredicate(IReadOnlyList<string> filterWords)
    {
        // 本処理が重いとアプリがフリーズするので判定元データは事前に非同期で用意しています
        return obj =>
        {
            if (filterWords.Count <= 0)
                return true;

            if (obj is not ICompositeColoredText texts)
                return false;

            return texts.IsHitWords(filterWords);
        };
    }
}
