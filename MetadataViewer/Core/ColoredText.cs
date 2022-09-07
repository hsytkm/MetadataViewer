using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.Core
{
    /// <summary>
    /// 文字列を色付け位置込みで管理する
    /// </summary>
    public class ColoredText
    {
        private static readonly IReadOnlyCollection<Range> _empty = Array.Empty<Range>();

        /// <summary>表示テキスト</summary>
        public string Text { get; }

        /// <summary>色付け文字の位置管理</summary>
        public IReadOnlyCollection<Range> ColoredRanges { get; private set; }

        public ColoredText(string text) => (Text, ColoredRanges) = (text, _empty);

        /// <summary>文字列と検索語から色付け文字の位置を求めます</summary>
        /// <param name="sourceText">ヒットの対象文字列</param>
        /// <param name="words">検索語</param>
        /// <returns>色付け文字の位置</returns>
        private static IReadOnlyList<Range> FilterByWords(string sourceText, IReadOnlyCollection<string> words)
        {
            if (string.IsNullOrWhiteSpace(sourceText))
                return Array.Empty<Range>();

            // char毎に色付けフラグを作る (yield内ではSpan使えないのでArrayPool)
            var isColoredCharLength = sourceText.Length;
            var isColoredCharArray = ArrayPool<bool>.Shared.Rent(isColoredCharLength);
            Span<bool> isColoredChar = isColoredCharArray.AsSpan()[0..isColoredCharLength];
            try
            {
                var ranges = new List<Range>(words.Count);

                // ヒットした文字列をフラグで管理(同じ文字が複数ヒットした場合に対応していません。Analog なら3文字目の a にヒットしません)
                foreach (var word in words)
                {
                    var index = sourceText.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                    if (index < 0) continue;

                    for (var i = index; i < index + word.Length; ++i)
                        isColoredChar[i] = true;
                }

                // 色付けフラグをRangeに変換（まずは start の頭出し）
                int startIndex = GetFirstTrueIndex(isColoredChar, 0), endIndex;
                while (startIndex < isColoredCharLength)
                {
                    for (endIndex = startIndex + 1; endIndex <= isColoredCharLength; endIndex++)
                    {
                        if (endIndex == isColoredCharLength || !isColoredChar[endIndex])   // true から false に変わった
                        {
                            ranges.Add(new Range(startIndex, endIndex));    // true の Range を返す
                            break;
                        }
                        else if (endIndex == isColoredCharLength - 1)      // true のまま最終文字まで至った
                        {
                            endIndex++;
                            ranges.Add(new Range(startIndex, endIndex));
                            break;
                        }
                    }
                    startIndex = GetFirstTrueIndex(isColoredChar, endIndex);
                }
                return ranges;
            }
            finally
            {
                ArrayPool<bool>.Shared.Return(isColoredCharArray, clearArray: true);
            }

            // bool[] から true の index を頭出し
            static int GetFirstTrueIndex(ReadOnlySpan<bool> flags, int startIndex)
            {
                for (var i = startIndex; i < flags.Length; ++i)
                {
                    if (flags[i]) return i;
                }
                return flags.Length;
            }
        }

        /// <summary>文字列と検索語から色付け文字の位置を更新します</summary>
        public void FilterWords(IReadOnlyCollection<string> words) => ColoredRanges = FilterByWords(Text, words);

        public void Clear() => ColoredRanges = _empty;
    }
}
