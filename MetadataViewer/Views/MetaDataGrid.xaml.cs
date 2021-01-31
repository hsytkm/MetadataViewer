using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MetadataViewer.Views
{
    public partial class MetaDataGrid : UserControl
    {
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register(nameof(Tags), typeof(IReadOnlyCollection<object>), typeof(MetaDataGrid));

        public string Tags
        {
            get => (string)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }

        public MetaDataGrid()
        {
            InitializeComponent();
        }
    }
}
