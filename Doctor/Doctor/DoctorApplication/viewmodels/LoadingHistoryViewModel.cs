using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels
{
    public class LoadingHistoryViewModel : ObservableObject, IWindow
    {
        private NavigationStore _navigationStore;
        private string _client { get; }
        private string _item;
        private List<double> _times;
        private List<double> _speeds;
        private List<double> _beats;

        public LoadingHistoryViewModel(NavigationStore navigationStore, string client, string item)
        {
            _times = new List<double>();
            _speeds = new List<double>();
            _beats = new List<double>();
            _navigationStore = navigationStore;
            _navigationStore.Client.ViewModel = this;
            _navigationStore.Client.GetSessionData(client, item);
            _client = client;
            _item = item;
        }

        public void OnChangedValues(State state, string[] args = null)
        {
            switch (state) 
            {
                case Success:
                    _navigationStore.CurrentViewModel = new ViewHistoryViewModel(_navigationStore, _client, _item, _speeds.ToArray(), _times.ToArray(), _beats.ToArray());
                    break;
                case Data:
                    _times.Add(double.Parse(args[5]));
                    _speeds.Add(double.Parse(args[0]));
                    _beats.Add(double.Parse(args[1]));
                    break;
            }
        }
    }
}
