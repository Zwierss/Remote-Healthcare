﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoctorApplication
{
    /// <summary>
    /// Interaction logic for DoctorMainPage.xaml
    /// </summary>
    public partial class DoctorMainPage : Window
    {

        public class Client
        {
            public string patientId { get; set; }
            public bool isSelected { get; set; }

        }

        public string command = "";

        public delegate void CommandDelegate(JObject jCommand);
        public CommandDelegate GiveCommand;

        public Network currentNetwork;

        public string sessionData = "";

        public List<string> clients = new List<string>()
        {
            "Kars", "Martijn", "Bram", "Xander", "Tommy", "Momin"
        };

        public DoctorMainPage(Network network)
        {
            InitializeComponent();

            this.currentNetwork = network;
            this.GiveCommand = network.returnCommand;

            clientListView.ItemsSource = clients;
            network.main = this;

        }

        public void ShowSessionData()
        {
            this.Dispatcher.Invoke(() =>
            {
                client1DataBox.Text = this.sessionData;

            });
        }

        private void SelectClients(object sender, RoutedEventArgs e)
        {
            List<string> selectedClients = new List<string>();
            foreach(string client in clientListView.SelectedItems)
            {
                selectedClients.Add(client);
            }
            GiveCommand(JObject.Parse(JsonMessageGenerator.GetJsonClientSelectMessage(selectedClients)));
        }

        private void StartSession(object sender, RoutedEventArgs e)
        {
            GiveCommand(JObject.Parse(JsonMessageGenerator.GetJsonStartSessionMessage("Kars")));

            
        }
        
        private void StopSession(object sender, RoutedEventArgs e)
        {
            GiveCommand(JObject.Parse(JsonMessageGenerator.GetJsonStopSessionMessage("Kars")));
        }
        
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            GiveCommand(JObject.Parse(JsonMessageGenerator.GetJsonSendMessage("Kars", messageTextBox.Text)));
        }
        
        private void EmergencyStop(object sender, RoutedEventArgs e)
        {
            GiveCommand(JObject.Parse(JsonMessageGenerator.GetJsonEmergencyStopMessage("Kars")));
        }
    }
}
