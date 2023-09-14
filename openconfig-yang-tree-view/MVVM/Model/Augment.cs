using System.Collections.Generic;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Augment
    {
        public string Name { get; set; }
        public List<string> Path { get; set; }
        public string Description { get; set; }
        public List<Container> Containers { get; set; }
        public List<Use> Uses { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<LeafList> LeafLists { get; set; }

        public Augment()
        {
            Uses = new List<Use>();
            Leafs = new List<Leaf>();
            Containers = new List<Container>();
            LeafLists = new List<LeafList>();
        }
    }
}
