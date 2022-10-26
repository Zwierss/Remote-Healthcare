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
        private NavigationStore _navigationStore;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore = new NavigationStore(new Client());

            _navigationStore.CurrentViewModel = new BeginViewModel(_navigationStore);
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            _navigationStore.Client.Stop(false);
        }
    }
}