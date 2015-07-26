using System.Windows;
using IpodCopyFix.Common;
using IpodCopyFix.Wpf.Services;
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

            _container = ConfigureContainer();

            Current.MainWindow = _container.Resolve<MainWindow>();
            Current.MainWindow.Show();
        }

        private IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<IIpodFix, IpodFix>();
            return container;
        }

        #region Fields

        private IUnityContainer _container;

        #endregion
    }
}
