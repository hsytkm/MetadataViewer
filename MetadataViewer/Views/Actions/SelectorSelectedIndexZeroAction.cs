using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls.Primitives;

namespace MetadataViewer.Views.Actions
{
    class SelectorSelectedIndexZeroAction : TriggerAction<Selector>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not Selector selector) return;
            selector.SelectedIndex = 0;
        }
    }
}
