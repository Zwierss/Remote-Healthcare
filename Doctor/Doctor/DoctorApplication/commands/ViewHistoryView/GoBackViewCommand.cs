using DoctorApplication.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApplication.commands.ViewHistoryView
{
    public class GoBackViewCommand : CommandBase
    {

        private ViewHistoryViewModel _view;

        /* A constructor. It is called when the object is created. */
        public GoBackViewCommand(ViewHistoryViewModel view)
        {
            _view = view;
        }

        /// <summary>
        /// The function is called when the user clicks the "Back" button. It sets the current view model to the
        /// SelectionViewModel
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        public override void Execute(object parameter)
        {
            _view.NavigationStore.CurrentViewModel = new SelectionViewModel(_view.NavigationStore);
        }
    }
}
