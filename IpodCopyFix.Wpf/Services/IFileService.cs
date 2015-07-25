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
        /// <returns>The selected file path.</returns>
        string OpenFileDialog();

        /// <summary>
        /// Displays an open folder dialog.
        /// </summary>
        /// <returns>The selected folder path.</returns>
        string OpenFolderDialog();

        /// <summary>
        /// Opens a file stream.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>File stream.</returns>
        IStream OpenFile(string path);
    }

}