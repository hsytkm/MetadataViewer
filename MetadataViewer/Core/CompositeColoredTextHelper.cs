namespace MetadataViewer.Core;

internal sealed class CompositeColoredTextHelper : ICompositeColoredText
{
    private readonly IReadOnlyList<IColoredText> _coloredTexts;

    // 検索の高速化用に 各ColoredText を連結した文字列を保持しておきます。
    private readonly string _concatText;

    public CompositeColoredTextHelper(ICompositeColoredText compositeColoredText)
    {
        var coloredTexts = compositeColoredText.GetColoredTexts().ToArray();

        _coloredTexts = coloredTexts;
        _concatText = string.Join(ColoredTextHelper.Separator, coloredTexts.Select(x => x.SourceText));
    }

    /// <inheritdoc/>
    public IEnumerable<IColoredText> GetColoredTexts() => _coloredTexts;

    /// <inheritdoc/>
    public void UpdateColoredTexts(IReadOnlyCollection<string> filterWords)
    {
        if (IsHitWords(filterWords))
        {
            foreach (var prop in _coloredTexts)
                prop.FilterWords(filterWords);
        }
        else
        {
            ClearColorTexts();
        }
    }

    /// <inheritdoc/>
    public bool IsHitWords(IEnumerable<string> filterWords)
    {
        var sourceText = _concatText;

        // 全てヒットならtrueを返します
        foreach (var word in filterWords)
        {
            if (!sourceText.Contains(word, StringComparison.OrdinalIgnoreCase))
                return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public void ClearColorTexts()
    {
        foreach (var prop in _coloredTexts)
            prop.ClearColor();
    }
}
