using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace ZwellTech.Threading
{
    public static class TaskExtensionMethods
    {
        #region Extension Methods - SemaphoreSlim

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and returns a <see cref="SemaphoreScopeLock"/> object that can scope the acquisitions lifetime.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scoped.</param>
        /// <returns>A <see cref="SemaphoreScopeLock"/> instance.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        public static Task<SemaphoreScopeLock> AcquireScopeLockAsync(this SemaphoreSlim semaphore)
        {
            return TaskExtensionMethods.AcquireScopeLockAsync(semaphore, Timeout.Infinite, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and returns a <see cref="SemaphoreScopeLock"/> object that can scope the acquisitions lifetime, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scoped.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>A <see cref="SemaphoreScopeLock"/> instance.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="OperationCanceledException">cancellationToken was canceled.</exception>
        public static Task<SemaphoreScopeLock> AcquireScopeLockAsync(this SemaphoreSlim semaphore, CancellationToken token)
        {
            return TaskExtensionMethods.AcquireScopeLockAsync(semaphore, Timeout.Infinite, token);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and returns a <see cref="SemaphoreScopeLock"/> object that can scope the acquisitions lifetime, 
        /// using a 32-bit signed integer to specify the time interval.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scoped.</param>
        /// <param name="timeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <returns>A <see cref="SemaphoreScopeLock"/> instance.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- timeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by timeout.</exception>
        public static Task<SemaphoreScopeLock> AcquireScopeLockAsync(this SemaphoreSlim semaphore, TimeSpan timeout)
        {
            return TaskExtensionMethods.AcquireScopeLockAsync(semaphore, (int)timeout.TotalMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and returns a <see cref="SemaphoreScopeLock"/> object that can scope the acquisitions lifetime, 
        /// using a 32-bit signed integer to specify the time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scoped.</param>
        /// <param name="timeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>A <see cref="SemaphoreScopeLock"/> instance.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- timeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by timeout.</exception>
        /// <exception cref="OperationCanceledException">cancellationToken was canceled.</exception>
        public static Task<SemaphoreScopeLock> AcquireScopeLockAsync(this SemaphoreSlim semaphore, TimeSpan timeout, CancellationToken token)
        {
            return TaskExtensionMethods.AcquireScopeLockAsync(semaphore, (int)timeout.TotalMilliseconds, token);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and returns a <see cref="SemaphoreScopeLock"/> object that can scope the acquisitions lifetime, 
        /// using a 32-bit signed integer to specify the time interval.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scoped.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <returns>A <see cref="SemaphoreScopeLock"/> instance.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">millisecondsTimeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- millisecondsTimeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by millisecondsTimeout.</exception>
        public static Task<SemaphoreScopeLock> AcquireScopeLockAsync(this SemaphoreSlim semaphore, int millisecondsTimeout)
        {
            return TaskExtensionMethods.AcquireScopeLockAsync(semaphore, millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and returns a <see cref="SemaphoreScopeLock"/> object that can scope the acquisitions lifetime, 
        /// using a 32-bit signed integer to specify the time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scoped.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>A <see cref="SemaphoreScopeLock"/> instance.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">millisecondsTimeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- millisecondsTimeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by millisecondsTimeout.</exception>
        /// <exception cref="OperationCanceledException">cancellationToken was canceled.</exception>
        public static async Task<SemaphoreScopeLock> AcquireScopeLockAsync(this SemaphoreSlim semaphore, int millisecondsTimeout, CancellationToken token)
        {
            SemaphoreScopeLock scopeLock = new SemaphoreScopeLock(semaphore);
            await scopeLock.AcquireAsync(millisecondsTimeout, token);

            return scopeLock;
        }

        #endregion
    }
}
