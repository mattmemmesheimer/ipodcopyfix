using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpodCopyFix.Wpf.AsyncSynchronization
{
    /// <summary>
    /// A sempahore that can be used with the async/await keywords.
    /// </summary>
    public class AsyncSemaphore
    {
        #region Constants

        private static readonly Task CompletedTask = Task.FromResult(true);

        #endregion

        /// <summary>
        /// Constructs a new <see cref="AsyncSemaphore"/> with the specified initial count.
        /// </summary>
        /// <param name="count">Initial count of the semaphore.</param>
        public AsyncSemaphore(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException(@"count must be greater than 0", "count");
            }
            _count = count;
            _waiters = new Queue<TaskCompletionSource<bool>>();
        }

        /// <summary>
        /// Asynchronously waits for access to the semaphore.
        /// </summary>
        /// <returns>An awaitable task allowing access to the semaphore.</returns>
        public Task WaitAsync()
        {
            lock (_waiters)
            {
                if (_count > 0)
                {
                    --_count;
                    return CompletedTask;
                }
                var waiter = new TaskCompletionSource<bool>();
                _waiters.Enqueue(waiter);
                return waiter.Task;
            }
        }

        /// <summary>
        /// Releases the sempahore.
        /// </summary>
        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (_waiters)
            {
                if (_waiters.Count > 0)
                {
                    toRelease = _waiters.Dequeue();
                }
                else
                {
                    ++_count;
                }
                if (toRelease != null)
                {
                    toRelease.SetResult(true);
                }
            }
        }

        #region Fields

        private int _count;
        private readonly Queue<TaskCompletionSource<bool>> _waiters;

        #endregion
    }
}
