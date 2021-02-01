using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Windows;

namespace MetadataViewer.Views
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static void ProcessStart(string url) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

        private void OpenMetadataExtractorOnNuget(object sender, RoutedEventArgs e)
            => ProcessStart("https://www.nuget.org/packages/MetadataExtractor/");

        private void OpenMetadataExtractorOnGitHub(object sender, RoutedEventArgs e)
            => ProcessStart("https://github.com/drewnoakes/metadata-extractor-dotnet");
    }
}
