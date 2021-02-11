using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataViewer.ViewModels
{
    record MetaTagViewModel
    {
        public ColoredText Group { get; }
        public ColoredText Id { get; }
        public ColoredText Name { get; }
        public ColoredText Description { get; }
        public ColoredText Type { get; }
        public ColoredText Data { get; }

        private readonly string _textConcatLower;

        private ReadOnlySpan<ColoredText> ColoredTextProperties => new[] { Group, Id, Name, Description, Type, Data };

        public MetaTagViewModel(MetadataStorage.MetaTag tag)
        {
            Group = new ColoredText(tag.PageName);
            Id = new ColoredText("0x" + tag.Id.ToString("x4"));
            Name = new ColoredText(tag.Name);
            Description = new ColoredText(tag.Description);
            Type = new ColoredText(tag.Data?.GetType().ToString() ?? "null");
            Data = new ColoredText(tag.Data?.ToString() ?? "null");

            _textConcatLower = GetConcatText(ColoredTextProperties).ToLower();
        }

        private static string GetConcatText(in ReadOnlySpan<ColoredText> coloredTexts)
        {
            var sb = new StringBuilder();
            foreach (var prop in coloredTexts)
            {
                sb.Append(prop.Text);
            }
            return sb.ToString();
        }

        /// <summary>文字列検索のヒット判定</summary>
        public bool IsContainsAll(IReadOnlyCollection<string> words)
        {
            // words は Lower で通知される取り決め（高速化のため）
            var hittedCounter = CountHittedWords(_textConcatLower, words);
            var isHitAll = hittedCounter >= words.Count;
            var isHitAny = hittedCounter > 0;

            if (!isHitAny)
            {
                ClearColors();
            }
            else
            {
                // 文字列のヒット位置を更新
                foreach (var prop in ColoredTextProperties)
                    prop.FilterWords(words);
            }
            return isHitAll;

            // 複数の検索文字列にヒットした数を返す
            static int CountHittedWords(string source, IReadOnlyCollection<string> words)
            {
                var counter = 0;
                foreach (var w in words)
                {
                    if (source.Contains(w)) counter++;
                }
                return counter;
            }
        }

        public void ClearColors()
        {
            foreach (var prop in ColoredTextProperties)
                prop.Clear();
        }
    }
}
