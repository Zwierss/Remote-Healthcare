using ClientGUI.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.commands
{
    public class GoBackCommand : CommandBase
    {

        private LoadingViewModel _view;

        public GoBackCommand(LoadingViewModel view)
        {
            _view = view;
        }

        public override void Execute(object parameter)
        {
            _view.NavigationStore.CurrentViewModel = new BeginViewModel(_view.NavigationStore);
            _view.NavigationStore.Client.Stop(true);
        }
    }
}
