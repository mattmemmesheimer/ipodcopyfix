using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using IpodCopyFix.Common;
using IpodCopyFix.Wpf.Services;
using Microsoft.Practices.Prism.Commands;

namespace IpodCopyFix.Wpf.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Command to choose the source directory.
        /// </summary>
        public ICommand ChooseSourceCommand { get; set; }

        /// <summary>
        /// Source path.
        /// </summary>
        public string SourcePath
        {
            get { return _sourcePath; }
            set { SetProperty(ref _sourcePath, value); }
        }

        /// <summary>
        /// Directories in the source directory.
        /// </summary>
        public string[] SourceDirectories 
        { 
            get { return _sourceDirectories; } 
            set { SetProperty(ref _sourceDirectories, value); } 
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileService">File service to use.</param>
        /// <param name="iPodFix">iPod fix to use.</param>
        public MainWindowViewModel(IFileService fileService, IIPodFix iPodFix)
        {
            _fileService = fileService;
            _iPodFix = iPodFix;
            ChooseSourceCommand = new DelegateCommand(OpenFolder);
        }

        private void OpenFolder()
        {
            var path = _fileService.OpenFolderDialog();
            if (path != string.Empty)
            {
                SourcePath = path;
                SourceDirectories = Directory.GetDirectories(SourcePath);
            }
        }

        private async Task StartAsync()
        {
            await _iPodFix.StartAsync(SourceDirectories);
        }

        #region Fields

        private readonly IIPodFix _iPodFix;
        private readonly IFileService _fileService;
        private string _sourcePath;
        private string[] _sourceDirectories;

        #endregion
    }
}
