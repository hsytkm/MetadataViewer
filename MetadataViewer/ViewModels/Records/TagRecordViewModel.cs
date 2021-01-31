using System;

namespace MetadataViewer.ViewModels.Records
{
    record TagRecordViewModel
    {
        public string Group { get; }
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        private readonly string _filterSource;
        private readonly static char[] _separator = new char[] { ' ' };

        public TagRecordViewModel(MetadataStorage.MetaTag tag)
        {
            Group = tag.DirectoryName;
            Id = (int)tag.Id;
            Name = tag.Name;
            Description = tag.Description;

            _filterSource = Group.ToLower() + "0x" + Id.ToString("x4") + Name.ToLower() + Description.ToLower();
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
