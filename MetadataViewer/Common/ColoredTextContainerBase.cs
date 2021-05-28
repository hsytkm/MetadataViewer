using System.Collections.Generic;
using System.Text;

namespace MetadataViewer.Common
{
    abstract record ColoredTextContainerBase : IColoredTextCollection
    {
        /// <summary>
        /// 継承レコードの ColoredText プロパティを返します。
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ColoredText> GetColoredTextProperties();

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
                    foreach (var coloredText in GetColoredTextProperties())
                    {
                        sb.Append(coloredText.Text.ToLower());
                    }
                    _concatLowerText = sb.ToString();
                }
                return _concatLowerText;
            }
        }
        private string? _concatLowerText = null;

        /// <summary>
        /// 引数の文字列にヒットする文字列に色を付けます</summary>
        /// </summary>
        /// <param name="words">色を付ける文字列</param>
        /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
        public bool ColorLetters(IReadOnlyCollection<string> words)
        {
            // words は Lower で通知される取り決め（高速化のため）
            var hitCounter = CountHitWords(ConcatLowerText, words);
            var isHitAll = hitCounter >= words.Count;

            if (!isHitAll)
            {
                ClearColors();
            }
            else
            {
                // 文字列のヒット位置を更新
                foreach (var prop in GetColoredTextProperties())
                    prop.FilterWords(words);
            }
            return isHitAll;

            // 複数の検索文字列にヒットした数を返す
            static int CountHitWords(string source, IReadOnlyCollection<string> words)
            {
                var counter = 0;
                foreach (var w in words)
                {
                    if (source.Contains(w)) counter++;
                }
                return counter;
            }
        }

        /// <summary>
        /// 文字列の色付けを解除します
        /// </summary>
        public void ClearColors()
        {
            foreach (var prop in GetColoredTextProperties())
                prop.Clear();
        }
    }
}