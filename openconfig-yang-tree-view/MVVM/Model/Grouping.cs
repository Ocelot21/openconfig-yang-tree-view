using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Grouping
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public string Description { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<Use> Uses { get; set; }
        public List<Container> Containers { get; set; }
        public Grouping()
        {
            Leafs = new List<Leaf>();
            Uses = new List<Use>();
            Containers = new List<Container>();
        }
    }
}
