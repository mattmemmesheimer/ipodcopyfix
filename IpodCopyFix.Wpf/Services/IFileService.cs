using System.Runtime.InteropServices.ComTypes;

namespace IpodCopyFix.Wpf.Services
{
    /// <summary>
    /// Defines an interface for interacting with files.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Displays an open file dialog.
        /// </summary>
        /// <param name="initialPath">Initial path.</param>
        /// <returns>The selected file path.</returns>
        string OpenFileDialog(string initialPath);

        /// <summary>
        /// Opens a file stream.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>File stream.</returns>
        IStream OpenFile(string path);
    }

}