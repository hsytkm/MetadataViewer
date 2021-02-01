using System;

namespace MetadataViewer.ViewModels.Records
{
    record MetaTagRecordViewModel
    {
        public string Group { get; }
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Type? Type { get; }
        public object? Data { get; }

        private readonly string _filterSource;
        private static readonly char[] _separator = new char[] { ' ' };

        public MetaTagRecordViewModel(MetadataStorage.MetaTag tag)
        {
            Group = tag.DirectoryName;
            Id = (int)tag.Id;
            Name = tag.Name;
            Description = tag.Description;
            Type = tag.Data?.GetType();
            Data = tag.Data;

            _filterSource = (Group + "0x" + Id.ToString("x4") + Name + Description
                + Type?.ToString() + Data?.ToString()).ToLower();
        }

        /// <summary>文字列検索のヒット判定(空白区切りは AND で検索)</summary>
        public bool IsContains(string word)
        {
            foreach (var w in word.Split(_separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!_filterSource.Contains(w.ToLower())) return false;
            }
            return true;
        }
    }
}
