using System.Windows;

namespace IpodCopyFix.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }
    }
}
