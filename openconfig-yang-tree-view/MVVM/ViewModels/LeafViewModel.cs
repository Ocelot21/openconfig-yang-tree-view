using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class LeafViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Config { get; set; }
        public string Type { get; set; }
    }
}
