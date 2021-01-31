using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MetadataViewer.Views
{
    public partial class MetaTabControl : UserControl
    {
        public static readonly DependencyProperty PagesProperty =
            DependencyProperty.Register(nameof(Pages), typeof(IReadOnlyCollection<object>), typeof(MetaTabControl));

        public string Pages
        {
            get => (string)GetValue(PagesProperty);
            set => SetValue(PagesProperty, value);
        }

        public MetaTabControl()
        {
            InitializeComponent();
        }
    }
}
