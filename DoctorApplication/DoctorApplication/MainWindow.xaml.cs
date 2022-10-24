using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DoctorApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _toDoctorMainPage;

        private Network _network = null!;

        private DoctorMainPage _page = null!;

        public MainWindow()
        {
            InitializeComponent();
            //usernameInput.Text = "Gebruikernaam:";
            //passwordInput.Text = "Wachtwoord:";
            //usernameInput.TextChanged += RemoveInputPlaceholder;
            //passwordInput.TextChanged += RemoveInputPlaceholder;

            List<string> list = new List<string>
            {
                "12345",
                "65422",
                "76858",
                "54224",
                "124890"
            };

            //Network network = new Network(this);

            //Thread thread = new Thread(network.StartConnection);
            //thread.Start();
            
        }

        public void TurnOnBool()
        {
            _toDoctorMainPage = true;
        }

        public async void WaitForLogin()
        {

        }

        public async Task ContinueLogin()
        {
            _page = new DoctorMainPage(_network);
            _network.showSessionData = _page.ShowSessionData;
            
            while (!_toDoctorMainPage)
            {
                Trace.WriteLine("waiting for bool");
            }
            Trace.WriteLine("about to change page");
            this.Content = _page.Content;
            
        }

        public async void DoctorLogin(object sender, RoutedEventArgs e)
        {
            _network = new Network(usernameInput.Text, passwordInput.Text, TurnOnBool);

            Thread thread = new Thread(_network.StartConnection);
            thread.Start();

            Task waitingForLoginCompleted = ContinueLogin();

            await waitingForLoginCompleted;
        }


        /*
        private void RemoveInputPlaceholder(object sender, TextChangedEventArgs e)
        {
            List<string> changes = e.Changes.ToList<TextChange>();
            (sender as TextBox).Text = e.Changes.ToList<string>();
            (sender as TextBox).TextChanged -= RemoveInputPlaceholder;
        }
        */

    }
}
