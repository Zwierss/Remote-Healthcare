﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using DoctorApplication.stores;
using DoctorApplication.viewmodels;
using DoctorLogic;
using MvvmHelpers;

namespace DoctorApplication.commands;

public class LogInCommand : CommandBase
{
    
    private readonly BeginViewModel _view;

    public LogInCommand(BeginViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.Client.ViewModel = _view;
        _view.NavigationStore.Client.SetupConnection(_view.Username, SecureStringToString(_view.SecurePassword), "localhost", 6666);
    }
    
    private string SecureStringToString(SecureString value)
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