using System.Windows;
using System.Windows.Controls;

namespace DoctorApplication;

public partial class ClientWindow : UserControl
{
    public ClientWindow()
    {
        InitializeComponent();
    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var slider = sender as Slider;
        int value = (int)slider!.Value;
        ((dynamic)DataContext).OnChangedResistance(value);
    }
}