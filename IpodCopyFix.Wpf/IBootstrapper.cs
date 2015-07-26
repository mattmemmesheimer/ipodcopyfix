using Microsoft.Practices.Unity;

namespace IpodCopyFix.Wpf
{
    /// <summary>
    /// Defines an interface for the application bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Unity container.
        /// </summary>
        IUnityContainer Container { get; }
    }

}