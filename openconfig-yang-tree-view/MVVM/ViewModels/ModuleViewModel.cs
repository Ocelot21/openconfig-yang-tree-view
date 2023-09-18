using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class ModuleViewModel : TreeNodeViewModel
    {
        public string Prefix { get; set; }
        public string YangVersion { get; set; }
        public string Namespace { get; set; }
    }
}
