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

        public GoBackViewCommand(ViewHistoryViewModel view)
        {
            _view = view;
        }

        public override void Execute(object parameter)
        {
            _view.NavigationStore.CurrentViewModel = new SelectionViewModel(_view.NavigationStore);
        }
    }
}
