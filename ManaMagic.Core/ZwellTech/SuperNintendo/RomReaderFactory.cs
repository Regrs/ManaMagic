using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using ZwellTech.Logging;

#nullable enable

namespace ZwellTech.SuperNintendo
{
    /// <summary>
    /// Factory class for creating <see cref="RomReader"/>s of various types for a <see cref="RomFile"/>.
    /// </summary>
    public static class RomReaderFactory
    {
        private const BindingFlags ActivatorInstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private static RomFile? RomFile = null;
        private static object[]? ParameterArray = null;
        private static bool Initialized = false;
        private static readonly Dictionary<Type, RomReader> RomReaderCache = new Dictionary<Type, RomReader>();

        /// <summary>
        /// Initializes the <see cref="RomReaderFactory"/> with the specified ROM file.
        /// </summary>
        /// <param name="romFile">A <see cref="RomFile"/> that contains data to be read by <see cref="RomReader"/>s produced by the factory.</param>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="RomReaderFactory"/> is already initialized.</exception>
        [MemberNotNull(nameof(RomFile), nameof(ParameterArray))]
        public static void Initialize(RomFile romFile)
        {
            if (RomReaderFactory.Initialized)
            {
                ThrowHelper.ThrowInvalidOperationException("Factory already initialized.");
            }

            RomReaderFactory.RomFile = romFile;
            ParameterArray = new object[] { RomReaderFactory.RomFile };
            RomReaderFactory.Initialized = true;

            LoggerEngine.Logger.LogVerbose(LogComponent.Initialization, $"[{nameof(RomReaderFactory)}]: Initialization Complete.");
        }

        /// <summary>
        /// Gets a <see cref="RomReader"/> of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="RomReader"/> to get.</typeparam>
        /// <returns>A <see cref="RomReader"/> of the specified type.</returns>
        public static T GetRomReader<T>() where T : RomReader
        {
            if (RomReaderFactory.Initialized)
            {
                Type readerType = typeof(T);
                if (!RomReaderFactory.RomReaderCache.TryGetValue(readerType, out RomReader? cachedReader))
                {
                    cachedReader = (T?)Activator.CreateInstance(readerType, RomReaderFactory.ActivatorInstanceFlags, null, ParameterArray, CultureInfo.InvariantCulture);
                    if (cachedReader == null)
                    {
                        ThrowHelper.ThrowTargetInvocationException($"Failed to create an instance of type {readerType}.");
                    }
                    RomReaderFactory.RomReaderCache.Add(readerType, cachedReader);
                }

                return (T)cachedReader;
            }

            return (T)ThrowHelper.ThrowInvalidOperationException("RomReaderFactory has not been initialized.");
        }
    }
}