#nullable enable

namespace ZwellTech.Logging
{
    /// <summary>
    /// Defines logging severity levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires immediate attention.
        /// </summary>
        Error = 0,
        /// <summary>
        /// Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the application execution to stop.
        /// </summary>
        Warning = 1,
        /// <summary>
        /// Logs that track the general flow of the application. These logs should have long-term value.
        /// </summary>
        Information = 2,
        /// <summary>
        /// Logs that are used for interactive investigation during development.
        /// </summary>
        Debug = 3,
        /// <summary>
        /// Logs that contain the most detailed messages.
        /// </summary>
        Verbose = 4,
    }
}