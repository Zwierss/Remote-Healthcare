using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ClientGUI.stores;

public static class UtilStore
{
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