using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.ViewModels
{
    public class ColoredText
    {
        private static readonly IReadOnlyCollection<Range> _empty = Array.Empty<Range>();

        /// <summary>表示テキスト</summary>
        public string Text { get; }

        /// <summary>色付け文字の位置管理</summary>
        public IReadOnlyCollection<Range> ColoredRanges { get; private set; }

        public ColoredText(string text) => (Text, ColoredRanges) = (text, _empty);

        /// <summary>文字列と検索ワードから色付け文字の位置を求めます</summary>
        /// <param name="sourceText">ヒットの対象文字列</param>
        /// <param name="words">検索ワード</param>
        /// <returns>色付け文字の位置</returns>
        private static IEnumerable<Range> FilterByWords(string sourceText, IReadOnlyCollection<string> words)
        {
            if (string.IsNullOrEmpty(sourceText)) yield break;

            // char毎に色付けフラグを作る (yield内ではSpan使えないのでArrayPool)
            var isColoredChar = ArrayPool<bool>.Shared.Rent(sourceText.Length);
            try
            {
                foreach (var word in words)
                {
                    var index = sourceText.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                    if (index < 0) continue;

                    for (var i = index; i < index + word.Length; ++i)
                        isColoredChar[i] |= true;
                }

                // 色付けフラグをRangeに変換
                int start = 0, end;
                do
                {
                    for (end = start + 1; end < isColoredChar.Length; ++end)
                    {
                        if (isColoredChar[start] != isColoredChar[end])
                            break;
                    }

                    // 色付け文字列ならRangeに追加
                    if (isColoredChar[start])
                        yield return new Range(start, end);

                    start = end;
                }
                while (end < isColoredChar.Length);
            }
            finally
            {
                ArrayPool<bool>.Shared.Return(isColoredChar, clearArray: true);
            }
        }

        /// <summary>文字列と検索ワードから色付け文字の位置を更新します</summary>
        public void FilterWords(IReadOnlyCollection<string> words) => ColoredRanges = FilterByWords(Text, words).ToArray();

        public void Clear() => ColoredRanges = _empty;
    }
}
