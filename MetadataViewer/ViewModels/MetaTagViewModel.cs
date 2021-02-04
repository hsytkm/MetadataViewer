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
            // 文字列のヒット位置を更新
            foreach (var prop in ColoredTextProperties)
                prop.FilterWords(words);

            // 全文字列がヒットしたか判定
            foreach (var w in words)
            {
                if (!_textConcatLower.Contains(w.ToLower()))
                    return false;
            }
            return true;
        }

        public void ClearColors()
        {
            foreach (var prop in ColoredTextProperties)
                prop.Clear();
        }
    }
}
