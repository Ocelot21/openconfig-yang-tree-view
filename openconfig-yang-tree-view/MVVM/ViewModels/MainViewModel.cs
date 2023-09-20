using openconfig_yang_tree_view.Core;
using openconfig_yang_tree_view.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToFilesCommand { get; set; }
        public RelayCommand NavigateToYangTreeViewCommand { get; set; }
        public RelayCommand NavigateToSettingsCommand { get; set; }

        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToFilesCommand = new RelayCommand(obj => { Navigation.NavigateTo<FilesViewModel>(); }, obj => true);
            NavigateToYangTreeViewCommand = new RelayCommand(obj => { Navigation.NavigateTo<TreeViewModel>(); }, obj => true);
            NavigateToSettingsCommand = new RelayCommand(obj => { Navigation.NavigateTo<FilesViewModel>(); }, obj => true);
        }
    }
}
