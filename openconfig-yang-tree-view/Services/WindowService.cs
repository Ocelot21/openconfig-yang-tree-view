using openconfig_yang_tree_view.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace openconfig_yang_tree_view.Services
{
    public interface IWindowService
    {
        void Minimize(Window window);
        void Maximize(Window window);
        void Close(Window window);
    }

    public class WindowService : ObservableObject, IWindowService
    {
        public void Minimize(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
        public void Maximize(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }

        public void Close(Window window)
        {
            window.Close();
        }
    }
}
