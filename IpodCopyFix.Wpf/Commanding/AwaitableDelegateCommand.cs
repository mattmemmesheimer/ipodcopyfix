using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace IpodCopyFix.Wpf.Commanding
{

    /// <summary>
    /// An awaitable <see cref="DelegateCommand"/>, enforcing the constraint
    /// that the generic type is an object (and therefore not a value type).
    /// </summary>
    public class AwaitableDelegateCommand : AwaitableDelegateCommand<object>, IAsyncCommand
    {
        /// <see cref="AwaitableDelegateCommand{T}"/>
        public AwaitableDelegateCommand(Func<Task> executeMethod) : base(o => executeMethod())
        {
        }

        /// <see cref="AwaitableDelegateCommand{T}"/>
        public AwaitableDelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base(o => executeMethod(), o => canExecuteMethod())
        {
        }
    }

    /// <summary>
    /// An awaitable <see cref="DelegateCommand"/>.
    /// </summary>
    /// <typeparam name="T">Command type.</typeparam>
    public class AwaitableDelegateCommand<T> : IAsyncCommand<T>, ICommand
    {
        #region Properties

        public ICommand Command
        {
            get { return this; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="executeMethod">Method to execute.</param>
        public AwaitableDelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, _ => true)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="executeMethod">Method to execute.</param>
        /// <param name="canExecuteMethod">CanExecute method.</param>
        public AwaitableDelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _executeMethod = executeMethod;
            _underlyingCommand = new DelegateCommand<T>(x => { }, canExecuteMethod);
        }

        /// <summary>
        /// Asynchronously executes the <see cref="ICommand.Execute"/> method.
        /// </summary>
        /// <param name="obj">Parameter.</param>
        /// <returns>Void task.</returns>
        public async Task ExecuteAsync(T obj)
        {
            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                await _executeMethod(obj);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        /// <see cref="ICommand.CanExecute"/>
        public bool CanExecute(object parameter)
        {
            return !_isExecuting && _underlyingCommand.CanExecute((T) parameter);
        }

        /// <summary>
        /// Event raised when the value of <see cref="ICommand.CanExecute"/> has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { _underlyingCommand.CanExecuteChanged += value; }
            remove { _underlyingCommand.CanExecuteChanged -= value; }
        }

        /// <summary>
        /// Asynchronously executes the <see cref="ICommand.Execute"/> method.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public async void Execute(object parameter)
        {
            await ExecuteAsync((T) parameter);
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            _underlyingCommand.RaiseCanExecuteChanged();
        }

        #region Fields

        private readonly Func<T, Task> _executeMethod;
        private readonly DelegateCommand<T> _underlyingCommand;
        private bool _isExecuting;

        #endregion
    }

}
