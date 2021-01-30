using System;
using System.Windows;
using System.Windows.Controls;

namespace MetadataViewer.Views.AttachedProperties
{
    class DataGridFormat
    {
        #region int
        public static readonly DependencyProperty IntFormatAutoGenerateProperty =
            DependencyProperty.RegisterAttached("IntFormatAutoGenerate", typeof(string), typeof(DataGridFormat),
                new PropertyMetadata(null, (d, e) => AddEventHandlerOnGenerating<int>(d, e)));
        public static string GetIntFormatAutoGenerate(DependencyObject obj)
            => (string)obj.GetValue(IntFormatAutoGenerateProperty);
        public static void SetIntFormatAutoGenerate(DependencyObject obj, string value)
            => obj.SetValue(IntFormatAutoGenerateProperty, value);
        #endregion

        private static void AddEventHandlerOnGenerating<T>(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid dGrid) return;
            if (e.NewValue is not string format) return;

            dGrid.AutoGeneratingColumn += (_, e) => AddFormat_OnGenerating<T>(e, format);
        }

        private static void AddFormat_OnGenerating<T>(DataGridAutoGeneratingColumnEventArgs e, string format)
        {
            if (e.PropertyType != typeof(T)) return;
            if (e.Column is not DataGridTextColumn column) return;

            column.Binding.StringFormat = format;
        }
    }
}
