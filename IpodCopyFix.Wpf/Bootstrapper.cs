using IpodCopyFix.Common;
using IpodCopyFix.Wpf.Services;
using log4net.Config;
using log4net.Core;
using Microsoft.Practices.Unity;

namespace IpodCopyFix.Wpf
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class Bootstrapper : IBootstrapper
    {
        #region Properties

        /// <see cref="IBootstrapper.Container"/>
        public IUnityContainer Container { get; private set; }

        #endregion

        /// <summary>
        /// Constructs a new <see cref="Bootstrapper"/>.
        /// </summary>
        public Bootstrapper()
        {
            Container = ConfigureContainer();

            BasicConfigurator.Configure();
        }

        private IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<IIpodFix, IpodFix>();
            return container;
        }
    }
}
