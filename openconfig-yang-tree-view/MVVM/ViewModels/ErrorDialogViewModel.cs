using openconfig_yang_tree_view.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class ErrorDialogViewModel : BaseViewModel
    { 
        private string _exceptionMessage;

        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
            set
            {
                _exceptionMessage = value;
                OnPropertyChanged(nameof(ExceptionMessage));
            }
        }

        public ErrorDialogViewModel(Exception exception)
        {
            ExceptionMessage = exception.Message;
        }
    }

}
