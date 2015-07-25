using IpodCopyFix.Wpf.Services;

namespace IpodCopyFix.Wpf.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties



        #endregion

        public MainWindowViewModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        #region Fields

        private readonly IFileService _fileService;

        #endregion
    }
}
