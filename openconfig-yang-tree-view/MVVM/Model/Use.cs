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
        public bool IsExternal { get; set; }
        public string? ExternalPrefix { get; set; }

        public bool IsImplemented { get; set; }

        public Use() 
        {
            IsExternal = false;
            IsImplemented = false;
        }
    }
}
