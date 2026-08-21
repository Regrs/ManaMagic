using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#nullable enable

namespace ZwellTech
{
    //[DebuggerNonUserCode]
    public static class ValidationHelper
    {
        public static void ThrowIfArgumentNull<T>([NotNull] ReadOnlySpan<T> argument, [CallerArgumentExpression("argument")] string? paramName = null) where T : class
        {
            if (argument == null)
            {
                ThrowHelper.ThrowArgumentNullException(paramName);
            }
        }
        public static void ThrowIfArgumentNull<T>([NotNull] T? argument, [CallerArgumentExpression("argument")] string? paramName = null) where T : class
        {
            if (argument == null)
            {
                ThrowHelper.ThrowArgumentNullException(paramName);
            }
        }
        public static void ThrowIfArgumentLessThan<T1, T2>(T1 argument, T2 value, string message, [CallerArgumentExpression("argument")] string? paramName = null) where T1 : notnull, IComparable where T2 : notnull, IComparable
        {
            if (argument.CompareTo(value) < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(paramName, argument, message);
            }
        }
        public static void ThrowIfArgumentGreaterThan<T1, T2>(T1 argument, T2 value, string message, [CallerArgumentExpression("argument")] string? paramName = null) where T1 : notnull, IComparable where T2 : notnull, IComparable
        {
            if (argument.CompareTo(value) > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(paramName, argument, message);
            }
        }
        public static void ThrowIfArgumentGreaterThanOrEqual<T1, T2>(T1 argument, T2 value, string message, [CallerArgumentExpression("argument")] string? paramName = null) where T1 : notnull, IComparable where T2 : notnull, IComparable
        {
            if (argument.CompareTo(value) >= 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(paramName, argument, message);
            }
        }
        public static void ThrowIfArgumentLengthIsZero<T>(IReadOnlyCollection<T> argument, string message, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument.Count == 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(paramName, message);
            }
        }
        public static void ThrowIfArgumentLengthIsZero<T>(ReadOnlySpan<T> argument, string message, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument.Length == 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(paramName, message);
            }
        }

        public static void ThrowIfObjectDisposed([DoesNotReturnIf(false)] bool result, string? objectName)
        {
            if (result)
            {
                ThrowHelper.ThrowObjectDisposedException(objectName);
            }
        }
    }
}