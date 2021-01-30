using MetadataViewer.Views;
using Prism.Ioc;
using Prism.Unity;
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
            //containerRegistry.RegisterSingleton<ITextFileWriter, CsvFileWriter>();
        }

        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //}

        //protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        //{
        //}

        // ここで例外をハンドルしなければアプリ死にます
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var message = "未ハンドルの例外を検出しました"+ Environment.NewLine
                + $"{e.Exception.GetType()} : {e.Exception.Message}";

            MessageBox.Show(message, "Exception occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
