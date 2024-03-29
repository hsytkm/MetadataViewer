﻿using MetadataViewer.Core;
using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace MetadataViewer.Views.Behaviors;

/// <summary>
/// DataGrid の Column を生成するための Behavior です。
/// 当初は Action で実装しましたが、初回の AutoGeneratingColumn イベントを捉えられないかったので Behavior で実装しました。
/// </summary>
[TypeConstraint(typeof(DataGrid))]
internal sealed class ColoredTextGeneratingColumnBehavior : Behavior<DataGrid>
{
    private readonly Dictionary<string, DataTemplate> _propertyNameToDataTemplateDict = new();

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AutoGeneratingColumn += AssociatedObject_AutoGeneratingColumn;
        AssociatedObject.Columns.CollectionChanged += Columns_CollectionChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.AutoGeneratingColumn -= AssociatedObject_AutoGeneratingColumn;
        AssociatedObject.Columns.CollectionChanged -= Columns_CollectionChanged;

        foreach (var column in AssociatedObject.Columns)
            column.CopyingCellClipboardContent -= CopyToClipboard;

        base.OnDetaching();
    }

    private static void Columns_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Remove && e.OldItems is not null)
        {
            foreach (DataGridColumn column in e.OldItems)
                column.CopyingCellClipboardContent -= CopyToClipboard;
        }
    }

    private void AssociatedObject_AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyType == typeof(IColoredText))
        {
            e.Column = new DataGridTemplateColumn
            {
                Header = e.PropertyName,
                CellTemplate = getColoredTextDataTemplate(_propertyNameToDataTemplateDict, e.PropertyName),
                IsReadOnly = true,
            };

            // Ctrl+C もう少しマシな実装ないのかな？
            e.Column.CopyingCellClipboardContent += CopyToClipboard;
        }

        static DataTemplate getColoredTextDataTemplate(IDictionary<string, DataTemplate> dict, string propertyName)
        {
            static ParserContext getParserContext(Type type)
            {
                var pc = new ParserContext();
                pc.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                pc.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                //pc.XmlnsDictionary.Add("z", "clr-namespace:MetadataViewer.Views.Helpers;assembly=MetadataViewer");
                pc.XmlnsDictionary.Add("z", "clr-namespace:" + type.Namespace + ";assembly=" + type.Assembly.GetName().Name);
                return pc;
            }

            static DataTemplate getDataTemplate(string name)
            {
                var type = typeof(ColoredTextBlockContentHelper);

                //var xaml = "<DataTemplate><TextBlock z:TextBoxContentHelper.ColoredTextItem=\"{Binding " + name + "}\"/></DataTemplate>";
                var xaml = "<DataTemplate><TextBlock z:" + type.Name + "." + ColoredTextBlockContentHelper.ColoredTextItem + "=\"{Binding " + name + "}\"/></DataTemplate>";
                var bs = Encoding.ASCII.GetBytes(xaml);
                using var sr = new MemoryStream(bs);
                return (DataTemplate)XamlReader.Load(sr, getParserContext(type));
            }

            if (!dict.TryGetValue(propertyName, out var dataTemplate))
            {
                dataTemplate = getDataTemplate(propertyName);
                dict.Add(propertyName, dataTemplate);
            }
            return dataTemplate;
        }
    }

    // Ctrl+C 対応
    private static void CopyToClipboard(object? sender, DataGridCellClipboardEventArgs e)
    {
        if (sender is not DataGridTemplateColumn column)
            return;

        if (e.Item is not ICompositeColoredText coloredTexts)
            return;

        if (column.Header is not string propName)
            throw new NotSupportedException("Cannot get property name.");

        if (coloredTexts.GetType().GetProperty(propName)?.GetValue(coloredTexts) is IColoredText ct)
            e.Content = ct.SourceText;
    }
}

/// <summary>
/// ColoredText の状態に応じて TextBlock を Run で色付けする添付プロパティです。
/// コードビハインドから DataTemplate 越しに設定します。
/// </summary>
public sealed class ColoredTextBlockContentHelper : DependencyObject
{
    internal const string ColoredTextItem = nameof(ColoredTextItem);

    public static readonly DependencyProperty ColoredTextItemProperty =
        DependencyProperty.RegisterAttached(ColoredTextItem, typeof(IColoredText), typeof(ColoredTextBlockContentHelper),
            new FrameworkPropertyMetadata(null, OnColoredTextItemsPropertyChanged));

    public static IColoredText GetColoredTextItem(DependencyObject obj) => (IColoredText)obj.GetValue(ColoredTextItemProperty);
    public static void SetColoredTextItem(DependencyObject obj, IColoredText value) => obj.SetValue(ColoredTextItemProperty, value);

    private static void OnColoredTextItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBlock textBlock)
            return;

        if (e.NewValue is not IColoredText ct)
            return;

        if (ct.ColoredRanges.Count > 0)         // may be faster than Any()
        {
            textBlock.Text = "";

            if (textBlock.Inlines.Count == 0)   // duplicate display when scrolling
                textBlock.Inlines.AddRange(createRuns(ct));
        }
        else
        {
            textBlock.Text = ct.SourceText;
        }

        static IEnumerable<Run> createRuns(IColoredText ct)
        {
            var sourceText = ct.SourceText;
            var index = 0;
            foreach (var range in ct.ColoredRanges)
            {
                var start = range.Start.Value;

                if (index < start)
                {
                    yield return new Run(sourceText[new Range(index, start)]);
                    index = start;
                }

                yield return new Run(sourceText[range]) { Background = Brushes.Orange };
                index = range.End.Value;
            }

            if (index < sourceText.Length)
                yield return new Run(sourceText[Range.StartAt(index)]);
        }
    }
}
