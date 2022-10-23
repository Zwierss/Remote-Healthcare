using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using DoctorApplication.commands;

namespace DoctorApplication;

public partial class SelectionWindow : UserControl
{
    public SelectionWindow()
    {
        InitializeComponent();
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var item = (sender as ListViewItem)!.ToString();
        ((dynamic)DataContext).SelectedClient = item!.Substring(38);
    }
}