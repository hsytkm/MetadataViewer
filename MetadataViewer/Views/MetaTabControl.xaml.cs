using MetadataViewer.ViewModels.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MetadataViewer.Views
{
    public partial class MetaTabControl : TabControl
    {
        internal static readonly DependencyProperty FilterWordProperty =
            DependencyProperty.Register(nameof(FilterWord), typeof(string), typeof(MetaTabControl),
                new FrameworkPropertyMetadata("", (o, e) => OnFilterWordPropertyChanged((MetaTabControl)o, (string)e.NewValue)));
        internal string FilterWord
        {
            get => (string)GetValue(FilterWordProperty);
            set => SetValue(FilterWordProperty, value);
        }

        public MetaTabControl()
        {
            InitializeComponent();

            // Filter tag item when SelectedItem(tab) changed
            var dpd = DependencyPropertyDescriptor.FromProperty(SelectedItemProperty, typeof(Selector));
            dpd?.AddValueChanged(this, OnSelectedItemPropertyChanged);
        }

        private static void OnFilterWordPropertyChanged(MetaTabControl tabControl, string word)
        {
            if (tabControl.SelectedItem is not MetaPageRecordViewModel page) return;
            if (page.Tags is not IReadOnlyCollection<MetaTagRecordViewModel> tags) return;

            var collectionView = CollectionViewSource.GetDefaultView(tags);
            collectionView.Filter = string.IsNullOrWhiteSpace(word)
                ? null      // static _ => true   // clear
                : x => (x as MetaTagRecordViewModel).IsContains(word);
        }

        private static void OnSelectedItemPropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not MetaTabControl tabControl) return;
            OnFilterWordPropertyChanged(tabControl, tabControl.FilterWord);
        }
    }
}