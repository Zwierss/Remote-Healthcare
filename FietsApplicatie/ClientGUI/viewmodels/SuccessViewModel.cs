using System.Windows.Input;
using ClientApplication;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;
using static ClientApplication.State;
using System.Windows.Documents;
using System.Collections.Generic;

namespace ClientGUI.viewmodels;

public class SuccessViewModel : ObservableObject, IClientCallback
{

    private List<string> _chats;
    public List<string> Chats 
    {
        get => _chats;
        set 
        {
            _chats = value;
            OnPropertyChanged();
        }
    }

    public NavigationStore NavigationStore { get; set; }
    public ICommand Stop{ get; set; }

    private string _image = "resources/checkmark.png";
    public string Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged();
        }
    }

    private string _message = "Gelukt! De fiets is klaar voor gebuik";
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    public SuccessViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        Stop = new StopCommand(this);
        Chats = new List<string>();
    }

    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Error:
                Message = value;
                Image = "resources/warning.png";
                break;
            case Chat:
                List<string> chats = new List<string>();
                chats.Add(value);
                foreach (string s in Chats) 
                {
                    chats.Add(s);
                }
                Chats = chats;
                foreach (string ss in Chats) 
                {
                    System.Console.WriteLine("chat: " + ss);
                }
                break;
        }
    }
}