using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Use
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public bool IsImplemented { get; set; }

        public Use() 
        {
            IsImplemented = false;
        }
    }
}
