using System.Threading.Tasks;

namespace IpodCopyFix.Common
{
    /// <summary>
    /// Defines an interface for fixing iPod music directories.
    /// </summary>
    public interface IIPodFix
    {
        Task StartAsync(string[] directories);
        Task FixDirectoryAsync(string path);
    }
}
