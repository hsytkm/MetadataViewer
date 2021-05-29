using MetadataViewer.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MetadataViewer
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<Models.MetaModel>();
        }

        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //}

        //protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        //{
        //}

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var message = "Occurred unhandled exception" + Environment.NewLine
                + $"{e.Exception.GetType()} : {e.Exception.Message}";

            MessageBox.Show(message, "Exception occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
