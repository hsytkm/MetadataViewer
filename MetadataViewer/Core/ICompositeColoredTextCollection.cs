using System;
using System.Collections.Immutable;

namespace MetadataViewer.Core
{
    interface ICompositeColoredTextCollection<T> where T : IColoredTextCollection
    {
        /// <summary>
        /// 検索対象のコレクション
        /// </summary>
        IImmutableList<T> ColoredTextContainers { get; }

        /// <summary>
        /// 引数の検索語がTagにヒットするかを判定する Predicate を返します。
        /// </summary>
        /// <param name="word"></param>
        /// <returns>検索語のヒットを判定する Predicate</returns>
        Predicate<object> IsHitPredicate(string word);
    }
}
