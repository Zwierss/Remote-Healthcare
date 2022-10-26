using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DoctorApplication;

public partial class ClientWindow : UserControl
{
    public ClientWindow()
    {
        InitializeComponent();
    }

    private void Slider_ValueChanged(object sender, DragCompletedEventArgs e)
    {
        var slider = sender as Slider;
        int value = (int)slider!.Value;
        ((dynamic)DataContext).OnChangedResistance(value);
    }
}