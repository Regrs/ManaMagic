using System;
using System.Runtime.InteropServices;
using ZwellTech.Interop.Win32;

#nullable enable

using User32NativeMethods = ZwellTech.Interop.User32.NativeMethods;

namespace ZwellTech.Interop.UxTheme
{
    /// <summary>
    /// Provides functions for setting a control's theme.
    /// </summary>
    public static class UXThemeManager
    {
        /// <summary>
        /// Causes a window to use a different set of visual style information than its class normally uses.
        /// </summary>
        /// <param name="handle">Handle to the window whose visual style information is to be changed.</param>
        /// <param name="applicationName">Pointer to a string that contains the application name to use in place of the calling application's name. If this parameter is NULL, the calling application's name is used.</param>
        /// <param name="idList">Pointer to a string that contains a semicolon-separated list of CLSID names to use in place of the actual list passed by the window's class. If this parameter is NULL, the ID list from the calling class is used.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <remarks>
        /// The theme manager retains the pszSubAppName and the pszSubIdList associations through the lifetime of the window, 
        /// even if visual styles subsequently change. The window is sent a WM_THEMECHANGED message at the end of a SetWindowTheme 
        /// call, so that the new visual style can be found and applied.
        /// 
        /// When pszSubAppName and pszSubIdList are NULL, the theme manager removes the previously applied associations. You can 
        /// prevent visual styles from being applied to a specified window by specifying an empty string, (L" "), which does not 
        /// match any section entries. 
        /// </remarks>
        public static int SetWindowTheme(IntPtr handle, string applicationName, string? idList)
        {
            return NativeMethods.SetWindowTheme(handle, applicationName, idList);
        }
        
        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="handle">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="index">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. </param>
        /// <param name="newValue">The replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer, zero otherwise.</returns>
        public static bool SetWindowAttribute(HandleRef handle, IntPtr index, IntPtr newValue)
        {
            if (IntPtr.Size == 8)
            {
                IntPtr resultLong = User32NativeMethods.SetWindowLongPtr(handle, index.ToInt32(), newValue);
                return resultLong.ToInt32() != 0;
            }

            int result = User32NativeMethods.SetWindowLong(handle, index.ToInt32(), newValue.ToInt32());
            return result != 0;
        }
        
        /// <summary>
        /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory. 
        /// </summary>
        /// <param name="handle">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="index">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer.</param>
        /// <returns>If the function succeeds, the return value is the requested value, zero otherwise.</returns>
        public static IntPtr GetWindowAttribute(HandleRef handle, IntPtr index)
        {
            if (IntPtr.Size == 8) { return User32NativeMethods.GetWindowLongPtr(handle, index.ToInt32()); }

            return new IntPtr(User32NativeMethods.GetWindowLong(handle, index.ToInt32()));
        }

        /// <summary>
        /// Sends the specified treeview message to the specified treeview with the stated wParam and lParam values.
        /// </summary>
        /// <param name="handle">A handle to the treeview.</param>
        /// <param name="message">The <see cref="DotSlash.InteropServices.TreeViewMessage"/> to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public static IntPtr SendMessage(IntPtr handle, TreeViewMessage message, IntPtr wParam, IntPtr lParam)
        {
            return User32NativeMethods.SendMessage(handle, (uint)message, wParam, lParam);
        }
    }
}