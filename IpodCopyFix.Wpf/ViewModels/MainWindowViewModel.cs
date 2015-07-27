using System.IO;
using System.Linq;
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
            set
            {
                SetProperty(ref _sourcePath, value);
                if (!string.IsNullOrEmpty(value))
                {
                    ChooseSourcePath(value);
                }
            }
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
        /// <param name="ipodFix">iPod fix to use.</param>
        public MainWindowViewModel(IFileService fileService, IIpodFix ipodFix)
        {
            _fileService = fileService;
            _ipodFix = ipodFix;
            ChooseSourceCommand = new DelegateCommand<string>(ChooseSourcePath);
            ChooseDestinationCommand = new DelegateCommand(ChooseDestinationPath);
            StartCommand = new AwaitableDelegateCommand<object>(StartAsync);
        }

        private void ChooseSourcePath(string path = null)
        {
            if (path == null)
            {
                path = _fileService.OpenFolderDialog();
                SourcePath = path;
            }
            if (path != string.Empty)
            {
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

        private async Task StartAsync(object param)
        {
            var items = (System.Collections.IList) param;
            var selected = items.Cast<string>().ToArray();
            await _ipodFix.StartAsync(selected, DestinationPath);
        }

        #region Fields

        private readonly IIpodFix _ipodFix;
        private readonly IFileService _fileService;
        private string _sourcePath;
        private string[] _sourceDirectories;
        private string _destinationPath;

        #endregion
    }
}
