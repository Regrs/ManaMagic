using System;

#nullable enable

namespace ZwellTech.Logging
{
    internal readonly struct LogEntry : IEquatable<LogEntry>
    {
        internal DateTime LogTime { get; }
        internal LogLevel Level { get; }
        internal LogComponent Component { get; }
        internal string Message { get; }

        internal LogEntry(LogLevel type, LogComponent component, string message)
        {
            this.LogTime = DateTime.Now;
            this.Level = type;
            this.Component = component;
            this.Message = message;
        }

        public bool Equals(LogEntry other)
        {
            return this.LogTime.Equals(other.LogTime) && this.Level.Equals(other.Level) && this.Component.Equals(other.Component) && this.Message.Equals(other.Message, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            if (obj is LogEntry other) { return this.Equals(other); }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.LogTime, this.Level, this.Component, this.Message);
        }

        public override string ToString()
        {
            return $"[{this.LogTime:hh:mm:ss}] ({this.Component}) {this.Level}: {this.Message}";
        }

        public static bool operator ==(LogEntry left, LogEntry right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogEntry left, LogEntry right)
        {
            return !left.Equals(right);
        }
    }
}