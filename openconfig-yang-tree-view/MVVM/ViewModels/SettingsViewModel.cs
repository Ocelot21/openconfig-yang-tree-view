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
            Ip = string.Empty;
            Port = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            IsHttps = false;
            SaveCommand = new RelayCommand(Save, obj => true);
        }

        private void Save(object obj)
        {
            SettingsService.Save(Ip, Port, Username, Password, IsHttps);
        }
    }
}
