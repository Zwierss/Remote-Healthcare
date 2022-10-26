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

        /* A constructor that takes a LoadingViewModel as a parameter. */
        public GoBackCommand(LoadingViewModel view)
        {
            _view = view;
        }

        /// <summary>
        /// It sets the current view model to the BeginViewModel, which is the first view model in the application
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        public override void Execute(object parameter)
        {
            _view.NavigationStore.CurrentViewModel = new BeginViewModel(_view.NavigationStore);
            _view.NavigationStore.Client.Stop(true);
        }
    }
}
