using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Module
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public string YangVersion { get; set; }
        public string Namespace { get; set; }
        public string Description { get; set; }
        //public string Organization { get; set; }
        //public string Contact { get; set; }
        public List<Container> Containers { get; set; }
        public List<Grouping> Groupings { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<LeafList> LeafLists { get; set; }
        public List<YangList> Lists { get; set; }
        public List<Use> Uses { get; set; }

        public Module()
        {
            Containers = new List<Container>();
            Groupings = new List<Grouping>();
            Leafs = new List<Leaf>();
            LeafLists = new List<LeafList>();
            Lists = new List<YangList>();
            Uses = new List<Use>();
        }

    }
}
