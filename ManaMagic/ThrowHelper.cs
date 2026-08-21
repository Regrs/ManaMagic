using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

#nullable enable

namespace ManaMagic
{
    [DebuggerNonUserCode]
    public static class ThrowHelper
    {
        [DoesNotReturn]
        public static object ThrowTypeAccessException(string? message, Exception? inner = null)
        {
            throw new TypeAccessException(message, inner);
        }

        [DoesNotReturn]
        public static object ThrowTargetInvocationException(string? message, Exception? inner = null)
        {
            throw new TargetInvocationException(message, inner);
        }

        [DoesNotReturn]
        public static object ThrowInvalidOperationException(string? message, Exception? inner = null)
        {
            throw new InvalidOperationException(message, inner);
        }

        [DoesNotReturn]
        public static object ThrowArgumentException(string? message, string? paramName, Exception? inner = null)
        {
            throw new ArgumentException(message, paramName, inner);
        }

        [DoesNotReturn]
        public static object ThrowArgumentNullException(string? paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string? paramName, object actualValue, string message)
        {
            throw new ArgumentOutOfRangeException(paramName, actualValue, message);
        }

        [DoesNotReturn]
        public static object ThrowFileNotFoundException(string? message, Exception? inner = null)
        {
            throw new FileNotFoundException(message, inner);
        }

        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName)
        {
            throw new ObjectDisposedException(objectName);
        }

        [DoesNotReturn]
        public static void ThrowTimeoutException(string message, Exception? innerException = null)
        {
            throw new TimeoutException(message, innerException);
        }
    }
}