using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ZwellTech.Threading;
using LoggingTimer = System.Timers.Timer;

#nullable enable

namespace ZwellTech.Logging
{
    /// <summary>
    /// Encapsulates properties and methods to enable logging messages to various outputs.
    /// </summary>
    public sealed class LoggerEngine : IDisposable
    {
        /// <summary>
        /// Gets the default <see cref="LoggerEngine"/> instance.
        /// </summary>
        public static LoggerEngine Logger { get; } = new LoggerEngine();

        private readonly ConcurrentQueue<LogEntry> logMessageQueue = new ConcurrentQueue<LogEntry>();
        private readonly List<TextWriter> writers = new List<TextWriter>(4);
        private readonly LoggingTimer logWriteTimer = new LoggingTimer(500);
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource disposingDisposalSource = new CancellationTokenSource();
        private bool isDisposed = false;

        /// <summary>
        /// Gets or sets the maximum level of messages allowed to be logged.
        /// </summary>
        public LogLevel DisplayLevel { get; set; } = LogLevel.Verbose;

        /// <summary>
        /// Gets if the logging engine is enabled.
        /// </summary>
        public bool Enabled { get; private set; } = false;

        private LoggerEngine()
        {
            this.logWriteTimer.Elapsed += LogWriteTimer_Elapsed;
            this.logWriteTimer.AutoReset = false;
        }

        /// <summary>
        /// Enables the logging engine.
        /// </summary>
        public void Enable()
        {
            ValidationHelper.ThrowIfObjectDisposed(this.isDisposed, this.GetType().FullName);
            if (!this.Enabled)
            {
                this.Enabled = true;
                this.logWriteTimer.Start();
            }
        }

        /// <summary>
        /// Disables the logging engine.
        /// </summary>
        public void Disable()
        {
            ValidationHelper.ThrowIfObjectDisposed(this.isDisposed, this.GetType().FullName);
            if (this.Enabled)
            {
                this.Enabled = false;
                this.logWriteTimer.Stop();
            }
        }

        /// <summary>
        /// Registers a <see cref="TextWriter"/> to receive log messages.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> that will receive log messages.</param>
        public void Register(TextWriter writer)
        {
            this.RegisterAsync(writer).Wait();
        }

        /// <summary>
        /// Asynchronously registers a <see cref="TextWriter"/> to receive log messages.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> that will receive log messages.</param>
        /// <returns>A <see cref="Task"/> representing the in progress register operation.</returns>
        public async Task RegisterAsync(TextWriter writer)
        {
            ValidationHelper.ThrowIfArgumentNull(writer);
            ValidationHelper.ThrowIfObjectDisposed(this.isDisposed, this.GetType().FullName);

            using SemaphoreScopeLock scopeLock = await this.semaphore.AcquireScopeLockAsync(CancellationToken.None);
            this.writers.Add(writer);
        }

        /// <summary>
        /// Asynchronously deregisters a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> that be deregistered.</param>
        /// <returns>A <see cref="Task"/> representing the in progress deregister operation.</returns>
        public async Task DeregisterAsync(TextWriter writer)
        {
            ValidationHelper.ThrowIfArgumentNull(writer);
            ValidationHelper.ThrowIfObjectDisposed(this.isDisposed, this.GetType().FullName);

            using SemaphoreScopeLock scopeLock = await this.semaphore.AcquireScopeLockAsync(CancellationToken.None);
            this.writers.Remove(writer);
        }

        /// <summary>
        /// Logs an error message about the specified component.
        /// </summary>
        /// <param name="component">The <see cref="LogComponent"/> that generated this message.</param>
        /// <param name="message">The message to be logged.</param>
        public void LogError(LogComponent component, string message)
        {
            this.LogEntry(LogLevel.Error, component, message);
        }

        /// <summary>
        /// Logs a warning message about the specified component.
        /// </summary>
        /// <param name="component">The <see cref="LogComponent"/> that generated this message.</param>
        /// <param name="message">The message to be logged.</param>
        public void LogWarning(LogComponent component, string message)
        {
            this.LogEntry(LogLevel.Warning, component, message);
        }

        /// <summary>
        /// Logs an information message about the specified component.
        /// </summary>
        /// <param name="component">The <see cref="LogComponent"/> that generated this message.</param>
        /// <param name="message">The message to be logged.</param>
        public void LogInformation(LogComponent component, string message)
        {
            this.LogEntry(LogLevel.Information, component, message);
        }

        /// <summary>
        /// Logs a debug message about the specified component.
        /// </summary>
        /// <param name="component">The <see cref="LogComponent"/> that generated this message.</param>
        /// <param name="message">The message to be logged.</param>
        public void LogDebug(LogComponent component, string message)
        {
            this.LogEntry(LogLevel.Debug, component, message);
        }

        /// <summary>
        /// Logs a verbose message about the specified component.
        /// </summary>
        /// <param name="component">The <see cref="LogComponent"/> that generated this message.</param>
        /// <param name="message">The message to be logged.</param>
        public void LogVerbose(LogComponent component, string message)
        {
            this.LogEntry(LogLevel.Verbose, component, message);
        }

        /// <summary>
        /// Disposes all resources used by this <see cref="LoggerEngine"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.disposingDisposalSource.Cancel();
                this.Disable();

                this.semaphore.Wait();
                this.semaphore.Release();

                this.logWriteTimer.Dispose();
                this.semaphore.Dispose();

                this.isDisposed = true;
            }
        }

        private void LogEntry(LogLevel level, LogComponent component, string message)
        {
            if (this.Enabled && !this.isDisposed && level <= this.DisplayLevel)
            {
                LogEntry logEntry = new LogEntry(level, component, message);
                this.logMessageQueue.Enqueue(logEntry);
            }
        }

        private async Task LogTimerWriteLineAsync(LogEntry entry)
        {
            string logLine = entry.ToString();
            foreach (TextWriter writer in this.writers)
            {
                await writer.WriteLineAsync(logLine);
            }
        }

        private async void LogWriteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CancellationToken token = this.disposingDisposalSource.Token;
            try
            {
                if (!token.IsCancellationRequested)
                {
                    using SemaphoreScopeLock scopeLock = await this.semaphore.AcquireScopeLockAsync(token);
                    if (this.Enabled)
                    {
                        while (!this.logMessageQueue.IsEmpty)
                        {
                            if (this.logMessageQueue.TryDequeue(out LogEntry entry))
                            {
                                await this.LogTimerWriteLineAsync(entry);
                            }
                            token.ThrowIfCancellationRequested();
                        }

                        token.ThrowIfCancellationRequested();
                        this.logWriteTimer.Start();
                    }
                }
            }
            catch (OperationCanceledException) { /* Exit */ }
            catch (ObjectDisposedException) { /* Exit */ }
        }
    }
}