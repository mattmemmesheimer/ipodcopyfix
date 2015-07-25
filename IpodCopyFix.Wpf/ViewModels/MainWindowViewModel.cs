using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using IpodCopyFix.Common;
using IpodCopyFix.Wpf.Commanding;
using IpodCopyFix.Wpf.Services;
using Microsoft.Practices.Prism.Commands;

namespace IpodCopyFix.Wpf.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands

        /// <summary>
        /// Command to choose the source directory.
        /// </summary>
        public ICommand ChooseSourceCommand { get; set; }

        /// <summary>
        /// Command to choose the destination directory.
        /// </summary>
        public ICommand ChooseDestinationCommand { get; set; }

        /// <summary>
        /// Command to start the fix process.
        /// </summary>
        public ICommand StartCommand { get; set; }

        #endregion

        #region Properties

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

        /// <summary>
        /// Destination path.
        /// </summary>
        public string DestinationPath
        {
            get { return _destinationPath; }
            set { SetProperty(ref _destinationPath, value); }
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
            ChooseSourceCommand = new DelegateCommand(ChooseSourcePath);
            ChooseDestinationCommand = new DelegateCommand(ChooseDestinationPath);
            StartCommand = new AwaitableDelegateCommand(StartAsync);
        }

        private void ChooseSourcePath()
        {
            var path = _fileService.OpenFolderDialog();
            if (path != string.Empty)
            {
                SourcePath = path;
                SourceDirectories = Directory.GetDirectories(SourcePath);
            }
        }

        private void ChooseDestinationPath()
        {
            var path = _fileService.OpenFolderDialog();
            if (path != string.Empty)
            {
                DestinationPath = path;
            }
        }

        private async Task StartAsync()
        {
            await _iPodFix.StartAsync(SourceDirectories, DestinationPath);
        }

        #region Fields

        private readonly IIPodFix _iPodFix;
        private readonly IFileService _fileService;
        private string _sourcePath;
        private string[] _sourceDirectories;
        private string _destinationPath;

        #endregion
    }
}
