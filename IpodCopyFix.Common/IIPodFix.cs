using System.Threading.Tasks;

namespace IpodCopyFix.Common
{
    /// <summary>
    /// Defines an interface for fixing iPod music directories.
    /// </summary>
    public interface IIpodFix
    {
        /// <summary>
        /// Starts to asynchronously process the specified directories.
        /// </summary>
        /// <param name="directories">Directories to process.</param>
        /// <param name="destinationPath">Destination.</param>
        /// <returns></returns>
        Task StartAsync(string[] directories, string destinationPath);
    }
}
