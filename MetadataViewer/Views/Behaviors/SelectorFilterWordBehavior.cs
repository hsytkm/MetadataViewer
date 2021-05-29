using MetadataViewer.Core;
using MetadataViewer.ViewModels;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MetadataViewer.Views.Behaviors
{
    interface IHasFilterWord
    {
        string FilterWord { get; }
    }

    /// <summary>
    /// Selector(TabControl).SelectedItem 内の ItemsSource を絞り込むための Behavior です。
    /// </summary>
    [TypeConstraint(typeof(Selector))]
    class SelectorFilterWordBehavior<T> : Behavior<Selector>, IHasFilterWord
        where T : IColoredTextCollection
    {
        public static readonly DependencyProperty FilterWordProperty =
            DependencyProperty.Register(nameof(FilterWord), typeof(string), typeof(SelectorFilterWordBehavior<T>),
                new FrameworkPropertyMetadata("", (sender, e) =>
                    OnFilterWordPropertyChanged(((SelectorFilterWordBehavior<T>)sender).AssociatedObject, (string)e.NewValue)));
        public string FilterWord
        {
            get => (string)GetValue(FilterWordProperty);
            set => SetValue(FilterWordProperty, value);
        }

        /// <summary>コレクションの絞り込み</summary>
        private static void OnFilterWordPropertyChanged(Selector selector, string word)
        {
            if (selector.SelectedItem is not ICompositeColoredTextCollection<T> selectedItem) return;
            if (selectedItem.ColoredTextContainers is not IImmutableList<T> itemsSource) return;

            var collectionView = CollectionViewSource.GetDefaultView(itemsSource);
            collectionView.Filter = selectedItem.IsHitPredicate(word);
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
            if (sender is not Selector selector) return;

            // this.FilterWord を取得するために、Selector の Behavior の interface から引っ張る（ちょっと強引…）
            var hasFilterBehavior = Interaction.GetBehaviors(selector)
                .FirstOrDefault(x => x is IHasFilterWord) as IHasFilterWord;
            if (hasFilterBehavior is null) return;

            OnFilterWordPropertyChanged(selector, hasFilterBehavior.FilterWord);
        }
    }

    // xaml上でGeneric指定できないと思っているので個別に定義
    sealed class MetaTagSelectorFilterWordBehavior : SelectorFilterWordBehavior<MetaTagViewModel> { }
}
