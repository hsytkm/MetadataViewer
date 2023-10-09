
namespace MetadataViewer.Core;

/// <summary>
/// 文字列を色付け位置込みで管理します
/// </summary>
public interface IColoredText
{
    /// <summary>表示テキスト</summary>
    string SourceText { get; }

    /// <summary>色付け文字の位置管理</summary>
    IReadOnlyList<Range> ColoredRanges { get; }

    /// <summary>文字列と検索語から色付け文字の位置を更新します</summary>
    void FilterWords(IReadOnlyCollection<string> words);

    /// <summary>色付けをクリアします</summary>
    void ClearColor();
}
