﻿using MetadataViewer.Core;
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
    internal interface IHasFilterWord
    {
        string FilterWord { get; }
    }

    /// <summary>
    /// フィルタ文字列でコレクションを絞り込むための 基底Behavior です。
    /// </summary>
    abstract class FilterWordBehaviorBase<T1, T2> : Behavior<T1>, IHasFilterWord
        where T1 : DependencyObject
        where T2 : ICompositeColoredText
    {
        public abstract string FilterWord { get; set; }

        /// <summary>フィルタ文字列でコレクションを絞り込みます</summary>
        internal static void OnFilterWordPropertyChanged(DependencyObject? dependencyObject, string word)
        {
            if (dependencyObject is null) return;

            IImmutableList<T2>? itemsSource = null;

            if (dependencyObject is TabControl tabControl)
            {
                if (tabControl.SelectedItem is not ICompositeColoredTextCollection<T2> selectedItem) return;
                if (selectedItem.Collection is not IImmutableList<T2> itemsSource1) return;
                itemsSource = itemsSource1;
            }
            else if (dependencyObject is DataGrid dataGrid)
            {
                if (dataGrid.ItemsSource is not IImmutableList<T2> itemsSource1) return;
                itemsSource = itemsSource1;
            }

            if (itemsSource is null) return;

            var collectionView = CollectionViewSource.GetDefaultView(itemsSource);
            collectionView.Filter = CompositeColoredText.IsHitPredicate(word);
        }

        /// <summary>DependencyObject から Behavior を引っ張って、フィルタ文字列で絞り込みます</summary>
        internal static void OnFilterWordPropertyChanged(DependencyObject dependencyObject)
        {
            // this.FilterWord を取得するために、Behavior の interface から引っ張る（ちょっと強引…）
            var hasFilterBehavior = Interaction.GetBehaviors(dependencyObject)
                .FirstOrDefault(x => x is IHasFilterWord) as IHasFilterWord;

            if (hasFilterBehavior is null) return;

            OnFilterWordPropertyChanged(dependencyObject, hasFilterBehavior.FilterWord);
        }
    }

    /// <summary>
    /// TabControl.SelectedItem 内の ItemsSource を絞り込むための Behavior です。
    /// </summary>
    [TypeConstraint(typeof(TabControl))]
    class TabControlFilterWordBehavior<T> : FilterWordBehaviorBase<TabControl, T>
        where T : ICompositeColoredText
    {
        public static readonly DependencyProperty FilterWordProperty =
            DependencyProperty.Register(nameof(FilterWord), typeof(string), typeof(TabControlFilterWordBehavior<T>),
                new FrameworkPropertyMetadata("", (sender, e) =>
                    OnFilterWordPropertyChanged(((TabControlFilterWordBehavior<T>)sender).AssociatedObject, (string)e.NewValue)));
        public override string FilterWord
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
            if (sender is not TabControl tabControl) return;
            OnFilterWordPropertyChanged(tabControl);
        }
    }

    /// <summary>
    /// DataGrid.ItemsSource を絞り込むための Behavior です。
    /// </summary>
    //[TypeConstraint(typeof(DataGrid))]
    //class DataGridFilterWordBehavior<T> : FilterWordBehaviorBase<DataGrid, T>
    //    where T : ICompositeColoredText
    //{
    //    public static readonly DependencyProperty FilterWordProperty =
    //        DependencyProperty.Register(nameof(FilterWord), typeof(string), typeof(DataGridFilterWordBehavior<T>),
    //            new FrameworkPropertyMetadata("", (sender, e) =>
    //                OnFilterWordPropertyChanged(((DataGridFilterWordBehavior<T>)sender).AssociatedObject, (string)e.NewValue)));
    //    public override string FilterWord
    //    {
    //        get => (string)GetValue(FilterWordProperty);
    //        set => SetValue(FilterWordProperty, value);
    //    }
    //}

    // xaml上でGeneric指定できないと思っているので個別に定義
    sealed class MetaTagSelectorFilterWordBehavior : TabControlFilterWordBehavior<MetaTagItemViewModel> { }
}