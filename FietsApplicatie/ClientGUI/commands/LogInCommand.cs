using System;
using System.Runtime.InteropServices;
using System.Security;
using ClientGUI.viewmodels;

namespace ClientGUI.commands;

public class LogInCommand : CommandBase
{
    
    private readonly BeginViewModel _view;

    public LogInCommand(BeginViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        int port;
        string password = SecureStringToString(_view.SecurePassword);
        try
        {
            port = int.Parse(_view.Port);
        }
        catch (Exception)
        {
            _view.ErrorMessage = "Port moet een getal zijn";
            return;
        }

        _view.NavigationStore.Client.SetupConnection(_view.Username, password, _view.Ip, port, _view.FietsNr, _view.IsChecked);
    }
    
    private static string SecureStringToString(SecureString value)
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