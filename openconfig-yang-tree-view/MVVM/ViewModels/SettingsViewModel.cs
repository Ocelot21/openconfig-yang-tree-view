using openconfig_yang_tree_view.Core;
using openconfig_yang_tree_view.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private string _ip;
        public string Ip { 
            get => _ip;
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(Ip));
            } 
        }

        private string _port;
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private string _username;
        public string Username 
        { 
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _isHttps;
        public bool IsHttps 
        { 
            get => _isHttps; 
            set
            {
                if (_isHttps != value)
                {
                    _isHttps = value;
                    OnPropertyChanged(nameof(IsHttps));
                }
            }
        }

        private bool _subfoldersEnabled;
        public bool SubfoldersEnabled
        {
            get => _subfoldersEnabled;
            set
            {
                if (_subfoldersEnabled != value)
                {
                    _subfoldersEnabled = value;
                    OnPropertyChanged(nameof(SubfoldersEnabled));
                }
            }
        }

        private ISettingsService _settingsService;
        public ISettingsService SettingsService
        {
            get => _settingsService;
            set
            {
                _settingsService = value;
                OnPropertyChanged(nameof(SettingsService));
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        public SettingsViewModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;
            Ip = Properties.Settings.Default.Ip;
            Port = Properties.Settings.Default.Port;
            Username = Properties.Settings.Default.Username;
            Password = Properties.Settings.Default.Password;
            IsHttps = Properties.Settings.Default.IsHttps;
            SubfoldersEnabled = Properties.Settings.Default.SubfoldersEnabled;
            SaveCommand = new RelayCommand(Save, obj => true);
        }

        private void Save(object obj)
        {
            SettingsService.Save(Ip, Port, Username, Password, IsHttps, SubfoldersEnabled);
        }
    }
}
