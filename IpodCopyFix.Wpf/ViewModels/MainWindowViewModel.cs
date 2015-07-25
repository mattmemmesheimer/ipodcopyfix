using System.Windows.Input;
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

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileService">File service to use.</param>
        public MainWindowViewModel(IFileService fileService)
        {
            _fileService = fileService;

            ChooseSourceCommand = new DelegateCommand(OpenFile);
        }

        private void OpenFile()
        {
            SourcePath = _fileService.OpenFileDialog();
        }

        #region Fields

        private readonly IFileService _fileService;
        private string _sourcePath;

        #endregion
    }
}
