using System;
using System.Windows;
using ClientApplication;
using ClientGUI.viewmodels;
using DoctorApplication.stores;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new(new Client());

            navigationStore.CurrentViewModel = new BeginViewModel(navigationStore);
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}