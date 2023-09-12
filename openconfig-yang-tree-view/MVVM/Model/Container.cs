using System.Collections.Generic;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Container
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Config { get; set; }
        public List<Use> Uses { get; set; }
        public List<YangList> Lists { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<LeafList> LeafLists { get; set; }
        public List<Container> Containers { get; set; }

        public Container()
        {
            Uses = new List<Use>();
            Lists = new List<YangList>();
            Leafs = new List<Leaf>();
            LeafLists = new List<LeafList>();
            Containers = new List<Container>();
        }
    }
}
