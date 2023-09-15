using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class GroupingViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<LeafViewModel> Leafs { get; } = new ObservableCollection<LeafViewModel>();
        public ObservableCollection<ContainerViewModel> Containers { get; } = new ObservableCollection<ContainerViewModel>();
    }
}