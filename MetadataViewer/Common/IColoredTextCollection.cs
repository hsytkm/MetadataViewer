using System.Collections.Generic;
using System;

namespace MetadataViewer.Common
{
    interface IColoredTextCollection
    {
        /// <summary>
        /// 引数の文字列にヒットする文字列に色を付けます</summary>
        /// </summary>
        /// <param name="words">色を付ける文字列</param>
        /// <returns>引数の文字列を全て含むかどうかフラグ</returns>
        bool ColorLetters(IReadOnlyCollection<string> words);

        /// <summary>
        /// 文字列の色付けを解除します
        /// </summary>
        void ClearColors();
    }
}