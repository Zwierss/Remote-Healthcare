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
    /* A property that is used to bind the list of chats to the view. */
    public List<string> Chats 
    {
        get => _chats;
        set 
        {
            _chats = value;
            OnPropertyChanged();
        }
    }

    public NavigationStore NavigationStore { get; }
    public ICommand Stop{ get; }

    private string _image = "resources/checkmark.png";
    /* This is a property that is used to bind the image to the view. */
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
    /* This is a property that is used to bind the message to the view. */
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    /* This is the constructor of the viewmodel. It is used to initialize the viewmodel. */
    public SuccessViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        Stop = new StopCommand(this);
        Chats = new List<string>();
    }

    /// <summary>
    /// It takes a state and a string value, and depending on the state, it either sets the message to the value, or adds
    /// the value to the list of chats
    /// </summary>
    /// <param name="State">This is the state of the callback. It can be one of the following:</param>
    /// <param name="value">The value of the message.</param>
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