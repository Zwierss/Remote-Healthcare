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
        public bool loginClicked = false;
        public bool toDoctorMainPage = false;

        Network network;

        DoctorMainPage page;

        public MainWindow()
        {
            InitializeComponent();
            //usernameInput.Text = "Gebruikernaam:";
            //passwordInput.Text = "Wachtwoord:";
            //usernameInput.TextChanged += RemoveInputPlaceholder;
            //passwordInput.TextChanged += RemoveInputPlaceholder;

            List<string> list = new List<string>();
            list.Add("12345");
            list.Add("65422");
            list.Add("76858");
            list.Add("54224");
            list.Add("124890");
            
            //Network network = new Network(this);

            //Thread thread = new Thread(network.StartConnection);
            //thread.Start();
            
        }

        public void TurnOnBool()
        {
            toDoctorMainPage = true;
        }

        public async void WaitForLogin()
        {

        }

        public async Task ContinueLogin()
        {
            page = new DoctorMainPage(network);
            network.showSessionData = page.ShowSessionData;
            
            while (!toDoctorMainPage)
            {
                Trace.WriteLine("waiting for bool");
            }
            Trace.WriteLine("about to change page");
            this.Content = page.Content;
            
        }

        public async void DoctorLogin(object sender, RoutedEventArgs e)
        {
            network = new Network(usernameInput.Text, passwordInput.Text, TurnOnBool);

            Thread thread = new Thread(network.StartConnection);
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
