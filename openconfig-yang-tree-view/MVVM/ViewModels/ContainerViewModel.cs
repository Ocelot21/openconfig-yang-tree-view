using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class ContainerViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Config { get; set; }
        public ObservableCollection<ContainerViewModel> Containers { get; } = new ObservableCollection<ContainerViewModel>();
        public ObservableCollection<ListViewModel> Lists { get; } = new ObservableCollection<ListViewModel>();
    }
}
