using System.Windows;
using DoctorApplication.stores;
using DoctorApplication.viewmodels;
using DoctorLogic;

namespace DoctorApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private NavigationStore _navigationStore;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore = new(new DoctorClient());

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
            _navigationStore.Client.Stop();
            _navigationStore.Client.SelfDestruct();   
        }
    }
}