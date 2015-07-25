using System.Runtime.InteropServices.ComTypes;

namespace IpodCopyFix.Wpf.Services
{
    /// <summary>
    /// Concrete implementation of <see cref="IFileService"/>.
    /// </summary>
    public class FileService : IFileService
    {
        /// <see cref="IFileService.OpenFileDialog"/>
        public string OpenFileDialog(string initialPath)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            var result = dialog.ShowDialog();
            return result == true ? dialog.FileName : null;
        }

        /// <see cref="IFileService.OpenFile"/>
        public IStream OpenFile(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
