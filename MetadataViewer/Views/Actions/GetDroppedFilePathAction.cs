using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MetadataViewer.Views.Actions
{
    class GetDroppedFilePathAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty DroppedPathProperty =
            DependencyProperty.Register(nameof(DroppedPath), typeof(string), typeof(GetDroppedFilePathAction));

        public string DroppedPath
        {
            get => (string)GetValue(DroppedPathProperty);
            set => SetValue(DroppedPathProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is UIElement element)
                element.AllowDrop = true;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is UIElement element)
                element.AllowDrop = false;

            base.OnDetaching();
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is not DragEventArgs e) return;

            var path = GetFilePaths(e.Data).FirstOrDefault();
            if (path is not null) DroppedPath = path;
        }

        private static IEnumerable<string> GetFilePaths(IDataObject data)
        {
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                if (data.GetData(DataFormats.FileDrop) is string[] ss)
                {
                    foreach (var s in ss)
                        yield return s;
                }
                else { throw new FormatException(); }
            }
            else
            {
                var path = data.GetData(DataFormats.Text)?.ToString();
                yield return path ?? "";
            }
        }
    }
}
