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
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new(new DoctorClient());

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