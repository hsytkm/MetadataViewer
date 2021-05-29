using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace MetadataViewer.Core
{
    abstract record CompositeColoredTextContainerBase<T> : ICompositeColoredTextCollection<T>
         where T : IColoredTextCollection
    {
        public IImmutableList<T> ColoredTextContainers { get; }

        public CompositeColoredTextContainerBase(IEnumerable<T> items)
        {
            ColoredTextContainers = ImmutableArray.CreateRange(items);
        }

        /// <summary>
        /// 引数の検索語がTagにヒットするかを判定する Predicate を返します。
        /// </summary>
        /// <param name="word"></param>
        /// <returns>検索語のヒットを判定する Predicate</returns>
        public Predicate<object> IsHitPredicate(string word)
        {
            // 空白で単語を分けて検索する（IgnoreCaseの仕様なので小文字化して渡す）
            var lowerWords = word.ToLower(CultureInfo.InvariantCulture)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return lowerWords.Length > 0
            ? obj =>
            {
                var container = obj as IColoredTextCollection;

                return (container is not null)
                    ? container.ColorLetters(lowerWords)
                    : false;
            }
            : static obj =>
            {
                (obj as IColoredTextCollection)?.ClearColors();
                return true;
            };
        }
    }
}
