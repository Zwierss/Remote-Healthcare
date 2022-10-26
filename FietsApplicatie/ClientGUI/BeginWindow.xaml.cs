using System.Windows;
using System.Windows.Controls;

namespace ClientGUI;

public partial class BeginWindow : UserControl
{
    public BeginWindow()
    {
        InitializeComponent();
    }
    
    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((dynamic)DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
        }
    }
}