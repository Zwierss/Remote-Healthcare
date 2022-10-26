using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DoctorApplication.stores;

public static class UtilStore
{
    /// <summary>
    /// Convert a SecureString to a string by copying the SecureString to unmanaged memory, then copying the unmanaged
    /// memory to a managed string, then zeroing out the unmanaged memory.
    /// </summary>
    /// <param name="SecureString">The SecureString object to convert to a string.</param>
    /// <returns>
    /// A string
    /// </returns>
    public static string SecureStringToString(SecureString value)
    {
        IntPtr valuePtr = IntPtr.Zero;
        try
        {
            valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
            return Marshal.PtrToStringUni(valuePtr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
        }
    }
}