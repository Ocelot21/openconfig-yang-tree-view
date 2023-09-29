using System.Collections.Generic;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Grouping
    {
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<Use> Uses { get; set; }
        public List<Container> Containers { get; set; }
        public List<Grouping> Groupings { get; set; }
        public List<YangList> Lists { get; set; }
        public Grouping()
        {
            Leafs = new List<Leaf>();
            Uses = new List<Use>();
            Containers = new List<Container>();
            Groupings = new List<Grouping>();
            Lists = new List<YangList>();
        }
    }
}
