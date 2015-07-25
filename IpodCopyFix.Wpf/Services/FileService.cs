using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace IpodCopyFix.Wpf.Services
{
    /// <summary>
    /// Concrete implementation of <see cref="IFileService"/>.
    /// </summary>
    public class FileService : IFileService
    {
        /// <see cref="IFileService.OpenFileDialog"/>
        public string OpenFileDialog()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            var result = dialog.ShowDialog();
            return result == true ? dialog.FileName : null;
        }

        /// <see cref="IFileService.OpenFolderDialog"/>
        public string OpenFolderDialog()
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            return dialog.SelectedPath;
        }

        /// <see cref="IFileService.OpenFile"/>
        public IStream OpenFile(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
