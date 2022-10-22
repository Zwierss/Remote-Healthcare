using System;
using System.Threading;
using System.Windows;
using DoctorLogic;

namespace DoctorApplication 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            DoctorClient client = new(uuid.Text, pass.Password, "localhost", 6666);
            client.Start();
            //while (client.LoginSuccessful == null) { }

            
        }
    }
}