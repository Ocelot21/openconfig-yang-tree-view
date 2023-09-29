using openconfig_yang_tree_view.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.Services
{
    public interface ISettingsService
    {
        void Save(string ip, string port, string username, string password, bool isHttps, bool subfoldersEnabled);
    }
    public class SettingsService : ObservableObject, ISettingsService
    {
        public void Save(string ip, string port, string username, string password, bool isHttps, bool subfoldersEnabled) 
        {
            Properties.Settings.Default.Ip = ip;
            Properties.Settings.Default.Port = port;
            Properties.Settings.Default.Username = username;
            Properties.Settings.Default.Password = password;
            Properties.Settings.Default.IsHttps = isHttps;
            Properties.Settings.Default.SubfoldersEnabled = subfoldersEnabled;
            Properties.Settings.Default.Save();
        }
    }
}
