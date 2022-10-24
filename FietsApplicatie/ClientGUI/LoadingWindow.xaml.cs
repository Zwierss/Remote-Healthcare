using System;
using System.Windows;
using System.Windows.Controls;

namespace ClientGUI;

public partial class LoadingWindow : UserControl
{
    public static readonly string Picture = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("ClientGUI", StringComparison.Ordinal)) + "ClientGUI\\resources\\load.gif";
    
    public LoadingWindow()
    {
        InitializeComponent();
    }
}