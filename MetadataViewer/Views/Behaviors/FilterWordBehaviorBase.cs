using MetadataViewer.Core;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MetadataViewer.Views.Behaviors;

/// <summary>
/// フィルタ文字列でコレクションを絞り込むための 基底Behavior です。
/// </summary>
internal abstract class FilterWordBehaviorBase<T> : Behavior<T>, IHasFilterWord
    where T : Selector
{
    public static readonly DependencyProperty FilterWordProperty =
        DependencyProperty.Register(nameof(FilterWord), typeof(string), typeof(FilterWordBehaviorBase<T>),
            new FrameworkPropertyMetadata("", (d, e) =>
                OnFilterWordPropertyChanged(((FilterWordBehaviorBase<T>)d).AssociatedObject, (string)e.NewValue)));
    public string FilterWord
    {
        get => (string)GetValue(FilterWordProperty);
        set => SetValue(FilterWordProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        base.OnDetaching();
    }

    /// <summary>SelectedItem変化時の絞り込み</summary>
    private static void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not DependencyObject dep)
            return;

        // 以下の条件がないと子要素(メタ情報)の選択変化で以降の処理が実行される。
        // そうすると Ctrl+C, Ctrl+A などがほとんど効かなくなる。原因不明…
        if (!e.AddedItems.OfType<ICompositeColoredTextsList>().Any())
            return;

        OnFilterWordPropertyChanged(dep);
    }

    /// <summary>DependencyObject から Behavior を引っ張って、フィルタ文字列で絞り込みます</summary>
    private static void OnFilterWordPropertyChanged(DependencyObject dep)
    {
        // this.FilterWord を取得するために、Behavior の interface から引っ張る（ちょっと強引…）
        if (Interaction.GetBehaviors(dep).FirstOrDefault(x => x is IHasFilterWord) is not IHasFilterWord hasFilterWord)
            return;

        OnFilterWordPropertyChanged(dep, hasFilterWord.FilterWord);
    }

    /// <summary>フィルタ文字列でコレクションを絞り込みます</summary>
    internal static async void OnFilterWordPropertyChanged(DependencyObject? dep, string filterSource)
    {
        var textsSource = dep switch
        {
            TabControl tc => (tc.SelectedItem as ICompositeColoredTextsList)?.ColoredTexts,
            DataGrid dg => dg.ItemsSource as IReadOnlyList<ICompositeColoredText>,
            _ => null
        };

        if (textsSource is null)
            return;

        var filterWords = ColoredTextHelper.SplitFilterWords(filterSource);

        // 時間が掛かることがあるので非同期で元データを用意することで、ICollectionView.Filter(Predicate<object>)を高速化します
        await Task.Run(() =>
        {
            if (filterWords.Count > 0)
            {
                foreach (var text in textsSource)
                    text.UpdateColoredTexts(filterWords);
            }
            else
            {
                foreach (var text in textsSource)
                    text.ClearColorTexts();
            }
        });

        var collectionView = CollectionViewSource.GetDefaultView(textsSource);
        collectionView.Filter = ColoredTextHelper.GetIsHitPredicate(filterWords);
    }
}

internal sealed class TabControlFilterWordBehavior : FilterWordBehaviorBase<TabControl>;
