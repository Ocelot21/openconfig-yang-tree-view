using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class YangList
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public List<Container> Containers { get; set; }
        public List<Grouping> Groupings { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<LeafList> LeafLists { get; set; }
        public List<YangList> Lists { get; set; }
        public List<Use> Uses { get; set; }

        public YangList()
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
