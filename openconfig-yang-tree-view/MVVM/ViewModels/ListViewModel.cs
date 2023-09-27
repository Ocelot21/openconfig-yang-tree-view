using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class ListViewModel : TreeNodeViewModel
    {
        public string Key { get; set; }
        public string Path { get; set; }
    }
}