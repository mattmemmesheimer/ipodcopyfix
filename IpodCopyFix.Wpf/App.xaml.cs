using System.Windows;
using Microsoft.Practices.Unity;

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

            _bootstrapper = new Bootstrapper();

            Current.MainWindow = _bootstrapper.Container.Resolve<MainWindow>();
            Current.MainWindow.Show();
        }

        #region Fields

        private Bootstrapper _bootstrapper;

        #endregion
    }
}
