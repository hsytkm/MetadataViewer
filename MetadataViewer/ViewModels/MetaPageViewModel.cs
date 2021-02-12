using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataViewer.ViewModels
{
    record MetaPageViewModel
    {
        public string Name { get; }
        public IReadOnlyCollection<MetaTagViewModel> Tags { get; }

        public MetaPageViewModel(string name, IReadOnlyCollection<MetaTagViewModel> tags)
        {
            Name = name;
            Tags = tags.ToArray();
        }

        public MetaPageViewModel(string name, IEnumerable<MetaTagViewModel> tags)
            : this(name, tags.ToArray()) { }

        public MetaPageViewModel(MetadataStorage.MetaPage page)
            : this(page.Name, page.Tags.Select(x => new MetaTagViewModel(x)).ToArray()) { }

        /// <summary>引数の検索語が対象Tagにヒットするか判定</summary>
        public Predicate<object> IsHitWord(string word)
        {
            // 空白で単語を分けて検索する（IgnoreCaseの仕様なので小文字化して渡す）
            var lowerWords = word.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return lowerWords.Length > 0
                ? x =>
                {
                    return (x as MetaTagViewModel).IsContainsAll(lowerWords);
                }
                : static x =>
                {
                    (x as MetaTagViewModel).ClearColors();
                    return true;
                };
        }

    }
}
