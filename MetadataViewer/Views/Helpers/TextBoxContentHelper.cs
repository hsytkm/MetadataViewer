using MetadataViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MetadataViewer.Views.Helpers
{
    public class TextBoxContentHelper : DependencyObject
    {
        public static readonly DependencyProperty ColoredTextItemProperty =
            DependencyProperty.RegisterAttached("ColoredTextItem", typeof(ColoredText), typeof(TextBoxContentHelper),
                new FrameworkPropertyMetadata(null, OnColoredTextItemsPropertyChanged));

        public static ColoredText GetColoredTextItem(DependencyObject obj) => (ColoredText)obj.GetValue(ColoredTextItemProperty);
        public static void SetColoredTextItem(DependencyObject obj, ColoredText value) => obj.SetValue(ColoredTextItemProperty, value);

        private static void OnColoredTextItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBlock textBlock) return;
            if (e.NewValue is not ColoredText ct) return;

            if (ct.ColoredRanges.Count > 0)         // may be faster than Any()
            {
                if (textBlock.Inlines.Count == 0)   // duplicate display when scrolling
                    textBlock.Inlines.AddRange(CreateRuns(ct));
            }
            else
            {
                textBlock.Text = ct.Text;
            }

            static IEnumerable<Run> CreateRuns(ColoredText ct)
            {
                var sourceText = ct.Text;
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
}
