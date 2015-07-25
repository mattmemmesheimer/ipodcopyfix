using System.Windows.Input;

namespace IpodCopyFix.Wpf.Commanding
{
    /// <summary>
    /// Defines an interface for when <see cref="ICommand.CanExecuteChanged"/> should be raised.
    /// </summary>
    public interface IRaiseCanExecuteChanged
    {
        void RaiseCanExecuteChanged();
    }
}
