using openconfig_yang_tree_view.MVVM.ViewModels;
using openconfig_yang_tree_view.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.Services
{
    public class ExceptionHandlingService
    {
        public ExceptionHandlingService()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleGlobalException;
        }

        private void HandleGlobalException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            var errorDialogViewModel = new ErrorDialogViewModel(exception);
            var errorDialog = new ErrorDialogWindow();
            errorDialog.DataContext = errorDialogViewModel;
            errorDialog.ShowDialog();

        }
    }

}
