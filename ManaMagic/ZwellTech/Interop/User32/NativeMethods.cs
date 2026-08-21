using System;
using System.Runtime.InteropServices;

#nullable enable

namespace ZwellTech.Interop.User32
{
    internal static partial class NativeMethods
    {
        /// <summary>
        /// Sends the specified unicode message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
        /// <param name="Msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "SendMessage", SetLastError = true)]
        internal static extern IntPtr SendMessage([In] IntPtr hWnd, [In] uint Msg, [In] IntPtr wParam, [In] IntPtr lParam);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the following values. </param>
        /// <param name="dwNewLong">The replacement value. </param>
        /// <returns>
        /// If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("User32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        internal static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory. 
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs. The SetWindowLongPtr function fails if the process that owns the window specified by the hWnd parameter is at a higher process privilege in the UIPI hierarchy than the process the calling thread resides in.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the following values.</param>
        /// <param name="dwNewLong">The replacement value. </param>
        /// <returns>
        /// If the function succeeds, the return value is the previous value of the specified offset.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
        /// </returns>
        [DllImport("User32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory. 
        /// </summary>
        /// <param name="window">A handle to the window and, indirectly, the class to which the window belongs. </param>
        /// <param name="index">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer.</param>
        /// <returns>
        /// If the function succeeds, the return value is the requested value.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// If SetWindowLong has not been called previously, GetWindowLong returns zero for values in the extra window or class memory.
        /// </returns>
        [DllImport("User32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        internal static extern int GetWindowLong(HandleRef window, int index);

        /// <summary>
        /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory. 
        /// </summary>
        /// <param name="window">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="index">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
        /// <returns>
        /// If the function succeeds, the return value is the requested value.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// If SetWindowLong has not been called previously, GetWindowLong returns zero for values in the extra window or class memory.
        /// </returns>
        [DllImport("User32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        internal static extern IntPtr GetWindowLongPtr(HandleRef window, int index);
    }
}