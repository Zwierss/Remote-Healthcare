using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoctorApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool loginClicked = false;

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

        public DoctorMainPage ContinueLogin(bool succeeded)
        {
            if (succeeded)
            {
                DoctorMainPage page = new DoctorMainPage();
                this.Content = page.Content;
                return page;
            }
            else
            {
                Trace.WriteLine("failed");
                return new DoctorMainPage();
            }
        }

        public void ToDoctorMainPage(object sender, RoutedEventArgs e)
        {

            Network network = new Network(usernameInput.Text, passwordInput.Text, ContinueLogin);

            Thread thread = new Thread(network.StartConnection);
            thread.Start();
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
