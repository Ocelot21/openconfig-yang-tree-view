using System.Collections.Generic;
using openconfig_yang_tree_view.MVVM.Model;
using openconfig_yang_tree_view.MVVM.ViewModels;

namespace openconfig_yang_tree_view.DataAccess
{
    public class InMemoryDb
    {
        public List<Module> Modules { get; set; }

        public InMemoryDb()
        {
            Modules = new List<Module>();
        }
    }
}
