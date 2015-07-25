using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IpodCopyFix.Wpf.ViewModels
{
    /// <summary>
    /// Base class for view models.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
