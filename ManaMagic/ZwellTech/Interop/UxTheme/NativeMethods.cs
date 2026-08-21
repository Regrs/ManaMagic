using System;
using System.Runtime.InteropServices;

#nullable enable

namespace ZwellTech.Interop.UxTheme
{
    internal static partial class NativeMethods
    {
        /// <summary>
        /// Causes a window to use a different set of visual style information than its class normally uses.
        /// </summary>
        /// <param name="hWnd">Handle to the window whose visual style information is to be changed.</param>
        /// <param name="pszSubAppName">Pointer to a string that contains the application name to use in place of the calling application's name. If this parameter is NULL, the calling application's name is used.</param>
        /// <param name="pszSubIdList">Pointer to a string that contains a semicolon-separated list of CLSID names to use in place of the actual list passed by the window's class. If this parameter is NULL, the ID list from the calling class is used.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <see cref="https://docs.microsoft.com/en-us/windows/win32/api/uxtheme/nf-uxtheme-setwindowtheme"/>
        [DllImport("UXTheme.dll", EntryPoint = "SetWindowTheme", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode, BestFitMapping = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern int SetWindowTheme([In] IntPtr hWnd,
                                              [In, MarshalAs(UnmanagedType.LPWStr)] string pszSubAppName,
                                              [In, MarshalAs(UnmanagedType.LPWStr)] string? pszSubIdList);
    }
}