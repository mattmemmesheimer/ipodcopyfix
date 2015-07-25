using System.Threading.Tasks;
using System.Windows.Input;

namespace IpodCopyFix.Wpf.Commanding
{
    /// <summary>
    /// Defines an interface for an asynchronous <see cref="ICommand"/>, enforcing the constraint
    /// that the generic type is an object (and therefore not a value type).
    /// </summary>
    public interface IAsyncCommand : IAsyncCommand<object>
    {
    }

    /// <summary>
    /// Defines an interface for an asynchronous <see cref="ICommand"/>.
    /// </summary>
    /// <typeparam name="T">Command type.</typeparam>
    public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged
    {
        /// <summary>
        /// Asynchronously executes <see cref="ICommand.Execute"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task ExecuteAsync(T obj);

        /// <see cref="ICommand.CanExecute"/>
        bool CanExecute(object obj);

        /// <summary>
        /// Command.
        /// </summary>
        ICommand Command { get; }
    }

}
