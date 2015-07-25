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
            throw new System.NotImplementedException();
        }

        /// <see cref="IFileService.OpenFile"/>
        public IStream OpenFile(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
