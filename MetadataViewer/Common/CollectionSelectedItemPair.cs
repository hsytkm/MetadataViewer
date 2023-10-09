using System.ComponentModel;

namespace MetadataViewer.Common;

/// <summary>
/// IImmutableList&lt;T&gt; と SelectedItem をペアで管理する。
/// コレクションが変化しない場合に使用する（変化する場合は ObservableCollectionSelectedItemPair）
/// </summary>
internal sealed class CollectionSelectedItemPair<T> : INotifyPropertyChanged
{
    public IReadOnlyList<T> Collection { get; }

    public T? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem is null && value is null)
                return;

            if (_selectedItem?.Equals(value) ?? false)
                return;

            _selectedItem = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
        }
    }
    private T? _selectedItem;

    public event PropertyChangedEventHandler? PropertyChanged;

    public CollectionSelectedItemPair(IReadOnlyList<T> items)
    {
        if (items.Count is 0)
            throw new ArgumentException("items is empty.");

        (Collection, SelectedItem) = (items, items[0]);
    }
}

internal static class CollectionSelectedItemPair
{
    public static CollectionSelectedItemPair<T> Create<T>(IReadOnlyList<T> items) => new(items);
    public static CollectionSelectedItemPair<T> Create<T>(IEnumerable<T> items) => Create(items.ToArray());
}
