using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class Tree
    {
        public List<Grouping> Groupings { get; set; }

        public Tree()
        {
            Groupings = new List<Grouping>();
        }
    }
}
