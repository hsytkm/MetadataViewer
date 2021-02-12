using MetadataViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

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
            if (tabControl.SelectedItem is not MetaPageViewModel page) return;
            if (page.Tags is not IReadOnlyCollection<MetaTagViewModel> tags) return;

            var collectionView = CollectionViewSource.GetDefaultView(tags);
            collectionView.Filter = page.IsHitWord(word);
        }

        private static void OnSelectedItemPropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not MetaTabControl tabControl) return;
            OnFilterWordPropertyChanged(tabControl, tabControl.FilterWord);
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(ColoredText))
            {
                e.Column = new DataGridTemplateColumn
                {
                    Header = e.PropertyName,
                    CellTemplate = GetColoredTextDataTemplate(_propertyNameToDataTemplateDict, e.PropertyName),
                    IsReadOnly = true,
                };
            }

            static DataTemplate GetColoredTextDataTemplate(IDictionary<string, DataTemplate> dict, string name)
            {
                static ParserContext GetParserContext()
                {
                    var pc = new ParserContext();
                    pc.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                    pc.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                    pc.XmlnsDictionary.Add("h", "clr-namespace:MetadataViewer.Views.Helpers;assembly=MetadataViewer");
                    return pc;
                }

                static DataTemplate GetDataTemplate(string name)
                {
                    var xaml = "<DataTemplate><TextBlock h:TextBoxContentHelper.ColoredTextItem=\"{Binding " + name + "}\"/></DataTemplate>";
                    var bs = Encoding.ASCII.GetBytes(xaml);
                    using var sr = new MemoryStream(bs);
                    return (DataTemplate)XamlReader.Load(sr, GetParserContext());
                }

                if (!dict.TryGetValue(name, out var dataTemplate))
                {
                    dataTemplate = GetDataTemplate(name);
                    dict.Add(name, dataTemplate);
                }
                return dataTemplate;
            }
        }
        private readonly Dictionary<string, DataTemplate> _propertyNameToDataTemplateDict = new();

    }
}