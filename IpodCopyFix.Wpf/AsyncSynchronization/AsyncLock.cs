using System;
using System.Threading;
using System.Threading.Tasks;

namespace IpodCopyFix.Wpf.AsyncSynchronization
{
    /// <summary>
    /// A lock that can be used with the async/await keywords.
    /// </summary>
    public class AsyncLock
    {
        #region Constants

        private const int LockCount = 1;

        #endregion

        /// <summary>
        /// Constructs a new <see cref="AsyncLock"/>.
        /// </summary>
        public AsyncLock()
        {
            _semaphore = new AsyncSemaphore(LockCount);
            _releaser = Task.FromResult(new Releaser(this));
        }

        /// <summary>
        /// Asynchronously waits for access to the lock.
        /// </summary>
        /// <returns></returns>
        public Task<Releaser> LockAsync()
        {
            var wait = _semaphore.WaitAsync();
            return wait.IsCompleted
                ? _releaser
                : wait.ContinueWith((_, state) => new Releaser((AsyncLock) state), this,
                    CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }

        #region Fields

        private readonly AsyncSemaphore _semaphore;
        private readonly Task<Releaser> _releaser;

        #endregion

        #region Releaser

        /// <summary>
        /// Struct implementing <see cref="IDisposable"/> to allow us to use the 
        /// <see cref="AsyncLock"/> within a "using" statement.
        /// </summary>
        public struct Releaser : IDisposable
        {
            internal Releaser(AsyncLock lockToRelease)
            {
                _lock = lockToRelease;
            }

            /// <see cref="IDisposable.Dispose"/>
            public void Dispose()
            {
                if (_lock != null)
                {
                    _lock._semaphore.Release();
                }
            }

            #region Fields

            private readonly AsyncLock _lock;

            #endregion
        }

        #endregion
    }
}
