using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using openconfig_yang_tree_view.MVVM.Model;

namespace openconfig_yang_tree_view.InMemoryDB
{
    public class InMemoryDB
    {
        public List<Module> Modules { get; set; }

        public InMemoryDB()
        {
            Modules = new List<Module>();
        }
    }
}
