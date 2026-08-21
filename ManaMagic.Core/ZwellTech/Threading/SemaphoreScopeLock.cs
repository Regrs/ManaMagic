using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace ZwellTech.Threading
{
    /// <summary>
    /// Represents a scopeable semaphore.
    /// </summary>
    public struct SemaphoreScopeLock : IEquatable<SemaphoreScopeLock>, IDisposable
    {
        private SemaphoreSlim semaphore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwaiterException"/> class with the specified <see cref="SemaphoreSlim"/> object.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> to be scope locked.</param>
        public SemaphoreScopeLock(SemaphoreSlim semaphore)
        {
            ValidationHelper.ThrowIfArgumentNull(semaphore);
            this.semaphore = semaphore;
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/>.
        /// </summary>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        public Task AcquireAsync()
        {
            return this.AcquireAsync(Timeout.Infinite, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/>, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="OperationCanceledException">cancellationToken was canceled.</exception>
        public Task AcquireAsync(CancellationToken token)
        {
            return this.AcquireAsync(Timeout.Infinite, token);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/>, using a 32-bit signed integer to specify the time interval.
        /// </summary>
        /// <param name="timeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- timeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by timeout.</exception>
        public Task AcquireAsync(TimeSpan timeout)
        {
            return this.AcquireAsync((int)timeout.TotalMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/>, using a 32-bit signed integer to specify the time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="timeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">timeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- timeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by timeout.</exception>
        /// <exception cref="OperationCanceledException">cancellationToken was canceled.</exception>
        public Task AcquireAsync(TimeSpan timeout, CancellationToken token)
        {
            return this.AcquireAsync((int)timeout.TotalMilliseconds, token);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/>, using a 32-bit signed integer to specify the time interval.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">millisecondsTimeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- millisecondsTimeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by millisecondsTimeout.</exception>
        public Task AcquireAsync(int millisecondsTimeout)
        {
            return this.AcquireAsync(millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/>, using a 32-bit signed integer to specify the time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">millisecondsTimeout is a negative number other than -1 milliseconds, which represents an infinite time-out. -or- millisecondsTimeout is greater than <see cref="Int32.MaxValue" />.</exception>
        /// <exception cref="TimeoutException">Did not enter the <see cref="SemaphoreSlim"/> in the time interval provided by millisecondsTimeout.</exception>
        /// <exception cref="OperationCanceledException">cancellationToken was canceled.</exception>
        public async Task AcquireAsync(int millisecondsTimeout, CancellationToken token)
        {
            ValidationHelper.ThrowIfArgumentLessThan(millisecondsTimeout, Timeout.Infinite, $"millisecondsTimeout cannot be less than {Timeout.Infinite}");

            // Fast Path.
            if (this.semaphore.Wait(0)) { return; }
            else if (millisecondsTimeout == 0) { ThrowHelper.ThrowTimeoutException("A timeout occurred before the semaphore could be acquired."); }

            if (!await this.semaphore.WaitAsync(millisecondsTimeout, token))
            {
                ThrowHelper.ThrowTimeoutException("A timeout occurred before the semaphore could be acquired.");
            }
        }

        /// <summary>
        /// Releases the <see cref="SemaphoreSlim"/> once.
        /// </summary>
        /// <returns>The previous count of the <see cref="SemaphoreSlim"/>.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="SemaphoreSlim"/> has already been disposed.</exception>
        /// <exception cref="SemaphoreFullException">The <see cref="SemaphoreSlim"/> has already reached its maximum size.</exception>
        public int Release()
        {
            return this.semaphore.Release();
        }

        /// <summary>
        /// Releases the <see cref="SemaphoreSlim"/> once and releases this objects reference to the <see cref="SemaphoreSlim"/> object.
        /// </summary>
        public void Dispose()
        {
            if (this.semaphore != null)
            {
                this.Release();
                this.semaphore = null!;
            }
        }

        #region Interface Implementation: IEquatable

        /// <summary>
        /// Indicates whether the current <see cref="SemaphoreScopeLock" /> object is equal to another <see cref="SemaphoreScopeLock" /> object.
        /// </summary>
        /// <param name="other">An <see cref="SemaphoreScopeLock" /> object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(SemaphoreScopeLock other)
        {
            return this.semaphore.Equals(other);
        }

        #endregion
        #region Method Overrides: Object

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is SemaphoreScopeLock other) { return this.Equals(other); }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.semaphore.GetHashCode();
        }

        #endregion
        #region Operator Overrides

        /// <summary>
        /// Indicates whether one <see cref="SemaphoreScopeLock" /> object is equal to another <see cref="SemaphoreScopeLock" /> object.
        /// </summary>
        /// <param name="left">The first <see cref="SemaphoreScopeLock" /> object to compare.</param>
        /// <param name="right">The second <see cref="SemaphoreScopeLock" /> object to compare.</param>
        /// <returns>true if the the objects are equal; otherwise, false.</returns>
        public static bool operator ==(SemaphoreScopeLock left, SemaphoreScopeLock right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether one <see cref="SemaphoreScopeLock" /> object is not equal to another <see cref="SemaphoreScopeLock" /> object.
        /// </summary>
        /// <param name="left">The first <see cref="SemaphoreScopeLock" /> object to compare.</param>
        /// <param name="right">The second <see cref="SemaphoreScopeLock" /> object to compare.</param>
        /// <returns>true if the the objects are not equal; otherwise, false.</returns>
        public static bool operator !=(SemaphoreScopeLock left, SemaphoreScopeLock right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
