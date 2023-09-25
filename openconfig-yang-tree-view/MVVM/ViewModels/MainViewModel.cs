using openconfig_yang_tree_view.Core;
using openconfig_yang_tree_view.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private INavigationService _navagiation;
        public INavigationService Navigation 
        {
            get => _navagiation; 
            set
            {
                _navagiation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }

        private IWindowService _windowService;
        public IWindowService WindowService
        {
            get => _windowService;
            set
            {
                _windowService = value;
                OnPropertyChanged(nameof(WindowService));
            }
        }

        public RelayCommand NavigateToFilesCommand { get; set; }
        public RelayCommand NavigateToYangTreeViewCommand { get; set; }
        public RelayCommand NavigateToSettingsCommand { get; set; }
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand MaximizeCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public MainViewModel(INavigationService navigation, IWindowService windowService)
        {
            Navigation = navigation;
            WindowService = windowService;
            NavigateToFilesCommand = new RelayCommand(obj => { Navigation.NavigateTo<FilesViewModel>(); }, obj => true);
            NavigateToYangTreeViewCommand = new RelayCommand(obj => { Navigation.NavigateTo<TreeViewModel>(); }, obj => true);
            NavigateToSettingsCommand = new RelayCommand(obj => { Navigation.NavigateTo<FilesViewModel>(); }, obj => true);
            MinimizeCommand = new RelayCommand(ExecuteMinimizeCommand, obj => true);
            MaximizeCommand = new RelayCommand(ExecuteMaximizeCommand, obj => true);
            CloseCommand = new RelayCommand(ExecuteCloseCommand, obj => true);

        }

        private void ExecuteMinimizeCommand(object obj)
        {
            _windowService.Minimize(Application.Current.MainWindow);
        }

        private void ExecuteMaximizeCommand(object obj)
        {
            _windowService.Maximize(Application.Current.MainWindow);
        }

        private void ExecuteCloseCommand(object obj)
        {
            _windowService.Close(Application.Current.MainWindow);
        }
    }
}
    